using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using NiceHashMiner.Interfaces;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners {
    using NiceHashMiner.Miners.Grouping;
    using NiceHashMiner.Miners.Equihash;
    public static class MinersManager {

        private static MiningSession CurMiningSession;

        public static void StopAllMiners() {
            if (CurMiningSession != null) CurMiningSession.StopAllMiners();
            CurMiningSession = null;
        }

        public static void StopAllMinersNonProfitable() {
            if (CurMiningSession != null) CurMiningSession.StopAllMinersNonProfitable();
        }

        public static string GetActiveMinersGroup() {
            if (CurMiningSession != null) {
                return CurMiningSession.GetActiveMinersGroup();
            }
            // if no session it is idle
            return "IDLE";
        }

        public static double GetTotalRate() {
            if (CurMiningSession != null) return CurMiningSession.GetTotalRate();
            return 0;
        }

        public static bool StartInitialize(IMainFormRatesComunication mainFormRatesComunication,
            string miningLocation, string worker, string btcAdress) {
                
            CurMiningSession = new MiningSession(ComputeDeviceManager.Avaliable.AllAvaliableDevices,
                mainFormRatesComunication, miningLocation, worker, btcAdress);

            return CurMiningSession.IsMiningEnabled;
        }

        public static bool IsMiningEnabled() {
            if (CurMiningSession != null) return CurMiningSession.IsMiningEnabled;
            return false;
        }


        /// <summary>
        /// SwichMostProfitable should check the best combination for most profit.
        /// Calculate profit for each supported algorithm per device and group.
        /// </summary>
        /// <param name="NiceHashData"></param>
        public static void SwichMostProfitableGroupUpMethod(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            if (CurMiningSession != null) CurMiningSession.SwichMostProfitableGroupUpMethod(NiceHashData);
        }

        public static void MinerStatsCheck(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            if (CurMiningSession != null) CurMiningSession.MinerStatsCheck(NiceHashData);
        }
    }
}
