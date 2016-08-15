using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Enums;


namespace NiceHashMiner.Forms.Components {
    public partial class BenchmarkOptions : UserControl {

        public BenchmarkPerformanceType PerformanceType { get; private set; }

        public BenchmarkOptions() {
            InitializeComponent();
        }

        public void InitLocale() {
            radioButton_QuickBenchmark.Text = International.GetText("form2_radioButton_QuickBenchmark");
            radioButton_StandardBenchmark.Text = International.GetText("form2_radioButton_StandardBenchmark");
            radioButton_PreciseBenchmark.Text = International.GetText("form2_radioButton_PreciseBenchmark");
        }

        private void radioButton_QuickBenchmark_CheckedChanged(object sender, EventArgs e) {
            PerformanceType = BenchmarkPerformanceType.Quick;
        }

        private void radioButton_StandardBenchmark_CheckedChanged(object sender, EventArgs e) {
            PerformanceType = BenchmarkPerformanceType.Standard;
        }

        private void radioButton_PreciseBenchmark_CheckedChanged(object sender, EventArgs e) {
            PerformanceType = BenchmarkPerformanceType.Precise;
        }
    }
}
