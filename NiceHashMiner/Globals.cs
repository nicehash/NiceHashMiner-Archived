using System;
using System.Collections.Generic;
using NiceHashMiner.Enums;

namespace NiceHashMiner {
    public class Globals {
        public static string[] MiningLocation = { "eu", "usa", "hk", "jp" };
        public static Dictionary<AlgorithmType, NiceHashSMA> NiceHashData = null;
        public static double BitcoinRate;
    }
}