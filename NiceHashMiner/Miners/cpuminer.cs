using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners {
    public class cpuminer : Miner {
        private int Threads;
        private ulong AffinityMask;
        private string CPUMinerPath;

        public cpuminer(int id, int threads, ulong affinity)
            : base(DeviceType.CPU, "CPU" + id.ToString()) {
            Threads = threads;
            AffinityMask = affinity;

            bool isInitialized = InitializeMinerPaths();
            // if our CPU is supported add it to devices
            // TODO if Miner and ComputeDevice decoupling redo this this is going to be at detecting CPUs
            if (isInitialized) {
                CDevs.Add(new ComputeDevice(0, MinerDeviceName, CPUID.GetCPUName().Trim(), Threads, true));
            }
        }

        private bool InitializeMinerPaths() {
            // this is the order we check and initialize if automatic
            CPUExtensionType[] detectOrder = new CPUExtensionType[] { 
                CPUExtensionType.AVX2_AES,
                CPUExtensionType.AVX2,
                CPUExtensionType.AVX_AES,
                CPUExtensionType.AVX,
                CPUExtensionType.AES,
                CPUExtensionType.SSE2,
            };

            // #1 try to initialize with Configured extension
            bool isInitialized = InitializeMinerPaths(ConfigManager.Instance.GeneralConfig.ForceCPUExtension);
            // #2 if automatic or does not support then initialize in order
            if (isInitialized == false) {
                ConfigManager.Instance.GeneralConfig.ForceCPUExtension = CPUExtensionType.Automatic; // set to automatic if not supported
                for (int i = 0; i < detectOrder.Length; ++i) {
                    isInitialized = InitializeMinerPaths(detectOrder[i]);
                    if (isInitialized) {
                        break; // stop if initialized
                    }
                }
            }
            return isInitialized;
        }

        public override void SetCDevs(string[] deviceUUIDs) {
            // DO NOTHING, CPU MINER DEVICES ARE SET ONLY AT THE BEGINING
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000; // 1 minute max, whole waiting time 75seconds
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.CPU);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        /// <summary>
        /// InitializeMinerPaths initializes cpuminer paths based on CPUExtensionType.
        /// Make sure to check if extensions enabled. Currently using CPUID fo checking
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Returns False if CPUExtensionType.Automatic, otherwise True and initializes paths</returns>
        private bool InitializeMinerPaths(CPUExtensionType type) {
            bool isInitialized = false;
            // if type not automatic and has extension support set initialized
            if (HasExtensionSupport(type)) {
                isInitialized = true;
                switch (type) {
                    case CPUExtensionType.AVX2_AES:
                        CPUMinerPath = MinerPaths.cpuminer_opt_AVX2_AES;
                        break;
                    case CPUExtensionType.AVX2:
                        CPUMinerPath = MinerPaths.cpuminer_opt_AVX2;
                        break;
                    case CPUExtensionType.AVX_AES:
                        CPUMinerPath = MinerPaths.cpuminer_opt_AVX_AES;
                        break;
                    case CPUExtensionType.AVX:
                        CPUMinerPath = MinerPaths.cpuminer_opt_AVX;
                        break;
                    case CPUExtensionType.AES:
                        CPUMinerPath = MinerPaths.cpuminer_opt_AES;
                        break;
                    case CPUExtensionType.SSE2:
                        CPUMinerPath = MinerPaths.cpuminer_opt_SSE2;
                        break;
                    default: // CPUExtensionType.Automatic
                        break;
                }
            }
            return isInitialized;
        }

        public override string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename = "", bool isOptimized = false) {
            return CPUMinerPath;
        }

        /// <summary>
        /// HasExtensionSupport checks CPU extensions support, if type automatic just return false.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>False if type Automatic otherwise True if supported</returns>
        private bool HasExtensionSupport(CPUExtensionType type) {
            switch (type) {
                case CPUExtensionType.AVX2_AES: return (CPUID.SupportsAVX2() == 1) && (CPUID.SupportsAES() == 1);
                case CPUExtensionType.AVX2: return CPUID.SupportsAVX2() == 1;
                case CPUExtensionType.AVX_AES: return (CPUID.SupportsAVX() == 1) && (CPUID.SupportsAES() == 1);
                case CPUExtensionType.AVX: return CPUID.SupportsAVX() == 1;
                case CPUExtensionType.AES: return CPUID.SupportsAES() == 1;
                case CPUExtensionType.SSE2: return CPUID.SupportsSSE2() == 1;
                default: // CPUExtensionType.Automatic
                    break;
            }
            return false;
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            // to set miner paths
            InitializeMinerPaths();

            CurrentMiningAlgorithm = miningAlgorithm;
            if (ProcessHandle != null) return; // ignore, already running

            if (CDevs.Count == 0 || !CDevs[0].Enabled) return;

            if (miningAlgorithm == null) return;

            Path = GetOptimizedMinerPath(miningAlgorithm.NiceHashID);

            LastCommandLine = "--algo=" + miningAlgorithm.MinerName +
                              " --url=" + url +
                              " --userpass=" + username + ":" + Algorithm.PasswordDefault +
                              ExtraLaunchParametersParser.ParseForCDevs(
                                                                CDevs,
                                                                CurrentMiningAlgorithm.NiceHashID,
                                                                DeviceType.CPU) +
                              " --api-bind=" + APIPort.ToString();

            ProcessHandle = _Start();
        }

        public override APIData GetSummary() {
            return GetSummaryCPU_CCMINER();
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer(willswitch);
        }

        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();

            if (AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, AffinityMask);

            return P;
        }

        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            return UpdateBindPortCommand_ccminer_cpuminer(oldPort, newPort);
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // to set miner paths
            InitializeMinerPaths();

            Path = GetOptimizedMinerPath(algorithm.NiceHashID);

            return "--algo=" + algorithm.MinerName +
                         " --benchmark" +
                         ExtraLaunchParametersParser.ParseForCDevs(
                                                                CDevs,
                                                                algorithm.NiceHashID,
                                                                DeviceType.CPU) +
                         " --time-limit " + time.ToString();
        }

        protected override Process BenchmarkStartProcess(string CommandLine) {
            Process BenchmarkHandle = base.BenchmarkStartProcess(CommandLine);

            if (AffinityMask != 0 && BenchmarkHandle != null)
                CPUID.AdjustAffinity(BenchmarkHandle.Id, AffinityMask);

            return BenchmarkHandle;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            double lastSpeed = 0;
            if (double.TryParse(outdata, out lastSpeed)) {
                BenchmarkAlgorithm.BenchmarkSpeed = lastSpeed;
                return true;
            }
            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        #endregion // Decoupled benchmarking routines
    }
}
