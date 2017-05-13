using Newtonsoft.Json;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace NiceHashMiner.Miners {
    public class excavator : Miner {

        private class DeviceStat {
            public int id { get; set; }
            public string name { get; set; }
            public double speed_hps { get; set; }
        }

        private class Result {
            public bool connected { get; set; }
            public double interval_seconds { get; set; }
            public double speed_hps { get; set; }
            public List<DeviceStat> devices { get; set; }
            public double accepted_per_minute { get; set; }
            public double rejected_per_minute { get; set; }
        }

        private class JsonApiResponse {
            public string method { get; set; }
            public Result result { get; set; }
            public object error { get; set; }
        }

        public excavator()
            : base("excavator") {
                ConectionType = NHMConectionType.NONE;
                IsNeverHideMiningWindow = true;
        }

        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = GetDevicesCommandString() + " -a " + this.MiningSetup.MinerName + " -p " + APIPort + " -s " + url + " -u " + username + ":x";
            ProcessHandle = _Start();
        }

        protected override string GetDevicesCommandString() {
            List<MiningPair> CT_MiningPairs = new List<MiningPair>();
            string deviceStringCommand = " -cd ";
            int default_CT = 1;
            if(this.MiningSetup.CurrentAlgorithmType == AlgorithmType.Equihash) {
                default_CT = 2;
            } 
            foreach (var nvidia_pair in this.MiningSetup.MiningPairs) {
                if (nvidia_pair.CurrentExtraLaunchParameters.Contains("-ct")) {
                    for (int i = 0; i < ExtraLaunchParametersParser.GetEqmCudaThreadCount(nvidia_pair); ++i) {
                        deviceStringCommand += nvidia_pair.Device.ID + " ";
                        CT_MiningPairs.Add(nvidia_pair);
                    }
                } else { // use default default_CT for best performance
                    for (int i = 0; i < default_CT; ++i) {
                        deviceStringCommand += nvidia_pair.Device.ID + " ";
                        CT_MiningPairs.Add(nvidia_pair);
                    }
                }
            }
            
            MiningSetup CT_MiningSetup = new MiningSetup(CT_MiningPairs);
            //deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForMiningSetup(this.MiningSetup, DeviceType.NVIDIA);
            deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForMiningSetup(CT_MiningSetup, DeviceType.NVIDIA);

            return deviceStringCommand;
        }

        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            string ret = " -a " + this.MiningSetup.MinerName + " -b " + time + " " + GetDevicesCommandString();
            return ret;
        }

        const string TOTAL_MES = "Total measured:";
        protected override bool BenchmarkParseLine(string outdata) {

            if (outdata.Contains(TOTAL_MES)) {
                try {
                    int speedStart = outdata.IndexOf(TOTAL_MES);
                    string speed = outdata.Substring(speedStart, outdata.Length - speedStart).Replace(TOTAL_MES, "");
                    var splitSrs = speed.Trim().Split(' ');
                    if (splitSrs.Length >= 2) {
                        string speedStr = splitSrs[0];
                        string postfixStr = splitSrs[1];
                        double spd = Double.Parse(speedStr, CultureInfo.InvariantCulture);
                        if (postfixStr.Contains("kH/s"))
                            spd *= 1000;
                        else if (postfixStr.Contains("MH/s"))
                            spd *= 1000000;
                        else if (postfixStr.Contains("GH/s"))
                            spd *= 1000000000;

                        // wrong benchmark workaround over 3gh/s is considered false
                        if (this.MiningSetup.CurrentAlgorithmType == AlgorithmType.Pascal
                            && spd > 3.0d * 1000000000.0d
                            ) {
                            return false;
                        }

                        BenchmarkAlgorithm.BenchmarkSpeed = spd;
                        return true;
                    }
                } catch {
                }
            }
            return false;
        }

        public override APIData GetSummary() {
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            TcpClient client = null;
            JsonApiResponse resp = null;
            try {
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("status\n");
                client = new TcpClient("127.0.0.1", APIPort);
                NetworkStream nwStream = client.GetStream();
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                string respStr = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                resp = JsonConvert.DeserializeObject<JsonApiResponse>(respStr, Globals.JsonSettings);
                client.Close();
            } catch (Exception ex) {
                Helpers.ConsolePrint("ERROR", ex.Message);
            }

            if (resp != null && resp.error == null) {
                ad.Speed = resp.result.speed_hps;
                _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
                if (ad.Speed == 0) {
                    _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;
                }
            }

            return ad;
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000 * 5; // 5 minute max, whole waiting time 75seconds
        }

        // benchmark stuff

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }
    }
}
