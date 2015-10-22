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


        public cpuminer(int id, int threads, ulong affinity)
        {
            MinerDeviceName = "CPU" + id.ToString();
            APIPort = 4040 + id;
            Threads = threads;
            AffinityMask = affinity;

            SupportedAlgorithms = new Algorithm[] { 
                    new Algorithm(9, "lyra2re", "lyra2"),
                    new Algorithm(13, "axiom", "axiom"),
                    new Algorithm(15, "scryptjanenf16", "scryptjane:16")
                };

            if (Config.ConfigData.ForceCPUExtension > 0)
            {
                if (Config.ConfigData.ForceCPUExtension == 1)
                {
                    Path = "bin\\cpuminer_x64_SSE2.exe";
                }
                else if (Config.ConfigData.ForceCPUExtension == 2)
                {
                    Path = "bin\\cpuminer_x64_AVX.exe";
                }
                else
                {
                    Path = "bin\\cpuminer_x64_AVX2.exe";
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

                        Path = "bin\\cpuminer_x64_SSE2.exe";
                    }
                    else
                        Path = "bin\\cpuminer_x64_AVX.exe";
                }
                else
                {
                    Path = "bin\\cpuminer_x64_AVX2.exe";
                }
            }

            CDevs.Add(new ComputeDevice(0, MinerDeviceName, CPUID.GetCPUName()));
        }


        public override string PrintSpeed(double spd)
        {
            // print in kH/s
            return (spd * 0.001).ToString("F3", CultureInfo.InvariantCulture) + " kH/s";
        }


        protected override string BenchmarkCreateCommandLine(int index, int time)
        {
            return "--algo=" + SupportedAlgorithms[index].MinerName + 
                   " --benchmark" + 
                   " --time-limit " + time.ToString() +
                   " --threads=" + Threads.ToString() +
                   " " + ExtraLaunchParameters + 
                   " " + SupportedAlgorithms[index].ExtraLaunchParameters;
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

            LastCommandLine = "--algo=" + Algo.MinerName + 
                              " --url=" + url + 
                              " --userpass=" + username + ":" + GetPassword(Algo) + 
                              " --api-bind=" + APIPort.ToString() + 
                              " --threads=" + Threads.ToString() + 
                              " " + ExtraLaunchParameters + 
                              " " + Algo.ExtraLaunchParameters;

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
