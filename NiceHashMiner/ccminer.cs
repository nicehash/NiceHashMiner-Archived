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
                CommandLine = " --benchmark-warmup 10 --benchmark-trial 20" +
                              " " + ExtraLaunchParameters +
                              " " + SupportedAlgorithms[index].ExtraLaunchParameters +
                              " --dag-dir " + Config.ConfigData.DAGDirectory + "\\" + MinerDeviceName +
                              " --cuda --cuda-devices ";

                for (int i = 0; i < CDevs.Count; i++)
                    if (EtherDevices[i] != -1 && CDevs[i].Enabled)
                        CommandLine += i + " ";

                CommandLine += " --benchmark ";
                if (Ethereum.GetCurrentBlock(MinerDeviceName))
                    CommandLine += Ethereum.CurrentBlockNum;

                // Check if dag-dir exist to avoid ethminer from crashing
                if (!Ethereum.CreateDAGDirectory(MinerDeviceName)) return "";
            }
            else
            {
                CommandLine = " --algo=" + SupportedAlgorithms[index].MinerName +
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

                // Create DAG file ahead of time
                if (!Ethereum.CreateDAGFile(Config.ConfigData.HideMiningWindows, MinerDeviceName)) return;

                // Starts up ether-proxy
                if (!Ethereum.StartProxy(true, url, username)) return;

                LastCommandLine = " --cuda -F http://127.0.0.1:" + Config.ConfigData.APIBindPortEthereumProxy + "/miner/10/" + MinerDeviceName + " " +
                                  " --erase-dags old" +
                                  " " + ExtraLaunchParameters +
                                  " " + Algo.ExtraLaunchParameters +
                                  " --dag-dir " + Config.ConfigData.DAGDirectory + "\\" + MinerDeviceName +
                                  " --cuda-devices ";

                for (int i = 0; i < CDevs.Count; i++)
                    if (EtherDevices[i] != -1 && CDevs[i].Enabled)
                        LastCommandLine += EtherDevices[i] + " ";
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

        protected void AddEthereum(string match)
        {
            EtherDevices = new int[CDevs.Count];
            try
            {
                Process P = new Process();
                P.StartInfo.FileName = Ethereum.EtherMinerPath;
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.CreateNoWindow = true;

                string outdata;

                P.StartInfo.Arguments = "--list-devices --cuda";
                P.Start();

                int index = 0, device = 0;
                do
                {
                    outdata = P.StandardOutput.ReadLine();
                    if (outdata != null && outdata.Contains("GeForce"))
                    {
                        string compute = P.StandardOutput.ReadLine();
                        string memory = P.StandardOutput.ReadLine();

                        // Find only the right cards
                        if (compute.Contains(match))
                        {
                            EtherDevices[index] = -1;
                            string [] memsplit = memory.Split(':');

                            long memsize = Convert.ToInt64(memsplit[memsplit.Length - 1]);
                            if (memsize >= 2147483648)
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (GOOD!)");
                                EtherDevices[index] = device;
                                index++;
                            }
                            else
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (NOT GOOD!)");
                            }
                        }
                        device++;
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
                if (CDevs.Count != 0)
                {
                    if (this is ccminer_sp)
                    {
                        Helpers.ConsolePrint(MinerDeviceName, "Adding Ethereum..");
                        AddEthereum("Compute version: 5.2");
                        AddEthereum("Compute version: 5.0");
                    }
                    else if (this is ccminer_tpruvot)
                    {
                        Helpers.ConsolePrint(MinerDeviceName, "Adding Ethereum..");
                        AddEthereum("Compute version: 3.0");
                    }
                }
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
