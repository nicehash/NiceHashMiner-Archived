using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NiceHashMiner.Interfaces;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners {
    // typedefs
    using DeviceSubsetList = List<SortedSet<string>>;
    using PerDeviceSpeedDictionary = Dictionary<string, Dictionary<AlgorithmType, double>>;

    using GroupedDevices = SortedSet<string>;
    public class MinersManager_NEW : BaseLazySingleton<MinersManager_NEW> {

        private MiningSession CurMiningSession;

        public void StopAllMiners() {
            if (CurMiningSession != null) CurMiningSession.StopAllMiners();
        }

        public void StopAllMinersNonProfitable() {
            if (CurMiningSession != null) CurMiningSession.StopAllMinersNonProfitable();
        }

        public string GetActiveMinersGroup() {
            if (CurMiningSession != null) {
                return CurMiningSession.GetActiveMinersGroup();
            }
            // if no session it is idle
            return "IDLE";
        }

        public static bool EquihashCPU_USE_eqm() {
            var mostOptimized = CPUUtils.GetMostOptimized();
            return mostOptimized == CPUExtensionType.AVX || mostOptimized == CPUExtensionType.AVX2
                || mostOptimized == CPUExtensionType.AVX_AES || mostOptimized == CPUExtensionType.AVX2_AES;
        }

        // create miner creates new miners, except cpuminer, those are saves and called from GetCpuMiner()
        public static Miner CreateMiner(DeviceGroupType deviceGroupType, AlgorithmType algorithmType) {
            if (AlgorithmType.Equihash == algorithmType) {
                if (DeviceGroupType.NVIDIA_5_x == deviceGroupType || DeviceGroupType.NVIDIA_6_x == deviceGroupType
                    || (EquihashCPU_USE_eqm() && DeviceGroupType.CPU == deviceGroupType)) {
                    return new eqm();
                } else {
                    return new nheqminer();
                }
            } else if (AlgorithmType.DaggerHashimoto == algorithmType) {
                if (DeviceGroupType.AMD_OpenCL == deviceGroupType) {
                    return new MinerEtherumOCL();
                } else {
                    return new MinerEtherumCUDA();
                }
            } else {
                switch (deviceGroupType) {
                    case DeviceGroupType.AMD_OpenCL:
                        return new sgminer();
                    case DeviceGroupType.NVIDIA_2_1:
                        return new ccminer_sm21();
                    case DeviceGroupType.NVIDIA_3_x:
                        return new ccminer_sm3x();
                    case DeviceGroupType.NVIDIA_5_x:
                        return new ccminer_sm5x();
                    case DeviceGroupType.NVIDIA_6_x:
                        return new ccminer_sm6x();
                    case DeviceGroupType.CPU:
                        return new cpuminer();
                }
            }
            
            return null;
        }

        public double GetTotalRate() {
            if (CurMiningSession != null) return CurMiningSession.GetTotalRate();
            return 0;
        }

        public bool StartInitialize(IMainFormRatesComunication mainFormRatesComunication,
            string miningLocation, string worker, string btcAdress) {
                
            CurMiningSession = new MiningSession(ComputeDeviceManager.Avaliable.AllAvaliableDevices,
                mainFormRatesComunication, miningLocation, worker, btcAdress);

            return CurMiningSession.IsMiningEnabled;
        }



        /// <summary>
        /// SwichMostProfitable should check the best combination for most profit.
        /// Calculate profit for each supported algorithm per device group.
        /// Build from ground up compatible devices and algorithms.
        /// See #region Groupping logic
        /// Device groups are CPU, AMD_OpenCL and NVIDIA CUDA SM.x.x.
        /// NVIDIA SMx.x should be paired separately except for daggerhashimoto.
        /// </summary>
        /// <param name="NiceHashData"></param>
        public void SwichMostProfitableGroupUpMethod(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            if (CurMiningSession != null) CurMiningSession.SwichMostProfitableGroupUpMethod(NiceHashData);
        }

        public void MinerStatsCheck(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            if (CurMiningSession != null) CurMiningSession.MinerStatsCheck(NiceHashData);
        }
    }
}
