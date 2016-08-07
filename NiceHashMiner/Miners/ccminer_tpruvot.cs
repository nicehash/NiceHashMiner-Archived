using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners
{
    class ccminer_tpruvot : ccminer
    {
        public ccminer_tpruvot(bool queryComputeDevices) :
            base(queryComputeDevices)
        {
            MinerDeviceName = "NVIDIA3.x";
            Path = MinerPaths.ccminer_tpruvot;
            APIPort = 4049;

            TryQueryCDevs();
        }

        protected override bool IsGroupQueryEnabled() {
            return !ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia3X;
        }

        protected override string GetOptimizedMinerPath(AlgorithmType algorithmType) {
            if (AlgorithmType.Decred == algorithmType) {
                return MinerPaths.ccminer_decred;
            }
            return MinerPaths.ccminer_tpruvot;
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 3.x
            return name.Contains("SM 3.");
        }
    }
}
