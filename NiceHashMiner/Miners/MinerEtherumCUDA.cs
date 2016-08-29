using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class MinerEtherumCUDA : MinerEtherum {

        public MinerEtherumCUDA()
            : base("NVIDIA",
            ConfigManager.Instance.GeneralConfig.EthminerDagGenerationTypeNvidia) {
            MinerDeviceName = this.GetType().Name;
        }

        protected override MinerType GetMinerType() {
            return MinerType.MinerEtherumCUDA;
        }


        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            return " --cuda"
                + " " + ExtraLaunchParameters
                + " " + miningAlgorithm.ExtraLaunchParameters
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + GetPassword(miningAlgorithm)
                + " --api-port " + APIPort.ToString()
                + " --cuda-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm) {
            return " --benchmark-warmup 40 --benchmark-trial 20"
                + " " + benchmarkConfig.ExtraLaunchParameters
                + " " + algorithm.ExtraLaunchParameters
                + " --cuda --cuda-devices ";
        }

    }
}
