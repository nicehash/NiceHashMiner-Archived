using NiceHashMiner.Configs;
using NiceHashMiner.Configs.Data;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms {
    public partial class Form_Settings : Form {


        private bool _isInitFinished = false;
        private bool _isChange = false;
        public bool IsChange {
            get { return _isChange; }
            private set {
                if (_isInitFinished) {
                    _isChange = value;
                } else {
                    _isChange = false;
                }
            }
        }
        public bool IsChangeSaved { get; private set; }
        public bool IsRestartNeeded { get; private set; }

        // most likely we wil have settings only per unique devices
        bool ShowUniqueDeviceList = true;

        ComputeDevice _selectedComputeDevice;

        public Form_Settings() {
            InitializeComponent();
            this.Icon = NiceHashMiner.Properties.Resources.logo;

            //ret = 1; // default
            IsChange = false;
            IsChangeSaved = false;

            // backup settings
            ConfigManager.CreateBackup();
            
            // initialize form
            InitializeFormTranslations();

            // Initialize toolTip
            InitializeToolTip();

            // Initialize tabs
            InitializeGeneralTab();

            // initialization calls 
            InitializeDevicesTab();
            // link algorithm list with algorithm settings control
            algorithmSettingsControl1.Enabled = false;
            algorithmsListView1.ComunicationInterface = algorithmSettingsControl1;
            //algorithmsListView1.RemoveRatioRates();


            // set first device selected {
            if (ComputeDeviceManager.Avaliable.AllAvaliableDevices.Count > 0) {
                _selectedComputeDevice = ComputeDeviceManager.Avaliable.AllAvaliableDevices[0];
                algorithmsListView1.SetAlgorithms(_selectedComputeDevice, _selectedComputeDevice.Enabled);
                groupBoxAlgorithmSettings.Text = String.Format(International.GetText("FormSettings_AlgorithmsSettings"), _selectedComputeDevice.Name);
            }

            // At the very end set to true
            _isInitFinished = true;
        }

        #region Initializations

        private void InitializeToolTip() {
            // Setup Tooltips
            toolTip1.SetToolTip(this.comboBox_Language, International.GetText("Form_Settings_ToolTip_Language"));
            toolTip1.SetToolTip(this.label_Language, International.GetText("Form_Settings_ToolTip_Language"));
            toolTip1.SetToolTip(this.pictureBox_Language, International.GetText("Form_Settings_ToolTip_Language"));
            
            toolTip1.SetToolTip(this.checkBox_DebugConsole, International.GetText("Form_Settings_ToolTip_checkBox_DebugConsole"));
            toolTip1.SetToolTip(this.pictureBox_DebugConsole, International.GetText("Form_Settings_ToolTip_checkBox_DebugConsole"));
            
            toolTip1.SetToolTip(this.textBox_BitcoinAddress, International.GetText("Form_Settings_ToolTip_BitcoinAddress"));
            toolTip1.SetToolTip(this.label_BitcoinAddress, International.GetText("Form_Settings_ToolTip_BitcoinAddress"));
            toolTip1.SetToolTip(this.pictureBox_Info_BitcoinAddress, International.GetText("Form_Settings_ToolTip_BitcoinAddress"));
            
            toolTip1.SetToolTip(this.textBox_WorkerName, International.GetText("Form_Settings_ToolTip_WorkerName"));
            toolTip1.SetToolTip(this.label_WorkerName, International.GetText("Form_Settings_ToolTip_WorkerName"));
            toolTip1.SetToolTip(this.pictureBox_WorkerName, International.GetText("Form_Settings_ToolTip_WorkerName"));
            
            toolTip1.SetToolTip(this.comboBox_ServiceLocation, International.GetText("Form_Settings_ToolTip_ServiceLocation"));
            toolTip1.SetToolTip(this.label_ServiceLocation, International.GetText("Form_Settings_ToolTip_ServiceLocation"));
            toolTip1.SetToolTip(this.pictureBox_ServiceLocation, International.GetText("Form_Settings_ToolTip_ServiceLocation"));
            
            toolTip1.SetToolTip(this.checkBox_HideMiningWindows, International.GetText("Form_Settings_ToolTip_checkBox_HideMiningWindows"));
            toolTip1.SetToolTip(this.pictureBox_HideMiningWindows, International.GetText("Form_Settings_ToolTip_checkBox_HideMiningWindows"));
            
            toolTip1.SetToolTip(this.checkBox_MinimizeToTray, International.GetText("Form_Settings_ToolTip_checkBox_MinimizeToTray"));
            toolTip1.SetToolTip(this.pictureBox_MinimizeToTray, International.GetText("Form_Settings_ToolTip_checkBox_MinimizeToTray"));

            toolTip1.SetToolTip(this.checkBox_Use3rdPartyMiners, International.GetText("Form_Settings_General_3rdparty_ToolTip"));
            toolTip1.SetToolTip(this.pictureBox_Use3rdPartyMiners, International.GetText("Form_Settings_General_3rdparty_ToolTip"));

            toolTip1.SetToolTip(this.checkBox_AllowMultipleInstances, International.GetText("Form_Settings_General_AllowMultipleInstances_ToolTip"));
            toolTip1.SetToolTip(this.pictureBox_AllowMultipleInstances, International.GetText("Form_Settings_General_AllowMultipleInstances_ToolTip"));
            

            toolTip1.SetToolTip(this.textBox_SwitchMinSecondsFixed, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsFixed"));
            toolTip1.SetToolTip(this.label_SwitchMinSecondsFixed, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsFixed"));
            toolTip1.SetToolTip(this.pictureBox_SwitchMinSecondsFixed, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsFixed"));

            toolTip1.SetToolTip(this.label_MinProfit, International.GetText("Form_Settings_ToolTip_MinimumProfit"));
            toolTip1.SetToolTip(this.pictureBox_MinProfit, International.GetText("Form_Settings_ToolTip_MinimumProfit"));
            toolTip1.SetToolTip(this.textBox_MinProfit, International.GetText("Form_Settings_ToolTip_MinimumProfit"));

            toolTip1.SetToolTip(this.textBox_SwitchMinSecondsDynamic, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsDynamic"));
            toolTip1.SetToolTip(this.label_SwitchMinSecondsDynamic, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsDynamic"));
            toolTip1.SetToolTip(this.pictureBox_SwitchMinSecondsDynamic, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsDynamic"));

            toolTip1.SetToolTip(this.textBox_SwitchMinSecondsAMD, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsAMD"));
            toolTip1.SetToolTip(this.label_SwitchMinSecondsAMD, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsAMD"));
            toolTip1.SetToolTip(this.pictureBox_SwitchMinSecondsAMD, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsAMD"));

            toolTip1.SetToolTip(this.textBox_MinerAPIQueryInterval, International.GetText("Form_Settings_ToolTip_MinerAPIQueryInterval"));
            toolTip1.SetToolTip(this.label_MinerAPIQueryInterval, International.GetText("Form_Settings_ToolTip_MinerAPIQueryInterval"));
            toolTip1.SetToolTip(this.pictureBox_MinerAPIQueryInterval, International.GetText("Form_Settings_ToolTip_MinerAPIQueryInterval"));

            toolTip1.SetToolTip(this.textBox_MinerRestartDelayMS, International.GetText("Form_Settings_ToolTip_MinerRestartDelayMS"));
            toolTip1.SetToolTip(this.label_MinerRestartDelayMS, International.GetText("Form_Settings_ToolTip_MinerRestartDelayMS"));
            toolTip1.SetToolTip(this.pictureBox_MinerRestartDelayMS, International.GetText("Form_Settings_ToolTip_MinerRestartDelayMS"));

            toolTip1.SetToolTip(this.textBox_APIBindPortStart, International.GetText("Form_Settings_ToolTip_APIBindPortStart"));
            toolTip1.SetToolTip(this.label_APIBindPortStart, International.GetText("Form_Settings_ToolTip_APIBindPortStart"));
            toolTip1.SetToolTip(this.pictureBox_APIBindPortStart, International.GetText("Form_Settings_ToolTip_APIBindPortStart"));

            toolTip1.SetToolTip(this.comboBox_DagLoadMode, International.GetText("Form_Settings_ToolTip_DagGeneration"));
            toolTip1.SetToolTip(this.label_DagGeneration, International.GetText("Form_Settings_ToolTip_DagGeneration"));
            toolTip1.SetToolTip(this.pictureBox_DagGeneration, International.GetText("Form_Settings_ToolTip_DagGeneration"));

            benchmarkLimitControlCPU.SetToolTip(ref toolTip1, "CPUs");
            benchmarkLimitControlNVIDIA.SetToolTip(ref toolTip1, "NVIDIA GPUs");
            benchmarkLimitControlAMD.SetToolTip(ref toolTip1, "AMD GPUs");

            toolTip1.SetToolTip(this.checkBox_DisableDetectionNVIDIA, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "NVIDIA"));
            toolTip1.SetToolTip(this.checkBox_DisableDetectionAMD, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "AMD"));
            toolTip1.SetToolTip(this.pictureBox_DisableDetectionNVIDIA, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "NVIDIA"));
            toolTip1.SetToolTip(this.pictureBox_DisableDetectionAMD, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "AMD"));

            toolTip1.SetToolTip(this.checkBox_AutoScaleBTCValues, International.GetText("Form_Settings_ToolTip_checkBox_AutoScaleBTCValues"));
            toolTip1.SetToolTip(this.pictureBox_AutoScaleBTCValues, International.GetText("Form_Settings_ToolTip_checkBox_AutoScaleBTCValues"));
            
            toolTip1.SetToolTip(this.checkBox_StartMiningWhenIdle, International.GetText("Form_Settings_ToolTip_checkBox_StartMiningWhenIdle"));
            toolTip1.SetToolTip(this.pictureBox_StartMiningWhenIdle, International.GetText("Form_Settings_ToolTip_checkBox_StartMiningWhenIdle"));

            toolTip1.SetToolTip(this.textBox_MinIdleSeconds, International.GetText("Form_Settings_ToolTip_MinIdleSeconds"));
            toolTip1.SetToolTip(this.label_MinIdleSeconds, International.GetText("Form_Settings_ToolTip_MinIdleSeconds"));
            toolTip1.SetToolTip(this.pictureBox_MinIdleSeconds, International.GetText("Form_Settings_ToolTip_MinIdleSeconds"));
            
            toolTip1.SetToolTip(this.checkBox_LogToFile, International.GetText("Form_Settings_ToolTip_checkBox_LogToFile"));
            toolTip1.SetToolTip(this.pictureBox_LogToFile, International.GetText("Form_Settings_ToolTip_checkBox_LogToFile"));


            toolTip1.SetToolTip(this.textBox_LogMaxFileSize, International.GetText("Form_Settings_ToolTip_LogMaxFileSize"));
            toolTip1.SetToolTip(this.label_LogMaxFileSize, International.GetText("Form_Settings_ToolTip_LogMaxFileSize"));
            toolTip1.SetToolTip(this.pictureBox_LogMaxFileSize, International.GetText("Form_Settings_ToolTip_LogMaxFileSize"));

            toolTip1.SetToolTip(this.checkBox_ShowDriverVersionWarning, International.GetText("Form_Settings_ToolTip_checkBox_ShowDriverVersionWarning"));
            toolTip1.SetToolTip(this.pictureBox_ShowDriverVersionWarning, International.GetText("Form_Settings_ToolTip_checkBox_ShowDriverVersionWarning"));
            
            toolTip1.SetToolTip(this.checkBox_DisableWindowsErrorReporting, International.GetText("Form_Settings_ToolTip_checkBox_DisableWindowsErrorReporting"));
            toolTip1.SetToolTip(this.pictureBox_DisableWindowsErrorReporting, International.GetText("Form_Settings_ToolTip_checkBox_DisableWindowsErrorReporting"));
            
            toolTip1.SetToolTip(this.checkBox_NVIDIAP0State, International.GetText("Form_Settings_ToolTip_checkBox_NVIDIAP0State"));
            toolTip1.SetToolTip(this.pictureBox_NVIDIAP0State, International.GetText("Form_Settings_ToolTip_checkBox_NVIDIAP0State"));


            toolTip1.SetToolTip(this.checkBox_AutoStartMining, International.GetText("Form_Settings_ToolTip_checkBox_AutoStartMining"));
            toolTip1.SetToolTip(this.pictureBox_AutoStartMining, International.GetText("Form_Settings_ToolTip_checkBox_AutoStartMining"));

            
            toolTip1.SetToolTip(this.textBox_ethminerDefaultBlockHeight, International.GetText("Form_Settings_ToolTip_ethminerDefaultBlockHeight"));
            toolTip1.SetToolTip(this.label_ethminerDefaultBlockHeight, International.GetText("Form_Settings_ToolTip_ethminerDefaultBlockHeight"));
            toolTip1.SetToolTip(this.pictureBox_ethminerDefaultBlockHeight, International.GetText("Form_Settings_ToolTip_ethminerDefaultBlockHeight"));

            toolTip1.SetToolTip(this.label_displayCurrency, International.GetText("Form_Settings_ToolTip_DisplayCurrency"));
            toolTip1.SetToolTip(this.pictureBox_displayCurrency, International.GetText("Form_Settings_ToolTip_DisplayCurrency"));
            toolTip1.SetToolTip(this.currencyConverterCombobox, International.GetText("Form_Settings_ToolTip_DisplayCurrency"));
            
            // Setup Tooltips CPU
            toolTip1.SetToolTip(comboBox_CPU0_ForceCPUExtension, International.GetText("Form_Settings_ToolTip_CPU_ForceCPUExtension"));
            toolTip1.SetToolTip(label_CPU0_ForceCPUExtension, International.GetText("Form_Settings_ToolTip_CPU_ForceCPUExtension"));
            toolTip1.SetToolTip(pictureBox_CPU0_ForceCPUExtension, International.GetText("Form_Settings_ToolTip_CPU_ForceCPUExtension"));

            // amd disable temp control
            toolTip1.SetToolTip(checkBox_AMD_DisableAMDTempControl, International.GetText("Form_Settings_ToolTip_DisableAMDTempControl"));
            toolTip1.SetToolTip(pictureBox_AMD_DisableAMDTempControl, International.GetText("Form_Settings_ToolTip_DisableAMDTempControl"));

            // disable default optimizations
            toolTip1.SetToolTip(checkBox_DisableDefaultOptimizations, International.GetText("Form_Settings_ToolTip_DisableDefaultOptimizations"));
            toolTip1.SetToolTip(pictureBox_DisableDefaultOptimizations, International.GetText("Form_Settings_ToolTip_DisableDefaultOptimizations"));

            // internet connection mining check
            toolTip1.SetToolTip(checkBox_IdleWhenNoInternetAccess, International.GetText("Form_Settings_ToolTip_ContinueMiningIfNoInternetAccess"));
            toolTip1.SetToolTip(pictureBox_IdleWhenNoInternetAccess, International.GetText("Form_Settings_ToolTip_ContinueMiningIfNoInternetAccess"));

            toolTip1.SetToolTip(pictureBox_SwitchProfitabilityThreshold, International.GetText("Form_Settings_ToolTip_SwitchProfitabilityThreshold"));
            toolTip1.SetToolTip(label_SwitchProfitabilityThreshold, International.GetText("Form_Settings_ToolTip_SwitchProfitabilityThreshold"));

            this.Text = International.GetText("Form_Settings_Title");

            algorithmSettingsControl1.InitLocale(toolTip1);
        }

        #region Form this
        private void InitializeFormTranslations() {
            buttonDefaults.Text = International.GetText("Form_Settings_buttonDefaultsText");
            buttonSaveClose.Text = International.GetText("Form_Settings_buttonSaveText");
            buttonCloseNoSave.Text = International.GetText("Form_Settings_buttonCloseNoSaveText");
        }
        #endregion //Form this

        #region Tab General

        private void InitializeGeneralTabTranslations() {
            checkBox_DebugConsole.Text = International.GetText("Form_Settings_General_DebugConsole");
            checkBox_AutoStartMining.Text = International.GetText("Form_Settings_General_AutoStartMining");
            checkBox_HideMiningWindows.Text = International.GetText("Form_Settings_General_HideMiningWindows");
            checkBox_MinimizeToTray.Text = International.GetText("Form_Settings_General_MinimizeToTray");
            checkBox_DisableDetectionNVIDIA.Text = String.Format(International.GetText("Form_Settings_General_DisableDetection"), "NVIDIA");
            checkBox_DisableDetectionAMD.Text = String.Format(International.GetText("Form_Settings_General_DisableDetection"), "AMD");
            checkBox_AutoScaleBTCValues.Text = International.GetText("Form_Settings_General_AutoScaleBTCValues");
            checkBox_StartMiningWhenIdle.Text = International.GetText("Form_Settings_General_StartMiningWhenIdle");
            checkBox_ShowDriverVersionWarning.Text = International.GetText("Form_Settings_General_ShowDriverVersionWarning");
            checkBox_DisableWindowsErrorReporting.Text = International.GetText("Form_Settings_General_DisableWindowsErrorReporting");
            checkBox_Use3rdPartyMiners.Text = International.GetText("Form_Settings_General_3rdparty_Text");
            checkBox_NVIDIAP0State.Text = International.GetText("Form_Settings_General_NVIDIAP0State");
            checkBox_LogToFile.Text = International.GetText("Form_Settings_General_LogToFile");
            checkBox_AMD_DisableAMDTempControl.Text = International.GetText("Form_Settings_General_DisableAMDTempControl");
            checkBox_AllowMultipleInstances.Text = International.GetText("Form_Settings_General_AllowMultipleInstances_Text");

            label_Language.Text = International.GetText("Form_Settings_General_Language") + ":";
            label_BitcoinAddress.Text = International.GetText("BitcoinAddress") + ":";
            label_WorkerName.Text = International.GetText("WorkerName") + ":";
            label_ServiceLocation.Text = International.GetText("Service_Location") + ":";
            label_MinIdleSeconds.Text = International.GetText("Form_Settings_General_MinIdleSeconds") + ":";
            label_MinerRestartDelayMS.Text = International.GetText("Form_Settings_General_MinerRestartDelayMS") + ":";
            label_MinerAPIQueryInterval.Text = International.GetText("Form_Settings_General_MinerAPIQueryInterval") + ":";
            label_LogMaxFileSize.Text = International.GetText("Form_Settings_General_LogMaxFileSize") + ":";

            label_SwitchMinSecondsFixed.Text = International.GetText("Form_Settings_General_SwitchMinSecondsFixed") + ":";
            label_SwitchMinSecondsDynamic.Text = International.GetText("Form_Settings_General_SwitchMinSecondsDynamic") + ":";
            label_SwitchMinSecondsAMD.Text = International.GetText("Form_Settings_General_SwitchMinSecondsAMD") + ":";

            label_ethminerDefaultBlockHeight.Text = International.GetText("Form_Settings_General_ethminerDefaultBlockHeight") + ":";
            label_DagGeneration.Text = International.GetText("Form_Settings_DagGeneration") + ":";
            label_APIBindPortStart.Text = International.GetText("Form_Settings_APIBindPortStart") + ":";

            label_MinProfit.Text = International.GetText("Form_Settings_General_MinimumProfit") + ":";

            label_displayCurrency.Text = International.GetText("Form_Settings_DisplayCurrency");

            // Benchmark time limits
            // internationalization change
            groupBoxBenchmarkTimeLimits.Text = International.GetText("Form_Settings_General_BenchmarkTimeLimits_Title") + ":";
            benchmarkLimitControlCPU.GroupName = International.GetText("Form_Settings_General_BenchmarkTimeLimitsCPU_Group") + ":";
            benchmarkLimitControlNVIDIA.GroupName = International.GetText("Form_Settings_General_BenchmarkTimeLimitsNVIDIA_Group") + ":";
            benchmarkLimitControlAMD.GroupName = International.GetText("Form_Settings_General_BenchmarkTimeLimitsAMD_Group") + ":";
            // moved from constructor because of editor
            benchmarkLimitControlCPU.InitLocale();
            benchmarkLimitControlNVIDIA.InitLocale();
            benchmarkLimitControlAMD.InitLocale();

            // device enabled listview translation
            devicesListViewEnableControl1.InitLocale();
            algorithmsListView1.InitLocale();

            // Setup Tooltips CPU
            label_CPU0_ForceCPUExtension.Text = International.GetText("Form_Settings_General_CPU_ForceCPUExtension") + ":";
            // new translations
            tabControlGeneral.TabPages[0].Text = International.GetText("FormSettings_Tab_General");
            tabControlGeneral.TabPages[1].Text = International.GetText("FormSettings_Tab_Advanced");
            tabControlGeneral.TabPages[2].Text = International.GetText("FormSettings_Tab_Devices_Algorithms");
            groupBox_Main.Text = International.GetText("FormSettings_Tab_General_Group_Main");
            groupBox_Localization.Text = International.GetText("FormSettings_Tab_General_Group_Localization");
            groupBox_Logging.Text = International.GetText("FormSettings_Tab_General_Group_Logging");
            groupBox_Misc.Text = International.GetText("FormSettings_Tab_General_Group_Misc");
            // advanced
            groupBox_Miners.Text = International.GetText("FormSettings_Tab_Advanced_Group_Miners");
            groupBoxBenchmarkTimeLimits.Text = International.GetText("FormSettings_Tab_Advanced_Group_BenchmarkTimeLimits");

            buttonAllProfit.Text = International.GetText("FormSettings_Tab_Devices_Algorithms_Check_ALLProfitability");
            buttonSelectedProfit.Text = International.GetText("FormSettings_Tab_Devices_Algorithms_Check_SingleProfitability");

            checkBox_DisableDefaultOptimizations.Text = International.GetText("Form_Settings_Text_DisableDefaultOptimizations");
            checkBox_IdleWhenNoInternetAccess.Text = International.GetText("Form_Settings_Text_ContinueMiningIfNoInternetAccess");

            label_SwitchProfitabilityThreshold.Text = International.GetText("Form_Settings_General_SwitchProfitabilityThreshold");
        }

        private void InitializeGeneralTabCallbacks() {
            // Add EventHandler for all the general tab's checkboxes
            {
                this.checkBox_AutoScaleBTCValues.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableDetectionAMD.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableDetectionNVIDIA.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_MinimizeToTray.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_HideMiningWindows.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DebugConsole.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_ShowDriverVersionWarning.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableWindowsErrorReporting.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_StartMiningWhenIdle.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_NVIDIAP0State.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_LogToFile.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_AutoStartMining.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_AllowMultipleInstances.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
            }
            // Add EventHandler for all the general tab's textboxes
            {
                this.textBox_BitcoinAddress.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_WorkerName.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                // these are ints only
                this.textBox_SwitchMinSecondsFixed.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_SwitchMinSecondsDynamic.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_SwitchMinSecondsAMD.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_MinerAPIQueryInterval.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_MinerRestartDelayMS.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_MinIdleSeconds.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_LogMaxFileSize.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_ethminerDefaultBlockHeight.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_APIBindPortStart.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_MinProfit.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                // set int only keypress
                this.textBox_SwitchMinSecondsFixed.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_SwitchMinSecondsDynamic.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_SwitchMinSecondsAMD.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinerAPIQueryInterval.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinerRestartDelayMS.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinIdleSeconds.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_LogMaxFileSize.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_ethminerDefaultBlockHeight.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_APIBindPortStart.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                // set double only keypress
                this.textBox_MinProfit.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxDoubleOnly_KeyPress);
            }
            // Add EventHandler for all the general tab's textboxes
            {
                this.comboBox_Language.Leave += new System.EventHandler(this.GeneralComboBoxes_Leave);
                this.comboBox_ServiceLocation.Leave += new System.EventHandler(this.GeneralComboBoxes_Leave);
                this.comboBox_DagLoadMode.Leave += new System.EventHandler(this.GeneralComboBoxes_Leave);
            }

            // CPU exceptions
            comboBox_CPU0_ForceCPUExtension.SelectedIndex = (int)ConfigManager.GeneralConfig.ForceCPUExtension;
            comboBox_CPU0_ForceCPUExtension.SelectedIndexChanged += comboBox_CPU0_ForceCPUExtension_SelectedIndexChanged;
            // fill dag dropdown
            comboBox_DagLoadMode.Items.Clear();
            for (int i = 0; i < (int)DagGenerationType.END; ++i) {
                comboBox_DagLoadMode.Items.Add(MinerEtherum.GetDagGenerationString((DagGenerationType)i));
            }
            // set selected
            comboBox_DagLoadMode.SelectedIndex = (int)ConfigManager.GeneralConfig.EthminerDagGenerationType;
        }

        private void InitializeGeneralTabFieldValuesReferences() {
            // Checkboxes set checked value
            {
                checkBox_DebugConsole.Checked = ConfigManager.GeneralConfig.DebugConsole;
                checkBox_AutoStartMining.Checked = ConfigManager.GeneralConfig.AutoStartMining;
                checkBox_HideMiningWindows.Checked = ConfigManager.GeneralConfig.HideMiningWindows;
                checkBox_MinimizeToTray.Checked = ConfigManager.GeneralConfig.MinimizeToTray;
                checkBox_DisableDetectionNVIDIA.Checked = ConfigManager.GeneralConfig.DeviceDetection.DisableDetectionNVIDIA;
                checkBox_DisableDetectionAMD.Checked = ConfigManager.GeneralConfig.DeviceDetection.DisableDetectionAMD;
                checkBox_AutoScaleBTCValues.Checked = ConfigManager.GeneralConfig.AutoScaleBTCValues;
                checkBox_StartMiningWhenIdle.Checked = ConfigManager.GeneralConfig.StartMiningWhenIdle;
                checkBox_ShowDriverVersionWarning.Checked = ConfigManager.GeneralConfig.ShowDriverVersionWarning;
                checkBox_DisableWindowsErrorReporting.Checked = ConfigManager.GeneralConfig.DisableWindowsErrorReporting;
                checkBox_NVIDIAP0State.Checked = ConfigManager.GeneralConfig.NVIDIAP0State;
                checkBox_LogToFile.Checked = ConfigManager.GeneralConfig.LogToFile;
                checkBox_AMD_DisableAMDTempControl.Checked = ConfigManager.GeneralConfig.DisableAMDTempControl;
                checkBox_DisableDefaultOptimizations.Checked = ConfigManager.GeneralConfig.DisableDefaultOptimizations;
                checkBox_IdleWhenNoInternetAccess.Checked = ConfigManager.GeneralConfig.IdleWhenNoInternetAccess;
                this.checkBox_Use3rdPartyMiners.Checked = ConfigManager.GeneralConfig.Use3rdPartyMiners == Use3rdPartyMiners.YES;
                this.checkBox_AllowMultipleInstances.Checked = ConfigManager.GeneralConfig.AllowMultipleInstances;
            }

            // Textboxes
            {
                textBox_BitcoinAddress.Text = ConfigManager.GeneralConfig.BitcoinAddress;
                textBox_WorkerName.Text = ConfigManager.GeneralConfig.WorkerName;
                textBox_SwitchMinSecondsFixed.Text = ConfigManager.GeneralConfig.SwitchMinSecondsFixed.ToString();
                textBox_SwitchMinSecondsDynamic.Text = ConfigManager.GeneralConfig.SwitchMinSecondsDynamic.ToString();
                textBox_SwitchMinSecondsAMD.Text = ConfigManager.GeneralConfig.SwitchMinSecondsAMD.ToString();
                textBox_MinerAPIQueryInterval.Text = ConfigManager.GeneralConfig.MinerAPIQueryInterval.ToString();
                textBox_MinerRestartDelayMS.Text = ConfigManager.GeneralConfig.MinerRestartDelayMS.ToString();
                textBox_MinIdleSeconds.Text = ConfigManager.GeneralConfig.MinIdleSeconds.ToString();
                textBox_LogMaxFileSize.Text = ConfigManager.GeneralConfig.LogMaxFileSize.ToString();
                textBox_ethminerDefaultBlockHeight.Text = ConfigManager.GeneralConfig.ethminerDefaultBlockHeight.ToString();
                textBox_APIBindPortStart.Text = ConfigManager.GeneralConfig.ApiBindPortPoolStart.ToString();
                textBox_MinProfit.Text = ConfigManager.GeneralConfig.MinimumProfit.ToString("F2").Replace(',', '.'); // force comma;
                textBox_SwitchProfitabilityThreshold.Text = ConfigManager.GeneralConfig.SwitchProfitabilityThreshold.ToString("F2").Replace(',', '.'); // force comma;
            }

            // set custom control referances
            {
                benchmarkLimitControlCPU.TimeLimits = ConfigManager.GeneralConfig.BenchmarkTimeLimits.CPU;
                benchmarkLimitControlNVIDIA.TimeLimits = ConfigManager.GeneralConfig.BenchmarkTimeLimits.NVIDIA;
                benchmarkLimitControlAMD.TimeLimits = ConfigManager.GeneralConfig.BenchmarkTimeLimits.AMD;

                // here we want all devices
                devicesListViewEnableControl1.SetComputeDevices(ComputeDeviceManager.Avaliable.AllAvaliableDevices);
                devicesListViewEnableControl1.SetAlgorithmsListView(algorithmsListView1);
                devicesListViewEnableControl1.IsSettingsCopyEnabled = true;
            }

            // Add language selections list
            {
                Dictionary<LanguageType, string> lang = International.GetAvailableLanguages();

                comboBox_Language.Items.Clear();
                for (int i = 0; i < lang.Count; i++) {
                    comboBox_Language.Items.Add(lang[(LanguageType)i]);
                }
            }

            // ComboBox
            {
                comboBox_Language.SelectedIndex = (int)ConfigManager.GeneralConfig.Language;
                comboBox_ServiceLocation.SelectedIndex = ConfigManager.GeneralConfig.ServiceLocation;

                currencyConverterCombobox.SelectedItem = ConfigManager.GeneralConfig.DisplayCurrency;
            }
        }

        private void InitializeGeneralTab() {
            InitializeGeneralTabTranslations();
            InitializeGeneralTabCallbacks();
            InitializeGeneralTabFieldValuesReferences();
        }

        #endregion //Tab General

        #region Tab Devices

        private void InitializeDevicesTab() {
            InitializeDevicesCallbacks();
        }

        private void InitializeDevicesCallbacks() {
            devicesListViewEnableControl1.SetDeviceSelectionChangedCallback(devicesListView1_ItemSelectionChanged);
        }

        #endregion //Tab Devices


        #endregion // Initializations

        #region Form Callbacks

        #region Tab General
        private void GeneralCheckBoxes_CheckedChanged(object sender, EventArgs e) {
            if (!_isInitFinished) return;
            // indicate there has been a change
            IsChange = true;
            ConfigManager.GeneralConfig.DebugConsole = checkBox_DebugConsole.Checked;
            ConfigManager.GeneralConfig.AutoStartMining = checkBox_AutoStartMining.Checked;
            ConfigManager.GeneralConfig.HideMiningWindows = checkBox_HideMiningWindows.Checked;
            ConfigManager.GeneralConfig.MinimizeToTray = checkBox_MinimizeToTray.Checked;
            ConfigManager.GeneralConfig.DeviceDetection.DisableDetectionNVIDIA = checkBox_DisableDetectionNVIDIA.Checked;
            ConfigManager.GeneralConfig.DeviceDetection.DisableDetectionAMD = checkBox_DisableDetectionAMD.Checked;
            ConfigManager.GeneralConfig.AutoScaleBTCValues = checkBox_AutoScaleBTCValues.Checked;
            ConfigManager.GeneralConfig.StartMiningWhenIdle = checkBox_StartMiningWhenIdle.Checked;
            ConfigManager.GeneralConfig.ShowDriverVersionWarning = checkBox_ShowDriverVersionWarning.Checked;
            ConfigManager.GeneralConfig.DisableWindowsErrorReporting = checkBox_DisableWindowsErrorReporting.Checked;
            ConfigManager.GeneralConfig.NVIDIAP0State = checkBox_NVIDIAP0State.Checked;
            ConfigManager.GeneralConfig.LogToFile = checkBox_LogToFile.Checked;
            ConfigManager.GeneralConfig.IdleWhenNoInternetAccess = checkBox_IdleWhenNoInternetAccess.Checked;
            ConfigManager.GeneralConfig.AllowMultipleInstances = checkBox_AllowMultipleInstances.Checked;
        }

        private void checkBox_AMD_DisableAMDTempControl_CheckedChanged(object sender, EventArgs e) {
            if (!_isInitFinished) return;

            // indicate there has been a change
            IsChange = true;
            ConfigManager.GeneralConfig.DisableAMDTempControl = checkBox_AMD_DisableAMDTempControl.Checked;
            foreach (var cDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                if (cDev.DeviceType == DeviceType.AMD) {
                    foreach (var algorithm in cDev.GetAlgorithmSettings()) {
                        if (algorithm.NiceHashID != AlgorithmType.DaggerHashimoto) {
                            algorithm.ExtraLaunchParameters += AmdGpuDevice.TemperatureParam;
                            algorithm.ExtraLaunchParameters = ExtraLaunchParametersParser.ParseForMiningPair(
                                new MiningPair(cDev, algorithm), algorithm.NiceHashID, DeviceType.AMD, false);
                        }
                    }
                }
            }
        }

        private void checkBox_DisableDefaultOptimizations_CheckedChanged(object sender, EventArgs e) {
            if (!_isInitFinished) return;

            // indicate there has been a change
            IsChange = true;
            ConfigManager.GeneralConfig.DisableDefaultOptimizations = checkBox_DisableDefaultOptimizations.Checked;
            if (ConfigManager.GeneralConfig.DisableDefaultOptimizations) {
                foreach (var cDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                    foreach (var algorithm in cDev.GetAlgorithmSettings()) {
                        algorithm.ExtraLaunchParameters = "";
                        if (cDev.DeviceType == DeviceType.AMD && algorithm.NiceHashID != AlgorithmType.DaggerHashimoto) {
                            algorithm.ExtraLaunchParameters += AmdGpuDevice.TemperatureParam;
                            algorithm.ExtraLaunchParameters = ExtraLaunchParametersParser.ParseForMiningPair(
                                new MiningPair(cDev, algorithm), algorithm.NiceHashID, cDev.DeviceType, false);
                        }
                    }
                }
            } else {
                foreach (var cDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                    if (cDev.DeviceType == DeviceType.CPU) continue; // cpu has no defaults
                    var deviceDefaultsAlgoSettings = GroupAlgorithms.CreateForDeviceList(cDev);
                    foreach (var defaultAlgoSettings in deviceDefaultsAlgoSettings) {
                        var toSetAlgo = cDev.GetAlgorithm(defaultAlgoSettings.MinerBaseType, defaultAlgoSettings.NiceHashID, defaultAlgoSettings.SecondaryNiceHashID);
                        if (toSetAlgo != null) {
                            toSetAlgo.ExtraLaunchParameters = defaultAlgoSettings.ExtraLaunchParameters;
                            toSetAlgo.ExtraLaunchParameters = ExtraLaunchParametersParser.ParseForMiningPair(
                                new MiningPair(cDev, toSetAlgo), toSetAlgo.NiceHashID, cDev.DeviceType, false);
                        }
                    }
                }
            }
        }

        private void GeneralTextBoxes_Leave(object sender, EventArgs e) {
            if (!_isInitFinished) return;
            IsChange = true;
            ConfigManager.GeneralConfig.BitcoinAddress = textBox_BitcoinAddress.Text.Trim();
            ConfigManager.GeneralConfig.WorkerName = textBox_WorkerName.Text.Trim();

            ConfigManager.GeneralConfig.SwitchMinSecondsFixed = Helpers.ParseInt(textBox_SwitchMinSecondsFixed.Text);
            ConfigManager.GeneralConfig.SwitchMinSecondsDynamic = Helpers.ParseInt(textBox_SwitchMinSecondsDynamic.Text);
            ConfigManager.GeneralConfig.SwitchMinSecondsAMD = Helpers.ParseInt(textBox_SwitchMinSecondsAMD.Text);
            ConfigManager.GeneralConfig.MinerAPIQueryInterval = Helpers.ParseInt(textBox_MinerAPIQueryInterval.Text);
            ConfigManager.GeneralConfig.MinerRestartDelayMS = Helpers.ParseInt(textBox_MinerRestartDelayMS.Text);
            ConfigManager.GeneralConfig.MinIdleSeconds = Helpers.ParseInt(textBox_MinIdleSeconds.Text);
            ConfigManager.GeneralConfig.LogMaxFileSize = Helpers.ParseLong(textBox_LogMaxFileSize.Text);
            ConfigManager.GeneralConfig.ethminerDefaultBlockHeight = Helpers.ParseInt(textBox_ethminerDefaultBlockHeight.Text);
            ConfigManager.GeneralConfig.ApiBindPortPoolStart = Helpers.ParseInt(textBox_APIBindPortStart.Text);
            // min profit
            ConfigManager.GeneralConfig.MinimumProfit = Helpers.ParseDouble(textBox_MinProfit.Text);
            ConfigManager.GeneralConfig.SwitchProfitabilityThreshold = Helpers.ParseDouble(textBox_SwitchProfitabilityThreshold.Text);

            // Fix bounds
            ConfigManager.GeneralConfig.FixSettingBounds();
            // update strings
            textBox_MinProfit.Text = ConfigManager.GeneralConfig.MinimumProfit.ToString("F2").Replace(',', '.'); // force comma
            textBox_SwitchProfitabilityThreshold.Text = ConfigManager.GeneralConfig.SwitchProfitabilityThreshold.ToString("F2").Replace(',', '.'); // force comma
            textBox_SwitchMinSecondsFixed.Text = ConfigManager.GeneralConfig.SwitchMinSecondsFixed.ToString();
            textBox_SwitchMinSecondsDynamic.Text = ConfigManager.GeneralConfig.SwitchMinSecondsDynamic.ToString();
            textBox_SwitchMinSecondsAMD.Text = ConfigManager.GeneralConfig.SwitchMinSecondsAMD.ToString();
            textBox_MinerAPIQueryInterval.Text = ConfigManager.GeneralConfig.MinerAPIQueryInterval.ToString();
            textBox_MinerRestartDelayMS.Text = ConfigManager.GeneralConfig.MinerRestartDelayMS.ToString();
            textBox_MinIdleSeconds.Text = ConfigManager.GeneralConfig.MinIdleSeconds.ToString();
            textBox_LogMaxFileSize.Text = ConfigManager.GeneralConfig.LogMaxFileSize.ToString();
            textBox_ethminerDefaultBlockHeight.Text = ConfigManager.GeneralConfig.ethminerDefaultBlockHeight.ToString();
            textBox_APIBindPortStart.Text = ConfigManager.GeneralConfig.ApiBindPortPoolStart.ToString();
        }

        private void GeneralComboBoxes_Leave(object sender, EventArgs e) {
            if (!_isInitFinished) return;
            IsChange = true;
            ConfigManager.GeneralConfig.Language = (LanguageType)comboBox_Language.SelectedIndex;
            ConfigManager.GeneralConfig.ServiceLocation = comboBox_ServiceLocation.SelectedIndex;
            ConfigManager.GeneralConfig.EthminerDagGenerationType = (DagGenerationType)comboBox_DagLoadMode.SelectedIndex;
        }

        private void comboBox_CPU0_ForceCPUExtension_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox cmbbox = (ComboBox)sender;
            ConfigManager.GeneralConfig.ForceCPUExtension = (CPUExtensionType)cmbbox.SelectedIndex;
        }

        #endregion //Tab General


        #region Tab Device
        private void devicesListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {

            algorithmSettingsControl1.Deselect();
            // show algorithms
            _selectedComputeDevice = ComputeDeviceManager.Avaliable.GetCurrentlySelectedComputeDevice(e.ItemIndex, ShowUniqueDeviceList);
            algorithmsListView1.SetAlgorithms(_selectedComputeDevice, _selectedComputeDevice.Enabled);
            groupBoxAlgorithmSettings.Text = String.Format(International.GetText("FormSettings_AlgorithmsSettings"), _selectedComputeDevice.Name);
        }

        private void buttonSelectedProfit_Click(object sender, EventArgs e) {
            if (_selectedComputeDevice == null) {
                MessageBox.Show(International.GetText("FormSettings_ButtonProfitSingle"),
                                International.GetText("Warning_with_Exclamation"),
                                MessageBoxButtons.OK);
                return;
            }
            var url = Links.NHM_Profit_Check + _selectedComputeDevice.Name;
            foreach (var algorithm in _selectedComputeDevice.GetAlgorithmSettingsFastest()) {
                var id = (int)algorithm.NiceHashID;
                url += "&speed" + id + "=" + ProfitabilityCalculator.GetFormatedSpeed(algorithm.BenchmarkSpeed, algorithm.NiceHashID).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            }
            url += "&nhmver=" + Application.ProductVersion.ToString();  // Add version info
            url += "&cost=1&power=1"; // Set default power and cost to 1
            System.Diagnostics.Process.Start(url);
        }

        private void buttonAllProfit_Click(object sender, EventArgs e) {
            var url = Links.NHM_Profit_Check + "CUSTOM";
            Dictionary<AlgorithmType, double> total = new Dictionary<AlgorithmType,double>();
            foreach (var curCDev in ComputeDeviceManager.Avaliable.AllAvaliableDevices) {
                foreach (var algorithm in curCDev.GetAlgorithmSettingsFastest()) {
                    if (total.ContainsKey(algorithm.NiceHashID)) {
                        total[algorithm.NiceHashID] += algorithm.BenchmarkSpeed;
                    } else {
                        total[algorithm.NiceHashID] = algorithm.BenchmarkSpeed;
                    }
                }
            }
            foreach (var algorithm in total) {
                var id = (int)algorithm.Key;
                url += "&speed" + id + "=" + ProfitabilityCalculator.GetFormatedSpeed(algorithm.Value, algorithm.Key).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
            }
            url += "&nhmver=" + Application.ProductVersion.ToString();  // Add version info
            url += "&cost=1&power=1"; // Set default power and cost to 1
            System.Diagnostics.Process.Start(url);
        }

        #endregion //Tab Device


        private void toolTip1_Popup(object sender, PopupEventArgs e) {
            toolTip1.ToolTipTitle = International.GetText("Form_Settings_ToolTip_Explaination");
        }

        #region Form Buttons
        private void buttonDefaults_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show(International.GetText("Form_Settings_buttonDefaultsMsg"),
                                                  International.GetText("Form_Settings_buttonDefaultsTitle"),
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes) {
                IsChange = true;
                IsChangeSaved = true;
                ConfigManager.GeneralConfig.SetDefaults();

                International.Initialize(ConfigManager.GeneralConfig.Language);
                InitializeGeneralTabFieldValuesReferences();
                InitializeGeneralTabTranslations();
            }
        }

        private void buttonSaveClose_Click(object sender, EventArgs e) {
            MessageBox.Show(International.GetText("Form_Settings_buttonSaveMsg"),
                            International.GetText("Form_Settings_buttonSaveTitle"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            IsChange = true;
            IsChangeSaved = true;

            this.Close();
        }

        private void buttonCloseNoSave_Click(object sender, EventArgs e) {
            IsChangeSaved = false;
            this.Close();
        }
        #endregion // Form Buttons

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e) {
            if (IsChange && !IsChangeSaved) {
                DialogResult result = MessageBox.Show(International.GetText("Form_Settings_buttonCloseNoSaveMsg"),
                                                      International.GetText("Form_Settings_buttonCloseNoSaveTitle"),
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No) {
                    e.Cancel = true;
                    return;
                }
            }

            // check restart parameters change
            IsRestartNeeded = ConfigManager.IsRestartNeeded();

            if (IsChangeSaved) {
                ConfigManager.GeneralConfigFileCommit();
                ConfigManager.CommitBenchmarks();
                International.Initialize(ConfigManager.GeneralConfig.Language);
            } else {
                ConfigManager.RestoreBackup();
            }
        }

        private void currencyConverterCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            var Selected = currencyConverterCombobox.SelectedItem.ToString();
            ConfigManager.GeneralConfig.DisplayCurrency = Selected;
        }

        #endregion Form Callbacks

        private void tabControlGeneral_Selected(object sender, TabControlEventArgs e) {
            // set first device selected {
            if (ComputeDeviceManager.Avaliable.AllAvaliableDevices.Count > 0) {
                algorithmSettingsControl1.Deselect();
            }
        }

        private void checkBox_Use3rdPartyMiners_CheckedChanged(object sender, EventArgs e) {
            if (!_isInitFinished) return;
            if (this.checkBox_Use3rdPartyMiners.Checked) {
                // Show TOS
                Form tos = new Form_3rdParty_TOS();
                tos.ShowDialog(this);
                this.checkBox_Use3rdPartyMiners.Checked = ConfigManager.GeneralConfig.Use3rdPartyMiners == Use3rdPartyMiners.YES;
            } else {
                ConfigManager.GeneralConfig.Use3rdPartyMiners = Use3rdPartyMiners.NO;
            }
        }

    }
}
