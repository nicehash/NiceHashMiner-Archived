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

        public void SetPerformanceType(BenchmarkPerformanceType performanceType) {
            switch (performanceType) {
                case BenchmarkPerformanceType.Quick:
                    radioButton_QuickBenchmark.Checked = true;
                    PerformanceType = BenchmarkPerformanceType.Quick;
                    break;
                case BenchmarkPerformanceType.Standard:
                    radioButton_StandardBenchmark.Checked = true;
                    PerformanceType = BenchmarkPerformanceType.Standard;
                    break;
                case BenchmarkPerformanceType.Precise:
                    radioButton_PreciseBenchmark.Checked = true;
                    PerformanceType = BenchmarkPerformanceType.Precise;
                    break;
                default:
                    radioButton_StandardBenchmark.Checked = true;
                    break;
            }
        }

        public void InitLocale() {
            groupBox1.Text = International.GetText("BenchmarkOptions_Benchmark_Type");
            radioButton_QuickBenchmark.Text = International.GetText("Form_Benchmark_radioButton_QuickBenchmark");
            radioButton_StandardBenchmark.Text = International.GetText("Form_Benchmark_radioButton_StandardBenchmark");
            radioButton_PreciseBenchmark.Text = International.GetText("Form_Benchmark_radioButton_PreciseBenchmark");
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
