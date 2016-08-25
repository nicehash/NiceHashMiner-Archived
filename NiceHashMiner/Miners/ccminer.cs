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
        public ccminer() : base() { }

        // cryptonight benchmark exception
        int _cryptonightTotalCount = 0;
        double _cryptonightTotal = 0;

        public override void Start(Algorithm miningAlgorithm, string url, string username)
        {
            //if (ProcessHandle != null) return; // ignore, already running 

            CurrentMiningAlgorithm = miningAlgorithm;
            if (miningAlgorithm == null) return;

            string algo = "";
            string apiBind = "";
            if (CurrentMiningAlgorithm.NiceHashID != AlgorithmType.CryptoNight) {
                algo = "--algo=" + miningAlgorithm.MinerName;
                apiBind = " --api-bind=" + APIPort.ToString();
            }

            LastCommandLine = algo +
                                  " --url=" + url +
                                  " --userpass=" + username + ":" + GetPassword(miningAlgorithm) +
                                  apiBind +
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

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time) {
            string timeLimit = algorithm.NiceHashID == AlgorithmType.CryptoNight ? "" : " --time-limit " + time.ToString();
            string CommandLine = " --algo=" + algorithm.MinerName +
                              " --benchmark" +
                              timeLimit +
                              " " + ExtraLaunchParameters +
                              " " + algorithm.ExtraLaunchParameters +
                              " --devices ";

            CommandLine += GetDevicesCommandString();

            Path = GetOptimizedMinerPath(algorithm.NiceHashID);

            // cryptonight exception helper variables
            _cryptonightTotalCount = BenchmarkTimeInSeconds / 2;
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
                    _cryptonightTotal += tmp;
                    _cryptonightTotalCount--;
                }
                if (_cryptonightTotalCount <= 0) {
                    double spd = _cryptonightTotal / (BenchmarkTimeInSeconds / 2);
                    BenchmarkAlgorithm.BenchmarkSpeed = spd;
                    BenchmarkSignalFinnished = true;
                }
            }

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
            // CryptoNight does not have api bind port
            if (CurrentAlgorithmType == AlgorithmType.CryptoNight) {
                // check if running
                try {
                    var runningProcess = Process.GetProcessById(ProcessHandle.Id);
                } catch (ArgumentException ex) {
                    //Restart();
                    return null; // will restart outside
                } catch (InvalidOperationException ex) {
                    //Restart();
                    return null; // will restart outside
                }
                // extra check
                if (CurrentMiningAlgorithm == null) {
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
                return CryptoNightData;
            }
            return GetSummaryCPU_CCMINER();
        }

    }
}
