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
        private static List<MinerEtherum> MinerEtherumOCLList = new List<MinerEtherum>();

        private readonly int GPUPlatformNumber;

        public MinerEtherumOCL()
            : base(DeviceType.AMD, "MinerEtherumOCL", "AMD OpenCL") {
            GPUPlatformNumber = ComputeDeviceQueryManager.Instance.AMDOpenCLPlatformNum;
            MinerEtherumOCLList.Add(this);
        }

        ~MinerEtherumOCL() {
            // remove from list
            MinerEtherumOCLList.Remove(this);
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            Helpers.ConsolePrint(MinerTAG(), "Starting MinerEtherumOCL, checking existing MinerEtherumOCL to stop");
            base.Start(miningAlgorithm, url, username, MinerEtherumOCLList);
        }

        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            // set directory
            WorkingDirectory = "";
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " "
                + ExtraLaunchParametersParser.ParseForCDevs(
                                                    CDevs,
                                                    AlgorithmType.DaggerHashimoto,
                                                    DeviceType.AMD)
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + Algorithm.PasswordDefault
                + " --api-port " + APIPort.ToString()
                + " --opencl-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(Algorithm algorithm) {
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " "
                + ExtraLaunchParametersParser.ParseForCDevs(
                                                    CDevs,
                                                    AlgorithmType.DaggerHashimoto,
                                                    DeviceType.AMD)
                + " --benchmark-warmup 40 --benchmark-trial 20"
                + " --opencl-devices ";
        }

    }
}
