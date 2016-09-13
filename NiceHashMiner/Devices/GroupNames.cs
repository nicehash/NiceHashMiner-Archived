using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices
{
    static public class GroupNames
    {
        private static readonly string[] _names = {
                                        "CPU", // TODO we can have more then one CPU
                                        "AMD_OpenCL",
                                        "NVIDIA2.1",
                                        "NVIDIA3.x",
                                        "NVIDIA5.x",
                                        "NVIDIA6.x",
                                                  };

        private static readonly string[] _namesGeneral = {
                                        "CPU",
                                        "AMD",
                                        "NVIDIA",
                                        "NVIDIA",
                                        "NVIDIA",
                                        "NVIDIA",
                                                  };

        public static string GetName(DeviceGroupType type) { return _names[(int)type]; }

        public static string GetNameGeneral(DeviceGroupType type) { return _namesGeneral[(int)type]; }

        public static DeviceGroupType GetType(string name) {
            int i = 0;
            for (; i < _names.Length; ++i) {
                if (name.Contains(_names[i])) break;
            }
            return (DeviceGroupType)i;
        }

    }
}
