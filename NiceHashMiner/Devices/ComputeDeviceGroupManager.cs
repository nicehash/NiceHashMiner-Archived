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
    public class ComputeDeviceGroupManager
    {
        #region SINGLETON Stuff
        private static ComputeDeviceGroupManager _instance = new ComputeDeviceGroupManager();

        public static ComputeDeviceGroupManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ComputeDeviceGroupManager();
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Use enum types, we could use a list here but keep dict for now
        /// </summary>
        Dictionary<DeviceGroupType, ComputeDeviceGroup> _groups;

        private ComputeDeviceGroupManager()
        {
            // we create our groups
            _groups = new Dictionary<DeviceGroupType, ComputeDeviceGroup>();
            // TODO we have 5 used groups for now add NVIDIA_6_x later
            for (int i = 0; i < 5; ++i)
            {
                DeviceGroupType curType = (DeviceGroupType)i;
                _groups.Add(curType, new ComputeDeviceGroup(curType));
            }
        }

        public void AddDevice(ComputeDevice computeDevice) {
            // TODO this is based on the current Miners implementation
            ComputeDeviceGroup selectedGroup = null;
            bool isGetFound = false;
            // check and get group for vendor
            switch (computeDevice.Vendor)
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
                    bool isCPU = computeDevice.Vendor.Contains("CPU");
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
                Helpers.ConsolePrint("ComputeDeviceGroupManager", computeDevice.Vendor + " group not found or null");
            }
        }

        
    }
}
