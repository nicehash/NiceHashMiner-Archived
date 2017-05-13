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
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using System.Threading;

namespace NiceHashMiner.Miners
{
    class sgminer : Miner
    {
        private readonly int GPUPlatformNumber;
        Stopwatch _benchmarkTimer = new Stopwatch();

        public sgminer()
            : base("sgminer_AMD")
        {
            GPUPlatformNumber = ComputeDeviceManager.Avaliable.AMDOpenCLPlatformNum;
            IsKillAllUsedMinerProcs = true;
        }

        // use ONLY for exiting a benchmark
        public void KillSGMiner() {
            foreach (Process process in Process.GetProcessesByName("sgminer")) {
                try { process.Kill(); } catch (Exception e) { Helpers.ConsolePrint(MinerDeviceName, e.ToString()); }
            }
        }

        public override void EndBenchmarkProcces() {
            if (BenchmarkProcessStatus != BenchmarkProcessStatus.Killing && BenchmarkProcessStatus != BenchmarkProcessStatus.DoneKilling) {
                BenchmarkProcessStatus = BenchmarkProcessStatus.Killing;
                try {
                    Helpers.ConsolePrint("BENCHMARK", String.Format("Trying to kill benchmark process {0} algorithm {1}", BenchmarkProcessPath, BenchmarkAlgorithm.AlgorithmName));
                    KillSGMiner();
                } catch { } finally {
                    BenchmarkProcessStatus = BenchmarkProcessStatus.DoneKilling;
                    Helpers.ConsolePrint("BENCHMARK", String.Format("Benchmark process {0} algorithm {1} KILLED", BenchmarkProcessPath, BenchmarkAlgorithm.AlgorithmName));
                    //BenchmarkHandle = null;
                }
            }
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 90 * 1000; // 1.5 minute max, whole waiting time 75seconds
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        public override void Start(string url, string btcAdress, string worker)
        {
            if (!IsInit) {
                Helpers.ConsolePrint(MinerTAG(), "MiningSetup is not initialized exiting Start()");
                return;
            }
            string username = GetUsername(btcAdress, worker);
            
            LastCommandLine = " --gpu-platform " + GPUPlatformNumber +
                              " -k " + MiningSetup.MinerName +
                              " --url=" + url +
                              " --userpass=" + username +
                              " -p x " +
                              " --api-listen" +
                              " --api-port=" + APIPort.ToString() +
                              " " +
                              ExtraLaunchParametersParser.ParseForMiningSetup(
                                                                MiningSetup,
                                                                DeviceType.AMD) +
                              " --device ";

            LastCommandLine += GetDevicesCommandString();

            ProcessHandle = _Start();
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            string CommandLine;

            string url = Globals.GetLocationURL(algorithm.NiceHashID, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);

            // demo for benchmark
            string username = Globals.DemoUser;

            if (ConfigManager.GeneralConfig.WorkerName.Length > 0)
                username += "." + ConfigManager.GeneralConfig.WorkerName.Trim();

            // cd to the cgminer for the process bins
            CommandLine = " /C \"cd /d " + WorkingDirectory + " && sgminer.exe " +
                          " --gpu-platform " + GPUPlatformNumber +
                          " -k " + algorithm.MinerName +
                          " --url=" + url +
                          " --userpass=" + username +
                          " -p x " +
                          " --sched-stop " + DateTime.Now.AddSeconds(time).ToString("HH:mm") +
                          " -T --log 10 --log-file dump.txt" +
                          " " +
                          ExtraLaunchParametersParser.ParseForMiningSetup(
                                                                MiningSetup,
                                                                DeviceType.AMD) +
                          " --device ";

            CommandLine += GetDevicesCommandString();

            CommandLine += " && del dump.txt\"";

            return CommandLine;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            if (outdata.Contains("Average hashrate:") && outdata.Contains("/s") && BenchmarkAlgorithm.NiceHashID != AlgorithmType.DaggerHashimoto) {
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
            } else if (outdata.Contains(String.Format("GPU{0}", MiningSetup.MiningPairs[0].Device.ID)) && outdata.Contains("s):") && BenchmarkAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto) {
                int i = outdata.IndexOf("s):");
                int k = outdata.IndexOf("(avg)");

                // save speed
                string hashSpeed = outdata.Substring(i + 3, k - i + 3).Trim();
                hashSpeed = hashSpeed.Replace("(avg):", "");
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + hashSpeed);

                double mult = 1;
                if (hashSpeed.Contains("K")) {
                    hashSpeed = hashSpeed.Replace("K", " ");
                    mult = 1000;
                } else if (hashSpeed.Contains("M")) {
                    hashSpeed = hashSpeed.Replace("M", " ");
                    mult = 1000000;
                }

                hashSpeed = hashSpeed.Substring(0, hashSpeed.IndexOf(" "));
                double speed = Double.Parse(hashSpeed, CultureInfo.InvariantCulture) * mult;

                BenchmarkAlgorithm.BenchmarkSpeed = speed;

                return true;
            }
            return false;
        }

        protected override void BenchmarkThreadRoutineStartSettup() {
            // sgminer extra settings
            AlgorithmType NHDataIndex = BenchmarkAlgorithm.NiceHashID;

            if (Globals.NiceHashData == null) {
                Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no internet " +
                    "connection. Sgminer needs internet connection to do benchmarking.");

                throw new Exception("No internet connection");
            }

            if (Globals.NiceHashData[NHDataIndex].paying == 0) {
                Helpers.ConsolePrint("BENCHMARK", "Skipping sgminer benchmark because there is no work on Nicehash.com " +
                    "[algo: " + BenchmarkAlgorithm.AlgorithmName + "(" + NHDataIndex + ")]");

                throw new Exception("No work can be used for benchmarking");
            }

