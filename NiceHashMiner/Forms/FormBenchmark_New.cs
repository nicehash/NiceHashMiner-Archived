using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Forms {
    public partial class FormBenchmark_New : Form {
        public FormBenchmark_New() {
            InitializeComponent();

            SettupDevicesTab();
        }

        private void SettupDevicesTab() {
            List<BenchmarkConfig> benchmarkConfigs = new List<BenchmarkConfig>();
            foreach (var CDev in ComputeDevice.AllAvaliableDevices) {
                var benchConfig = BenchmarkConfigManager.Instance.GetConfig(CDev.DeviceGroupType, CDev.Name, new int[] { CDev.ID });

                benchmarkConfigs.Add(benchConfig);
            }

            devicesListView1.SetComputeDevices(ComputeDevice.AllAvaliableDevices);

            algorithmsListView1.SetAlgorithms(benchmarkConfigs);
            algorithmsListView1.ComunicationInterface = benchmarkAlgorithmSettup1;
            algorithmsListView1.SetSetupComplete();
        }

        private void button1_Click(object sender, EventArgs e) {
            devicesListView1.Enabled = false;
            //algorithmsListView1.Enabled = false;
            benchmarkAlgorithmSettup1.Enabled = false;
            benchmarkOptions1.Enabled = false;
            algorithmsListView1.testMe();
        }

    }
}
