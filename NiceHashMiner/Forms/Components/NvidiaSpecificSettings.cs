using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NiceHashMiner.Enums;
using NiceHashMiner.Configs;
using NiceHashMiner.Miners;

namespace NiceHashMiner.Forms.Components {
    public partial class NvidiaSpecificSettings : UserControl {
        public NvidiaSpecificSettings() {
            InitializeComponent();

            // fill dag dropdown
            comboBox_DagLoadMode.Items.Clear();
            for (int i = 0; i < (int)DagGenerationType.END; ++i) {
                comboBox_DagLoadMode.Items.Add(MinerEtherum.GetDagGenerationString((DagGenerationType)i));
            }
            // set selected
            comboBox_DagLoadMode.SelectedIndex = (int)ConfigManager.Instance.GeneralConfig.EthminerDagGenerationTypeNvidia;
        }

        private void comboBox_DagLoadMode_Leave(object sender, EventArgs e) {
            ConfigManager.Instance.GeneralConfig.EthminerDagGenerationTypeNvidia = (DagGenerationType)comboBox_DagLoadMode.SelectedIndex;
        }
    }
}
