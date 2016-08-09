using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceGroupManager class manages all the avaliable groups.
    /// For now used only for the settings.
    /// TODO for now we do not detect SM 6.x, NVIDIA6x
    /// </summary>
    public class ComputeDeviceGroupManager : BaseLazySingleton<ComputeDeviceGroupManager>
    {

        private Dictionary<DeviceGroupType, int> _groupCount;

        // TODO for now string CPU are divided in diferent groups
        private Dictionary<string, DeviceGroupConfig> _groupSettings;
        public Dictionary<string, DeviceGroupConfig> GroupSettings {
            get { return _groupSettings; }
            set {
                if (value == null) return;
                _groupSettings = value;
            }
        }

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

        // group settings hardcoded maybe we won't need this in the future
        public void InitializeGroupSettings() {
            _groupSettings = new Dictionary<string, DeviceGroupConfig>();
            foreach (var device in ComputeDevice.AllAvaliableDevices) {
                if (_groupSettings.ContainsKey(device.Group) == false) {
                    _groupSettings.Add(device.Group, CreateGroupSettings(device.Group, device.DeviceGroupType));
                }
            }
        }

        public DeviceGroupConfig GetDeviceGroupSettings(string vendor) {
            if (_groupSettings.ContainsKey(vendor)) {
                return _groupSettings[vendor];
            }
            return null;
        }


        private DeviceGroupConfig CreateGroupSettings(string vendor, DeviceGroupType groupType) {
            return new DeviceGroupConfig(vendor);
        }

    }
}
