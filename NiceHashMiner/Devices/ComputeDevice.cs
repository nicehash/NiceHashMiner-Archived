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
        //[JsonIgnore]
        //readonly public int PlatformId;
        readonly public int ID;
        readonly public string Group;
        readonly public string Name;
        public bool Enabled;

        public readonly bool IsEtherumCapale;
        
        [JsonIgnore]
        readonly public DeviceGroupType DeviceGroupType;
        // UUID now used for saving
        readonly public string UUID;

        [JsonIgnore]
        public static readonly ulong MEMORY_2GB = 2147483648;

        [JsonIgnore]
        CudaDevice _cudaDevice;
        [JsonIgnore]
        AmdGpuDevice _amdDevice;
        // sgminer extra quickfix
        [JsonIgnore]
        public bool IsOptimizedVersion { get; private set; }
        [JsonIgnore]
        public string Codename { get; private set; }

        // temp value for grouping new profits
        [JsonIgnore]
        public Algorithm MostProfitableAlgorithm { get; set; }

        [JsonIgnore]
        public DeviceBenchmarkConfig DeviceBenchmarkConfig { get; private set; }

        // 
        readonly public static List<ComputeDevice> AllAvaliableDevices = new List<ComputeDevice>();
        readonly public static List<ComputeDevice> UniqueAvaliableDevices = new List<ComputeDevice>();

        [JsonConstructor]
        public ComputeDevice(int id, string group, string name, string uuid, bool enabled = true) {
            ID = id;
            Group = group;
            Name = name;
            UUID = uuid;
            Enabled = enabled;
        }

        public ComputeDevice(int id, string group, string name, bool addToGlobalList = false, bool enabled = true)
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

        public ComputeDevice(CudaDevice cudaDevice, string group, bool addToGlobalList = false, bool enabled = true) {
            _cudaDevice = cudaDevice;
            ID = (int)cudaDevice.DeviceID;
            Group = group;
            Name = cudaDevice.DeviceName;
            Enabled = enabled;
            DeviceGroupType = GroupNames.GetType(Group);
            IsEtherumCapale = cudaDevice.IsEtherumCapable();
            if (addToGlobalList) {
                // add to all devices
                AllAvaliableDevices.Add(this);
                // compare new device with unique list scope
                {
                    bool isNewUnique = true;
                    foreach (var d in UniqueAvaliableDevices) {
                        if (this.Name == d.Name) {
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
            UUID = cudaDevice.UUID;
        }

        public ComputeDevice(AmdGpuDevice amdDevice, bool addToGlobalList = false, bool enabled = true) {
            _amdDevice = amdDevice;
            ID = amdDevice.DeviceID;
            DeviceGroupType = DeviceGroupType.AMD_OpenCL;
            Group = GroupNames.GetName(DeviceGroupType.AMD_OpenCL);
            Name = amdDevice.DeviceName;
            Enabled = enabled;
            IsEtherumCapale = amdDevice.IsEtherumCapable();
            if (addToGlobalList) {
                // add to all devices
                AllAvaliableDevices.Add(this);
                // compare new device with unique list scope
                {
                    bool isNewUnique = true;
                    foreach (var d in UniqueAvaliableDevices) {
                        if (this.Name == d.Name) {
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
            UUID = amdDevice.UUID;
            // sgminer extra
            IsOptimizedVersion = amdDevice.UseOptimizedVersion;
            Codename = amdDevice.Codename;
        }

        // TODO add file check and stuff like that
        public void SetDeviceBenchmarkConfig(DeviceBenchmarkConfig deviceBenchmarkConfig) {
            DeviceBenchmarkConfig = deviceBenchmarkConfig;
            // check initialization
            if (!DeviceBenchmarkConfig.IsAlgorithmSettingsInit) {
                DeviceBenchmarkConfig.IsAlgorithmSettingsInit = true;
                // only AMD has extra initialization
                if (_amdDevice != null) {
                    // Check for optimized version
                    if (_amdDevice.UseOptimizedVersion) {
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.X11].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Qubit].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Quark].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Lyra2REv2].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 512  --thread-concurrency 0 --worksize 64 --gpu-threads 1";
                    } else {
                        // this is not the same as the constructor values?? check!
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.X11].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 64 --gpu-threads 2";
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Qubit].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 128 --gpu-threads 4";
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Quark].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 256 --gpu-threads 1";
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Lyra2REv2].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 64 --gpu-threads 2";
                    }
                    if (!_amdDevice.Codename.Contains("Tahiti")) {
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.NeoScrypt].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 2";
                        Helpers.ConsolePrint("ComputeDevice", "The GPU detected (" + _amdDevice.Codename + ") is not Tahiti. Changing default gpu-threads to 2.");
                    }
                }
                if (_cudaDevice != null) {
                    if (DeviceBenchmarkConfig.AlgorithmSettings.ContainsKey(AlgorithmType.CryptoNight)) {
                        var CryptoNightAlgo = DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.CryptoNight];
                        if (CryptoNightAlgo.ExtraLaunchParameters == "") {
                            // comon popular cards settings
                            if (Name.Contains("GTX 980")) {
                                CryptoNightAlgo.ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x16";
                            } else if (Name.Contains("GTX 970")) {
                                CryptoNightAlgo.ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x13";
                            } else if (Name.Contains("GTX 960")) {
                                CryptoNightAlgo.ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x8";
                            } else if (Name.Contains("GTX 950")) {
                                CryptoNightAlgo.ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x6";
                            } else {
                                // TODO 
                                //CryptoNightAlgo.ExtraLaunchParameters = "";
                            }
                        }
                    }
                }
            }
        }


        // static methods
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

        private static string GetUUID(int id, string group, string name, DeviceGroupType deviceGroupType) {
            var SHA256 = new SHA256Managed();
            var hash = new StringBuilder();
            string mixedAttr = id.ToString() + group + name + ((int)deviceGroupType).ToString();
            byte[] hashedBytes = SHA256.ComputeHash(Encoding.UTF8.GetBytes(mixedAttr), 0, Encoding.UTF8.GetByteCount(mixedAttr));
            foreach (var b in hashedBytes) {
                hash.Append(b.ToString("x2"));
            }
            // GEN indicates the UUID has been generated and cannot be presumed to be immutable
            return "GEN-" + hash.ToString();
        }
    }
}
