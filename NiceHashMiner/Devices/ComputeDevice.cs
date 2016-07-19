using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices
{
    public class ComputeDevice
    {
        readonly public int ID;
        readonly public string Vendor;
        readonly public string Name;
        public bool Enabled;

        // 
        readonly public static List<ComputeDevice> AllAvaliableDevices = new List<ComputeDevice>();

        public ComputeDevice(int id, string vendor, string name, bool enabled = true)
        {
            ID = id;
            Vendor = vendor;
            Name = name;
            Enabled = enabled;
            // add to all devices
            AllAvaliableDevices.Add(this);
            // add to group manager
            ComputeDeviceGroupManager.Instance.AddDevice(this);
        }
    }
}
