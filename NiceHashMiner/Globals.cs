using System;
using System.Collections.Generic;
using NiceHashMiner.Enums;
using Newtonsoft.Json;

namespace NiceHashMiner {
    public class Globals {
        public static string[] MiningLocation = { "eu", "usa", "hk", "jp" };
        public static Dictionary<AlgorithmType, NiceHashSMA> NiceHashData = null;
        public static double BitcoinRate;
        public static string DemoUser = "34HKWdzLxWBduUfJE9JxaFhoXnfC6gmePG";
        public static JsonSerializerSettings JsonSettings = null;
        public static int ThreadsPerCPU;
        // quickfix guard for checking internet conection
        public static bool IsFirstNetworkCheckTimeout = true;
        public static int FirstNetworkCheckTimeoutTimeMS = 500;
        public static int FirstNetworkCheckTimeoutTries = 10;
    }
}