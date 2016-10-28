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
        //private int Threads;
        //private ulong AffinityMask;

        public cpuminer()
            : base(DeviceType.CPU, "CPU") {
        }

        public static CPUExtensionType GetMostOptimized() {
            // this is the order we check and initialize if automatic
            CPUExtensionType ret = CPUExtensionType.Automatic;
            CPUExtensionType[] detectOrder = new CPUExtensionType[] { 
                CPUExtensionType.AVX2_AES,
                CPUExtensionType.AVX2,
                CPUExtensionType.AVX_AES,
                CPUExtensionType.AVX,
                CPUExtensionType.AES,
                CPUExtensionType.SSE2,
            };

            if (ConfigManager.Instance.GeneralConfig.ForceCPUExtension == CPUExtensionType.Automatic) {
                for (int i = 0; i < detectOrder.Length; ++i) {
                    if (HasExtensionSupport(detectOrder[i])) {
                        return detectOrder[i];
                    }
                }
            } else if (HasExtensionSupport(ConfigManager.Instance.GeneralConfig.ForceCPUExtension)) {
                return ConfigManager.Instance.GeneralConfig.ForceCPUExtension;
            }

            return ret;
        }

        public static bool InitializeMinerPaths() {
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
            bool isInitialized = "" != GetOptimizedMinerPath(ConfigManager.Instance.GeneralConfig.ForceCPUExtension);
            // #2 if automatic or does not support then initialize in order
            if (isInitialized == false) {
                ConfigManager.Instance.GeneralConfig.ForceCPUExtension = CPUExtensionType.Automatic; // set to automatic if not supported
                for (int i = 0; i < detectOrder.Length; ++i) {
                    isInitialized = "" != GetOptimizedMinerPath(detectOrder[i]);
                    if (isInitialized) {
                        break; // stop if initialized
                    }
                }
            }
            return isInitialized;
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
        public static string GetOptimizedMinerPath(CPUExtensionType type) {
            // if type not automatic and has extension support set initialized
            if (HasExtensionSupport(type)) {
                switch (type) {
                    case CPUExtensionType.AVX2_AES:
                        return MinerPaths.cpuminer_opt_AVX2_AES;
                    case CPUExtensionType.AVX2:
                        return MinerPaths.cpuminer_opt_AVX2;
                    case CPUExtensionType.AVX_AES:
                        return MinerPaths.cpuminer_opt_AVX_AES;
                    case CPUExtensionType.AVX:
                        return MinerPaths.cpuminer_opt_AVX;
                    case CPUExtensionType.AES:
                        return MinerPaths.cpuminer_opt_AES;
                    case CPUExtensionType.SSE2:
                        return MinerPaths.cpuminer_opt_SSE2;
                    default: // CPUExtensionType.Automatic
                        break;
                }
            }
            return "";
        }

        public override string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename = "", bool isOptimized = false) {
            return GetOptimizedMinerPath(GetMostOptimized());
        }

        /// <summary>
        /// HasExtensionSupport checks CPU extensions support, if type automatic just return false.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>False if type Automatic otherwise True if supported</returns>
        public static bool HasExtensionSupport(CPUExtensionType type) {
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
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();

            if (CDevs[0].AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, CDevs[0].AffinityMask);

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

            if (CDevs[0].AffinityMask != 0 && BenchmarkHandle != null)
                CPUID.AdjustAffinity(BenchmarkHandle.Id, CDevs[0].AffinityMask);

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
