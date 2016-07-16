using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using NiceHashMiner.Configs;

namespace NiceHashMiner
{
    public partial class Form_Benchmark : Form
    {
        private int index;
        private bool inBenchmark;

        private int Time;
        private int TimeIndex = 1;
        private Miner CurrentlyBenchmarking;

        public Form_Benchmark(bool autostart)
        {
            InitializeComponent();

            this.Text = International.GetText("form2_title");
            buttonStartBenchmark.Text = International.GetText("form2_buttonStartBenchmark");
            buttonStopBenchmark.Text = International.GetText("form2_buttonStopBenchmark");
            buttonReset.Text = International.GetText("form2_buttonReset");
            buttonClose.Text = International.GetText("form2_buttonClose");
            buttonCheckProfitability.Text = International.GetText("form2_buttonCheckProfitability");
            buttonSubmitHardware.Text = International.GetText("form2_buttonSubmitHardware");

            radioButton_QuickBenchmark.Text = International.GetText("form2_radioButton_QuickBenchmark");
            radioButton_StandardBenchmark.Text = International.GetText("form2_radioButton_StandardBenchmark");
            radioButton_PreciseBenchmark.Text = International.GetText("form2_radioButton_PreciseBenchmark");

            listView1.Columns[0].Text = International.GetText("ListView_Enabled");
            listView1.Columns[1].Text = International.GetText("ListView_Device");
            listView1.Columns[2].Text = International.GetText("ListView_Algorithm");
            listView1.Columns[3].Text = International.GetText("ListView_Speed");

            foreach (Miner m in Globals.Miners)
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
                //lvi.SubItems[3].Text = "Please wait...";
                inBenchmark = true;
                CurrentlyBenchmarking = m;

                if (m is cpuminer)
                {
                    Time = Config.ConfigData.BenchmarkTimeLimitsCPU[TimeIndex];
                    lvi.SubItems[3].Text = String.Format(International.GetText("form2_listView_WaitSeconds"), Time);
                }
                else if (m is ccminer)
                {
                    Time = Config.ConfigData.BenchmarkTimeLimitsNVIDIA[TimeIndex];

                    if (lvi.SubItems[2].Text.Equals("daggerhashimoto"))
                        lvi.SubItems[3].Text = International.GetText("form2_listView_WaitForEth");
                    else
                        lvi.SubItems[3].Text = String.Format(International.GetText("form2_listView_WaitSeconds"), Time);
                }
                else
                {
                    Time = Config.ConfigData.BenchmarkTimeLimitsAMD[TimeIndex] / 60;

                    // add an aditional minute if second is not 0
                    if (DateTime.Now.Second != 0)
                        Time += 1;

                    if (lvi.SubItems[2].Text.Equals("daggerhashimoto"))
                        lvi.SubItems[3].Text = International.GetText("form2_listView_WaitForEth");
                    else
                        lvi.SubItems[3].Text = String.Format(International.GetText("form2_listView_WaitMinutes"), Time);
                }

                m.BenchmarkStart(i, Time, BenchmarkCompleted, lvi);
            }
            else
            {
                // average all cpu benchmarks
                if (Globals.Miners[0] is cpuminer)
                {
                    Helpers.ConsolePrint("BENCHMARK", "Calculating average CPU speeds:");

                    double[] Speeds = new double[Globals.Miners[0].SupportedAlgorithms.Length];
                    int[] MTaken = new int[Globals.Miners[0].SupportedAlgorithms.Length];

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
                        Helpers.ConsolePrint("BENCHMARK", Globals.Miners[0].SupportedAlgorithms[i].NiceHashName + " average speed: " + Globals.Miners[0].PrintSpeed(Speeds[i]));

                        foreach (Miner m in Globals.Miners)
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
                buttonClose.Enabled = true;
                buttonCheckProfitability.Enabled = true;
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
            buttonClose.Enabled = false;
            buttonCheckProfitability.Enabled = false;
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
            foreach (Miner m in Globals.Miners)
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

            for (int i = 0; i < Globals.Miners.Length; i++)
            {
                for (int j = 0; j < Globals.Miners[i].CDevs.Count; j++)
                {
                    Globals.Miners[i].CDevs[j].Enabled = true;
                    for (int k = 0; k < Config.ConfigData.Groups[i].DisabledDevices.Length; k++)
                    {
                        if (Config.ConfigData.Groups[i].DisabledDevices[k] == j)
                            Globals.Miners[i].CDevs[j].Enabled = false;
                    }
                }
            }
        }

        private void buttonCheckProfitability_Click(object sender, EventArgs e)
        {
            string url = "https://www.nicehash.com/?p=calc&name=CUSTOM";
            int len = Globals.NiceHashData == null ? 23 : Globals.NiceHashData.Length;
            double[] total = new double[len];

            for (int i = 0; i < len; i++)
                total[i] = 0;

            for (int i = 0; i < Globals.Miners.Length; i++)
            {
                if (Globals.Miners[i].EnabledDeviceCount() < 1) continue;
                for (int j = 0; j < Globals.Miners[i].SupportedAlgorithms.Length; j++)
                {
                    total[Globals.Miners[i].SupportedAlgorithms[j].NiceHashID] += Globals.Miners[i].SupportedAlgorithms[j].BenchmarkSpeed;
                }
            }

            for (int i = 0; i < len; i++)
                url += "&speed" + i + "=" + (total[i] / SubmitResultDialog.div[i]).ToString("F2", CultureInfo.InvariantCulture);

            System.Diagnostics.Process.Start(url);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
