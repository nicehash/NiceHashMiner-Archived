using System;
using System.Collections.Generic;
using NiceHashMiner.Enums;
using Newtonsoft.Json;

namespace NiceHashMiner {
    public class Globals {
        public static string[] MiningLocation = { "eu", "usa", "hk", "jp" };
        public static Dictionary<AlgorithmType, NiceHashSMA> NiceHashData = null;
        public static double BitcoinRate;
        public static readonly string DemoUser = "3DJhaQaKA6oyRaGyDZYdkZcise4b9DrCi2";
        public static readonly string PasswordDefault = "x";
        public static JsonSerializerSettings JsonSettings = null;
        public static int ThreadsPerCPU;
        // quickfix guard for checking internet conection
        public static bool IsFirstNetworkCheckTimeout = true;
        public static int FirstNetworkCheckTimeoutTimeMS = 500;
        public static int FirstNetworkCheckTimeoutTries = 10;
        // sgminer Ellesmere/Polaris ignore setting (sgminer needs more testing)
        public static bool IsEllesmereSgminerIgnore = true;

        // change this if TOS changes
        public static int CURRENT_TOS_VER = 1;
    }
}