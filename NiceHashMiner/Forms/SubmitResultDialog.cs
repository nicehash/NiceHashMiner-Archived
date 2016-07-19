using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;

namespace NiceHashMiner
{
    public partial class SubmitResultDialog : Form
    {
        // TODO get back to this recheck if global
        public static double[] div;
        private bool InBenchmark;
        private int Time, TimeIndex, DeviceChecked_Index;
        private string CurrentAlgoName;
        private Miner mm;

        public SubmitResultDialog(int tI)
        {
            InitializeComponent();

            this.Text = International.GetText("SubmitResultDialog_title");
            labelInstruction.Text = International.GetText("SubmitResultDialog_labelInstruction");
            StartStopBtn.Text = International.GetText("SubmitResultDialog_StartBtn");
            CloseBtn.Text = International.GetText("SubmitResultDialog_CloseBtn");

            DevicesListView.Columns[0].Text = International.GetText("ListView_Group");
            DevicesListView.Columns[1].Text = International.GetText("ListView_Device");

            TimeIndex = tI;
            InBenchmark = false;
            DeviceChecked_Index = 0;
            mm = null;

            // #new way // # TODO CPU hangs up NullReference Exception (old implementation same issue)
            foreach (var computeDevice in ComputeDevice.AllAvaliableDevices)
            {
                ListViewItem lvi = new ListViewItem(computeDevice.Vendor);
                lvi.SubItems.Add(computeDevice.Name);
                lvi.SubItems.Add(computeDevice.ID.ToString());
                lvi.Tag = computeDevice.Miner;
                DevicesListView.Items.Add(lvi);
            }
        }

