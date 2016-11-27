using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs.Data
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
        readonly static public int SIZE = 3;
        #endregion CONSTANTS

        #region PRIVATES
        private int[] _benchmarkTimeLimitsCPU = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
        private int[] _benchmarkTimeLimitsNVIDIA = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
        private int[] _benchmarkTimeLimitsAMD = MemoryHelper.DeepClone(DEFAULT_AMD);

        private bool isValid(int[] value) { return value != null && value.Length == SIZE; }
        #endregion PRIVATES

        #region PROPERTIES
        public int[] CPU {
            get { return _benchmarkTimeLimitsCPU; }
            set {
                if (isValid(value)) {
                    _benchmarkTimeLimitsCPU = MemoryHelper.DeepClone(value);
                }
                else {
                    _benchmarkTimeLimitsCPU = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
                }
            }
        }
        public int[] NVIDIA {
            get { return _benchmarkTimeLimitsNVIDIA; }
            set {
                if (isValid(value)) {
                    _benchmarkTimeLimitsNVIDIA = MemoryHelper.DeepClone(value);
                } else {
                    _benchmarkTimeLimitsNVIDIA = MemoryHelper.DeepClone(DEFAULT_CPU_NVIDIA);
                }
            }
        }
        public int[] AMD {
            get { return _benchmarkTimeLimitsAMD; }
            set {
                if (isValid(value)) {
                    _benchmarkTimeLimitsAMD = MemoryHelper.DeepClone(value);
                } else {
                    _benchmarkTimeLimitsAMD = MemoryHelper.DeepClone(DEFAULT_AMD);
                }
            }
        }
        #endregion PROPERTIES

        public int GetBenchamrktime(BenchmarkPerformanceType benchmarkPerformanceType, DeviceGroupType deviceGroupType) {
            if (deviceGroupType == DeviceGroupType.CPU) {
                return CPU[(int)benchmarkPerformanceType];
            }
            if (deviceGroupType == DeviceGroupType.AMD_OpenCL) {
                return AMD[(int)benchmarkPerformanceType];
            }

            return NVIDIA[(int)benchmarkPerformanceType];
        }

    }
}
