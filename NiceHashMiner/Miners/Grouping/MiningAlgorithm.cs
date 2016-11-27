using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners.Grouping {
    public class MiningAlgorithm {
        public MiningAlgorithm(ComputeDevice dev, Algorithm algo) {
            this.AlgoRef = algo;
            // init speed that will be avaraged later
            this.AvaragedSpeed = algo.BenchmarkSpeed;
            this.MinerPath = MinerPaths.GetOptimizedMinerPath(dev, algo);
        }
        public Algorithm AlgoRef { get; private set; }
        public string MinerPath { get; private set; }
        // avarage speed of same devices to increase mining stability
        public double AvaragedSpeed = 0;
        public double CurrentProfit = 0;
        public double CurNhmSMADataVal = 0;
    }
}
