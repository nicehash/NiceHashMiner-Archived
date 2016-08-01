using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Management;

namespace NiceHashMiner
{
    public partial class Form_Main : Form, Form_Loading.AfterInitializationCaller
    {
        private static string VisitURL = "http://www.nicehash.com";

        private Timer MinerStatsCheck;
        private Timer UpdateCheck;
        private Timer SMACheck;
        private Timer BalanceCheck;
        private Timer SMAMinerCheck;
        private Timer BitcoinExchangeCheck;
        private Timer StartupTimer;
        private Timer IdleCheck;
        private int CPUs;
        private bool ShowWarningNiceHashData;
        private bool DemoMode;

        private Random R;

        private Form_Loading LoadingScreen;
        private Form_Benchmark BenchmarkForm;


        public Form_Main(bool showConfigSettingsDialog)
        {
            InitializeComponent();

            MessageBoxManager.Yes = International.GetText("Global_Yes");
            MessageBoxManager.No = International.GetText("Global_No");
            MessageBoxManager.OK = International.GetText("Global_OK");
            MessageBoxManager.Register();

            labelServiceLocation.Text = International.GetText("Service_Location") + ":";
            labelBitcoinAddress.Text = International.GetText("BitcoinAddress") + ":";
            labelWorkerName.Text = International.GetText("WorkerName") + ":";

            linkLabelVisitUs.Text = International.GetText("form1_visit_us");
            linkLabelCheckStats.Text = International.GetText("form1_check_stats");
            linkLabelChooseBTCWallet.Text = International.GetText("form1_choose_bitcoin_wallet");

            label_RateCPU.Text = International.GetText("Rate") + ":";
            label_RateNVIDIA5X.Text = International.GetText("Rate") + ":";
            label_RateNVIDIA3X.Text = International.GetText("Rate") + ":";
            label_RateNVIDIA2X.Text = International.GetText("Rate") + ":";
            label_RateAMD.Text = International.GetText("Rate") + ":";

            label_RateCPUBTC.Text = "0.00000000 BTC/" + International.GetText("Day");
            label_RateNVIDIA5XBTC.Text = "0.00000000 BTC/" + International.GetText("Day");
            label_RateNVIDIA3XBTC.Text = "0.00000000 BTC/" + International.GetText("Day");
            label_RateNVIDIA2XBTC.Text = "0.00000000 BTC/" + International.GetText("Day");
            label_RateAMDBTC.Text = "0.00000000 BTC/" + International.GetText("Day");



            label_RateCPUDollar.Text = String.Format("0.00 {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            label_RateNVIDIA5XDollar.Text = String.Format("0.00 {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            label_RateNVIDIA3XDollar.Text = String.Format("0.00 {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            label_RateNVIDIA2XDollar.Text = String.Format("0.00 {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            label_RateAMDDollar.Text = String.Format("0.00 {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");

            toolStripStatusLabelGlobalRateText.Text = International.GetText("form1_global_rate") + ":";
            toolStripStatusLabelBTCDayText.Text = "BTC/" + International.GetText("Day");
            toolStripStatusLabelBalanceText.Text = (Config.ConfigData.DisplayCurrency + "/") + International.GetText("Day") + "     " + International.GetText("form1_balance") + ":";

            listViewDevices.Columns[0].Text = International.GetText("ListView_Enabled");
            listViewDevices.Columns[1].Text = International.GetText("ListView_Group");
            listViewDevices.Columns[2].Text = International.GetText("ListView_Device");

            buttonBenchmark.Text = International.GetText("form1_benchmark");
            buttonSettings.Text = International.GetText("form1_settings");
            buttonStartMining.Text = International.GetText("form1_start");
            buttonStopMining.Text = International.GetText("form1_stop");

            if (showConfigSettingsDialog)
            {
                Form_ConfigSettings f4 = new Form_ConfigSettings();
                f4.ShowDialog();
            }

            // Log the computer's amount of Total RAM and Page File Size
            ManagementObjectCollection moc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem").Get();
            foreach (ManagementObject mo in moc)
            {
                long TotalRam = long.Parse(mo["TotalVisibleMemorySize"].ToString()) / 1024;
                long PageFileSize = (long.Parse(mo["TotalVirtualMemorySize"].ToString()) / 1024) - TotalRam;
                Helpers.ConsolePrint("NICEHASH", "Total RAM: "      + TotalRam     + "MB");
                Helpers.ConsolePrint("NICEHASH", "Page File Size: " + PageFileSize + "MB");
            }

            R = new Random((int)DateTime.Now.Ticks);

            Text += " v" + Application.ProductVersion;

            if (Config.ConfigData.ServiceLocation >= 0 && Config.ConfigData.ServiceLocation < Globals.MiningLocation.Length)
                comboBoxLocation.SelectedIndex = Config.ConfigData.ServiceLocation;
            else
                comboBoxLocation.SelectedIndex = 0;

            textBoxBTCAddress.Text = Config.ConfigData.BitcoinAddress;
            textBoxWorkerName.Text = Config.ConfigData.WorkerName;
            ShowWarningNiceHashData = true;
            DemoMode = false;

            toolStripStatusLabelBalanceDollarValue.Text = "(" + Config.ConfigData.DisplayCurrency + ")";
        }


        public void AfterLoadComplete()
        {
            // TODO dispose object, check LoadingScreen => this is tightly coupled with the
            // check box functionality for rebuilding Groups. Find a way around and fix.
            LoadingScreen = null;

            this.Enabled = true;

            if (Config.ConfigData.AutoStartMining)
            {
                buttonStartMining_Click(null, null);
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
                    buttonStopMining_Click(null, null);
                    Helpers.ConsolePrint("NICEHASH", "Resumed from idling");
                }
            }
            else
            {
                if (BenchmarkForm == null && (MSIdle > (Config.ConfigData.MinIdleSeconds * 1000)))
                {
                    Helpers.ConsolePrint("NICEHASH", "Entering idling state");
                    buttonStartMining_Click(null, null);
                }
            }
        }


        private void StartupTimer_Tick(object sender, EventArgs e)
        {
            StartupTimer.Stop();
            StartupTimer = null;

            // get all CPUs
            CPUs = CPUID.GetPhysicalProcessorCount();

            // get all cores (including virtual - HT can benefit mining)
            int ThreadsPerCPU = CPUID.GetVirtualCoresCount() / CPUs;

            if (!Helpers.InternalCheckIsWow64() && !Config.ConfigData.AutoStartMining)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMining64bitMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            if (ThreadsPerCPU * CPUs > 64)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMining64CoresMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            int ThreadsPerCPUMask = ThreadsPerCPU;
            ThreadsPerCPU -= Config.ConfigData.LessThreads;
            if (ThreadsPerCPU < 1)
            {
                MessageBox.Show(International.GetText("form1_msgbox_CPUMiningLessThreadMsg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CPUs = 0;
            }

            Globals.Miners = new Miner[CPUs + 4];

            if (CPUs == 1)
                Globals.Miners[0] = new cpuminer(0, ThreadsPerCPU, 0);
            else
            {
                for (int i = 0; i < CPUs; i++)
                    Globals.Miners[i] = new cpuminer(i, ThreadsPerCPU, CPUID.CreateAffinityMask(i, ThreadsPerCPUMask));
            }

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_NVIDIA5X"));

            Globals.Miners[CPUs] = new ccminer_sp();

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_NVIDIA3X"));

            Globals.Miners[CPUs + 1] = new ccminer_tpruvot();

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_NVIDIA2X"));

            Globals.Miners[CPUs + 2] = new ccminer_tpruvot_sm21();

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_AMD"));

            Globals.Miners[CPUs + 3] = new sgminer();

            // Auto uncheck CPU if any GPU is found
            for (int i = 0; i < CPUs; i++)
            {
                try
                {
                    if ((Globals.Miners[CPUs + 0].CDevs.Count > 0 || Globals.Miners[CPUs + 1].CDevs.Count > 0 || Globals.Miners[CPUs + 2].CDevs.Count > 0 || Globals.Miners[CPUs + 3].CDevs.Count > 0) && i < CPUs)
                    {
                        Globals.Miners[i].CDevs[0].Enabled = false;
                    }
                }
                catch { }
            }

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_SaveConfig"));
            
            for (int i = 0; i < Globals.Miners.Length; i++)
            {
                if (Config.ConfigData.Groups.Length > i)
                {
                    Globals.Miners[i].ExtraLaunchParameters = Config.ConfigData.Groups[i].ExtraLaunchParameters;
                    Globals.Miners[i].UsePassword = Config.ConfigData.Groups[i].UsePassword;
                    Globals.Miners[i].MinimumProfit = Config.ConfigData.Groups[i].MinimumProfit;
                    Globals.Miners[i].DaggerHashimotoGenerateDevice = Config.ConfigData.Groups[i].DaggerHashimotoGenerateDevice;
                    if (Config.ConfigData.Groups[i].APIBindPort > 0)
                        Globals.Miners[i].APIPort = Config.ConfigData.Groups[i].APIBindPort;
                    for (int z = 0; z < Config.ConfigData.Groups[i].Algorithms.Length && z < Globals.Miners[i].SupportedAlgorithms.Length; z++)
                    {
                        Globals.Miners[i].SupportedAlgorithms[z].BenchmarkSpeed = Config.ConfigData.Groups[i].Algorithms[z].BenchmarkSpeed;
                        Globals.Miners[i].SupportedAlgorithms[z].ExtraLaunchParameters = Config.ConfigData.Groups[i].Algorithms[z].ExtraLaunchParameters;
                        Globals.Miners[i].SupportedAlgorithms[z].UsePassword = Config.ConfigData.Groups[i].Algorithms[z].UsePassword;
                        Globals.Miners[i].SupportedAlgorithms[z].Skip = Config.ConfigData.Groups[i].Algorithms[z].Skip;

                        Globals.Miners[i].SupportedAlgorithms[z].DisabledDevice = new bool[Globals.Miners[i].CDevs.Count];
                        if (Config.ConfigData.Groups[i].Algorithms[z].DisabledDevices != null)
                        {
                            if (Config.ConfigData.Groups[i].Algorithms[z].DisabledDevices.Length < Globals.Miners[i].CDevs.Count)
                            {
                                for (int j = 0; j < Config.ConfigData.Groups[i].Algorithms[z].DisabledDevices.Length; j++)
                                    Globals.Miners[i].SupportedAlgorithms[z].DisabledDevice[j] = Config.ConfigData.Groups[i].Algorithms[z].DisabledDevices[j];
                                for (int j = Config.ConfigData.Groups[i].Algorithms[z].DisabledDevices.Length; j < Globals.Miners[i].CDevs.Count; j++)
                                    Globals.Miners[i].SupportedAlgorithms[z].DisabledDevice[j] = false;
                            }
                            else
                            {
                                for (int j = 0; j < Globals.Miners[i].CDevs.Count; j++)
                                    Globals.Miners[i].SupportedAlgorithms[z].DisabledDevice[j] = Config.ConfigData.Groups[i].Algorithms[z].DisabledDevices[j];
                            }
                        }
                        else
                        {
                            Globals.Miners[i].GetDisabledDevicePerAlgo();
                        }
                    }
                }
                else
                {
                    Globals.Miners[i].GetDisabledDevicePerAlgo();
                }
                for (int k = 0; k < Globals.Miners[i].CDevs.Count; k++)
                {
                    ComputeDevice D = Globals.Miners[i].CDevs[k];
                    if (Config.ConfigData.Groups.Length > i)
                    {
                        D.Enabled = true;
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
                    listViewDevices.Items.Add(lvi);
                }
            }

            Config.RebuildGroups();


            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_CheckLatestVersion"));

            MinerStatsCheck = new Timer();
            MinerStatsCheck.Tick += MinerStatsCheck_Tick;
            MinerStatsCheck.Interval = Config.ConfigData.MinerAPIQueryInterval * 1000;

            SMAMinerCheck = new Timer();
            SMAMinerCheck.Tick += SMAMinerCheck_Tick;
            SMAMinerCheck.Interval = Config.ConfigData.SwitchMinSecondsFixed * 1000 + R.Next(Config.ConfigData.SwitchMinSecondsDynamic * 1000);
            if (Globals.Miners[CPUs + 3].CDevs.Count > 0) SMAMinerCheck.Interval = (Config.ConfigData.SwitchMinSecondsAMD + Config.ConfigData.SwitchMinSecondsFixed) * 1000 + R.Next(Config.ConfigData.SwitchMinSecondsDynamic * 1000);
            
            UpdateCheck = new Timer();
            UpdateCheck.Tick += UpdateCheck_Tick;
            UpdateCheck.Interval = 1000 * 3600; // every 1 hour
            UpdateCheck.Start();
            UpdateCheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_GetNiceHashSMA"));

            SMACheck = new Timer();
            SMACheck.Tick += SMACheck_Tick;
            SMACheck.Interval = 60 * 1000; // every 60 seconds
            SMACheck.Start();
            SMACheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_GetBTCRate"));

            BitcoinExchangeCheck = new Timer();
            BitcoinExchangeCheck.Tick += BitcoinExchangeCheck_Tick;
            BitcoinExchangeCheck.Interval = 1000 * 3601; // every 1 hour and 1 second
            BitcoinExchangeCheck.Start();
            BitcoinExchangeCheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_GetNiceHashBalance"));

            BalanceCheck = new Timer();
            BalanceCheck.Tick += BalanceCheck_Tick;
            BalanceCheck.Interval = 61 * 1000; // every 61 seconds
            BalanceCheck.Start();
            BalanceCheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_SetEnvironmentVariable"));

            SetEnvironmentVariables();

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("form1_loadtext_SetWindowsErrorReporting"));
            
            Helpers.DisableWindowsErrorReporting(Config.ConfigData.DisableWindowsErrorReporting);

            LoadingScreen.IncreaseLoadCounter();
            if (Config.ConfigData.NVIDIAP0State)
            {
                LoadingScreen.SetInfoMsg(International.GetText("form1_loadtext_NVIDIAP0State"));
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = "nvidiasetp0state.exe";
                    psi.Verb = "runas";
                    psi.UseShellExecute = true;
                    psi.CreateNoWindow = true;
                    Process p = Process.Start(psi);
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                        Helpers.ConsolePrint("NICEHASH", "nvidiasetp0state returned error code: " + p.ExitCode.ToString());
                    else
                        Helpers.ConsolePrint("NICEHASH", "nvidiasetp0state all OK");
                }
                catch (Exception ex)
                {
                    Helpers.ConsolePrint("NICEHASH", "nvidiasetp0state error: " + ex.Message);
                }
            }

            LoadingScreen.IncreaseLoadCounter();
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            LoadingScreen = new Form_Loading(this, International.GetText("form1_loadtext_CPU"));
            LoadingScreen.Location = new Point(this.Location.X + (this.Width - LoadingScreen.Width) / 2, this.Location.Y + (this.Height - LoadingScreen.Height) / 2);
            LoadingScreen.Show();

            StartupTimer = new Timer();
            StartupTimer.Tick += StartupTimer_Tick;
            StartupTimer.Interval = 200;
            StartupTimer.Start();
        }


        


        private void SMAMinerCheck_Tick(object sender, EventArgs e)
        {
            SMAMinerCheck.Interval = Config.ConfigData.SwitchMinSecondsFixed * 1000 + R.Next(Config.ConfigData.SwitchMinSecondsDynamic * 1000);
            if (Globals.Miners[CPUs + 3].CDevs.Count > 0) SMAMinerCheck.Interval = (Config.ConfigData.SwitchMinSecondsAMD + Config.ConfigData.SwitchMinSecondsFixed) * 1000 + R.Next(Config.ConfigData.SwitchMinSecondsDynamic * 1000);

            string Worker = textBoxWorkerName.Text.Trim();
            if (Worker.Length > 0)
                Worker = textBoxBTCAddress.Text.Trim() + "." + Worker;
            else
                Worker = textBoxBTCAddress.Text.Trim();

            foreach (Miner m in Globals.Miners)
            {
                if (m.EnabledDeviceCount() == 0) continue;

                int MaxProfitIndex = m.GetMaxProfitIndex(Globals.NiceHashData);

                if (m.NotProfitable || MaxProfitIndex == -1)
                {
                    Helpers.ConsolePrint(m.MinerDeviceName, "Miner is not profitable.. STOPPING..");
                    m.Stop(false);
                    continue;
                }
                
                if (m.CurrentAlgo != MaxProfitIndex)
                {
                    Helpers.ConsolePrint(m.MinerDeviceName, "Switching to most profitable algorithm: " + m.SupportedAlgorithms[MaxProfitIndex].NiceHashName);

                    MinerStatsCheck.Stop();
                    if (m.CurrentAlgo >= 0)
                    {
                        m.Stop(true);
                        // wait 0.5 seconds before going on
                        System.Threading.Thread.Sleep(Config.ConfigData.MinerRestartDelayMS);
                    }
                    m.CurrentAlgo = MaxProfitIndex;

                    m.Start(m.SupportedAlgorithms[MaxProfitIndex].NiceHashID,
                        "stratum+tcp://" + Globals.NiceHashData[m.SupportedAlgorithms[MaxProfitIndex].NiceHashID].name + "." + Globals.MiningLocation[comboBoxLocation.SelectedIndex] + ".nicehash.com:" + Globals.NiceHashData[m.SupportedAlgorithms[MaxProfitIndex].NiceHashID].port, Worker);
                    MinerStatsCheck.Start();
                }
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

            foreach (Miner m in Globals.Miners)
            {
                if (!m.IsRunning) continue;

                if (m is cpuminer && m.AlgoNameIs("hodl"))
                {
                    string pname = m.Path.Split('\\')[2];
                    pname = pname.Substring(0, pname.Length - 4);

                    Process[] processes = Process.GetProcessesByName(pname);

                    if (processes.Length < CPUID.GetPhysicalProcessorCount())
                        m.Restart();

                    int algoIndex = m.GetAlgoIndex("hodl");
                    CPUAlgoName = "hodl";
                    CPUTotalSpeed = m.SupportedAlgorithms[algoIndex].BenchmarkSpeed;
                    CPUTotalRate = Globals.NiceHashData[m.SupportedAlgorithms[algoIndex].NiceHashID].paying * CPUTotalSpeed * 0.000000001;

                    continue;
                }

                APIData AD = m.GetSummary();
                if (AD == null)
                {
                    Helpers.ConsolePrint(m.MinerDeviceName, "GetSummary returned null..");

                    // Make sure sgminer has time to start
                    // properly on slow CPU system
                    if (m.StartingUpDelay && m.NumRetries > 0)
                    {
                        m.NumRetries--;
                        if (m.NumRetries == 0) m.StartingUpDelay = false;
                        Helpers.ConsolePrint(m.MinerDeviceName, "NumRetries: " + m.NumRetries);
                        continue;
                    }

                    // API is inaccessible, try to restart miner
                    m.Restart();

                    continue;
                }
                else
                    m.StartingUpDelay = false;

                if (Globals.NiceHashData != null)
                    m.CurrentRate = Globals.NiceHashData[AD.AlgorithmID].paying * AD.Speed * 0.000000001;
                else
                    m.CurrentRate = 0;

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

            if (CPUAlgoName != null && CPUAlgoName.Length > 0)
            {
                SetCPUStats(CPUAlgoName, CPUTotalSpeed, CPUTotalRate);
            }
        }


        private void SetCPUStats(string aname, double speed, double paying)
        {
            labelCPU_Mining_Speed.Text = FormatSpeedOutput(speed) + aname;
            if (aname.Equals("hodl")) labelCPU_Mining_Speed.Text += "**";
            label_RateCPUBTC.Text = FormatPayingOutput(paying);
            label_RateCPUDollar.Text = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(paying *Globals.BitcoinRate).ToString("F2", CultureInfo.InvariantCulture)
                + String.Format(" {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            UpdateGlobalRate();
        }


        private void SetNVIDIAtp21Stats(string aname, double speed, double paying)
        {
            labelNVIDIA2X_Mining_Speed.Text = FormatSpeedOutput(speed) + aname;
            label_RateNVIDIA2XBTC.Text = FormatPayingOutput(paying);
            label_RateNVIDIA2XDollar.Text = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(paying *Globals.BitcoinRate).ToString("F2", CultureInfo.InvariantCulture)
                + String.Format(" {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            UpdateGlobalRate();
        }


        private void SetNVIDIAtpStats(string aname, double speed, double paying)
        {
            labelNVIDIA3X_Mining_Speed.Text = FormatSpeedOutput(speed) + aname;
            label_RateNVIDIA3XBTC.Text = FormatPayingOutput(paying);
            label_RateNVIDIA3XDollar.Text = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(paying *Globals.BitcoinRate).ToString("F2", CultureInfo.InvariantCulture)
                + String.Format(" {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            UpdateGlobalRate();
        }


        private void SetNVIDIAspStats(string aname, double speed, double paying)
        {
            labelNVIDIA5X_Mining_Speed.Text = FormatSpeedOutput(speed) + aname;
            label_RateNVIDIA5XBTC.Text = FormatPayingOutput(paying);
            label_RateNVIDIA5XDollar.Text = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(paying *Globals.BitcoinRate).ToString("F2", CultureInfo.InvariantCulture) 
                + String.Format(" {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            UpdateGlobalRate();
        }


        private void SetAMDOpenCLStats(string aname, double speed, double paying)
        {
            labelAMDOpenCL_Mining_Speed.Text = FormatSpeedOutput(speed) + aname;
            label_RateAMDBTC.Text = FormatPayingOutput(paying);
            label_RateAMDDollar.Text = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(paying *Globals.BitcoinRate).ToString("F2", CultureInfo.InvariantCulture)
                + String.Format(" {0}/", Config.ConfigData.DisplayCurrency) + International.GetText("Day");
            UpdateGlobalRate();
        }


        private void UpdateGlobalRate()
        {
            double TotalRate = 0;
            foreach (Miner m in Globals.Miners)
                TotalRate += m.CurrentRate;

            if (Config.ConfigData.AutoScaleBTCValues && TotalRate < 0.1)
            {
                toolStripStatusLabelBTCDayText.Text = "mBTC/" + International.GetText("Day");
                toolStripStatusLabelGlobalRateValue.Text = (TotalRate * 1000).ToString("F7", CultureInfo.InvariantCulture);
            }
            else
            {
                toolStripStatusLabelBTCDayText.Text = "BTC/" + International.GetText("Day");
                toolStripStatusLabelGlobalRateValue.Text = (TotalRate).ToString("F8", CultureInfo.InvariantCulture);
            }


            toolStripStatusLabelBTCDayValue.Text = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency((TotalRate * Globals.BitcoinRate)).ToString("F2", CultureInfo.InvariantCulture);
        }


        void BalanceCheck_Tick(object sender, EventArgs e)
        {
            if (VerifyMiningAddress(false))
            {
                Helpers.ConsolePrint("NICEHASH", "Balance get");
                double Balance = NiceHashStats.GetBalance(textBoxBTCAddress.Text.Trim(), textBoxBTCAddress.Text.Trim() + "." + textBoxWorkerName.Text.Trim());
                if (Balance > 0)
                {
                    if (Config.ConfigData.AutoScaleBTCValues && Balance < 0.1)
                    {
                        toolStripStatusLabelBalanceBTCCode.Text = "mBTC";
                        toolStripStatusLabelBalanceBTCValue.Text = (Balance * 1000).ToString("F7", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        toolStripStatusLabelBalanceBTCCode.Text = "BTC";
                        toolStripStatusLabelBalanceBTCValue.Text = Balance.ToString("F8", CultureInfo.InvariantCulture);
                    }

                    double Amount = (Balance * Globals.BitcoinRate);
                    Amount = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(Amount);
                    toolStripStatusLabelBalanceDollarText.Text = Amount.ToString("F2", CultureInfo.InvariantCulture);
                }
            }
        }


        void BitcoinExchangeCheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("COINBASE", "Bitcoin rate get");
            double BR = Bitcoin.GetUSDExchangeRate();
            if (BR > 0) Globals.BitcoinRate = BR;
            Helpers.ConsolePrint("COINBASE", "Current Bitcoin rate: " + Globals.BitcoinRate.ToString("F2", CultureInfo.InvariantCulture));
        }


        void SMACheck_Tick(object sender, EventArgs e)
        {
            string worker = textBoxBTCAddress.Text.Trim() + "." + textBoxWorkerName.Text.Trim();
            Helpers.ConsolePrint("NICEHASH", "SMA get");
            NiceHashSMA[] t = NiceHashStats.GetAlgorithmRates(worker);

            for (int i = 0; i < 3; i++)
            {
                if (t != null)
                {
                    Globals.NiceHashData = t;
                    break;
                }

                Helpers.ConsolePrint("NICEHASH", "SMA get failed .. retrying");
                System.Threading.Thread.Sleep(1000);
                t = NiceHashStats.GetAlgorithmRates(worker);
            }

            if (t == null && Globals.NiceHashData == null && ShowWarningNiceHashData)
            {
                ShowWarningNiceHashData = false;
                DialogResult dialogResult = MessageBox.Show(International.GetText("form1_msgbox_NoInternetMsg"),
                                                            International.GetText("form1_msgbox_NoInternetTitle"),
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (dialogResult == DialogResult.Yes)
                    return;
                else if (dialogResult == DialogResult.No)
                    System.Windows.Forms.Application.Exit();
            }
        }


        void UpdateCheck_Tick(object sender, EventArgs e)
        {
            Helpers.ConsolePrint("NICEHASH", "Version get");
            string ver = NiceHashStats.GetVersion(textBoxBTCAddress.Text.Trim() + "." + textBoxWorkerName.Text.Trim());

            if (ver == null) return;

            Version programVersion = new Version(Application.ProductVersion);
            Version onlineVersion = new Version(ver);
            int ret = programVersion.CompareTo(onlineVersion);

            if (ret < 0)
            {
                linkLabelVisitUs.Text = String.Format(International.GetText("form1_new_version_released"), ver);
                VisitURL = "https://github.com/nicehash/NiceHashMiner/releases/tag/" + ver;
            }
        }


        void SetEnvironmentVariables()
        {
            Helpers.ConsolePrint("NICEHASH", "Setting environment variables");

            string[] envName = { "GPU_MAX_ALLOC_PERCENT", "GPU_USE_SYNC_OBJECTS",
                                 "GPU_SINGLE_ALLOC_PERCENT", "GPU_MAX_HEAP_SIZE", "GPU_FORCE_64BIT_PTR" };
            string[] envValue = { "100", "1", "100", "100", "0" };

            for (int i = 0; i < envName.Length; i++)
            {
                // Check if all the variables is set
                if (Environment.GetEnvironmentVariable(envName[i]) == null)
                {
                    try { Environment.SetEnvironmentVariable(envName[i], envValue[i]); }
                    catch (Exception e) { Helpers.ConsolePrint("NICEHASH", e.ToString()); }
                }

                // Check to make sure all the values are set correctly
                if (!Environment.GetEnvironmentVariable(envName[i]).Equals(envValue[i]))
                {
                    try { Environment.SetEnvironmentVariable(envName[i], envValue[i]); }
                    catch (Exception e) { Helpers.ConsolePrint("NICEHASH", e.ToString()); }
                }
            }
        }


        private bool VerifyMiningAddress(bool ShowError)
        {
            if (!BitcoinAddress.ValidateBitcoinAddress(textBoxBTCAddress.Text.Trim()) && ShowError)
            {
                DialogResult result = MessageBox.Show(International.GetText("form1_msgbox_InvalidBTCAddressMsg"),
                                                      International.GetText("Error_with_Exclamation"),
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                
                if (result == System.Windows.Forms.DialogResult.Yes)
                    System.Diagnostics.Process.Start("https://www.nicehash.com/index.jsp?p=faq#faqs15");
                
                textBoxBTCAddress.Focus();
                return false;
            }
            else if (!BitcoinAddress.ValidateWorkerName(textBoxWorkerName.Text.Trim()) && ShowError)
            {
                DialogResult result = MessageBox.Show(International.GetText("form1_msgbox_InvalidWorkerNameMsg"),
                                                      International.GetText("Error_with_Exclamation"),
                                                      MessageBoxButtons.OK, MessageBoxIcon.Error);

                textBoxWorkerName.Focus();
                return false;
            }

            return true;
        }


        private void linkLabelVisitUs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(VisitURL);
        }


        private void linkLabelCheckStats_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!VerifyMiningAddress(true)) return;

            System.Diagnostics.Process.Start("http://www.nicehash.com/index.jsp?p=miners&addr=" + textBoxBTCAddress.Text.Trim());
        }


        private void linkLabelChooseBTCWallet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nicehash.com/index.jsp?p=faq#faqs15");
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
            foreach (Miner m in Globals.Miners)
                m.Stop(false);

            MessageBoxManager.Unregister();
        }


        private void buttonBenchmark_Click(object sender, EventArgs e)
        {
            bool NoBTCAddress = false;

            Config.ConfigData.ServiceLocation = comboBoxLocation.SelectedIndex;

            if (textBoxBTCAddress.Text == "")
            {
                NoBTCAddress = true;
                textBoxBTCAddress.Text = "34HKWdzLxWBduUfJE9JxaFhoXnfC6gmePG";
                Config.ConfigData.BitcoinAddress = textBoxBTCAddress.Text;
            }

            SMACheck.Stop();
            BenchmarkForm = new Form_Benchmark(false);
            BenchmarkForm.ShowDialog();
            BenchmarkForm = null;
            SMACheck.Start();

            if (NoBTCAddress)
            {
                NoBTCAddress = false;
                textBoxBTCAddress.Text = "";
                Config.ConfigData.BitcoinAddress = "";
            }
        }


        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (!Config.ConfigData.UseNewSettingsPage)
            {
                Process PHandle = new Process();
                PHandle.StartInfo.FileName = Application.ExecutablePath;
                PHandle.StartInfo.Arguments = "-config";
                PHandle.Start();

                Close();
            }
            else
            {
                Form_Settings Settings = new Form_Settings();
                Settings.ShowDialog();

                if (Settings.ret == 1) return;

                Process PHandle = new Process();
                PHandle.StartInfo.FileName = Application.ExecutablePath;
                PHandle.Start();

                Close();
            }
        }


        private void buttonStartMining_Click(object sender, EventArgs e)
        {
            if (textBoxBTCAddress.Text.Equals(""))
            {
                DialogResult result = MessageBox.Show(International.GetText("form1_DemoModeMsg"),
                                                      International.GetText("form1_DemoModeTitle"),
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    DemoMode = true;
                    labelDemoMode.Visible = true;
                    labelDemoMode.Text = International.GetText("form1_DemoModeLabel");

                    textBoxBTCAddress.Text = "34HKWdzLxWBduUfJE9JxaFhoXnfC6gmePG";
                }
                else
                    return;
            }
            else if (!VerifyMiningAddress(true)) return;

            if (Globals.NiceHashData == null)
            {
                MessageBox.Show(International.GetText("form1_msgbox_NullNiceHashDataMsg"),
                                International.GetText("Error_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the user has run benchmark first
            foreach (Miner m in Globals.Miners)
            {
                if (m.EnabledDeviceCount() == 0) continue;

                if (m.CountBenchmarkedAlgos() == 0)
                {
                    DialogResult result = MessageBox.Show(String.Format(International.GetText("form1_msgbox_HaveNotBenchmarkedMsg"), m.MinerDeviceName),
                                                          International.GetText("Warning_with_Exclamation"),
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!(m is cpuminer))
                        {
                            // quick and ugly way to prevent GPUs from starting on extremely unprofitable x11
                            m.SupportedAlgorithms[14].BenchmarkSpeed = 1;
                        }
                        break;
                    }
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        DemoMode = false;
                        labelDemoMode.Visible = false;

                        textBoxBTCAddress.Text = "";
                        Config.ConfigData.BitcoinAddress = "";
                        Config.Commit();

                        return;
                    }
                }
            }

            textBoxBTCAddress.Enabled = false;
            textBoxWorkerName.Enabled = false;
            comboBoxLocation.Enabled = false;
            buttonBenchmark.Enabled = false;
            buttonStartMining.Enabled = false;
            buttonSettings.Enabled = false;
            listViewDevices.Enabled = false;
            buttonStopMining.Enabled = true;

            Config.ConfigData.BitcoinAddress = textBoxBTCAddress.Text.Trim();
            Config.ConfigData.WorkerName = textBoxWorkerName.Text.Trim();
            Config.ConfigData.ServiceLocation = comboBoxLocation.SelectedIndex;
            if (!DemoMode) Config.Commit();

            SMAMinerCheck.Interval = 100;
            SMAMinerCheck.Start();
            //SMAMinerCheck_Tick(null, null);
            MinerStatsCheck.Start();
        }


        private void buttonStopMining_Click(object sender, EventArgs e)
        {
            MinerStatsCheck.Stop();
            SMAMinerCheck.Stop();

            foreach (Miner m in Globals.Miners)
            {
                m.Stop(false);
                m.IsRunning = false;
                m.CurrentAlgo = -1;
                m.CurrentRate = 0;
            }

            SetCPUStats("", 0, 0);
            SetNVIDIAtp21Stats("", 0, 0);
            SetNVIDIAspStats("", 0, 0);
            SetNVIDIAtpStats("", 0, 0);
            SetAMDOpenCLStats("", 0, 0);

            textBoxBTCAddress.Enabled = true;
            textBoxWorkerName.Enabled = true;
            comboBoxLocation.Enabled = true;
            buttonBenchmark.Enabled = true;
            buttonStartMining.Enabled = true;
            buttonSettings.Enabled = true;
            listViewDevices.Enabled = true;
            buttonStopMining.Enabled = false;

            if (DemoMode)
            {
                DemoMode = false;
                labelDemoMode.Visible = false;

                textBoxBTCAddress.Text = "";
                Config.ConfigData.BitcoinAddress = "";
                Config.Commit();
            }

            UpdateGlobalRate();
        }


        private string FormatSpeedOutput(double speed)
        {
            string ret = "";

            if (speed < 1000)
                ret = (speed).ToString("F3", CultureInfo.InvariantCulture) + " H/s ";
            else if (speed < 100000)
                ret = (speed * 0.001).ToString("F3", CultureInfo.InvariantCulture) + " kH/s ";
            else if (speed < 100000000)
                ret = (speed * 0.000001).ToString("F3", CultureInfo.InvariantCulture) + " MH/s ";
            else
                ret = (speed * 0.000000001).ToString("F3", CultureInfo.InvariantCulture) + " GH/s ";

            return ret;
        }

        private string FormatPayingOutput(double paying)
        {
            string ret = "";

            if (Config.ConfigData.AutoScaleBTCValues && paying < 0.1)
                ret = (paying * 1000).ToString("F7", CultureInfo.InvariantCulture) + " mBTC/" + International.GetText("Day");
            else
                ret = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/" + International.GetText("Day");

            return ret;
        }


        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nicehash/NiceHashMiner");
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

        private void textBoxCheckBoxMain_Leave(object sender, EventArgs e)
        {
            if (VerifyMiningAddress(false))
            {
                // Commit to config.json
                Config.ConfigData.BitcoinAddress = textBoxBTCAddress.Text.Trim();
                Config.ConfigData.WorkerName = textBoxWorkerName.Text.Trim();
                Config.ConfigData.ServiceLocation = comboBoxLocation.SelectedIndex;
                Config.Commit();
            }
        }

        // Minimize to system tray if MinimizeToTray is set to true
        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Properties.Resources.logo;
            notifyIcon1.Text = Application.ProductName + " v" + Application.ProductVersion + "\nDouble-click to restore..";

            if (Config.ConfigData.MinimizeToTray && FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
        }

        // Restore NiceHashMiner from the system tray
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
