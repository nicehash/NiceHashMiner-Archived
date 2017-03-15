using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices
{
    static public class GroupNames
    {
        private static readonly string[] _names = {
                                        "CPU", // we can have more then one CPU
                                        "AMD_OpenCL",
                                        "NVIDIA2.1",
                                        "NVIDIA3.x",
                                        "NVIDIA5.x",
                                        "NVIDIA6.x",
                                                  };

        public static string GetGroupName(DeviceGroupType type, int id) {
            if(DeviceGroupType.CPU == type) {
                return "CPU"+id;
            } else if ((int)type < _names.Length && (int)type >= 0) {
                return _names[(int)type];
            }
            return "UnknownGroup";
        }

        public static string GetNameGeneral(DeviceType type) {
            if(DeviceType.CPU == type) {
                return "CPU";
            } else if(DeviceType.NVIDIA == type) {
                return "NVIDIA";
            } else if (DeviceType.AMD == type) {
                return "AMD";
            } 
            return "UnknownDeviceType";
        }
    }
}
