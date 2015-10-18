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

        //public cpuminer()
        //{
        //    MinerDeviceName = "CPU(s)";
        //    APIPort = 4047;

        //    SupportedAlgorithms = new Algorithm[] { 
        //        new Algorithm(9, "lyra2re", "lyra2"),
        //        new Algorithm(13, "axiom", "axiom"),
        //        new Algorithm(15, "scryptjaneleo", "scryptjane:16")
        //    };

        //    // detect CPU capabilities
        //    if (CPUID.SupportsAVX2() == 0)
        //    {
        //        if (CPUID.SupportsAVX() == 0)
        //        {
        //            if (CPUID.SupportsSSE2() == 0)
        //                return;

        //            Path = "bin\\cpuminer_x64_SSE2.exe";
        //        }
        //        else
        //            Path = "bin\\cpuminer_x64_AVX.exe";
        //    }
        //    else
        //    {
        //        Path = "bin\\cpuminer_x64_AVX2.exe";
        //    }

        //    CDevs.Add(new ComputeDevice(0, CPUID.GetCPUVendor(), CPUID.GetCPUName()));
        //}

        public cpuminer(int id, int threads, ulong affinity)
        {
            MinerDeviceName = "CPU" + id.ToString();
            APIPort = 4040 + id;
            Threads = threads;
            AffinityMask = affinity;

            SupportedAlgorithms = new Algorithm[] { 
                    new Algorithm(9, "lyra2re", "lyra2"),
                    new Algorithm(13, "axiom", "axiom"),
                    new Algorithm(15, "scryptjaneleo", "scryptjane:16")
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


        public override void BenchmarkStart(int index, BenchmarkComplete oncomplete, object tag)
        {
            OnBenchmarkComplete = oncomplete;
            
            if (BenchmarkTimer != null)
            {
                OnBenchmarkComplete("Benchmark running", tag);
                return;
            }

            if (ProcessHandle != null)
            {
                OnBenchmarkComplete("Miner running", tag);
                return; // ignore, already running
            }

            if (index >= SupportedAlgorithms.Length)
            {
                OnBenchmarkComplete("Unknown algorithm", tag);
                return;
            }

            if (CDevs.Count == 0 || !CDevs[0].Enabled)
            {
                OnBenchmarkComplete("Disabled", tag);
                return; // ignore, disabled device
            }

            BenchmarkTag = tag;
            BenchmarkIndex = index;

            string CommandLine = "--algo=" + SupportedAlgorithms[index].MinerName + " --benchmark";

            ProcessHandle = new Process();
            ProcessHandle.StartInfo.FileName = Path;
            ProcessHandle.StartInfo.Arguments = CommandLine;
            ProcessHandle.StartInfo.UseShellExecute = false;
            ProcessHandle.StartInfo.RedirectStandardOutput = true;
            ProcessHandle.StartInfo.CreateNoWindow = true;
            ProcessHandle.Start();
            if (AffinityMask != 0)
                CPUID.AdjustAffinity(ProcessHandle.Id, AffinityMask);

            BenchmarkTimer = new System.Windows.Forms.Timer();
            BenchmarkTimer.Interval = 100;
            BenchmarkTimer.Tick += BenchmarkTimer_Tick;
            BenchmarkTimer.Start();
        }


        void BenchmarkTimer_Tick(object sender, EventArgs e)
        {
            string outdata;
            //do
            //{
                outdata = ProcessHandle.StandardOutput.ReadLine();
                if (outdata != null)
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
                        return;
                    }
                }
            //    break;
            //} while (outdata != null);
        }


        private void Miner_Exited_Benchmark(object sender, EventArgs e)
        {
            BenchmarkStop();
            OnBenchmarkComplete("User exited", BenchmarkTag);
        }


        public override void Start(int nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running

            if (CDevs.Count == 0 || !CDevs[0].Enabled) return;

            string AlgoName = GetMinerAlgorithmName(nhalgo);
            if (AlgoName == null) return;

            string CommandLine = "--algo=" + AlgoName + " --url=" + url + " --userpass=" + username + ":x --api-bind=" + APIPort.ToString() + " --threads=" + Threads.ToString();

            Debug.Print(MinerDeviceName + " Starting miner: " + CommandLine);

            ProcessHandle = new Process();
            ProcessHandle.StartInfo.FileName = Path;
            ProcessHandle.StartInfo.Arguments = CommandLine;
            ProcessHandle.Exited += Miner_Exited;
            ProcessHandle.Start();
            if (AffinityMask != 0)
                CPUID.AdjustAffinity(ProcessHandle.Id, AffinityMask);
        }


        private void Miner_Exited(object sender, EventArgs e)
        {
            Stop();
        }


        public override APIData GetSummary()
        {
            string resp = GetAPIData(APIPort, "summary");
            if (resp == null) return null;

            string aname = null;
            APIData ad = new APIData();

            try
            {
                string[] resps = resp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < resps.Length; i++)
                {
                    string[] optval = resps[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (optval.Length != 2) continue;
                    if (optval[0] == "ALGO")
                    {
                        aname = optval[1];
                        if (aname == "scryptjane") aname += ":16"; // temporary, fix this later
                    }
                    else if (optval[0] == "KHS")
                        ad.Speed = double.Parse(optval[1], CultureInfo.InvariantCulture) * 1000; // HPS
                }
            }
            catch
            {
                return null;
            }

            FillAlgorithm(aname, ref ad);

            return ad;
        }
    }
}
