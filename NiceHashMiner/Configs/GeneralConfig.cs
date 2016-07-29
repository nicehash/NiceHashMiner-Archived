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
        public Dictionary<string, DeviceGroupSettings> GroupSettings { get; set; }
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
        protected override void InitializeObject() { }

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
            LastDevicesSettup = ComputeDevice.AllAvaliableDevices;
        }
    }
}
