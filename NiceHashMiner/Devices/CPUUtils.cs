using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices {
    public static class CPUUtils {
        // this is the order we check and initialize if automatic
        private static CPUExtensionType[] _detectOrder = new CPUExtensionType[] { 
                CPUExtensionType.AVX2_AES,
                CPUExtensionType.AVX2,
                CPUExtensionType.AVX_AES,
                CPUExtensionType.AVX,
                CPUExtensionType.AES,
                CPUExtensionType.SSE2, // disabled
            };

        /// <summary>
        /// HasExtensionSupport checks CPU extensions support, if type automatic just return false.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>False if type Automatic otherwise True if supported</returns>
        private static bool HasExtensionSupport(CPUExtensionType type) {
            switch (type) {
                case CPUExtensionType.AVX2_AES: return (CPUID.SupportsAVX2() == 1) && (CPUID.SupportsAES() == 1);
                case CPUExtensionType.AVX2: return CPUID.SupportsAVX2() == 1;
                case CPUExtensionType.AVX_AES: return (CPUID.SupportsAVX() == 1) && (CPUID.SupportsAES() == 1);
                case CPUExtensionType.AVX: return CPUID.SupportsAVX() == 1;
                case CPUExtensionType.AES: return CPUID.SupportsAES() == 1;
                case CPUExtensionType.SSE2: return CPUID.SupportsSSE2() == 1;
                default: // CPUExtensionType.Automatic
                    break;
            }
            return false;
        }

        ///// <summary>
        ///// Returns most performant CPU extension based on settings.
        ///// Returns automatic if NO extension is avaliable
        ///// </summary>
        ///// <returns></returns>
        //public static CPUExtensionType GetMostOptimized() {
        //    if (ConfigManager.GeneralConfig.ForceCPUExtension == CPUExtensionType.Automatic) {
        //        for (int i = 0; i < _detectOrder.Length; ++i) {
        //            if (HasExtensionSupport(_detectOrder[i])) {
        //                return _detectOrder[i];
        //            }
        //        }
        //    } else if (HasExtensionSupport(ConfigManager.GeneralConfig.ForceCPUExtension)) {
        //        return ConfigManager.GeneralConfig.ForceCPUExtension;
        //    }
        //    return CPUExtensionType.Automatic;
        //}

        /// <summary>
        /// Checks if CPU mining is capable, CPU must have AES support
        /// </summary>
        /// <returns></returns>
        public static bool IsCPUMiningCapable() {
            return HasExtensionSupport(CPUExtensionType.AES);
        }
    }
}
