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
        /// <summary>
        /// Use enum types, we could use a list here but keep dict for now
        /// </summary>
        private Dictionary<DeviceGroupType, ComputeDeviceGroup> _groups;

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
            DeviceGroupType.NVIDIA_5_x
        };


        protected ComputeDeviceGroupManager()
            : base() {
            // we create our groups
            _groups = new Dictionary<DeviceGroupType, ComputeDeviceGroup>();
            // TODO we have 5 used groups for now add NVIDIA_6_x later
            for (int i = 0; i < 5; ++i) {
                DeviceGroupType curType = (DeviceGroupType)i;
                _groups.Add(curType, new ComputeDeviceGroup(curType));
            }
        }

        public int GetGroupCount(DeviceGroupType type) {
            ComputeDeviceGroup group;
            if (_groups.TryGetValue(type, out group)) {
                return group.Count;
            } 
            return 0;
            
        }

        public void AddDevice(ComputeDevice computeDevice) {
            // TODO this is based on the current Miners implementation it is bound to change
            ComputeDeviceGroup selectedGroup = null;
            bool isGetFound = false;
            // check and get group for vendor
            switch (computeDevice.Group)
            {
                case "NVIDIA5.x":
                    isGetFound = _groups.TryGetValue(DeviceGroupType.NVIDIA_5_x, out selectedGroup);
                    break;
                case "NVIDIA3.x":
                    isGetFound = _groups.TryGetValue(DeviceGroupType.NVIDIA_3_x, out selectedGroup);
                    break;
                case "NVIDIA2.1":
                    isGetFound = _groups.TryGetValue(DeviceGroupType.NVIDIA_2_1, out selectedGroup);
                    break;
                case "AMD_OpenCL":
                    isGetFound = _groups.TryGetValue(DeviceGroupType.AMD_OpenCL, out selectedGroup);
                    break;
                default:
                    bool isCPU = computeDevice.Group.Contains("CPU");
                    if (isCPU) {
                        isGetFound = _groups.TryGetValue(DeviceGroupType.CPU, out selectedGroup);
                    } else {
                        Helpers.ConsolePrint("ComputeDeviceGroupManager", "ComputeDevice Vendor not recognized");
                    }
                    break;
            }
            if (isGetFound && selectedGroup != null) {
                selectedGroup.AddNewDevice(computeDevice);
            }
            else {
                Helpers.ConsolePrint("ComputeDeviceGroupManager", computeDevice.Group + " group not found or null");
            }
        }

        public bool IsGroupEnabled(DeviceGroupType deviceGroupType)
        {
            ComputeDeviceGroup selectedGroup = null;
            bool isGetFound = _groups.TryGetValue(deviceGroupType, out selectedGroup);
            return isGetFound && selectedGroup.IsEnabled;
        }

        public void DisableCpuGroup() {
            _groups[DeviceGroupType.CPU].DisableGroup();
        }
        public bool ContainsGPUs {
            get {
                foreach (var groupType in _gpuGroups) {
                    if (_groups[groupType].Count > 0) {
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


        private static int cpuCount = 0;
        private DeviceGroupConfig CreateGroupSettings(string vendor, DeviceGroupType groupType) {
            bool isInitSuccess = true;
            int APIBindPort = -1;
            switch(groupType) {
                case DeviceGroupType.CPU:
                    APIBindPort = 4040 + cpuCount;
                    ++cpuCount;
                    break;
                case DeviceGroupType.AMD_OpenCL:
                    APIBindPort = 4050;
                    break;
                case DeviceGroupType.NVIDIA_2_1:
                    APIBindPort = 4021;
                    break;
                case DeviceGroupType.NVIDIA_3_x:
                    APIBindPort = 4049;
                    break;
                case DeviceGroupType.NVIDIA_5_x:
                    APIBindPort = 4048;
                    break;
                default:
                    isInitSuccess = false;
                    break;
            }
            if (isInitSuccess && APIBindPort > 0) {
                return new DeviceGroupConfig(vendor) {
                    APIBindPort = APIBindPort
                };
            }
            return null;
        }

    }
}
