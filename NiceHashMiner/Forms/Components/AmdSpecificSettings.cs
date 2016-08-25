using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;

namespace NiceHashMiner.Forms.Components {
    public partial class AmdSpecificSettings : UserControl {
        public AmdSpecificSettings() {
            InitializeComponent();

            checkBox_AMD_DisableAMDTempControl.Checked = ConfigManager.Instance.GeneralConfig.DisableAMDTempControl;
        }

        public void InitLocale(ToolTip toolTip1) {
            checkBox_AMD_DisableAMDTempControl.Text = International.GetText("Form_Settings_General_DisableAMDTempControl");
            toolTip1.SetToolTip(checkBox_AMD_DisableAMDTempControl, International.GetText("Form_Settings_ToolTip_DisableAMDTempControl"));
        }

        private void checkBox_AMD_DisableAMDTempControl_CheckedChanged(object sender, EventArgs e) {
            CheckBox chkbox = (CheckBox)sender;
            ConfigManager.Instance.GeneralConfig.DisableAMDTempControl = chkbox.Checked;
        }

    }
}
