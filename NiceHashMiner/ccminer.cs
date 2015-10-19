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
    public class ccminer : Miner
    {
        public ccminer()
        {
            MinerDeviceName = "NVIDIA GPU(s)";
            Path = "bin\\ccminer.exe";
            APIPort = 4048;

            SupportedAlgorithms = new Algorithm[] { 
                new Algorithm(0, "scrypt", "scrypt"),
                new Algorithm(12, "quark", "quark")
            };

            QueryCDevs();
        }


        public override void BenchmarkStart(int index, BenchmarkComplete oncomplete, object tag) { }

        public override void Start(int nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running 

            //string CommandLine = "--url=sumplemultialgo+NiceHash+" + suburl + " --userpass=" + username + ":x --api-bind=" + APIPort.ToString() + " --devices ";
            string CommandLine = "--algo=" + "test" + " --url=" + url + " --userpass=" + username + ":x --api-bind=" + APIPort.ToString() + " --devices ";

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled)
                    CommandLine += G.ID.ToString() + ",";

            if (CommandLine.EndsWith(","))
            {
                CommandLine = CommandLine.Remove(CommandLine.Length - 1);
            }
            else
                return; // no GPUs to start mining on

            ProcessHandle = Process.Start(Path, CommandLine);
            ProcessHandle.EnableRaisingEvents = true;
            ProcessHandle.Exited += Miner_Exited;
        }


        public override void Restart()
        {
        }


        private void Miner_Exited(object sender, EventArgs e)
        {
            Stop();
        }


        private void QueryCDevs()
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
                    CDevs.Add(new ComputeDevice(CDevs.Count, "NVIDIA", outdata.Split(':')[1]));
            } while (outdata != null);

            P.WaitForExit();
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
                        aname = optval[1];
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
