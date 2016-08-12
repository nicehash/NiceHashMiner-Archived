using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner.Forms {
    public partial class FormSettings : Form {


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

        // most likely we wil have settings only per unique devices
        bool ShowUniqueDeviceList = true;

        // deep copy initial state if we want to discard changes
        private GeneralConfig _generalConfigBackup;
        private Dictionary<string, DeviceBenchmarkConfig> _benchmarkConfigsBackup;


        public FormSettings() {
            InitializeComponent();

            //ret = 1; // default
            IsChange = false;
            IsChangeSaved = false;

            _benchmarkConfigsBackup = MemoryHelper.DeepClone(ConfigManager.Instance.BenchmarkConfigs);
            _generalConfigBackup = MemoryHelper.DeepClone(ConfigManager.Instance.GeneralConfig);

            // initialize device lists, unique or every single one
            if (ShowUniqueDeviceList) {
                devicesListView1.SetComputeDevices(ComputeDevice.UniqueAvaliableDevices);
            } else {
                devicesListView1.SetComputeDevices(ComputeDevice.AllAvaliableDevices);
            }

            // initialize form
            InitializeFormTranslations();

            // Initialize toolTip
            InitializeToolTip();

            // Initialize tabs
            InitializeGeneralTab();

            // initialization calls 
            InitializeCallbacks();
            // link algorithm list with algorithm settings control
            algorithmSettingsControl1.Enabled = false;
            algorithmsListView1.ComunicationInterface = algorithmSettingsControl1;


            // At the very end set to true
            _isInitFinished = true;
        }

        private ComputeDevice GetCurrentlySelectedComputeDevice(int index) {
            // TODO index checking
            if (ShowUniqueDeviceList) {
                return ComputeDevice.UniqueAvaliableDevices[index];
            } else {
                return ComputeDevice.AllAvaliableDevices[index];
            }
        }

        #region Initializations

        private void InitializeToolTip() {
            // Setup Tooltips
            toolTip1.SetToolTip(this.comboBox_Language, International.GetText("Form_Settings_ToolTip_Language"));
            toolTip1.SetToolTip(this.label_Language, International.GetText("Form_Settings_ToolTip_Language"));
            toolTip1.SetToolTip(this.checkBox_DebugConsole, International.GetText("Form_Settings_ToolTip_checkBox_DebugConsole"));
            toolTip1.SetToolTip(this.textBox_BitcoinAddress, International.GetText("Form_Settings_ToolTip_BitcoinAddress"));
            toolTip1.SetToolTip(this.label_BitcoinAddress, International.GetText("Form_Settings_ToolTip_BitcoinAddress"));
            toolTip1.SetToolTip(this.textBox_WorkerName, International.GetText("Form_Settings_ToolTip_WorkerName"));
            toolTip1.SetToolTip(this.label_WorkerName, International.GetText("Form_Settings_ToolTip_WorkerName"));
            toolTip1.SetToolTip(this.comboBox_ServiceLocation, International.GetText("Form_Settings_ToolTip_ServiceLocation"));
            toolTip1.SetToolTip(this.label_ServiceLocation, International.GetText("Form_Settings_ToolTip_ServiceLocation"));
            toolTip1.SetToolTip(this.checkBox_AutoStartMining, International.GetText("Form_Settings_ToolTip_checkBox_AutoStartMining"));
            toolTip1.SetToolTip(this.checkBox_HideMiningWindows, International.GetText("Form_Settings_ToolTip_checkBox_HideMiningWindows"));
            toolTip1.SetToolTip(this.checkBox_MinimizeToTray, International.GetText("Form_Settings_ToolTip_checkBox_MinimizeToTray"));

            toolTip1.SetToolTip(this.textBox_SwitchMinSecondsFixed, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsFixed"));
            toolTip1.SetToolTip(this.label_SwitchMinSecondsFixed, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsFixed"));
            toolTip1.SetToolTip(this.textBox_SwitchMinSecondsDynamic, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsDynamic"));
            toolTip1.SetToolTip(this.label_SwitchMinSecondsDynamic, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsDynamic"));
            toolTip1.SetToolTip(this.textBox_SwitchMinSecondsAMD, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsAMD"));
            toolTip1.SetToolTip(this.label_SwitchMinSecondsAMD, International.GetText("Form_Settings_ToolTip_SwitchMinSecondsAMD"));

            toolTip1.SetToolTip(this.textBox_MinerAPIQueryInterval, International.GetText("Form_Settings_ToolTip_MinerAPIQueryInterval"));
            toolTip1.SetToolTip(this.label_MinerAPIQueryInterval, International.GetText("Form_Settings_ToolTip_MinerAPIQueryInterval"));
            toolTip1.SetToolTip(this.textBox_MinerRestartDelayMS, International.GetText("Form_Settings_ToolTip_MinerRestartDelayMS"));
            toolTip1.SetToolTip(this.label_MinerRestartDelayMS, International.GetText("Form_Settings_ToolTip_MinerRestartDelayMS"));
            toolTip1.SetToolTip(this.textBox_MinerAPIGraceSeconds, International.GetText("Form_Settings_ToolTip_MinerAPIGraceSeconds"));
            toolTip1.SetToolTip(this.label_MinerAPIGraceSeconds, International.GetText("Form_Settings_ToolTip_MinerAPIGraceSeconds"));
            toolTip1.SetToolTip(this.textBox_MinerAPIGraceSecondsAMD, International.GetText("Form_Settings_ToolTip_MinerAPIGraceSecondsAMD"));
            toolTip1.SetToolTip(this.label_MinerAPIGraceSecondsAMD, International.GetText("Form_Settings_ToolTip_MinerAPIGraceSecondsAMD"));

            benchmarkLimitControlCPU.SetToolTip(ref toolTip1, "CPUs");
            benchmarkLimitControlNVIDIA.SetToolTip(ref toolTip1, "NVIDIA GPUs");
            benchmarkLimitControlAMD.SetToolTip(ref toolTip1, "AMD GPUs");

            toolTip1.SetToolTip(this.checkBox_DisableDetectionNVidia5X, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "NVIDIA5.x"));
            toolTip1.SetToolTip(this.checkBox_DisableDetectionNVidia3X, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "NVIDIA3.x"));
            toolTip1.SetToolTip(this.checkBox_DisableDetectionNVidia2X, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "NVIDIA2.x"));
            toolTip1.SetToolTip(this.checkBox_DisableDetectionAMD, String.Format(International.GetText("Form_Settings_ToolTip_checkBox_DisableDetection"), "AMD"));

            toolTip1.SetToolTip(this.checkBox_AutoScaleBTCValues, International.GetText("Form_Settings_ToolTip_checkBox_AutoScaleBTCValues"));
            toolTip1.SetToolTip(this.checkBox_StartMiningWhenIdle, International.GetText("Form_Settings_ToolTip_checkBox_StartMiningWhenIdle"));

            toolTip1.SetToolTip(this.textBox_MinIdleSeconds, International.GetText("Form_Settings_ToolTip_MinIdleSeconds"));
            toolTip1.SetToolTip(this.label_MinIdleSeconds, International.GetText("Form_Settings_ToolTip_MinIdleSeconds"));
            toolTip1.SetToolTip(this.checkBox_LogToFile, International.GetText("Form_Settings_ToolTip_checkBox_LogToFile"));
            toolTip1.SetToolTip(this.textBox_LogMaxFileSize, International.GetText("Form_Settings_ToolTip_LogMaxFileSize"));
            toolTip1.SetToolTip(this.label_LogMaxFileSize, International.GetText("Form_Settings_ToolTip_LogMaxFileSize"));

            toolTip1.SetToolTip(this.checkBox_ShowDriverVersionWarning, International.GetText("Form_Settings_ToolTip_checkBox_ShowDriverVersionWarning"));
            toolTip1.SetToolTip(this.checkBox_DisableWindowsErrorReporting, International.GetText("Form_Settings_ToolTip_checkBox_DisableWindowsErrorReporting"));
            //toolTip1.SetToolTip(this.checkBox_UseNewSettingsPage, International.GetText("Form_Settings_ToolTip_checkBox_UseNewSettingsPage"));
            toolTip1.SetToolTip(this.checkBox_NVIDIAP0State, International.GetText("Form_Settings_ToolTip_checkBox_NVIDIAP0State"));
            //toolTip1.SetToolTip(this.textBox_ethminerAPIPortNvidia, String.Format(International.GetText("Form_Settings_ToolTip_ethminerAPIPort"), "NVIDIA"));
            //toolTip1.SetToolTip(this.label_ethminerAPIPortNvidia, String.Format(International.GetText("Form_Settings_ToolTip_ethminerAPIPort"), "NVIDIA"));
            //toolTip1.SetToolTip(this.textBox_ethminerAPIPortAMD, String.Format(International.GetText("Form_Settings_ToolTip_ethminerAPIPort"), "AMD"));
            //toolTip1.SetToolTip(this.label_ethminerAPIPortAMD, String.Format(International.GetText("Form_Settings_ToolTip_ethminerAPIPort"), "AMD"));
            toolTip1.SetToolTip(this.textBox_ethminerDefaultBlockHeight, International.GetText("Form_Settings_ToolTip_ethminerDefaultBlockHeight"));
            toolTip1.SetToolTip(this.label_ethminerDefaultBlockHeight, International.GetText("Form_Settings_ToolTip_ethminerDefaultBlockHeight"));
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
            checkBox_DisableDetectionNVidia5X.Text = String.Format(International.GetText("Form_Settings_General_DisableDetection"), "NVIDIA5.x");
            checkBox_DisableDetectionNVidia3X.Text = String.Format(International.GetText("Form_Settings_General_DisableDetection"), "NVIDIA3.x");
            checkBox_DisableDetectionNVidia2X.Text = String.Format(International.GetText("Form_Settings_General_DisableDetection"), "NVIDIA2.x");
            checkBox_DisableDetectionAMD.Text = String.Format(International.GetText("Form_Settings_General_DisableDetection"), "AMD");
            checkBox_AutoScaleBTCValues.Text = International.GetText("Form_Settings_General_AutoScaleBTCValues");
            checkBox_StartMiningWhenIdle.Text = International.GetText("Form_Settings_General_StartMiningWhenIdle");
            checkBox_ShowDriverVersionWarning.Text = International.GetText("Form_Settings_General_ShowDriverVersionWarning");
            checkBox_DisableWindowsErrorReporting.Text = International.GetText("Form_Settings_General_DisableWindowsErrorReporting");
            //checkBox_UseNewSettingsPage.Text = International.GetText("Form_Settings_General_UseNewSettingsPage");
            checkBox_NVIDIAP0State.Text = International.GetText("Form_Settings_General_NVIDIAP0State");
            checkBox_LogToFile.Text = International.GetText("Form_Settings_General_LogToFile");

            label_Language.Text = International.GetText("Form_Settings_General_Language") + ":";
            label_BitcoinAddress.Text = International.GetText("BitcoinAddress") + ":";
            label_WorkerName.Text = International.GetText("WorkerName") + ":";
            label_ServiceLocation.Text = International.GetText("Service_Location") + ":";
            label_MinIdleSeconds.Text = International.GetText("Form_Settings_General_MinIdleSeconds") + ":";
            label_MinerRestartDelayMS.Text = International.GetText("Form_Settings_General_MinerRestartDelayMS") + ":";
            label_MinerAPIGraceSeconds.Text = International.GetText("Form_Settings_General_MinerAPIGraceSeconds") + ":";
            label_MinerAPIGraceSecondsAMD.Text = International.GetText("Form_Settings_General_MinerAPIGraceSecondsAMD") + ":";
            label_MinerAPIQueryInterval.Text = International.GetText("Form_Settings_General_MinerAPIQueryInterval") + ":";
            label_LogMaxFileSize.Text = International.GetText("Form_Settings_General_LogMaxFileSize") + ":";

            label_SwitchMinSecondsFixed.Text = International.GetText("Form_Settings_General_SwitchMinSecondsFixed") + ":";
            label_SwitchMinSecondsDynamic.Text = International.GetText("Form_Settings_General_SwitchMinSecondsDynamic") + ":";
            label_SwitchMinSecondsAMD.Text = International.GetText("Form_Settings_General_SwitchMinSecondsAMD") + ":";

            label_ethminerDefaultBlockHeight.Text = International.GetText("Form_Settings_General_ethminerDefaultBlockHeight") + ":";
            //label_ethminerAPIPortNvidia.Text = International.GetText("Form_Settings_General_ethminerAPIPortNVIDIA") + ":";
            //label_ethminerAPIPortAMD.Text = International.GetText("Form_Settings_General_ethminerAPIPortAMD") + ":";

            displayCurrencyLabel.Text = International.GetText("Form_Settings_DisplayCurrency");

            // Benchmark time limits
            // TODO internationalization change
            groupBoxBenchmarkTimeLimits.Text = International.GetText("Form_Settings_General_BenchmarkTimeLimitsCPU_Group") + ":";
            benchmarkLimitControlCPU.GroupName = International.GetText("Form_Settings_General_BenchmarkTimeLimitsCPU_Group") + ":";
            benchmarkLimitControlNVIDIA.GroupName = International.GetText("Form_Settings_General_BenchmarkTimeLimitsNVIDIA_Group") + ":";
            benchmarkLimitControlAMD.GroupName = International.GetText("Form_Settings_General_BenchmarkTimeLimitsAMD_Group") + ":";
            // moved from constructor because of editor
            benchmarkLimitControlCPU.InitLocale();
            benchmarkLimitControlNVIDIA.InitLocale();
            benchmarkLimitControlAMD.InitLocale();

            // device enabled listview translation
            devicesListViewEnableControl1.InitLocale();
        }

        private void InitializeGeneralTabCallbacks() {
            // Add EventHandler for all the general tab's checkboxes
            {
                this.checkBox_AutoScaleBTCValues.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableDetectionAMD.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableDetectionNVidia2X.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableDetectionNVidia3X.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableDetectionNVidia5X.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_MinimizeToTray.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_HideMiningWindows.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_AutoStartMining.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DebugConsole.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_ShowDriverVersionWarning.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_DisableWindowsErrorReporting.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_StartMiningWhenIdle.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                //this.checkBox_UseNewSettingsPage.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_NVIDIAP0State.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
                this.checkBox_LogToFile.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
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
                this.textBox_MinerAPIGraceSeconds.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_MinerAPIGraceSecondsAMD.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_MinIdleSeconds.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_LogMaxFileSize.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsCPU_Quick.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsCPU_Standard.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsCPU_Precise.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsAMD_Quick.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsAMD_Standard.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_BenchmarkTimeLimitsAMD_Precise.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_ethminerAPIPortNvidia.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                //this.textBox_ethminerAPIPortAMD.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                this.textBox_ethminerDefaultBlockHeight.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
                // set int only keypress
                this.textBox_SwitchMinSecondsFixed.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_SwitchMinSecondsDynamic.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_SwitchMinSecondsAMD.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinerAPIQueryInterval.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinerRestartDelayMS.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinerAPIGraceSeconds.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinerAPIGraceSecondsAMD.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_MinIdleSeconds.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_LogMaxFileSize.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                //this.textBox_ethminerAPIPortNvidia.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                //this.textBox_ethminerAPIPortAMD.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
                this.textBox_ethminerDefaultBlockHeight.KeyPress += new KeyPressEventHandler(TextBoxKeyPressEvents.textBoxIntsOnly_KeyPress);
            }
            // Add EventHandler for all the general tab's textboxes
            {
                this.comboBox_Language.Leave += new System.EventHandler(this.GeneralComboBoxes_Leave);
                this.comboBox_ServiceLocation.Leave += new System.EventHandler(this.GeneralComboBoxes_Leave);
            }
        }

        private void InitializeGeneralTabFieldValuesReferences() {
            // Checkboxes set checked value
            {
                checkBox_DebugConsole.Checked = ConfigManager.Instance.GeneralConfig.DebugConsole;
                checkBox_AutoStartMining.Checked = ConfigManager.Instance.GeneralConfig.AutoStartMining;
                checkBox_HideMiningWindows.Checked = ConfigManager.Instance.GeneralConfig.HideMiningWindows;
                checkBox_MinimizeToTray.Checked = ConfigManager.Instance.GeneralConfig.MinimizeToTray;
                checkBox_DisableDetectionNVidia5X.Checked = ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia5X;
                checkBox_DisableDetectionNVidia3X.Checked = ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia3X;
                checkBox_DisableDetectionNVidia2X.Checked = ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia2X;
                checkBox_DisableDetectionAMD.Checked = ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionAMD;
                checkBox_AutoScaleBTCValues.Checked = ConfigManager.Instance.GeneralConfig.AutoScaleBTCValues;
                checkBox_StartMiningWhenIdle.Checked = ConfigManager.Instance.GeneralConfig.StartMiningWhenIdle;
                checkBox_ShowDriverVersionWarning.Checked = ConfigManager.Instance.GeneralConfig.ShowDriverVersionWarning;
                checkBox_DisableWindowsErrorReporting.Checked = ConfigManager.Instance.GeneralConfig.DisableWindowsErrorReporting;
                //checkBox_UseNewSettingsPage.Checked = ConfigManager.Instance.GeneralConfig.UseNewSettingsPage;
                checkBox_NVIDIAP0State.Checked = ConfigManager.Instance.GeneralConfig.NVIDIAP0State;
                checkBox_LogToFile.Checked = ConfigManager.Instance.GeneralConfig.LogToFile;
            }

            // Textboxes
            {
                textBox_BitcoinAddress.Text = ConfigManager.Instance.GeneralConfig.BitcoinAddress;
                textBox_WorkerName.Text = ConfigManager.Instance.GeneralConfig.WorkerName;
                textBox_SwitchMinSecondsFixed.Text = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed.ToString();
                textBox_SwitchMinSecondsDynamic.Text = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic.ToString();
                textBox_SwitchMinSecondsAMD.Text = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsAMD.ToString();
                textBox_MinerAPIQueryInterval.Text = ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval.ToString();
                textBox_MinerRestartDelayMS.Text = ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS.ToString();
                textBox_MinerAPIGraceSeconds.Text = ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds.ToString();
                textBox_MinerAPIGraceSecondsAMD.Text = ConfigManager.Instance.GeneralConfig.MinerAPIGraceSecondsAMD.ToString();
                textBox_MinIdleSeconds.Text = ConfigManager.Instance.GeneralConfig.MinIdleSeconds.ToString();
                textBox_LogMaxFileSize.Text = ConfigManager.Instance.GeneralConfig.LogMaxFileSize.ToString();
                //textBox_ethminerAPIPortNvidia.Text = ConfigManager.Instance.GeneralConfig.ethminerAPIPortNvidia.ToString();
                //textBox_ethminerAPIPortAMD.Text = ConfigManager.Instance.GeneralConfig.ethminerAPIPortAMD.ToString();
                textBox_ethminerDefaultBlockHeight.Text = ConfigManager.Instance.GeneralConfig.ethminerDefaultBlockHeight.ToString();
            }

            // set custom control referances
            {
                benchmarkLimitControlCPU.TimeLimits = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits.CPU;
                benchmarkLimitControlNVIDIA.TimeLimits = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits.NVIDIA;
                benchmarkLimitControlAMD.TimeLimits = ConfigManager.Instance.GeneralConfig.BenchmarkTimeLimits.AMD;

                // here we want all devices
                devicesListViewEnableControl1.SetComputeDevices(ComputeDevice.AllAvaliableDevices);
                devicesListViewEnableControl1.AutoSaveChange = false;
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
                comboBox_Language.SelectedIndex = (int)ConfigManager.Instance.GeneralConfig.Language;
                comboBox_ServiceLocation.SelectedIndex = ConfigManager.Instance.GeneralConfig.ServiceLocation;

                currencyConverterCombobox.SelectedItem = ConfigManager.Instance.GeneralConfig.DisplayCurrency;
            }
        }

        private void InitializeGeneralTab() {
            InitializeGeneralTabTranslations();
            InitializeGeneralTabCallbacks();
            InitializeGeneralTabFieldValuesReferences();
        }

        #endregion //Tab General

        private void InitializeCallbacks() {
            devicesListView1.SetDeviceSelectionChangedCallback(devicesListView1_ItemSelectionChanged);
        }

        #endregion // Initializations

        // TODO
        #region Evaluate to be removed
        private bool ParseStringToInt32(ref TextBox textBox) {
            int configInt; // dummy variable
            if (!Int32.TryParse(textBox.Text, out configInt)) {
                MessageBox.Show(International.GetText("Form_Settings_ParseIntMsg"),
                                International.GetText("Form_Settings_ParseIntTitle"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }

            return true;
        }

        private bool ParseStringToInt64(ref TextBox textBox) {
            long configInt; // dummy variable
            if (!Int64.TryParse(textBox.Text, out configInt)) {
                MessageBox.Show(International.GetText("Form_Settings_ParseIntMsg"),
                                International.GetText("Form_Settings_ParseIntTitle"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
                return false;
            }

            return true;
        }
        #endregion // Evaluate to be removed

        #region Form Callbacks

        #region Tab General
        private void GeneralCheckBoxes_CheckedChanged(object sender, EventArgs e) {
            // indicate there has been a change
            IsChange = true;
            ConfigManager.Instance.GeneralConfig.DebugConsole = checkBox_DebugConsole.Checked;
            ConfigManager.Instance.GeneralConfig.AutoStartMining = checkBox_AutoStartMining.Checked;
            ConfigManager.Instance.GeneralConfig.HideMiningWindows = checkBox_HideMiningWindows.Checked;
            ConfigManager.Instance.GeneralConfig.MinimizeToTray = checkBox_MinimizeToTray.Checked;
            ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia5X = checkBox_DisableDetectionNVidia5X.Checked;
            ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia3X = checkBox_DisableDetectionNVidia3X.Checked;
            ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionNVidia2X = checkBox_DisableDetectionNVidia2X.Checked;
            ConfigManager.Instance.GeneralConfig.DeviceDetection.DisableDetectionAMD = checkBox_DisableDetectionAMD.Checked;
            ConfigManager.Instance.GeneralConfig.AutoScaleBTCValues = checkBox_AutoScaleBTCValues.Checked;
            ConfigManager.Instance.GeneralConfig.StartMiningWhenIdle = checkBox_StartMiningWhenIdle.Checked;
            ConfigManager.Instance.GeneralConfig.ShowDriverVersionWarning = checkBox_ShowDriverVersionWarning.Checked;
            ConfigManager.Instance.GeneralConfig.DisableWindowsErrorReporting = checkBox_DisableWindowsErrorReporting.Checked;
            //ConfigManager.Instance.GeneralConfig.UseNewSettingsPage = checkBox_UseNewSettingsPage.Checked;
            ConfigManager.Instance.GeneralConfig.NVIDIAP0State = checkBox_NVIDIAP0State.Checked;
            ConfigManager.Instance.GeneralConfig.LogToFile = checkBox_LogToFile.Checked;
        }

        private void GeneralTextBoxes_Leave(object sender, EventArgs e) {
            IsChange = true;
            ConfigManager.Instance.GeneralConfig.BitcoinAddress = textBox_BitcoinAddress.Text.Trim();
            ConfigManager.Instance.GeneralConfig.WorkerName = textBox_WorkerName.Text.Trim();
            // TODO IMPORTANT fix this
            // int's only settings - keypress handles only ints should be safe. If string empty or null focus and alert
            // after number init set new value text back because it can be out of bounds
            // try to refactor this mess
            if (!ParseStringToInt32(ref textBox_SwitchMinSecondsFixed)) return;
            ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed = Int32.Parse(textBox_SwitchMinSecondsFixed.Text);
            textBox_SwitchMinSecondsFixed.Text = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsFixed.ToString();

            if (!ParseStringToInt32(ref textBox_SwitchMinSecondsDynamic)) return;
            ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic = Int32.Parse(textBox_SwitchMinSecondsDynamic.Text);
            textBox_SwitchMinSecondsDynamic.Text = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsDynamic.ToString();

            if (!ParseStringToInt32(ref textBox_SwitchMinSecondsAMD)) return;
            ConfigManager.Instance.GeneralConfig.SwitchMinSecondsAMD = Int32.Parse(textBox_SwitchMinSecondsAMD.Text);
            textBox_SwitchMinSecondsAMD.Text = ConfigManager.Instance.GeneralConfig.SwitchMinSecondsAMD.ToString();

            if (!ParseStringToInt32(ref textBox_MinerAPIQueryInterval)) return;
            ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval = Int32.Parse(textBox_MinerAPIQueryInterval.Text);
            textBox_MinerAPIQueryInterval.Text = ConfigManager.Instance.GeneralConfig.MinerAPIQueryInterval.ToString();

            if (!ParseStringToInt32(ref textBox_MinerRestartDelayMS)) return;
            ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS = Int32.Parse(textBox_MinerRestartDelayMS.Text);
            textBox_MinerRestartDelayMS.Text = ConfigManager.Instance.GeneralConfig.MinerRestartDelayMS.ToString();

            if (!ParseStringToInt32(ref textBox_MinerAPIGraceSeconds)) return;
            ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds = Int32.Parse(textBox_MinerAPIGraceSeconds.Text);
            textBox_MinerAPIGraceSeconds.Text = ConfigManager.Instance.GeneralConfig.MinerAPIGraceSeconds.ToString();

            if (!ParseStringToInt32(ref textBox_MinerAPIGraceSecondsAMD)) return;
            ConfigManager.Instance.GeneralConfig.MinerAPIGraceSecondsAMD = Int32.Parse(textBox_MinerAPIGraceSecondsAMD.Text);
            textBox_MinerAPIGraceSecondsAMD.Text = ConfigManager.Instance.GeneralConfig.MinerAPIGraceSecondsAMD.ToString();

            if (!ParseStringToInt32(ref textBox_MinIdleSeconds)) return;
            ConfigManager.Instance.GeneralConfig.MinIdleSeconds = Int32.Parse(textBox_MinIdleSeconds.Text);
            textBox_MinIdleSeconds.Text = ConfigManager.Instance.GeneralConfig.MinIdleSeconds.ToString();

            if (!ParseStringToInt64(ref textBox_LogMaxFileSize)) return;
            ConfigManager.Instance.GeneralConfig.LogMaxFileSize = Int64.Parse(textBox_LogMaxFileSize.Text);
            textBox_LogMaxFileSize.Text = ConfigManager.Instance.GeneralConfig.LogMaxFileSize.ToString();

            //if (!ParseStringToInt32(ref textBox_ethminerAPIPortNvidia)) return;
            //ConfigManager.Instance.GeneralConfig.ethminerAPIPortNvidia = Int32.Parse(textBox_ethminerAPIPortNvidia.Text);
            //textBox_ethminerAPIPortNvidia.Text = ConfigManager.Instance.GeneralConfig.ethminerAPIPortNvidia.ToString();

            //if (!ParseStringToInt32(ref textBox_ethminerAPIPortAMD)) return;
            //ConfigManager.Instance.GeneralConfig.ethminerAPIPortAMD = Int32.Parse(textBox_ethminerAPIPortAMD.Text);
            //textBox_ethminerAPIPortAMD.Text = ConfigManager.Instance.GeneralConfig.ethminerAPIPortAMD.ToString();
            
            if (!ParseStringToInt32(ref textBox_ethminerDefaultBlockHeight)) return;
            ConfigManager.Instance.GeneralConfig.ethminerDefaultBlockHeight = Int32.Parse(textBox_ethminerDefaultBlockHeight.Text);
            textBox_ethminerDefaultBlockHeight.Text = ConfigManager.Instance.GeneralConfig.ethminerDefaultBlockHeight.ToString();
        }

        private void GeneralComboBoxes_Leave(object sender, EventArgs e) {
            IsChange = true;
            ConfigManager.Instance.GeneralConfig.Language = (LanguageType)comboBox_Language.SelectedIndex;
            ConfigManager.Instance.GeneralConfig.ServiceLocation = comboBox_ServiceLocation.SelectedIndex;
        }

        #endregion //Tab General


        #region Tab Device
        // TODO indicate change
        private void devicesListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            // check if device settings enabled
            if (deviceSettingsControl1.Enabled == false) {
                deviceSettingsControl1.Enabled = true;
            }
            algorithmSettingsControl1.Deselect();
            // show algorithms
            var selectedComputeDevice = GetCurrentlySelectedComputeDevice(e.ItemIndex);
            deviceSettingsControl1.SelectedComputeDevice = selectedComputeDevice;
            algorithmsListView1.SetAlgorithms(
                DeviceBenchmarkConfigManager.Instance.GetConfig(
                selectedComputeDevice.DeviceGroupType,
                selectedComputeDevice.Name)
                );
        }

        #endregion //Tab Device


        private void toolTip1_Popup(object sender, PopupEventArgs e) {
            toolTip1.ToolTipTitle = International.GetText("Form_Settings_ToolTip_Explaination");
        }

        #region Form Buttons
        private void buttonDefaults_Click(object sender, EventArgs e) {
            // TODO change translation NHM will not restart
            DialogResult result = MessageBox.Show(International.GetText("Form_Settings_buttonDefaultsMsg"),
                                                  International.GetText("Form_Settings_buttonDefaultsTitle"),
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes) {
                IsChange = true;
                IsChangeSaved = true;
                ConfigManager.Instance.GeneralConfig.SetDefaults();

                // TODO reset International
                International.Initialize(ConfigManager.Instance.GeneralConfig.Language);
                InitializeGeneralTabFieldValuesReferences();
                InitializeGeneralTabTranslations();

                //// old stuff
                //Config.SetDefaults();
                //Config.Commit();
                //ret = 2;
                //this.Close();
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

            if (IsChangeSaved) {
                ConfigManager.Instance.GeneralConfig.Commit();
                ConfigManager.Instance.CommitBenchmarks();
                devicesListViewEnableControl1.SaveOptions();
                Config.Commit();
                International.Initialize(ConfigManager.Instance.GeneralConfig.Language);
            } else if (IsChange) {
                ConfigManager.Instance.GeneralConfig = _generalConfigBackup;
                ConfigManager.Instance.BenchmarkConfigs = _benchmarkConfigsBackup;
            }
        }

        private void currencyConverterCombobox_SelectedIndexChanged(object sender, EventArgs e) {
            //Helpers.ConsolePrint("CurrencyConverter", "Currency Set to: " + currencyConverterCombobox.SelectedItem);
            var Selected = currencyConverterCombobox.SelectedItem.ToString();
            ConfigManager.Instance.GeneralConfig.DisplayCurrency = Selected;
        }

        #endregion Form Callbacks
    }
}
