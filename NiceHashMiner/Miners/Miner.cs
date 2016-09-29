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

using Timer = System.Timers.Timer;
using System.Timers;

namespace NiceHashMiner
{
    public class APIData
    {
        public AlgorithmType AlgorithmID;
        public string AlgorithmName;
        public double Speed;
    }

    // 
    public class MinerPID_Data {
        public string minerBinPath = null;
        public int PID = -1;
    }

    public abstract class Miner {


        // MINER_ID_COUNT used to identify miners creation
        protected static long MINER_ID_COUNT { get; private set; }

        // used to identify miner instance
        protected readonly long MINER_ID;
        private string _minetTag = null;
        public string MinerDeviceName { get; private set; }
        protected int APIPort { get; private set; }
        // if miner has no API bind port for reading curentlly only CryptoNight on ccminer
        public bool IsAPIReadException { get; protected set; }
        protected List<ComputeDevice> CDevs;
        public DeviceType DeviceType { get; private set; }

        // mining algorithm stuff
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
        protected string Path;
        protected string LastCommandLine { get; set; }
        // TODO check this 
        protected double PreviousTotalMH;
        // the defaults will be 
        protected string WorkingDirectory;
        protected NiceHashProcess ProcessHandle;
        private MinerPID_Data _currentPidData;
        private List<MinerPID_Data> _allPidData = new List<MinerPID_Data>();

        // Benchmark stuff
        public bool BenchmarkSignalQuit;
        public bool BenchmarkSignalHanged;
        Stopwatch BenchmarkTimeOutStopWatch = null;
        public bool BenchmarkSignalTimedout = false;
        protected bool BenchmarkSignalFinnished;
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
        public BenchmarkProcessStatus BenchmarkProcessStatus { get; protected set; }
        protected string BenchmarkProcessPath { get; private set; }
        protected Process BenchmarkHandle { get; private set; }
        protected Exception BenchmarkException = null;
        protected int BenchmarkTimeInSeconds;

        
        protected bool _isEthMinerExit = false;
        protected AlgorithmType[] _supportedMinerAlgorithms;

        // TODO maybe set for individual miner cooldown/retries logic variables
        // this replaces MinerAPIGraceSeconds(AMD)
        private const int _MIN_CooldownTimeInMilliseconds = 5 * 1000; // 5 seconds
        //private const int _MIN_CooldownTimeInMilliseconds = 1000; // TESTING

        //private const int _MAX_CooldownTimeInMilliseconds = 60 * 1000; // 1 minute max, whole waiting time 75seconds
        private readonly int _MAX_CooldownTimeInMilliseconds; // = GET_MAX_CooldownTimeInMilliseconds();
        protected abstract int GET_MAX_CooldownTimeInMilliseconds();
        private Timer _cooldownCheckTimer;
        protected MinerAPIReadStatus _currentMinerReadStatus { get; set; }
        private int _currentCooldownTimeInSeconds = _MIN_CooldownTimeInMilliseconds;
        private int _currentCooldownTimeInSecondsLeft = _MIN_CooldownTimeInMilliseconds;
        private const int IS_COOLDOWN_CHECK_TIMER_ALIVE_CAP = 15;

        private bool isEnded = false;

        public Miner(DeviceType deviceType, string minerDeviceName)
        {
            MINER_ID = MINER_ID_COUNT++;
            CDevs = new List<ComputeDevice>();
            DeviceType = deviceType;
            MinerDeviceName = minerDeviceName;

            //WorkingDirectory = @"bin\dlls";
            WorkingDirectory = "";

            CurrentAlgorithmType = AlgorithmType.NONE;
            CurrentRate = 0;
            IsRunning = false;
            PreviousTotalMH = 0.0;

            LastCommandLine = "";

            InitSupportedMinerAlgorithms();

            APIPort = MinersApiPortsManager.Instance.GetAvaliablePort();
            IsAPIReadException = false;
            _MAX_CooldownTimeInMilliseconds = GET_MAX_CooldownTimeInMilliseconds();
            // 
            Helpers.ConsolePrint(MinerTAG(), "NEW MINER CREATED");
        }

        ~Miner() {
            // free the port
            MinersApiPortsManager.Instance.RemovePort(APIPort);
            Helpers.ConsolePrint(MinerTAG(), "MINER DESTROYED");
        }

