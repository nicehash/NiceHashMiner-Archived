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
        public ccminer_sm21() : base()
        {
            MinerDeviceName = "NVIDIA2.1";
            Path = MinerPaths.ccminer_tpruvot;
        }

        protected override MinerType GetMinerType() {
            return MinerType.ccminer_sm21;
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.NVIDIA_2_1);
            allGroupSupportedList.Remove(AlgorithmType.DaggerHashimoto);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        public override string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename, bool isOptimized) {
            if (AlgorithmType.Decred == algorithmType) {
                return MinerPaths.ccminer_decred;
            }
            if (AlgorithmType.CryptoNight == algorithmType) {
                return MinerPaths.ccminer_cryptonight;
            } 
            return MinerPaths.ccminer_tpruvot;
        }
    }
}
