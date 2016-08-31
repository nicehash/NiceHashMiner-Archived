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

        public MinerEtherumCUDA()
            : base("NVIDIA",
            ConfigManager.Instance.GeneralConfig.EthminerDagGenerationTypeNvidia) {
            MinerDeviceName = this.GetType().Name;
        }

        protected override string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username) {
            return " --cuda"
                + " " + miningAlgorithm.ExtraLaunchParameters
                + " -S " + url.Substring(14)
                + " -O " + username + ":" + Algorithm.PasswordDefault
                + " --api-port " + APIPort.ToString()
                + " --cuda-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(ComputeDevice benchmarkDevice, Algorithm algorithm) {
            return " --benchmark-warmup 40 --benchmark-trial 20"
                + " " + algorithm.ExtraLaunchParameters
                + " --cuda --cuda-devices ";
        }

    }
}