        virtual public void SetCDevs(string[] deviceUUIDs) {
            foreach (var uuid in deviceUUIDs) {
                CDevs.Add(ComputeDevice.GetDeviceWithUUID(uuid));
            }
            // sort devices by id
            CDevs.Sort((a, b) =>  a.ID - b.ID);
        }

        // TAG for identifying miner
        public string MinerTAG() {
            if (_minetTag == null) {
                const string MASK = "{0}-MINER_ID({1})-DEVICE_IDs({2})";
                // no devices set
                if (CDevs.Count == 0) {
                    return String.Format(MASK, MinerDeviceName, MINER_ID, "NOT_SET");
                }
                // contains ids
                List<int> ids = new List<int>();
                foreach (var cdevs in CDevs) ids.Add(cdevs.ID);
                _minetTag = String.Format(MASK, MinerDeviceName, MINER_ID, string.Join(",", ids));
            }
            return _minetTag;
        }

        private string ProcessTag(MinerPID_Data pidData) {
            return String.Format("[pid({0})|bin({1})]", pidData.PID, pidData.minerBinPath);
        }

        public string ProcessTag() {
            if (_currentPidData == null) {
                return "PidData is NULL";
            }
            return ProcessTag(_currentPidData);
        }

        public bool IsSupportedMinerAlgorithms(AlgorithmType algorithmType) {
            foreach (var supportedType in _supportedMinerAlgorithms) {
                if (supportedType == algorithmType) return true;
            }
            return false;
        }
        
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

        public void KillAllUsedMinerProcesses() {
            List<MinerPID_Data> toRemovePidData = new List<MinerPID_Data>();
            Helpers.ConsolePrint(MinerTAG(), "Trying to kill all miner processes for this instance:");
            foreach (var PidData in _allPidData) {
                try {
                    Process process = Process.GetProcessById(PidData.PID);
                    if (process != null && PidData.minerBinPath.Contains(process.ProcessName)) {
                        Helpers.ConsolePrint(MinerTAG(), String.Format("Trying to kill {0}", ProcessTag(PidData)));
                        try { process.Kill(); } catch (Exception e) {
                            Helpers.ConsolePrint(MinerTAG(), String.Format("Exception killing {0}, exMsg {1}", ProcessTag(PidData), e.Message));
                        }
                    }

                } catch (Exception e) {
                    toRemovePidData.Add(PidData);
                    Helpers.ConsolePrint(MinerTAG(), String.Format("Nothing to kill {0}, exMsg {1}", ProcessTag(PidData), e.Message));
                }
            }
            _allPidData.RemoveAll( x => toRemovePidData.Contains(x));
        }

        abstract public void Start(Algorithm miningAlgorithm, string url, string username);


        abstract protected void _Stop(MinerStopType willswitch);
        virtual public void Stop(MinerStopType willswitch = MinerStopType.SWITCH, bool needsRestart = false)
        {
            if (_cooldownCheckTimer != null) _cooldownCheckTimer.Stop();
            if (!needsRestart) {
                _Stop(willswitch);
                PreviousTotalMH = 0.0;
                IsRunning = false;
            } else {
                //_currentMinerReadStatus = MinerAPIReadStatus.RESTART;
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Manually closed, will restart");
                Restart();
            }
        }

        public void End() {
            isEnded = true;
            Stop(MinerStopType.FORCE_END);
            CurrentAlgorithmType = AlgorithmType.NONE;
            CurrentRate = 0;
        }

        protected void ChangeToNextAvaliablePort() {
            // change to new port
            var oldApiPort = APIPort;
            var newApiPort = MinersApiPortsManager.Instance.GetAvaliablePort();
            // check if update last command port
            if (UpdateBindPortCommand(oldApiPort, newApiPort)) {
                Helpers.ConsolePrint(MinerTAG(), String.Format("Changing miner port from {0} to {1}",
                    oldApiPort.ToString(),
                    newApiPort.ToString()));
                // free old set new
                MinersApiPortsManager.Instance.RemovePort(oldApiPort);
                APIPort = newApiPort;
            } else { // release new
                MinersApiPortsManager.Instance.RemovePort(newApiPort);
            }
        }

