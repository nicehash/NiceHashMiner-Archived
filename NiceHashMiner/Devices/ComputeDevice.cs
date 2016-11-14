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
        [JsonIgnore]
        readonly public int ID;
        [JsonIgnore]
        readonly public string Group;
        public string Name { get; set; }
        // to identify equality;
        [JsonIgnore]
        readonly public string _nameNoNums;
        // name count is the short name for displaying in moning groups
        [JsonIgnore]
        readonly public string NameCount;
        public bool Enabled;

        [JsonIgnore]
        public readonly bool IsEtherumCapale;

        [JsonIgnore]
        public string DeviceGroupString { get; private set; }
        [JsonIgnore]
        readonly public DeviceGroupType DeviceGroupType;
        // UUID now used for saving
        readonly public string UUID;

        // Current Extra Launch Parameters copied from most profitable algorithm for benchmarking
        [JsonIgnore]
        public string CurrentExtraLaunchParameters { get; set; }

        // CPU, NVIDIA, AMD
        [JsonIgnore]
        readonly public int Threads;

        [JsonIgnore]
        readonly public ulong AffinityMask;

        // CPU, NVIDIA, AMD
        [JsonIgnore]
        public DeviceType DeviceType { get; private set; }

        [JsonIgnore]
        public string BenchmarkCopyUUID { get; set; }

        [JsonIgnore]
        public static readonly ulong MEMORY_2GB = 2147483648;

        [JsonIgnore]
        CudaDevice _cudaDevice = null;
        [JsonIgnore]
        AmdGpuDevice _amdDevice = null;
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

        [JsonIgnore]
        [field: NonSerialized]
        public NiceHashMiner.Forms.Components.DevicesListViewEnableControl.ComputeDeviceEnabledOption ComputeDeviceEnabledOption { get; set; }

        [JsonConstructor]
        public ComputeDevice(int id, string group, string name, string uuid, bool enabled = true) {
            ID = id;
            Group = group;
            Name = name;
            _nameNoNums = name;
            UUID = uuid;
            Enabled = enabled;
        }

        // CPU 
        public ComputeDevice(int id, string group, string name, int threads, ulong affinityMask, int CPUCount)
        {
            ID = id;
            Group = group;
            Name = name;
            Threads = threads;
            AffinityMask = affinityMask;
            _nameNoNums = name;
            Enabled = true;
            DeviceGroupType = GroupNames.GetType(Group);
            DeviceGroupString = GroupNames.GetNameGeneral(DeviceGroupType);
            DeviceType = DeviceType.CPU;
            NameCount = String.Format(International.GetText("ComputeDevice_Short_Name_CPU"), CPUCount);
            UUID = GetUUID(ID, Group, Name, DeviceGroupType);
        }

        // GPU NVIDIA
        public ComputeDevice(CudaDevice cudaDevice, string group, int GPUCount) {
            _cudaDevice = cudaDevice;
            ID = (int)cudaDevice.DeviceID;
            Group = group;
            Name = cudaDevice.GetName();
            _nameNoNums = cudaDevice.GetName();
            Enabled = true;
            DeviceGroupType = GroupNames.GetType(Group);
            DeviceGroupString = GroupNames.GetNameGeneral(DeviceGroupType);
            IsEtherumCapale = cudaDevice.IsEtherumCapable();
            DeviceType = DeviceType.NVIDIA;
            NameCount = String.Format(International.GetText("ComputeDevice_Short_Name_NVIDIA_GPU"), GPUCount);
            UUID = cudaDevice.UUID;
        }

        // GPU AMD
        public ComputeDevice(AmdGpuDevice amdDevice, int GPUCount) {
            _amdDevice = amdDevice;
            ID = amdDevice.DeviceID;
            DeviceGroupType = DeviceGroupType.AMD_OpenCL;
            Group = GroupNames.GetName(DeviceGroupType.AMD_OpenCL);
            DeviceGroupString = GroupNames.GetNameGeneral(DeviceGroupType);
            Name = amdDevice.DeviceName;
            _nameNoNums = amdDevice.DeviceName;
            Enabled = true;
            IsEtherumCapale = amdDevice.IsEtherumCapable();
            DeviceType = DeviceType.AMD;
            NameCount = String.Format(International.GetText("ComputeDevice_Short_Name_AMD_GPU"), GPUCount);
            UUID = amdDevice.UUID;
            // sgminer extra
            IsOptimizedVersion = amdDevice.UseOptimizedVersion;
            Codename = amdDevice.Codename;
        }

        // combines long and short name
        public string GetFullName() {
            return String.Format(International.GetText("ComputeDevice_Full_Device_Name"), NameCount, Name);
        }

        // TODO add file check and stuff like that
        public void SetDeviceBenchmarkConfig(DeviceBenchmarkConfig deviceBenchmarkConfig, bool forceSet = false) {

            DeviceBenchmarkConfig = deviceBenchmarkConfig;
            // check initialization
            if (!DeviceBenchmarkConfig.IsAlgorithmSettingsInit || forceSet) {
                DeviceBenchmarkConfig.IsAlgorithmSettingsInit = true;
                // only AMD has extra initialization
                if (_amdDevice != null) {
                    // Check for optimized version
                    if (_amdDevice.UseOptimizedVersion) {
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Qubit].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Quark].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency 0 --worksize 64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Lyra2REv2].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 512  --thread-concurrency 0 --worksize 64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                    } else {
                        // this is not the same as the constructor values?? check!
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Qubit].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 128 --gpu-threads 4" + AmdGpuDevice.TemperatureParam;
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Quark].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 256 --gpu-threads 1" + AmdGpuDevice.TemperatureParam;
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.Lyra2REv2].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity 64 --thread-concurrency 0 --worksize 64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam;
                    }
                    if (!_amdDevice.Codename.Contains("Tahiti")) {
                        DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.NeoScrypt].ExtraLaunchParameters = AmdGpuDevice.DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam;
                        Helpers.ConsolePrint("ComputeDevice", "The GPU detected (" + _amdDevice.Codename + ") is not Tahiti. Changing default gpu-threads to 2.");
                    }
                }
                // CUDA extra initializations
                if (_cudaDevice != null) {
                    if (DeviceBenchmarkConfig.AlgorithmSettings.ContainsKey(AlgorithmType.CryptoNight)) {
                        var CryptoNightAlgo = DeviceBenchmarkConfig.AlgorithmSettings[AlgorithmType.CryptoNight];
                        if (_cudaDevice.SM_major >= 5 && Name.Contains("Ti") == false) {
                            CryptoNightAlgo.ExtraLaunchParameters = "--bsleep=0 --bfactor=0 --launch=32x" + _cudaDevice.SMX.ToString();
                        }
                    }
                }
            }
        }

        public void CopyBenchmarkSettingsFrom(ComputeDevice copyBenchCDev) {
            foreach (var copyAlgSpeeds in copyBenchCDev.DeviceBenchmarkConfig.AlgorithmSettings) {
                if (this.DeviceBenchmarkConfig.AlgorithmSettings.ContainsKey(copyAlgSpeeds.Key)) {
                    var setAlgo = this.DeviceBenchmarkConfig.AlgorithmSettings[copyAlgSpeeds.Key];
                    setAlgo.BenchmarkSpeed = copyAlgSpeeds.Value.BenchmarkSpeed;
                    setAlgo.ExtraLaunchParameters = copyAlgSpeeds.Value.ExtraLaunchParameters;
                    setAlgo.LessThreads = copyAlgSpeeds.Value.LessThreads;
                }
            }
        }

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
