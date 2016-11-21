using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class ClaymoreZcashMiner : Miner {
        public ClaymoreZcashMiner()
            : base("ClaymoreZcashMiner") {
            Path = MinerPaths.ClaymoreZcashMiner;
            WorkingDirectory = MinerPaths.ClaymoreZcashMiner.Replace("ZecMiner64.exe", "");
        }
        const int BENCHMARK_MAX_GET = 10;
        const string LOOK_FOR_START = "ZEC - Total Speed:";
        const string LOOK_FOR_END = " H/s";
        int benchmark_read_count = 0;
        double benchmark_sum = 0.0d;


        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = GetDevicesCommandString() + " -mport " + APIPort + " -zpool " + url + " -zwal " + username;
            ProcessHandle = _Start();
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000 * 5; // 5 minute max, whole waiting time 75seconds
        }

        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            // TODO
            //const string MASK = "-a {0}";
            //var oldApiBindStr = String.Format(MASK, oldPort);
            //var newApiBindStr = String.Format(MASK, newPort);
            //if (LastCommandLine != null && LastCommandLine.Contains(oldApiBindStr)) {
            //    LastCommandLine = LastCommandLine.Replace(oldApiBindStr, newApiBindStr);
            //    return true;
            //}
            return false;
        }

        protected override void _Stop(MinerStopType willswitch) {
            // STOP
            //Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        public override APIData GetSummary() {
            // TODO
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            APIData ad = new APIData(MiningSetup.CurrentAlgorithmType);

            //TcpClient client = null;
            //JsonApiResponse resp = null;
            //try {
            //    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("status\n");
            //    client = new TcpClient("127.0.0.1", APIPort);
            //    NetworkStream nwStream = client.GetStream();
            //    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            //    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            //    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            //    string respStr = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            //    resp = JsonConvert.DeserializeObject<JsonApiResponse>(respStr, Globals.JsonSettings);
            //    client.Close();
            //} catch (Exception ex) {
            //    Helpers.ConsolePrint("ERROR", ex.Message);
            //}

            //if (resp != null && resp.error == null) {
            //    ad.Speed = resp.result.speed_sps;
            //    _currentMinerReadStatus = MinerAPIReadStatus.GOT_READ;
            //    if (ad.Speed == 0) {
            //        _currentMinerReadStatus = MinerAPIReadStatus.READ_SPEED_ZERO;
            //    }
            //} else if (resp == null) {
            //    _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            //}

            return ad;
        }

        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " -di ";
            List<int> ids = new List<int>();
            foreach (var mPair in MiningSetup.MiningPairs) {
                ids.Add(mPair.Device.ID);
            }
            deviceStringCommand += string.Join("", ids);

            return deviceStringCommand;
        }

        // benchmark stuff
        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            String ret = "-benchmark 1 " + GetDevicesCommandString();
            return ret;
        }
        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
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
