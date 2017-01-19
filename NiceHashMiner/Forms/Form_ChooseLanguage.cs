using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms {
    public partial class Form_ChooseLanguage : Form {

        private static readonly string TOS_TEXT = "NiceHash Miner Terms Of Use NiceHash operates as in intermediate service by providing hashing power from hashing power owners to hashing power buyers. NiceHash does not directly provide it's own hashing power. We will do our best to provide stable, secure and feature full service. We do not take responsibility for any kind of hashing power loss or funds loss and do not take any kind of financial, material, legal or other responsibilities for any issues that my arise from using NiceHash Miner. NiceHash service and it's products (NiceHash Miner, etc.) is still in development, therefore some bugs or other issues may arise. We will work hard to fix any issues as soon as possible, add new features and overall improve our service. NiceHash reserves the rights to seize any funds of suspicious illegal activity such as mining with botnets, money laundering, hacking attempts, etc.\r\n\r\nAs a user of NiceHash Miner, you are providing your hashing power (your are a seller) to the hashing power buyers though NiceHash's hashing power marketplace. You earn Bitcoins from selling your hashing power for every valid share your miner generates and is accepted by NiceHash service. In some cases no shares are sent to or are accepted by NiceHash service. This cases are rare and includes usage of slower hardware, software or network errors or simmilar. In these cases (no shares generated and accepted by NiceHash service) no reward in form of Bitcoins can be accounted to you. Payouts are automatic and are paid to BTC address, used in NiceHash Miner. Payment schedule can be found in FAQ on our website and is subject to change. You can always monitor your statistics on this site: https://www.nicehash.com/?p=myminer \r\n\r\nIf you have any questions relating to these Terms of Use, your rights and obligations arising from these Terms and/or your use of the NiceHash service, or any other matter, please contact us at support@nicehash.com.";

        public Form_ChooseLanguage() {
            InitializeComponent();

            // Add language selections list
            Dictionary<LanguageType, string> lang = International.GetAvailableLanguages();

            comboBox_Languages.Items.Clear();
            for (int i = 0; i < lang.Count; i++) {
                comboBox_Languages.Items.Add(lang[(LanguageType)i]);
            }

            comboBox_Languages.SelectedIndex = 0;

            //label_Instruction.Location = new Point((this.Width - label_Instruction.Size.Width) / 2, label_Instruction.Location.Y);
            //button_OK.Location = new Point((this.Width - button_OK.Size.Width) / 2, button_OK.Location.Y);
            //comboBox_Languages.Location = new Point((this.Width - comboBox_Languages.Size.Width) / 2, comboBox_Languages.Location.Y);
            this.textBox_TOS.Text = TOS_TEXT;

        }

        private void button_OK_Click(object sender, EventArgs e) {
            ConfigManager.GeneralConfig.Language = (LanguageType)comboBox_Languages.SelectedIndex;
            ConfigManager.GeneralConfigFileCommit();
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if (checkBox_TOS.Checked) {
                ConfigManager.GeneralConfig.agreedWithTOS = Globals.CURRENT_TOS_VER;
                comboBox_Languages.Enabled = true;
                button_OK.Enabled = true;
            } else {
                ConfigManager.GeneralConfig.agreedWithTOS = 0;
                comboBox_Languages.Enabled = false;
                button_OK.Enabled = false;
            }
        }
    }
}
