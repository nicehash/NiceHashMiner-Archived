using Newtonsoft.Json;
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
        //TODO now we have a cyclic dependancy, redo and rethink this in the future
        // the miner we dont want to serialize
        [JsonIgnore]
        readonly public Miner Miner;

        // 
        readonly public static List<ComputeDevice> AllAvaliableDevices = new List<ComputeDevice>();

        public ComputeDevice(int id, string vendor, string name, Miner miner, bool enabled = true)
        {
            ID = id;
            Vendor = vendor;
            Name = name;
            Enabled = enabled;
            // TODO temp solution
            Miner = miner;
            // add to all devices
            AllAvaliableDevices.Add(this);
            // add to group manager
            ComputeDeviceGroupManager.Instance.AddDevice(this);
        }
    }
}
