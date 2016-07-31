using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    public class Algorithm
    {
        public int NiceHashID;
        public string NiceHashName;
        public string MinerName;
        public double BenchmarkSpeed;
        public double CurrentProfit;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public bool Skip;
        public bool[] DisabledDevice;

        public Algorithm(int id, string nhname, string mname)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            MinerName = mname;
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = "";
            UsePassword = null;
            Skip = false;
        }

        public Algorithm(int id, string nhname, string mname, string xtraparam)
        {
            NiceHashID = id;
            NiceHashName = nhname;
            MinerName = mname;
            BenchmarkSpeed = 0;
            ExtraLaunchParameters = xtraparam;
            UsePassword = null;
            Skip = false;
        }
    }


    public class APIData
    {
        public int AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }


    public class ComputeDevice
    {
        public int ID;
        public string Vendor;
        public string Name;
        public bool Enabled;

        public ComputeDevice(int id, string v, string n)
        {
            ID = id;
            Vendor = v;
            Name = n;
            Enabled = true;
        }
    }

    public delegate void BenchmarkComplete(bool success, string text, object tag);

    public abstract class Miner
    {
        public string MinerDeviceName;
        public int APIPort;
        public List<ComputeDevice> CDevs;
        public Algorithm[] SupportedAlgorithms;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double MinimumProfit;
        public int DaggerHashimotoGenerateDevice;
        public int CurrentAlgo;
        public double CurrentRate;
        public bool NotProfitable;
        public bool IsRunning;
        public bool BenchmarkSignalQuit;
        public bool BenchmarkSignalHanged;
        public int NumRetries;
        public bool StartingUpDelay;
        public string Path;

        protected int[] EtherDevices;
        protected string WorkingDirectory;
        protected NiceHashProcess ProcessHandle;
        protected BenchmarkComplete OnBenchmarkComplete;
        protected object BenchmarkTag;
        protected int BenchmarkIndex;
        protected int BenchmarkTime;
        protected string LastCommandLine;
        protected double PreviousTotalMH;

        protected NiceHashProcess ethminerProcess;
        protected ethminerAPI ethminerLink;

        public Miner()
        {
            CDevs = new List<ComputeDevice>();

            WorkingDirectory = "";
            ExtraLaunchParameters = "";
            UsePassword = null;
            StartingUpDelay = false;

            CurrentAlgo = -1;
            CurrentRate = 0;
            NotProfitable = true;
            IsRunning = false;
            PreviousTotalMH = 0.0;
        }

        public void KillSGMiner()
        {
            foreach (Process process in Process.GetProcessesByName("sgminer"))
            {
                try { process.Kill(); }
                catch (Exception e) { Helpers.ConsolePrint(MinerDeviceName, e.ToString()); }
            }
        }

        abstract public void Start(int nhalgo, string url, string username);

        virtual public void Stop(bool willswitch)
        {
            if (willswitch && AlgoNameIs("daggerhashimoto"))
            {
                // daggerhashimoto - we only "pause" mining
                Helpers.ConsolePrint(MinerDeviceName, "Pausing ethminer..");
                ethminerLink.StopMining();
                return;
            }

            Helpers.ConsolePrint(MinerDeviceName, "Shutting down miner");

            if (ProcessHandle != null)
            {
                try { ProcessHandle.Kill(); }
                catch { }
                ProcessHandle.Close();
                ProcessHandle = null;

                if (MinerDeviceName == "AMD_OpenCL") KillSGMiner();
            }

            if (!willswitch && ethminerProcess != null)
            {
                try { ethminerProcess.Kill(); }
                catch { }
                ethminerProcess.Close();
                ethminerProcess = null;
            }

            StartingUpDelay = false;
            PreviousTotalMH = 0.0;
            NotProfitable = true;
            //IsRunning = false;
            //CurrentAlgo = -1;
        }

        abstract protected string BenchmarkCreateCommandLine(int index, int time);


        virtual public void BenchmarkStart(int index, int time, BenchmarkComplete oncomplete, object tag)
        {
            OnBenchmarkComplete = oncomplete;

            if (index >= SupportedAlgorithms.Length)
            {
                OnBenchmarkComplete(false, "Unknown algorithm", tag);
                return;
            }

            if (EnabledDeviceCount() == 0 || EnabledDevicePerAlgoCount(index) < 1)
            {
                Helpers.ConsolePrint("BENCHMARK", "No device to benchmark..");
                OnBenchmarkComplete(false, "Disabled", tag);
                return; // ignore, disabled device
            }

            BenchmarkTag = tag;
            BenchmarkIndex = index;
            CurrentAlgo = index;
            BenchmarkTime = time;

            string CommandLine = BenchmarkCreateCommandLine(index, time);

            Thread BenchmarkThread = new Thread(BenchmarkThreadRoutine);
            BenchmarkThread.Start(CommandLine);
        }


        virtual protected bool BenchmarkParseLine(string outdata)
        {
            // lyra2re nvidia fix
            double lastSpeed = 0;

            // parse line
            if (outdata.Contains("Benchmark: ") && outdata.Contains("/s"))
            {
                int i = outdata.IndexOf("Benchmark:");
                int k = outdata.IndexOf("/s");
                string hashspeed = outdata.Substring(i + 11, k - i - 9);
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + hashspeed);

                // save speed
                int b = hashspeed.IndexOf(" ");
                double spd = Double.Parse(hashspeed.Substring(0, b), CultureInfo.InvariantCulture);
                if (hashspeed.Contains("kH/s"))
                    spd *= 1000;
                else if (hashspeed.Contains("MH/s"))
                    spd *= 1000000;
                else if (hashspeed.Contains("GH/s"))
                    spd *= 1000000000;
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = spd;

                OnBenchmarkComplete(true, PrintSpeed(spd), BenchmarkTag);
                return true;
            }
            else if (outdata.Contains("Average hashrate:") && outdata.Contains("/s"))
            {
                int i = outdata.IndexOf(": ");
                int k = outdata.IndexOf("/s");

                // save speed
                string hashSpeed = outdata.Substring(i + 2, k - i + 2);
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + hashSpeed);

                hashSpeed = hashSpeed.Substring(0, hashSpeed.IndexOf(" "));
                double speed = Double.Parse(hashSpeed, CultureInfo.InvariantCulture);

                if (outdata.Contains("Kilohash"))
                    speed *= 1000;
                else if (outdata.Contains("Megahash"))
                    speed *= 1000000;
                
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = speed;

                OnBenchmarkComplete(true, PrintSpeed(speed), BenchmarkTag);
                return true;
            }
            else if (outdata.Contains("min/mean/max:"))
            {
                string[] splt = outdata.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                int index = Array.IndexOf(splt, "mean");
                double avg_spd = Convert.ToDouble(splt[index + 2]);
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + avg_spd + "H/s");

                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = avg_spd;

                OnBenchmarkComplete(true, PrintSpeed(avg_spd), BenchmarkTag);
                return true;
            }
            else if (double.TryParse(outdata, out lastSpeed)) {
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = lastSpeed;
                OnBenchmarkComplete(true, PrintSpeed(lastSpeed), BenchmarkTag);
                return true;
            }

            return false;
        }


        virtual protected string BenchmarkGetConsoleOutputLine(Process BenchmarkHandle)
        {
            return BenchmarkHandle.StandardOutput.ReadLine();
        }


        virtual protected Process BenchmarkStartProcess(string CommandLine)
        {
            Helpers.ConsolePrint(MinerDeviceName, "Starting benchmark: " + CommandLine);

            Process BenchmarkHandle = new Process();
            if (AlgoNameIs("daggerhashimoto"))
                BenchmarkHandle.StartInfo.FileName = Ethereum.EtherMinerPath;
            else
                BenchmarkHandle.StartInfo.FileName = Path;
            BenchmarkHandle.StartInfo.Arguments = (string)CommandLine;
            BenchmarkHandle.StartInfo.UseShellExecute = false;
            BenchmarkHandle.StartInfo.RedirectStandardError = true;
            BenchmarkHandle.StartInfo.RedirectStandardOutput = true;
            BenchmarkHandle.StartInfo.CreateNoWindow = true;
            if (!BenchmarkHandle.Start()) return null;

            return BenchmarkHandle;
        }


        virtual protected void BenchmarkThreadRoutine(object CommandLine)
        {
            Thread.Sleep(Config.ConfigData.MinerRestartDelayMS);

            bool once = true;
            Stopwatch timer = new Stopwatch();
            BenchmarkSignalQuit = false;

            Process BenchmarkHandle = null;

            try
            {
                Helpers.ConsolePrint("BENCHMARK", "Benchmark starts");
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);

                if (AlgoNameIs("daggerhashimoto"))
                {
                    while (true)
                    {
                        string outdata = BenchmarkHandle.StandardOutput.ReadLine();

                        if (outdata.Contains("No GPU device with sufficient memory was found"))
                            throw new Exception("[daggerhashimoto] No GPU device with sufficient memory was found.");

                        if (BenchmarkParseLine(outdata))
                            break;
                    }
                }
                else if (this is cpuminer && AlgoNameIs("hodl"))
                {
                    int count = BenchmarkTime / 5;
                    double total = 0, tmp;

                    while (count > 0)
                    {
                        string outdata = BenchmarkHandle.StandardError.ReadLine();
                        if (outdata != null)
                        {
                            if (outdata.Contains("Total: "))
                            {
                                int st = outdata.IndexOf("Total:") + 7;
                                int len = outdata.Length - 6 - st;

                                string parse = outdata.Substring(st, len).Trim();
                                Double.TryParse(parse, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp);
                                total += tmp;
                                count--;
                            }
                        }
                        if (BenchmarkSignalQuit)
                            throw new Exception("Termined by user request");
                    }

                    double spd = total / (BenchmarkTime / 5);
                    SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = spd;
                    OnBenchmarkComplete(true, PrintSpeed(spd), BenchmarkTag);
                }
                else
                {
                    if (MinerDeviceName.Equals("AMD_OpenCL"))
                    {
                        int NHDataIndex = SupportedAlgorithms[BenchmarkIndex].NiceHashID;

                        if (Globals.NiceHashData == null)
                        {
                            Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no internet " +
                                "connection. Sgminer needs internet connection to do benchmarking.");

                            throw new Exception("No internet connection");
                        }

                        if (Globals.NiceHashData[NHDataIndex].paying == 0)
                        {
                            Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no work on Nicehash.com " +
                                "[algo: " + SupportedAlgorithms[BenchmarkIndex].NiceHashName + "(" + NHDataIndex + ")]");

                            throw new Exception("No work can be used for benchmarking");
                        }

                        timer.Reset();
                        timer.Start();
                    }

                    while (true)
                    {
                        if (MinerDeviceName.Equals("AMD_OpenCL"))
                        {
                            if (timer.Elapsed.Minutes >= BenchmarkTime + 1 && once == true)
                            {
                                once = false;
                                string resp = GetAPIData(APIPort, "quit").TrimEnd(new char[] { (char)0 });
                                Helpers.ConsolePrint("BENCHMARK", "SGMiner Response: " + resp);
                            }
                            if (timer.Elapsed.Minutes >= BenchmarkTime + 2)
                            {
                                timer.Stop();
                                KillSGMiner();
                                BenchmarkSignalHanged = true;
                            }
                        }

                        string outdata = BenchmarkGetConsoleOutputLine(BenchmarkHandle);
                        if (outdata != null)
                        {
                            if (outdata.Contains("Cuda error"))
                                throw new Exception("CUDA error");
                            if (outdata.Contains("is not supported"))
                                throw new Exception("N/A");
                            if (outdata.Contains("illegal memory access"))
                                throw new Exception("CUDA error");
                            if (outdata.Contains("unknown error"))
                                throw new Exception("Unknown error");
                            if (outdata.Contains("No servers could be used! Exiting."))
                                throw new Exception("No pools or work can be used for benchmarking");
                            if (BenchmarkParseLine(outdata))
                                break;
                        }
                        if (BenchmarkSignalQuit)
                            throw new Exception("Termined by user request");
                        if (BenchmarkSignalHanged)
                            throw new Exception("SGMiner is not responding");
                    }
                }
            }
            catch (Exception ex)
            {
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = 0;

                Helpers.ConsolePrint(MinerDeviceName, "Benchmark Exception: " + ex.Message);

                try { if (BenchmarkHandle != null) BenchmarkHandle.Kill(); }
                catch { }
                
                OnBenchmarkComplete(false, "Terminated", BenchmarkTag);
            }
            Helpers.ConsolePrint("BENCHMARK", "Benchmark ends");

            if (BenchmarkHandle != null)
            {
                try { BenchmarkHandle.Kill(); BenchmarkHandle.Close(); }
                catch { }
            }
        }


        virtual protected string GetPassword(Algorithm a)
        {
            if (a.UsePassword != null && a.UsePassword.Length > 0)
                return a.UsePassword;

            if (UsePassword != null && UsePassword.Length > 0)
                return UsePassword;

            return "x";
        }


        virtual protected NiceHashProcess _Start()
        {
            // check if dagger already running
            if (AlgoNameIs("daggerhashimoto") && ethminerProcess != null)
            {
                Helpers.ConsolePrint(MinerDeviceName, "Resuming ethminer..");
                ethminerLink.StartMining();
                IsRunning = true;
                return null;
            }

            PreviousTotalMH = 0.0;
            if (LastCommandLine.Length == 0 || EnabledDeviceCount() == 0) return null;

            NiceHashProcess P = new NiceHashProcess();

            if (WorkingDirectory.Length > 1)
            {
                P.StartInfo.WorkingDirectory = WorkingDirectory;
            }

            NumRetries = Config.ConfigData.MinerAPIGraceSeconds / Config.ConfigData.MinerAPIQueryInterval;
            if (this is sgminer && !AlgoNameIs("daggerhashimoto"))
                NumRetries = (Config.ConfigData.MinerAPIGraceSeconds + Config.ConfigData.MinerAPIGraceSecondsAMD) / Config.ConfigData.MinerAPIQueryInterval;

            if (AlgoNameIs("daggerhashimoto"))
            {
                ethminerLink = new ethminerAPI((this is sgminer) ? Config.ConfigData.ethminerAPIPortAMD : Config.ConfigData.ethminerAPIPortNvidia);
                P.StartInfo.FileName = Ethereum.EtherMinerPath;
                P.ExitEvent = ethMiner_Exited;
            }
            else
            {
                P.StartInfo.FileName = Path;
                P.ExitEvent = Miner_Exited;
            }

            P.StartInfo.Arguments = LastCommandLine;
            P.StartInfo.CreateNoWindow = Config.ConfigData.HideMiningWindows;
            P.StartInfo.UseShellExecute = false;

            Helpers.ConsolePrint(MinerDeviceName, "Starting miner (" + P.StartInfo.FileName + "): " + LastCommandLine);

            try
            {
                if (P.Start())
                {
                    IsRunning = true;

                    if (AlgoNameIs("daggerhashimoto"))
                    {
                        ethminerProcess = P;
                        return null;
                    }
                    return P;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint(MinerDeviceName, "_Start: " + ex.Message);
                return null;
            }
        }


        virtual protected void Miner_Exited()
        {
            Stop(true);
        }


        virtual protected void ethMiner_Exited()
        {
            Stop(false);
        }


        virtual public void Restart()
        {
            Helpers.ConsolePrint(MinerDeviceName, "Restarting miner..");
            Stop(true); // stop miner first
            if (this is sgminer) StartingUpDelay = true;
            ProcessHandle = _Start(); // start with old command line
        }


        virtual public string PrintSpeed(double spd)
        {
            // print in MH/s
            return (spd * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s";
        }


        public int EnabledDeviceCount()
        {
            int en = 0;

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled) en++;

            return en;
        }
        

        protected void FillAlgorithm(string aname, ref APIData AD)
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].MinerName.Equals(aname))
                {
                    AD.AlgorithmID = SupportedAlgorithms[i].NiceHashID;
                    AD.AlgorithmName = SupportedAlgorithms[i].NiceHashName;
                }
            }
        }


        protected Algorithm GetMinerAlgorithm(int nhid)
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].NiceHashID == nhid)
                {
                    return SupportedAlgorithms[i];
                }
            }

            return null;
        }


        protected string GetAPIData(int port, string cmd)
        {
            string ResponseFromServer = null;
            try
            {
                TcpClient tcpc = new TcpClient("127.0.0.1", port);
                string DataToSend = "GET /" + cmd + " HTTP/1.1\r\n" +
                                    "Host: 127.0.0.1\r\n" +
                                    "User-Agent: NiceHashMiner/" + Application.ProductVersion + "\r\n" +
                                    "\r\n";

                if (MinerDeviceName.Equals("AMD_OpenCL"))
                    DataToSend = cmd;

                byte[] BytesToSend = ASCIIEncoding.ASCII.GetBytes(DataToSend);
                tcpc.Client.Send(BytesToSend);

                byte[] IncomingBuffer = new byte[5000];
                int offset = 0;
                bool fin = false;

                while (!fin && tcpc.Client.Connected)
                {
                    int r = tcpc.Client.Receive(IncomingBuffer, offset, 5000 - offset, SocketFlags.None);
                    for (int i = offset; i < offset + r; i++)
                    {
                        if (IncomingBuffer[i] == 0x7C || IncomingBuffer[i] == 0x00)
                        {
                            fin = true;
                            break;
                        }
                    }
                    offset += r;
                }

                tcpc.Close();

                if (offset > 0)
                    ResponseFromServer = ASCIIEncoding.ASCII.GetString(IncomingBuffer);
            }
            catch
            {
                return null;
            }

            return ResponseFromServer;
        }


        public APIData GetSummary()
        {
            string resp;
            string aname = null;
            APIData ad = new APIData();

            if (AlgoNameIs("daggerhashimoto"))
            {
                FillAlgorithm("daggerhashimoto", ref ad);

                bool ismining;
                if (!ethminerLink.GetSpeed(out ismining, out ad.Speed))
                {
                    if (NumRetries > 0)
                    {
                        NumRetries--;
                        ad.Speed = 0;
                        return ad;
                    }

                    Helpers.ConsolePrint(MinerDeviceName, "ethminer is not running.. restarting..");
                    Stop(false);
                    _Start();
                    ad.Speed = 0;
                    return ad;
                }
                else if (!ismining)
                {
                    // resend start mining command
                    ethminerLink.StartMining();
                }
                ad.Speed *= 1000 * 1000;
            }
            else
            {
                resp = GetAPIData(APIPort, "summary");
                if (resp == null) return null;

                try
                {
                    string[] resps;

                    if (!MinerDeviceName.Equals("AMD_OpenCL"))
                    {
                        resps = resp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < resps.Length; i++)
                        {
                            string[] optval = resps[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            if (optval.Length != 2) continue;
                            if (optval[0] == "ALGO")
                                aname = optval[1];
                            else if (optval[0] == "KHS")
                                ad.Speed = double.Parse(optval[1], CultureInfo.InvariantCulture) * 1000; // HPS
                        }
                    }
                    else
                    {
                        // Checks if all the GPUs are Alive first
                        string resp2 = GetAPIData(APIPort, "devs");
                        if (resp2 == null) return null;

                        string[] checkGPUStatus = resp2.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 1; i < checkGPUStatus.Length - 1; i++)
                        {
                            if (!checkGPUStatus[i].Contains("Status=Alive"))
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "GPU " + i + ": Sick/Dead/NoStart/Initialising/Disabled/Rejecting/Unknown");
                                return null;
                            }
                        }

                        resps = resp.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        if (resps[1].Contains("SUMMARY"))
                        {
                            string[] data = resps[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            // Get miner's current total speed
                            string[] speed = data[4].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            // Get miner's current total MH
                            double total_mh = Double.Parse(data[18].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1], new CultureInfo("en-US"));

                            ad.Speed = Double.Parse(speed[1]) * 1000;

                            aname = SupportedAlgorithms[CurrentAlgo].MinerName;

                            if (total_mh <= PreviousTotalMH)
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "SGMiner might be stuck as no new hashes are being produced");
                                Helpers.ConsolePrint(MinerDeviceName, "Prev Total MH: " + PreviousTotalMH + " .. Current Total MH: " + total_mh);
                                return null;
                            }

                            PreviousTotalMH = total_mh;
                        }
                        else
                        {
                            ad.Speed = 0;
                        }
                    }
                }
                catch
                {
                    return null;
                }
            
                FillAlgorithm(aname, ref ad);
            }

            return ad;
        }


        virtual public int GetMaxProfitIndex(NiceHashSMA[] NiceHashData)
        {
            double MaxProfit = -1;
            int MaxProfitIndex = -1;

            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].Skip) continue;
                if (EnabledDevicePerAlgoCount(i) == 0) continue;

                SupportedAlgorithms[i].CurrentProfit = SupportedAlgorithms[i].BenchmarkSpeed *
                    NiceHashData[SupportedAlgorithms[i].NiceHashID].paying * 0.000000001;

                Helpers.ConsolePrint(MinerDeviceName, NiceHashData[SupportedAlgorithms[i].NiceHashID].name +
                                     " paying " + SupportedAlgorithms[i].CurrentProfit.ToString("F8") + " BTC/Day");

                if (SupportedAlgorithms[i].CurrentProfit > MaxProfit)
                {
                    MaxProfit = SupportedAlgorithms[i].CurrentProfit;
                    MaxProfitIndex = i;
                }
            }

            if ((MaxProfit * Globals.BitcoinRate) < MinimumProfit)
                NotProfitable = true;
            else
                NotProfitable = false;


            return MaxProfitIndex;
        }


        public int GetAlgoIndex(string aname)
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
                if (SupportedAlgorithms[i].NiceHashName.Equals(aname))
                    return i;
            
            return 0;
        }


        public bool AlgoNameIs(string algoname)
        {
            try
            {
                if (SupportedAlgorithms[CurrentAlgo].NiceHashName.Equals(algoname))
                    return true;
            }
            catch { return false; }

            return false;
        }

        public int CountBenchmarkedAlgos()
        {
            int count = 0;

            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                if (SupportedAlgorithms[i].BenchmarkSpeed > 0)
                    count++;
            }

            return count;
        }

        public void GetDisabledDevicePerAlgo()
        {
            for (int i = 0; i < SupportedAlgorithms.Length; i++)
            {
                SupportedAlgorithms[i].DisabledDevice = new bool[CDevs.Count];
                for (int j = 0; j < CDevs.Count; j++)
                {
                    SupportedAlgorithms[i].DisabledDevice[j] = false;
                    if ((CDevs[j].Name.Contains("750") && CDevs[j].Name.Contains("Ti")) &&
                        (SupportedAlgorithms[i].NiceHashName.Equals("daggerhashimoto")))
                    {
                        Helpers.ConsolePrint(MinerDeviceName, "GTX 750Ti found! By default this device will be disabled for ethereum as it is generally too slow to mine on it.");
                        SupportedAlgorithms[i].DisabledDevice[j] = true;
                    }
                }
            }
        }

        public int EnabledDevicePerAlgoCount(int algoIndex)
        {
            int count = 0;

            for (int i = 0; i < CDevs.Count; i++)
            {
                if (CDevs[i].Enabled && !SupportedAlgorithms[algoIndex].DisabledDevice[i])
                {
                    if (SupportedAlgorithms[algoIndex].NiceHashName.Equals("daggerhashimoto"))
                    {
                        if (EtherDevices[i] != -1)
                            count++;
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
