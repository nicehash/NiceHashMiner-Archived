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

namespace NiceHashMiner.Forms.Components {
    public partial class AlgorithmsListView : UserControl {

        public interface IAlgorithmsListView {
            void SetCurrentlySelected(ListViewItem lvi);
            void HandleCheck(ListViewItem lvi);
        }

        public AlgorithmsListView() {
            InitializeComponent();
            // callback initializations
            listViewAlgorithms.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listViewAlgorithms_ItemSelectionChanged);
            listViewAlgorithms.ItemChecked += new ItemCheckedEventHandler(listViewAlgorithms_ItemChecked);
        }

        public IAlgorithmsListView ComunicationInterface { get; set; }

        private static Color _disabledColor = Color.DarkGray;
        private static Color _benchmarkedColor = Color.LightGreen;
        private static Color _unbenchmarkedColor = Color.LightBlue;

        public void SetAlgorithms(DeviceBenchmarkConfig benchmarkConfig) {
            SetAlgorithms(new List<DeviceBenchmarkConfig>() { benchmarkConfig });
        }

        public void SetAlgorithms(List<DeviceBenchmarkConfig> benchmarkConfigs) {
            listViewAlgorithms.Items.Clear();
            bool switchColor = false;
            foreach (var config in benchmarkConfigs) {
                foreach (var alg in config.AlgorithmSettings) {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Checked = !alg.Value.Skip;
                    lvi.SubItems.Add(config.DeviceName);
                    ListViewItem.ListViewSubItem sub = lvi.SubItems.Add(alg.Value.NiceHashName);

                    //sub.Tag = alg.Value;
                    if (alg.Value.BenchmarkSpeed > 0) {
                        lvi.SubItems.Add(Helpers.FormatSpeedOutput(alg.Value.BenchmarkSpeed));

                    } else {
                        lvi.SubItems.Add("none");
                    }
                    lvi.Tag = alg.Value;
                    LviSetColor(ref lvi);
                    listViewAlgorithms.Items.Add(lvi);
                }
                switchColor = !switchColor;
            }
        }

        private void LviSetColor(ref ListViewItem lvi) {
            Algorithm algorithm = lvi.Tag as Algorithm;
            if (algorithm != null) {
                if (algorithm.Skip) {
                    lvi.BackColor = _disabledColor;
                } else if (algorithm.BenchmarkSpeed > 0) {
                    lvi.BackColor = _benchmarkedColor;
                } else {
                    lvi.BackColor = _unbenchmarkedColor;
                }
            }
        }

        #region Callbacks Events
        private void listViewAlgorithms_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            if (ComunicationInterface != null) {
                ComunicationInterface.SetCurrentlySelected(e.Item);
            }
        }

        private void listViewAlgorithms_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (ComunicationInterface != null) {
                var algo = e.Item.Tag as Algorithm;
                if(algo != null) {
                    algo.Skip = !e.Item.Checked;
                }
                ComunicationInterface.HandleCheck(e.Item);
            }
            var lvi = e.Item as ListViewItem;
            LviSetColor(ref lvi);
        }
        #endregion //Callbacks Events

    }
}
