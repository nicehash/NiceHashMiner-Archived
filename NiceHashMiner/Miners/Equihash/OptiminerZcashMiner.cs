using Newtonsoft.Json;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Miners.Equihash {
    public class OptiminerZcashMiner : Miner {
        public OptiminerZcashMiner()
            : base("OptiminerZcashMiner") {
            Path = MinerPaths.OptiminerZcashMiner;
            WorkingDirectory = MinerPaths.OptiminerZcashMiner.Replace("Optiminer.exe", "");
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

        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = " " + GetDevicesCommandString() + " -m " + APIPort + " -s " + url + " -u " + username + " -p x";
            ProcessHandle = _Start();
        }

        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            const string MASK = "-m {0}";
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

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000 * 5; // 5 minute max, whole waiting time 75seconds
        }

        protected override string GetDevicesCommandString() {
            string extraParams = ""; //ExtraLaunchParametersParser.ParseForMiningSetup(MiningSetup, DeviceType.AMD);
            string deviceStringCommand = " -c " + ComputeDeviceManager.Avaliable.AMDOpenCLPlatformNum;
            deviceStringCommand += " -d ";
            List<string> ids = new List<string>();
            foreach (var mPair in MiningSetup.MiningPairs) {
                ids.Add(mPair.Device.ID.ToString());
            }
            deviceStringCommand += StringHelper.Join(" ", ids);

            return deviceStringCommand + extraParams;
        }

        public override APIData GetSummary() {
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            TcpClient client = null;
            JsonApiResponse resp = null;
            try {
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("status\r\n");
                client = new TcpClient("127.0.0.1", APIPort);
                //client.ReceiveTimeout = 500;
                //client.NoDelay = true;
                NetworkStream nwStream = client.GetStream();
                //nwStream.ReadTimeout = 5000;
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                if (nwStream.CanRead) {
                    do {
                        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                        string respStr = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                        Helpers.ConsolePrint("OptiminerZcashMiner", "respStr: " + respStr);
                    } while (nwStream.DataAvailable);
                } else {
                    Helpers.ConsolePrint("OptiminerZcashMiner API :", "nwStream.CanRead == false");
                }
                
                if (nwStream.DataAvailable == false) {
                    Helpers.ConsolePrint("OptiminerZcashMiner API :", "nwStream.DataAvailable == false");
                }

                //int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                //string respStr = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                //resp = JsonConvert.DeserializeObject<JsonApiResponse>(respStr, Globals.JsonSettings);
                client.Close();
                //Helpers.ConsolePrint("OptiminerZcashMiner API back:", respStr);
            } catch (Exception ex) {
                Helpers.ConsolePrint("OptiminerZcashMiner", "GetSummary exception: " + ex.Message);
            }

            if (resp != null && resp.solution_rate == null) {
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

            return ad;
        }

        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // TODO waiting api implementation
            string ret = "TODO";
            return ret;
        }
        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        protected override bool BenchmarkParseLine(string outdata) {
            Helpers.ConsolePrint("BENCHMARK", outdata);
            // TODO waiting api implementation
            return false;
        }

    }
}
