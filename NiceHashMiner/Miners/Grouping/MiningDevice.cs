using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Grouping {
    public class MiningDevice {

        // switch testing quick and dirty, runtime versions 
#if (SWITCH_TESTING)
        static List<AlgorithmType> testingAlgos = new List<AlgorithmType>() {
            //AlgorithmType.X13,
            //AlgorithmType.Keccak,
            //AlgorithmType.X15,
            //AlgorithmType.Nist5,
            //AlgorithmType.NeoScrypt,
            AlgorithmType.Lyra2RE,
            //AlgorithmType.WhirlpoolX,
            //AlgorithmType.Qubit,
            //AlgorithmType.Quark,
            //AlgorithmType.Lyra2REv2,
            //AlgorithmType.Blake256r8,
            //AlgorithmType.Blake256r14,
            //AlgorithmType.Blake256r8vnl,
            AlgorithmType.Hodl,
            //AlgorithmType.DaggerHashimoto,
            //AlgorithmType.Decred,
            AlgorithmType.CryptoNight,
            //AlgorithmType.Lbry,
            AlgorithmType.Equihash
        };
        static int next = -1;
        public static void SetNextTest() {
            ++next;
            if (next >= testingAlgos.Count) next = 0;
            var mostProfitKeyName = AlgorithmNiceHashNames.GetName(testingAlgos[next]);
            Helpers.ConsolePrint("SWITCH_TESTING", String.Format("Setting most MostProfitKey to {0}", mostProfitKeyName));
        }

        static bool ForceNone = false;
        // globals testing variables
        static int seconds = 20;
        public static int SMAMinerCheckInterval = seconds * 1000; // 30s
        public static bool ForcePerCardMiners = false;
#endif

        public MiningDevice(ComputeDevice device) {
            Device = device;
            foreach (var kvp in Device.AlgorithmSettings) {
                AlgorithmType key = kvp.Key;
                Algorithm algo = kvp.Value;
                bool isAlgoMiningCapable = GroupSetupUtils.IsAlgoMiningCapable(algo);
                bool isValidMinerPath = GroupSetupUtils.IsValidMinerPath(device, algo);
                if (isAlgoMiningCapable && isValidMinerPath) {
                    Algorithms[key] = new MiningAlgorithm(device, algo);
                }
            }
        }
        public ComputeDevice Device { get; private set; }
        public Dictionary<AlgorithmType, MiningAlgorithm> Algorithms = new Dictionary<AlgorithmType, MiningAlgorithm>();

        public AlgorithmType MostProfitableKey { get; private set; }

        public double GetCurrentMostProfitValue {
            get {
                if (AlgorithmType.NONE != MostProfitableKey) {
                    return Algorithms[MostProfitableKey].CurrentProfit;
                }
                return 0;
            }
        }

        public MiningPair GetMostProfitablePair() {
            return new MiningPair(this.Device, Algorithms[MostProfitableKey].AlgoRef);
        }

        public bool HasProfitableAlgo() {
            return MostProfitableKey != AlgorithmType.NONE;
        }

        public void CalculateProfits(Dictionary<AlgorithmType, NiceHashSMA> NiceHashData) {
            // assume none is profitable
            MostProfitableKey = AlgorithmType.NONE;
            // calculate new profits
            foreach (var miningAlgo in Algorithms) {
                AlgorithmType key = miningAlgo.Key;
                MiningAlgorithm algo = miningAlgo.Value;
                if (NiceHashData.ContainsKey(key)) {
                    algo.CurNhmSMADataVal = NiceHashData[key].paying;
                    algo.CurrentProfit = algo.CurNhmSMADataVal * algo.AvaragedSpeed * 0.000000001;
                } else {
                    algo.CurrentProfit = 0;
                }
            }
            // find max paying value and save key
            double maxProfit = 0;
            foreach (var miningAlgo in Algorithms) {
                AlgorithmType key = miningAlgo.Key;
                MiningAlgorithm algo = miningAlgo.Value;
                if (maxProfit < algo.CurrentProfit) {
                    maxProfit = algo.CurrentProfit;
                    MostProfitableKey = key;
                }
            }
#if (SWITCH_TESTING)
            var devName = Device.GetFullName();
            // set new most profit
            if (Algorithms.ContainsKey(testingAlgos[next])) {
                MostProfitableKey = testingAlgos[next];
            } else if(ForceNone) {
                MostProfitableKey = AlgorithmType.NONE;
            }
            var mostProfitKeyName = AlgorithmNiceHashNames.GetName(MostProfitableKey);
            Helpers.ConsolePrint("SWITCH_TESTING", String.Format("Setting device {0} to {1}", devName, mostProfitKeyName));
#endif
        }
    }
}
