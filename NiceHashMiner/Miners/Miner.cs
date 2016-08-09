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
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

namespace NiceHashMiner
{
    public class APIData
    {
        public AlgorithmType AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }

    public delegate void BenchmarkComplete(bool success, string text, object tag);

    public abstract class Miner
    {
        public string MinerDeviceName { get; protected set; }
        protected int APIPort { get; private set; }
        protected List<ComputeDevice> CDevs;
        
        // TODO remove
        //public Dictionary<AlgorithmType, Algorithm> SupportedAlgorithms;
        
        // this is now related to devices
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double MinimumProfit;
        
        public AlgorithmType CurrentAlgorithmType { get; protected set; }
        private Algorithm _currentMiningAlgorithm;
        public Algorithm CurrentMiningAlgorithm {
            get { return _currentMiningAlgorithm; }
            protected set {
                if (value == null) {
                    CurrentAlgorithmType = AlgorithmType.NONE;
                } else {
                    CurrentAlgorithmType = value.NiceHashID;
                }
                _currentMiningAlgorithm = value;
            }
        }
        public double CurrentRate;
        public bool NotProfitable;
        public bool IsRunning { get; protected set; }
        public bool BenchmarkSignalQuit;
        public bool BenchmarkSignalHanged;
        public int NumRetries;
        public bool StartingUpDelay;
        protected string Path;

        protected int[] EtherDevices;
        protected string WorkingDirectory;
        protected NiceHashProcess ProcessHandle;
        protected BenchmarkComplete OnBenchmarkComplete;
        protected object BenchmarkTag;
        protected AlgorithmType BenchmarkKey;
        Algorithm BenchmarkAlgorithm = null;
        protected int BenchmarkTime;
        protected string LastCommandLine;
        protected double PreviousTotalMH;

        

        private bool QueryComputeDevices;
        protected bool _isEthMinerExit = false;
        protected AlgorithmType[] _supportedMinerAlgorithms;

        // queryComputeDevices is a quickfix to decouple device querying, TODO move to dev query logic
        public Miner(bool queryComputeDevices)
        {
            CDevs = new List<ComputeDevice>();

            WorkingDirectory = "";
            ExtraLaunchParameters = "";
            UsePassword = null;
            StartingUpDelay = false;

            CurrentAlgorithmType = AlgorithmType.NONE;
            CurrentRate = 0;
            NotProfitable = true;
            IsRunning = false;
            PreviousTotalMH = 0.0;

            QueryComputeDevices = queryComputeDevices;

            InitSupportedMinerAlgorithms();

            APIPort = MinersApiPortsManager.Instance.GetAvaliablePort(GetMinerType());
        }

        ~Miner() {
            // free the port
            MinersApiPortsManager.Instance.RemovePort(APIPort);
        }

        abstract protected void QueryCDevs();
        abstract protected bool IsGroupQueryEnabled();

        protected void TryQueryCDevs() {
            if (QueryComputeDevices && IsGroupQueryEnabled()) {
                QueryCDevs();
            }
        }

        virtual public void SetCDevs(string[] deviceUUIDs) {
            foreach (var uuid in deviceUUIDs) {
                CDevs.Add(ComputeDevice.GetDeviceWithUUID(uuid));
            }
        }

        public bool IsSupportedMinerAlgorithms(AlgorithmType algorithmType) {
            foreach (var supportedType in _supportedMinerAlgorithms) {
                if (supportedType == algorithmType) return true;
            }
            return false;
        }

        protected abstract MinerType GetMinerType();

        protected abstract void InitSupportedMinerAlgorithms();

        /// <summary>
        /// GetOptimizedMinerPath returns optimized miner path based on algorithm type
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <returns>Optimized or default string path</returns>
        abstract protected string GetOptimizedMinerPath(AlgorithmType algorithmType);

        public void KillSGMiner()
        {
            foreach (Process process in Process.GetProcessesByName("sgminer"))
            {
                try { process.Kill(); }
                catch (Exception e) { Helpers.ConsolePrint(MinerDeviceName, e.ToString()); }
            }
        }

        abstract public void Start(Algorithm miningAlgorithm, string url, string username);


