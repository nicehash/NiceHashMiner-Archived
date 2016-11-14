using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class MinerEtherumCUDA : MinerEtherum {

        // reference to all MinerEtherumCUDA make sure to clear this after miner Stop
        // we make sure only ONE instance of MinerEtherumCUDA is running
        private static List<MinerEtherum> MinerEtherumCUDAList = new List<MinerEtherum>();

        public MinerEtherumCUDA()
            : base(DeviceType.NVIDIA, "MinerEtherumCUDA", "NVIDIA") {
                MinerEtherumCUDAList.Add(this);
        }

        ~MinerEtherumCUDA() {
            // remove from list
            MinerEtherumCUDAList.Remove(this);
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            Helpers.ConsolePrint(MinerTAG(), "Starting MinerEtherumCUDA, checking existing MinerEtherumCUDA to stop");
            base.Start(miningAlgorithm, url, username, MinerEtherumCUDAList);
        }

        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            return " --cuda"
                + " "
                + ExtraLaunchParametersParser.ParseForCDevs(
                                                    CDevs,
                                                    AlgorithmType.DaggerHashimoto,
                                                    DeviceType.NVIDIA)
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + Algorithm.PasswordDefault
                + " --api-port " + APIPort.ToString()
                + " --cuda-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(Algorithm algorithm) {
            return " --benchmark-warmup 40 --benchmark-trial 20"
                + " "
                + ExtraLaunchParametersParser.ParseForCDevs(
                                                    CDevs,
                                                    AlgorithmType.DaggerHashimoto,
                                                    DeviceType.NVIDIA)
                + " --cuda --cuda-devices ";
        }

    }
}
