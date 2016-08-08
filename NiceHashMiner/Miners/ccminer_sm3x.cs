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
        public ccminer_sm3x(bool queryComputeDevices) :
            base(queryComputeDevices)
        {
            MinerDeviceName = "NVIDIA3.x";
            Path = MinerPaths.ccminer_tpruvot;

            TryQueryCDevs();
        }

        protected override MinerType GetMinerType() {
            return MinerType.ccminer_sm3x;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_3_x);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
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
