using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Management;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners
{
    // for now AMD only
    class sgminer : Miner
    {
        private readonly int GPUPlatformNumber;
        const string TemperatureParam = " --gpu-fan 30-95 --temp-cutoff 95 --temp-overheat 90" +
                                        " --temp-target 75 --auto-fan --auto-gpu";
        
        // we only group devices that are compatible. for sgminer we have gpucodename and enabled optimized vesrion as mandatory extra parameters
        private string CommonGpuCodenameSetting = "";
        private bool EnableOptimizedVersion = true;

        // benchmark helper variables
        bool _benchmarkOnce = true;
        Stopwatch _benchmarkTimer = new Stopwatch();

        public sgminer()
            : base()
        {            
            MinerDeviceName = "AMD_OpenCL";
            Path = MinerPaths.sgminer_5_4_0_general;
            EnableOptimizedVersion = true;
            GPUPlatformNumber = ComputeDeviceQueryManager.Instance.AMDOpenCLPlatformNum;
        }

        protected override MinerType GetMinerType() {
            return MinerType.sgminer;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.AMD_OpenCL);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        public override void SetCDevs(string[] deviceUUIDs) {
            base.SetCDevs(deviceUUIDs);
            // now set extra sgminer settings, first dev is enough because of grouping logic
            if(CDevs.Count != 0) {
                CommonGpuCodenameSetting = CDevs[0].Codename;
                EnableOptimizedVersion = CDevs[0].IsOptimizedVersion;
            }
        }

        protected override int CalculateNumRetries() {
            return (ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds + ConfigManager.Instance.GeneralConfig.MinerAPIGraceSecondsAMD) / ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval;
        }

        public override void Restart() {
            StartingUpDelay = true;
            base.Restart();
        }

        protected override void _Stop(bool willswitch) {
            Stop_cpu_ccminer_sgminer(willswitch);
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username)
        {
            //if (ProcessHandle != null) return; // ignore, already running 

            //Algorithm miningAlgorithm = null;//GetMinerAlgorithm(nhalgo);
            CurrentMiningAlgorithm = miningAlgorithm;
            if (miningAlgorithm == null)
            {
                Helpers.ConsolePrint(MinerDeviceName, "GetMinerAlgorithm(" + miningAlgorithm.NiceHashID + "): Algo equals to null");
                return;
            }

            StartingUpDelay = true;

            Path = GetOptimizedMinerPath(miningAlgorithm.NiceHashID, CommonGpuCodenameSetting, EnableOptimizedVersion);

            LastCommandLine = " --gpu-platform " + GPUPlatformNumber +
                              " -k " + miningAlgorithm.MinerName +
                              " --url=" + url +
                              " --userpass=" + username + ":" + GetPassword(miningAlgorithm) +
                              " --api-listen" +
                              " --api-port=" + APIPort.ToString() +
                              " " + ExtraLaunchParameters +
                              " " + miningAlgorithm.ExtraLaunchParameters +
                              " --device ";

            for (int i = 0; i < CDevs.Count; i++)
                if (CDevs[i].Enabled)
                    LastCommandLine += CDevs[i].ID.ToString() + ",";

            if (LastCommandLine.EndsWith(","))
                LastCommandLine = LastCommandLine.Remove(LastCommandLine.Length - 1);
            else {
                LastCommandLine = "";
                return; // no GPUs to start mining on
            }

            if (ConfigManager.Instance.GeneralConfig.DisableAMDTempControl == false)
                LastCommandLine += TemperatureParam;

            ProcessHandle = _Start();
        }

        protected override void UpdateBindPortCommand(int oldPort, int newPort) {
            // --api-port=
            const string MASK = "--api-port={0}";
            var oldApiBindStr = String.Format(MASK, oldPort);
            var newApiBindStr = String.Format(MASK, newPort);
            if (LastCommandLine.Contains(oldApiBindStr)) {
                LastCommandLine = LastCommandLine.Replace(oldApiBindStr, newApiBindStr);
            }
        }

        public override string GetOptimizedMinerPath(AlgorithmType type, string gpuCodename, bool isOptimized) {
            if (EnableOptimizedVersion) {
                if (AlgorithmType.X11 == type || AlgorithmType.Quark == type || AlgorithmType.Lyra2REv2 == type || AlgorithmType.Qubit == type) {
                    if (!(gpuCodename.Contains("Hawaii") || gpuCodename.Contains("Pitcairn") || gpuCodename.Contains("Tahiti"))) {
                        if (!Helpers.InternalCheckIsWow64())
                            return MinerPaths.sgminer_5_4_0_general;

                        return MinerPaths.sgminer_5_4_0_tweaked;
                    }
                    // TODO CRITICAL IMPORTANT these segfault
                    if (AlgorithmType.X11 == type || AlgorithmType.Quark == type || AlgorithmType.Lyra2REv2 == type)
                        return MinerPaths.sgminer_5_1_0_optimized;
                    else
                        return MinerPaths.sgminer_5_1_1_optimized;
                }
            }

            return MinerPaths.sgminer_5_4_0_general;
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time) {
            string CommandLine;
            Path = "cmd";
            string MinerPath = GetOptimizedMinerPath(algorithm.NiceHashID, CommonGpuCodenameSetting, EnableOptimizedVersion);

            var nhAlgorithmData = Globals.NiceHashData[algorithm.NiceHashID];
            string url = "stratum+tcp://" + nhAlgorithmData.name + "." +
                         Globals.MiningLocation[ConfigManager.Instance.GeneralConfig.ServiceLocation] + ".nicehash.com:" +
                         nhAlgorithmData.port;

            string username = ConfigManager.Instance.GeneralConfig.BitcoinAddress.Trim();
            if (ConfigManager.Instance.GeneralConfig.WorkerName.Length > 0)
                username += "." + ConfigManager.Instance.GeneralConfig.WorkerName.Trim();

            CommandLine = " /C \"cd /d " + MinerPath.Replace("sgminer.exe", "") + " && sgminer.exe " +
                          " --gpu-platform " + GPUPlatformNumber +
                          " -k " + algorithm.MinerName +
                          " --url=" + url +
                          " --userpass=" + username + ":" + GetPassword(algorithm) +
                          " --sched-stop " + DateTime.Now.AddSeconds(time).ToString("HH:mm") +
                          " -T --log 10 --log-file dump.txt" +
                          " " + ExtraLaunchParameters +
                          " " + algorithm.ExtraLaunchParameters +
                          " --device ";

            CommandLine += GetDevicesCommandString();

            if (ConfigManager.Instance.GeneralConfig.DisableAMDTempControl == false)
                CommandLine += TemperatureParam;
            CommandLine += " && del dump.txt\"";

            return CommandLine;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            if (outdata.Contains("Average hashrate:") && outdata.Contains("/s")) {
                int i = outdata.IndexOf(": ");
                int k = outdata.IndexOf("/s");

                // save speed
                string hashSpeed = outdata.Substring(i + 2, k - i + 2);
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + hashSpeed);

                hashSpeed = hashSpeed.Substring(0, hashSpeed.IndexOf(" "));
                double speed = Double.Parse(hashSpeed, CultureInfo.InvariantCulture);

                if (outdata.Contains("Kilohash"))
                    speed *= 1000;
                else if (outdata.Contains("Megahash"))
                    speed *= 1000000;

                BenchmarkAlgorithm.BenchmarkSpeed = speed;
                return true;
            }
            return false;
        }

        protected override void BenchmarkThreadRoutineStartSettup() {
            if (MinerDeviceName.Equals("AMD_OpenCL")) {
                AlgorithmType NHDataIndex = BenchmarkAlgorithm.NiceHashID;

                if (Globals.NiceHashData == null) {
                    Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no internet " +
                        "connection. Sgminer needs internet connection to do benchmarking.");

                    throw new Exception("No internet connection");
                }

                if (Globals.NiceHashData[NHDataIndex].paying == 0) {
                    Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no work on Nicehash.com " +
                        "[algo: " + BenchmarkAlgorithm.NiceHashName + "(" + NHDataIndex + ")]");

                    throw new Exception("No work can be used for benchmarking");
                }

                _benchmarkTimer.Reset();
                _benchmarkTimer.Start();
            }
            base.BenchmarkThreadRoutineStartSettup();
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            if (_benchmarkTimer.Elapsed.Minutes >= BenchmarkTimeInSeconds + 1 && _benchmarkOnce == true) {
                _benchmarkOnce = false;
                string resp = GetAPIData(APIPort, "quit").TrimEnd(new char[] { (char)0 });
                Helpers.ConsolePrint("BENCHMARK", "SGMiner Response: " + resp);
            }
            if (_benchmarkTimer.Elapsed.Minutes >= BenchmarkTimeInSeconds + 2) {
                _benchmarkTimer.Stop();
                KillSGMiner();
                BenchmarkSignalHanged = true;
            }
            if (!BenchmarkSignalFinnished) {
                CheckOutdata(outdata);
            }
        }

        #endregion // Decoupled benchmarking routines

        public override APIData GetSummary() {
            string resp;
            string aname = null;
            APIData ad = new APIData();

            resp = GetAPIData(APIPort, "summary");
            if (resp == null) return null;

            try {
                string[] resps;

                if (!MinerDeviceName.Equals("AMD_OpenCL")) {
                    resps = resp.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < resps.Length; i++) {
                        string[] optval = resps[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (optval.Length != 2) continue;
                        if (optval[0] == "ALGO")
                            aname = optval[1];
                        else if (optval[0] == "KHS")
                            ad.Speed = double.Parse(optval[1], CultureInfo.InvariantCulture) * 1000; // HPS
                    }
                } else {
                    // Checks if all the GPUs are Alive first
                    string resp2 = GetAPIData(APIPort, "devs");
                    if (resp2 == null) return null;

                    string[] checkGPUStatus = resp2.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 1; i < checkGPUStatus.Length - 1; i++) {
                        if (!checkGPUStatus[i].Contains("Status=Alive")) {
                            Helpers.ConsolePrint(MinerDeviceName, "GPU " + i + ": Sick/Dead/NoStart/Initialising/Disabled/Rejecting/Unknown");
                            return null;
                        }
                    }

                    resps = resp.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    if (resps[1].Contains("SUMMARY")) {
                        string[] data = resps[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Get miner's current total speed
                        string[] speed = data[4].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        // Get miner's current total MH
                        double total_mh = Double.Parse(data[18].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1], new CultureInfo("en-US"));

                        ad.Speed = Double.Parse(speed[1]) * 1000;

                        //aname = SupportedAlgorithms[CurrentAlgorithmType].MinerName;
                        aname = CurrentMiningAlgorithm.MinerName;


                        if (total_mh <= PreviousTotalMH) {
                            Helpers.ConsolePrint(MinerDeviceName, "SGMiner might be stuck as no new hashes are being produced");
                            Helpers.ConsolePrint(MinerDeviceName, "Prev Total MH: " + PreviousTotalMH + " .. Current Total MH: " + total_mh);
                            return null;
                        }

                        PreviousTotalMH = total_mh;
                    } else {
                        ad.Speed = 0;
                    }
                }
            } catch {
                return null;
            }

            FillAlgorithm(aname, ref ad);
            return ad;
        }
    }
}
