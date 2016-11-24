using Newtonsoft.Json;
using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class ClaymoreZcashMiner : Miner {
        public ClaymoreZcashMiner()
            : base("ClaymoreZcashMiner") {
            Path = MinerPaths.ClaymoreZcashMiner;
            WorkingDirectory = MinerPaths.ClaymoreZcashMiner.Replace("ZecMiner64.exe", "");
            //Path = WorkingDirectory + "benchmark.bat";

            IsKillAllUsedMinerProcs = true;
        }
        const int BENCHMARK_MAX_GET = 10;
        const string LOOK_FOR_START = "ZEC - Total Speed:";
        const string LOOK_FOR_END = " H/s";
        const string LOG_FILE_NAME = "cl_log_noappend.txt";
        int benchmark_read_count = 0;
        double benchmark_sum = 0.0d;

        private class JsonApiResponse {
            public List<string> result { get; set; }
            public int id { get; set; }
            public object error { get; set; }
        }

        public void KillZecMiner64() {
            foreach (Process process in Process.GetProcessesByName("ZecMiner64")) {
                try { process.Kill(); } catch (Exception e) { Helpers.ConsolePrint(MinerDeviceName, e.ToString()); }
            }
        }


        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = "-logfile cl_log_noappend.txt " + GetDevicesCommandString() + " -mport -" + APIPort + " -zpool " + url + " -zwal " + username;
            ProcessHandle = _Start();
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000 * 5; // 5 minute max, whole waiting time 75seconds
        }

        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            const string MASK = "-mport -{0}";
            var oldApiBindStr = String.Format(MASK, oldPort);
            var newApiBindStr = String.Format(MASK, newPort);
            if (LastCommandLine != null && LastCommandLine.Contains(oldApiBindStr)) {
                LastCommandLine = LastCommandLine.Replace(oldApiBindStr, newApiBindStr);
                return true;
            }
            return false;
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        public override APIData GetSummary() {
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            TcpClient client = null;
            JsonApiResponse resp = null;
            try {
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("{\"id\":0,\"jsonrpc\":\"2.0\",\"method\":\"miner_getstat1\"}n");
                client = new TcpClient("127.0.0.1", APIPort);
                NetworkStream nwStream = client.GetStream();
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                string respStr = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                resp = JsonConvert.DeserializeObject<JsonApiResponse>(respStr, Globals.JsonSettings);
                client.Close();
                //Helpers.ConsolePrint("ClaymoreZcashMiner API back:", respStr);
            } catch (Exception ex) {
                Helpers.ConsolePrint("ClaymoreZcashMiner", "GetSummary exception: " + ex.Message);
            }

            if (resp != null && resp.error == null) {
                //Helpers.ConsolePrint("ClaymoreZcashMiner API back:", "resp != null && resp.error == null");
                if (resp.result != null && resp.result.Count > 4) {
                    //Helpers.ConsolePrint("ClaymoreZcashMiner API back:", "resp.result != null && resp.result.Count > 4");
                    var speeds = resp.result[3].Split(';');
                    ad.Speed = 0;
                    foreach (var speed in speeds) {
                        //Helpers.ConsolePrint("ClaymoreZcashMiner API back:", "foreach (var speed in speeds) {");
                        double tmpSpeed = 0;
                        try {
                            tmpSpeed = Double.Parse(speed, CultureInfo.InvariantCulture);
                        } catch {
                            tmpSpeed = 0;
                        }
                        ad.Speed += tmpSpeed;
                    }
                    _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
                }
                if (ad.Speed == 0) {
                    _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;
                }
            }

            return ad;
        }

        protected override string GetDevicesCommandString() {
            string extraParams = ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.AMD);
            string deviceStringCommand = " -di ";
            List<int> ids = new List<int>();
            foreach (var mPair in MiningSetup.MiningPairs) {
                ids.Add(mPair.Device.ID);
            }
            deviceStringCommand += string.Join("", ids);

            return deviceStringCommand + extraParams;
        }

        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            string ret = "-logfile " + LOG_FILE_NAME + " -benchmark 1 " + GetDevicesCommandString();
            return ret;
        }
        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        protected override void BenchmarkThreadRoutine(object CommandLine) {
            Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);

            BenchmarkSignalQuit = false;
            BenchmarkSignalHanged = false;
            BenchmarkSignalFinnished = false;
            BenchmarkException = null;

            try {
                Helpers.ConsolePrint("BENCHMARK", "Benchmark starts");
                BenchmarkHandle = BenchmarkStartProcess((string)CommandLine);
                Stopwatch _benchmarkTimer = new Stopwatch();
                _benchmarkTimer.Reset();
                _benchmarkTimer.Start();
                //BenchmarkThreadRoutineStartSettup();
                // wait a little longer then the benchmark routine if exit false throw
                //var timeoutTime = BenchmarkTimeoutInSeconds(BenchmarkTimeInSeconds);
                //var exitSucces = BenchmarkHandle.WaitForExit(timeoutTime * 1000);
                // don't use wait for it breaks everything
                BenchmarkProcessStatus = BenchmarkProcessStatus.Running;
                while (true) {
                    //string outdata = BenchmarkHandle.StandardOutput.ReadLine();
                    //BenchmarkOutputErrorDataReceivedImpl(outdata);
                    // terminate process situations
                    if (_benchmarkTimer.Elapsed.Seconds >= 45 + 2
                        || BenchmarkSignalQuit
                        || BenchmarkSignalFinnished
                        || BenchmarkSignalHanged
                        || BenchmarkSignalTimedout
                        || BenchmarkException != null) {
                        // maybe will have to KILL process
                        KillZecMiner64();
                        if (BenchmarkSignalTimedout) {
                            throw new Exception("Benchmark timedout");
                        }
                        if (BenchmarkException != null) {
                            throw BenchmarkException;
                        }
                        if (BenchmarkSignalQuit) {
                            throw new Exception("Termined by user request");
                        }
                        if (BenchmarkSignalFinnished) {
                            break;
                        }
                        if (_benchmarkTimer.Elapsed.Seconds >= 30) {
                            break;
                        }
                    }
                }
            } catch (Exception ex) {
                BenchmarkAlgorithm.BenchmarkSpeed = 0;
                Helpers.ConsolePrint(MinerTAG(), "Benchmark Exception: " + ex.Message);
                if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                    OnBenchmarkCompleteCalled = true;
                    BenchmarkComunicator.OnBenchmarkComplete(false, BenchmarkSignalTimedout ? International.GetText("Benchmark_Timedout") : International.GetText("Benchmark_Terminated"));
                }
            } finally {
                BenchmarkAlgorithm.BenchmarkSpeed = 0;
                BenchmarkProcessStatus = BenchmarkProcessStatus.Finished;
                // read file log
                if(File.Exists(WorkingDirectory + LOG_FILE_NAME)) {
                    var lines = File.ReadAllLines(WorkingDirectory + LOG_FILE_NAME);
                    foreach (var line in lines) {
                        if (line != null && line.Contains(LOOK_FOR_START)) {
                            benchmark_sum += getNumber(line);
                            ++benchmark_read_count;
                        }
                    }
                    if (benchmark_read_count > 0) {
                        BenchmarkAlgorithm.BenchmarkSpeed = benchmark_sum / benchmark_read_count;
                        BenchmarkProcessStatus = BenchmarkProcessStatus.Success;
                    }
                }
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + Helpers.FormatSpeedOutput(BenchmarkAlgorithm.BenchmarkSpeed));
                Helpers.ConsolePrint("BENCHMARK", "Benchmark ends");
                if (BenchmarkComunicator != null && !OnBenchmarkCompleteCalled) {
                    OnBenchmarkCompleteCalled = true;
                    BenchmarkComunicator.OnBenchmarkComplete(true, "Success");
                }
            }
        }

        double getNumber(string outdata) {
            try {
                int speedStart = outdata.IndexOf(LOOK_FOR_START);
                String speed = outdata.Substring(speedStart, outdata.Length - speedStart);
                speed = speed.Replace(LOOK_FOR_START, "");
                speed = speed.Substring(0, speed.IndexOf(LOOK_FOR_END));
                speed = speed.Trim();
                return Double.Parse(speed, CultureInfo.InvariantCulture);
            } catch {
            }
            return 0;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            Helpers.ConsolePrint("BENCHMARK", outdata);
            if (outdata.Contains(LOOK_FOR_START)) {
                benchmark_sum += getNumber(outdata);
                ++benchmark_read_count;
                if (benchmark_read_count == BENCHMARK_MAX_GET) {
                    BenchmarkAlgorithm.BenchmarkSpeed = benchmark_sum / BENCHMARK_MAX_GET;
                    return true;
                }
            }
            return false;
        }
    }
}
