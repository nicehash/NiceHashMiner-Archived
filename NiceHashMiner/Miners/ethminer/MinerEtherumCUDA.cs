using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;

namespace NiceHashMiner.Miners {
    public class MinerEtherumCUDA : MinerEtherum {

        // reference to all MinerEtherumCUDA make sure to clear this after miner Stop
        // we make sure only ONE instance of MinerEtherumCUDA is running
        private static List<MinerEtherum> MinerEtherumCUDAList = new List<MinerEtherum>();

        public MinerEtherumCUDA()
            : base("MinerEtherumCUDA", "NVIDIA") {
                MinerEtherumCUDAList.Add(this);
        }

        ~MinerEtherumCUDA() {
            // remove from list
            MinerEtherumCUDAList.Remove(this);
        }

        public override void Start(string url, string btcAdress, string worker) {
            Helpers.ConsolePrint(MinerTAG(), "Starting MinerEtherumCUDA, checking existing MinerEtherumCUDA to stop");
            base.Start(url, btcAdress, worker, MinerEtherumCUDAList);
        }

        protected override string GetStartCommandStringPart(string url, string username) {
            return " --cuda"
                + " "
                + ExtraLaunchParametersParser.ParseForMiningSetup(
                                                    MiningSetup,
                                                    DeviceType.NVIDIA)
                + " -S " + url.Substring(14)
                + " -O " + username + ":x " 
                + " --api-port " + APIPort.ToString()
                + " --cuda-devices ";
        }

        protected override string GetBenchmarkCommandStringPart(Algorithm algorithm) {
            return " --benchmark-warmup 40 --benchmark-trial 20"
                + " "
                + ExtraLaunchParametersParser.ParseForMiningSetup(
                                                    MiningSetup,
                                                    DeviceType.NVIDIA)
                + " --cuda --cuda-devices ";
        }

    }
}
