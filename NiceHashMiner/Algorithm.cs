using Newtonsoft.Json;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner {
    public class Algorithm {

        public readonly string AlgorithmName;
        public readonly string SecondaryAlgorithmName;
        public readonly string MinerBaseTypeName;
        public readonly AlgorithmType NiceHashID;
        public readonly AlgorithmType SecondaryNiceHashID;
        public readonly MinerBaseType MinerBaseType;
        public readonly string AlgorithmStringID;
        // Miner name is used for miner ALGO flag parameter
        public readonly string MinerName;
        public double BenchmarkSpeed { get; set; }
        public double SecondaryBenchmarkSpeed { get; set; }
        public string ExtraLaunchParameters { get; set; }
        public bool Enabled { get; set; }

        // CPU miners only setting
        public int LessThreads { get; set; }

        // avarage speed of same devices to increase mining stability
        public double AvaragedSpeed { get; set; }
        // based on device and settings here we set the miner path
        public string MinerBinaryPath = "";
        // these are changing (logging reasons)
        public double CurrentProfit = 0;
        public double CurNhmSMADataVal = 0;

        public Algorithm(MinerBaseType minerBaseType, AlgorithmType niceHashID, string minerName) {
            this.AlgorithmName = AlgorithmNiceHashNames.GetName(niceHashID);
            this.MinerBaseTypeName = Enum.GetName(typeof(MinerBaseType), minerBaseType);
            this.AlgorithmStringID = this.MinerBaseTypeName + "_" + this.AlgorithmName;

            MinerBaseType = minerBaseType;
            NiceHashID = niceHashID;
            SecondaryNiceHashID = AlgorithmType.NONE;
            MinerName = minerName;

            BenchmarkSpeed = 0.0d;
            SecondaryBenchmarkSpeed = 0.0d;
            ExtraLaunchParameters = "";
            LessThreads = 0;
            Enabled = true;
            BenchmarkStatus = "";
        }

        // for ClaymoreDual algos
        public Algorithm(MinerBaseType minerBaseType, AlgorithmType niceHashID, AlgorithmType secondaryNiceHashID, string minerName) {
            this.AlgorithmName = AlgorithmNiceHashNames.GetName(niceHashID);
            this.SecondaryAlgorithmName = AlgorithmNiceHashNames.GetName(secondaryNiceHashID);
            this.MinerBaseTypeName = Enum.GetName(typeof(MinerBaseType), minerBaseType);
            this.AlgorithmStringID = this.MinerBaseTypeName + "_" + this.AlgorithmName + "_" + this.SecondaryAlgorithmName;

            MinerBaseType = minerBaseType;
            NiceHashID = niceHashID;
            SecondaryNiceHashID = secondaryNiceHashID;
            MinerName = minerName;

            BenchmarkSpeed = 0.0d;
            SecondaryBenchmarkSpeed = 1.0d;
            ExtraLaunchParameters = "";
            LessThreads = 0;
            Enabled = false;
            BenchmarkStatus = "";
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
                if (SecondaryNiceHashID != AlgorithmType.NONE) {
                    ratio += "/" + Globals.NiceHashData[SecondaryNiceHashID].paying.ToString("F8");
                }
                return ratio;
            }
        }
        public string CurPayingRate {
            get {
                string rate = International.GetText("BenchmarkRatioRateN_A");
                var payingRate = 0.0d;
                if (Globals.NiceHashData != null) {
                    rate = (BenchmarkSpeed * Globals.NiceHashData[NiceHashID].paying * 0.000000001).ToString("F8");
                    if (BenchmarkSpeed > 0)  {
                        payingRate += BenchmarkSpeed * Globals.NiceHashData[NiceHashID].paying * 0.000000001;
                    }
                    if (SecondaryBenchmarkSpeed > 0 && SecondaryNiceHashID != AlgorithmType.NONE) {
                        payingRate += SecondaryBenchmarkSpeed * Globals.NiceHashData[SecondaryNiceHashID].paying * 0.000000001;
                    }
                    rate = payingRate.ToString("F8");
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

        private bool IsPendingString() {
            return BenchmarkStatus == International.GetText("Algorithm_Waiting_Benchmark")
                || BenchmarkStatus == "."
                || BenchmarkStatus == ".."
                || BenchmarkStatus == "...";
        }

        public void ClearBenchmarkPending() {
            IsBenchmarkPending = false;
            if (IsPendingString()) {
                BenchmarkStatus = "";
            }
        }

        public void ClearBenchmarkPendingFirst() {
            IsBenchmarkPending = false;
            BenchmarkStatus = "";
        }

        public string BenchmarkSpeedString() {
            if (Enabled && IsBenchmarkPending && !string.IsNullOrEmpty(BenchmarkStatus)) {
                return BenchmarkStatus;
            } else if (BenchmarkSpeed > 0) {
                var speedString = Helpers.FormatSpeedOutput(BenchmarkSpeed);
                if (SecondaryNiceHashID != AlgorithmType.NONE)  {
                    speedString += "/" + Helpers.FormatSpeedOutput(SecondaryBenchmarkSpeed);
                }
                return speedString;
            } else if (!IsPendingString() && !string.IsNullOrEmpty(BenchmarkStatus)) {
                return BenchmarkStatus;
            }
            return International.GetText("BenchmarkSpeedStringNone");
        }

        // convenience for formatting name text
        public string FormattedSecondaryName() {
            if (SecondaryAlgorithmName != null)
            {
                return "/" + SecondaryAlgorithmName;
            }
            return "";
        }
    }
}
