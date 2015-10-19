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

        private Random R;


        public Form1()
        {
            InitializeComponent();

            if (Config.ConfigData.DebugConsole)
                Helpers.AllocConsole();

            Helpers.ConsolePrint("Starting up");

            R = new Random((int)DateTime.Now.Ticks);

            Text += " v" + Application.ProductVersion;

            comboBox1.SelectedIndex = Config.ConfigData.Location;
            textBox1.Text = Config.ConfigData.BitcoinAddress;
            textBox2.Text = Config.ConfigData.WorkerName;

            // get all CPUs
            int CPUs = CPUID.GetPhysicalProcessorCount();

            // get all cores (including virtual - HT can benefit mining)
            int ThreadsPerCPU = CPUID.GetVirtualCoresCount() / CPUs;

            if (!Helpers.InternalCheckIsWow64())
            {
                MessageBox.Show("NiceHash Miner works only on 64 bit version of OS for CPU mining. CPU mining will be disabled.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            if (ThreadsPerCPU * CPUs > 64)
            {
                MessageBox.Show("NiceHash Miner does not support more than 64 virtual cores. CPU mining will be disabled.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            int ThreadsPerCPUMask = ThreadsPerCPU;
            ThreadsPerCPU -= Config.ConfigData.LessThreads;
            if (ThreadsPerCPU < 1)
            {
                MessageBox.Show("LessThreads greater than number of threads per CPU. CPU mining will be disabled.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            Miners = new Miner[CPUs]; // todo: add cc and sgminer

            if (CPUs == 1)
                Miners[0] = new cpuminer(0, ThreadsPerCPU, 0);
            else
            {
                for (int i = 0; i < CPUs; i++)
                    Miners[i] = new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask));
            }

            // todo: initialize ccminer
            
            // todo: initialize sgminer

            for (int i = 0; i < Miners.Length; i++)
            {
                if (Config.ConfigData.Groups.Length > i)
                {
                    for (int z = 0; z < Config.ConfigData.Groups[i].BenchmarkSpeeds.Length && z < Miners[i].SupportedAlgorithms.Length; z++)
                    {
                        Miners[i].SupportedAlgorithms[z].BenchmarkSpeed = Config.ConfigData.Groups[i].BenchmarkSpeeds[z];
                    }
                }
                for (int k = 0; k < Miners[i].CDevs.Count; k++)
                {
                    ComputeDevice D = Miners[i].CDevs[k];
                    if (Config.ConfigData.Groups.Length > i)
                    {
                        for (int z = 0; z < Config.ConfigData.Groups[i].DisabledDevices.Length; z++)
                        {
                            if (Config.ConfigData.Groups[i].DisabledDevices[z] == k)
                            {
                                D.Enabled = false;
                                break;
                            }
                        }
                    }
                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Add(D.Vendor);
                    lvi.SubItems.Add(D.Name);
                    lvi.Checked = D.Enabled;
                    lvi.Tag = D;
                    listView1.Items.Add(lvi);
                }
            }

            Config.RebuildGroups();

            MinerStatsCheck = new Timer();
            MinerStatsCheck.Tick += MinerStatsCheck_Tick;
            MinerStatsCheck.Interval = 5000; // every 5 seconds

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

                Helpers.ConsolePrint("CPU " + NiceHashData[Miners[0].SupportedAlgorithms[i].NiceHashID].name + " paying " + Miners[0].SupportedAlgorithms[i].CurrentProfit.ToString("F8") + " BTC/Day");

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
            Helpers.ConsolePrint("NICEHASH: balance get");
            double Balance = NiceHashStats.GetBalance(textBox1.Text.Trim());
            if (Balance > 0) toolStripStatusLabel6.Text = Balance.ToString("F8");
        }


        void SMACheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("NICEHASH: sma get");
            NiceHashSMA[] t = NiceHashStats.GetAlgorithmRates();
            if (t != null) NiceHashData = t;
        }


        void UpdateCheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("NICEHASH: version get");
            string ver = NiceHashStats.GetVersion();
            if (ver == null) return;

            if (ver != Application.ProductVersion)
            {
                linkLabel2.Text = "IMPORTANT! New version v" + ver + " has\r\nbeen released. Click here to download it.";
                VisitURL = "https://github.com/nicehash/NiceHashMiner/releases/tag/" + ver;
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
                if (AD == null)
                {
                    // API is inaccessible, try to restart miner
                    m.Restart();
                    continue;
                }

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

            int algo = 0;
            if (CurrentCPUAlgo >= 0) algo = Miners[0].SupportedAlgorithms[CurrentCPUAlgo].NiceHashID;

            System.Diagnostics.Process.Start("http://www.nicehash.com/index.jsp?p=miners&addr=" + textBox1.Text.Trim() + "&l=" + location + "&a=" + algo);
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
            MinerStatsCheck.Start();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            MinerStatsCheck.Stop();
            SMACPUCheck.Stop();

            foreach (Miner m in Miners)
                m.Stop();

            SetCPUStats("", 0, 0);

            CurrentCPUAlgo = -1;
            CurrentNVIDIAAlgo = -1;
            CurrentAMDAlgo = -1;
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(VisitURL);
        }


        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ComputeDevice G = e.Item.Tag as ComputeDevice;
            G.Enabled = e.Item.Checked;
            Config.RebuildGroups();
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
