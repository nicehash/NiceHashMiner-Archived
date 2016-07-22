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
    class cpuminer : Miner
    {
        private int Threads;
        private ulong AffinityMask;
        private string CPUMinerPath;
        private string HodlMinerPath;

        public cpuminer(int id, int threads, ulong affinity)
        {
            MinerDeviceName = "CPU" + id.ToString();
            APIPort = 4040 + id;
            Threads = threads;
            AffinityMask = affinity;

            // ReadOnlyDictionary that would be great [ not avaliable in .NET 2.0, Available since 4.5 ]
            SupportedAlgorithms = new Dictionary<AlgorithmType, Algorithm>() {
                { AlgorithmType.Lyra2RE, new Algorithm(AlgorithmType.Lyra2RE, "lyra2") },
                { AlgorithmType.Axiom, new Algorithm(AlgorithmType.Axiom, "axiom") },
                { AlgorithmType.ScryptJaneNf16, new Algorithm(AlgorithmType.ScryptJaneNf16, "scryptjane:16") },
                { AlgorithmType.Hodl, new Algorithm(AlgorithmType.Hodl, "hodl") { ExtraLaunchParameters = "--extranonce-subscribe"} }
            };

            // this is the order we check and initialize if automatic
            CPUExtensionType[] detectOrder = new CPUExtensionType[] { CPUExtensionType.AVX2, CPUExtensionType.AVX, CPUExtensionType.SSE2 };

            // #1 try to initialize with Configured extension
            bool isInitialized = InitializeMinerPaths((CPUExtensionType)Config.ConfigData.ForceCPUExtension);
            // #2 if automatic or does not support then initialize in order
            if (isInitialized == false) {
                Config.ConfigData.ForceCPUExtension = (int)CPUExtensionType.Automatic; // set to automatic if not supported
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


        protected override string BenchmarkCreateCommandLine(AlgorithmType algorithmType, int time)
        {
            Path = CPUMinerPath;
            if (SupportedAlgorithms[algorithmType].NiceHashName.Equals("hodl")) Path = HodlMinerPath;

            string ret = "--algo=" + SupportedAlgorithms[algorithmType].MinerName + 
                         " --benchmark" + 
                         " --threads=" + Threads.ToString() +
                         " " + ExtraLaunchParameters + 
                         " " + SupportedAlgorithms[algorithmType].ExtraLaunchParameters;

            if (!SupportedAlgorithms[algorithmType].NiceHashName.Equals("hodl"))
                ret += " --time-limit " + time.ToString();

            return ret;
        }


        protected override Process BenchmarkStartProcess(string CommandLine)
        {
            Process BenchmarkHandle = base.BenchmarkStartProcess(CommandLine);

            if (AffinityMask != 0 && BenchmarkHandle != null)
                CPUID.AdjustAffinity(BenchmarkHandle.Id, AffinityMask);

            return BenchmarkHandle;
        }


        public override void Start(AlgorithmType nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running

            if (CDevs.Count == 0 || !CDevs[0].Enabled) return;

            Algorithm Algo = GetMinerAlgorithm(nhalgo);
            if (Algo == null) return;

            Path = CPUMinerPath;
            if (Algo.NiceHashName.Equals("hodl")) Path = HodlMinerPath;

            LastCommandLine = "--algo=" + Algo.MinerName + 
                              " --url=" + url + 
                              " --userpass=" + username + ":" + GetPassword(Algo) + 
                              " --threads=" + Threads.ToString() + 
                              " " + ExtraLaunchParameters + 
                              " " + Algo.ExtraLaunchParameters;

            if (!Algo.NiceHashName.Equals("hodl"))
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
    }
}
