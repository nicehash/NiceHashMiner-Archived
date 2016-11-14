using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners
{
    class ccminer_sm3x : ccminer
    {
        public ccminer_sm3x() : base(DeviceGroupType.NVIDIA_3_x, "NVIDIA3.x")
        {
            Path = MinerPaths.ccminer_tpruvot;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_3_x);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            allGroupSupportedList.Remove(AlgorithmType.Equihash);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }
    }
}
