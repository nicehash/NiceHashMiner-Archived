using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using NiceHashMiner.Enums;
using System.Security.Cryptography;
using NiceHashMiner.Configs;
using NiceHashMiner.Configs.Data;

namespace NiceHashMiner.Devices
{
    public class ComputeDevice
    {
        readonly public int ID;
        // to identify equality;
        readonly public string Name; // { get; set; }
        // name count is the short name for displaying in moning groups
        readonly public string NameCount;
        public bool Enabled;

        readonly public DeviceGroupType DeviceGroupType;
        // CPU, NVIDIA, AMD
        readonly public DeviceType DeviceType;
        // UUID now used for saving
        readonly public string UUID;


        // CPU extras
        readonly public int Threads;
        readonly public ulong AffinityMask;

        // GPU extras
        public readonly bool IsEtherumCapale;
        public static readonly ulong MEMORY_3GB = 3221225472;

        //CudaDevice _cudaDevice = null;
        //AmdGpuDevice _amdDevice = null;
        // sgminer extra quickfix
        public readonly bool IsOptimizedVersion;
        public readonly string Codename;
        public readonly string InfSection;

        //public DeviceBenchmarkConfig_rem DeviceBenchmarkConfig { get; private set; }
        public Dictionary<AlgorithmType, Algorithm> AlgorithmSettings { get; set; }

        public string BenchmarkCopyUUID { get; set; }

        // Fake dev
        public ComputeDevice(int id) {
            ID = id;
            Name = "fake_" + id;
            NameCount = Name;
            Enabled = true;
            DeviceType = DeviceType.CPU;
            DeviceGroupType = DeviceGroupType.NONE;
            IsEtherumCapale = false;
            IsOptimizedVersion = false;
            Codename = "fake";
            UUID = GetUUID(ID, GroupNames.GetGroupName(DeviceGroupType, ID), Name, DeviceGroupType);
        }

        // CPU 
        public ComputeDevice(int id, string group, string name, int threads, ulong affinityMask, int CPUCount)
        {
            ID = id;
            Name = name;
            Threads = threads;
            AffinityMask = affinityMask;
            Enabled = true;
            DeviceGroupType = DeviceGroupType.CPU;
            DeviceType = DeviceType.CPU;
            NameCount = String.Format(International.GetText("ComputeDevice_Short_Name_CPU"), CPUCount);
            UUID = GetUUID(ID, GroupNames.GetGroupName(DeviceGroupType, ID), Name, DeviceGroupType);
            AlgorithmSettings = GroupAlgorithms.CreateForDevice(this);
            IsEtherumCapale = false;
        }

        // GPU NVIDIA
        public ComputeDevice(CudaDevice cudaDevice, DeviceGroupType group, int GPUCount) {
            ID = (int)cudaDevice.DeviceID;
            Name = cudaDevice.GetName();
            Enabled = true;
            DeviceGroupType = group;
            IsEtherumCapale = cudaDevice.IsEtherumCapable();
            DeviceType = DeviceType.NVIDIA;
            NameCount = String.Format(International.GetText("ComputeDevice_Short_Name_NVIDIA_GPU"), GPUCount);
            UUID = cudaDevice.UUID;
            AlgorithmSettings = GroupAlgorithms.CreateForDevice(this);
        }

        // GPU AMD
        public ComputeDevice(AmdGpuDevice amdDevice, int GPUCount, bool isDetectionFallback) {
            ID = amdDevice.DeviceID;
            DeviceGroupType = DeviceGroupType.AMD_OpenCL;
            Name = amdDevice.DeviceName;
            Enabled = true;
            IsEtherumCapale = amdDevice.IsEtherumCapable();
            DeviceType = DeviceType.AMD;
            NameCount = String.Format(International.GetText("ComputeDevice_Short_Name_AMD_GPU"), GPUCount);
            if (isDetectionFallback) {
                UUID = GetUUID(ID, GroupNames.GetGroupName(DeviceGroupType, ID), Name, DeviceGroupType);
            } else {
                UUID = amdDevice.UUID;
            }
            // sgminer extra
            IsOptimizedVersion = amdDevice.UseOptimizedVersion;
            Codename = amdDevice.Codename;
            InfSection = amdDevice.InfSection;
            AlgorithmSettings = GroupAlgorithms.CreateForDevice(this);
        }

