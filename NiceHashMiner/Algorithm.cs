using Newtonsoft.Json;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner {
    public class Algorithm {
        readonly public AlgorithmType NiceHashID;
        // Miner name is used for miner ALGO flag parameter
        readonly public string MinerName;
        public double BenchmarkSpeed { get; set; }
        public string ExtraLaunchParameters { get; set; }
        public bool Skip { get; set; }

        // CPU miners only setting
        public int LessThreads { get; set; }

        // avarage speed of same devices to increase mining stability
        public double AvaragedSpeed { get; set; }

        public Algorithm(AlgorithmType niceHashID, string minerName) {
            NiceHashID = niceHashID;
            MinerName = minerName;

            BenchmarkSpeed = 0.0d;
            ExtraLaunchParameters = "";
            LessThreads = 0;
            Skip = false;
            BenchmarkStatus = "";
        }

        public string GetName() {
            return AlgorithmNiceHashNames.GetName(this.NiceHashID);
        }

        // benchmark info
        public string BenchmarkStatus { get; set; }
        public bool IsBenchmarkPending { get; private set; }
        public string CurPayingRatio {
            get {
                string ratio = International.GetText("BenchmarkRatioRateN_A");
                if (Globals.NiceHashData != null) {
                    ratio = Globals.NiceHashData[NiceHashID].paying.ToString("F8");
                }
                return ratio;
            }
        }
        public string CurPayingRate {
            get {
                string rate = International.GetText("BenchmarkRatioRateN_A");
                if (BenchmarkSpeed > 0 && Globals.NiceHashData != null) {
                    rate = (BenchmarkSpeed * Globals.NiceHashData[NiceHashID].paying * 0.000000001).ToString("F8");
                }
                return rate;
            }
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
