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
using NiceHashMiner.Interfaces;

namespace NiceHashMiner
{
    public class APIData
    {
        public AlgorithmType AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }

    public abstract class Miner
    {
        public string MinerDeviceName { get; protected set; }
        protected int APIPort { get; private set; }
        protected List<ComputeDevice> CDevs;
        
        // this is now related to devices
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double MinimumProfit;
        
        public AlgorithmType CurrentAlgorithmType { get; protected set; }
        private Algorithm _currentMiningAlgorithm;
        protected Algorithm CurrentMiningAlgorithm {
            get { return _currentMiningAlgorithm; }
            set {
                if (value == null) {
                    CurrentAlgorithmType = AlgorithmType.NONE;
                } else {
                    CurrentAlgorithmType = value.NiceHashID;
                }
                _currentMiningAlgorithm = value;
            }
        }
        public double CurrentRate;
        public bool IsRunning { get; protected set; }
        public bool BenchmarkSignalQuit;
        public bool BenchmarkSignalHanged;
        Stopwatch BenchmarkTimeOutStopWatch = null;
        public bool BenchmarkSignalTimedout = false;
        protected bool BenchmarkSignalFinnished;
        public int NumRetries;
        public bool StartingUpDelay;
        protected string Path;

        protected string WorkingDirectory;
        protected NiceHashProcess ProcessHandle;

        // Benchmark stuff
        IBenchmarkComunicator BenchmarkComunicator;
        private bool OnBenchmarkCompleteCalled = false; 
        protected AlgorithmType CurrentBenchmarkAlgorithmType { get; private set; }
        private Algorithm _benchmarkAlgorithm;
        protected Algorithm BenchmarkAlgorithm {
            get { return _benchmarkAlgorithm; }
            set {
                if (value == null) {
                    CurrentBenchmarkAlgorithmType = AlgorithmType.NONE;
                } else {
                    CurrentBenchmarkAlgorithmType = value.NiceHashID;
                }
                _benchmarkAlgorithm = value;
            }
        }
        public BenchmarkProcessStatus BenchmarkProcessStatus { get; private set; }
        private string BenchmarkProcessPath;
        private Process BenchmarkHandle = null;
        protected Exception BenchmarkException = null;
        protected int BenchmarkTimeInSeconds;
        protected string LastCommandLine { get; set; }
        protected double PreviousTotalMH;

        
        protected bool _isEthMinerExit = false;
        protected AlgorithmType[] _supportedMinerAlgorithms;

        public Miner()
        {
            CDevs = new List<ComputeDevice>();

            WorkingDirectory = "";
            ExtraLaunchParameters = "";
            UsePassword = null;
            StartingUpDelay = false;

            CurrentAlgorithmType = AlgorithmType.NONE;
            CurrentRate = 0;
            IsRunning = false;
            PreviousTotalMH = 0.0;

            InitSupportedMinerAlgorithms();

            APIPort = MinersApiPortsManager.Instance.GetAvaliablePort(GetMinerType());
        }

        ~Miner() {
            // free the port
            MinersApiPortsManager.Instance.RemovePort(APIPort);
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
        /// GetOptimizedMinerPath returns optimized miner path based on algorithm type and device codename.
        /// Device codename is a quickfix for sgminer, other miners don't use it
        /// </summary>
        /// <param name="algorithmType">determines what miner path to return</param>
        /// <param name="devCodename">sgminer extra</param>
        /// <param name="isOptimized">sgminer extra</param>
        /// <returns></returns>
        abstract public string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename = "", bool isOptimized = true);

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
            IsRunning = false;
        }

        public void End() {
            Stop(false);
            IsRunning = false;
            CurrentAlgorithmType = AlgorithmType.NONE;
            CurrentRate = 0;
        }

        protected void ChangeToNextAvaliablePort() {
            // change to new port
            var oldApiPort = APIPort;
            var newApiPort = MinersApiPortsManager.Instance.GetAvaliablePort(0);
            // check if update last command port
            if (UpdateBindPortCommand(oldApiPort, newApiPort)) {
                Helpers.ConsolePrint(MinerDeviceName, String.Format("Changing miner port from {0} to {1}",
                    oldApiPort.ToString(),
                    newApiPort.ToString()));
                // free old set new
                MinersApiPortsManager.Instance.RemovePort(oldApiPort);
                APIPort = newApiPort;
            } else { // release new
                MinersApiPortsManager.Instance.RemovePort(newApiPort);
            }
        }

        protected void Stop_cpu_ccminer_sgminer(bool willswitch) {
            Helpers.ConsolePrint(MinerDeviceName, "Shutting down miner");
            ChangeToNextAvaliablePort();

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

        public int BenchmarkTimeoutInSeconds(int timeInSeconds) {
            if (CurrentBenchmarkAlgorithmType == AlgorithmType.DaggerHashimoto) {
                return 4 * 60 + 120; // 4 minutes plus two minutes
            } 
            return timeInSeconds + 120; // wait time plus two minutes
        }

        abstract protected string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time);