        // combines long and short name
        public string GetFullName() {
            return String.Format(International.GetText("ComputeDevice_Full_Device_Name"), NameCount, Name);
        }

        public void CopyBenchmarkSettingsFrom(ComputeDevice copyBenchCDev) {
            foreach (var copyAlgSpeeds in copyBenchCDev.AlgorithmSettings) {
                if (this.AlgorithmSettings.ContainsKey(copyAlgSpeeds.Key)) {
                    var setAlgo = this.AlgorithmSettings[copyAlgSpeeds.Key];
                    setAlgo.BenchmarkSpeed = copyAlgSpeeds.Value.BenchmarkSpeed;
                    setAlgo.ExtraLaunchParameters = copyAlgSpeeds.Value.ExtraLaunchParameters;
                    setAlgo.LessThreads = copyAlgSpeeds.Value.LessThreads;
                }
            }
        }

        public void _3rdPartyMinerChange() {
            var TmpAlgorithmSettings = GroupAlgorithms.CreateForDevice(this);
            // check to remove
            {
                List<AlgorithmType> toRemoveKeys = new List<AlgorithmType>();
                foreach (var containsKey in AlgorithmSettings.Keys) {
                    if (TmpAlgorithmSettings.ContainsKey(containsKey) == false) {
                        toRemoveKeys.Add(containsKey);
                    }
                }
                foreach (var removeKey in toRemoveKeys) {
                    AlgorithmSettings.Remove(removeKey);
                }
            }
            // check to add
            {
                List<AlgorithmType> toAddKeys = new List<AlgorithmType>();
                foreach (var containsKey in TmpAlgorithmSettings.Keys) {
                    if (AlgorithmSettings.ContainsKey(containsKey) == false) {
                        toAddKeys.Add(containsKey);
                    }
                }
                foreach (var addKey in toAddKeys) {
                    AlgorithmSettings.Add(addKey, TmpAlgorithmSettings[addKey]);
                }
            }
        }

        #region Config Setters/Getters
        // settings
        // setters
        public void SetFromComputeDeviceConfig(ComputeDeviceConfig config) {
            if (config != null && config.UUID == UUID) {
                this.Enabled = config.Enabled;
            }
        }
        public void SetAlgorithmDeviceConfig(DeviceBenchmarkConfig config) {
            if (config != null && config.DeviceUUID == UUID && config.AlgorithmSettings != null) {
                this.AlgorithmSettings = GroupAlgorithms.CreateForDevice(this);
                foreach (var algoSetting in config.AlgorithmSettings) {
                    AlgorithmType key = algoSetting.Key;
                    AlgorithmConfig conf = algoSetting.Value;
                    if (this.AlgorithmSettings.ContainsKey(key)) {
                        this.AlgorithmSettings[key].BenchmarkSpeed = conf.BenchmarkSpeed;
                        this.AlgorithmSettings[key].ExtraLaunchParameters = conf.ExtraLaunchParameters;
                        this.AlgorithmSettings[key].Skip = conf.Skip;
                        this.AlgorithmSettings[key].LessThreads = conf.LessThreads;
                    }
                }
            }
        }
        // getters
        public ComputeDeviceConfig GetComputeDeviceConfig() {
            ComputeDeviceConfig ret = new ComputeDeviceConfig();
            ret.Enabled = this.Enabled;
            ret.Name = this.Name;
            ret.UUID = this.UUID;
            return ret;
        }
        public DeviceBenchmarkConfig GetAlgorithmDeviceConfig() {
            DeviceBenchmarkConfig ret = new DeviceBenchmarkConfig();
            ret.DeviceName = this.Name;
            ret.DeviceUUID = this.UUID;
            // init algo settings
            foreach (var algo in this.AlgorithmSettings.Values) {
                AlgorithmType key = algo.NiceHashID;
                // create/setup
                AlgorithmConfig conf = new AlgorithmConfig();
                conf.NiceHashID = key;
                conf.MinerName = algo.MinerName; // TODO probably not needed
                conf.BenchmarkSpeed = algo.BenchmarkSpeed;
                conf.ExtraLaunchParameters = algo.ExtraLaunchParameters;
                conf.Skip = algo.Skip;
                conf.LessThreads = algo.LessThreads;
                // insert
                ret.AlgorithmSettings[key] = conf;
            }
            return ret;
        }
        #endregion Config Setters/Getters
        
        // static methods
        
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
