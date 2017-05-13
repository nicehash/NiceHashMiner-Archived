using Newtonsoft.Json;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace NiceHashMiner.Miners {
    public abstract class nheqBase : Miner {
        protected MiningSetup CPU_Setup = new MiningSetup(null);
        protected MiningSetup NVIDIA_Setup = new MiningSetup(null);
        protected readonly int AMD_OCL_PLATFORM;
        protected MiningSetup AMD_Setup = new MiningSetup(null);

        // extra benchmark stuff
        protected double curSpeed = 0;
        static protected readonly String Iter_PER_SEC = "I/s";
        static protected readonly String Sols_PER_SEC = "Sols/s";
        protected const double SolMultFactor = 1.9;

        private class Result {
            public double interval_seconds { get; set; }
            public double speed_ips { get; set; }
            public double speed_sps { get; set; }
            public double accepted_per_minute { get; set; }
            public double rejected_per_minute { get; set; }
        }

        private class JsonApiResponse {
            public string method { get; set; }
            public Result result { get; set; }
            public object error { get; set; }
        }

        public nheqBase(string minerDeviceName)
            : base(minerDeviceName) {
                AMD_OCL_PLATFORM = ComputeDeviceManager.Avaliable.AMDOpenCLPlatformNum;
        }

        public override void InitMiningSetup(MiningSetup miningSetup) {
            base.InitMiningSetup(miningSetup);
            List<MiningPair> CPUs = new List<MiningPair>();
            List<MiningPair> NVIDIAs = new List<MiningPair>();
            List<MiningPair> AMDs = new List<MiningPair>();
            foreach (var pairs in MiningSetup.MiningPairs) {
                if (pairs.Device.DeviceType == DeviceType.CPU) {
                    CPUs.Add(pairs);
                }
                if (pairs.Device.DeviceType == DeviceType.NVIDIA) {
                    NVIDIAs.Add(pairs);
                }
                if (pairs.Device.DeviceType == DeviceType.AMD) {
                    AMDs.Add(pairs);
                }
            }
            // reinit
            CPU_Setup = new MiningSetup(CPUs);
            NVIDIA_Setup = new MiningSetup(NVIDIAs);
            AMD_Setup = new MiningSetup(AMDs);
        }

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // TODO nvidia extras
            String ret = "-b " + GetDevicesCommandString();
            return ret;
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
                ad.Speed = resp.result.speed_sps;
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
