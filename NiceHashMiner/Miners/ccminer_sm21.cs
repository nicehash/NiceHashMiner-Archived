using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners
{
    class ccminer_sm21 : ccminer
    {
        public ccminer_sm21()
            : base(DeviceGroupType.NVIDIA_2_1, "NVIDIA2.1")
        {
            Path = MinerPaths.ccminer_tpruvot;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_2_1);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            allGroupSupportedList.Remove(AlgorithmType.Equihash);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }
    }
}
