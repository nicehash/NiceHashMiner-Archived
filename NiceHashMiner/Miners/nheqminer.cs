using Newtonsoft.Json;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class nheqminer : Miner {
        private readonly int AMD_OCL_PLATFORM;

        List<ComputeDevice> CPUs = new List<ComputeDevice>();
        List<ComputeDevice> NVIDIAs = new List<ComputeDevice>();
        List<ComputeDevice> AMDs = new List<ComputeDevice>();

        // extra benchmark stuff
        double curSpeed = 0;

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

        public nheqminer()
            : base(DeviceType.ALL, "nheqminer") {
                Path = MinerPaths.nheqminer;
                WorkingDirectory = MinerPaths.nheqminer.Replace("nheqminer.exe", "");
                AMD_OCL_PLATFORM = ComputeDeviceQueryManager.Instance.AMDOpenCLPlatformNum;
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            CurrentMiningAlgorithm = miningAlgorithm;
            LastCommandLine = GetDevicesCommandString() + " -a " + APIPort + " -l " + url + " -u " + username;
            ProcessHandle = _Start();
        }

        public override void SetCDevs(string[] deviceUUIDs) {
            base.SetCDevs(deviceUUIDs);
            foreach (var cDev in CDevs) {
                if(cDev.DeviceType == DeviceType.CPU) {
                    CPUs.Add(cDev);
                }
                if (cDev.DeviceType == DeviceType.NVIDIA) {
                    NVIDIAs.Add(cDev);
                }
                if (cDev.DeviceType == DeviceType.AMD) {
                    AMDs.Add(cDev);
                }
            }
        }

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // TODO nvidia extras
            CurrentMiningAlgorithm = algorithm;
            String ret = "-b " + GetDevicesCommandString();
            return ret;
        }

        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            if (CPUs.Count > 0) {
                if (CPUs[0].MostProfitableAlgorithm.LessThreads > 0 || !string.IsNullOrEmpty(CPUs[0].MostProfitableAlgorithm.ExtraLaunchParameters)) {
                    // TODO parse
                    deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(CPUs, AlgorithmType.Equihash, DeviceType.CPU);
                }
            } else {
                // disable CPU
                deviceStringCommand += " -t 0 ";
            }

            if (NVIDIAs.Count > 0) {
                deviceStringCommand += " -cd ";
                foreach (var nvidia in NVIDIAs) {
                    deviceStringCommand += nvidia.ID + " ";
                }
                // use always -cv 1
                deviceStringCommand += " -cv 1";
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(NVIDIAs, AlgorithmType.Equihash, DeviceType.NVIDIA);
            }

            if (AMDs.Count > 0) {
                deviceStringCommand += " -op " + AMD_OCL_PLATFORM.ToString();
                deviceStringCommand += " -od ";
                foreach (var amd in AMDs) {
                    deviceStringCommand += amd.ID + " ";
                }
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(AMDs, AlgorithmType.Equihash, DeviceType.AMD);
            }

            return deviceStringCommand;
        }

        
        public override APIData GetSummary() {
            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            APIData ad = new APIData();
            ad.AlgorithmID = AlgorithmType.Equihash;
            ad.AlgorithmName = "equihash";
            ad.Speed = 0;

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
            } catch(Exception ex) {
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

        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();
            if(CPUs.Count > 0 && CPUs[0].AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, CPUs[0].AffinityMask);

            return P;
        }

        // DONE
        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            const string MASK = "-a {0}";
            var oldApiBindStr = String.Format(MASK, oldPort);
            var newApiBindStr = String.Format(MASK, newPort);
            if (LastCommandLine != null && LastCommandLine.Contains(oldApiBindStr)) {
                LastCommandLine = LastCommandLine.Replace(oldApiBindStr, newApiBindStr);
                return true;
            }
            return false;
        }
        protected override void InitSupportedMinerAlgorithms() {
            _supportedMinerAlgorithms = new AlgorithmType[] { AlgorithmType.Equihash };
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000 * 5; // 5 minute max, whole waiting time 75seconds
        }

        private double getNumber(string outdata, string startF, string remF) {
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
        static private readonly String Iter_PER_SEC = "I/s";
        static private readonly String Sols_PER_SEC = "Sols/s";
        private const double SolMultFactor = 1.9;
        protected override bool BenchmarkParseLine(string outdata) {

            if (outdata.Contains(Iter_PER_SEC)) {
                curSpeed = getNumber(outdata, "Speed: ", Iter_PER_SEC) * SolMultFactor;
            }
            if (outdata.Contains(Sols_PER_SEC)) {
                var sols = getNumber(outdata, "Speed: ", Sols_PER_SEC);
                if (sols > 0) {
                    BenchmarkAlgorithm.BenchmarkSpeed = curSpeed;
                    return true;
                }
            }
            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        // STUBS
        public override string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename, bool isOptimized) {
            return MinerPaths.nheqminer;
        }
    }
}
