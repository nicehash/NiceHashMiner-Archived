using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NiceHashMiner.Forms.Components {
    public partial class GroupProfitControl : UserControl {
        public GroupProfitControl() {
            InitializeComponent();
        }


        public void UpdateProfitStats(string groupName, string deviceStringInfo,
            string speedString, string btcRateString, string currencyRateString) {
            bool isGPU = groupName.Contains("NVIDIA") || groupName.Contains("AMD");
            string gpuPrefix = isGPU ? "GPU " : "";
            //groupBoxMinerGroup.Text = String.Format("{0}{1} Mining Devices {2}:", gpuPrefix, groupName, deviceStringInfo);
            groupBoxMinerGroup.Text = String.Format(International.GetText("Form_Main_MiningDevices"), deviceStringInfo);
            labelSpeedValue.Text = speedString;
            labelBTCRateValue.Text = btcRateString;
            labelCurentcyPerDayVaue.Text = currencyRateString;
        }

    }
}
