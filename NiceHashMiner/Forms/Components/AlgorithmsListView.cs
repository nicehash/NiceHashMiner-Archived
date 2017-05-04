using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Enums;
using NiceHashMiner.Configs;
using NiceHashMiner.Forms.Components;
using NiceHashMiner.Devices;
using NiceHashMiner.Miners;
using NiceHashMiner.Interfaces;

namespace NiceHashMiner.Forms.Components {
    public partial class AlgorithmsListView : UserControl {

        private const int ENABLED   = 0;
        private const int ALGORITHM = 1;
        private const int SPEED     = 2;
        private const int RATIO     = 3;
        private const int RATE      = 4;

        public interface IAlgorithmsListView {
            void SetCurrentlySelected(ListViewItem lvi, ComputeDevice computeDevice);
            void HandleCheck(ListViewItem lvi);
            void ChangeSpeed(ListViewItem lvi);
        }

        public IAlgorithmsListView ComunicationInterface { get; set; }

        public IBenchmarkCalculation BenchmarkCalculation { get; set; }

        ComputeDevice _computeDevice;

        private class DefaultAlgorithmColorSeter : IListItemCheckColorSetter {
            private static Color DISABLED_COLOR = Color.DarkGray;
            private static Color BENCHMARKED_COLOR = Color.LightGreen;
            private static Color UNBENCHMARKED_COLOR = Color.LightBlue;
            public void LviSetColor(ListViewItem lvi) {
                Algorithm algorithm = lvi.Tag as Algorithm;
                if (algorithm != null) {
                    if (algorithm.Enabled == false && !algorithm.IsBenchmarkPending) {
                        lvi.BackColor = DISABLED_COLOR;
                    } else if (algorithm.BenchmarkSpeed > 0 && !algorithm.IsBenchmarkPending) {
                        lvi.BackColor = BENCHMARKED_COLOR;
                    } else {
                        lvi.BackColor = UNBENCHMARKED_COLOR;
                    }
                }
            }
        }

        IListItemCheckColorSetter _listItemCheckColorSetter = new DefaultAlgorithmColorSeter();

        // disable checkboxes when in benchmark mode
        private bool _isInBenchmark = false;
        // helper for benchmarking logic
        public bool IsInBenchmark {
            get { return _isInBenchmark; }
            set {
                if (value) {
                    _isInBenchmark = value;
                    listViewAlgorithms.CheckBoxes = false;
                } else {
                    _isInBenchmark = value;
                    listViewAlgorithms.CheckBoxes = true;
                }
            }
        }

        public AlgorithmsListView() {
            InitializeComponent();
            // callback initializations
            listViewAlgorithms.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listViewAlgorithms_ItemSelectionChanged);
            listViewAlgorithms.ItemChecked += (ItemCheckedEventHandler)listViewAlgorithms_ItemChecked;
            IsInBenchmark = false;
        }

        public void InitLocale() {
            listViewAlgorithms.Columns[ENABLED].Text = International.GetText("AlgorithmsListView_Enabled");
            listViewAlgorithms.Columns[ALGORITHM].Text = International.GetText("AlgorithmsListView_Algorithm");
            listViewAlgorithms.Columns[SPEED].Text = International.GetText("AlgorithmsListView_Speed");
            listViewAlgorithms.Columns[RATIO].Text = International.GetText("AlgorithmsListView_Ratio");
            listViewAlgorithms.Columns[RATE].Text = International.GetText("AlgorithmsListView_Rate");
        }

        public void SetAlgorithms(ComputeDevice computeDevice, bool isEnabled) {
            _computeDevice = computeDevice;
            listViewAlgorithms.BeginUpdate();
            listViewAlgorithms.Items.Clear();
            foreach (var alg in computeDevice.GetAlgorithmSettings()) {
                ListViewItem lvi = new ListViewItem();
                ListViewItem.ListViewSubItem sub = lvi.SubItems.Add(String.Format("{0} ({1})", alg.AlgorithmName, alg.MinerBaseTypeName));

                //sub.Tag = alg.Value;
                lvi.SubItems.Add(alg.BenchmarkSpeedString());
                lvi.SubItems.Add(alg.CurPayingRatio);
                lvi.SubItems.Add(alg.CurPayingRate);
                lvi.Tag = alg;
                lvi.Checked = alg.Enabled;
                listViewAlgorithms.Items.Add(lvi);
            }
            listViewAlgorithms.EndUpdate();
            this.Enabled = isEnabled;
        }

        public void RepaintStatus(bool isEnabled, string uuid) {
            if (_computeDevice != null && _computeDevice.UUID == uuid) {
                foreach (ListViewItem lvi in listViewAlgorithms.Items) {
                    Algorithm algo = lvi.Tag as Algorithm;
                    lvi.SubItems[SPEED].Text = algo.BenchmarkSpeedString();
                    _listItemCheckColorSetter.LviSetColor(lvi);
                }
                this.Enabled = isEnabled;
            }
        }

