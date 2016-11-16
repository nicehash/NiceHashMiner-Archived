using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    using MinersManager = MinersManager_NEW;
    public static class GroupingLogic {
        private static bool IsAlgorithmSettingsSame(Algorithm a, Algorithm b) {
            return a.NiceHashID == b.NiceHashID
#if (SWITCH_TESTING)
 && (!ForcePerCardMiners) // this will force individual miners
#endif
;
        }

        private static bool IsNvidiaDevice(ComputeDevice a) {
            return a.DeviceType == DeviceType.NVIDIA;
        }

        public static bool IsEquihashGroupLogic(ComputeDevice a, ComputeDevice b) {
            // eqm
            if (IsEquihashDevice_eqm(a) && IsEquihashDevice_eqm(b)) { // both eqm
                return IsEquihashAnd_eqm(a, b);
            }
                // nheqmnier
            else if (!IsEquihashDevice_eqm(a) && !IsEquihashDevice_eqm(b)) { // both NOT eqm
                return IsEquihashAnd_nheqminer(a, b);
            }
            return false;
        }

        // nheqminer
        private static bool IsEquihashAnd_nheqminer(ComputeDevice a, ComputeDevice b) {
            return a.MostProfitableAlgorithm.NiceHashID == AlgorithmType.Equihash
                && a.MostProfitableAlgorithm.NiceHashID == b.MostProfitableAlgorithm.NiceHashID
                && IsEquihashAnd_CPU_nheqminer(a)
                && IsEquihashAnd_CPU_nheqminer(b);
        }
        // group only first CPU split
        private static bool IsEquihashAnd_CPU_nheqminer(ComputeDevice a) {
            return a.DeviceType != DeviceType.CPU // if not cpu then ignore case always good
                || a.ID == 0; // if CPU ID must be 0
        }

        // eqm
        private static bool IsEquihashAnd_eqm(ComputeDevice a, ComputeDevice b) {
            return a.MostProfitableAlgorithm.NiceHashID == AlgorithmType.Equihash
                && a.MostProfitableAlgorithm.NiceHashID == b.MostProfitableAlgorithm.NiceHashID;
        }
        private static bool IsEquihashDevice_eqm(ComputeDevice a) {
            return IsEquihashCPU_eqm(a) || IsEquihashNVIDIA_eqm(a);
        }
        private static bool IsEquihashCPU_eqm(ComputeDevice a) {
            return MinersManager.EquihashCPU_USE_eqm() && a.DeviceType == DeviceType.CPU;
        }
        private static bool IsEquihashNVIDIA_eqm(ComputeDevice a) {
            return a.DeviceGroupType == DeviceGroupType.NVIDIA_5_x || a.DeviceGroupType == DeviceGroupType.NVIDIA_6_x;
        }

        // checks if dagger algo, same settings and if compute platform is same
        public static bool IsDaggerAndSameComputePlatform(ComputeDevice a, ComputeDevice b) {
            return a.MostProfitableAlgorithm.NiceHashID == AlgorithmType.DaggerHashimoto
                && a.MostProfitableAlgorithm.NiceHashID == b.MostProfitableAlgorithm.NiceHashID //IsAlgorithmSettingsSame(a.MostProfitableAlgorithm, b.MostProfitableAlgorithm)
                // check if both etherum capable
                && a.IsEtherumCapale && b.IsEtherumCapale
                // compute platforms must be same
                && (IsNvidiaDevice(a) == IsNvidiaDevice(b));
        }

        private static bool IsNotCpuGroups(ComputeDevice a, ComputeDevice b) {
            return a.DeviceGroupType != DeviceGroupType.CPU && b.DeviceGroupType != DeviceGroupType.CPU;
        }

        // this will not check Ethminer path
        private static bool IsSameBinPath(ComputeDevice a, ComputeDevice b) {
            // same group uses same Miner class and therefore same binary path for same algorithm
            bool sameGroup = a.DeviceGroupType == b.DeviceGroupType;
            if (!sameGroup) {
                var a_algoType = a.MostProfitableAlgorithm.NiceHashID;
                var b_algoType = b.MostProfitableAlgorithm.NiceHashID;
                // a and b algorithm settings should be the same if we call this function
                return MinerPaths.GetOptimizedMinerPath(a_algoType, a.DeviceType, a.DeviceGroupType, a.Codename, a.IsOptimizedVersion)
                    == MinerPaths.GetOptimizedMinerPath(b_algoType, b.DeviceType, b.DeviceGroupType, b.Codename, b.IsOptimizedVersion);
            }

            return true;
        }

        // we don't want to group CPU devices
        public static bool IsGroupBinaryAndAlgorithmSame(ComputeDevice a, ComputeDevice b) {
            return IsNotCpuGroups(a, b)
                && IsAlgorithmSettingsSame(a.MostProfitableAlgorithm, b.MostProfitableAlgorithm)
                && IsSameBinPath(a, b);
        }
    }
}