        abstract protected void _Stop(bool willswitch);
        virtual public void Stop(bool willswitch)
        {
            _Stop(willswitch);

            StartingUpDelay = false;
            PreviousTotalMH = 0.0;
            NotProfitable = true;
            //IsRunning = false;
            //CurrentAlgorithmType = -1;
        }

        public void End() {
            if (IsRunning) {
                Stop(false);
                IsRunning = false;
                CurrentAlgorithmType = AlgorithmType.NONE;
                CurrentRate = 0;
            }
        }

        protected void Stop_cpu_ccminer_sgminer(bool willswitch) {
            Helpers.ConsolePrint(MinerDeviceName, "Shutting down miner");

            if (ProcessHandle != null) {
                try { ProcessHandle.Kill(); } catch { }
                ProcessHandle.Close();
                ProcessHandle = null;

                if (MinerDeviceName == "AMD_OpenCL") KillSGMiner();
            }
        }

        virtual protected string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            List<string> ids = new List<string>();
            foreach (var cdev in CDevs) {
                ids.Add(cdev.ID.ToString());
            }
            deviceStringCommand += string.Join(",", ids);

            return deviceStringCommand;
        }


        #region BENCHMARK DE-COUPLED Decoupled benchmarking routines

        abstract protected string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time);

        // The benchmark config and algorithm must guarantee that they are compatible with miner
        // we guarantee algorithm is supported
        // we will not have empty benchmark configs, all benchmark configs will have device list
        virtual public void BenchmarkStart(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time, BenchmarkComplete oncomplete, object tag) {
            OnBenchmarkComplete = oncomplete;

            BenchmarkTag = tag;
            BenchmarkAlgorithm = algorithm;
            CurrentAlgorithmType = algorithm.NiceHashID; // find a way to decouple this as well
            BenchmarkTime = time;

            string CommandLine = BenchmarkCreateCommandLine(benchmarkConfig, algorithm, time);

            Thread BenchmarkThread = new Thread(BenchmarkThreadRoutine);
            BenchmarkThread.Start(CommandLine);
        }

        virtual protected Process BenchmarkStartProcess(string CommandLine) {
            Helpers.ConsolePrint(MinerDeviceName, "Starting benchmark: " + CommandLine);

            Process BenchmarkHandle = new Process();
            if (BenchmarkAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto)
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

        virtual protected void BenchmarkThreadRoutine(object CommandLine) {
            Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);

            bool once = true;
            Stopwatch timer = new Stopwatch();
            BenchmarkSignalQuit = false;

            Process BenchmarkHandle = null;

            try {
                Helpers.ConsolePrint("BENCHMARK", "Benchmark starts");
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);

                if (BenchmarkAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto) {
                    while (true) {
                        string outdata = BenchmarkHandle.StandardOutput.ReadLine();

                        if (outdata.Contains("No GPU device with sufficient memory was found"))
                            throw new Exception("[daggerhashimoto] No GPU device with sufficient memory was found.");

                        if (BenchmarkParseLine(outdata))
                            break;
                    }
                } else if (this is cpuminer && BenchmarkAlgorithm.NiceHashID == AlgorithmType.Hodl) {
                    int count = BenchmarkTime / 5;
                    double total = 0, tmp;

                    while (count > 0) {
                        string outdata = BenchmarkHandle.StandardError.ReadLine();
                        if (outdata != null) {
                            if (outdata.Contains("Total: ")) {
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
                    BenchmarkAlgorithm.BenchmarkSpeed = spd;
                    OnBenchmarkComplete(true, PrintSpeed(spd), BenchmarkTag);
                } else {
                    if (MinerDeviceName.Equals("AMD_OpenCL")) {
                        AlgorithmType NHDataIndex = BenchmarkAlgorithm.NiceHashID;

                        if (Globals.NiceHashData == null) {
                            Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no internet " +
                                "connection. Sgminer needs internet connection to do benchmarking.");

                            throw new Exception("No internet connection");
                        }

                        if (Globals.NiceHashData[NHDataIndex].paying == 0) {
                            Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no work on Nicehash.com " +
                                "[algo: " + BenchmarkAlgorithm.NiceHashName + "(" + NHDataIndex + ")]");

                            throw new Exception("No work can be used for benchmarking");
                        }

                        timer.Reset();
                        timer.Start();
                    }

                    while (true) {
                        if (MinerDeviceName.Equals("AMD_OpenCL")) {
                            if (timer.Elapsed.Minutes >= BenchmarkTime + 1 && once == true) {
                                once = false;
                                string resp = GetAPIData(APIPort, "quit").TrimEnd(new char[] { (char)0 });
                                Helpers.ConsolePrint("BENCHMARK", "SGMiner Response: " + resp);
                            }
                            if (timer.Elapsed.Minutes >= BenchmarkTime + 2) {
                                timer.Stop();
                                KillSGMiner();
                                BenchmarkSignalHanged = true;
                            }
                        }

                        var outdataStd_out_err = BenchmarkGetConsoleOutputLine(BenchmarkHandle);
                        bool whileBreak = false;
                        foreach (var outdata in outdataStd_out_err) {
                            if (outdata != null) {
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
                                if (BenchmarkParseLine(outdata)) {
                                    whileBreak = true;
                                    break;
                                }
                            }
                        }
                        if (whileBreak) break;
                        if (BenchmarkSignalQuit)
                            throw new Exception("Termined by user request");
                        if (BenchmarkSignalHanged)
                            throw new Exception("SGMiner is not responding");
                    }
                }
            } catch (Exception ex) {
                BenchmarkAlgorithm.BenchmarkSpeed = 0;

                Helpers.ConsolePrint(MinerDeviceName, "Benchmark Exception: " + ex.Message);

                try { if (BenchmarkHandle != null) BenchmarkHandle.Kill(); } catch { }
                if (OnBenchmarkComplete != null) OnBenchmarkComplete(false, "Terminated", BenchmarkTag);
            }
            Helpers.ConsolePrint("BENCHMARK", "Benchmark ends");

            if (BenchmarkHandle != null) {
                try { BenchmarkHandle.Kill(); BenchmarkHandle.Close(); } catch { }
            }
        }

        virtual protected bool BenchmarkParseLine(string outdata) {
            // parse line
            if (outdata.Contains("Benchmark: ") && outdata.Contains("/s")) {
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
                BenchmarkAlgorithm.BenchmarkSpeed = spd;

                OnBenchmarkComplete(true, PrintSpeed(spd), BenchmarkTag);
                return true;
            } else if (outdata.Contains("Average hashrate:") && outdata.Contains("/s")) {
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

                BenchmarkAlgorithm.BenchmarkSpeed = speed;

                OnBenchmarkComplete(true, PrintSpeed(speed), BenchmarkTag);
                return true;
            } else if (outdata.Contains("min/mean/max:")) {
                string[] splt = outdata.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                int index = Array.IndexOf(splt, "mean");
                double avg_spd = Convert.ToDouble(splt[index + 2]);
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + avg_spd + "H/s");

                BenchmarkAlgorithm.BenchmarkSpeed = avg_spd;

                OnBenchmarkComplete(true, PrintSpeed(avg_spd), BenchmarkTag);
                return true;
            }

            return false;
        }

        // returns stdout and stderr
        private string[] BenchmarkGetConsoleOutputLine(Process BenchmarkHandle) {
            return new string[] {
                BenchmarkHandle.StandardOutput.ReadLine(),
                BenchmarkHandle.StandardError.ReadLine()
            };
        }

        #endregion //BENCHMARK DE-COUPLED Decoupled benchmarking routines


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
            PreviousTotalMH = 0.0;
            if (LastCommandLine.Length == 0) return null;

            NiceHashProcess P = new NiceHashProcess();

            if (WorkingDirectory.Length > 1)
            {
                P.StartInfo.WorkingDirectory = WorkingDirectory;
            }

            NumRetries = ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds / ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval;
            if (this is sgminer && !IsCurrentAlgo(AlgorithmType.DaggerHashimoto))
                NumRetries = (ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds + ConfigManager.Instance.GeneralConfig.MinerAPIGraceSecondsAMD) / ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval;

            P.StartInfo.FileName = Path;
            P.ExitEvent = Miner_Exited;

            P.StartInfo.Arguments = LastCommandLine;
            P.StartInfo.CreateNoWindow = ConfigManager.Instance.GeneralConfig.HideMiningWindows;
            P.StartInfo.UseShellExecute = false;

            Helpers.ConsolePrint(MinerDeviceName, "Starting miner (" + P.StartInfo.FileName + "): " + LastCommandLine);

            try
            {
                if (P.Start())
                {
                    IsRunning = true;
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


        virtual protected void Miner_Exited() {
            bool willswitch = _isEthMinerExit ? false : true;
            Stop(willswitch);
            //Stop(true);
        }
        //virtual protected void ethMiner_Exited()
        //{
        //    Stop(false);
        //}


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

        protected void FillAlgorithm(string aname, ref APIData AD) {
            // TODO this check is not really needed
            if (CurrentMiningAlgorithm.MinerName.Equals(aname)) {
                AD.AlgorithmID = CurrentMiningAlgorithm.NiceHashID;
                AD.AlgorithmName = CurrentMiningAlgorithm.NiceHashName;
            }
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


        public abstract APIData GetSummary();

        protected APIData GetSummaryCPU_CCMINER() {
            string resp;
            string aname = null;
            APIData ad = new APIData();

            resp = GetAPIData(APIPort, "summary");
            if (resp == null) return null;

            try {
                string[] resps = resp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < resps.Length; i++) {
                    string[] optval = resps[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (optval.Length != 2) continue;
                    if (optval[0] == "ALGO")
                        aname = optval[1];
                    else if (optval[0] == "KHS")
                        ad.Speed = double.Parse(optval[1], CultureInfo.InvariantCulture) * 1000; // HPS
                }
            } catch {
                return null;
            }

            FillAlgorithm(aname, ref ad);
            return ad;
        }

        

        //virtual public AlgorithmType GetMaxProfitKey(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData)
        //{
        //    double MaxProfit = -1;
        //    AlgorithmType MaxProfitIndex = AlgorithmType.NONE;

        //    foreach (var key in SupportedAlgorithms.Keys)
        //    {
        //        if (SupportedAlgorithms[key].Skip) continue;
        //        if (EnabledDevicePerAlgoCount(key) == 0) continue;

        //        SupportedAlgorithms[key].CurrentProfit = SupportedAlgorithms[key].BenchmarkSpeed *
        //            NiceHashData[SupportedAlgorithms[key].NiceHashID].paying * 0.000000001;

        //        Helpers.ConsolePrint(MinerDeviceName, NiceHashData[SupportedAlgorithms[key].NiceHashID].name +
        //                             " paying " + SupportedAlgorithms[key].CurrentProfit.ToString("F8") + " BTC/Day");

        //        if (SupportedAlgorithms[key].CurrentProfit > MaxProfit)
        //        {
        //            MaxProfit = SupportedAlgorithms[key].CurrentProfit;
        //            MaxProfitIndex = key;
        //        }
        //    }

        //    if ((MaxProfit * Globals.BitcoinRate) < MinimumProfit)
        //        NotProfitable = true;
        //    else
        //        NotProfitable = false;


        //    return MaxProfitIndex;
        //}

        // todo remove
        public bool IsCurrentAlgo(AlgorithmType algorithmType) {
            return CurrentAlgorithmType == algorithmType;
        }

        //// TODO replace this
        // TODO IMPORTANT put this in the DeviceQuery Manager
        //public void GetDisabledDevicePerAlgo()
        //{
        //    foreach (var key in SupportedAlgorithms.Keys)
        //    {
        //        //SupportedAlgorithms[key].DisabledDevice = new bool[CDevs.Count];
        //        for (int j = 0; j < CDevs.Count; j++)
        //        {
        //            //SupportedAlgorithms[key].DisabledDevice[j] = false;
        //            if ((CDevs[j].Name.Contains("750") && CDevs[j].Name.Contains("Ti")) &&
        //                SupportedAlgorithms[key].NiceHashID == AlgorithmType.DaggerHashimoto)
        //            {
        //                Helpers.ConsolePrint(MinerDeviceName, "GTX 750Ti found! By default this device will be disabled for ethereum as it is generally too slow to mine on it.");
        //                //SupportedAlgorithms[key].DisabledDevice[j] = true;
        //            }
        //        }
        //    }
        //}
    }
}
