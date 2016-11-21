using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners.Grouping {
    public static class GroupingLogic {

        public static bool ShouldGroup(MiningPair a, MiningPair b) {
            // group if same bin path and same algo type
            if (IsSameBinPath(a, b) && IsSameAlgorithmType(a, b)) {
                AlgorithmType algorithmType = a.Algorithm.NiceHashID;
                // AlgorithmType.Equihash is special case
                if (AlgorithmType.Equihash == algorithmType) {
                    string minerPath = MinerPaths.GetOptimizedMinerPath(a);
                    return EquihashGroup.IsEquihashGroupLogic(minerPath,
                        a, b);
                }
                    // all other algorithms are grouped if DeviceType is same and is not CPU
                else if (IsNotCpuGroups(a, b) && IsSameDeviceType(a, b)) {
                    return true;
                }
            }
            return false;
        }

        private static class EquihashGroup {
            public static bool IsEquihashGroupLogic(string minerPath, MiningPair a, MiningPair b) {
                // eqm
                if (MinerPaths.eqm == minerPath) {
                    return Is_eqm(a, b);
                }
                    // nheqmnier
                else if (MinerPaths.nheqminer == minerPath) {
                    return Is_nheqminer(a, b);
                }
                return false;
            }

            // nheqminer
            private static bool Is_nheqminer(MiningPair a, MiningPair b) {
                return IsDevice_nheqminer(a) && IsDevice_nheqminer(b);
            }
            // group only first CPU split
            private static bool IsDevice_nheqminer(MiningPair a) {
                return a.Device.DeviceType != DeviceType.CPU // if not cpu then ignore case always good
                    || a.Device.ID == 0; // if CPU ID must be 0
            }

            // eqm
            private static bool Is_eqm(MiningPair a, MiningPair b) {
                return IsDevice_eqm(a) && IsDevice_eqm(b);
            }
            private static bool IsDevice_eqm(MiningPair a) {
                return IsCPU_eqm(a) || IsNVIDIA_eqm(a);
            }
            private static bool IsCPU_eqm(MiningPair a) {
                return MinersManager.EquihashCPU_USE_eqm() && a.Device.DeviceType == DeviceType.CPU;
            }
            private static bool IsNVIDIA_eqm(MiningPair a) {
                var groupType = a.Device.DeviceGroupType;
                return DeviceGroupType.NVIDIA_5_x == groupType || DeviceGroupType.NVIDIA_6_x == groupType;
            }
        }

        private static bool IsNotCpuGroups(MiningPair a, MiningPair b) {
            return a.Device.DeviceType != DeviceType.CPU && b.Device.DeviceType != DeviceType.CPU;
        }

        private static bool IsSameBinPath(MiningPair a, MiningPair b) {
            return MinerPaths.GetOptimizedMinerPath(a)
                    == MinerPaths.GetOptimizedMinerPath(b);
        }
        private static bool IsSameAlgorithmType(MiningPair a, MiningPair b) {
            return a.Algorithm.NiceHashID == b.Algorithm.NiceHashID;
        }
        private static bool IsSameDeviceType(MiningPair a, MiningPair b) {
            return a.Device.DeviceType == b.Device.DeviceType;
        }
    }
}
