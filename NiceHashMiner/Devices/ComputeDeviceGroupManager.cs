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
    public class ComputeDeviceGroupManager : BaseLazySingleton<ComputeDeviceGroupManager>
    {

        private Dictionary<DeviceGroupType, int> _groupCount;
        
        DeviceGroupType[] _gpuGroups = new DeviceGroupType[] {
            DeviceGroupType.AMD_OpenCL,
            DeviceGroupType.NVIDIA_2_1,
            DeviceGroupType.NVIDIA_3_x,
            DeviceGroupType.NVIDIA_5_x,
            DeviceGroupType.NVIDIA_6_x
        };

        protected ComputeDeviceGroupManager()
            : base() {

            _groupCount = new Dictionary<DeviceGroupType, int>();
            for (DeviceGroupType type = 0; type < DeviceGroupType.LAST; ++type) {
                _groupCount.Add(type, 0);
            }
        }

        public int GetGroupCount(DeviceGroupType type) {
            return _groupCount[type];
        }

        public void AddDevice(ComputeDevice computeDevice) {
            _groupCount[computeDevice.DeviceGroupType]++;
        }

        public void DisableCpuGroup() {
            foreach (var device in ComputeDevice.AllAvaliableDevices) {
                if (device.DeviceGroupType == DeviceGroupType.CPU) {
                    device.Enabled = false;
                }
            }
        }
        public bool ContainsGPUs {
            get {
                foreach (var groupType in _gpuGroups) {
                    if (_groupCount[groupType] > 0) {
                        return true;
                    }
                }
                return false;
            }
        }

    }
}
