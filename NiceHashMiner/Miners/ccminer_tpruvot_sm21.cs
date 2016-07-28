using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

namespace NiceHashMiner
{
    class ccminer_tpruvot_sm21 : ccminer
    {
        public ccminer_tpruvot_sm21() : base()
        {
            MinerDeviceName = "NVIDIA2.1";
            Path = MinerPaths.ccminer_tpruvot;
            APIPort = 4021;

            SupportedAlgorithms = GroupAlgorithms.CreateDefaultsForGroup(DeviceGroupType.NVIDIA_2_1);

            if (!Config.ConfigData.DisableDetectionNVidia2X)
                QueryCDevs();
        }

        protected override string GetOptimizedMinerPath(AlgorithmType algorithmType) {
            if (AlgorithmType.Decred == algorithmType) {
                return MinerPaths.ccminer_decred;
            }
            return MinerPaths.ccminer_tpruvot;
        }

        protected override bool IsPotentialDevSM(string name) {
            // add only SM 2.1
            return name.Contains("SM 2.1");
        }
    }
}