            _benchmarkTimer.Reset();
            _benchmarkTimer.Start();
            // call base, read only outpus
            //BenchmarkHandle.BeginOutputReadLine();
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            if (_benchmarkTimer.Elapsed.TotalSeconds >= BenchmarkTimeInSeconds) {
                string resp = GetAPIData(APIPort, "quit").TrimEnd(new char[] { (char)0 });
                Helpers.ConsolePrint("BENCHMARK", "SGMiner Response: " + resp);
            }
            if (_benchmarkTimer.Elapsed.TotalSeconds >= BenchmarkTimeInSeconds + 2) {
                _benchmarkTimer.Stop();
                // this is safe in a benchmark
                KillSGMiner();
                BenchmarkSignalHanged = true;
            }
            if (!BenchmarkSignalFinnished && outdata != null) {
                CheckOutdata(outdata);
            }
        }

        protected override string GetFinalBenchmarkString() {
            if (BenchmarkAlgorithm.BenchmarkSpeed <= 0) {
                Helpers.ConsolePrint("sgminer_GetFinalBenchmarkString", International.GetText("sgminer_precise_try"));
                return International.GetText("sgminer_precise_try");
            }
            return base.GetFinalBenchmarkString();
        }

        protected override void BenchmarkThreadRoutine(object CommandLine) {
            Thread.Sleep(ConfigManager.GeneralConfig.MinerRestartDelayMS * 3); // increase wait for sgminer

            BenchmarkSignalQuit = false;
            BenchmarkSignalHanged = false;
            BenchmarkSignalFinnished = false;
            BenchmarkException = null;

            try {
                Helpers.ConsolePrint("BENCHMARK", "Benchmark starts");
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);
                BenchmarkThreadRoutineStartSettup();
                // wait a little longer then the benchmark routine if exit false throw
                //var timeoutTime = BenchmarkTimeoutInSeconds(BenchmarkTimeInSeconds);
                //var exitSucces = BenchmarkHandle.WaitForExit(timeoutTime * 1000);
                // don't use wait for it breaks everything
                BenchmarkProcessStatus = BenchmarkProcessStatus.Running;
                while(true) {
                    string outdata = BenchmarkHandle.StandardOutput.ReadLine();
                    BenchmarkOutputErrorDataReceivedImpl(outdata);
                    // terminate process situations
                    if (BenchmarkSignalQuit
                        || BenchmarkSignalFinnished
                        || BenchmarkSignalHanged
                        || BenchmarkSignalTimedout
                        || BenchmarkException != null) {
                        //EndBenchmarkProcces();
                        // this is safe in a benchmark
                        KillSGMiner();
                        if (BenchmarkSignalTimedout) {
                            throw new Exception("Benchmark timedout");
                        }
                        if (BenchmarkException != null) {
                            throw BenchmarkException;
                        }
                        if (BenchmarkSignalQuit) {
                            throw new Exception("Termined by user request");
                        }
                        if (BenchmarkSignalHanged) {
                            throw new Exception("SGMiner is not responding");
                        }
                        if (BenchmarkSignalFinnished) {
                            break;
                        }
                    } else {
                        // wait a second reduce CPU load
                        Thread.Sleep(1000);
                    }
                }
            } catch (Exception ex) {
                BenchmarkThreadRoutineCatch(ex);
            } finally {
                BenchmarkThreadRoutineFinish();
            }
        }

        #endregion // Decoupled benchmarking routines

        // TODO _currentMinerReadStatus
        public override APIData GetSummary() {
            string resp;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            resp = GetAPIData(APIPort, "summary");
            if (resp == null) {
                _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                return null;
            }
            //// sgminer debug log
            //Helpers.ConsolePrint("sgminer-DEBUG_resp", resp);

            try {
                // Checks if all the GPUs are Alive first
                string resp2 = GetAPIData(APIPort, "devs");
                if (resp2 == null) {
                    _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                    return null;
                }
                //// sgminer debug log
                //Helpers.ConsolePrint("sgminer-DEBUG_resp2", resp2);

                string[] checkGPUStatus = resp2.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < checkGPUStatus.Length - 1; i++) {
                    if (checkGPUStatus[i].Contains("Enabled=Y") && !checkGPUStatus[i].Contains("Status=Alive")) {
                        Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " GPU " + i + ": Sick/Dead/NoStart/Initialising/Disabled/Rejecting/Unknown");
                        _currentMinerReadStatus = MinerAPIReadStatus.WAIT;
                        return null;
                    }
                }

                string[] resps = resp.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (resps[1].Contains("SUMMARY")) {
                    string[] data = resps[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // Get miner's current total speed
                    string[] speed = data[4].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    // Get miner's current total MH
                    double total_mh = Double.Parse(data[18].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1], new CultureInfo("en-US"));

                    ad.Speed = Double.Parse(speed[1]) * 1000;

                    if (total_mh <= PreviousTotalMH) {
                        Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " SGMiner might be stuck as no new hashes are being produced");
                        Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Prev Total MH: " + PreviousTotalMH + " .. Current Total MH: " + total_mh);
                        _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                        return null;
                    }

                    PreviousTotalMH = total_mh;
                } else {
                    ad.Speed = 0;
                }
            } catch {
                _currentMinerReadStatus = MinerAPIReadStatus.NONE;
                return null;
            }

            _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
            // check if speed zero
            if (ad.Speed == 0) _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;

            return ad;
        }
    }
}
