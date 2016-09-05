using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Devices;
using NiceHashMiner.Configs;
using NiceHashMiner.Interfaces;

namespace NiceHashMiner.Forms.Components {
    public partial class DevicesListViewEnableControl : UserControl {

        private const int ENABLED = 0;
        private const int DEVICE = 1;

        private class DefaultDevicesColorSeter : IListItemCheckColorSetter {
            private static Color ENABLED_COLOR = Color.White;
            private static Color DISABLED_COLOR = Color.DarkGray;
            public void LviSetColor(ListViewItem lvi) {
                ComputeDeviceEnabledOption cdvo = lvi.Tag as ComputeDeviceEnabledOption;
                if (cdvo != null) {
                    if(cdvo.IsEnabled) {
                        lvi.BackColor = ENABLED_COLOR;
                    } else {
                        lvi.BackColor = DISABLED_COLOR;
                    }
                }
            }
        }

        IListItemCheckColorSetter _listItemCheckColorSetter = new DefaultDevicesColorSeter();

        private AlgorithmsListView _algorithmsListView = null;

        [Serializable]
        public class ComputeDeviceEnabledOption {
            public bool IsEnabled { get; set; }
            public ComputeDevice CDevice { get; set; }
            public void SaveOption() {
                CDevice.Enabled = IsEnabled;
            }
        }

        public string FirstColumnText {
            get { return listViewDevices.Columns[ENABLED].Text; }
            set { if (value != null) listViewDevices.Columns[ENABLED].Text = value; }
        }

        public List<ComputeDeviceEnabledOption> Options { get; private set; }

        // to automatically save on enabled click or not
        public bool AutoSaveChange { get; set; }
        public bool SaveToGeneralConfig { get; set; }
        public bool SetAllEnabled { get; set; }

        public DevicesListViewEnableControl() {
            InitializeComponent();

            AutoSaveChange = false;
            SaveToGeneralConfig = false;
            SetAllEnabled = false;
            Options = new List<ComputeDeviceEnabledOption>();
            // intialize ListView callbacks
            listViewDevices.ItemChecked += new ItemCheckedEventHandler(listViewDevicesItemChecked);
        }

        public void SetIListItemCheckColorSetter(IListItemCheckColorSetter listItemCheckColorSetter) {
            _listItemCheckColorSetter = listItemCheckColorSetter;
        }

        public void SetAlgorithmsListView(AlgorithmsListView algorithmsListView) {
            _algorithmsListView = algorithmsListView;
        }

        public void ResetListItemColors() {
            foreach (ListViewItem lvi in listViewDevices.Items) {
                if (_listItemCheckColorSetter != null) {
                    _listItemCheckColorSetter.LviSetColor(lvi);
                }
            }
        }

        public void SetComputeDevices(List<ComputeDevice> computeDevices) {
            // to not run callbacks when setting new
            bool tmp_AutoSaveChange = AutoSaveChange;
            bool tmp_SaveToGeneralConfig = SaveToGeneralConfig;
            AutoSaveChange = false;
            SaveToGeneralConfig = false;
            // set devices
            foreach (var computeDevice in computeDevices) {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(computeDevice.Name);
                lvi.Checked = computeDevice.Enabled || SetAllEnabled;
                ComputeDeviceEnabledOption newTag = new ComputeDeviceEnabledOption() {
                    IsEnabled = computeDevice.Enabled || SetAllEnabled,
                    CDevice = computeDevice
                };
                computeDevice.ComputeDeviceEnabledOption = newTag;
                Options.Add(newTag);
                lvi.Tag = newTag;
                listViewDevices.Items.Add(lvi);
                _listItemCheckColorSetter.LviSetColor(lvi);
            }
            // reset properties
            AutoSaveChange = tmp_AutoSaveChange;
            SaveToGeneralConfig = tmp_SaveToGeneralConfig;
        }

        public void ResetComputeDevices(List<ComputeDevice> computeDevices) {
            Options.Clear();
            listViewDevices.Items.Clear();
            SetComputeDevices(computeDevices);
        }

        public void InitLocale() {
            listViewDevices.Columns[ENABLED].Text = International.GetText("ListView_Enabled");
            listViewDevices.Columns[DEVICE].Text = International.GetText("ListView_Device");
        }

        private void listViewDevicesItemChecked(object sender, ItemCheckedEventArgs e) {
            ComputeDeviceEnabledOption G = e.Item.Tag as ComputeDeviceEnabledOption;
            G.IsEnabled = e.Item.Checked;
            if (AutoSaveChange) {
                G.SaveOption();
            }
            if (SaveToGeneralConfig) {
                ConfigManager.Instance.GeneralConfig.Commit();
            }
            var lvi = e.Item as ListViewItem;
            if (lvi != null) _listItemCheckColorSetter.LviSetColor(lvi);
            if (_algorithmsListView != null) _algorithmsListView.RepaintStatus(G.IsEnabled, G.CDevice.UUID);
        }

        public void SaveOptions() {
            foreach (var option in Options) {
                option.SaveOption();
            }
        }

        public void SetDeviceSelectionChangedCallback(ListViewItemSelectionChangedEventHandler callback) {
            listViewDevices.ItemSelectionChanged += callback;
        }

    }
}