        #region Callbacks Events
        private void listViewAlgorithms_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            if (ComunicationInterface != null) {
                ComunicationInterface.SetCurrentlySelected(e.Item, _computeDevice);
            }
        }

        private void listViewAlgorithms_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (IsInBenchmark) {
                e.Item.Checked = !e.Item.Checked;
                return;
            }
            var algo = e.Item.Tag as Algorithm;
            if (algo != null) {
                algo.Enabled = e.Item.Checked;
            }
            if (ComunicationInterface != null) {
                ComunicationInterface.HandleCheck(e.Item);
            }
            var lvi = e.Item as ListViewItem;
            _listItemCheckColorSetter.LviSetColor(lvi);
            // update benchmark status data
            if (BenchmarkCalculation != null) BenchmarkCalculation.CalcBenchmarkDevicesAlgorithmQueue();
        }
        #endregion //Callbacks Events

        public void ResetListItemColors() {
            foreach (ListViewItem lvi in listViewAlgorithms.Items) {
                if (_listItemCheckColorSetter != null) {
                    _listItemCheckColorSetter.LviSetColor(lvi);
                }
            }
        }

        // benchmark settings
        public void SetSpeedStatus(ComputeDevice computeDevice, Algorithm algorithm, string status) {
            if (algorithm != null) {
                algorithm.BenchmarkStatus = status;
                // gui update only if same as selected
                if (_computeDevice != null && computeDevice.UUID == _computeDevice.UUID) {
                    foreach (ListViewItem lvi in listViewAlgorithms.Items) {
                        Algorithm algo = lvi.Tag as Algorithm;
                        if (algo != null && algo.AlgorithmStringID == algorithm.AlgorithmStringID) {
                            // TODO handle numbers
                            lvi.SubItems[SPEED].Text = algorithm.BenchmarkSpeedString();
                            lvi.SubItems[RATE].Text = algorithm.CurPayingRate;
                            lvi.SubItems[RATIO].Text = algorithm.CurPayingRatio;
                            _listItemCheckColorSetter.LviSetColor(lvi);
                            break;
                        }
                    }
                }
            }
        }

        private void listViewAlgorithms_MouseClick(object sender, MouseEventArgs e) {
            if (IsInBenchmark) return;
            if (e.Button == MouseButtons.Right) {
                contextMenuStrip1.Items.Clear();
                // disable all
                {
                    var disableAllItems = new ToolStripMenuItem();
                    disableAllItems.Text = International.GetText("AlgorithmsListView_ContextMenu_DisableAll");
                    disableAllItems.Click += toolStripMenuItemDisableAll_Click;
                    contextMenuStrip1.Items.Add(disableAllItems);
                }
                // enable all
                {
                    var enableAllItems = new ToolStripMenuItem();
                    enableAllItems.Text = International.GetText("AlgorithmsListView_ContextMenu_EnableAll");
                    enableAllItems.Click += toolStripMenuItemEnableAll_Click;
                    contextMenuStrip1.Items.Add(enableAllItems);
                }
                // clear item
                {
                    var clearItem = new ToolStripMenuItem();
                    clearItem.Text = International.GetText("AlgorithmsListView_ContextMenu_ClearItem");
                    clearItem.Click += toolStripMenuItemClear_Click;
                    contextMenuStrip1.Items.Add(clearItem);
                }
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void toolStripMenuItemEnableAll_Click(object sender, EventArgs e) {
            foreach (ListViewItem lvi in listViewAlgorithms.Items) {
                lvi.Checked = true;
            }
        }

        private void toolStripMenuItemDisableAll_Click(object sender, EventArgs e) {
            foreach (ListViewItem lvi in listViewAlgorithms.Items) {
                lvi.Checked = false;
            }
        }

        private void toolStripMenuItemClear_Click(object sender, EventArgs e) {
            if (_computeDevice != null) {
                foreach (ListViewItem lvi in listViewAlgorithms.SelectedItems) {
                    var algorithm = lvi.Tag as Algorithm;
                    if (algorithm != null) {
                        algorithm.BenchmarkSpeed = 0;
                        RepaintStatus(_computeDevice.Enabled, _computeDevice.UUID);
                        // update benchmark status data
                        if (BenchmarkCalculation != null) BenchmarkCalculation.CalcBenchmarkDevicesAlgorithmQueue();
                        // update settings
                        if (ComunicationInterface != null) ComunicationInterface.ChangeSpeed(lvi);
                    }
                }
            }
        }

    }
}
