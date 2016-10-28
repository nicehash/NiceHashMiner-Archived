using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class nheqminer : Miner {
        private readonly int AMD_OCL_PLATFORM;

        public nheqminer()
            : base(DeviceType.ALL, "nheqminer") {
                Path = MinerPaths.nheqminer;
                AMD_OCL_PLATFORM = ComputeDeviceQueryManager.Instance.AMDOpenCLPlatformNum;
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            // TODO
            ProcessHandle = _Start();
        }

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // TODO
            return "";
        }

        protected override string GetDevicesCommandString() {
            // TODO
            string deviceStringCommand = " ";

            List<string> ids = new List<string>();
            foreach (var cdev in CDevs) {
                ids.Add(cdev.ID.ToString());
            }
            return deviceStringCommand;
        }

        public override APIData GetSummary() {
            // TODO
            APIData ad = new APIData();

            FillAlgorithm("equihash", ref ad);
            

            _currentMinerReadStatus = MinerAPIReadStatus.NONE;
            return null;
        }

        // benchmark stuff
        protected override bool BenchmarkParseLine(string outdata) {
            // TODO

            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
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
            _supportedMinerAlgorithms = new AlgorithmType[] { AlgorithmType.DaggerHashimoto };
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000; // 1 minute max, whole waiting time 75seconds
        }

        // STUBS
        public override string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename, bool isOptimized) {
            return MinerPaths.nheqminer;
        }
    }
}
