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
using NiceHashMiner.Miners.Grouping;

using Timer = System.Timers.Timer;
using System.Timers;
using NiceHashMiner.Net20_backport;
using System.IO;

namespace NiceHashMiner
{
    public class APIData
    {
        public AlgorithmType AlgorithmID;
        public AlgorithmType SecondaryAlgorithmID;
        public string AlgorithmName;
        public double Speed;
        public double SecondarySpeed;
        public APIData(AlgorithmType algorithmID, AlgorithmType secondaryAlgorithmID=AlgorithmType.NONE) {
            this.AlgorithmID = algorithmID;
            this.SecondaryAlgorithmID = secondaryAlgorithmID;
            this.AlgorithmName = AlgorithmNiceHashNames.GetName(DualAlgorithmID());
            this.Speed = 0.0;
            this.SecondarySpeed = 0.0;
        }
        public AlgorithmType DualAlgorithmID() {
            if (AlgorithmID == AlgorithmType.DaggerHashimoto) {
                switch (SecondaryAlgorithmID) {
                    case AlgorithmType.Decred:
                        return AlgorithmType.DaggerDecred;
                    case AlgorithmType.Lbry:
                        return AlgorithmType.DaggerLbry;
                    case AlgorithmType.Pascal:
                        return AlgorithmType.DaggerPascal;
                }
            }
            return AlgorithmID;
        }
    }

    // 
    public class MinerPID_Data {
        public string minerBinPath = null;
        public int PID = -1;
    }

    public abstract class Miner {


        // MINER_ID_COUNT used to identify miners creation
        protected static long MINER_ID_COUNT { get; private set; }


        public NHMConectionType ConectionType { get; protected set; }
        // used to identify miner instance
        protected readonly long MINER_ID;
        private string _minetTag = null;
        public string MinerDeviceName { get; set; }
        protected int APIPort { get; private set; }
        // if miner has no API bind port for reading curentlly only CryptoNight on ccminer
        public bool IsAPIReadException { get; protected set; }
        public bool IsNeverHideMiningWindow { get; protected set; }
        // mining algorithm stuff
        protected bool IsInit { get; private set; }
        protected MiningSetup MiningSetup { get; set; }
        // sgminer/zcash claymore workaround
        protected bool IsKillAllUsedMinerProcs { get; set; }
        public bool IsRunning { get; protected set; }
        protected string Path { get; private set; }
        protected string LastCommandLine { get; set; }
        // TODO check this 
        protected double PreviousTotalMH;
        // the defaults will be 
        protected string WorkingDirectory { get; private set; }
        protected string MinerExeName { get; private set; }
        protected NiceHashProcess ProcessHandle;
        private MinerPID_Data _currentPidData;
        private List<MinerPID_Data> _allPidData = new List<MinerPID_Data>();

        // Benchmark stuff
        public bool BenchmarkSignalQuit;
        public bool BenchmarkSignalHanged;
        Stopwatch BenchmarkTimeOutStopWatch = null;
        public bool BenchmarkSignalTimedout = false;
        protected bool BenchmarkSignalFinnished;
        protected IBenchmarkComunicator BenchmarkComunicator;
        protected bool OnBenchmarkCompleteCalled = false;
        protected Algorithm BenchmarkAlgorithm { get; set; }
        public BenchmarkProcessStatus BenchmarkProcessStatus { get; protected set; }
        protected string BenchmarkProcessPath { get; set; }
        protected Process BenchmarkHandle { get; set; }
        protected Exception BenchmarkException = null;
        protected int BenchmarkTimeInSeconds;

        string benchmarkLogPath = "";
        protected List<string> bench_lines;

        
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
        private bool NeedsRestart = false;

        private bool isEnded = false;

        public Miner(string minerDeviceName)
        {
            ConectionType = NHMConectionType.STRATUM_TCP;
            MiningSetup = new MiningSetup(null);
            IsInit = false;
            MINER_ID = MINER_ID_COUNT++;

            MinerDeviceName = minerDeviceName;

            WorkingDirectory = "";

            IsRunning = false;
            PreviousTotalMH = 0.0;

            LastCommandLine = "";

            IsAPIReadException = false;
            IsNeverHideMiningWindow = false;
            IsKillAllUsedMinerProcs = false;
            _MAX_CooldownTimeInMilliseconds = GET_MAX_CooldownTimeInMilliseconds();
            // 
            Helpers.ConsolePrint(MinerTAG(), "NEW MINER CREATED");
        }

        ~Miner() {
            // free the port
            MinersApiPortsManager.RemovePort(APIPort);
            Helpers.ConsolePrint(MinerTAG(), "MINER DESTROYED");
        }

