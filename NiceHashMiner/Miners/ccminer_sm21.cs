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
        public ccminer_sm21(bool queryComputeDevices)
            : base(queryComputeDevices)
        {
            MinerDeviceName = "NVIDIA2.1";
            Path = MinerPaths.ccminer_tpruvot;

            TryQueryCDevs();
        }

        protected override MinerType GetMinerType() {
            return MinerType.ccminer_sm21;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_2_1);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        protected override bool IsGroupQueryEnabled() {
            return !ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia2X;
        }

        public override string GetOptimizedMinerPath(AlgorithmType algorithmType) {
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
