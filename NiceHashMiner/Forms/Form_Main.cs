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
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Forms;
using NiceHashMiner.Miners;
using NiceHashMiner.Interfaces;
using NiceHashMiner.Forms.Components;
using NiceHashMiner.Utils;
using NiceHashMiner.PInvoke;

using SystemTimer = System.Timers.Timer;
using Timer = System.Windows.Forms.Timer;
using System.Timers;

namespace NiceHashMiner
{
    public partial class Form_Main : Form, Form_Loading.IAfterInitializationCaller, IMainFormRatesComunication
    {
        private static string VisitURL = Links.VisitURL;

        private Timer MinerStatsCheck;
        private Timer UpdateCheck;
        private SystemTimer SMACheck;
        private Timer BalanceCheck;
        private Timer SMAMinerCheck;
        private Timer BitcoinExchangeCheck;
        private Timer StartupTimer;
        private Timer IdleCheck;

        private bool ShowWarningNiceHashData;
        private bool DemoMode;

        private Random R;

        private Form_Loading _downloadUnzipForm;
        private Form_Loading LoadingScreen;
        private Form BenchmarkForm;

        int flowLayoutPanelVisibleCount = 0;
        int flowLayoutPanelRatesIndex = 0;
                
        const string _betaAlphaPostfixString = "";

        private bool _isDeviceDetectionInitialized = false;

        public Form_Main()
        {
            InitializeComponent();
            this.Icon = NiceHashMiner.Properties.Resources.logo;

            InitLocalization();

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

            Text += " v" + Application.ProductVersion + _betaAlphaPostfixString;

            label_NotProfitable.Visible = false;

            InitMainConfigGUIData();

            // for resizing
            InitFlowPanelStart();
            ClearRatesALL();
        }

        private void InitLocalization() {
            MessageBoxManager.Unregister();
            MessageBoxManager.Yes = International.GetText("Global_Yes");
            MessageBoxManager.No = International.GetText("Global_No");
            MessageBoxManager.OK = International.GetText("Global_OK");
            MessageBoxManager.Register();

            labelServiceLocation.Text = International.GetText("Service_Location") + ":";
            labelBitcoinAddress.Text = International.GetText("BitcoinAddress") + ":";
            labelWorkerName.Text = International.GetText("WorkerName") + ":";

            linkLabelVisitUs.Text = International.GetText("Form_Main_visit_us");
            linkLabelCheckStats.Text = International.GetText("Form_Main_check_stats");
            linkLabelChooseBTCWallet.Text = International.GetText("Form_Main_choose_bitcoin_wallet");

            // these strings are no longer used, check and use them as base
            string rateString = International.GetText("Rate") + ":";
            string ratesBTCInitialString = "0.00000000 BTC/" + International.GetText("Day");
            string ratesDollarInitialString = String.Format("0.00 {0}/", ConfigManager.Instance.GeneralConfig.DisplayCurrency) + International.GetText("Day");

            toolStripStatusLabelGlobalRateText.Text = International.GetText("Form_Main_global_rate") + ":";
            toolStripStatusLabelBTCDayText.Text = "BTC/" + International.GetText("Day");
            toolStripStatusLabelBalanceText.Text = (ConfigManager.Instance.GeneralConfig.DisplayCurrency + "/") + International.GetText("Day") + "     " + International.GetText("Form_Main_balance") + ":";

            devicesListViewEnableControl1.InitLocale();

            buttonBenchmark.Text = International.GetText("Form_Main_benchmark");
            buttonSettings.Text = International.GetText("Form_Main_settings");
            buttonStartMining.Text = International.GetText("Form_Main_start");
            buttonStopMining.Text = International.GetText("Form_Main_stop");

            label_NotProfitable.Text = International.GetText("Form_Main_MINING_NOT_PROFITABLE");
            groupBox1.Text = International.GetText("Form_Main_Group_Device_Rates");
        }

