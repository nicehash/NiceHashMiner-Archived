using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Enums;
using System.Security.Cryptography;
using NiceHashMiner.Configs;

namespace NiceHashMiner.Devices
{
    [Serializable]
    public class ComputeDevice
    {
        readonly public int ID;
        readonly public string Group;
        readonly public string Name;
        public bool Enabled;
        
        [JsonIgnore]
        readonly public DeviceGroupType DeviceGroupType;
        // close to uuid, hash readonly members and we should be safe
        // it is used only at runtime, do not save to configs
        [JsonIgnore]
        readonly public string UUID;

        // temp value for grouping new profits
        [JsonIgnore]
        public Algorithm MostProfitableAlgorithm { get; set; }

        [JsonIgnore]
        public DeviceBenchmarkConfig DeviceBenchmarkConfig { get; private set; }

        // 
        readonly public static List<ComputeDevice> AllAvaliableDevices = new List<ComputeDevice>();
        readonly public static List<ComputeDevice> UniqueAvaliableDevices = new List<ComputeDevice>();

        public ComputeDevice(int id, string group, string name, Miner miner, bool addToGlobalList = false, bool enabled = true)
        {
            ID = id;
            Group = group;
            Name = name;
            Enabled = enabled;
            DeviceGroupType = GroupNames.GetType(Group);
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
            }
            UUID = GetUUID(ID, Group, Name, DeviceGroupType);
        }

        public void SetDeviceBenchmarkConfig(DeviceBenchmarkConfig deviceBenchmarkConfig) {
            DeviceBenchmarkConfig = deviceBenchmarkConfig;
        }

        public static ComputeDevice GetDeviceWithUUID(string uuid) {
            foreach (var dev in AllAvaliableDevices) {
                if (uuid == dev.UUID) return dev;
            }
            return null;
        }

        public static int GetEnabledDeviceNameCount(string name) {
            int count = 0;
            foreach (var dev in AllAvaliableDevices) {
                if (dev.Enabled && name == dev.Name) ++count;
            }
            return count;
        }

        public static string GetUUID(int id, string group, string name, DeviceGroupType deviceGroupType) {
            var SHA256 = new SHA256Managed();
            var hash = new StringBuilder();
            string mixedAttr = id.ToString() + group + name + ((int)deviceGroupType).ToString();
            byte[] hashedBytes = SHA256.ComputeHash(Encoding.UTF8.GetBytes(mixedAttr), 0, Encoding.UTF8.GetByteCount(mixedAttr));
            foreach (var b in hashedBytes) {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }

        public static List<ComputeDevice> GetEnabledDevices() {
            List<ComputeDevice> enabledCDevs = new List<ComputeDevice>();

            foreach (var dev in AllAvaliableDevices) {
                if (dev.Enabled) enabledCDevs.Add(dev);
            }

            return enabledCDevs;
        }

        public static string GetEnabledDeviceUUIDForName(string name) {
            foreach (var dev in AllAvaliableDevices) {
                if (dev.Name == name) return dev.UUID;
            }
            return null;
        }

    }
}
