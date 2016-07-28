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

        public FormSettings_New() {
            InitializeComponent();

            // initialize device lists, unique or every single one
            if (ShowUniqueDeviceList) {
                devicesListView1.SetComputeDevices(ComputeDevice.UniqueAvaliableDevices);
            } else {
                devicesListView1.SetComputeDevices(ComputeDevice.AllAvaliableDevices);
            }

            InitializeCallbacks();
        }

        private ComputeDevice GetCurrentlySelectedComputeDevice(int index) {
            // TODO index checking
            if (ShowUniqueDeviceList) {
                return ComputeDevice.UniqueAvaliableDevices[index];
            } else {
                return ComputeDevice.AllAvaliableDevices[index];
            }
        }

        #region Form Callbacks

        private void InitializeCallbacks() {
            devicesListView1.SetDeviceSelectionChangedCallback(devicesListView1_ItemSelectionChanged);
        }

        private void devicesListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            // check if device settings enabled
            if (deviceSettingsControl1.Enabled == false) {
                deviceSettingsControl1.Enabled = true;
            }
            // show algorithms
            var selectedComputeDevice = GetCurrentlySelectedComputeDevice(e.ItemIndex);
            deviceSettingsControl1.SelectedComputeDevice = selectedComputeDevice;
            algorithmsListView1.SetAlgorithms(
                BenchmarkConfigManager.Instance.GetConfig(
                selectedComputeDevice.DeviceGroupType,
                selectedComputeDevice.Name,
                new int[] { selectedComputeDevice.ID })
                );
        }


        #endregion Form Callbacks

    }
}
