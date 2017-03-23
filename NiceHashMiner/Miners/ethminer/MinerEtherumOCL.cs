using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;

namespace NiceHashMiner.Miners {

    // TODO for NOW ONLY AMD
    // AMD or TODO it could be something else
    public class MinerEtherumOCL : MinerEtherum {

        // reference to all MinerEtherumOCL make sure to clear this after miner Stop
        // we make sure only ONE instance of MinerEtherumOCL is running
        private static List<MinerEtherum> MinerEtherumOCLList = new List<MinerEtherum>();

        private readonly int GPUPlatformNumber;

        public MinerEtherumOCL()
            : base("MinerEtherumOCL", "AMD OpenCL") {
            GPUPlatformNumber = ComputeDeviceManager.Avaliable.AMDOpenCLPlatformNum;
            MinerEtherumOCLList.Add(this);
        }

        ~MinerEtherumOCL() {
            // remove from list
            MinerEtherumOCLList.Remove(this);
        }

        public override void Start(string url, string btcAdress, string worker) {
            Helpers.ConsolePrint(MinerTAG(), "Starting MinerEtherumOCL, checking existing MinerEtherumOCL to stop");
            base.Start(url, btcAdress, worker, MinerEtherumOCLList);
        }

        protected override string GetStartCommandStringPart(string url, string username) {
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " "
                + ExtraLaunchParametersParser.ParseForMiningSetup(
                                                    MiningSetup,
                                                    DeviceType.AMD)
                + " -S " + url.Substring(14)
                + " -O " + username + ":x " 
                + " --api-port " + APIPort.ToString()
                + " --opencl-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(Algorithm algorithm) {
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " "
                + ExtraLaunchParametersParser.ParseForMiningSetup(
                                                    MiningSetup,
                                                    DeviceType.AMD)
                + " --benchmark-warmup 40 --benchmark-trial 20"
                + " --opencl-devices ";
        }

    }
}
