using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Enums;
using System.Security.Cryptography;

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
            // TODO temp solution
            //Miner = miner;
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

        public static ComputeDevice GetDeviceWithUUID(string uuid) {
            foreach (var dev in AllAvaliableDevices) {
                if (uuid == dev.UUID) return dev;
            }
            return null;
        }

        public static ComputeDevice GetUniqueDeviceWithName(string name) {
            foreach (var dev in UniqueAvaliableDevices) {
                if (name == dev.Name) return dev;
            }
            return null;
        }

        public static int GetDeviceNameCount(string name) {
            int count = 0;
            foreach (var dev in AllAvaliableDevices) {
                if (name == dev.Name) ++count;
            }
            return count;
        }

        public static string[] GetEnabledDevicesUUUIDsForNames(string[] deviceNames) {
            List<string> uuids = new List<string>();

            foreach (var dev in AllAvaliableDevices) {
                foreach (var devName in deviceNames) {
                    if (dev.Name == devName) {
                        uuids.Add(dev.UUID);
                    }
                }
            }

            return uuids.ToArray();
        }

        public static string[] GetEnabledDevicesUUUIDsForNames(SortedSet<string> uuidsSet) {
            string[] deviceNames = new string[uuidsSet.Count];
                int devNamesIndex = 0;
                foreach (var uuid in uuidsSet) {
                    deviceNames[devNamesIndex++] = ComputeDevice.GetDeviceWithUUID(uuid).Name;
                }

            return GetEnabledDevicesUUUIDsForNames(deviceNames);
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

        public static HashSet<string> GetUniqueEnabledDevicesUUIDsForGroup(DeviceGroupType type) {
            HashSet<string> uuids = new HashSet<string>();

            foreach (var cd in UniqueAvaliableDevices) {
                if (cd.Enabled && cd.DeviceGroupType == type) {
                    uuids.Add(cd.UUID);
                }
            }

            return uuids;
        }

        public static HashSet<string> GetUniqueEnabledDevicesNamesForGroup(DeviceGroupType type) {
            HashSet<string> names = new HashSet<string>();

            foreach (var cd in UniqueAvaliableDevices) {
                if (cd.Enabled && cd.DeviceGroupType == type) {
                    names.Add(cd.Name);
                }
            }

            return names;
        }

    }
}
