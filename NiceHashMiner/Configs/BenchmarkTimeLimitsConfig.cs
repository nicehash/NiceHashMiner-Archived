using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs
{
    /// <summary>
    /// BenchmarkTimeLimitsConfig is used to set the time limits for benchmarking.
    /// There are three types: Quick, Standard,Precise (look at BenchmarkType.cs).
    /// </summary>
    public class BenchmarkTimeLimitsConfig
    {
        #region CONSTANTS
        readonly static private int[] DEFAULT_CPU_NVIDIA = { 10, 20, 60 };
        readonly static private int[] DEFAULT_AMD = { 120, 180, 240 };
        readonly static private int SIZE = 3;
        #endregion CONSTANTS

        #region PRIVATES
        private int[] _benchmarkTimeLimitsCPU = DEFAULT_CPU_NVIDIA;
        private int[] _benchmarkTimeLimitsNVIDIA = DEFAULT_CPU_NVIDIA;
        private int[] _benchmarkTimeLimitsAMD = DEFAULT_AMD;

        private bool isValid(int[] value) { return value != null && value.Length == SIZE; }
        private bool notValid(int[] value) { return value == null || value.Length < SIZE; }
        #endregion PRIVATES

        #region PROPERTIES
        public int[] BenchmarkTimeLimitsCPU {
            get { return _benchmarkTimeLimitsCPU; }
            set {
                if (notValid(value)) {
                    _benchmarkTimeLimitsCPU = DEFAULT_CPU_NVIDIA;
                }
                else {
                    _benchmarkTimeLimitsCPU = value;
                }
            }
        }
        public int[] BenchmarkTimeLimitsNVIDIA {
            get { return _benchmarkTimeLimitsNVIDIA; }
            set {
                if (notValid(value)) {
                    _benchmarkTimeLimitsNVIDIA = DEFAULT_CPU_NVIDIA;
                } else {
                    _benchmarkTimeLimitsNVIDIA = value;
                }
            }
        }
        public int[] BenchmarkTimeLimitsAMD {
            get { return _benchmarkTimeLimitsAMD; }
            set {
                if (notValid(value)) {
                    _benchmarkTimeLimitsAMD = DEFAULT_AMD;
                } else {
                    _benchmarkTimeLimitsAMD = value;
                }
            }
        }
        #endregion PROPERTIES
    }
}
