using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Net20_backport {
    public static class StringHelper {
        public static string Join(string delim, IList<string> values) {
            if (values.Count == 1) {
                return values[0];
            }
            string ret = "";
            if (values.Count > 1) {
                for (int i = 0; i < values.Count - 1; ++i) {
                    ret += values[i] + delim;
                }
                // append last
                ret += values[values.Count - 1];
            }
            return ret;
        }

        public static bool IsNullOrWhiteSpace(string check) {
            if (check == null) {
                return true;
            }
            if (check == " ") {
                return true;
            }
            return false;
        }
    }
}
