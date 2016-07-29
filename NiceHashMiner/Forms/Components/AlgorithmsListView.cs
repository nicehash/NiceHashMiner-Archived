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

        private Miner CurrentlyBenchmarking;

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

        private static Color DisabledColor = Color.DarkGray;
        private static Color BenchmarkingColor = Color.LightGreen;

        List<DeviceBenchmarkConfig> _benchmarkConfigs;

        public void SetAlgorithms(DeviceBenchmarkConfig benchmarkConfig) {
            SetAlgorithms(new List<DeviceBenchmarkConfig>() { benchmarkConfig });
        }

        public void SetAlgorithms(List<DeviceBenchmarkConfig> benchmarkConfigs) {
            listViewAlgorithms.Items.Clear();
            _benchmarkConfigs = benchmarkConfigs;
            bool switchColor = false;
            foreach (var config in benchmarkConfigs) {
                foreach (var alg in config.BenchmarkSpeeds) {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Checked = !alg.Value.Skip;
                    if (switchColor) {
                        lvi.BackColor = Color.LightBlue;
                    }
                    lvi.SubItems.Add(config.DeviceName);
                    ListViewItem.ListViewSubItem sub = lvi.SubItems.Add(alg.Value.NiceHashName);

                    //sub.Tag = alg.Value;
                    if (alg.Value.BenchmarkSpeed > 0) {
                        //lvi.SubItems.Add(m.PrintSpeed(alg.Value.BenchmarkSpeed));
                        lvi.SubItems.Add(alg.Value.BenchmarkSpeed.ToString());
                    } else {
                        lvi.SubItems.Add("none");
                    }
                    lvi.Tag = alg.Value;
                    listViewAlgorithms.Items.Add(lvi);
                }
                switchColor = !switchColor;
            }
        }

        
        public void testMe() {
            StartBenchmark();
        }

        int _bechmarkCurrentIndex = 0;

        void StartBenchmark() {
            _bechmarkCurrentIndex = -1;
            GreyAll();
            NextBenchmark();
        }

        void NextBenchmark() {
            ++_bechmarkCurrentIndex;
            if (_bechmarkCurrentIndex >= listViewAlgorithms.Items.Count) {
                EndBenchmark();
                return;
            }

            int benchConfigIndex = 0;
            // get benchConfigIndex scope
            {
                int sum = 0;
                foreach (var config in _benchmarkConfigs) {
                    sum += config.BenchmarkSpeeds.Count;
                    if (sum > _bechmarkCurrentIndex) {
                        break;
                    }
                    ++benchConfigIndex;
                }
            }
            var currentConfig = _benchmarkConfigs[benchConfigIndex];
            var lvi = listViewAlgorithms.Items[_bechmarkCurrentIndex];
            var benchAlgorithm = lvi.Tag as Algorithm;
            CurrentlyBenchmarking = MinersManager.Instance.CreateBenchmarkMiner(currentConfig.DeviceGroupType);
            // TODO make skipp option for already benchmarked
            if (CurrentlyBenchmarking != null && benchAlgorithm != null && !benchAlgorithm.Skip) {
                // TODO time
                CurrentlyBenchmarking.BenchmarkStart(currentConfig, benchAlgorithm, 5, BenchmarkCompleted, lvi);
            } else {
                NextBenchmark();
            }

            MarkBenchColorComplete(_bechmarkCurrentIndex - 1);
            MarkBenchColorInProgress(_bechmarkCurrentIndex);
        }

        void EndBenchmark() {
            SplitGroupColors();
        }

        private void BenchmarkCompleted(bool success, string text, object tag) {
            if (this.InvokeRequired) {
                BenchmarkComplete d = new BenchmarkComplete(BenchmarkCompleted);
                this.Invoke(d, new object[] { success, text, tag });
            } else {
                //inBenchmark = false;
                //CurrentlyBenchmarking = null;

                ListViewItem lvi = tag as ListViewItem;
                lvi.SubItems[3].Text = text;

                // initiate new benchmark
                //InitiateBenchmark();
                NextBenchmark();
            }
        }

        private void GreyAll() {
            for (int i = 0; i < listViewAlgorithms.Items.Count; ++i) {
                listViewAlgorithms.Items[i].BackColor = DisabledColor;
            }
        }

        private void MarkBenchColorComplete(int index) {
            if (index >= 0 && index < listViewAlgorithms.Items.Count) {
                listViewAlgorithms.Items[index].BackColor = Color.LightBlue;
            }
        }
        private void MarkBenchColorInProgress(int index) {
            if (index >= 0 && index < listViewAlgorithms.Items.Count) {
                listViewAlgorithms.Items[index].BackColor = Color.LightGreen;
            }
        }

        private void SplitGroupColors() {
            bool switchColor = false;
            int currentIndex = 0;
            foreach (var config in _benchmarkConfigs) {
                foreach (var alg in config.BenchmarkSpeeds) {

                    if (switchColor) {
                        listViewAlgorithms.Items[currentIndex].BackColor = Color.LightBlue;
                    } else {
                        listViewAlgorithms.Items[currentIndex].BackColor = Color.White;
                    }
                    ++currentIndex;
                }
                switchColor = !switchColor;
            }
        }

        private void disableAllBut(int index) {
            for (int i = 0; i < listViewAlgorithms.Items.Count; ++i) {
                if (i != index) {
                    listViewAlgorithms.Items[i].BackColor = DisabledColor;
                    //listViewAlgorithms.Items[i].Selected = false;
                } else {
                    listViewAlgorithms.Items[i].BackColor = BenchmarkingColor;
                    if (ComunicationInterface != null) {
                        ComunicationInterface.SetCurrentlySelected(listViewAlgorithms.Items[i]);
                    }
                    //listViewAlgorithms.Items[i].Selected = true;
                }
            }
            //listViewAlgorithms.Select();
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
        }
        #endregion //Callbacks Events

    }
}
