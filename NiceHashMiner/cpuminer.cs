using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;

namespace NiceHashMiner
{
    class cpuminer : Miner
    {
        public cpuminer()
        {
            APIPort = 4047;

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

            CDevs.Add(new ComputeDevice(0, CPUID.GetCPUVendor(), CPUID.GetCPUName()));
        }


        public override void Start(int nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running

            string AlgoName = GetMinerAlgorithmName(nhalgo);
            if (AlgoName == null) return;

            string CommandLine = "--algo=" + AlgoName + " --url=" + url + " --userpass=" + username + ":x --api-bind=" + APIPort.ToString();

            if (CDevs.Count == 0 || !CDevs[0].Enabled) return;

            ProcessHandle = Process.Start(Path, CommandLine);
            ProcessHandle.EnableRaisingEvents = true;
            ProcessHandle.Exited += Miner_Exited;
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
