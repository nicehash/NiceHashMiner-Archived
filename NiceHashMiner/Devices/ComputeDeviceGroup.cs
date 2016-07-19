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
        // here we save references to our devices
        private List<ComputeDevice> _devices;

        readonly public DeviceGroupType Type;

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
            Type = type;
        }

        public void AddNewDevice(ComputeDevice device)
        {
            // TODO maybe check if already added or something
            _devices.Add(device);
        }
    }
}
