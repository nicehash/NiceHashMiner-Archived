using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs
{
    /// <summary>
    /// BenchmarkTimeLimitsConfig is used to set the time limits for benchmarking.
    /// There are three types: Quick, Standard,Precise (look at BenchmarkType.cs).
    /// </summary>
    /// 
    [Serializable]
    public class BenchmarkTimeLimitsConfig
    {
        #region CONSTANTS
        [field: NonSerialized]
        readonly static private int[] DEFAULT_CPU_NVIDIA = { 10, 20, 60 };
        [field: NonSerialized]
        readonly static private int[] DEFAULT_AMD = { 120, 180, 240 };
        [field: NonSerialized]
        readonly static private int SIZE = 3;
        #endregion CONSTANTS

        #region PRIVATES
        private int[] _benchmarkTimeLimitsCPU = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
        private int[] _benchmarkTimeLimitsNVIDIA = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
        private int[] _benchmarkTimeLimitsAMD = MemoryHelper.DeepClone(DEFAULT_AMD);

        private bool isValid(int[] value) { return value != null && value.Length == SIZE; }
        private bool notValid(int[] value) { return value == null || value.Length < SIZE; }
        #endregion PRIVATES

        #region PROPERTIES
        public int[] CPU {
            get { return _benchmarkTimeLimitsCPU; }
            set {
                if (notValid(value)) {
                    _benchmarkTimeLimitsCPU = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
                }
                else {
                    _benchmarkTimeLimitsCPU = MemoryHelper.DeepClone(value);
                }
            }
        }
        public int[] NVIDIA {
            get { return _benchmarkTimeLimitsNVIDIA; }
            set {
                if (notValid(value)) {
                    _benchmarkTimeLimitsNVIDIA = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
                } else {
                    _benchmarkTimeLimitsNVIDIA = MemoryHelper.DeepClone(value);
                }
            }
        }
        public int[] AMD {
            get { return _benchmarkTimeLimitsAMD; }
            set {
                if (notValid(value)) {
                    _benchmarkTimeLimitsAMD = MemoryHelper.DeepClone(DEFAULT_AMD);
                } else {
                    _benchmarkTimeLimitsAMD = MemoryHelper.DeepClone(value);
                }
            }
        }
        #endregion PROPERTIES
    }
}