        private void InitMainConfigGUIData() {
            if (ConfigManager.Instance.GeneralConfig.ServiceLocation >= 0 && ConfigManager.Instance.GeneralConfig.ServiceLocation < Globals.MiningLocation.Length)
                comboBoxLocation.SelectedIndex = ConfigManager.Instance.GeneralConfig.ServiceLocation;
            else
                comboBoxLocation.SelectedIndex = 0;

            textBoxBTCAddress.Text = ConfigManager.Instance.GeneralConfig.BitcoinAddress;
            textBoxWorkerName.Text = ConfigManager.Instance.GeneralConfig.WorkerName;
            ShowWarningNiceHashData = true;
            DemoMode = false;

            toolStripStatusLabelBalanceDollarValue.Text = "(" + ConfigManager.Instance.GeneralConfig.DisplayCurrency + ")";

            if (_isDeviceDetectionInitialized) {
                devicesListViewEnableControl1.ResetComputeDevices(ComputeDevice.AllAvaliableDevices);
            }
        }

        public void AfterLoadComplete()
        {
            LoadingScreen = null;
            this.Enabled = true;

            IdleCheck = new Timer();
            IdleCheck.Tick += IdleCheck_Tick;
            IdleCheck.Interval = 500;
            IdleCheck.Start();
        }


        private void IdleCheck_Tick(object sender, EventArgs e)
        {
            if (!ConfigManager.Instance.GeneralConfig.StartMiningWhenIdle) return;

            uint MSIdle = Helpers.GetIdleTime();

            if (MinerStatsCheck.Enabled)
            {
                if (MSIdle < (ConfigManager.Instance.GeneralConfig.MinIdleSeconds * 1000))
                {
                    buttonStopMining_Click(null, null);
                    Helpers.ConsolePrint("NICEHASH", "Resumed from idling");
                }
            }
            else
            {
                if (BenchmarkForm == null && (MSIdle > (ConfigManager.Instance.GeneralConfig.MinIdleSeconds * 1000)))
                {
                    Helpers.ConsolePrint("NICEHASH", "Entering idling state");
                    buttonStartMining_Click(null, null);
                }
            }
        }

