using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners.Grouping {
    public class MiningSetup {
        public List<MiningPair> MiningPairs { get; private set; }
        public string MinerPath { get; private set; }
        public string MinerName { get; private set; }
        public AlgorithmType CurrentAlgorithmType { get; private set; }
        public AlgorithmType CurrentSecondaryAlgorithmType { get; private set; }
        public bool IsInit { get; private set; }

        public MiningSetup(List<MiningPair> miningPairs) {
            this.IsInit = false;
            this.CurrentAlgorithmType = AlgorithmType.NONE;
            if (miningPairs != null && miningPairs.Count > 0) {
                this.MiningPairs = miningPairs;
                this.MiningPairs.Sort((a, b) => a.Device.ID - b.Device.ID);
                this.MinerName = miningPairs[0].Algorithm.MinerName;
                this.CurrentAlgorithmType = miningPairs[0].Algorithm.NiceHashID;
                this.CurrentSecondaryAlgorithmType = miningPairs[0].Algorithm.SecondaryNiceHashID;
                this.MinerPath = miningPairs[0].Algorithm.MinerBinaryPath;
                this.IsInit = MinerPaths.IsValidMinerPath(this.MinerPath);
            }
        }
    }
}
