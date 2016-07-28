using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Enums;

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
        [JsonIgnore]
        readonly public DeviceGroupType DeviceGroupType;

        // 
        readonly public static List<ComputeDevice> AllAvaliableDevices = new List<ComputeDevice>();
        readonly public static List<ComputeDevice> UniqueAvaliableDevices = new List<ComputeDevice>();

        public ComputeDevice(int id, string vendor, string name, Miner miner, bool addToGlobalList = false, bool enabled = true)
        {
            ID = id;
            Vendor = vendor;
            Name = name;
            Enabled = enabled;
            // TODO temp solution
            Miner = miner;
            if (addToGlobalList) {
                // add to all devices
                AllAvaliableDevices.Add(this);
                // compare new device with unique list scope
                {
                    bool isNewUnique = true;
                    foreach (var d in UniqueAvaliableDevices) {
                        if(this.Name == d.Name) {
                            isNewUnique = false;
                            break;
                        }
                    }
                    if (isNewUnique) {
                        UniqueAvaliableDevices.Add(this);
                    }
                }
                // add to group manager
                ComputeDeviceGroupManager.Instance.AddDevice(this);
                DeviceGroupType = GroupNames.GetType(Vendor);
            }
            
        }
    }
}