        protected void Stop_cpu_ccminer_sgminer(MinerStopType willswitch) {
            if (IsRunning) {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Shutting down miner");
            }
            if (willswitch != MinerStopType.FORCE_END) ChangeToNextAvaliablePort();
            if (ProcessHandle != null) {
                try { ProcessHandle.Kill(); } catch { }
                ProcessHandle.Close();
                ProcessHandle = null;

                // sgminer needs to be removed and kill by PID
                if (DeviceType == DeviceType.AMD) KillAllUsedMinerProcesses();
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
                return 5 * 60 + 120; // 5 minutes plus two minutes
            }
            if (CurrentBenchmarkAlgorithmType == AlgorithmType.CryptoNight) {
                return 5 * 60 + 120; // 5 minutes plus two minutes
            }
            return timeInSeconds + 120; // wait time plus two minutes
        }

        abstract protected string BenchmarkCreateCommandLine(Algorithm algorithm, int time);

        // The benchmark config and algorithm must guarantee that they are compatible with miner
        // we guarantee algorithm is supported
        // we will not have empty benchmark configs, all benchmark configs will have device list
        virtual public void BenchmarkStart(ComputeDevice benchmarkDevice, Algorithm algorithm, int time, IBenchmarkComunicator benchmarkComunicator) {
            // for extra launch parameters parsing purposes
            benchmarkDevice.MostProfitableAlgorithm = algorithm;

            BenchmarkComunicator = benchmarkComunicator;
            BenchmarkAlgorithm = algorithm;
            BenchmarkTimeInSeconds = time;
            BenchmarkSignalFinnished = true;
            // check and kill 
            BenchmarkHandle = null;
            OnBenchmarkCompleteCalled = false;
            BenchmarkTimeOutStopWatch = null;

            string CommandLine = BenchmarkCreateCommandLine(algorithm, time);

            Thread BenchmarkThread = new Thread(BenchmarkThreadRoutine);
            BenchmarkThread.Start(CommandLine);
        }

