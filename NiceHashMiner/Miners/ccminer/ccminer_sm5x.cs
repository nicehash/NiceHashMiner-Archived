using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners
{
    class ccminer_sm5x : ccminer
    {
        public ccminer_sm5x(DeviceGroupType deviceGroupType = DeviceGroupType.NVIDIA_5_x, string minerDeviceName = "NVIDIA5.x") :
            base(deviceGroupType, minerDeviceName)
        {
            Path = MinerPaths.ccminer_sp;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_5_x);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            allGroupSupportedList.Remove(AlgorithmType.Equihash);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }
    }
}
