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
    using GroupedDevices = SortedSet<string>;
    using NiceHashMiner.Miners.Grouping;
    public class MinersManager : BaseLazySingleton<MinersManager> {

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
        public static Miner CreateMiner(ComputeDevice device, Algorithm algorithm) {
            var minerPath = MinerPaths.GetOptimizedMinerPath(device, algorithm);
            if (minerPath != MinerPaths.NONE) {
                return CreateMiner(device.DeviceType, minerPath);
            }
            return null;
        }

        public static Miner CreateMiner(DeviceType deviceType, string minerPath) {
            if (minerPath == MinerPaths.eqm && DeviceType.AMD != deviceType) {
                return new eqm();
            } else if (minerPath == MinerPaths.nheqminer) {
                return new nheqminer();
            } else if (
                ConfigManager_rem.Instance.GeneralConfig.Use3rdPartyMiners == Use3rdPartyMiners.YES
                && minerPath == MinerPaths.ClaymoreZcashMiner && DeviceType.AMD == deviceType) {
                return new ClaymoreZcashMiner();
            } else if (minerPath.Contains("ethminer") && DeviceType.CPU != deviceType) {
                if (DeviceType.AMD == deviceType) {
                    return new MinerEtherumOCL();
                } else {
                    return new MinerEtherumCUDA();
                }
            } else if (minerPath.Contains("cpuminer") && DeviceType.CPU == deviceType) {
                return new cpuminer();
            } else if (minerPath.Contains("sgminer") && DeviceType.AMD == deviceType) {
                return new sgminer();
            } else if(minerPath.Contains("ccminer") && DeviceType.NVIDIA == deviceType) {
                return new ccminer();
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
        /// Calculate profit for each supported algorithm per device and group.
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
