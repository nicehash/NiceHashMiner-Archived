using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

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
                new Algorithm(6, "x15", "x15"),
                new Algorithm(7, "nist5", "nist5"),
                new Algorithm(8, "neoscrypt", "neoscrypt"),
                new Algorithm(10, "whirlpoolx", "whirlpoolx"),
                new Algorithm(11, "qubit", "qubit"),
                new Algorithm(12, "quark", "quark"),
                new Algorithm(14, "lyra2rev2", "lyra2v2"),
                new Algorithm(16, "blake256r8", "blakecoin"),
                new Algorithm(17, "blake256r14", "blake"),
                new Algorithm(18, "blake256r8vnl", "vanilla"),
                new Algorithm(19, "ethereum", "ethereum")
            };
        }


        protected override string BenchmarkCreateCommandLine(int index, int time)
        {
            string CommandLine = "";

            if (SupportedAlgorithms[index].NiceHashName.Equals("ethereum"))
            {
                CommandLine = " --benchmark --benchmark-warmup 10 --benchmark-trial 20" +
                              " " + ExtraLaunchParameters +
                              " --cuda --cuda-devices ";

                for (int i = 0; i < CDevs.Count; i++)
                    if (EtherDevices[i] != -1 && CDevs[i].Enabled)
                        CommandLine += i + " ";
            }
            else
            {
                CommandLine = "--algo=" + SupportedAlgorithms[index].MinerName +
                              " --benchmark" +
                              " --time-limit " + time.ToString() +
                              " " + ExtraLaunchParameters +
                              " " + SupportedAlgorithms[index].ExtraLaunchParameters +
                              " --devices ";

                foreach (ComputeDevice G in CDevs)
                    if (G.Enabled)
                        CommandLine += G.ID.ToString() + ",";

                CommandLine = CommandLine.Remove(CommandLine.Length - 1);

                if (this is ccminer_sp && SupportedAlgorithms[index].NiceHashName.Equals("neoscrypt"))
                    Path = "bin\\ccminer_neoscrypt.exe";
                else if (this is ccminer_sp)
                    Path = "bin\\ccminer_sp.exe";
            }

            return CommandLine;
        }


        public override void Start(int nhalgo, string url, string username)
        {
            if (ProcessHandle != null) return; // ignore, already running 

            Algorithm Algo = GetMinerAlgorithm(nhalgo);
            if (Algo == null) return;

            if (Algo.NiceHashName.Equals("ethereum"))
            {
                StartingUpDelay = true;

                // Check if dag-dir exist to avoid ethminer from crashing
                if (!Directory.Exists(Config.ConfigData.DAGDirectory + "\\" + MinerDeviceName))
                    Directory.CreateDirectory(Config.ConfigData.DAGDirectory + "\\" + MinerDeviceName);

                // Create DAG file ahead of time
                if (!Ethereum.CreateDAGFile(MinerDeviceName)) return;

                // Starts up ether-proxy
                if (!Ethereum.StartProxy(true, url, username)) return;

                LastCommandLine = " --cuda -F http://127.0.0.1:" + Config.ConfigData.APIBindPortEthereumProxy + "/miner/10/" + MinerDeviceName + " " +
                                  " " + ExtraLaunchParameters +
                                  " --dag-dir " + Config.ConfigData.DAGDirectory + "\\" + MinerDeviceName +
                                  " --cuda-devices ";

                for (int i = 0; i < CDevs.Count; i++)
                    if (EtherDevices[i] != -1 && CDevs[i].Enabled)
                        LastCommandLine += i + " ";
            }
            else
            {
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

                if (this is ccminer_sp && Algo.NiceHashName.Equals("neoscrypt"))
                    Path = "bin\\ccminer_neoscrypt.exe";
                else if (this is ccminer_sp)
                    Path = "bin\\ccminer_sp.exe";
            }

            ProcessHandle = _Start();
        }


        abstract protected void AddPotentialCDev(string text);

        protected void AddEthereum(string aa)
        {
            EtherDevices = new int[CDevs.Count];
            try
            {
                Process P = new Process();
                P.StartInfo.FileName = Ethereum.EtherMinerPath;
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.CreateNoWindow = true;

                string outdata;

                P.StartInfo.Arguments = "--list-devices --cuda";
                P.Start();

                int i = 0;
                do
                {
                    outdata = P.StandardError.ReadLine();
                    if (outdata != null)
                    {
                        // Find only the right cards
                        if (outdata.Contains("GeForce") && outdata.Contains(aa))
                        {
                            outdata = P.StandardError.ReadLine();
                            outdata = P.StandardError.ReadLine();

                            long memsize = Convert.ToInt64(outdata.Split(':')[1]);
                            if (memsize >= 2147483648)
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (GOOD!)");
                                EtherDevices[i] = i;
                                i++;
                            }
                            else
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (NOT GOOD!)");
                                EtherDevices[i] = -1;
                                i++;
                            }
                        }
                    }
                } while (outdata != null);

                P.WaitForExit();
            }
            catch (Exception e)
            {
                Helpers.ConsolePrint(MinerDeviceName, "Exception: " + e.ToString());
            }
        }

        protected void QueryCDevs()
        {
            try
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

                // Check for ethereum mining
                if (this is ccminer_sp) AddEthereum(" 9");
            }
            catch (Exception e)
            {
                Helpers.ConsolePrint(MinerDeviceName, "Exception: " + e.ToString());
                
                MinerFileNotFoundDialog WarningDialog = new MinerFileNotFoundDialog(MinerDeviceName, Path);
                WarningDialog.ShowDialog();

                if (WarningDialog.DisableDetection)
                {
                    if (this is ccminer_sp)
                        Config.ConfigData.DisableDetectionNVidia5X = true;
                    else if (this is ccminer_tpruvot)
                        Config.ConfigData.DisableDetectionNVidia3X = true;
                    else if (this is ccminer_tpruvot_sm21)
                        Config.ConfigData.DisableDetectionNVidia2X = true;
                }

                WarningDialog = null;

                return;
            }
        }
    }
}
