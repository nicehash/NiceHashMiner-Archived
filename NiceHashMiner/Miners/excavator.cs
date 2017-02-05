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

        private class DviceStat {
            public int id { get; set; }
            public string name { get; set; }
            public double speed_hps { get; set; }
        }

        private class Result {
            public bool connected { get; set; }
            public double interval_seconds { get; set; }
            public double speed_hps { get; set; }
            public List<DviceStat> devices { get; set; }
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

        public override void InitMiningSetup(MiningSetup miningSetup) {
            base.InitMiningSetup(miningSetup);
        }

        protected override string GetDevicesCommandString() {
            Path = this.MiningSetup.MinerPath;
            WorkingDirectory = this.MiningSetup.MinerPath.Replace("excavator.exe", "");

            string deviceStringCommand = " -cd ";

            foreach (var nvidia_pair in this.MiningSetup.MiningPairs) {
                deviceStringCommand += nvidia_pair.Device.ID + " ";
            }
            //// no extra launch params
            //deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForMiningSetup(NVIDIA_Setup, DeviceType.NVIDIA);

            return deviceStringCommand;
        }

        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            String ret = "-b " + time + " " + GetDevicesCommandString();
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

        protected double getNumber(string outdata, string startF, string remF) {
            try {
                int speedStart = outdata.IndexOf(startF);
                String speed = outdata.Substring(speedStart, outdata.Length - speedStart);
                speed = speed.Replace(startF, "");
                speed = speed.Replace(remF, "");
                speed = speed.Trim();
                return Double.Parse(speed, CultureInfo.InvariantCulture);
            } catch {
            }
            return 0;
        }

        // benchmark stuff

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }
    }
}
