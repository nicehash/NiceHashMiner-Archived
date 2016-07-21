using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Devices
{
    /// <summary>
    /// ComputeDeviceGroup class is used just to track if devices in group are enabled.
    /// If only one device from group is enabled then it means the group is enabled
    /// </summary>
    public class ComputeDeviceGroup
    {
        // here we save references devices for the group
        private List<ComputeDevice> _devices;
        // references just for unique devices (like a set of different card types), will not save same card type more then once
        private List<ComputeDevice> _uniqueDevices;

        readonly public DeviceGroupType Type;
        readonly public string Name;

        public bool IsEnabled
        {
            get
            {
                int enabledCount = 0;
                foreach (var device in _devices) {
                    enabledCount += device.Enabled ? 1 : 0;
                }
                return enabledCount != 0;
            }
        }

        public ComputeDeviceGroup(DeviceGroupType type)
        {
            _devices = new List<ComputeDevice>();
            _uniqueDevices = new List<ComputeDevice>();
            Type = type;
            // TODO will work for now different logic for CPU
            Name = GroupNames.GetName(type);
        }

        public void AddNewDevice(ComputeDevice device)
        {
            // TODO maybe check if already added or something
            _devices.Add(device);
            addUniqueDevice(device);
        }

        private void addUniqueDevice(ComputeDevice device) {
            bool containsModel = false;
            foreach (var curCDev in _uniqueDevices) {
                // Vendor is the same for the group
                if (curCDev.Name == device.Name /*&& curCDev.Vendor == device.Vendor*/) {
                    containsModel = true;
                    break;
                }
            }
            if (!containsModel) {
                _uniqueDevices.Add(device);
            }
        }
    }
}
