using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceGroupManager class manages all the avaliable groups.
    /// For now used only for the settings.
    /// </summary>
    public static class ComputeDeviceGroupManager {
        public static void DisableCpuGroup() {
            foreach (var device in ComputeDevice.AllAvaliableDevices) {
                if (device.DeviceType == DeviceType.CPU) {
                    device.Enabled = false;
                }
            }
        }

        public static bool ContainsAMD_GPUs {
            get {
                foreach (var device in ComputeDevice.AllAvaliableDevices) {
                    if (device.DeviceType == DeviceType.AMD) {
                        return true;
                    }
                }
                return false;
            }
        }

        public static bool ContainsGPUs {
            get {
                foreach (var device in ComputeDevice.AllAvaliableDevices) {
                    if (device.DeviceType == DeviceType.NVIDIA
                        || device.DeviceType == DeviceType.AMD) {
                        return true;
                    }
                }
                return false;
            }
        }

    }
}