        protected void SetWorkingDirAndProgName(string fullPath) {
            this.WorkingDirectory = "";
            this.Path = fullPath;
            int lastIndex = fullPath.LastIndexOf("\\") + 1;
            if(lastIndex > 0) {
                this.WorkingDirectory = fullPath.Substring(0, lastIndex);
                this.MinerExeName = fullPath.Substring(lastIndex);
            }
        }

        private void SetAPIPort() {
            if (IsInit) {
                var minerBase = MiningSetup.MiningPairs[0].Algorithm.MinerBaseType;
                var algoType = MiningSetup.MiningPairs[0].Algorithm.NiceHashID;
                var path = MiningSetup.MinerPath;
                var reservedPorts = MinersSettingsManager.GetPortsListFor(minerBase, path, algoType);
                APIPort = -1; // not set
                foreach (var reservedPort in reservedPorts) {
                    if (MinersApiPortsManager.IsPortAvaliable(reservedPort)) {
                        APIPort = reservedPort;
                        break;
                    }
                }
                if (APIPort == -1) {
                    APIPort = MinersApiPortsManager.GetAvaliablePort();
                }
            }
        }

        virtual public void InitMiningSetup(MiningSetup miningSetup) {
            MiningSetup = miningSetup;
            IsInit = MiningSetup.IsInit;
            SetAPIPort();
            SetWorkingDirAndProgName(MiningSetup.MinerPath);
        }

        public void InitBenchmarkSetup(MiningPair benchmarkPair) {
            InitMiningSetup(new MiningSetup(new List<MiningPair>() { benchmarkPair }));
            BenchmarkAlgorithm = benchmarkPair.Algorithm;
        }

