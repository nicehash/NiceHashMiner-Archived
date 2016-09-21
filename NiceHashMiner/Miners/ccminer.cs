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
        public ccminer(string minerDeviceName) : base(DeviceType.NVIDIA, minerDeviceName) { }

        // cryptonight benchmark exception
        int _cryptonightTotalCount = 0;
        double _cryptonightTotal = 0;
        const int _cryptonightTotalDelim = 2;

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000; // 1 minute max, whole waiting time 75seconds
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username)
        {
            CurrentMiningAlgorithm = miningAlgorithm;
            if (miningAlgorithm == null) return;

            string algo = "";
            string apiBind = "";
            if (CurrentMiningAlgorithm.NiceHashID != AlgorithmType.CryptoNight) {
                algo = "--algo=" + miningAlgorithm.MinerName;
                apiBind = " --api-bind=" + APIPort.ToString();
            }

            IsAPIReadException = CurrentMiningAlgorithm.NiceHashID == AlgorithmType.CryptoNight;

            LastCommandLine = algo +
                                  " --url=" + url +
                                  " --userpass=" + username + ":" + Algorithm.PasswordDefault +
                                  apiBind + " " +
                                  ExtraLaunchParametersParser.ParseForCDevs(
                                                                CDevs,
                                                                CurrentMiningAlgorithm.NiceHashID,
                                                                DeviceType.NVIDIA) +
                                  " --devices ";

            LastCommandLine += GetDevicesCommandString();

            Path = GetOptimizedMinerPath(miningAlgorithm.NiceHashID);

            ProcessHandle = _Start();
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer(willswitch);
        }

        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            return UpdateBindPortCommand_ccminer_cpuminer(oldPort, newPort);
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            string timeLimit = algorithm.NiceHashID == AlgorithmType.CryptoNight ? "" : " --time-limit " + time.ToString();
            string CommandLine = " --algo=" + algorithm.MinerName +
                              " --benchmark" +
                              timeLimit + " " +
                              ExtraLaunchParametersParser.ParseForCDevs(
                                                                CDevs,
                                                                algorithm.NiceHashID,
                                                                DeviceType.NVIDIA) +
                              " --devices ";

            CommandLine += GetDevicesCommandString();

            Path = GetOptimizedMinerPath(algorithm.NiceHashID);

            // cryptonight exception helper variables
            _cryptonightTotalCount = BenchmarkTimeInSeconds / _cryptonightTotalDelim;
            _cryptonightTotal = 0.0d;

            return CommandLine;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            // cryptonight exception
            if (BenchmarkAlgorithm.NiceHashID == AlgorithmType.CryptoNight) {
                if (outdata.Contains("Total: ")) {
                    int st = outdata.IndexOf("Total:") + 7;
                    int len = outdata.Length - 6 - st;

                    string parse = outdata.Substring(st, len).Trim();
                    double tmp;
                    Double.TryParse(parse, NumberStyles.Any, CultureInfo.InvariantCulture, out tmp);

                    // save speed
                    int i = outdata.IndexOf("Benchmark:");
                    int k = outdata.IndexOf("/s");
                    string hashspeed = outdata.Substring(i + 11, k - i - 9);
                    int b = hashspeed.IndexOf(" ");
                    if (hashspeed.Contains("kH/s"))
                        tmp *= 1000;
                    else if (hashspeed.Contains("MH/s"))
                        tmp *= 1000000;
                    else if (hashspeed.Contains("GH/s"))
                        tmp *= 1000000000;

                    _cryptonightTotal += tmp;
                    _cryptonightTotalCount--;
                }
                if (_cryptonightTotalCount <= 0) {
                    double spd = _cryptonightTotal / (BenchmarkTimeInSeconds / _cryptonightTotalDelim);
                    BenchmarkAlgorithm.BenchmarkSpeed = spd;
                    BenchmarkSignalFinnished = true;
                }
            }

            double lastSpeed = BenchmarkParseLine_cpu_ccminer_extra(outdata);
            if (lastSpeed > 0.0d) {
                BenchmarkAlgorithm.BenchmarkSpeed = lastSpeed;
                return true;
            }

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
            // CryptoNight does not have api bind port
            if (CurrentAlgorithmType == AlgorithmType.CryptoNight) {
                // check if running
                if (ProcessHandle == null) {
                    _currentMinerReadStatus = MinerAPIReadStatus.RESTART;
                    Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Could not read data from CryptoNight Proccess is null");
                    return null;
                }
                try {
                    var runningProcess = Process.GetProcessById(ProcessHandle.Id);
                } catch (ArgumentException ex) {
                    _currentMinerReadStatus = MinerAPIReadStatus.RESTART;
                    Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Could not read data from CryptoNight");
                    return null; // will restart outside
                } catch (InvalidOperationException ex) {
                    _currentMinerReadStatus = MinerAPIReadStatus.RESTART;
                    Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Could not read data from CryptoNight");
                    return null; // will restart outside
                }
                // extra check
                if (CurrentMiningAlgorithm == null) {
                    _currentMinerReadStatus = MinerAPIReadStatus.RESTART;
                    Helpers.ConsolePrint(MinerTAG(), ProcessTag() + " Could not read data from CryptoNight Proccess CurrentMiningAlgorithm is NULL");
                    return null;
                }

                var totalSpeed = 0.0d;
                foreach (var cdev in CDevs) {
                    totalSpeed += cdev.DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.CryptoNight].BenchmarkSpeed;
                }

                APIData CryptoNightData = new APIData();
                CryptoNightData.AlgorithmID = AlgorithmType.CryptoNight;
                CryptoNightData.AlgorithmName = "cryptonight";
                CryptoNightData.Speed = totalSpeed;
                _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
                // check if speed zero
                if (CryptoNightData.Speed == 0) _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;
                return CryptoNightData;
            }
            return GetSummaryCPU_CCMINER();
        }

    }
}
