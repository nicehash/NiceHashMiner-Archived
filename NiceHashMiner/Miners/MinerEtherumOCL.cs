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
    public class MinerEtherumOCL : MinerEtherum {

        private readonly int GPUPlatformNumber;

        public MinerEtherumOCL() : base() {
            // AMD or TODO it could be something else
            MinerDeviceName = "AMD OpenCL";
            GPUPlatformNumber = ComputeDeviceQueryManager.Instance.AMDOpenCLPlatformNum;
        }

        protected override MinerType GetMinerType() {
            return MinerType.MinerEtherumOCL;
        }

        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";
            var cdqm = ComputeDeviceQueryManager.Instance;
            List<string> ids = new List<string>();
            foreach (var cdev in CDevs) {
                // get AMD ethminer OCL ids
                ids.Add(
                    cdqm.GetEthminerOpenCLID(ComputePlatformType.AMD, cdev.ID).ToString()
                    );
            }
            deviceStringCommand += string.Join(" ", ids);
            deviceStringCommand += " --dag-load-mode singlekeep " + DaggerHashimotoGenerateDevice.ID.ToString();
            return deviceStringCommand;
        }

        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            // set directory
            WorkingDirectory = "";
            return " --opencl --opencl-platform " + GPUPlatformNumber
                + " " + ExtraLaunchParameters
                + " " + miningAlgorithm.ExtraLaunchParameters
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + GetPassword(miningAlgorithm)
                + " --api-port " + APIPort.ToString()
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
