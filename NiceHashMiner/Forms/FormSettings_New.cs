using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms {
    public partial class FormSettings_New : Form {

        bool ShowUniqueDeviceList = true;

        // deep copy initial state if we want to discard changes
        private GeneralConfig _generalConfigBackup;
        private Dictionary<string, DeviceBenchmarkConfig> _benchmarkConfigsBackup;


        public FormSettings_New() {
            InitializeComponent();

            _benchmarkConfigsBackup = MemoryHelper.DeepClone(ConfigManager.Instance.BenchmarkConfigs);
            _generalConfigBackup = MemoryHelper.DeepClone(ConfigManager.Instance.GeneralConfig);

            // initialize device lists, unique or every single one
            if (ShowUniqueDeviceList) {
                devicesListView1.SetComputeDevices(ComputeDevice.UniqueAvaliableDevices);
            } else {
                devicesListView1.SetComputeDevices(ComputeDevice.AllAvaliableDevices);
            }

            // initialization calls 
            InitializeCallbacks();
            InitializeBenchmarkLimitSettings();
            // link algorithm list with algorithm settings control
            algorithmSettingsControl1.Enabled = false;
            algorithmsListView1.ComunicationInterface = algorithmSettingsControl1;

        }

        private ComputeDevice GetCurrentlySelectedComputeDevice(int index) {
            // TODO index checking
            if (ShowUniqueDeviceList) {
                return ComputeDevice.UniqueAvaliableDevices[index];
            } else {
                return ComputeDevice.AllAvaliableDevices[index];
            }
        }

        #region Initializations
        private void InitializeCallbacks() {
            devicesListView1.SetDeviceSelectionChangedCallback(devicesListView1_ItemSelectionChanged);
        }
        private void InitializeBenchmarkLimitSettings() {
            // set referances
            benchmarkLimitControlCPU.TimeLimits = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits.CPU;
            benchmarkLimitControlNVIDIA.TimeLimits = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits.NVIDIA;
            benchmarkLimitControlAMD.TimeLimits = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits.AMD;
        }

        #endregion // Initializations


        #region Form Callbacks

        private void devicesListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            // check if device settings enabled
            if (deviceSettingsControl1.Enabled == false) {
                deviceSettingsControl1.Enabled = true;
            }
            algorithmSettingsControl1.Deselect();
            // show algorithms
            var selectedComputeDevice = GetCurrentlySelectedComputeDevice(e.ItemIndex);
            deviceSettingsControl1.SelectedComputeDevice = selectedComputeDevice;
            algorithmsListView1.SetAlgorithms(
                DeviceBenchmarkConfigManager.Instance.GetConfig(
                selectedComputeDevice.DeviceGroupType,
                selectedComputeDevice.Name,
                new int[] { selectedComputeDevice.ID })
                );
        }


        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e) {
            ConfigManager.Instance.GeneralConfig.Commit();
        }

        #endregion Form Callbacks

    }
}