        static SubmitResultDialog()
        {
            div = new double[] { 1000000,       //   0 (MH/s) Scrypt
                                 1000000000000, //   1 (TH/s) SHA256
                                 1000000,       //   2 (MH/s) ScryptNf
                                 1000000,       //   3 (MH/s) X11
                                 1000000,       //   4 (MH/s) X13
                                 1000000,       //   5 (MH/s) Keccak
                                 1000000,       //   6 (MH/s) X15
                                 1000000,       //   7 (MH/s) Nist5
                                 1000000,       //   8 (MH/s) NeoScrypt
                                 1000000,       //   9 (MH/s) Lyra2RE
                                 1000000,       //  10 (MH/s) WhirlpoolX
                                 1000000,       //  11 (MH/s) Qubit
                                 1000000,       //  12 (MH/s) Quark
                                 1000,          //  13 (kH/s) Axiom
                                 1000000,       //  14 (MH/s) Lyra2REv2
                                 1000,          //  15 (kH/s) ScryptJaneNf16
                                 1000000000,    //  16 (GH/s) Blake256r8
                                 1000000000,    //  17 (GH/s) Blake256r14
                                 1000000000,    //  18 (GH/s) Blake256r8vnl
                                 1000,          //  19 (kH/s) Hodl
                                 1000000,       //  20 (MH/s) Daggerhashimoto
                                 1000000000,    //  21 (GH/s) Decred
                                 1000000 };     // 999 (MH/s) Ethereum
        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BenchmarkBtn_Click(object sender, EventArgs e)
        {
            if (InBenchmark)
            {
                mm.BenchmarkSignalQuit = true;
                InBenchmark = false;
                index = 9999;
                return;
            }

            bool DeviceChecked = false;
            string DeviceName = "";

            for (int i = 0; i < DevicesListView.Items.Count; i++)
            {
                if (DevicesListView.Items[i].Selected)
                {
                    DeviceChecked = true;
                    Int32.TryParse(DevicesListView.Items[i].SubItems[2].Text, out DeviceChecked_Index);

                    mm = DevicesListView.Items[i].Tag as Miner;
                    DeviceName = DevicesListView.Items[i].SubItems[1].Text;
                }
            }

            if (!DeviceChecked)
            {
                MessageBox.Show(International.GetText("SubmitResultDialog_NoDeviceCheckedMsg"),
                                International.GetText("SubmitResultDialog_NoDeviceCheckedTitle"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InBenchmark = true;
            DevicesListView.Enabled = false;
            CloseBtn.Enabled = false;
            StartStopBtn.Text = International.GetText("SubmitResultDialog_StopBtn");
            LabelProgressPercentage.Text = "0.00%";
            index = 0;

            Helpers.ConsolePrint("SubmitResultDialog", "Number of Devices: " + mm.CDevs.Count);
            if (mm.CDevs.Count == 1 && mm.CountBenchmarkedAlgos() != 0)
            {
                DialogResult result = MessageBox.Show(International.GetText("SubmitResultDialog_UsePreviousBenchmarkedValueMsg"),
                                                      International.GetText("SubmitResultDialog_UsePreviousBenchmarkedValueTitle"),
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes) index = 9999;
            }

            // Temporarily disable the other ComputeDevices in the same Group
            for (int i = 0; i < mm.CDevs.Count; i++)
            {
                if (mm.CDevs[i].ID != DeviceChecked_Index)
                    mm.CDevs[i].Enabled = false;
                else
                    mm.CDevs[i].Enabled = true;
            }

            BenchmarkProgressBar.Maximum = mm.SupportedAlgorithms.Length;

            // Parse GPU name
            Helpers.ConsolePrint("SubmitResultDialog", "Old DeviceName: " + DeviceName);
            if (DeviceName.Contains("GeForce") || DeviceName.Contains("GTX") || DeviceName.Contains("GT"))
            {
                string [] DeviceNameSplit = DeviceName.Split(' ');

                for (int i = 0; i < DeviceNameSplit.Length; i++)
                {
                    Helpers.ConsolePrint("DEBUG", "DeviceNameSplit[" + i + "]: " + DeviceNameSplit[i]);

                    if (DeviceNameSplit[i].Equals("GT") || DeviceNameSplit[i].Equals("GTX"))
                    {
                        if ((i + 2) <= DeviceNameSplit.Length)
                        {
                            DeviceName = "NVIDIA " + DeviceNameSplit[i] + DeviceNameSplit[i + 1];
                            for (int j = i + 2; j < DeviceNameSplit.Length; j++)
                            {
                                DeviceName += " " + DeviceNameSplit[j];
                            }

                            break;
                        }
                    }
                }
            }
            Helpers.ConsolePrint("SubmitResultDialog", "New DeviceName: " + DeviceName);

            url = "https://www.nicehash.com/?p=calc&name=" + DeviceName;
            InitiateBenchmark();
        }

        private int index;
        private string url;

        private void InitiateBenchmark()
        {
            int algoIndex = index;
            index++;

            if (algoIndex < mm.SupportedAlgorithms.Length)
            {
                if (!mm.SupportedAlgorithms[algoIndex].Skip)
                {
                    if (mm is cpuminer)
                        Time = Config.ConfigData.BenchmarkTimeLimitsCPU[TimeIndex];
                    else if (mm is ccminer)
                        Time = Config.ConfigData.BenchmarkTimeLimitsNVIDIA[TimeIndex];
                    else
                    {
                        Time = Config.ConfigData.BenchmarkTimeLimitsAMD[TimeIndex] / 60;

                        // add an aditional minute if second is not 0
                        if (DateTime.Now.Second != 0)
                            Time += 1;
                    }

                    CurrentAlgoName = mm.SupportedAlgorithms[algoIndex].NiceHashName;
                    UpdateProgressBar(false);
                    mm.BenchmarkStart(algoIndex, Time, BenchmarkCompleted, DevicesListView.Items[DeviceChecked_Index].Tag);
                }
                else
                {
                    UpdateProgressBar(true);
                    InitiateBenchmark();
                }
            }
            else
            {

                for (int i = 0; i < mm.SupportedAlgorithms.Length; i++)
                {
                    if (!mm.SupportedAlgorithms[i].Skip)
                    {
                        int id = mm.SupportedAlgorithms[i].NiceHashID;
                        url += "&speed" + id + "=" + (mm.SupportedAlgorithms[i].BenchmarkSpeed / div[id]).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                InBenchmark = false;
                DevicesListView.Enabled = true;
                CloseBtn.Enabled = true;
                StartStopBtn.Text = International.GetText("SubmitResultDialog_StartBtn");
                BenchmarkProgressBar.Value = 0;
                LabelProgressPercentage.Text = International.GetText("SubmitResultDialog_LabelProgressPercentageCompleted");

                if (mm.BenchmarkSignalQuit)
                {
                    LabelProgressPercentage.Text = International.GetText("SubmitResultDialog_LabelProgressPercentageStopped");
                    return;
                }

                url += "&nhmver=" + Application.ProductVersion.ToString();  // Add version info
                url += "&cost=1&power=1"; // Set default power and cost to 1
                System.Diagnostics.Process.Start(url);
            }
        }

        private void BenchmarkCompleted(bool success, string text, object tag)
        {
            if (this.InvokeRequired)
            {
                UpdateProgressBar(true);
                BenchmarkComplete d = new BenchmarkComplete(BenchmarkCompleted);
                this.Invoke(d, new object[] { success, text, tag });
            }
            else
            {
                InitiateBenchmark();
            }
        }

        private void UpdateProgressBar(bool step)
        {
            if (step) BenchmarkProgressBar.PerformStep();
            LabelProgressPercentage.Text = String.Format(International.GetText("SubmitResultDialog_LabelProgressPercentageInProgress"),
                                           ((double)((double)BenchmarkProgressBar.Value / (double)BenchmarkProgressBar.Maximum) * 100).ToString("F2"), CurrentAlgoName);
        }
    }
}
