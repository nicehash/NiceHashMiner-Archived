using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace NiceHashMiner
{
    public partial class Form1 : Form
    {
        public static string[] MiningLocation = { "eu", "usa", "hk", "jp" };
        private static string VisitURL = "http://www.nicehash.com";
        public static NiceHashSMA[] NiceHashData = null;

        public static Miner[] Miners;

        private Timer MinerStatsCheck;
        private Timer UpdateCheck;
        private Timer SMACheck;
        private Timer BalanceCheck;
        private Timer SMAMinerCheck;
        private Timer BitcoinExchangeCheck;
        private Timer StartupTimer;
        private Timer IdleCheck;
        private Form3 LoadingScreen;
        private int LoadCounter = 0;
        private int TotalLoadSteps = 10;

        private Random R;

        private double BitcoinRate;

        private Form2 BenchmarkForm;


        public Form1(bool ss)
        {
            InitializeComponent();

            if (ss)
            {
                Form4 f4 = new Form4();
                f4.ShowDialog();
            }

            if (Config.ConfigData.DebugConsole)
                Helpers.AllocConsole();

            Helpers.ConsolePrint("Starting up");

            R = new Random((int)DateTime.Now.Ticks);

            Text += " v" + Application.ProductVersion;

            if (Config.ConfigData.Location >= 0 && Config.ConfigData.Location <= 3)
                comboBox1.SelectedIndex = Config.ConfigData.Location;
            else
                comboBox1.SelectedIndex = 0;

            textBox1.Text = Config.ConfigData.BitcoinAddress;
            textBox2.Text = Config.ConfigData.WorkerName;
        }


        private void AfterLoadComplete()
        {
            this.Enabled = true;

            if (Config.ConfigData.AutoStartMining)
            {
                button1_Click(null, null);
            }

            IdleCheck = new Timer();
            IdleCheck.Tick += IdleCheck_Tick;
            IdleCheck.Interval = 500;
            IdleCheck.Start();
        }


        private void IdleCheck_Tick(object sender, EventArgs e)
        {
            if (!Config.ConfigData.StartMiningWhenIdle) return;

            uint MSIdle = Helpers.GetIdleTime();

            if (MinerStatsCheck.Enabled)
            {
                if (MSIdle < (Config.ConfigData.MinIdleSeconds * 1000))
                {
                    button2_Click(null, null);
                    Helpers.ConsolePrint("resumed from idling");
                }
            }
            else
            {
                if (BenchmarkForm == null && (MSIdle > (Config.ConfigData.MinIdleSeconds * 1000)))
                {
                    Helpers.ConsolePrint("entering idling state");
                    button1_Click(null, null);
                }
            }
        }


        private void StartupTimer_Tick(object sender, EventArgs e)
        {
            StartupTimer.Stop();
            StartupTimer = null;

            // get all CPUs
            int CPUs = CPUID.GetPhysicalProcessorCount();

            // get all cores (including virtual - HT can benefit mining)
            int ThreadsPerCPU = CPUID.GetVirtualCoresCount() / CPUs;

            if (!Helpers.InternalCheckIsWow64() && !Config.ConfigData.AutoStartMining)
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

            Miners = new Miner[CPUs + 4];

            if (CPUs == 1)
                Miners[0] = new cpuminer(0, ThreadsPerCPU, 0);
            else
            {
                for (int i = 0; i < CPUs; i++)
                    Miners[i] = new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask));
            }

            LoadingScreen.LoadText.Text = "Querying NVIDIA5.x devices...";
            IncreaseLoadCounter();

            Miners[CPUs] = new ccminer_sp();

            LoadingScreen.LoadText.Text = "Querying NVIDIA3.x devices...";
            IncreaseLoadCounter();
            
            Miners[CPUs + 1] = new ccminer_tpruvot();

            LoadingScreen.LoadText.Text = "Querying NVIDIA2.1 devices...";
            IncreaseLoadCounter();

            Miners[CPUs + 2] = new ccminer_tpruvot_sm21();

            LoadingScreen.LoadText.Text = "Querying AMD OpenCL devices...";
            IncreaseLoadCounter();

            Miners[CPUs + 3] = new sgminer();

            LoadingScreen.LoadText.Text = "Saving config...";
            IncreaseLoadCounter();
            
            for (int i = 0; i < Miners.Length; i++)
            {
                if (Config.ConfigData.Groups.Length > i)
                {
                    Miners[i].ExtraLaunchParameters = Config.ConfigData.Groups[i].ExtraLaunchParameters;
                    Miners[i].UsePassword = Config.ConfigData.Groups[i].UsePassword;
                    if (Config.ConfigData.Groups[i].APIBindPort > 0)
                        Miners[i].APIPort = Config.ConfigData.Groups[i].APIBindPort;
                    for (int z = 0; z < Config.ConfigData.Groups[i].Algorithms.Length && z < Miners[i].SupportedAlgorithms.Length; z++)
                    {
                        Miners[i].SupportedAlgorithms[z].BenchmarkSpeed = Config.ConfigData.Groups[i].Algorithms[z].BenchmarkSpeed;
                        Miners[i].SupportedAlgorithms[z].ExtraLaunchParameters = Config.ConfigData.Groups[i].Algorithms[z].ExtraLaunchParameters;
                        Miners[i].SupportedAlgorithms[z].UsePassword = Config.ConfigData.Groups[i].Algorithms[z].UsePassword;
                        Miners[i].SupportedAlgorithms[z].Skip = Config.ConfigData.Groups[i].Algorithms[z].Skip;
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

            LoadingScreen.LoadText.Text = "Checking for latest version...";
            IncreaseLoadCounter();

            MinerStatsCheck = new Timer();
            MinerStatsCheck.Tick += MinerStatsCheck_Tick;
            MinerStatsCheck.Interval = Config.ConfigData.MinerAPIQueryInterval * 1000;

            SMAMinerCheck = new Timer();
            SMAMinerCheck.Tick += SMAMinerCheck_Tick;
            SMAMinerCheck.Interval = Config.ConfigData.SwitchMinSecondsFixed * 1000 + R.Next(Config.ConfigData.SwitchMinSecondsDynamic * 1000);

            UpdateCheck = new Timer();
            UpdateCheck.Tick += UpdateCheck_Tick;
            UpdateCheck.Interval = 1000 * 3600; // every 1 hour
            UpdateCheck.Start();
            UpdateCheck_Tick(null, null);

            LoadingScreen.LoadText.Text = "Getting NiceHash SMA information...";
            IncreaseLoadCounter();

            SMACheck = new Timer();
            SMACheck.Tick += SMACheck_Tick;
            SMACheck.Interval = 60 * 1000; // every 60 seconds
            SMACheck.Start();
            SMACheck_Tick(null, null);

            LoadingScreen.LoadText.Text = "Getting Bitcoin exchange rate...";
            IncreaseLoadCounter();

            BitcoinExchangeCheck = new Timer();
            BitcoinExchangeCheck.Tick += BitcoinExchangeCheck_Tick;
            BitcoinExchangeCheck.Interval = 1000 * 3601; // every 1 hour and 1 second
            BitcoinExchangeCheck.Start();
            BitcoinExchangeCheck_Tick(null, null);

            LoadingScreen.LoadText.Text = "Getting NiceHash balance...";
            IncreaseLoadCounter();

            BalanceCheck = new Timer();
            BalanceCheck.Tick += BalanceCheck_Tick;
            BalanceCheck.Interval = 61 * 1000; // every 61 seconds
            BalanceCheck.Start();
            BalanceCheck_Tick(null, null);

            IncreaseLoadCounter();
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            LoadingScreen = new Form3();
            LoadingScreen.Location = new Point(this.Location.X + (this.Width - LoadingScreen.Width) / 2, this.Location.Y + (this.Height - LoadingScreen.Height) / 2);
            LoadingScreen.Show();
            LoadingScreen.progressBar1.Maximum = TotalLoadSteps;
            LoadingScreen.progressBar1.Value = 0;
            LoadingScreen.LoadText.Text = "Querying CPU devices...";

            StartupTimer = new Timer();
            StartupTimer.Tick += StartupTimer_Tick;
            StartupTimer.Interval = 200;
            StartupTimer.Start();
        }


        private void IncreaseLoadCounter()
        {
            LoadCounter++;
            LoadingScreen.progressBar1.Value = LoadCounter;
            LoadingScreen.Update();
            if (LoadCounter >= TotalLoadSteps)
            {
                if (LoadingScreen != null)
                {
                    LoadingScreen.Close();
                    LoadingScreen = null;

                    AfterLoadComplete();
                }
            }
        }


        private void SMAMinerCheck_Tick(object sender, EventArgs e)
        {
            SMAMinerCheck.Interval = Config.ConfigData.SwitchMinSecondsFixed * 1000 + R.Next(Config.ConfigData.SwitchMinSecondsDynamic * 1000);

            string Worker = textBox2.Text.Trim();
            if (Worker.Length > 0)
                Worker = textBox1.Text.Trim() + "." + Worker;
            else
                Worker = textBox1.Text.Trim();

            foreach (Miner m in Miners)
            {
                if (m.EnabledDeviceCount() == 0) continue;

                int MaxProfitIndex = m.GetMaxProfitIndex(NiceHashData);

                if (m.CurrentAlgo != MaxProfitIndex)
                {
                    if (m.CurrentAlgo >= 0)
                    {
                        m.Stop();
                        // wait 0.5 seconds before going on
                        System.Threading.Thread.Sleep(Config.ConfigData.MinerRestartDelayMS);
                    }
                }

                m.Start(m.SupportedAlgorithms[MaxProfitIndex].NiceHashID,
                    "stratum+tcp://" + NiceHashData[m.SupportedAlgorithms[MaxProfitIndex].NiceHashID].name + "." + MiningLocation[comboBox1.SelectedIndex] + ".nicehash.com:" +
                    NiceHashData[m.SupportedAlgorithms[MaxProfitIndex].NiceHashID].port, Worker);

                m.CurrentAlgo = MaxProfitIndex;
                m.NumRetries = 7;
            }
        }


        private void MinerStatsCheck_Tick(object sender, EventArgs e)
        {
            string CPUAlgoName = "";
            double CPUTotalSpeed = 0;
            double CPUTotalRate = 0;

            // Reset all stats
            SetCPUStats("", 0, 0);
            SetNVIDIAtp21Stats("", 0, 0);
            SetNVIDIAspStats("", 0, 0);
            SetNVIDIAtpStats("", 0, 0);
            SetAMDOpenCLStats("", 0, 0);

            foreach (Miner m in Miners)
            {
                if (m.EnabledDeviceCount() == 0) continue;

                APIData AD = m.GetSummary();
                if (AD == null)
                {
                    // Make sure sgminer has time to start properly on slow CPU system
                    if (m is sgminer && m.NumRetries > 0)
                    {
                        m.NumRetries--;
                        continue;
                    }

                    // API is inaccessible, try to restart miner
                    m.NumRetries = 7;
                    m.Restart();
                    
                    continue;
                }

                if (NiceHashData != null)
                    m.CurrentRate = NiceHashData[AD.AlgorithmID].paying * AD.Speed * 0.000000001;

                if (m is cpuminer)
                {
                    CPUAlgoName = AD.AlgorithmName;
                    CPUTotalSpeed += AD.Speed;
                    CPUTotalRate += m.CurrentRate;
                }
                else if (m is ccminer_tpruvot_sm21)
                {
                    SetNVIDIAtp21Stats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                }
                else if (m is ccminer_tpruvot)
                {
                    SetNVIDIAtpStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                }
                else if (m is ccminer_sp)
                {
                    SetNVIDIAspStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                }
                else if (m is sgminer)
                {
                    SetAMDOpenCLStats(AD.AlgorithmName, AD.Speed, m.CurrentRate);
                }
            }

            if (CPUAlgoName.Length > 0)
            {
                SetCPUStats(CPUAlgoName, CPUTotalSpeed, CPUTotalRate);
            }
        }


        private void SetCPUStats(string aname, double speed, double paying)
        {
            label5.Text = (speed * 0.001).ToString("F3", CultureInfo.InvariantCulture) + " kH/s " + aname;
            label11.Text = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/Day";
            label16.Text = (paying * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture) + " $/Day";
            UpdateGlobalRate();
        }


        private void SetNVIDIAtp21Stats(string aname, double speed, double paying)
        {
            label22.Text = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s " + aname;
            label20.Text = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/Day";
            label19.Text = (paying * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture) + " $/Day";
            UpdateGlobalRate();
        }


        private void SetNVIDIAtpStats(string aname, double speed, double paying)
        {
            label8.Text = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s " + aname;
            label14.Text = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/Day";
            label18.Text = (paying * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture) + " $/Day";
            UpdateGlobalRate();
        }


        private void SetNVIDIAspStats(string aname, double speed, double paying)
        {
            label6.Text = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s " + aname;
            label12.Text = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/Day";
            label17.Text = (paying * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture) + " $/Day";
            UpdateGlobalRate();
        }


        private void SetAMDOpenCLStats(string aname, double speed, double paying)
        {
            label_AMDOpenCL_Mining_Speed.Text = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s " + aname;
            label_AMDOpenCL_Mining_BTC_Day_Value.Text = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/Day";
            label_AMDOpenCL_Mining_USD_Day_Value.Text = (paying * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture) + " $/Day";
            UpdateGlobalRate();
        }


        private void UpdateGlobalRate()
        {
            double TotalRate = 0;
            foreach (Miner m in Miners)
                TotalRate += m.CurrentRate;
            toolStripStatusLabel4.Text = (TotalRate).ToString("F8", CultureInfo.InvariantCulture);
            toolStripStatusLabel2.Text = (TotalRate * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture);
        }


        void BalanceCheck_Tick(object sender, EventArgs e)
        {
            if (VerifyMiningAddress())
            {
                Helpers.ConsolePrint("NICEHASH: balance get");
                double Balance = NiceHashStats.GetBalance(textBox1.Text.Trim(), textBox1.Text.Trim() + "." + textBox2.Text.Trim());
                if (Balance > 0)
                {
                    toolStripStatusLabel6.Text = Balance.ToString("F8", CultureInfo.InvariantCulture);
                    toolStripStatusLabel3.Text = (Balance * BitcoinRate).ToString("F2", CultureInfo.InvariantCulture);
                }
            }
        }


        void BitcoinExchangeCheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("COINBASE: bitcoin rate get");
            double BR = Bitcoin.GetUSDExchangeRate();
            if (BR > 0) BitcoinRate = BR;
            Helpers.ConsolePrint("Current Bitcoin rate: " + BitcoinRate.ToString("F2", CultureInfo.InvariantCulture));
        }


        void SMACheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("NICEHASH: sma get");
            NiceHashSMA[] t = NiceHashStats.GetAlgorithmRates(textBox1.Text.Trim() + "." + textBox2.Text.Trim());
            if (t != null) NiceHashData = t;
        }


        void UpdateCheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("NICEHASH: version get");
            string ver = NiceHashStats.GetVersion(textBox1.Text.Trim() + "." + textBox2.Text.Trim());

            if (ver == null) return;

            if (ver != Application.ProductVersion)
            {
                linkLabel2.Text = "IMPORTANT! New version v" + ver + " has\r\nbeen released. Click here to download it.";
                VisitURL = "https://github.com/nicehash/NiceHashMiner/releases/tag/" + ver;
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!VerifyMiningAddress()) return;

            int location = comboBox1.SelectedIndex;
            if (location > 1) location = 1;

            int algo = 0;
            // find first working algo
            foreach (Miner m in Miners)
            {
                if (m.CurrentAlgo >= 0)
                {
                    algo = m.SupportedAlgorithms[m.CurrentAlgo].NiceHashID;
                    break;
                }
            }

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

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            comboBox1.Enabled = false;
            button3.Enabled = false;
            button1.Enabled = false;
            button4.Enabled = false;
            listView1.Enabled = false;
            button2.Enabled = true;

            // todo: commit saving when values are changed
            Config.ConfigData.BitcoinAddress = textBox1.Text.Trim();
            Config.ConfigData.WorkerName = textBox2.Text.Trim();
            Config.ConfigData.Location = comboBox1.SelectedIndex;
            Config.Commit();

            SMAMinerCheck.Start();
            SMAMinerCheck_Tick(null, null);
            MinerStatsCheck.Start();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            MinerStatsCheck.Stop();
            SMAMinerCheck.Stop();

            foreach (Miner m in Miners)
            {
                m.Stop();
                m.CurrentAlgo = -1;
                m.CurrentRate = 0;
            }

            SetCPUStats("", 0, 0);
            SetNVIDIAtp21Stats("", 0, 0);
            SetNVIDIAspStats("", 0, 0);
            SetNVIDIAtpStats("", 0, 0);
            SetAMDOpenCLStats("", 0, 0);

            textBox1.Enabled = true;
            textBox2.Enabled = true;
            comboBox1.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = true;
            button4.Enabled = true;
            listView1.Enabled = true;
            button2.Enabled = false;

            UpdateGlobalRate();
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(VisitURL);
        }


        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ComputeDevice G = e.Item.Tag as ComputeDevice;
            G.Enabled = e.Item.Checked;
            if (LoadingScreen == null)
                Config.RebuildGroups();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Miner m in Miners)
                m.Stop();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Config.ConfigData.Location = comboBox1.SelectedIndex;

            BenchmarkForm = new Form2(false);
            BenchmarkForm.ShowDialog();
            BenchmarkForm = null;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Editing additional settings is for advanced users.\r\n\r\nIf " + ProductName +
                " crashes due to bad config value you can restore it by deleting 'config.json' file.\r\n\r\nContinue with editing settings?", 
                "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                return;

            Process PHandle = new Process();
            PHandle.StartInfo.FileName = Application.ExecutablePath;
            PHandle.StartInfo.Arguments = "-config";
            PHandle.Start();

            Close();
        }


        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nicehash/NiceHashMiner");
        }

        private void linkLabel_Choose_BTC_Wallet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nicehash.com/index.jsp?p=faq#faqs15");
        }

        private void toolStripStatusLabel10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nicehash.com/index.jsp?p=faq#faqs6");
        }

        private void toolStripStatusLabel10_MouseHover(object sender, EventArgs e)
        {
            statusStrip1.Cursor = Cursors.Hand;
        }

        private void toolStripStatusLabel10_MouseLeave(object sender, EventArgs e)
        {
            statusStrip1.Cursor = Cursors.Default;
        }
    }
}
