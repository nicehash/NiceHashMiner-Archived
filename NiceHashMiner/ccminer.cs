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


        protected override string BenchmarkCreateCommandLine(int index, int time)
        {
            string CommandLine = "--algo=" + SupportedAlgorithms[index].MinerName + 
                                 " --benchmark" +
                                 " --time-limit " + time.ToString() +
                                 " " + ExtraLaunchParameters +
                                 " " + SupportedAlgorithms[index].ExtraLaunchParameters + 
                                 " --devices ";

            foreach (ComputeDevice G in CDevs)
                if (G.Enabled)
                    CommandLine += G.ID.ToString() + ",";

            CommandLine = CommandLine.Remove(CommandLine.Length - 1);

            return CommandLine;
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

            ProcessHandle = _Start();
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
