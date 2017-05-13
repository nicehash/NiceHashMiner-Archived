using Newtonsoft.Json;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NiceHashMiner.Miners.Equihash {
    public class OptiminerZcashMiner : Miner {
        public OptiminerZcashMiner()
            : base("OptiminerZcashMiner") {
            ConectionType = NHMConectionType.NONE;
        }

        private class Stratum {
            public string target { get; set; }
            public bool connected { get; set; }
            public int connection_failures { get; set; }
            public string host { get; set; }
            public int port { get; set; }
        }

        private class JsonApiResponse {
            public double uptime;
            public Dictionary<string, Dictionary<string, double>> solution_rate;
            public Dictionary<string, double> share;
            public Dictionary<string, Dictionary<string, double>> iteration_rate;
            public Stratum stratum;
        }

        // give some time or else it will crash
        Stopwatch _startAPI = null;
        bool _skipAPICheck = true;
        int waitSeconds = 30;

        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = " " + GetDevicesCommandString() + " -m " + APIPort + " -s " + url + " -u " + username + " -p x";
            ProcessHandle = _Start();

            //
            _startAPI = new Stopwatch();
            _startAPI.Start();
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000 * 5; // 5 minute max, whole waiting time 75seconds
        }

        protected override string GetDevicesCommandString() {
            string extraParams = ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.AMD);
            string deviceStringCommand = " -c " + ComputeDeviceManager.Avaliable.AMDOpenCLPlatformNum;
            deviceStringCommand += " ";
            List<string> ids = new List<string>();
            foreach (var mPair in MiningSetup.MiningPairs) {
                ids.Add("-d " + mPair.Device.ID.ToString());
            }
            deviceStringCommand += StringHelper.Join(" ", ids);

            return deviceStringCommand + extraParams;
        }

        public override APIData GetSummary() {
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            if (_skipAPICheck == false) {
                JsonApiResponse resp = null;
                try {
                    string DataToSend = GetHttpRequestNHMAgentStrin("");
                    string respStr = GetAPIData(APIPort, DataToSend, true);
                    if (respStr != null && respStr.Contains("{")) {
                        int start = respStr.IndexOf("{");
                        if (start > -1) {
                            string respStrJSON = respStr.Substring(start);
                            resp = JsonConvert.DeserializeObject<JsonApiResponse>(respStrJSON.Trim(), Globals.JsonSettings);
                        }
                    }
                    //Helpers.ConsolePrint("OptiminerZcashMiner API back:", respStr);
                } catch (Exception ex) {
                    Helpers.ConsolePrint("OptiminerZcashMiner", "GetSummary exception: " + ex.Message);
                }

                if (resp != null && resp.solution_rate != null) {
                    //Helpers.ConsolePrint("OptiminerZcashMiner API back:", "resp != null && resp.error == null");
                    const string total_key = "Total";
                    const string _5s_key = "5s";
                    if (resp.solution_rate.ContainsKey(total_key)) {
                        var total_solution_rate_dict = resp.solution_rate[total_key];
                        if (total_solution_rate_dict != null && total_solution_rate_dict.ContainsKey(_5s_key)) {
                            ad.Speed = total_solution_rate_dict[_5s_key];
                            _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
                        }
                    }
                    if (ad.Speed == 0) {
                        _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;
                    }
                }
            } else if (_skipAPICheck && _startAPI.Elapsed.TotalSeconds > waitSeconds) {
                _startAPI.Stop();
                _skipAPICheck = false;
            }

            return ad;
        }

        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            int t = time / 9; // sgminer needs 9 times more than this miner so reduce benchmark speed
            string ret = " " + GetDevicesCommandString() + " --benchmark " + t;
            return ret;
        }
        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        protected override bool BenchmarkParseLine(string outdata) {
            const string FIND = "Benchmark:";
            if (outdata.Contains(FIND)) {
                int start = outdata.IndexOf("Benchmark:") + FIND.Length;
                string itersAndVars = outdata.Substring(start).Trim();
                var ar = itersAndVars.Split(new char[] { ' ' });
                if (ar.Length >= 4) {
                    // gets sols/s
                    BenchmarkAlgorithm.BenchmarkSpeed = Helpers.ParseDouble(ar[2]);
                    return true;
                }
            }
            return false;
        }

    }
}
