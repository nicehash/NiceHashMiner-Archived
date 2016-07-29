using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Forms.Components {
    public partial class DevicesListView : UserControl {
        public DevicesListView() {
            InitializeComponent();

            // intialize ListView callbacks
            listViewDevices.ItemChecked += new ItemCheckedEventHandler(listViewDevicesItemChecked);
        }


        public void SetComputeDevices(List<ComputeDevice> computeDevices) {
            foreach (var computeDevice in computeDevices) {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(computeDevice.Group);
                lvi.SubItems.Add(computeDevice.Name);
                lvi.Checked = computeDevice.Enabled;
                lvi.Tag = computeDevice;
                listViewDevices.Items.Add(lvi);
            }
        }

        private void listViewDevicesItemChecked(object sender, ItemCheckedEventArgs e) {
            ComputeDevice G = e.Item.Tag as ComputeDevice;
            G.Enabled = e.Item.Checked;
        }

        public void SetDeviceSelectionChangedCallback(ListViewItemSelectionChangedEventHandler callback) {
            listViewDevices.ItemSelectionChanged += callback;
        }

    }
}
