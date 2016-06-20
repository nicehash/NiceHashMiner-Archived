using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public partial class Form2 : Form
    {
        private int index;
        private bool inBenchmark;

        private int Time;
        private int TimeIndex = 1;
        private Miner CurrentlyBenchmarking;

        public Form2(bool autostart)
        {
            InitializeComponent();

            foreach (Miner m in Form1.Miners)
            {
                for (int i = 0; i < m.SupportedAlgorithms.Length; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Checked = !m.SupportedAlgorithms[i].Skip;
                    if (m.EnabledDeviceCount() == 0)
                    {
                        lvi.Checked = false;
                        lvi.BackColor = Color.LightGray;
                    }
                    lvi.SubItems.Add(m.MinerDeviceName);
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
                buttonStartBenchmark_Click(null, null);
        }


        private void BenchmarkCompleted(bool success, string text, object tag)
        {
            if (this.InvokeRequired)
            {
                BenchmarkComplete d = new BenchmarkComplete(BenchmarkCompleted);
                this.Invoke(d, new object[] { success, text, tag });
            }
            else
            {
                inBenchmark = false;
                CurrentlyBenchmarking = null;

                ListViewItem lvi = tag as ListViewItem;
                lvi.SubItems[3].Text = text;

                // initiate new benchmark
                InitiateBenchmark();
            }
        }


        private void InitiateBenchmark()
        {
            if (listView1.Items.Count > index)
            {
                ListViewItem lvi = listView1.Items[index];
                index++;

                if (!lvi.Checked)
                {
                    InitiateBenchmark();
                    return;
                }

                Miner m = lvi.Tag as Miner;
                int i = (int)lvi.SubItems[2].Tag;
                lvi.SubItems[3].Text = "Please wait...";
                inBenchmark = true;
                CurrentlyBenchmarking = m;

                if (m is cpuminer)
                {
                    Time = Config.ConfigData.BenchmarkTimeLimitsCPU[TimeIndex];
                    lvi.SubItems[3].Text = "Please wait about " + Time + " seconds...";
                }
                else if (m is ccminer)
                {
                    Time = Config.ConfigData.BenchmarkTimeLimitsNVIDIA[TimeIndex];

                    if (lvi.SubItems[2].Text.Equals("daggerhashimoto"))
                        lvi.SubItems[3].Text = "Benchmarking (2-4 minutes)...";
                    else
                        lvi.SubItems[3].Text = "Please wait about " + Time + " seconds...";
                }
                else
                {
                    Time = Config.ConfigData.BenchmarkTimeLimitsAMD[TimeIndex] / 60;

                    // add an aditional minute if second is not 0
                    if (DateTime.Now.Second != 0)
                        Time += 1;

                    if (lvi.SubItems[2].Text.Equals("daggerhashimoto"))
                        lvi.SubItems[3].Text = "Benchmarking (2-4 minutes)...";
                    else
                        lvi.SubItems[3].Text = "Please wait about " + Time + " minutes...";
                }

                m.BenchmarkStart(i, Time, BenchmarkCompleted, lvi);
            }
            else
            {
                // average all cpu benchmarks
                if (Form1.Miners[0] is cpuminer)
                {
                    Helpers.ConsolePrint("BENCHMARK", "Calculating average CPU speeds:");

                    double[] Speeds = new double[Form1.Miners[0].SupportedAlgorithms.Length];
                    int[] MTaken = new int[Form1.Miners[0].SupportedAlgorithms.Length];

                    foreach (ListViewItem lvi in listView1.Items)
                    {
                        if (lvi.Tag is cpuminer)
                        {
                            Miner m = lvi.Tag as Miner;
                            int i = (int)lvi.SubItems[2].Tag;
                            if (m.SupportedAlgorithms[i].BenchmarkSpeed > 0)
                            {
                                Speeds[i] += m.SupportedAlgorithms[i].BenchmarkSpeed;
                                MTaken[i]++;
                            }
                        }
                    }

                    for (int i = 0; i < Speeds.Length; i++)
                    {
                        if (MTaken[i] > 0) Speeds[i] /= MTaken[i];
                        Helpers.ConsolePrint("BENCHMARK", Form1.Miners[0].SupportedAlgorithms[i].NiceHashName + " average speed: " + Form1.Miners[0].PrintSpeed(Speeds[i]));

                        foreach (Miner m in Form1.Miners)
                        {
                            if (m is cpuminer)
                                m.SupportedAlgorithms[i].BenchmarkSpeed = Speeds[i];
                        }
                    }
                }

                foreach (ListViewItem lvi in listView1.Items)
                {
                    Miner m = lvi.Tag as Miner;
                    int i = (int)lvi.SubItems[2].Tag;
                    lvi.SubItems[3].Text = m.PrintSpeed(m.SupportedAlgorithms[i].BenchmarkSpeed);
                }
                
                Config.RebuildGroups();

                buttonStartBenchmark.Enabled = true;
                buttonStopBenchmark.Enabled = false;
                buttonReset.Enabled = true;
                buttonSubmitHardware.Enabled = true;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (inBenchmark) e.Cancel = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TimeIndex = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TimeIndex = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TimeIndex = 2;
        }

        private void buttonStartBenchmark_Click(object sender, EventArgs e)
        {
            index = 0;
            buttonStartBenchmark.Enabled = false;
            buttonStopBenchmark.Enabled = true;
            buttonReset.Enabled = false;
            buttonSubmitHardware.Enabled = false;
            InitiateBenchmark();
        }

        private void buttonStopBenchmark_Click(object sender, EventArgs e)
        {
            if (CurrentlyBenchmarking != null)
                CurrentlyBenchmarking.BenchmarkSignalQuit = true;
            index = 9999;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            foreach (Miner m in Form1.Miners)
            {
                for (int i = 0; i < m.SupportedAlgorithms.Length; i++)
                {
                    m.SupportedAlgorithms[i].BenchmarkSpeed = 0;
                }
            }

            Config.RebuildGroups();

            foreach (ListViewItem lvi in listView1.Items)
            {
                lvi.SubItems[3].Text = "";
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Miner m = e.Item.Tag as Miner;
            if (m.EnabledDeviceCount() == 0)
                e.Item.Checked = false;
            else
            {
                int i = (int)e.Item.SubItems[2].Tag;
                m.SupportedAlgorithms[i].Skip = !e.Item.Checked;
                Config.RebuildGroups();
            }
        }

        private void buttonSubmitHardware_Click(object sender, EventArgs e)
        {
            Form SubmitResultDialog = new SubmitResultDialog(TimeIndex);
            SubmitResultDialog.ShowDialog();
            SubmitResultDialog = null;

            for (int i = 0; i < Form1.Miners.Length; i++)
            {
                for (int j = 0; j < Form1.Miners[i].CDevs.Count; j++)
                {
                    Form1.Miners[i].CDevs[j].Enabled = true;
                    for (int k = 0; k < Config.ConfigData.Groups[i].DisabledDevices.Length; k++)
                    {
                        if (Config.ConfigData.Groups[i].DisabledDevices[k] == j)
                            Form1.Miners[i].CDevs[j].Enabled = false;
                    }
                }
            }
        }

        private void buttonCheckProfitability_Click(object sender, EventArgs e)
        {
            string url = "https://www.nicehash.com/?p=calc&name=CUSTOM";
            double[] total = new double[Form1.NiceHashData.Length];

            for (int i = 0; i < Form1.NiceHashData.Length; i++)
                total[i] = 0;

            for (int i = 0; i < Form1.Miners.Length; i++)
            {
                if (Form1.Miners[i].EnabledDeviceCount() < 1) continue;
                for (int j = 0; j < Form1.Miners[i].SupportedAlgorithms.Length; j++)
                {
                    total[Form1.Miners[i].SupportedAlgorithms[j].NiceHashID] += Form1.Miners[i].SupportedAlgorithms[j].BenchmarkSpeed;
                }
            }

            for (int i = 0; i < Form1.NiceHashData.Length; i++)
                url += "&speed" + i + "=" + (total[i] / SubmitResultDialog.div[i]);

            System.Diagnostics.Process.Start(url);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