        // TAG for identifying miner
        public string MinerTAG() {
            if (_minetTag == null) {
                const string MASK = "{0}-MINER_ID({1})-DEVICE_IDs({2})";
                // no devices set
                if (!IsInit) {
                    return String.Format(MASK, MinerDeviceName, MINER_ID, "NOT_SET");
                }
                // contains ids
                List<string> ids = new List<string>();
                foreach (var cdevs in MiningSetup.MiningPairs) ids.Add(cdevs.Device.ID.ToString());
                _minetTag = String.Format(MASK, MinerDeviceName, MINER_ID, StringHelper.Join(",", ids));
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

        public void KillAllUsedMinerProcesses() {
            List<MinerPID_Data> toRemovePidData = new List<MinerPID_Data>();
            Helpers.ConsolePrint(MinerTAG(), "Trying to kill all miner processes for this instance:");
            foreach (var PidData in _allPidData) {
                try {
                    Process process = Process.GetProcessById(PidData.PID);
                    if (process != null && PidData.minerBinPath.Contains(process.ProcessName)) {
                        Helpers.ConsolePrint(MinerTAG(), String.Format("Trying to kill {0}", ProcessTag(PidData)));
                        try {
                            process.Kill();
                            process.Close();
                            process.WaitForExit(1000 * 60 * 1);
                        } catch (Exception e) {
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

        abstract public void Start(string url, string btcAdress, string worker);

        protected string GetUsername(string btcAdress, string worker) {
            if (worker.Length > 0) {
                return btcAdress + "." + worker;
            }
            return btcAdress;
        }

        abstract protected void _Stop(MinerStopType willswitch);
        virtual public void Stop(MinerStopType willswitch = MinerStopType.SWITCH)
        {
            if (_cooldownCheckTimer != null) _cooldownCheckTimer.Stop();
            _Stop(willswitch);
            PreviousTotalMH = 0.0;
            IsRunning = false;
        }

        public void End() {
            isEnded = true;
            Stop(MinerStopType.FORCE_END);
        }

        protected void Stop_cpu_ccminer_sgminer_nheqminer(MinerStopType willswitch) {
            if (IsRunning) {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Shutting down miner");
            }
            if (ProcessHandle != null) {
                try { ProcessHandle.Kill(); } catch { }
                //try { ProcessHandle.SendCtrlC((uint)Process.GetCurrentProcess().Id); } catch { }
                ProcessHandle.Close();
                ProcessHandle = null;

                // sgminer needs to be removed and kill by PID
                if (IsKillAllUsedMinerProcs) KillAllUsedMinerProcesses();
            }
        }

        virtual protected string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            List<string> ids = new List<string>();
            foreach (var mPair in MiningSetup.MiningPairs) {
                ids.Add(mPair.Device.ID.ToString());
            }
            deviceStringCommand += StringHelper.Join(",", ids);

            return deviceStringCommand;
        }

        #region BENCHMARK DE-COUPLED Decoupled benchmarking routines

        public int BenchmarkTimeoutInSeconds(int timeInSeconds) {
            if (BenchmarkAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto) {
                return 5 * 60 + 120; // 5 minutes plus two minutes
            }
            if (BenchmarkAlgorithm.NiceHashID == AlgorithmType.CryptoNight) {
                return 5 * 60 + 120; // 5 minutes plus two minutes
            }
            return timeInSeconds + 120; // wait time plus two minutes
        }

        // TODO remove algorithm
        abstract protected string BenchmarkCreateCommandLine(Algorithm algorithm, int time);

        // The benchmark config and algorithm must guarantee that they are compatible with miner
        // we guarantee algorithm is supported
        // we will not have empty benchmark configs, all benchmark configs will have device list
        virtual public void BenchmarkStart(int time, IBenchmarkComunicator benchmarkComunicator) {
            BenchmarkComunicator = benchmarkComunicator;
            BenchmarkTimeInSeconds = time;
            BenchmarkSignalFinnished = true;
            // check and kill 
            BenchmarkHandle = null;
            OnBenchmarkCompleteCalled = false;
            BenchmarkTimeOutStopWatch = null;


            try {
                if (!Directory.Exists("logs")) {
                    Directory.CreateDirectory("logs");
                }
            } catch { }
            bench_lines = new List<string>();
            benchmarkLogPath = String.Format("{0}Log_{1}_{2}", Logger._logPath, MiningSetup.MiningPairs[0].Device.UUID, MiningSetup.MiningPairs[0].Algorithm.AlgorithmStringID);

            string CommandLine = BenchmarkCreateCommandLine(BenchmarkAlgorithm, time);

            Thread BenchmarkThread = new Thread(BenchmarkThreadRoutine);
            BenchmarkThread.Start(CommandLine);
        }

        virtual protected Process BenchmarkStartProcess(string CommandLine) {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Helpers.ConsolePrint(MinerTAG(), "Starting benchmark: " + CommandLine);

            Process BenchmarkHandle = new Process();

            BenchmarkHandle.StartInfo.FileName = MiningSetup.MinerPath;

            // sgminer quickfix
            if (this is sgminer) {
                BenchmarkProcessPath = "cmd / " + BenchmarkHandle.StartInfo.FileName;
                BenchmarkHandle.StartInfo.FileName = "cmd";
            } else {
                BenchmarkProcessPath = BenchmarkHandle.StartInfo.FileName;
                Helpers.ConsolePrint(MinerTAG(), "Using miner: " + BenchmarkHandle.StartInfo.FileName);
                BenchmarkHandle.StartInfo.WorkingDirectory = WorkingDirectory;
            }
            // set sys variables
            if (MinersSettingsManager.MinerSystemVariables.ContainsKey(Path)) {
                foreach (var kvp in MinersSettingsManager.MinerSystemVariables[Path]) {
                    string envName = kvp.Key;
                    string envValue = kvp.Value;
                    BenchmarkHandle.StartInfo.EnvironmentVariables[envName] = envValue;
                }
            }

            BenchmarkHandle.StartInfo.Arguments = (string)CommandLine;
            BenchmarkHandle.StartInfo.UseShellExecute = false;
            BenchmarkHandle.StartInfo.RedirectStandardError = true;
            BenchmarkHandle.StartInfo.RedirectStandardOutput = true;
            BenchmarkHandle.StartInfo.CreateNoWindow = true;
            BenchmarkHandle.OutputDataReceived += BenchmarkOutputErrorDataReceived;
            BenchmarkHandle.ErrorDataReceived += BenchmarkOutputErrorDataReceived;
            BenchmarkHandle.Exited += BenchmarkHandle_Exited;

            if (!BenchmarkHandle.Start()) return null;

            _currentPidData = new MinerPID_Data();
            _currentPidData.minerBinPath = BenchmarkHandle.StartInfo.FileName;
            _currentPidData.PID = BenchmarkHandle.Id;
            _allPidData.Add(_currentPidData);

            return BenchmarkHandle;
        }

        void BenchmarkHandle_Exited(object sender, EventArgs e) {
            BenchmarkSignalFinnished = true;
        }

        private void BenchmarkOutputErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (BenchmarkTimeOutStopWatch == null) {
                BenchmarkTimeOutStopWatch = new Stopwatch();
                BenchmarkTimeOutStopWatch.Start();
            } else if (BenchmarkTimeOutStopWatch.Elapsed.TotalSeconds > BenchmarkTimeoutInSeconds(BenchmarkTimeInSeconds)) {
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
            //Helpers.ConsolePrint("BENCHMARK" + benchmarkLogPath, outdata);
            bench_lines.Add(outdata);
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

        public void InvokeBenchmarkSignalQuit() {
            KillAllUsedMinerProcesses();
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
                if (b < 0) {
                    int stub;
                    for (int _i = hashspeed.Length - 1; _i >= 0; --_i ) {
                        if (Int32.TryParse(hashspeed[_i].ToString(), out stub)) {
                            b = _i;
                            break;
                        }
                    }
                }
                if (b >= 0) {
                    string speedStr = hashspeed.Substring(0, b);
                    double spd = Helpers.ParseDouble(speedStr);
                    if (hashspeed.Contains("kH/s"))
                        spd *= 1000;
                    else if (hashspeed.Contains("MH/s"))
                        spd *= 1000000;
                    else if (hashspeed.Contains("GH/s"))
                        spd *= 1000000000;

                    return spd;
                }
            }
            return 0.0d;
        }

        // killing proccesses can take time
        virtual public void EndBenchmarkProcces() {
            if (BenchmarkHandle != null && BenchmarkProcessStatus != BenchmarkProcessStatus.Killing && BenchmarkProcessStatus != BenchmarkProcessStatus.DoneKilling) {
                BenchmarkProcessStatus = BenchmarkProcessStatus.Killing;
                try {
                    Helpers.ConsolePrint("BENCHMARK", String.Format("Trying to kill benchmark process {0} algorithm {1}", BenchmarkProcessPath, BenchmarkAlgorithm.AlgorithmName));
                    BenchmarkHandle.Kill();
                    BenchmarkHandle.Close();
                    KillAllUsedMinerProcesses();
                } catch { }
                finally {
                    BenchmarkProcessStatus = BenchmarkProcessStatus.DoneKilling;
                    Helpers.ConsolePrint("BENCHMARK", String.Format("Benchmark process {0} algorithm {1} KILLED", BenchmarkProcessPath, BenchmarkAlgorithm.AlgorithmName));
                    //BenchmarkHandle = null;
                }
            }
        }


        virtual protected void BenchmarkThreadRoutineStartSettup() {
            BenchmarkHandle.BeginErrorReadLine();
            BenchmarkHandle.BeginOutputReadLine();
        }

        protected void BenchmarkThreadRoutineCatch(Exception ex) {
            BenchmarkAlgorithm.BenchmarkSpeed = 0;

            Helpers.ConsolePrint(MinerTAG(), "Benchmark Exception: " + ex.Message);
            if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                OnBenchmarkCompleteCalled = true;
                BenchmarkComunicator.OnBenchmarkComplete(false, GetFinalBenchmarkString());
            }
        }

        protected virtual string GetFinalBenchmarkString() {
            return BenchmarkSignalTimedout ? International.GetText("Benchmark_Timedout") : International.GetText("Benchmark_Terminated");
        }

        protected void BenchmarkThreadRoutineFinish() {
            BenchmarkProcessStatus status = BenchmarkProcessStatus.Finished;

            if (BenchmarkAlgorithm.BenchmarkSpeed > 0) {
                status = BenchmarkProcessStatus.Success;
            }

            using (StreamWriter sw = File.AppendText(benchmarkLogPath)) {
                foreach (var line in bench_lines) {
                    sw.WriteLine(line);
                }
            }
            BenchmarkProcessStatus = status;
            Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + Helpers.FormatDualSpeedOutput(BenchmarkAlgorithm.BenchmarkSpeed, BenchmarkAlgorithm.SecondaryBenchmarkSpeed));
            Helpers.ConsolePrint("BENCHMARK", "Benchmark ends");
            if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                OnBenchmarkCompleteCalled = true;
                bool isOK = BenchmarkProcessStatus.Success == status;
                string msg = GetFinalBenchmarkString();
                BenchmarkComunicator.OnBenchmarkComplete(isOK, isOK ? "" : msg);
            }
        }



        virtual protected void BenchmarkThreadRoutine(object CommandLine) {
            Thread.Sleep(ConfigManager.GeneralConfig.MinerRestartDelayMS);

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
                BenchmarkThreadRoutineCatch(ex);
            } finally {
                BenchmarkThreadRoutineFinish();
            }
        }

        abstract protected bool BenchmarkParseLine(string outdata);

        #endregion //BENCHMARK DE-COUPLED Decoupled benchmarking routines

        virtual protected NiceHashProcess _Start()
        {
            // never start when ended
            if (isEnded) {
                return null;
            }

            PreviousTotalMH = 0.0;
            if (LastCommandLine.Length == 0) return null;

            NiceHashProcess P = new NiceHashProcess();

            if (WorkingDirectory.Length > 1)
            {
                P.StartInfo.WorkingDirectory = WorkingDirectory;
            }
            if (MinersSettingsManager.MinerSystemVariables.ContainsKey(Path)) {
                foreach (var kvp in MinersSettingsManager.MinerSystemVariables[Path]) {
                    string envName = kvp.Key;
                    string envValue = kvp.Value;
                    P.StartInfo.EnvironmentVariables[envName] = envValue;
                }
            }

            P.StartInfo.FileName = Path;
            P.ExitEvent = Miner_Exited;

            P.StartInfo.Arguments = LastCommandLine;
            if (IsNeverHideMiningWindow) {
                P.StartInfo.CreateNoWindow = false;
                if (ConfigManager.GeneralConfig.HideMiningWindows) {
                    P.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    P.StartInfo.UseShellExecute = true;
                }
            } else {
                P.StartInfo.CreateNoWindow = ConfigManager.GeneralConfig.HideMiningWindows;
            }
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
            ScheduleRestart(5000);
        }

        protected void ScheduleRestart(int ms) {
            var RestartInMS = ConfigManager.GeneralConfig.MinerRestartDelayMS > ms ?
                ConfigManager.GeneralConfig.MinerRestartDelayMS : ms;
            Helpers.ConsolePrint(MinerTAG(), ProcessTag() + String.Format(" Miner_Exited Will restart in {0} ms", RestartInMS));
            _currentMinerReadStatus = MinerAPIReadStatus.RESTART;
            NeedsRestart = true;
            _currentCooldownTimeInSecondsLeft = RestartInMS;
        }

        protected void Restart() {
            if (!isEnded) {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Restarting miner..");
                Stop(MinerStopType.END); // stop miner first
                System.Threading.Thread.Sleep(ConfigManager.GeneralConfig.MinerRestartDelayMS);
                ProcessHandle = _Start(); // start with old command line
            }
        }

        protected string GetAPIData(int port, string DataToSend, bool exitHack = false)
        {
            string ResponseFromServer = null;
            try
            {
                TcpClient tcpc = new TcpClient("127.0.0.1", port);

                byte[] BytesToSend = ASCIIEncoding.ASCII.GetBytes(DataToSend);
                tcpc.Client.Send(BytesToSend);

                byte[] IncomingBuffer = new byte[5000];
                int prevOffset = -1;
                int offset = 0;
                bool fin = false;

                while (!fin && tcpc.Client.Connected)
                {
                    int r = tcpc.Client.Receive(IncomingBuffer, offset, 5000 - offset, SocketFlags.None);
                    for (int i = offset; i < offset + r; i++)
                    {
                        if (IncomingBuffer[i] == 0x7C || IncomingBuffer[i] == 0x00) {
                            fin = true;
                            break;
                        }
                        // Not working
                        //if (IncomingBuffer[i] == 0x5d || IncomingBuffer[i] == 0x5e) {
                        //    fin = true;
                        //    break;
                        //}
                    }
                    offset += r;
                    if (exitHack) {
                        if (prevOffset == offset) {
                            fin = true;
                            break;
                        }
                        prevOffset = offset;
                    }
                }

                tcpc.Close();

                if (offset > 0)
                    ResponseFromServer = ASCIIEncoding.ASCII.GetString(IncomingBuffer);
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " GetAPIData reason: " + ex.Message);
                return null;
            }

            return ResponseFromServer;
        }

        public abstract APIData GetSummary();

        protected string GetHttpRequestNHMAgentStrin(string cmd) {
            return "GET /" + cmd + " HTTP/1.1\r\n" +
                    "Host: 127.0.0.1\r\n" +
                    "User-Agent: NiceHashMiner/" + Application.ProductVersion + "\r\n" +
                    "\r\n";
        }

        protected APIData GetSummaryCPU_CCMINER() {
            string resp;
            // TODO aname
            string aname = null;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            string DataToSend = GetHttpRequestNHMAgentStrin("summary");

            resp = GetAPIData(APIPort, DataToSend);
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
            if (isEnded) {
                End();
                return;
            }
            _currentCooldownTimeInSecondsLeft -= (int)_cooldownCheckTimer.Interval;
            // if times up
            if (_currentCooldownTimeInSecondsLeft <= 0) {
                if (NeedsRestart) {
                    NeedsRestart = false;
                    Restart();
                } else if (_currentMinerReadStatus == MinerAPIReadStatus.GOT_READ) {
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
