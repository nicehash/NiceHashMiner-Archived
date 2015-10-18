using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace NiceHashMiner
{
    public partial class Form1 : Form
    {
        private static string[] MiningLocation = { "eu", "usa", "hk", "jp" };
        private static string VisitURL = "http://www.nicehash.com";
        private static NiceHashSMA[] NiceHashData = null;
        private static int SwitchTimeFixed = 3 * 60 * 1000;
        private static int SwitchTimeDynamic = 3 * 60 * 1000;

        public static Miner[] Miners;

        private Timer MinerStatsCheck;
        private Timer UpdateCheck;
        private Timer SMACheck;
        private Timer BalanceCheck;

        private Timer SMACPUCheck;

        private double CPURate = 0;
        private double NVIDIARate = 0;
        private double AMDRate = 0;

        private int CurrentCPUAlgo = -1;
        private int CurrentNVIDIAAlgo = -1;
        private int CurrentAMDAlgo = -1;

        private Random R = new Random((int)DateTime.Now.Ticks);

        public Form1()
        {
            InitializeComponent();

            Text += " v" + Application.ProductVersion;

            comboBox1.SelectedIndex = Config.ConfigData.Location;
            textBox1.Text = Config.ConfigData.BitcoinAddress;
            textBox2.Text = Config.ConfigData.WorkerName;

            if (!Helpers.InternalCheckIsWow64())
            {
                MessageBox.Show("NiceHash Miner works only on 64 bit version of OS!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // get all CPUs
            int CPUs = CPUID.GetPhysicalProcessorCount();

            Miners = new Miner[CPUs]; // todo: add cc and sgminer

            int ThreadsPerCPU = CPUID.GetVirtualCoresCount() / CPUs;

            if (ThreadsPerCPU * CPUs > 64)
            {
                MessageBox.Show("NiceHash Miner does not support more than 64 virtual cores!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (CPUs == 1)
                    Miners[0] = new cpuminer(0, ThreadsPerCPU, 0);
                else
                {
                    for (int i = 0; i < CPUs; i++)
                        Miners[i] = new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPU));
                }
            }

            foreach (Miner m in Miners)
            {
                foreach (ComputeDevice D in m.CDevs)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Add(D.Vendor);
                    lvi.SubItems.Add(D.Name);
                    lvi.Checked = D.Enabled;
                    lvi.Tag = D;
                    listView1.Items.Add(lvi);
                }
            }

            MinerStatsCheck = new Timer();
            MinerStatsCheck.Tick += MinerStatsCheck_Tick;
            MinerStatsCheck.Interval = 5000; // every 5 seconds
            MinerStatsCheck.Start();

            UpdateCheck = new Timer();
            UpdateCheck.Tick += UpdateCheck_Tick;
            UpdateCheck.Interval = 1000 * 3600; // every 1 hour
            UpdateCheck.Start();
            UpdateCheck_Tick(null, null);

            SMACheck = new Timer();
            SMACheck.Tick += SMACheck_Tick;
            SMACheck.Interval = 60 * 1000; // every 60 seconds
            SMACheck.Start();
            SMACheck_Tick(null, null);

            BalanceCheck = new Timer();
            BalanceCheck.Tick += BalanceCheck_Tick;
            BalanceCheck.Interval = 61 * 1000; // every 61 seconds
            BalanceCheck.Start();
            BalanceCheck_Tick(null, null);

            SMACPUCheck = new Timer();
            SMACPUCheck.Tick += SMACPUCheck_Tick;
            SMACPUCheck.Interval = SwitchTimeFixed + R.Next(SwitchTimeDynamic);
        }


        void SMACPUCheck_Tick(object sender, EventArgs e)
        {
            SMACPUCheck.Interval = SwitchTimeFixed + R.Next(SwitchTimeDynamic);

            if (Miners.Length == 0 || !(Miners[0] is cpuminer))
                return;

            // determine current best CPU algorithm
            // assuming that all CPUs have equal power
            double MaxProfit = 0;
            int MaxProfitIndex = 0;
            for (int i = 0; i < Miners[0].SupportedAlgorithms.Length; i++)
            {
                Miners[0].SupportedAlgorithms[i].CurrentProfit = Miners[0].SupportedAlgorithms[i].BenchmarkSpeed * 
                    NiceHashData[Miners[0].SupportedAlgorithms[i].NiceHashID].paying * 0.000000001;

                Debug.Print("CPU " + NiceHashData[Miners[0].SupportedAlgorithms[i].NiceHashID].name + " paying " + Miners[0].SupportedAlgorithms[i].CurrentProfit.ToString("F8") + " BTC/Day");

                if (Miners[0].SupportedAlgorithms[i].CurrentProfit > MaxProfit)
                {
                    MaxProfit = Miners[0].SupportedAlgorithms[i].CurrentProfit;
                    MaxProfitIndex = i;
                }
            }

            if (CurrentCPUAlgo != MaxProfitIndex)
            {
                if (!VerifyMiningAddress()) return;

                if (CurrentCPUAlgo >= 0)
                {
                    foreach (Miner m in Miners)
                        if (m is cpuminer) m.Stop();
                }

                string Worker = textBox2.Text.Trim();
                if (Worker.Length > 0)
                    Worker = textBox1.Text.Trim() + "." + Worker;
                else
                    Worker = textBox1.Text.Trim();

                foreach (Miner m in Miners)
                    if (m is cpuminer)
                        m.Start(m.SupportedAlgorithms[MaxProfitIndex].NiceHashID,
                            "stratum+tcp://" + NiceHashData[m.SupportedAlgorithms[MaxProfitIndex].NiceHashID].name + "." + MiningLocation[comboBox1.SelectedIndex] + ".nicehash.com:" + 
                            NiceHashData[m.SupportedAlgorithms[MaxProfitIndex].NiceHashID].port, Worker);

                CurrentCPUAlgo = MaxProfitIndex;
            }
        }


        void BalanceCheck_Tick(object sender, EventArgs e)
        {
            if (!VerifyMiningAddress()) return;
            double Balance = NiceHashStats.GetBalance(textBox1.Text.Trim());
            toolStripStatusLabel6.Text = Balance.ToString("F8");
        }


        void SMACheck_Tick(object sender, EventArgs e)
        {
            NiceHashData = NiceHashStats.GetAlgorithmRates();
        }


        void UpdateCheck_Tick(object sender, EventArgs e)
        {
            string ver = NiceHashStats.GetVersion();
            if (ver == null) return;

            if (ver != Application.ProductVersion)
            {
                linkLabel2.Text = "IMPORTANT! New version v" + ver + " has\r\nbeen released. Click here to download it.";
                VisitURL = "https://github.com/nicehash/NiceHashMiner/releases/tag/" + ver;
                UpdateCheck.Stop();
            }
        }


        private void MinerStatsCheck_Tick(object sender, EventArgs e)
        {
            APIData CPUdata = new APIData();
            CPUdata.AlgorithmName = null;
            CPUdata.Speed = 0;

            foreach (Miner m in Miners)
            {
                APIData AD = m.GetSummary();
                if (AD == null) continue;

                if (m is cpuminer)
                {
                    CPUdata.AlgorithmID = AD.AlgorithmID;
                    CPUdata.AlgorithmName = AD.AlgorithmName;
                    CPUdata.Speed += AD.Speed;
                }
            }

            if (CPUdata.AlgorithmName != null && NiceHashData != null)
            {
                double Paying = NiceHashData[CPUdata.AlgorithmID].paying * CPUdata.Speed * 0.000000001;
                SetCPUStats(CPUdata.AlgorithmName, CPUdata.Speed, Paying);
            }
        }


        private void SetCPUStats(string aname, double speed, double paying)
        {
            label5.Text = (speed * 0.001).ToString("F2") + " kH/s " + aname;
            label11.Text = paying.ToString("F8") + " BTC/Day";
            CPURate = paying;
            UpdateGlobalRate();
        }


        private void UpdateGlobalRate()
        {
            toolStripStatusLabel4.Text = (CPURate + NVIDIARate + AMDRate).ToString("F8");
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!VerifyMiningAddress()) return;

            int location = comboBox1.SelectedIndex;
            if (location > 1) location = 1;

            System.Diagnostics.Process.Start("http://www.nicehash.com/index.jsp?p=miners&addr=" + textBox1.Text.Trim());
        }


        private bool VerifyMiningAddress()
        {
            if (!BitcoinAddress.ValidateBitcoinAddress(textBox1.Text.Trim()))
            {
                MessageBox.Show("Invalid Bitcoin address!\n\nPlease, enter valid Bitcoin address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBox1.Focus();
                return false;
            }

            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!VerifyMiningAddress()) return;

            if (NiceHashData == null)
            {
                MessageBox.Show("Unable to get NiceHash profitability data. If you are connected to internet, try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // todo: commit saving when values are changed
            Config.ConfigData.BitcoinAddress = textBox1.Text.Trim();
            Config.ConfigData.WorkerName = textBox2.Text.Trim();
            Config.ConfigData.Location = comboBox1.SelectedIndex;
            Config.Commit();

            SMACPUCheck.Start();
            SMACPUCheck_Tick(null, null);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            SMACPUCheck.Stop();

            foreach (Miner m in Miners)
                m.Stop();

            SetCPUStats("", 0, 0);

            CurrentCPUAlgo = -1;
            CurrentNVIDIAAlgo = -1;
            CurrentAMDAlgo = -1;
        }


        //delegate void ResetButtonTextCallback();

        //private void ResetButtonText()
        //{
        //    if (this.button1.InvokeRequired)
        //    {
        //        ResetButtonTextCallback d = new ResetButtonTextCallback(ResetButtonText);
        //        this.Invoke(d, new object[] { });
        //    }
        //    else
        //    {
        //        this.button1.Text = "Start";
        //    }
        //}

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(VisitURL);
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ComputeDevice G = e.Item.Tag as ComputeDevice;
            G.Enabled = e.Item.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Miner m in Miners)
                m.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(false);
            f2.ShowDialog();
        }
    }
}
