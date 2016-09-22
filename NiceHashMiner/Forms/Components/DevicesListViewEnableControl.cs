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

        public IBenchmarkCalculation BenchmarkCalculation { get; set; }

        private AlgorithmsListView _algorithmsListView = null;

        // disable checkboxes when in benchmark mode
        private bool _isInBenchmark = false;
        // helper for benchmarking logic
        public bool IsInBenchmark {
            get { return _isInBenchmark; }
            set {
                if (value) {
                    _isInBenchmark = value;
                    listViewDevices.CheckBoxes = false;
                } else {
                    _isInBenchmark = value;
                    listViewDevices.CheckBoxes = true;
                }
            }
        }
        private bool _isMining = false;
        public bool IsMining {
            get { return _isMining; }
            set {
                if (value) {
                    _isMining = value;
                    listViewDevices.CheckBoxes = false;
                } else {
                    _isMining = value;
                    listViewDevices.CheckBoxes = true;
                }
            }
        }

        public bool IsBenchmarkForm = false;
        public bool IsSettingsCopyEnabled = false;

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
            //listViewDevices.CheckBoxes = false;
            IsMining = false;
            BenchmarkCalculation = null;
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
            listViewDevices.BeginUpdate();
            listViewDevices.Items.Clear();
            // set devices
            foreach (var computeDevice in computeDevices) {
                ListViewItem lvi = new ListViewItem();
                //lvi.SubItems.Add(computeDevice.Name);
                lvi.Checked = computeDevice.Enabled || SetAllEnabled;
                lvi.Text = computeDevice.GetFullName();
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
            listViewDevices.EndUpdate();
            listViewDevices.Invalidate(true);
            // reset properties
            AutoSaveChange = tmp_AutoSaveChange;
            SaveToGeneralConfig = tmp_SaveToGeneralConfig;
        }

        public void ResetComputeDevices(List<ComputeDevice> computeDevices) {
            Options.Clear();
            SetComputeDevices(computeDevices);
        }

        public void InitLocale() {
            listViewDevices.Columns[ENABLED].Text = International.GetText("ListView_Device");  //International.GetText("ListView_Enabled");
            //listViewDevices.Columns[DEVICE].Text = International.GetText("ListView_Device");
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
            if (BenchmarkCalculation != null) BenchmarkCalculation.CalcBenchmarkDevicesAlgorithmQueue();
        }

        public void SaveOptions() {
            foreach (var option in Options) {
                option.SaveOption();
            }
        }

        public void SetDeviceSelectionChangedCallback(ListViewItemSelectionChangedEventHandler callback) {
            listViewDevices.ItemSelectionChanged += callback;
        }

        private void listViewDevices_MouseClick(object sender, MouseEventArgs e) {
            if (IsInBenchmark) return;
            if (IsMining) return; 
            if (e.Button == MouseButtons.Right) {
                if (listViewDevices.FocusedItem.Bounds.Contains(e.Location) == true) {
                    contextMenuStrip1.Items.Clear();
                    ComputeDeviceEnabledOption G = listViewDevices.FocusedItem.Tag as ComputeDeviceEnabledOption;
                    //if (G.IsEnabled) {
                    //    var disableItem = new ToolStripMenuItem();
                    //    disableItem.Text = International.GetText("DeviceListView_ContextMenu_DisableDevice");
                    //    //disableItem.Checked = true;
                    //    disableItem.Click += toolStripMenuItemEnable_Click;
                    //    contextMenuStrip1.Items.Add(disableItem);
                    //} else {
                    //    var disableItem = new ToolStripMenuItem();
                    //    disableItem.Text = International.GetText("DeviceListView_ContextMenu_EnableDevice");
                    //    //disableItem.Checked = false;
                    //    disableItem.Click += toolStripMenuItemEnable_Click;
                    //    contextMenuStrip1.Items.Add(disableItem);
                    //}
                    if (IsSettingsCopyEnabled) {
                        var sameDevTypes = ComputeDevice.GetSameDevicesTypeAsDeviceWithUUID(G.CDevice.UUID);
                        if (sameDevTypes.Count > 0) {
                            var copyBenchItem = new ToolStripMenuItem();
                            //copyBenchItem.DropDownItems
                            foreach (var cDev in sameDevTypes) {
                                if (cDev.ComputeDeviceEnabledOption.IsEnabled) {
                                    var copyBenchDropDownItem = new ToolStripMenuItem();
                                    copyBenchDropDownItem.Text = cDev.Name;
                                    copyBenchDropDownItem.Checked = cDev.UUID == G.CDevice.BenchmarkCopyUUID;
                                    copyBenchDropDownItem.Click += toolStripMenuItemCopySettings_Click;
                                    copyBenchDropDownItem.Tag = cDev.UUID;
                                    copyBenchItem.DropDownItems.Add(copyBenchDropDownItem);
                                }
                            }
                            copyBenchItem.Text = International.GetText("DeviceListView_ContextMenu_CopySettings");
                            contextMenuStrip1.Items.Add(copyBenchItem);
                        }
                    }
                    contextMenuStrip1.Show(Cursor.Position);
                }
            } 
        }

        //private void toolStripMenuItemEnable_Click(object sender, EventArgs e) {
        //    ComputeDeviceEnabledOption G = listViewDevices.FocusedItem.Tag as ComputeDeviceEnabledOption;
        //    var isEnabled = !G.IsEnabled;
        //    G.IsEnabled = isEnabled;
        //    if (AutoSaveChange) {
        //        G.SaveOption();
        //    }
        //    if (SaveToGeneralConfig) {
        //        ConfigManager.Instance.GeneralConfig.Commit();
        //    }
        //    var lvi = listViewDevices.FocusedItem as ListViewItem;
        //    if (lvi != null) _listItemCheckColorSetter.LviSetColor(lvi);
        //    if (_algorithmsListView != null) _algorithmsListView.RepaintStatus(G.IsEnabled, G.CDevice.UUID);
        //    if (BenchmarkCalculation != null) _benchmarkCalculation.CalcBenchmarkDevicesAlgorithmQueue();
        //}

        private void toolStripMenuItemCopySettings_Click(object sender, EventArgs e) {
            ComputeDeviceEnabledOption G = listViewDevices.FocusedItem.Tag as ComputeDeviceEnabledOption;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if(item != null) {
                var uuid = item.Tag as string;
                if (uuid != null) {
                    var copyBenchCDev = ComputeDevice.GetDeviceWithUUID(uuid);
                    G.CDevice.BenchmarkCopyUUID = uuid;

                    var result = MessageBox.Show(
                        String.Format(
                        International.GetText("DeviceListView_ContextMenu_CopySettings_Confirm_Dialog_Msg"), copyBenchCDev.GetFullName(), G.CDevice.GetFullName()),
                                International.GetText("DeviceListView_ContextMenu_CopySettings_Confirm_Dialog_Title"),
                                MessageBoxButtons.YesNo);
                    if(result == DialogResult.Yes) {
                        // just copy
                        G.CDevice.CopyBenchmarkSettingsFrom(copyBenchCDev);
                        if (_algorithmsListView != null) _algorithmsListView.RepaintStatus(G.IsEnabled, G.CDevice.UUID);
                    }
                }
            }
        }

        private void DevicesListViewEnableControl_Resize(object sender, EventArgs e) {
            // only one 
            foreach (ColumnHeader ch in listViewDevices.Columns) {
                ch.Width = this.Width - 10;
            }
        }

        public void SetFirstSelected() {
            if (listViewDevices.Items.Count > 0) {
                this.listViewDevices.Items[0].Selected = true;
                this.listViewDevices.Select();
            }
        }

    }
}