        // The benchmark config and algorithm must guarantee that they are compatible with miner
        // we guarantee algorithm is supported
        // we will not have empty benchmark configs, all benchmark configs will have device list
        virtual public void BenchmarkStart(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time, IBenchmarkComunicator benchmarkComunicator) {
            
            BenchmarkComunicator = benchmarkComunicator;
            BenchmarkAlgorithm = algorithm;
            BenchmarkTimeInSeconds = time;
            BenchmarkSignalFinnished = true;
            // check and kill 
            BenchmarkHandle = null;
            OnBenchmarkCompleteCalled = false;
            BenchmarkTimeOutStopWatch = null;

            string CommandLine = BenchmarkCreateCommandLine(benchmarkConfig, algorithm, time);

            Thread BenchmarkThread = new Thread(BenchmarkThreadRoutine);
            BenchmarkThread.Start(CommandLine);
        }

        virtual protected Process BenchmarkStartProcess(string CommandLine) {
            Helpers.ConsolePrint(MinerDeviceName, "Starting benchmark: " + CommandLine);

            Process BenchmarkHandle = new Process();
            
            BenchmarkHandle.StartInfo.FileName = GetOptimizedMinerPath(BenchmarkAlgorithm.NiceHashID);

            // TODO sgminer quickfix
            if (this is sgminer) {
                BenchmarkProcessPath = "cmd / " + BenchmarkHandle.StartInfo.FileName;
                BenchmarkHandle.StartInfo.FileName = "cmd";
            } else {
                BenchmarkProcessPath = BenchmarkHandle.StartInfo.FileName;
                Helpers.ConsolePrint(MinerDeviceName, "Using miner: " + BenchmarkHandle.StartInfo.FileName);
            }

            BenchmarkHandle.StartInfo.Arguments = (string)CommandLine;
            BenchmarkHandle.StartInfo.UseShellExecute = false;
            BenchmarkHandle.StartInfo.RedirectStandardError = true;
            BenchmarkHandle.StartInfo.RedirectStandardOutput = true;
            BenchmarkHandle.StartInfo.CreateNoWindow = true;
            BenchmarkHandle.OutputDataReceived += BenchmarkOutputErrorDataReceived;
            BenchmarkHandle.ErrorDataReceived += BenchmarkOutputErrorDataReceived;

            if (!BenchmarkHandle.Start()) return null;

            return BenchmarkHandle;
        }


        private string ElapsedTimeString(long timeMillies) {
            if (timeMillies < 1000) {
                return timeMillies.ToString() + " milliseconds";
            } else if (timeMillies < 60 * 1000) {
                return (timeMillies / 1000).ToString() + " seconds";
            }
            return (timeMillies / (60 * 1000)).ToString() + " minutes";
        }

        private void BenchmarkOutputErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (BenchmarkTimeOutStopWatch == null) {
                BenchmarkTimeOutStopWatch = new Stopwatch();
                BenchmarkTimeOutStopWatch.Start();
            } else if (BenchmarkTimeOutStopWatch.ElapsedMilliseconds > BenchmarkTimeoutInSeconds(BenchmarkTimeInSeconds) * 1000) {
                BenchmarkTimeOutStopWatch.Stop();
                BenchmarkSignalTimedout = true;
            }

