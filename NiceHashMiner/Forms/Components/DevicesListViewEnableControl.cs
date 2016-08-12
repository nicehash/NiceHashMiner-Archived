using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Forms.Components {
    public partial class DevicesListViewEnableControl : UserControl {

        private const int ENABLED = 0;
        private const int GROUP = 1;
        private const int DEVICE = 2;

        private class ComputeDeviceEnabledOption {
            public bool IsEnabled { get; set; }
            public ComputeDevice CDevice { get; set; }
            public void SaveOption() {
                CDevice.Enabled = IsEnabled;
            }
        }

        private List<ComputeDeviceEnabledOption> _options;

        // to automatically save on enabled click or not
        public bool AutoSaveChange { get; set; }

        public DevicesListViewEnableControl() {
            InitializeComponent();

            AutoSaveChange = true;
            _options = new List<ComputeDeviceEnabledOption>();
            // intialize ListView callbacks
            listViewDevices.ItemChecked += new ItemCheckedEventHandler(listViewDevicesItemChecked);
        }


        public void SetComputeDevices(List<ComputeDevice> computeDevices) {
            foreach (var computeDevice in computeDevices) {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(computeDevice.Group);
                lvi.SubItems.Add(computeDevice.Name);
                lvi.Checked = computeDevice.Enabled;
                ComputeDeviceEnabledOption newTag = new ComputeDeviceEnabledOption() {
                    IsEnabled = computeDevice.Enabled,
                    CDevice = computeDevice
                };
                _options.Add(newTag);
                lvi.Tag = newTag;
                listViewDevices.Items.Add(lvi);
            }
        }

        public void ResetComputeDevices(List<ComputeDevice> computeDevices) {
            _options.Clear();
            listViewDevices.Items.Clear();
            SetComputeDevices(computeDevices);
        }

        public void InitLocale() {
            listViewDevices.Columns[ENABLED].Text = International.GetText("ListView_Enabled");
            listViewDevices.Columns[GROUP].Text = International.GetText("ListView_Group");
            listViewDevices.Columns[DEVICE].Text = International.GetText("ListView_Device");
        }

        private void listViewDevicesItemChecked(object sender, ItemCheckedEventArgs e) {
            ComputeDeviceEnabledOption G = e.Item.Tag as ComputeDeviceEnabledOption;
            G.IsEnabled = e.Item.Checked;
            if (AutoSaveChange) {
                G.SaveOption();
            }
        }

        public void SaveOptions() {
            foreach (var option in _options) {
                option.SaveOption();
            }
        }

        public void SetDeviceSelectionChangedCallback(ListViewItemSelectionChangedEventHandler callback) {
            listViewDevices.ItemSelectionChanged += callback;
        }

    }
}
