using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Management;

namespace NiceHashMiner
{
    class sgminer : Miner
    {
        bool EnableOptimizedVersion;
        int PlatformDevices, tmpPlatformDevices, GPUPlatformNumber;
        List<string> GPUCodeName;
        const string DefaultParam = "--keccak-unroll 0 --hamsi-expand-big 4 ";
        const string TemperatureParam = " --gpu-fan 30-95 --temp-cutoff 95 --temp-overheat 90" +
                                        " --temp-target 75 --auto-fan --auto-gpu";

        public sgminer()
        {
            SupportedAlgorithms = new Algorithm[] { 
                new Algorithm( 3, "x11",        "x11",        DefaultParam + "--nfactor 10 --xintensity  640 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm( 4, "x13",        "x13",        DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize  64 --gpu-threads 2"),
                new Algorithm( 5, "keccak",     "keccak",     DefaultParam + "--nfactor 10 --xintensity  300 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm( 6, "x15",        "x15",        DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize  64 --gpu-threads 2"),
                new Algorithm( 7, "nist5",      "nist5",      DefaultParam + "--nfactor 10 --xintensity   16 --thread-concurrency    0 --worksize  64 --gpu-threads 2"),
                new Algorithm( 8, "neoscrypt",  "neoscrypt",  DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 4"),
                new Algorithm(10, "whirlpoolx", "whirlpoolx", DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize 128 --gpu-threads 2"),
                new Algorithm(11, "qubit",      "qubitcoin",  DefaultParam + "--intensity 18 --worksize 64 --gpu-threads 2"),
                new Algorithm(12, "quark",      "quarkcoin",  DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm(14, "lyra2rev2",  "Lyra2REv2",  DefaultParam + "--nfactor 10 --xintensity  160 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm(16, "blake256r8", "blakecoin",  DefaultParam + "--intensity  24 --worksize 128 --gpu-threads 2"),
                new Algorithm(17, "blake256r14",   "blake",   DefaultParam + "--intensity  24 --worksize 128 --gpu-threads 2"),
                new Algorithm(18, "blake256r8vnl", "vanilla", DefaultParam + "--intensity  24 --worksize 128 --gpu-threads 2"),
                new Algorithm(20, "daggerhashimoto", "daggerhashimoto"),
                new Algorithm(21, "decred", "decred", "--gpu-threads 1 --remove-disabled --xintensity 256 --lookup-gap 2 --worksize 64")
            };

            MinerDeviceName = "AMD_OpenCL";
            Path = "bin\\sgminer-5-4-0-general\\sgminer.exe";
            APIPort = 4050;
            EnableOptimizedVersion = true;
            PlatformDevices = 0;
            GPUPlatformNumber = 0;
            GPUCodeName = new List<string>();

            if (!Config.ConfigData.DisableDetectionAMD)
                QueryCDevs();
        }

        protected void AddPotentialCDev(string text)
        {
            // get number of platform devices to be used to detect card's code name
            if (text.Contains("Platform devices:"))
            {
                try {
                    PlatformDevices = Int32.Parse(text.Split(' ')[3]);
                    tmpPlatformDevices = Int32.Parse(text.Split(' ')[3]);
                    Helpers.ConsolePrint(MinerDeviceName, "Platform Devices: " + PlatformDevices);
                }
                catch { PlatformDevices = 0; }

                return;
            }

            // gets all the card's code name
            if (PlatformDevices > 0)
            {
                PlatformDevices--;
                GPUCodeName.Add(text.Substring(14).Trim());
            }

            // skip useless lines
            if (!text.Contains("GPU") || !text.Contains("assigned")) return;

            string[] splt = text.Split(':');

            int id = (int)Char.GetNumericValue(splt[2][8]);
            string name = splt[splt.Length - 1];

            Helpers.ConsolePrint(MinerDeviceName, "Detected: " + name);

            // add AMD OpenCL devices
            CDevs.Add(new ComputeDevice(id, MinerDeviceName, name));
            Helpers.ConsolePrint(MinerDeviceName, "Added: " + name);
        }

        protected void QueryCDevs()
        {
            try
            {
                Process P = new Process();
                P.StartInfo.FileName = Path;
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.CreateNoWindow = true;

                string outdata;

                for (int i = 0; i < 4; i++)
                {
                    P.StartInfo.Arguments = "--gpu-platform " + i + " --ndevs";
                    P.Start();
                    
                    do
                    {
                        outdata = P.StandardError.ReadLine();
                        if (outdata != null)
                        {
                            if (outdata.Contains("CL Platform name") && outdata.Contains("AMD"))
                            {
                                GPUPlatformNumber = i;
                                i = 20;
                            }
                            else if (outdata.Contains("Specified platform that does not exist"))
                            {
                                GPUPlatformNumber = -1;
                                i = 20;
                            }
                        }
                    } while (outdata != null);

                    P.WaitForExit();
                }

                if (GPUPlatformNumber == -1)
                {
                    Helpers.ConsolePrint(MinerDeviceName, "No AMD GPUs found.");
                    return;
                }

                P.StartInfo.Arguments = "--gpu-platform " + GPUPlatformNumber + " --ndevs";
                P.Start();

                do
                {
                    outdata = P.StandardError.ReadLine();
                    if (outdata != null) AddPotentialCDev(outdata);
                } while (outdata != null);

                P.WaitForExit();

                PlatformDevices = tmpPlatformDevices;
            }
            catch (Exception e)
            {
                Helpers.ConsolePrint(MinerDeviceName, "Exception: " + e.ToString());
                
                MinerFileNotFoundDialog WarningDialog = new MinerFileNotFoundDialog(MinerDeviceName, Path);
                WarningDialog.ShowDialog();
                
                if (WarningDialog.DisableDetection)
                    Config.ConfigData.DisableDetectionAMD = true;
                
                WarningDialog = null;

                return;
            }

            // Check for optimized version
            for (int i = 0; i < GPUCodeName.Count; i++)
            {
                Helpers.ConsolePrint(MinerDeviceName, "List: " + GPUCodeName[i]);
                if (!(GPUCodeName[i].Equals("Bonaire")  || GPUCodeName[i].Equals("Fiji")   || GPUCodeName[i].Equals("Hawaii") ||
                      GPUCodeName[i].Equals("Pitcairn") || GPUCodeName[i].Equals("Tahiti") || GPUCodeName[i].Equals("Tonga")))
                {
                    Helpers.ConsolePrint(MinerDeviceName, "GPU (" + GPUCodeName[i] + ") is not optimized. Switching to general sgminer.");
                    EnableOptimizedVersion = false;
                }
                else
                {
                    SupportedAlgorithms[GetAlgoIndex("x11")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                    SupportedAlgorithms[GetAlgoIndex("qubit")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                    SupportedAlgorithms[GetAlgoIndex("quark")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                    SupportedAlgorithms[GetAlgoIndex("lyra2rev2")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 512  --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                }

                if (!GPUCodeName[i].Equals("Tahiti"))
                {
                    SupportedAlgorithms[GetAlgoIndex("neoscrypt")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 2";
                    Helpers.ConsolePrint(MinerDeviceName, "The GPU detected (" + GPUCodeName[i] + ") is not Tahiti. Changing default gpu-threads to 2.");
                }
            }

            // check the driver version
            bool ShowWarningDialog = false;
            ManagementObjectCollection moc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get();

            foreach (var manObj in moc)
            {
                Helpers.ConsolePrint(MinerDeviceName, "GPU Name (Driver Ver): " + manObj["Name"] + " (" + manObj["DriverVersion"] + ")");

                if (manObj["Name"].ToString().Contains("AMD") && ShowWarningDialog == false)
                {
                    if (PlatformDevices > 0 && CDevs.Count < PlatformDevices)
                    {
                        Helpers.ConsolePrint(MinerDeviceName, "Adding missed GPUs: " + manObj["name"].ToString());
                        CDevs.Add(new ComputeDevice(CDevs.Count, MinerDeviceName, manObj["Name"].ToString()));
                    }

                    Version AMDDriverVersion = new Version(manObj["DriverVersion"].ToString());

                    if (AMDDriverVersion.Major < 15)
                    {
                        ShowWarningDialog = true;
                        EnableOptimizedVersion = false;
                        Helpers.ConsolePrint(MinerDeviceName, "WARNING!!! Old AMD GPU driver detected! All optimized versions disabled, mining " +
                            "speed will not be optimal. Consider upgrading AMD GPU driver. Recommended AMD GPU driver version is 15.7.1.");
                    }
                    else if (AMDDriverVersion.Major == 16 && AMDDriverVersion.Minor >= 150)
                    {
                        string src = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" +
                                     Path.Split('\\')[0] + "\\" + Path.Split('\\')[1] + "\\kernel";

                        foreach (var file in Directory.GetFiles(src))
                        {
                            string dest = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\" + System.IO.Path.GetFileName(file);
                            if (!File.Exists(dest)) File.Copy(file, dest, false);
                        }
                    }
                }
            }

            if (EnableOptimizedVersion == false)
            {
                SupportedAlgorithms[GetAlgoIndex("x11")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 64 --gpu-threads 2";
                SupportedAlgorithms[GetAlgoIndex("qubit")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 128 --gpu-threads 4";
                SupportedAlgorithms[GetAlgoIndex("quark")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 256 --gpu-threads 1";
                SupportedAlgorithms[GetAlgoIndex("lyra2rev2")].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 64 --gpu-threads 2";
            }

            if (ShowWarningDialog == true && Config.ConfigData.ShowDriverVersionWarning == true)
            {
                Form WarningDialog = new DriverVersionConfirmationDialog();
                WarningDialog.ShowDialog();
                WarningDialog = null;
            }

            // Check for ethereum mining
            EtherDevices = new int[CDevs.Count];

            try
            {
                Process P = new Process();
                P.StartInfo.FileName = Ethereum.EtherMinerPath;
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.CreateNoWindow = true;

                string outdata;

                Helpers.ConsolePrint(MinerDeviceName, "Adding Ethereum..");
                P.StartInfo.Arguments = "--list-devices --opencl";
                P.Start();

                int i = 0;
                do
                {
                    outdata = P.StandardError.ReadLine();
                    if (outdata != null)
                    {
                        if (outdata.Contains("Intel") || outdata.Contains("GeForce"))
                        {
                            // skips to the next GPU
                            P.StandardError.ReadLine();
                            P.StandardError.ReadLine();
                            P.StandardError.ReadLine();
                            P.StandardError.ReadLine();
                            continue;
                        }

                        if (outdata.Contains("CL_DEVICE_GLOBAL_MEM_SIZE"))
                        {
                            long memsize = Convert.ToInt64(outdata.Split(':')[1]);
                            if (memsize >= 2147483648)
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (GOOD!)");
                                EtherDevices[i] = i;
                                i++;
                            }
                            else
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (NOT GOOD!)");
                                EtherDevices[i] = -1;
                                i++;
                            }
                        }
                    }
                } while (outdata != null);

                P.WaitForExit();
            }
            catch (Exception e)
            {
                Helpers.ConsolePrint(MinerDeviceName, "Exception: " + e.ToString());
            }
        }

        protected override string BenchmarkCreateCommandLine(int index, int time)
        {
            Algorithm Algo = GetMinerAlgorithm(SupportedAlgorithms[index].NiceHashID);
            if (Algo == null)
            {
                Helpers.ConsolePrint(MinerDeviceName, "GetMinerAlgorithm(" + index + "): Algo equals to null");
                return "";
            }

            string CommandLine;
            if (Algo.NiceHashName.Equals("daggerhashimoto"))
            {
                CommandLine = " --opencl --opencl-platform " + GPUPlatformNumber +
                              " " + ExtraLaunchParameters +
                              " " + Algo.ExtraLaunchParameters +
                              " --benchmark-warmup 40 --benchmark-trial 20" +
                              " --opencl-devices ";

                int dagdev = -1;
                for (int i = 0; i < CDevs.Count; i++)
                {
                    if (EtherDevices[i] != -1 && CDevs[i].Enabled && !Algo.DisabledDevice[i])
                    {
                        CommandLine += i + " ";
                        if (dagdev == -1) dagdev = i;
                    }
                }

                CommandLine += " --dag-load-mode single " + dagdev.ToString();

                Ethereum.GetCurrentBlock(MinerDeviceName);
                CommandLine += " --benchmark " + Ethereum.CurrentBlockNum;
            }
            else
            {
                Path = "cmd";
                string DirName = GetMinerDirectory(Algo.NiceHashName);

                string url = "stratum+tcp://" + Form1.NiceHashData[SupportedAlgorithms[index].NiceHashID].name + "." +
                             Form1.MiningLocation[Config.ConfigData.Location] + ".nicehash.com:" +
                             Form1.NiceHashData[SupportedAlgorithms[index].NiceHashID].port;

                string username = Config.ConfigData.BitcoinAddress.Trim();
                if (Config.ConfigData.WorkerName.Length > 0)
                    username += "." + Config.ConfigData.WorkerName.Trim();

                CommandLine = " /C \"cd /d " + DirName + " && sgminer.exe " +
                              " --gpu-platform " + GPUPlatformNumber +
                              " -k " + SupportedAlgorithms[index].MinerName +
                              " --url=" + url +
                              " --userpass=" + username + ":" + GetPassword(Algo) +
                              " --sched-stop " + DateTime.Now.AddMinutes(time).ToString("HH:mm") +
                              " -T --log 10 --log-file dump.txt" +
                              " --api-listen" +
                              " --api-port=" + APIPort.ToString() +
                              " --api-allow W:127.0.0.1" +
                              " " + ExtraLaunchParameters +
                              " " + SupportedAlgorithms[index].ExtraLaunchParameters +
                              " --device ";

                for (int i = 0; i < CDevs.Count; i++)
                    if (CDevs[i].Enabled && !Algo.DisabledDevice[i])
                        CommandLine += CDevs[i].ID.ToString() + ",";

                CommandLine = CommandLine.Remove(CommandLine.Length - 1);
                if (Config.ConfigData.DisableAMDTempControl == false)
                    CommandLine += TemperatureParam;
                CommandLine += " && del dump.txt\"";
            }

            return CommandLine;
        }


        public override void Start(int nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running 

            Algorithm Algo = GetMinerAlgorithm(nhalgo);
            if (Algo == null)
            {
                Helpers.ConsolePrint(MinerDeviceName, "GetMinerAlgorithm(" + nhalgo + "): Algo equals to null");
                return;
            }

            if (Algo.NiceHashName.Equals("daggerhashimoto"))
            {
                WorkingDirectory = "";
                LastCommandLine = " --opencl --opencl-platform " + GPUPlatformNumber +
                                  " " + ExtraLaunchParameters +
                                  " " + Algo.ExtraLaunchParameters +
                                  " -S " + url.Substring(14) +
                                  " -O " + username + ":" + GetPassword(Algo) +
                                  " --api-port " + Config.ConfigData.ethminerAPIPortAMD.ToString() +
                                  " --opencl-devices ";

                int dagdev = -1;
                for (int i = 0; i < CDevs.Count; i++)
                {
                    if (EtherDevices[i] != -1 && CDevs[i].Enabled && !Algo.DisabledDevice[i])
                    {
                        LastCommandLine += i.ToString() + " ";
                        if (dagdev == -1) dagdev = i;
                    }
                }

                LastCommandLine += " --dag-load-mode singlekeep " + dagdev.ToString();
            }
            else
            {
                StartingUpDelay = true;

                WorkingDirectory = GetMinerDirectory(Algo.NiceHashName);
                Path = WorkingDirectory + "sgminer.exe";

                LastCommandLine = " --gpu-platform " + GPUPlatformNumber +
                                  " -k " + Algo.MinerName +
                                  " --url=" + url +
                                  " --userpass=" + username + ":" + GetPassword(Algo) +
                                  " --api-listen" +
                                  " --api-port=" + APIPort.ToString() +
                                  " " + ExtraLaunchParameters +
                                  " " + Algo.ExtraLaunchParameters +
                                  " --device ";

                for (int i = 0; i < CDevs.Count; i++)
                    if (CDevs[i].Enabled && !Algo.DisabledDevice[i])
                        LastCommandLine += CDevs[i].ID.ToString() + ",";

                if (LastCommandLine.EndsWith(","))
                    LastCommandLine = LastCommandLine.Remove(LastCommandLine.Length - 1);
                else
                {
                    LastCommandLine = "";
                    return; // no GPUs to start mining on
                }

                if (Config.ConfigData.DisableAMDTempControl == false)
                    LastCommandLine += TemperatureParam;
            }

            ProcessHandle = _Start();
        }

        private string GetMinerDirectory(string algo)
        {
            string dir = "bin\\";

            if (EnableOptimizedVersion)
            {
                if (algo.Equals("x11") || algo.Equals("quark") || algo.Equals("lyra2rev2") || algo.Equals("qubit"))
                {
                    for (int i = 0; i < GPUCodeName.Count; i++)
                    {
                        if (!(GPUCodeName[i].Equals("Hawaii") || GPUCodeName[i].Equals("Pitcairn") || GPUCodeName[i].Equals("Tahiti")))
                        {
                            if (!Helpers.InternalCheckIsWow64())
                                return dir + "sgminer-5-4-0-general\\";

                            return dir + "sgminer-5-4-0-tweaked\\";
                        }
                    }

                    if (algo.Equals("x11") || algo.Equals("quark") || algo.Equals("lyra2rev2"))
                        return dir + "sgminer-5-1-0-optimized\\";
                    else
                        return dir + "sgminer-5-1-1-optimized\\";
                }
            }

            return dir + "sgminer-5-4-0-general\\";
        }
    }
}
