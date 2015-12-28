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
        int PlatformDevices;
        string[] Devices;
        const string DefaultParam = "--keccak-unroll 0 --hamsi-expand-big 4 " +
                                    "--gpu-fan 30-95 --temp-cutoff 95 --temp-overheat 90 " +
                                    "--temp-target 75 --auto-fan --auto-gpu ";

        public sgminer()
        {
            SupportedAlgorithms = new Algorithm[] { 
                new Algorithm( 3, "x11",        "x11",        DefaultParam + "--nfactor 10 --xintensity  640 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm( 4, "x13",        "x13",        DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize  64 --gpu-threads 2"),
                new Algorithm( 5, "keccak",     "keccak",     DefaultParam + "--nfactor 10 --xintensity  300 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm( 7, "nist5",      "nist5",      DefaultParam + "--nfactor 10 --xintensity   16 --thread-concurrency    0 --worksize  64 --gpu-threads 2"),
                new Algorithm( 8, "neoscrypt",  "neoscrypt",  DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 4"),
                new Algorithm(10, "whirlpoolx", "whirlpoolx", DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize 128 --gpu-threads 2"),
                new Algorithm(11, "qubit",      "qubitcoin",  DefaultParam + "--intensity 18 --worksize 64 --gpu-threads 2"),
                new Algorithm(12, "quark",      "quarkcoin",  DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency    0 --worksize  64 --gpu-threads 1"),
                new Algorithm(14, "lyra2rev2",  "Lyra2REv2",  DefaultParam + "--nfactor 10 --xintensity  160 --thread-concurrency    0 --worksize  64 --gpu-threads 1")
            };

            MinerDeviceName = "AMD_OpenCL";
            Path = "bin\\sgminer-5-2-1-general\\sgminer.exe";
            APIPort = 4050;
            EnableOptimizedVersion = true;
            PlatformDevices = 0;

            QueryCDevs();
        }

        protected void AddPotentialCDev(string text)
        {
            // get number of platform devices to be used to detect card's code name
            if (text.Contains("Platform devices:"))
            {
                PlatformDevices = (int)Char.GetNumericValue(text.Split(' ')[3][0]);
                Devices = new string[PlatformDevices];
                Helpers.ConsolePrint(MinerDeviceName, "Platform Devices: " + PlatformDevices);
                return;
            }

            // check the card's code name
            if (PlatformDevices > 0 && EnableOptimizedVersion)
            {
                Devices[PlatformDevices-1] = text.Substring(14);
                PlatformDevices--;
                if (!(text.Contains("Tahiti") || text.Contains("Hawaii") || text.Contains("Pitcairn")))
                {
                    EnableOptimizedVersion = false;
                    Helpers.ConsolePrint(MinerDeviceName, "One of the GPUs detected is not Tahiti, Hawaii or Pitcaird. Optimized version is disabled!");
                    return;
                }
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
            Process P = new Process();
            P.StartInfo.FileName = Path;
            P.StartInfo.Arguments = "--ndevs";
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.CreateNoWindow = true;
            P.Start();

            string outdata;

            do
            {
                outdata = P.StandardError.ReadLine();
                if (outdata != null) AddPotentialCDev(outdata);
            } while (outdata != null);

            P.WaitForExit();

            // check the driver version
            bool ShowWarningDialog = false;
            ManagementObjectCollection moc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get();

            foreach (var manObj in moc)
            {
                Helpers.ConsolePrint(MinerDeviceName, "GPU Name (Driver Ver): " + manObj["Name"] + " (" + manObj["DriverVersion"] + ")");

                if (manObj["Name"].ToString().Contains("AMD") && ShowWarningDialog == false)
                {
                    Version AMDDriverVersion = new Version(manObj["DriverVersion"].ToString());

                    if (AMDDriverVersion.Major < 15)
                    {
                        ShowWarningDialog = true;
                        EnableOptimizedVersion = false;
                        Helpers.ConsolePrint(MinerDeviceName, "WARNING!!! Old AMD GPU driver detected! All optimized versions disabled, mining " +
                            "speed will not be optimal. Consider upgrading AMD GPU driver. Recommended AMD GPU driver version is 15.7.1.");
                    }
                }
            }

            if (EnableOptimizedVersion == false)
            {
                SupportedAlgorithms[0].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency    0 --worksize  64 --gpu-threads 2";  // x11
                SupportedAlgorithms[6].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency    0 --worksize 128 --gpu-threads 4";  // qubit
                SupportedAlgorithms[7].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency    0 --worksize 256 --gpu-threads 1";  // quark
                SupportedAlgorithms[8].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 32 --thread-concurrency 8192 --worksize  32 --gpu-threads 4";  // lyra2rev2
            }

            for (int i = 0; i < Devices.Length; i++)
            {
                if (Devices[i] != null && !Devices[i].Contains("Tahiti"))
                {
                    Helpers.ConsolePrint(MinerDeviceName, "One of the GPUs detected is not Tahiti (" + Devices[i] + "). Changing default gpu-threads to 2");
                    SupportedAlgorithms[4].ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 2";  // neoscrypt
                    break;
                }
            }
            if (ShowWarningDialog == true && Config.ConfigData.ShowDriverVersionWarning == true)
            {
                Form WarningDialog = new DriverVersionConfirmationDialog();
                WarningDialog.ShowDialog();
                WarningDialog = null;
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

            Path = "cmd";
            string DirName = GetMinerDirectory(Algo.NiceHashName);

            string url = Form1.NiceHashData[SupportedAlgorithms[index].NiceHashID].name + "." +
                         Form1.MiningLocation[Config.ConfigData.Location] + ".nicehash.com:" +
                         Form1.NiceHashData[SupportedAlgorithms[index].NiceHashID].port;

            string username = Config.ConfigData.BitcoinAddress;
            if (Config.ConfigData.WorkerName.Length > 0)
                username += "." + Config.ConfigData.WorkerName;

            string CommandLine = " /C \"cd /d " + DirName + " && sgminer.exe " +
                                 "-k " + SupportedAlgorithms[index].MinerName +
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

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled)
                    CommandLine += G.ID.ToString() + ",";

            CommandLine = CommandLine.Remove(CommandLine.Length - 1) + " && del dump.txt\"";

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

            Path = "sgminer.exe";
            WorkingDirectory = GetMinerDirectory(Algo.NiceHashName);

            LastCommandLine = "-k " + Algo.MinerName +
                              " --url=" + url +
                              " --userpass=" + username + ":" + GetPassword(Algo) +
                              " --api-listen" +
                              " --api-port=" + APIPort.ToString() +
                              " " + ExtraLaunchParameters +
                              " " + Algo.ExtraLaunchParameters +
                              " --device ";

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled) LastCommandLine += G.ID.ToString() + ",";

            if (LastCommandLine.EndsWith(","))
                LastCommandLine = LastCommandLine.Remove(LastCommandLine.Length - 1);
            else
            {
                LastCommandLine = "";
                return; // no GPUs to start mining on
            }

            if (Config.ConfigData.HideMiningWindows)
            {
                Path = "cmd";
                LastCommandLine = " /C \"cd /d " + WorkingDirectory +
                                  " && sgminer.exe " + LastCommandLine + "\"";
            }

            ProcessHandle = _Start();
        }

        private string GetMinerDirectory(string algo)
        {
            string dir = new DirectoryInfo(".").FullName + "\\bin\\";

            if (EnableOptimizedVersion && (algo.Equals("x11") || algo.Equals("quark") || algo.Equals("lyra2rev2")))
            {
                dir += "sgminer-5-1-0-optimized";
            }
            else if (EnableOptimizedVersion && algo.Equals("qubit"))
            {
                dir += "sgminer-5-1-1-optimized";
            }
            else
            {
                dir += "sgminer-5-2-1-general";
            }

            return dir;
        }
    }
}
