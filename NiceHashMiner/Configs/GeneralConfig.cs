using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Configs {
    [Serializable]
    public class GeneralConfig : BaseConfigFile<GeneralConfig> {

        #region Members
        public BenchmarkTimeLimitsConfig BenchmarkTimeLimits { get; set; }
        public DeviceDetectionConfig DeviceDetection { get; set; }


        // After Device initialization
        public Dictionary<string, DeviceGroupConfig> GroupSettings { get; set; }
        /// <summary>
        /// LastDevicesSettup field should not be manually edited
        /// The changes can be of two scenarios:
        /// #1 Detect if the device is enabled/disabled,
        /// #2 Detect hardware changes/upgrades such as CPUs and GPUs.
        /// TODO change #2 with UUID methods
        /// </summary>
        public List<ComputeDevice> LastDevicesSettup { get; set; }

        #endregion //Members

        public GeneralConfig() {
            BenchmarkTimeLimits = new BenchmarkTimeLimitsConfig();
            DeviceDetection = new DeviceDetectionConfig();
        }

        protected override void InitializePaths() {
            FilePath = "General.json";
            FilePathOld = "General_old.json";
        }
        protected override void InitializeObject() {
            if (_self.BenchmarkTimeLimits != null) {
                this.BenchmarkTimeLimits = _self.BenchmarkTimeLimits;
            }
            if (_self.DeviceDetection != null) {
                this.DeviceDetection = _self.DeviceDetection;
            }
        }

        public void AfterDeviceQueryInitialization() {
            ComputeDeviceGroupManager.Instance.GroupSettings = GroupSettings;
            GroupSettings = ComputeDeviceGroupManager.Instance.GroupSettings;
            if (LastDevicesSettup != null) {
                for (int i = 0; i < ComputeDevice.AllAvaliableDevices.Count; ++i) {
                    var usedDevice = ComputeDevice.AllAvaliableDevices[i];
                    var configDevice = LastDevicesSettup[i];
                    usedDevice.Enabled = configDevice.Enabled;
                }
            }
            if (_self.GroupSettings != null) {
                foreach (var key in _self.GroupSettings.Keys) {
                    if (this.GroupSettings.ContainsKey(key)) {
                        this.GroupSettings[key] = _self.GroupSettings[key];
                    } else {
                        // TODO think if we let tamnpered data
                    }
                }
            } 

            LastDevicesSettup = ComputeDevice.AllAvaliableDevices;
        }
    }
}
