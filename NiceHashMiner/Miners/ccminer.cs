using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Miners
{
    abstract public class ccminer : Miner
    {
        public ccminer(bool queryComputeDevices) : base(queryComputeDevices) { }

        public override void Start(Algorithm miningAlgorithm, string url, string username)
        {
            //if (ProcessHandle != null) return; // ignore, already running 

            CurrentMiningAlgorithm = miningAlgorithm;
            if (miningAlgorithm == null) return;

            LastCommandLine = "--algo=" + miningAlgorithm.MinerName +
                                  " --url=" + url +
                                  " --userpass=" + username + ":" + GetPassword(miningAlgorithm) +
                                  " --api-bind=" + APIPort.ToString() +
                                  " " + ExtraLaunchParameters +
                                  " " + miningAlgorithm.ExtraLaunchParameters +
                                  " --devices ";

            LastCommandLine += GetDevicesCommandString();

            Path = GetOptimizedMinerPath(miningAlgorithm.NiceHashID);

            ProcessHandle = _Start();
        }

        protected override void _Stop(bool willswitch) {
            Stop_cpu_ccminer_sgminer(willswitch);
        }

        // this method checks SM versions
        abstract protected bool IsPotentialDevSM(string name);

        protected void AddPotentialCDev(string text) {
            if (!text.Contains("GPU")) return;

            Helpers.ConsolePrint(MinerDeviceName, "Detected: " + text);

            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            // add only suported SM by miner
            if (IsPotentialDevSM(name))
            {
                name = name.Substring(8);
                CDevs.Add(new ComputeDevice(id, MinerDeviceName, name, this, true));
                Helpers.ConsolePrint(MinerDeviceName, "Added: " + name);
            }
        }

        protected void AddEthereum(string match, bool initialize)
        {
            if (initialize)
            {
                for (int i = 0; i < CDevs.Count; i++)
                {
                    EtherDevices[i] = -1;
                }
            }

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
                            string[] memsplit = memory.Split(':');
                            long memsize = Convert.ToInt64(memsplit[memsplit.Length - 1]);
                            EtherDevices[index] = -1;

                            if (memsize >= 2147483648)
                            {
                                if (outdata.Contains("750") && outdata.Contains("Ti"))
                                {
                                    Helpers.ConsolePrint(MinerDeviceName, "GTX 750Ti found! By default this device will be disabled for ethereum as it is generally too slow to mine on it.");
                                }
                                else
                                {
                                    Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (GOOD!)");
                                    EtherDevices[index] = device;
                                    index++;
                                }
                            }
                            else
                            {
                                Helpers.ConsolePrint(MinerDeviceName, "Ethereum GPU MemSize: " + memsize + " (NOT GOOD!)");
                            }
                        }
                        else
                        {
                            Helpers.ConsolePrint(MinerDeviceName, "Skipping GPU " + outdata + " as it does match the criteria [" + match + "]");
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

        protected override void QueryCDevs()
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
                    EtherDevices = new int[CDevs.Count];

                    if (this is ccminer_sm5x)
                    {
                        Helpers.ConsolePrint(MinerDeviceName, "Adding Ethereum..");
                        //AddEthereum("Compute version: 6.0");
                        AddEthereum("Compute version: 5.2", true);
                        AddEthereum("Compute version: 5.0", false);
                    }
                    else if (this is ccminer_sm3x)
                    {
                        Helpers.ConsolePrint(MinerDeviceName, "Adding Ethereum..");
                        AddEthereum("Compute version: 3.0", true);
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
                    if (this is ccminer_sm5x)
                        ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia5X = true;
                    else if (this is ccminer_sm3x)
                        ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia3X = true;
                    else if (this is ccminer_sm21)
                        ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia2X = true;
                }

                WarningDialog = null;

                return;
            }
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines
        protected override string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time) {
            string CommandLine = " --algo=" + algorithm.MinerName +
                              " --benchmark" +
                              " --time-limit " + time.ToString() +
                              " " + ExtraLaunchParameters +
                              " " + algorithm.ExtraLaunchParameters +
                              " --devices ";

            CommandLine += GetDevicesCommandString();

            Path = GetOptimizedMinerPath(algorithm.NiceHashID);

            return CommandLine;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            double lastSpeed = 0;
            if (double.TryParse(outdata, out lastSpeed)) {
                BenchmarkAlgorithm.BenchmarkSpeed = lastSpeed;
                return true;
            }
            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        #endregion // Decoupled benchmarking routines

        public override APIData GetSummary() {
            return GetSummaryCPU_CCMINER();
        }

    }
}
