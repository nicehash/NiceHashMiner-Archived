using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    class ccminer_sm6x : ccminer {
        public ccminer_sm6x(bool queryComputeDevices) :
            base(queryComputeDevices)
        {
            MinerDeviceName = "NVIDIA6.x";
            Path = MinerPaths.ccminer_nanashi;

            TryQueryCDevs();
        }

        protected override MinerType GetMinerType() {
            return MinerType.ccminer_sm6x;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_5_x);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        protected override bool IsGroupQueryEnabled() {
            return !ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia5X;
        }

        protected override string GetOptimizedMinerPath(AlgorithmType algorithmType) {
            if (AlgorithmType.Decred == algorithmType) {
                return MinerPaths.ccminer_decred;
            }
            if (AlgorithmType.NeoScrypt == algorithmType) {
                return MinerPaths.ccminer_neoscrypt;
            }
            if (AlgorithmType.Lyra2RE == algorithmType || AlgorithmType.Lyra2REv2 == algorithmType) {
                return MinerPaths.ccminer_nanashi;
            }

            return MinerPaths.ccminer_sp;
        }

        protected override bool IsPotentialDevSM(string name) {
            return name.Contains("SM 6.");
        }

    }
}
