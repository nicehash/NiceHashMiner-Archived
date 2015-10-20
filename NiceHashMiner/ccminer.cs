using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;

namespace NiceHashMiner
{
    abstract public class ccminer : Miner
    {
        private List<int> BenchmarkedGPUs;

        public ccminer()
        {
            SupportedAlgorithms = new Algorithm[] { 
                new Algorithm(3, "x11", "x11"),
                new Algorithm(4, "x13", "x13"),
                new Algorithm(5, "keccak", "keccak"),
                new Algorithm(7, "nist5", "nist5"),
                new Algorithm(8, "neoscrypt", "neoscrypt"),
                new Algorithm(10, "whirlpoolx", "whirlpoolx"),
                new Algorithm(11, "qubit", "qubit"),
                new Algorithm(12, "quark", "quark"),
                new Algorithm(14, "lyra2rev2", "lyra2v2")
            };
        }


        protected override string BenchmarkCreateCommandLine(int index)
        {
            string CommandLine = "--algo=" + SupportedAlgorithms[index].MinerName + 
                                 " --benchmark" +
                                 " " + ExtraLaunchParameters +
                                 " " + SupportedAlgorithms[index].ExtraLaunchParameters + 
                                 " --devices ";

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled)
                    CommandLine += G.ID.ToString() + ",";

            CommandLine = CommandLine.Remove(CommandLine.Length - 1);

            return CommandLine;
        }


        public override void BenchmarkStart(int index, BenchmarkComplete oncomplete, object tag)
        {
            base.BenchmarkStart(index, oncomplete, tag);
            BenchmarkedGPUs = new List<int>();
        }


        protected override void BenchmarkParseLine(string outdata)
        {
            // parse line
            if (outdata.Contains("Total: ") && outdata.Contains("/s"))
            {
                if (EnabledDeviceCount() == BenchmarkedGPUs.Count)
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
                    else if (hashspeed.Contains("GH/s"))
                        spd *= 1000000000;
                    SupportedAlgorithms[BenchmarkIndex].BenchmarkSpeed = spd;

                    BenchmarkStop();
                    OnBenchmarkComplete(PrintSpeed(spd), BenchmarkTag);
                    return;
                }
            }
            else if (outdata.Contains("] GPU") && !outdata.Contains("Found") && !outdata.Contains("nounce"))
            {
                // remember this GPU
                int i = outdata.IndexOf("GPU ");
                int id = int.Parse(outdata.Substring(i + 5, 1));
                if (!BenchmarkedGPUs.Contains(id))
                    BenchmarkedGPUs.Add(id);
            }
        }


        public override void Start(int nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running 

            Algorithm Algo = GetMinerAlgorithm(nhalgo);
            if (Algo == null) return;

            LastCommandLine = "--algo=" + Algo.MinerName + 
                              " --url=" + url + 
                              " --userpass=" + username + ":" + GetPassword(Algo) + 
                              " --api-bind=" + APIPort.ToString() + 
                              " " + ExtraLaunchParameters +
                              " " + Algo.ExtraLaunchParameters + 
                              " --devices ";

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled)
                    LastCommandLine += G.ID.ToString() + ",";

            if (LastCommandLine.EndsWith(","))
            {
                LastCommandLine = LastCommandLine.Remove(LastCommandLine.Length - 1);
            }
            else
            {
                LastCommandLine = "";
                return; // no GPUs to start mining on
            }

            _Start();
        }


        abstract protected void AddPotentialCDev(string text);


        protected void QueryCDevs()
        {
            Process P = new Process();
            P.StartInfo.FileName = Path;
            P.StartInfo.Arguments = "--ndevs";
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.CreateNoWindow = true;
            P.Start();

            string outdata;

            do
            {
                outdata = P.StandardError.ReadLine();
                if (outdata != null)
                    AddPotentialCDev(outdata);
            } while (outdata != null);

            P.WaitForExit();
        }
    }
}
