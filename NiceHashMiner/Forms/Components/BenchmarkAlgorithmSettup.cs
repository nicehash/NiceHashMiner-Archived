using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms.Components {
    public partial class BenchmarkAlgorithmSettup : UserControl, AlgorithmsListView.IAlgorithmsListView {

        Algorithm _currentlySelectedAlgorithm = null;
        ListViewItem _currentlySelectedLvi = null;
        bool _settingNew = false;

        public BenchmarkAlgorithmSettup() {
            InitializeComponent();
        }
        
        private void checkBoxEnabled_CheckedChanged(object sender, EventArgs e) {
            toggleAlgorithmChecked(_currentlySelectedAlgorithm, checkBoxEnabled.Checked);
        }

        public void SetCurrentlySelected(ListViewItem lvi) {
            // should not happen ever
            if (lvi == null) return;

            _settingNew = true;
            var algorithm = lvi.Tag as Algorithm;
            if (algorithm != null) {
                _currentlySelectedAlgorithm = algorithm;
                _currentlySelectedLvi = lvi;
                // bind check boxes
                {
                    // remove old 
                    if (checkBoxEnabled.DataBindings.Count != 0) checkBoxEnabled.DataBindings.Clear();
                    // set new
                    checkBoxEnabled.DataBindings.Add("Checked", lvi, "Checked", true, DataSourceUpdateMode.OnPropertyChanged);
                }

                //labelDeviceGroup.Text = "Device or Group";
                //labelAlgorithm.Text = "Selected Algorithm: " + algorithm.NiceHashName;

                groupBoxSelectedAlgorithmSettings.Text = algorithm.NiceHashName + " Settings:";

                checkBoxEnabled.Checked = !algorithm.Skip;
                fieldBoxPassword.EntryText = algorithm.UsePassword;
                fieldBoxBenchmarkSpeed.EntryText = algorithm.BenchmarkSpeed <= 0 ? "" : algorithm.BenchmarkSpeed.ToString();
                richTextBoxExtraLaunchParameters.Text = algorithm.ExtraLaunchParameters;
                this.Update();
            } else {

            }
            _settingNew = false;
        }

        private void toggleAlgorithmChecked(Algorithm algorithm, bool isCheckedEnabled) {
            if (algorithm != null && _settingNew == false) {
                algorithm.Skip = !checkBoxEnabled.Checked;
            }
        }

        public void HandleCheck(ListViewItem lvi) {
            if (Object.ReferenceEquals(_currentlySelectedLvi, lvi)) {
                checkBoxEnabled.Checked = lvi.Checked;
            } else {
                toggleAlgorithmChecked(lvi.Tag as Algorithm, lvi.Checked);
            }
        }
    }
}
