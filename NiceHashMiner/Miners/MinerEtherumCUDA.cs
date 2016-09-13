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
        private static List<MinerEtherumCUDA> MinerEtherumCUDAList = new List<MinerEtherumCUDA>();

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
            foreach (var ethminer in MinerEtherumCUDAList) {
                if (ethminer.MINER_ID != MINER_ID && (ethminer.IsRunning || ethminer.IsPaused)) {
                    Helpers.ConsolePrint(MinerTAG(), String.Format("Will end {0} {1}",ethminer.MinerTAG(), ethminer.ProcessTag()));
                    ethminer.End();
                    System.Threading.Thread.Sleep(ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS);
                } 
            }
            base.Start(miningAlgorithm, url, username);
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
