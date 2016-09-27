using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NiceHashMiner.Enums;
using NiceHashMiner.Devices;
using Newtonsoft.Json;
using System.IO;

namespace NiceHashMiner.Configs
{
    [Serializable]
    public class DeviceBenchmarkConfig : BaseConfigFile<DeviceBenchmarkConfig> {
        
        public string DeviceUUID { get; private set; }
        [JsonIgnore]
        public DeviceGroupType DeviceGroupType { get; private set; }

        public string DeviceName { get; private set; }
        //public int TimeLimit { get; set; }
        public Dictionary<AlgorithmType, Algorithm> AlgorithmSettings { get; set; }

        // TODO remove in furure releases

        [field: NonSerialized]
        readonly public static string BENCHMARK_PREFIX = "benchmark_";

        [JsonIgnore]
        public bool IsAlgorithmSettingsInit { get; set; }

        public DeviceBenchmarkConfig(DeviceGroupType deviceGroupType,
            string deviceUUID,
            string deviceName,
            Dictionary<AlgorithmType, Algorithm> benchmarkSpeeds = null) {

            DeviceGroupType = deviceGroupType;
            DeviceUUID = deviceUUID;
            DeviceName = deviceName;
            if (benchmarkSpeeds != null) {
                AlgorithmSettings = benchmarkSpeeds;
            } else {
                AlgorithmSettings = GroupAlgorithms.CreateDefaultsForGroup(deviceGroupType);
            }

            IsAlgorithmSettingsInit = false;
        }

        protected override void InitializePaths() {
            // make device name
            char[] invalid = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            string fileName = BENCHMARK_PREFIX + DeviceUUID.Replace(' ', '_');
            foreach (var c in invalid) {
                fileName = fileName.Replace(c.ToString(), String.Empty);
            }
            const string extension = ".json";
            FilePath = fileName + extension;
            FilePathOld = fileName + "_OLD" + extension;
        }

        // TODO make generic initializations
        protected override void InitializeObject() {
            // if new backup benchmarks
            if (ConfigManager.Instance.GeneralConfig.IsNewVersion) {
                Helpers.ConsolePrint("DeviceBenchmarkConfig", String.Format("Backing up {0} to {1}..", FilePath, FilePathOld));
                try {
                    if (File.Exists(FilePathOld))
                        File.Delete(FilePathOld);
                    File.Move(FilePath, FilePathOld);
                } catch { }
            }


            // check if data tampered
            bool IsDataTampered = !(
                /*this.ID == _file.ID
                && this.DeviceGroupType == _file.DeviceGroupType
                && */this.DeviceName == _file.DeviceName
                );

            //this.TimeLimit = _file.TimeLimit;
            if (_file.DeviceUUID != null) {
                this.DeviceUUID = _file.DeviceUUID;
            }

            if (_file.AlgorithmSettings != null) {
                // settings from files are initialized
                foreach (var key in _file.AlgorithmSettings.Keys) {
                    if(this.AlgorithmSettings.ContainsKey(key)) {
                        this.AlgorithmSettings[key] = _file.AlgorithmSettings[key];
                    } else {
                        // TODO think if we let tamnpered data
                    }
                }
            }
            // if read fromfile then it is initialized
            IsAlgorithmSettingsInit = true;
        }
    }
}