        virtual protected Process BenchmarkStartProcess(string CommandLine) {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Helpers.ConsolePrint(MinerTAG(), "Starting benchmark: " + CommandLine);

            Process BenchmarkHandle = new Process();
            
            BenchmarkHandle.StartInfo.FileName = GetOptimizedMinerPath(BenchmarkAlgorithm.NiceHashID);

            // TODO sgminer quickfix

            if (this is sgminer) {
                BenchmarkProcessPath = "cmd / " + BenchmarkHandle.StartInfo.FileName;
                BenchmarkHandle.StartInfo.FileName = "cmd";
            } else {
                BenchmarkProcessPath = BenchmarkHandle.StartInfo.FileName;
                Helpers.ConsolePrint(MinerTAG(), "Using miner: " + BenchmarkHandle.StartInfo.FileName);
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

        protected double BenchmarkParseLine_cpu_ccminer_extra(string outdata) {
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

                return spd;
            }
            return 0.0d;
        }

        // killing proccesses can take time
        virtual public void EndBenchmarkProcces() {
            if (BenchmarkHandle != null && BenchmarkProcessStatus != BenchmarkProcessStatus.Killing && BenchmarkProcessStatus != BenchmarkProcessStatus.DoneKilling) {
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

                Helpers.ConsolePrint(MinerTAG(), "Benchmark Exception: " + ex.Message);
                if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                    OnBenchmarkCompleteCalled = true;
                    BenchmarkComunicator.OnBenchmarkComplete(false, BenchmarkSignalTimedout ? International.GetText("Benchmark_Timedout") : International.GetText("Benchmark_Terminated"));
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

        virtual protected NiceHashProcess _Start()
        {
            PreviousTotalMH = 0.0;
            if (LastCommandLine.Length == 0) return null;

            NiceHashProcess P = new NiceHashProcess();

            if (WorkingDirectory.Length > 1)
            {
                P.StartInfo.WorkingDirectory = WorkingDirectory;
            }

            P.StartInfo.FileName = Path;
            P.ExitEvent = Miner_Exited;

            P.StartInfo.Arguments = LastCommandLine;
            P.StartInfo.CreateNoWindow = ConfigManager.Instance.GeneralConfig.HideMiningWindows;
            P.StartInfo.UseShellExecute = false;

            try
            {
                if (P.Start()) {
                    IsRunning = true;

                    _currentPidData = new MinerPID_Data();
                    _currentPidData.minerBinPath = P.StartInfo.FileName;
                    _currentPidData.PID = P.Id;
                    _allPidData.Add(_currentPidData);

                    Helpers.ConsolePrint(MinerTAG(), "Starting miner " + ProcessTag() + " " + LastCommandLine);

                    StartCoolDownTimerChecker();
                    isEnded = false;

                    return P;
                } else {
                    Helpers.ConsolePrint(MinerTAG(), "NOT STARTED " + ProcessTag() + " " + LastCommandLine);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " _Start: " + ex.Message);
                return null;
            }
        }

        protected void StartCoolDownTimerChecker() {
            Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Starting cooldown checker");
            if (_cooldownCheckTimer != null && _cooldownCheckTimer.Enabled) _cooldownCheckTimer.Stop();
            // cool down init
            _cooldownCheckTimer = new Timer() {
                Interval = _MIN_CooldownTimeInMilliseconds
            };
            _cooldownCheckTimer.Elapsed += MinerCoolingCheck_Tick;
            _cooldownCheckTimer.Start();
            _currentCooldownTimeInSeconds = _MIN_CooldownTimeInMilliseconds;
            _currentCooldownTimeInSecondsLeft = _currentCooldownTimeInSeconds;
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
        } 


        virtual protected void Miner_Exited() {
            //bool willswitch = _isEthMinerExit ? false : true;
            //Stop(willswitch, true);

            Stop(MinerStopType.END, true);
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
            if (LastCommandLine != null && LastCommandLine.Contains(oldApiBindStr)) {
                LastCommandLine = LastCommandLine.Replace(oldApiBindStr, newApiBindStr);
                return true;
            }
            return false;
        }

        private void Restart() {
            if (!isEnded) {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Restarting miner..");
                Stop(MinerStopType.END); // stop miner first
                System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                ProcessHandle = _Start(); // start with old command line
            }
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

                if (DeviceType == DeviceType.AMD /*MinerDeviceName.Equals("AMD_OpenCL")*/)
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
            catch (Exception ex)
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
            if (resp == null) {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " summary is null");
                _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                return null;
            }

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
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Could not read data from API bind port");
                _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                return null;
            }

            _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
            FillAlgorithm(aname, ref ad);
            // check if speed zero
            if (ad.Speed == 0) _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;

            return ad;
        }


        #region Cooldown/retry logic
        /// <summary>
        /// decrement time for half current half time, if less then min ammend
        /// </summary>
        private void CoolDown() {
            if (_currentCooldownTimeInSeconds > _MIN_CooldownTimeInMilliseconds) {
                _currentCooldownTimeInSeconds = _MIN_CooldownTimeInMilliseconds;
                Helpers.ConsolePrint(MinerTAG(), String.Format("{0} Reseting cool time = {1} ms", ProcessTag(), _MIN_CooldownTimeInMilliseconds.ToString()));
                _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            }
        }

        /// <summary>
        /// increment time for half current half time, if more then max set restart
        /// </summary>
        private void CoolUp() {
            _currentCooldownTimeInSeconds *= 2;
            Helpers.ConsolePrint(MinerTAG(), String.Format("{0} Cooling UP, cool time is {1} ms", ProcessTag(), _currentCooldownTimeInSeconds.ToString()));
            if (_currentCooldownTimeInSeconds > _MAX_CooldownTimeInMilliseconds) {
                _currentMinerReadStatus = MinerAPIReadStatus.RESTART;
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " MAX cool time exceeded. RESTARTING");
                Restart();
            }
        }

        private void MinerCoolingCheck_Tick(object sender, ElapsedEventArgs e) {
            _currentCooldownTimeInSecondsLeft -= (int)_cooldownCheckTimer.Interval;
            // if times up
            if (_currentCooldownTimeInSecondsLeft <= 0) {
                if (_currentMinerReadStatus == MinerAPIReadStatus.GOT_READ) {
                    CoolDown();
                } else if (_currentMinerReadStatus == MinerAPIReadStatus.READ_SPEED_ZERO) {
                    Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " READ SPEED ZERO, will cool up");
                    CoolUp();
                } else if (_currentMinerReadStatus == MinerAPIReadStatus.RESTART) {
                    Restart();
                } else {
                    CoolUp();
                }
                // set new times left from the CoolUp/Down change
                _currentCooldownTimeInSecondsLeft = _currentCooldownTimeInSeconds;
            }
        }

        #endregion //Cooldown/retry logic

    }
}
