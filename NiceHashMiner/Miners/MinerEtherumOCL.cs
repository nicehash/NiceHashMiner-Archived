using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {

    // TODO for NOW ONLY AMD
    // AMD or TODO it could be something else
    public class MinerEtherumOCL : MinerEtherum {

        // reference to all MinerEtherumOCL make sure to clear this after miner Stop
        // we make sure only ONE instance of MinerEtherumOCL is running
        private static List<MinerEtherumOCL> MinerEtherumOCLList = new List<MinerEtherumOCL>();

        private readonly int GPUPlatformNumber;

        public MinerEtherumOCL()
            : base(DeviceType.AMD, "MinerEtherumOCL", "AMD OpenCL") {
            GPUPlatformNumber = ComputeDeviceQueryManager.Instance.AMDOpenCLPlatformNum;
        }

        ~MinerEtherumOCL() {
            // remove from list
            MinerEtherumOCLList.Remove(this);
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            Helpers.ConsolePrint(MinerTAG(), "Starting MinerEtherumOCL, checking existing MinerEtherumOCL to stop");
            foreach (var ethminer in MinerEtherumOCLList) {
                if (ethminer.MINER_ID != MINER_ID && (ethminer.IsRunning || ethminer.IsPaused)) {
                    Helpers.ConsolePrint(MinerTAG(), String.Format("Will end {0} {1}", ethminer.MinerTAG(), ethminer.ProcessTag()));
                    ethminer.End();
                    System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                }
            }
            base.Start(miningAlgorithm, url, username);
        }

        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            // set directory
            WorkingDirectory = "";
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " " + miningAlgorithm.ExtraLaunchParameters
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + Algorithm.PasswordDefault
                + " --api-port " + APIPort.ToString()
                + " --opencl-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(ComputeDevice benchmarkDevice, Algorithm algorithm) {
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " " + algorithm.ExtraLaunchParameters
                + " --benchmark-warmup 40 --benchmark-trial 20"
                + " --opencl-devices ";
        }

    }
}
