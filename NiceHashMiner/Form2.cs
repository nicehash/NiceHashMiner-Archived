using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public partial class Form2 : Form
    {
        private int index;
        private bool inBenchmark;

        public Form2(bool autostart)
        {
            InitializeComponent();

            foreach (Miner m in Form1.Miners)
            {
                for (int i = 0; i < m.SupportedAlgorithms.Length; i++)
                {
                    ListViewItem lvi = new ListViewItem(m.MinerDeviceName);
                    ListViewItem.ListViewSubItem sub = lvi.SubItems.Add(m.SupportedAlgorithms[i].NiceHashName);
                    sub.Tag = i;
                    if (m.SupportedAlgorithms[i].BenchmarkSpeed > 0)
                        lvi.SubItems.Add(m.PrintSpeed(m.SupportedAlgorithms[i].BenchmarkSpeed));
                    else
                        lvi.SubItems.Add("");
                    lvi.Tag = m;
                    listView1.Items.Add(lvi);
                }
            }

            inBenchmark = false;

            if (autostart)
                startBenchmarkToolStripMenuItem_Click(null, null);
        }


        private void startBenchmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startBenchmarkToolStripMenuItem.Text == "Stop benchmark")
            {
                index = 9999;
                return;
            }

            startBenchmarkToolStripMenuItem.Text = "Stop benchmark";

            index = 0;
            InitiateBenchmark();
        }


        private void BenchmarkComplete(string text, object tag)
        {
            inBenchmark = false;

            ListViewItem lvi = tag as ListViewItem;
            lvi.SubItems[2].Text = text;

            // todo: average out cpu benchmarks

            // initiate new benchmark
            InitiateBenchmark();
        }


        private void InitiateBenchmark()
        {
            if (listView1.Items.Count > index)
            {
                ListViewItem lvi = listView1.Items[index];
                index++;
                Miner m = lvi.Tag as Miner;
                int i = (int)lvi.SubItems[1].Tag;
                lvi.SubItems[2].Text = "Please wait...";
                inBenchmark = true;
                m.BenchmarkStart(i, BenchmarkComplete, lvi);
            }
            else
            {
                startBenchmarkToolStripMenuItem.Text = "Start benchmark";
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (inBenchmark) e.Cancel = true;
        }
    }
}