            string outdata = e.Data;
            if (e.Data != null) {
                BenchmarkOutputErrorDataReceivedImpl(outdata);
            }
            // terminate process situations
            if (BenchmarkSignalQuit
                || BenchmarkSignalFinnished
                || BenchmarkSignalHanged
                || BenchmarkSignalTimedout
                || BenchmarkException != null) {
                EndBenchmarkProcces();
            }
        }

        protected abstract void BenchmarkOutputErrorDataReceivedImpl(string outdata);

        protected void CheckOutdata(string outdata) {
            // ccminer, cpuminer
            if (outdata.Contains("Cuda error"))
                BenchmarkException = new Exception("CUDA error");
            if (outdata.Contains("is not supported"))
                BenchmarkException = new Exception("N/A");
            if (outdata.Contains("illegal memory access"))
                BenchmarkException = new Exception("CUDA error");
            if (outdata.Contains("unknown error"))
                BenchmarkException = new Exception("Unknown error");
            if (outdata.Contains("No servers could be used! Exiting."))
                BenchmarkException = new Exception("No pools or work can be used for benchmarking");
            //if (outdata.Contains("error") || outdata.Contains("Error"))
            //    BenchmarkException = new Exception("Unknown error #2");
            // Ethminer
            if (outdata.Contains("No GPU device with sufficient memory was found"))
                BenchmarkException = new Exception("[daggerhashimoto] No GPU device with sufficient memory was found.");

            // lastly parse data
            if (BenchmarkParseLine(outdata)) {
                BenchmarkSignalFinnished = true;
            }
        }

        // killing proccesses can take time
        public void EndBenchmarkProcces() {
            if (BenchmarkHandle != null && BenchmarkProcessStatus != BenchmarkProcessStatus.Killing) {
                BenchmarkProcessStatus = BenchmarkProcessStatus.Killing;
                try {
                    Helpers.ConsolePrint("BENCHMARK", String.Format("Trying to kill benchmark process {0} algorithm {1}", BenchmarkProcessPath, BenchmarkAlgorithm.NiceHashName));
                    BenchmarkHandle.Kill();
                    BenchmarkHandle.Close();
                } catch { }
                finally {
                    BenchmarkProcessStatus = BenchmarkProcessStatus.DoneKilling;
                    Helpers.ConsolePrint("BENCHMARK", String.Format("Benchmark process {0} algorithm {1} KILLED", BenchmarkProcessPath, BenchmarkAlgorithm.NiceHashName));
                    //BenchmarkHandle = null;
                }
            }
        }


        virtual protected void BenchmarkThreadRoutineStartSettup() {
            BenchmarkHandle.BeginErrorReadLine();
            BenchmarkHandle.BeginOutputReadLine();
        }

        virtual protected void BenchmarkThreadRoutine(object CommandLine) {
            Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);

            BenchmarkSignalQuit = false;
            BenchmarkSignalHanged = false;
            BenchmarkSignalFinnished = false;
            BenchmarkException = null;

            try {
                Helpers.ConsolePrint("BENCHMARK", "Benchmark starts");
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);

                BenchmarkThreadRoutineStartSettup();
                // wait a little longer then the benchmark routine if exit false throw
                //var timeoutTime = BenchmarkTimeoutInSeconds(BenchmarkTimeInSeconds);
                //var exitSucces = BenchmarkHandle.WaitForExit(timeoutTime * 1000);
                // don't use wait for it breaks everything
                BenchmarkProcessStatus = BenchmarkProcessStatus.Running;
                BenchmarkHandle.WaitForExit();
                if (BenchmarkSignalTimedout) {
                    throw new Exception("Benchmark timedout");
                }
                if (BenchmarkException != null) {
                    throw BenchmarkException;
                }
                if (BenchmarkSignalQuit) {
                    throw new Exception("Termined by user request");
                }
                if (BenchmarkSignalHanged) {
                    throw new Exception("SGMiner is not responding");
                }
                if (BenchmarkSignalFinnished) {
                    //break;
                }
            } catch (Exception ex) {
                BenchmarkAlgorithm.BenchmarkSpeed = 0;

                Helpers.ConsolePrint(MinerDeviceName, "Benchmark Exception: " + ex.Message);
                if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                    OnBenchmarkCompleteCalled = true;
                    BenchmarkComunicator.OnBenchmarkComplete(false, BenchmarkSignalTimedout ? "Timedout" : "Terminated");
                }
            } finally {
                BenchmarkProcessStatus = BenchmarkProcessStatus.Success;
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + Helpers.FormatSpeedOutput(BenchmarkAlgorithm.BenchmarkSpeed));
                Helpers.ConsolePrint("BENCHMARK", "Benchmark ends");
                if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                    OnBenchmarkCompleteCalled = true;
                    BenchmarkComunicator.OnBenchmarkComplete(true, "Success");
                }
            }
        }

        abstract protected bool BenchmarkParseLine(string outdata);

        #endregion //BENCHMARK DE-COUPLED Decoupled benchmarking routines


        virtual protected string GetPassword(Algorithm a)
        {
            if (a.UsePassword != null && a.UsePassword.Length > 0)
                return a.UsePassword;

            if (UsePassword != null && UsePassword.Length > 0)
                return UsePassword;

            return "x";
        }

        virtual protected int CalculateNumRetries() {
            return ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds / ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval;
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

            NumRetries = CalculateNumRetries();

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

        protected abstract bool UpdateBindPortCommand(int oldPort, int newPort);

        protected bool UpdateBindPortCommand_ccminer_cpuminer(int oldPort, int newPort) {
            // --api-bind=
            const string MASK = "--api-bind={0}";
            var oldApiBindStr = String.Format(MASK, oldPort);
            var newApiBindStr = String.Format(MASK, newPort);
            if (LastCommandLine.Contains(oldApiBindStr)) {
                LastCommandLine = LastCommandLine.Replace(oldApiBindStr, newApiBindStr);
                return true;
            }
            return false;
        }

        virtual public void Restart() {
            Helpers.ConsolePrint(MinerDeviceName, "Restarting miner..");
            Stop(true); // stop miner first
            ProcessHandle = _Start(); // start with old command line
        }

        protected void FillAlgorithm(string aname, ref APIData AD) {
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
                Helpers.ConsolePrint(MinerDeviceName, "Could not read data from API bind port");
                return null;
            }

            FillAlgorithm(aname, ref ad);
            return ad;
        }
    }
}
