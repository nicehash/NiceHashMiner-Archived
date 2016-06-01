using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

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

            SupportedAlgorithms = new Algorithm[] { 
                    new Algorithm(9, "lyra2re", "lyra2"),
                    new Algorithm(13, "axiom", "axiom"),
                    new Algorithm(15, "scryptjanenf16", "scryptjane:16"),
                    new Algorithm(19, "hodl", "hodl")
                };

            if (Config.ConfigData.ForceCPUExtension > 0)
            {
                if (Config.ConfigData.ForceCPUExtension == 1)
                {
                    CPUMinerPath = "bin\\cpuminer_x64_SSE2.exe";
                    HodlMinerPath = "bin\\hodlminer\\hodlminer_core2.exe";
                }
                else if (Config.ConfigData.ForceCPUExtension == 2)
                {
                    CPUMinerPath = "bin\\cpuminer_x64_AVX.exe";
                    HodlMinerPath = "bin\\hodlminer\\hodlminer_corei7_avx.exe";
                }
                else
                {
                    CPUMinerPath = "bin\\cpuminer_x64_AVX2.exe";
                    HodlMinerPath = "bin\\hodlminer\\hodlminer_core_avx2.exe";
                }
            }
            else
            {
                // detect CPU capabilities
                if (CPUID.SupportsAVX2() == 0)
                {
                    if (CPUID.SupportsAVX() == 0)
                    {
                        if (CPUID.SupportsSSE2() == 0)
                            return;

                        CPUMinerPath = "bin\\cpuminer_x64_SSE2.exe";
                        HodlMinerPath = "bin\\hodlminer\\hodlminer_core2.exe";
                    }
                    else
                    {
                        CPUMinerPath = "bin\\cpuminer_x64_AVX.exe";
                        HodlMinerPath = "bin\\hodlminer\\hodlminer_corei7_avx.exe";
                    }
                }
                else
                {
                    CPUMinerPath = "bin\\cpuminer_x64_AVX2.exe";
                    HodlMinerPath = "bin\\hodlminer\\hodlminer_core_avx2.exe";
                }
            }

            CDevs.Add(new ComputeDevice(0, MinerDeviceName, CPUID.GetCPUName().Trim()));
        }


        public override string PrintSpeed(double spd)
        {
            // print in kH/s
            return (spd * 0.001).ToString("F3", CultureInfo.InvariantCulture) + " kH/s";
        }


        protected override string BenchmarkCreateCommandLine(int index, int time)
        {
            Path = CPUMinerPath;
            if (SupportedAlgorithms[index].NiceHashName.Equals("hodl")) Path = HodlMinerPath;

            string ret = "--algo=" + SupportedAlgorithms[index].MinerName + 
                         " --benchmark" + 
                         " --threads=" + Threads.ToString() +
                         " " + ExtraLaunchParameters + 
                         " " + SupportedAlgorithms[index].ExtraLaunchParameters;

            if (!SupportedAlgorithms[index].NiceHashName.Equals("hodl"))
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


        public override void Start(int nhalgo, string url, string username)
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


        protected override Process _Start()
        {
            Process P = base._Start();

            if (AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, AffinityMask);

            return P;
        }
    }
}
