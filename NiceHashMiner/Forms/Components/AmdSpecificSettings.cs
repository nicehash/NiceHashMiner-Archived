using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;

namespace NiceHashMiner.Forms.Components {
    public partial class AmdSpecificSettings : UserControl {
        public AmdSpecificSettings() {
            InitializeComponent();

            checkBox_AMD_DisableAMDTempControl.Checked = ConfigManager.Instance.GeneralConfig.DisableAMDTempControl;
            // fill dag dropdown
            comboBox_DagLoadMode.Items.Clear();
            for (int i = 0; i < (int)DagGenerationType.END; ++i) {
                comboBox_DagLoadMode.Items.Add(MinerEtherum.GetDagGenerationString((DagGenerationType)i));
            }
            // set selected
            comboBox_DagLoadMode.SelectedIndex = (int)ConfigManager.Instance.GeneralConfig.EthminerDagGenerationTypeAMD;
        }

        public void InitLocale(ToolTip toolTip1) {
            checkBox_AMD_DisableAMDTempControl.Text = International.GetText("Form_Settings_General_DisableAMDTempControl");
            toolTip1.SetToolTip(checkBox_AMD_DisableAMDTempControl, International.GetText("Form_Settings_ToolTip_DisableAMDTempControl"));
        }

        private void checkBox_AMD_DisableAMDTempControl_CheckedChanged(object sender, EventArgs e) {
            CheckBox chkbox = (CheckBox)sender;
            ConfigManager.Instance.GeneralConfig.DisableAMDTempControl = chkbox.Checked;
        }

        private void comboBox_DagLoadMode_Leave(object sender, EventArgs e) {
            ConfigManager.Instance.GeneralConfig.EthminerDagGenerationTypeAMD = (DagGenerationType)comboBox_DagLoadMode.SelectedIndex;
        }

    }
}