        // This is a single shot _benchmarkTimer
        private void StartupTimer_Tick(object sender, EventArgs e)
        {
            StartupTimer.Stop();
            StartupTimer = null;

            if (!Helpers.Is45NetOrHigher()) {
                MessageBox.Show(International.GetText("NET45_Not_Intsalled_msg"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK);

                this.Close();
                return;
            }

            // 
            CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            if (!Helpers.InternalCheckIsWow64()) {
                MessageBox.Show(International.GetText("Form_Main_x64_Support_Only"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK);

                this.Close();
                return;
            }

            // Query Avaliable ComputeDevices
            ComputeDeviceQueryManager.Instance.QueryDevices(LoadingScreen);
            _isDeviceDetectionInitialized = true;

            /////////////////////////////////////////////
            /////// from here on we have our devices and Miners initialized
            ConfigManager.Instance.AfterDeviceQueryInitialization();
            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_SaveConfig"));
            
            // All devices settup should be initialized in AllDevices
            devicesListViewEnableControl1.ResetComputeDevices(ComputeDevice.AllAvaliableDevices);
            // set properties after
            devicesListViewEnableControl1.AutoSaveChange = true;
            devicesListViewEnableControl1.SaveToGeneralConfig = true;

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_CheckLatestVersion"));

            MinerStatsCheck = new Timer();
            MinerStatsCheck.Tick += MinerStatsCheck_Tick;
            MinerStatsCheck.Interval = ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval * 1000;

            SMAMinerCheck = new Timer();
            SMAMinerCheck.Tick += SMAMinerCheck_Tick;
            SMAMinerCheck.Interval = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed * 1000 + R.Next(ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic * 1000);
            if (ComputeDeviceGroupManager.Instance.GetGroupCount(DeviceGroupType.AMD_OpenCL) > 0) {
                SMAMinerCheck.Interval = (ConfigManager.Instance.GeneralConfig.SwitchMinSecondsAMD + ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed) * 1000 + R.Next(ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic * 1000);
            }

            UpdateCheck = new Timer();
            UpdateCheck.Tick += UpdateCheck_Tick;
            UpdateCheck.Interval = 1000 * 3600; // every 1 hour
            UpdateCheck.Start();
            UpdateCheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_GetNiceHashSMA"));

            SMACheck = new SystemTimer();
            SMACheck.Elapsed += SMACheck_Tick;
            SMACheck.Interval = 60 * 1000; // every 60 seconds
            SMACheck.Start();

            // increase timeout
            if (Globals.IsFirstNetworkCheckTimeout) {
                while (!Helpers.WebRequestTestGoogle() && Globals.FirstNetworkCheckTimeoutTries > 0) {
                    --Globals.FirstNetworkCheckTimeoutTries;
                }
            }

            SMACheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_GetBTCRate"));

            BitcoinExchangeCheck = new Timer();
            BitcoinExchangeCheck.Tick += BitcoinExchangeCheck_Tick;
            BitcoinExchangeCheck.Interval = 1000 * 3601; // every 1 hour and 1 second
            BitcoinExchangeCheck.Start();
            BitcoinExchangeCheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_GetNiceHashBalance"));

            BalanceCheck = new Timer();
            BalanceCheck.Tick += BalanceCheck_Tick;
            BalanceCheck.Interval = 61 * 1000; // every 61 seconds
            BalanceCheck.Start();
            BalanceCheck_Tick(null, null);

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_SetEnvironmentVariable"));

            SetEnvironmentVariables();

            LoadingScreen.IncreaseLoadCounterAndMessage(International.GetText("Form_Main_loadtext_SetWindowsErrorReporting"));
            
            Helpers.DisableWindowsErrorReporting(ConfigManager.Instance.GeneralConfig.DisableWindowsErrorReporting);

            LoadingScreen.IncreaseLoadCounter();
            if (ConfigManager.Instance.GeneralConfig.NVIDIAP0State)
            {
                LoadingScreen.SetInfoMsg(International.GetText("Form_Main_loadtext_NVIDIAP0State"));
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

            LoadingScreen.FinishLoad();

            // check if download needed
            if (!MinersDownloadManager.Instance.IsMinersBinsInit() && !ConfigManager.Instance.GeneralConfig.DownloadInit) {
                _downloadUnzipForm = new Form_Loading();
                SetChildFormCenter(_downloadUnzipForm);
                _downloadUnzipForm.ShowDialog();
            }
            // check if files are mising
            if (!MinersDownloadManager.Instance.IsMinersBinsInit()) {
                var result = MessageBox.Show(International.GetText("Form_Main_bins_folder_files_missing"),
                    International.GetText("Warning_with_Exclamation"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) {
                    ConfigManager.Instance.GeneralConfig.DownloadInit = false;
                    ConfigManager.Instance.GeneralConfig.Commit();
                    Process PHandle = new Process();
                    PHandle.StartInfo.FileName = Application.ExecutablePath;
                    PHandle.Start();
                    Close();
                    return;
                }
            }

            // no bots please
            if (ConfigManager.Instance.GeneralConfig.hwidLoadFromFile && !ConfigManager.Instance.GeneralConfig.hwidOK) {
                var result = MessageBox.Show("NiceHash Miner has detected change of hardware ID. If you did not download and install NiceHash Miner, your computer may be compromised. In that case, we suggest you to install an antivirus program or reinstall your Windows.\r\n\r\nContinue with NiceHash Miner?",
                    //International.GetText("Form_Main_msgbox_anti_botnet_msgbox"),
                    International.GetText("Warning_with_Exclamation"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No) {
                    Close();
                    return;
                } else {
                    // users agrees he installed it so commit changes
                    ConfigManager.Instance.GeneralConfig.Commit();
                }
            } else {
                if (ConfigManager.Instance.GeneralConfig.AutoStartMining) {
                    buttonStartMining_Click(null, null);
                }
            }
        }

        private void SetChildFormCenter(Form form) {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X + (this.Width - form.Width) / 2, this.Location.Y + (this.Height - form.Height) / 2);
        }

        private void Form_Main_Shown(object sender, EventArgs e)
        {
            // general loading indicator
            int TotalLoadSteps = 12;
            LoadingScreen = new Form_Loading(this,
                International.GetText("Form_Loading_label_LoadingText"),
                International.GetText("Form_Main_loadtext_CPU"), TotalLoadSteps);
            SetChildFormCenter(LoadingScreen);
            LoadingScreen.Show();

            StartupTimer = new Timer();
            StartupTimer.Tick += StartupTimer_Tick;
            StartupTimer.Interval = 200;
            StartupTimer.Start();
        }

        private void SMAMinerCheck_Tick(object sender, EventArgs e)
        {
            SMAMinerCheck.Interval = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed * 1000 + R.Next(ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic * 1000);
            if (ComputeDeviceGroupManager.Instance.GetGroupCount(DeviceGroupType.AMD_OpenCL) > 0) {
                SMAMinerCheck.Interval = (ConfigManager.Instance.GeneralConfig.SwitchMinSecondsAMD + ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed) * 1000 + R.Next(ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic * 1000);
            }

#if (SWITCH_TESTING)
            SMAMinerCheck.Interval = MinersManager.SMAMinerCheckInterval;
#endif
            MinersManager.Instance.SwichMostProfitableGroupUpMethod(Globals.NiceHashData);
        }


        private void MinerStatsCheck_Tick(object sender, EventArgs e) {
            MinersManager.Instance.MinerStatsCheck(Globals.NiceHashData);
        }

        private void InitFlowPanelStart() {
            flowLayoutPanelRates.Controls.Clear();
            // add for every cdev a 
            foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                if(cdev.Enabled) {
                    var newGroupProfitControl = new GroupProfitControl();
                    newGroupProfitControl.Visible = false;
                    flowLayoutPanelRates.Controls.Add(newGroupProfitControl);
                }
            }
        }

        public void ClearRatesALL() {
            HideNotProfitable();
            ClearRates(-1);
        }

        public void ClearRates(int groupCount) {
            float panelHeight = -1;
            if (flowLayoutPanelVisibleCount != groupCount) {
                flowLayoutPanelVisibleCount = groupCount;
                // hide some Controls
                int hideIndex = 0;
                foreach (var control in flowLayoutPanelRates.Controls) {
                    ((GroupProfitControl)control).Visible = hideIndex < groupCount ? true : false;
                    ++hideIndex;
                }
            }
            flowLayoutPanelRatesIndex = 0;
            int visibleGroupCount = groupCount + 1;
            if (visibleGroupCount <= 0) visibleGroupCount = 1;
            if (panelHeight <= 0) {
                if (flowLayoutPanelRates.Controls != null && flowLayoutPanelRates.Controls.Count > 0) {
                    var control = flowLayoutPanelRates.Controls[0];
                    panelHeight = ((GroupProfitControl)control).Size.Height * 1.2f;
                } else {
                    panelHeight = 40;
                }
            }

            var oldHeight = groupBox1.Size.Height;
            groupBox1.Size = new Size(groupBox1.Size.Width, (int)( (visibleGroupCount) * panelHeight ));
            // set new height
            this.Size = new Size(this.Size.Width, this.Size.Height + groupBox1.Size.Height - oldHeight);
        }

        public void AddRateInfo(string groupName, string deviceStringInfo, APIData iAPIData, double paying, bool isApiGetException) {
            string ApiGetExceptionString = isApiGetException ? "**" : "";

            string speedString = Helpers.FormatSpeedOutput(iAPIData.Speed) + iAPIData.AlgorithmName + ApiGetExceptionString;
            string rateBTCString = FormatPayingOutput(paying);
            string rateCurrencyString = CurrencyConverter.CurrencyConverter.ConvertToActiveCurrency(paying * Globals.BitcoinRate).ToString("F2", CultureInfo.InvariantCulture)
                + String.Format(" {0}/", ConfigManager.Instance.GeneralConfig.DisplayCurrency) + International.GetText("Day");
            
            ((GroupProfitControl)flowLayoutPanelRates.Controls[flowLayoutPanelRatesIndex++])
                .UpdateProfitStats(groupName, deviceStringInfo, speedString, rateBTCString, rateCurrencyString);

            UpdateGlobalRate();
        }

        public void ShowNotProfitable(string msg) {
            label_NotProfitable.Visible = true;
            label_NotProfitable.Text = msg;
            label_NotProfitable.Invalidate();
        }
        public void HideNotProfitable() {
            label_NotProfitable.Visible = false;
            label_NotProfitable.Invalidate();
        }

        private void UpdateGlobalRate()
        {
            double TotalRate = MinersManager.Instance.GetTotalRate();

            if (ConfigManager.Instance.GeneralConfig.AutoScaleBTCValues && TotalRate < 0.1)
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
                    if (ConfigManager.Instance.GeneralConfig.AutoScaleBTCValues && Balance < 0.1)
                    {
                        toolStripStatusLabelBalanceBTCCode.Text = "mBTC";
                        toolStripStatusLabelBalanceBTCValue.Text = (Balance * 1000).ToString("F7", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        toolStripStatusLabelBalanceBTCCode.Text = "BTC";
                        toolStripStatusLabelBalanceBTCValue.Text = Balance.ToString("F8", CultureInfo.InvariantCulture);
                    }

                    //Helpers.ConsolePrint("CurrencyConverter", "Using CurrencyConverter" + ConfigManager.Instance.GeneralConfig.DisplayCurrency);
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
            Dictionary<AlgorithmType, NiceHashSMA> t = NiceHashStats.GetAlgorithmRates(worker);

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
                DialogResult dialogResult = MessageBox.Show(International.GetText("Form_Main_msgbox_NoInternetMsg"),
                                                            International.GetText("Form_Main_msgbox_NoInternetTitle"),
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
                linkLabelVisitUs.Text = String.Format(International.GetText("Form_Main_new_version_released"), ver);
                VisitURL = Links.VisitURLNew + ver;
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
                DialogResult result = MessageBox.Show(International.GetText("Form_Main_msgbox_InvalidBTCAddressMsg"),
                                                      International.GetText("Error_with_Exclamation"),
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                
                if (result == System.Windows.Forms.DialogResult.Yes)
                    System.Diagnostics.Process.Start(Links.NHM_BTC_Wallet_Faq);
                
                textBoxBTCAddress.Focus();
                return false;
            }
            else if (!BitcoinAddress.ValidateWorkerName(textBoxWorkerName.Text.Trim()) && ShowError)
            {
                DialogResult result = MessageBox.Show(International.GetText("Form_Main_msgbox_InvalidWorkerNameMsg"),
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

            System.Diagnostics.Process.Start(Links.CheckStats + textBoxBTCAddress.Text.Trim());
        }


        private void linkLabelChooseBTCWallet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Links.NHM_BTC_Wallet_Faq);
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MinersManager.Instance.StopAllMiners();

            MessageBoxManager.Unregister();
        }        

        private void buttonBenchmark_Click(object sender, EventArgs e)
        {
            ConfigManager.Instance.GeneralConfig.ServiceLocation = comboBoxLocation.SelectedIndex;

            BenchmarkForm = new Form_Benchmark();
            SetChildFormCenter(BenchmarkForm);
            BenchmarkForm.ShowDialog();
            BenchmarkForm = null;

            InitMainConfigGUIData();
        }


        private void buttonSettings_Click(object sender, EventArgs e)
        {
            Form_Settings Settings = new Form_Settings();
            SetChildFormCenter(Settings);
            Settings.ShowDialog();

            if (Settings.IsChange && Settings.IsChangeSaved && Settings.IsRestartNeeded) {
                MessageBox.Show(
                    International.GetText("Form_Main_Restart_Required_Msg"),
                    International.GetText("Form_Main_Restart_Required_Title"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process PHandle = new Process();
                PHandle.StartInfo.FileName = Application.ExecutablePath;
                PHandle.Start();
                Close();
            } else if (Settings.IsChange && Settings.IsChangeSaved) {
                InitLocalization();
                InitMainConfigGUIData();
                UpdateGlobalRate(); // update currency changes
            }
        }


        private void buttonStartMining_Click(object sender, EventArgs e)
        {
            if (textBoxBTCAddress.Text.Equals(""))
            {
                DialogResult result = MessageBox.Show(International.GetText("Form_Main_DemoModeMsg"),
                                                      International.GetText("Form_Main_DemoModeTitle"),
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    DemoMode = true;
                    labelDemoMode.Visible = true;
                    labelDemoMode.Text = International.GetText("Form_Main_DemoModeLabel");

                    //textBoxBTCAddress.Text = "34HKWdzLxWBduUfJE9JxaFhoXnfC6gmePG";
                }
                else
                    return;
            }
            else if (!VerifyMiningAddress(true)) return;

            if (Globals.NiceHashData == null)
            {
                MessageBox.Show(International.GetText("Form_Main_msgbox_NullNiceHashDataMsg"),
                                International.GetText("Error_with_Exclamation"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // first value is a boolean if initialized or not
            var tuplePair = DeviceBenchmarkConfigManager.Instance.IsEnabledBenchmarksInitialized();
            bool isBenchInit = tuplePair.Item1;
            Dictionary<string, List<AlgorithmType>> nonBenchmarkedPerDevice = tuplePair.Item2;
            // Check if the user has run benchmark first
            if (!isBenchInit) {
                DialogResult result = MessageBox.Show(International.GetText("EnabledUnbenchmarkedAlgorithmsWarning"),
                                                          International.GetText("Warning_with_Exclamation"),
                                                          MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.Yes) {
                    List<ComputeDevice> enabledDevices = new List<ComputeDevice>();
                    HashSet<string> deviceNames = new HashSet<string>();
                    foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                        if (cdev.Enabled && !deviceNames.Contains(cdev.Name)) {
                            deviceNames.Add(cdev.Name);
                            enabledDevices.Add(cdev);
                        }
                    }
                    BenchmarkForm = new Form_Benchmark(
                        BenchmarkPerformanceType.Standard,
                        true);
                    SetChildFormCenter(BenchmarkForm);
                    BenchmarkForm.ShowDialog();
                    BenchmarkForm = null;
                    InitMainConfigGUIData();
                } else if (result == System.Windows.Forms.DialogResult.No) {
                    // check devices without benchmarks
                    foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                        bool Enabled = false;
                        foreach (var algo in cdev.DeviceBenchmarkConfig.AlgorithmSettings) {
                            if (algo.Value.BenchmarkSpeed > 0) {
                                Enabled = true;
                                break;
                            }
                        }
                        cdev.ComputeDeviceEnabledOption.IsEnabled = Enabled;
                    }
                } else {
                    return;
                }
            }

            // check if any device enabled
            // check devices without benchmarks
            bool noDeviceEnabled = true;
            foreach (var cdev in ComputeDevice.AllAvaliableDevices) {
                if (cdev.ComputeDeviceEnabledOption.IsEnabled) {
                    noDeviceEnabled = false;
                    break;
                }
            }
            if (noDeviceEnabled) {
                DialogResult result = MessageBox.Show(International.GetText("Form_Main_No_Device_Enabled_For_Mining"),
                                                          International.GetText("Warning_with_Exclamation"),
                                                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            textBoxBTCAddress.Enabled = false;
            textBoxWorkerName.Enabled = false;
            comboBoxLocation.Enabled = false;
            buttonBenchmark.Enabled = false;
            buttonStartMining.Enabled = false;
            buttonSettings.Enabled = false;
            devicesListViewEnableControl1.IsMining = true;
            buttonStopMining.Enabled = true;

            ConfigManager.Instance.GeneralConfig.BitcoinAddress = textBoxBTCAddress.Text.Trim();
            ConfigManager.Instance.GeneralConfig.WorkerName = textBoxWorkerName.Text.Trim();
            ConfigManager.Instance.GeneralConfig.ServiceLocation = comboBoxLocation.SelectedIndex;

            InitFlowPanelStart();
            ClearRatesALL();

            var btcAdress = DemoMode ? Globals.DemoUser : textBoxBTCAddress.Text.Trim();
            var isMining = MinersManager.Instance.StartInitialize(this, Globals.MiningLocation[comboBoxLocation.SelectedIndex], textBoxWorkerName.Text.Trim(), btcAdress);

            if (!DemoMode) ConfigManager.Instance.GeneralConfig.Commit();

            SMAMinerCheck.Interval = 100;
            SMAMinerCheck.Start();
            //SMAMinerCheck_Tick(null, null);
            MinerStatsCheck.Start();
        }


        private void buttonStopMining_Click(object sender, EventArgs e)
        {
            MinerStatsCheck.Stop();
            SMAMinerCheck.Stop();

            MinersManager.Instance.StopAllMiners();

            textBoxBTCAddress.Enabled = true;
            textBoxWorkerName.Enabled = true;
            comboBoxLocation.Enabled = true;
            buttonBenchmark.Enabled = true;
            buttonStartMining.Enabled = true;
            buttonSettings.Enabled = true;
            devicesListViewEnableControl1.IsMining = false;
            buttonStopMining.Enabled = false;

            if (DemoMode)
            {
                DemoMode = false;
                labelDemoMode.Visible = false;

                //textBoxBTCAddress.Text = "";
                //ConfigManager.Instance.GeneralConfig.BitcoinAddress = "";
            }

            UpdateGlobalRate();
        }

        private string FormatPayingOutput(double paying)
        {
            string ret = "";

            if (ConfigManager.Instance.GeneralConfig.AutoScaleBTCValues && paying < 0.1)
                ret = (paying * 1000).ToString("F7", CultureInfo.InvariantCulture) + " mBTC/" + International.GetText("Day");
            else
                ret = paying.ToString("F8", CultureInfo.InvariantCulture) + " BTC/" + International.GetText("Day");

            return ret;
        }


        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Links.NHM_Help);
        }

        private void toolStripStatusLabel10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Links.NHM_Paying_Faq);
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
                ConfigManager.Instance.GeneralConfig.BitcoinAddress = textBoxBTCAddress.Text.Trim();
                ConfigManager.Instance.GeneralConfig.WorkerName = textBoxWorkerName.Text.Trim();
                ConfigManager.Instance.GeneralConfig.ServiceLocation = comboBoxLocation.SelectedIndex;
                ConfigManager.Instance.GeneralConfig.Commit();
            }
        }

        // Minimize to system tray if MinimizeToTray is set to true
        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.Icon = Properties.Resources.logo;
            notifyIcon1.Text = Application.ProductName + " v" + Application.ProductVersion + "\nDouble-click to restore..";

            if (ConfigManager.Instance.GeneralConfig.MinimizeToTray && FormWindowState.Minimized == this.WindowState)
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
