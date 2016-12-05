using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NiceHashMiner.Utils {
    public static class MinersExistanceChecker {
        public static bool IsMinersBins_ALL_Init() {
            foreach (var filePath in Bins_Data.ALL_FILES_BINS) {
                if (!File.Exists(String.Format("bin{0}", filePath))) {
                    return false;
                }
            }
            return true;
        }

        public static bool IsMiners3rdPartyBinsInit() {
            foreach (var filePath in Bins_Data_3rd.ALL_FILES_BINS) {
                if (!File.Exists(String.Format("bin_3rdparty{0}", filePath))) {
                    return false;
                }
            }
            return true;
        }

        public static bool IsMinersBinsInit() {
            //return isOk;
            return IsMinersBins_ALL_Init();
        }
    }
}
