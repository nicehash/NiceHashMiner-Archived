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

            CDevs.Add(new ComputeDevice(0, MinerDeviceName, CPUID.GetCPUName()));
        }


        public override string PrintSpeed(double spd)
        {
            // print in kH/s
            return (spd * 0.001).ToString("F2") + " kH/s";
        }


        protected override string BenchmarkCreateCommandLine(int index)
        {
            return "--algo=" + SupportedAlgorithms[index].MinerName + 
                   " --benchmark" + 
                   " --threads=" + Threads.ToString() +
                   " " + ExtraLaunchParameters + 
                   " " + SupportedAlgorithms[index].ExtraLaunchParameters;
        }


        public override void BenchmarkStart(int index, BenchmarkComplete oncomplete, object tag)
        {
            base.BenchmarkStart(index, oncomplete, tag);
            if (AffinityMask != 0 && ProcessHandle != null)
                CPUID.AdjustAffinity(ProcessHandle.Id, AffinityMask);
        }


        protected override void BenchmarkParseLine(string outdata)
        {
            // parse line
            if (outdata.Contains(" Total: ") && outdata.Contains("/s"))
            {
                int i = outdata.IndexOf("Total:");
                int k = outdata.IndexOf("/s");
                string hashspeed = outdata.Substring(i + 7, k - i - 5);

                // save speed
                int b = hashspeed.IndexOf(" ");
                double spd = Double.Parse(hashspeed.Substring(0, b), CultureInfo.InvariantCulture);
                if (hashspeed.Contains("kH/s"))
                    spd *= 1000;
                else if (hashspeed.Contains("MH/s"))
                    spd *= 1000000;
                SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = spd;

                BenchmarkStop();
                OnBenchmarkComplete(PrintSpeed(spd), BenchmarkTag);
            }
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

            _Start();
        }


        protected override void _Start()
        {
            base._Start();
            if (AffinityMask != 0 && ProcessHandle != null)
                CPUID.AdjustAffinity(ProcessHandle.Id, AffinityMask);
        }
    }
}
