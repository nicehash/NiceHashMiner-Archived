using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Forms.Components {
    public partial class CpuSpecificSettings : UserControl {
        public CpuSpecificSettings() {
            InitializeComponent();

            comboBox_CPU0_ForceCPUExtension.SelectedIndex = (int)ConfigManager.Instance.GeneralConfig.ForceCPUExtension;
            comboBox_CPU0_ForceCPUExtension.SelectedIndexChanged += comboBox_CPU0_ForceCPUExtension_SelectedIndexChanged;
            textBox_CPU0_LessThreads.Text = ConfigManager.Instance.GeneralConfig.LessThreads.ToString();
            textBox_CPU0_LessThreads.Leave += LessThreads_Leave;
            textBox_CPU0_LessThreads.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
        }

        public void InitLocale(ToolTip toolTip1) {
            label_CPU0_ForceCPUExtension.Text = International.GetText("Form_Settings_General_CPU_ForceCPUExtension") + ":";
            label_CPU0_LessThreads.Text = International.GetText("Form_Settings_General_CPU_LessThreads") + ":";

            // Setup Tooltips
            toolTip1.SetToolTip(comboBox_CPU0_ForceCPUExtension, International.GetText("Form_Settings_ToolTip_CPU_ForceCPUExtension"));
            toolTip1.SetToolTip(label_CPU0_ForceCPUExtension, International.GetText("Form_Settings_ToolTip_CPU_ForceCPUExtension"));
            toolTip1.SetToolTip(textBox_CPU0_LessThreads, International.GetText("Form_Settings_ToolTip_CPU_LessThreads"));
            toolTip1.SetToolTip(label_CPU0_LessThreads, International.GetText("Form_Settings_ToolTip_CPU_LessThreads"));
        }

        private void comboBox_CPU0_ForceCPUExtension_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox cmbbox = (ComboBox)sender;
            ConfigManager.Instance.GeneralConfig.ForceCPUExtension = (CPUExtensionType)cmbbox.SelectedIndex;
        }

        private void LessThreads_Leave(object sender, EventArgs e) {
            TextBox txtbox = (TextBox)sender;

            int val;
            if (Int32.TryParse(txtbox.Text, out val))
                ConfigManager.Instance.GeneralConfig.LessThreads = val;
            else {
                MessageBox.Show(International.GetText("Form_Settings_LessThreadWarningMsg"),
                                International.GetText("Form_Settings_LessThreadWarningTitle"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtbox.Text = ConfigManager.Instance.GeneralConfig.LessThreads.ToString();
                txtbox.Focus();
            }
        }
        
    }
}
