using Newtonsoft.Json;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner {
    [Serializable]
    public class Algorithm {
        readonly public AlgorithmType NiceHashID;
        // ignore the dictionary atribute shows the name
        [JsonIgnore]
        readonly public string NiceHashName;
        // Miner name is used for miner ALGO flag parameter
        readonly public string MinerName;
        public double BenchmarkSpeed { get; set; }
        public string ExtraLaunchParameters { get; set; }

        // CPU miners only setting
        public int LessThreads { get; set; }

        public bool Skip { get; set; }

        public static readonly string PasswordDefault = "x";

        // benchmark info
        [JsonIgnore]
        public string BenchmarkStatus { get; set; }
        [JsonIgnore]
        public bool IsBenchmarkPending { get; private set; }
        [JsonIgnore]
        public string CurPayingRatio {
            get {
                string ratio = International.GetText("BenchmarkRatioRateN_A");
                if (Globals.NiceHashData != null) {
                    ratio = Globals.NiceHashData[NiceHashID].paying.ToString("F8");
                }
                return ratio;
            }
        }
        [JsonIgnore]
        public string CurPayingRate {
            get {
                string rate = International.GetText("BenchmarkRatioRateN_A");
                if (BenchmarkSpeed > 0 && Globals.NiceHashData != null) {
                    rate = (BenchmarkSpeed * Globals.NiceHashData[NiceHashID].paying * 0.000000001).ToString("F8");
                }
                return rate;
            }
        }

        public Algorithm(AlgorithmType niceHashID, string minerName) {
            NiceHashID = niceHashID;
            NiceHashName = AlgorithmNiceHashNames.GetName(niceHashID);
            MinerName = minerName;

            BenchmarkSpeed = 0.0d;
            ExtraLaunchParameters = "";
            LessThreads = 0;
            Skip = false;
            BenchmarkStatus = "";
        }

        public void SetBenchmarkPending() {
            IsBenchmarkPending = true;
            BenchmarkStatus = International.GetText("Algorithm_Waiting_Benchmark");
        }
        public void SetBenchmarkPendingNoMsg() {
            IsBenchmarkPending = true;
        }
        public void ClearBenchmarkPending() {
            IsBenchmarkPending = false;
            BenchmarkStatus = "";
        }

        public string BenchmarkSpeedString() {
            if (!Skip && IsBenchmarkPending && !string.IsNullOrEmpty(BenchmarkStatus)) {
                return BenchmarkStatus;
            } else if (BenchmarkSpeed > 0) {
                return Helpers.FormatSpeedOutput(BenchmarkSpeed);
            }
            return International.GetText("BenchmarkSpeedStringNone");
        }
    }
}
