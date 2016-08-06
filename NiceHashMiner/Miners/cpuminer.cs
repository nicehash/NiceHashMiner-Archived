using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

namespace NiceHashMiner
{
    public class cpuminer : Miner
    {
        private int Threads;
        private ulong AffinityMask;
        private string CPUMinerPath;
        private string HodlMinerPath;

        public cpuminer(int id, int threads, ulong affinity) : base(true)
        {
            MinerDeviceName = "CPU" + id.ToString();
            APIPort = 4040 + id;
            Threads = threads;
            AffinityMask = affinity;

            // this is the order we check and initialize if automatic
            CPUExtensionType[] detectOrder = new CPUExtensionType[] { CPUExtensionType.AVX2, CPUExtensionType.AVX, CPUExtensionType.SSE2 };

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
            // if our CPU is supported add it to devices
            // TODO if Miner and ComputeDevice decoupling redo this this is going to be at detecting CPUs
            if (isInitialized) {
                CDevs.Add(new ComputeDevice(0, MinerDeviceName, CPUID.GetCPUName().Trim(), this, true));
            }
        }

        // always query CPU
        protected override bool IsGroupQueryEnabled() {
            return true;
        }
        // querying done in ComputeDeviceQueryManager
        protected override void QueryCDevs() {
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
                switch(type) {
                    case CPUExtensionType.SSE2:
                        CPUMinerPath = MinerPaths.cpuminer_x64_SSE2;
                        HodlMinerPath = MinerPaths.hodlminer_core2;
                        break;
                    case CPUExtensionType.AVX:
                        CPUMinerPath = MinerPaths.cpuminer_x64_AVX;
                        HodlMinerPath = MinerPaths.hodlminer_corei7_avx;
                        break;
                    case CPUExtensionType.AVX2:
                        CPUMinerPath = MinerPaths.cpuminer_x64_AVX2;
                        HodlMinerPath = MinerPaths.hodlminer_core_avx2;
                        break;
                    default: // CPUExtensionType.Automatic
                        break;
                }
            }
            return isInitialized;
        }

        protected override string GetOptimizedMinerPath(AlgorithmType algorithmType) {
            if (algorithmType == AlgorithmType.Hodl) {
                return HodlMinerPath;
            }
            return CPUMinerPath;
        }

        /// <summary>
        /// HasExtensionSupport checks CPU extensions support, if type automatic just return false.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>False if type Automatic otherwise True if supported</returns>
        private bool HasExtensionSupport(CPUExtensionType type) {
            switch (type) {
                case CPUExtensionType.SSE2:
                    return CPUID.SupportsSSE2() == 1;
                case CPUExtensionType.AVX:
                    return CPUID.SupportsAVX() == 1;
                case CPUExtensionType.AVX2:
                    return CPUID.SupportsAVX2() == 1;
                default: // CPUExtensionType.Automatic
                    break;
            }
            return false;
        }

        public override string PrintSpeed(double spd)
        {
            // print in kH/s
            return (spd * 0.001).ToString("F3", CultureInfo.InvariantCulture) + " kH/s";
        }


        public override void Start(Algorithm miningAlgorithm, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running

            if (CDevs.Count == 0 || !CDevs[0].Enabled) return;

            //Algorithm miningAlgorithm = null;//GetMinerAlgorithm(algorithmType);
            if (miningAlgorithm == null) return;

            Path = GetOptimizedMinerPath(miningAlgorithm.NiceHashID);

            LastCommandLine = "--algo=" + miningAlgorithm.MinerName + 
                              " --url=" + url + 
                              " --userpass=" + username + ":" + GetPassword(miningAlgorithm) + 
                              " --threads=" + Threads.ToString() + 
                              " " + ExtraLaunchParameters + 
                              " " + miningAlgorithm.ExtraLaunchParameters;

            if (miningAlgorithm.NiceHashID != AlgorithmType.Hodl)
                LastCommandLine += " --api-bind=" + APIPort.ToString();

            ProcessHandle = _Start();
        }


        protected override NiceHashProcess _Start()
        {
            NiceHashProcess P = base._Start();

            if (AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, AffinityMask);

            return P;
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines
        // new decoupled benchmark, TODO fix the copy paste magic
        // TODO recheck this
        protected override string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time) {
            Path = GetOptimizedMinerPath(algorithm.NiceHashID);

            string ret = "--algo=" + algorithm.MinerName +
                         " --benchmark" +
                         " --threads=" + Threads.ToString() +
                         " " + benchmarkConfig.ExtraLaunchParameters +
                         " " + algorithm.ExtraLaunchParameters;

            if (algorithm.NiceHashID != AlgorithmType.Hodl)
                ret += " --time-limit " + time.ToString();

            return ret;
        }

        protected override Process BenchmarkStartProcess(string CommandLine) {
            Process BenchmarkHandle = base.BenchmarkStartProcess(CommandLine);

            if (AffinityMask != 0 && BenchmarkHandle != null)
                CPUID.AdjustAffinity(BenchmarkHandle.Id, AffinityMask);

            return BenchmarkHandle;
        }
        #endregion // Decoupled benchmarking routines
    }
}
