using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs.Data {
    [Serializable]
    public class AlgorithmConfig {
        public AlgorithmType NiceHashID = AlgorithmType.NONE;
        public string MinerName = "";  // TODO probably not needed
        public double BenchmarkSpeed = 0;
        public string ExtraLaunchParameters= "";
        public bool Skip = false;
        public int LessThreads = 0;
    }
}
