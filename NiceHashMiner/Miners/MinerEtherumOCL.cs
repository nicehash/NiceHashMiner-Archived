using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {

    // NOT TESTED broken 
    public class MinerEtherumOCL : MinerEtherum {

        int GPUPlatformNumber;

        public MinerEtherumOCL() {
            isOCL = true;
        }

        protected override MinerType GetMinerType() {
            return MinerType.MinerEtherumOCL;
        }

        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            // set directory
            WorkingDirectory = "";
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " " + ExtraLaunchParameters
                + " " + miningAlgorithm.ExtraLaunchParameters
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + GetPassword(miningAlgorithm)
                + " --api-port " + ConfigManager.Instance.GeneralConfig.ethminerAPIPortAMD.ToString()
                + " --opencl-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm) {
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " " + ExtraLaunchParameters
                + " " + algorithm.ExtraLaunchParameters
                + " --benchmark-warmup 40 --benchmark-trial 20"
                + " --opencl-devices ";
        }

    }
}
