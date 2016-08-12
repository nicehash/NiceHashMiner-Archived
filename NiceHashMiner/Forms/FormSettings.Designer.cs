namespace NiceHashMiner.Forms {
    partial class FormSettings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.splitContainerTabControlButtons = new System.Windows.Forms.SplitContainer();
            this.tabControlGeneral = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxBenchmarkTimeLimits = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.benchmarkLimitControlCPU = new NiceHashMiner.Forms.Components.BenchmarkLimitControl();
            this.benchmarkLimitControlNVIDIA = new NiceHashMiner.Forms.Components.BenchmarkLimitControl();
            this.benchmarkLimitControlAMD = new NiceHashMiner.Forms.Components.BenchmarkLimitControl();
            this.displayCurrencyLabel = new System.Windows.Forms.Label();
            this.currencyConverterCombobox = new System.Windows.Forms.ComboBox();
            this.textBox_MinerAPIGraceSecondsAMD = new System.Windows.Forms.TextBox();
            this.textBox_SwitchMinSecondsAMD = new System.Windows.Forms.TextBox();
            this.textBox_ethminerDefaultBlockHeight = new System.Windows.Forms.TextBox();
            this.textBox_LogMaxFileSize = new System.Windows.Forms.TextBox();
            this.textBox_MinIdleSeconds = new System.Windows.Forms.TextBox();
            this.textBox_MinerAPIGraceSeconds = new System.Windows.Forms.TextBox();
            this.textBox_MinerRestartDelayMS = new System.Windows.Forms.TextBox();
            this.textBox_MinerAPIQueryInterval = new System.Windows.Forms.TextBox();
            this.textBox_SwitchMinSecondsDynamic = new System.Windows.Forms.TextBox();
            this.textBox_SwitchMinSecondsFixed = new System.Windows.Forms.TextBox();
            this.textBox_WorkerName = new System.Windows.Forms.TextBox();
            this.textBox_BitcoinAddress = new System.Windows.Forms.TextBox();
            this.label_MinerAPIGraceSecondsAMD = new System.Windows.Forms.Label();
            this.label_SwitchMinSecondsAMD = new System.Windows.Forms.Label();
            this.checkBox_LogToFile = new System.Windows.Forms.CheckBox();
            this.label_ethminerDefaultBlockHeight = new System.Windows.Forms.Label();
            this.label_Language = new System.Windows.Forms.Label();
            this.comboBox_Language = new System.Windows.Forms.ComboBox();
            this.checkBox_NVIDIAP0State = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableWindowsErrorReporting = new System.Windows.Forms.CheckBox();
            this.label_LogMaxFileSize = new System.Windows.Forms.Label();
            this.label_MinIdleSeconds = new System.Windows.Forms.Label();
            this.label_MinerAPIGraceSeconds = new System.Windows.Forms.Label();
            this.label_MinerRestartDelayMS = new System.Windows.Forms.Label();
            this.label_MinerAPIQueryInterval = new System.Windows.Forms.Label();
            this.label_SwitchMinSecondsDynamic = new System.Windows.Forms.Label();
            this.label_SwitchMinSecondsFixed = new System.Windows.Forms.Label();
            this.comboBox_ServiceLocation = new System.Windows.Forms.ComboBox();
            this.label_ServiceLocation = new System.Windows.Forms.Label();
            this.label_WorkerName = new System.Windows.Forms.Label();
            this.label_BitcoinAddress = new System.Windows.Forms.Label();
            this.checkBox_ShowDriverVersionWarning = new System.Windows.Forms.CheckBox();
            this.checkBox_StartMiningWhenIdle = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoScaleBTCValues = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionAMD = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionNVidia2X = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionNVidia3X = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionNVidia5X = new System.Windows.Forms.CheckBox();
            this.checkBox_MinimizeToTray = new System.Windows.Forms.CheckBox();
            this.checkBox_HideMiningWindows = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoStartMining = new System.Windows.Forms.CheckBox();
            this.checkBox_DebugConsole = new System.Windows.Forms.CheckBox();
            this.tabPageDevices = new System.Windows.Forms.TabPage();
            this.splitContainerDevicesSettings = new System.Windows.Forms.SplitContainer();
            this.devicesListView1 = new NiceHashMiner.Forms.Components.DevicesListView();
            this.deviceSettingsControl1 = new NiceHashMiner.Forms.Components.DeviceSettingsControl();
            this.label1 = new System.Windows.Forms.Label();
            this.algorithmsListView1 = new NiceHashMiner.Forms.Components.AlgorithmsListView();
            this.algorithmSettingsControl1 = new NiceHashMiner.Forms.Components.AlgorithmSettingsControl();
            this.buttonSaveClose = new System.Windows.Forms.Button();
            this.buttonDefaults = new System.Windows.Forms.Button();
            this.buttonCloseNoSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTabControlButtons)).BeginInit();
            this.splitContainerTabControlButtons.Panel1.SuspendLayout();
            this.splitContainerTabControlButtons.Panel2.SuspendLayout();
            this.splitContainerTabControlButtons.SuspendLayout();
            this.tabControlGeneral.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxBenchmarkTimeLimits.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPageDevices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDevicesSettings)).BeginInit();
            this.splitContainerDevicesSettings.Panel1.SuspendLayout();
            this.splitContainerDevicesSettings.Panel2.SuspendLayout();
            this.splitContainerDevicesSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerTabControlButtons
            // 
            this.splitContainerTabControlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTabControlButtons.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerTabControlButtons.IsSplitterFixed = true;
            this.splitContainerTabControlButtons.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTabControlButtons.Name = "splitContainerTabControlButtons";
            this.splitContainerTabControlButtons.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTabControlButtons.Panel1
            // 
            this.splitContainerTabControlButtons.Panel1.Controls.Add(this.tabControlGeneral);
            this.splitContainerTabControlButtons.Panel1.Padding = new System.Windows.Forms.Padding(6);
            // 
            // splitContainerTabControlButtons.Panel2
            // 
            this.splitContainerTabControlButtons.Panel2.Controls.Add(this.buttonSaveClose);
            this.splitContainerTabControlButtons.Panel2.Controls.Add(this.buttonDefaults);
            this.splitContainerTabControlButtons.Panel2.Controls.Add(this.buttonCloseNoSave);
            this.splitContainerTabControlButtons.Size = new System.Drawing.Size(986, 698);
            this.splitContainerTabControlButtons.SplitterDistance = 649;
            this.splitContainerTabControlButtons.TabIndex = 1;
            // 
            // tabControlGeneral
            // 
            this.tabControlGeneral.Controls.Add(this.tabPageGeneral);
            this.tabControlGeneral.Controls.Add(this.tabPageDevices);
            this.tabControlGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlGeneral.Location = new System.Drawing.Point(6, 6);
            this.tabControlGeneral.Name = "tabControlGeneral";
            this.tabControlGeneral.SelectedIndex = 0;
            this.tabControlGeneral.Size = new System.Drawing.Size(974, 637);
            this.tabControlGeneral.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.groupBoxBenchmarkTimeLimits);
            this.tabPageGeneral.Controls.Add(this.displayCurrencyLabel);
            this.tabPageGeneral.Controls.Add(this.currencyConverterCombobox);
            this.tabPageGeneral.Controls.Add(this.textBox_MinerAPIGraceSecondsAMD);
            this.tabPageGeneral.Controls.Add(this.textBox_SwitchMinSecondsAMD);
            this.tabPageGeneral.Controls.Add(this.textBox_ethminerDefaultBlockHeight);
            this.tabPageGeneral.Controls.Add(this.textBox_LogMaxFileSize);
            this.tabPageGeneral.Controls.Add(this.textBox_MinIdleSeconds);
            this.tabPageGeneral.Controls.Add(this.textBox_MinerAPIGraceSeconds);
            this.tabPageGeneral.Controls.Add(this.textBox_MinerRestartDelayMS);
            this.tabPageGeneral.Controls.Add(this.textBox_MinerAPIQueryInterval);
            this.tabPageGeneral.Controls.Add(this.textBox_SwitchMinSecondsDynamic);
            this.tabPageGeneral.Controls.Add(this.textBox_SwitchMinSecondsFixed);
            this.tabPageGeneral.Controls.Add(this.textBox_WorkerName);
            this.tabPageGeneral.Controls.Add(this.textBox_BitcoinAddress);
            this.tabPageGeneral.Controls.Add(this.label_MinerAPIGraceSecondsAMD);
            this.tabPageGeneral.Controls.Add(this.label_SwitchMinSecondsAMD);
            this.tabPageGeneral.Controls.Add(this.checkBox_LogToFile);
            this.tabPageGeneral.Controls.Add(this.label_ethminerDefaultBlockHeight);
            this.tabPageGeneral.Controls.Add(this.label_Language);
            this.tabPageGeneral.Controls.Add(this.comboBox_Language);
            this.tabPageGeneral.Controls.Add(this.checkBox_NVIDIAP0State);
            this.tabPageGeneral.Controls.Add(this.checkBox_DisableWindowsErrorReporting);
            this.tabPageGeneral.Controls.Add(this.label_LogMaxFileSize);
            this.tabPageGeneral.Controls.Add(this.label_MinIdleSeconds);
            this.tabPageGeneral.Controls.Add(this.label_MinerAPIGraceSeconds);
            this.tabPageGeneral.Controls.Add(this.label_MinerRestartDelayMS);
            this.tabPageGeneral.Controls.Add(this.label_MinerAPIQueryInterval);
            this.tabPageGeneral.Controls.Add(this.label_SwitchMinSecondsDynamic);
            this.tabPageGeneral.Controls.Add(this.label_SwitchMinSecondsFixed);
            this.tabPageGeneral.Controls.Add(this.comboBox_ServiceLocation);
            this.tabPageGeneral.Controls.Add(this.label_ServiceLocation);
            this.tabPageGeneral.Controls.Add(this.label_WorkerName);
            this.tabPageGeneral.Controls.Add(this.label_BitcoinAddress);
            this.tabPageGeneral.Controls.Add(this.checkBox_ShowDriverVersionWarning);
            this.tabPageGeneral.Controls.Add(this.checkBox_StartMiningWhenIdle);
            this.tabPageGeneral.Controls.Add(this.checkBox_AutoScaleBTCValues);
            this.tabPageGeneral.Controls.Add(this.checkBox_DisableDetectionAMD);
            this.tabPageGeneral.Controls.Add(this.checkBox_DisableDetectionNVidia2X);
            this.tabPageGeneral.Controls.Add(this.checkBox_DisableDetectionNVidia3X);
            this.tabPageGeneral.Controls.Add(this.checkBox_DisableDetectionNVidia5X);
            this.tabPageGeneral.Controls.Add(this.checkBox_MinimizeToTray);
            this.tabPageGeneral.Controls.Add(this.checkBox_HideMiningWindows);
            this.tabPageGeneral.Controls.Add(this.checkBox_AutoStartMining);
            this.tabPageGeneral.Controls.Add(this.checkBox_DebugConsole);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(966, 611);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBoxBenchmarkTimeLimits
            // 
            this.groupBoxBenchmarkTimeLimits.Controls.Add(this.flowLayoutPanel1);
            this.groupBoxBenchmarkTimeLimits.Location = new System.Drawing.Point(737, 17);
            this.groupBoxBenchmarkTimeLimits.Name = "groupBoxBenchmarkTimeLimits";
            this.groupBoxBenchmarkTimeLimits.Size = new System.Drawing.Size(223, 404);
            this.groupBoxBenchmarkTimeLimits.TabIndex = 383;
            this.groupBoxBenchmarkTimeLimits.TabStop = false;
            this.groupBoxBenchmarkTimeLimits.Text = "Benchmark Time Limits:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.benchmarkLimitControlCPU);
            this.flowLayoutPanel1.Controls.Add(this.benchmarkLimitControlNVIDIA);
            this.flowLayoutPanel1.Controls.Add(this.benchmarkLimitControlAMD);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(217, 385);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // benchmarkLimitControlCPU
            // 
            this.benchmarkLimitControlCPU.GroupName = "CPU";
            this.benchmarkLimitControlCPU.Location = new System.Drawing.Point(3, 3);
            this.benchmarkLimitControlCPU.Name = "benchmarkLimitControlCPU";
            this.benchmarkLimitControlCPU.Size = new System.Drawing.Size(213, 121);
            this.benchmarkLimitControlCPU.TabIndex = 0;
            this.benchmarkLimitControlCPU.TimeLimits = null;
            // 
            // benchmarkLimitControlNVIDIA
            // 
            this.benchmarkLimitControlNVIDIA.GroupName = "NVIDIA";
            this.benchmarkLimitControlNVIDIA.Location = new System.Drawing.Point(3, 130);
            this.benchmarkLimitControlNVIDIA.Name = "benchmarkLimitControlNVIDIA";
            this.benchmarkLimitControlNVIDIA.Size = new System.Drawing.Size(213, 121);
            this.benchmarkLimitControlNVIDIA.TabIndex = 1;
            this.benchmarkLimitControlNVIDIA.TimeLimits = null;
            // 
            // benchmarkLimitControlAMD
            // 
            this.benchmarkLimitControlAMD.GroupName = "AMD";
            this.benchmarkLimitControlAMD.Location = new System.Drawing.Point(3, 257);
            this.benchmarkLimitControlAMD.Name = "benchmarkLimitControlAMD";
            this.benchmarkLimitControlAMD.Size = new System.Drawing.Size(213, 121);
            this.benchmarkLimitControlAMD.TabIndex = 2;
            this.benchmarkLimitControlAMD.TimeLimits = null;
            // 
            // displayCurrencyLabel
            // 
            this.displayCurrencyLabel.AutoSize = true;
            this.displayCurrencyLabel.Location = new System.Drawing.Point(568, 17);
            this.displayCurrencyLabel.Name = "displayCurrencyLabel";
            this.displayCurrencyLabel.Size = new System.Drawing.Size(86, 13);
            this.displayCurrencyLabel.TabIndex = 382;
            this.displayCurrencyLabel.Text = "Display Currency";
            // 
            // currencyConverterCombobox
            // 
            this.currencyConverterCombobox.FormattingEnabled = true;
            this.currencyConverterCombobox.Items.AddRange(new object[] {
            "AUD",
            "BGN",
            "BRL",
            "CAD",
            "CHF",
            "CNY",
            "CZK",
            "DKK",
            "EUR",
            "GBP",
            "HKD",
            "HRK",
            "HUF",
            "IDR",
            "ILS",
            "INR",
            "JPY",
            "KRW",
            "MXN",
            "MYR",
            "NOK",
            "NZD",
            "PHP",
            "PLN",
            "RON",
            "RUB",
            "SEK",
            "SGD",
            "THB",
            "TRY",
            "USD",
            "ZAR"});
            this.currencyConverterCombobox.Location = new System.Drawing.Point(568, 37);
            this.currencyConverterCombobox.Name = "currencyConverterCombobox";
            this.currencyConverterCombobox.Size = new System.Drawing.Size(121, 21);
            this.currencyConverterCombobox.Sorted = true;
            this.currencyConverterCombobox.TabIndex = 381;
            this.currencyConverterCombobox.SelectedIndexChanged += new System.EventHandler(this.currencyConverterCombobox_SelectedIndexChanged);
            // 
            // textBox_MinerAPIGraceSecondsAMD
            // 
            this.textBox_MinerAPIGraceSecondsAMD.Location = new System.Drawing.Point(397, 185);
            this.textBox_MinerAPIGraceSecondsAMD.Name = "textBox_MinerAPIGraceSecondsAMD";
            this.textBox_MinerAPIGraceSecondsAMD.Size = new System.Drawing.Size(139, 20);
            this.textBox_MinerAPIGraceSecondsAMD.TabIndex = 336;
            // 
            // textBox_SwitchMinSecondsAMD
            // 
            this.textBox_SwitchMinSecondsAMD.Location = new System.Drawing.Point(568, 234);
            this.textBox_SwitchMinSecondsAMD.Name = "textBox_SwitchMinSecondsAMD";
            this.textBox_SwitchMinSecondsAMD.Size = new System.Drawing.Size(139, 20);
            this.textBox_SwitchMinSecondsAMD.TabIndex = 342;
            // 
            // textBox_ethminerDefaultBlockHeight
            // 
            this.textBox_ethminerDefaultBlockHeight.Location = new System.Drawing.Point(221, 283);
            this.textBox_ethminerDefaultBlockHeight.Name = "textBox_ethminerDefaultBlockHeight";
            this.textBox_ethminerDefaultBlockHeight.Size = new System.Drawing.Size(160, 20);
            this.textBox_ethminerDefaultBlockHeight.TabIndex = 333;
            // 
            // textBox_LogMaxFileSize
            // 
            this.textBox_LogMaxFileSize.Location = new System.Drawing.Point(221, 332);
            this.textBox_LogMaxFileSize.Name = "textBox_LogMaxFileSize";
            this.textBox_LogMaxFileSize.Size = new System.Drawing.Size(160, 20);
            this.textBox_LogMaxFileSize.TabIndex = 334;
            // 
            // textBox_MinIdleSeconds
            // 
            this.textBox_MinIdleSeconds.Location = new System.Drawing.Point(397, 136);
            this.textBox_MinIdleSeconds.Name = "textBox_MinIdleSeconds";
            this.textBox_MinIdleSeconds.Size = new System.Drawing.Size(140, 20);
            this.textBox_MinIdleSeconds.TabIndex = 335;
            // 
            // textBox_MinerAPIGraceSeconds
            // 
            this.textBox_MinerAPIGraceSeconds.Location = new System.Drawing.Point(221, 185);
            this.textBox_MinerAPIGraceSeconds.Name = "textBox_MinerAPIGraceSeconds";
            this.textBox_MinerAPIGraceSeconds.Size = new System.Drawing.Size(160, 20);
            this.textBox_MinerAPIGraceSeconds.TabIndex = 331;
            // 
            // textBox_MinerRestartDelayMS
            // 
            this.textBox_MinerRestartDelayMS.Location = new System.Drawing.Point(568, 136);
            this.textBox_MinerRestartDelayMS.Name = "textBox_MinerRestartDelayMS";
            this.textBox_MinerRestartDelayMS.Size = new System.Drawing.Size(139, 20);
            this.textBox_MinerRestartDelayMS.TabIndex = 340;
            // 
            // textBox_MinerAPIQueryInterval
            // 
            this.textBox_MinerAPIQueryInterval.Location = new System.Drawing.Point(568, 185);
            this.textBox_MinerAPIQueryInterval.Name = "textBox_MinerAPIQueryInterval";
            this.textBox_MinerAPIQueryInterval.Size = new System.Drawing.Size(140, 20);
            this.textBox_MinerAPIQueryInterval.TabIndex = 341;
            // 
            // textBox_SwitchMinSecondsDynamic
            // 
            this.textBox_SwitchMinSecondsDynamic.Location = new System.Drawing.Point(397, 234);
            this.textBox_SwitchMinSecondsDynamic.Name = "textBox_SwitchMinSecondsDynamic";
            this.textBox_SwitchMinSecondsDynamic.Size = new System.Drawing.Size(140, 20);
            this.textBox_SwitchMinSecondsDynamic.TabIndex = 337;
            // 
            // textBox_SwitchMinSecondsFixed
            // 
            this.textBox_SwitchMinSecondsFixed.Location = new System.Drawing.Point(220, 234);
            this.textBox_SwitchMinSecondsFixed.Name = "textBox_SwitchMinSecondsFixed";
            this.textBox_SwitchMinSecondsFixed.Size = new System.Drawing.Size(160, 20);
            this.textBox_SwitchMinSecondsFixed.TabIndex = 332;
            // 
            // textBox_WorkerName
            // 
            this.textBox_WorkerName.Location = new System.Drawing.Point(568, 87);
            this.textBox_WorkerName.Name = "textBox_WorkerName";
            this.textBox_WorkerName.Size = new System.Drawing.Size(139, 20);
            this.textBox_WorkerName.TabIndex = 339;
            // 
            // textBox_BitcoinAddress
            // 
            this.textBox_BitcoinAddress.Location = new System.Drawing.Point(221, 86);
            this.textBox_BitcoinAddress.Name = "textBox_BitcoinAddress";
            this.textBox_BitcoinAddress.Size = new System.Drawing.Size(316, 20);
            this.textBox_BitcoinAddress.TabIndex = 329;
            // 
            // label_MinerAPIGraceSecondsAMD
            // 
            this.label_MinerAPIGraceSecondsAMD.AutoSize = true;
            this.label_MinerAPIGraceSecondsAMD.Location = new System.Drawing.Point(394, 164);
            this.label_MinerAPIGraceSecondsAMD.Name = "label_MinerAPIGraceSecondsAMD";
            this.label_MinerAPIGraceSecondsAMD.Size = new System.Drawing.Size(148, 13);
            this.label_MinerAPIGraceSecondsAMD.TabIndex = 380;
            this.label_MinerAPIGraceSecondsAMD.Text = "MinerAPIGraceSecondsAMD:";
            // 
            // label_SwitchMinSecondsAMD
            // 
            this.label_SwitchMinSecondsAMD.AutoSize = true;
            this.label_SwitchMinSecondsAMD.Location = new System.Drawing.Point(565, 213);
            this.label_SwitchMinSecondsAMD.Name = "label_SwitchMinSecondsAMD";
            this.label_SwitchMinSecondsAMD.Size = new System.Drawing.Size(125, 13);
            this.label_SwitchMinSecondsAMD.TabIndex = 362;
            this.label_SwitchMinSecondsAMD.Text = "SwitchMinSecondsAMD:";
            // 
            // checkBox_LogToFile
            // 
            this.checkBox_LogToFile.AutoSize = true;
            this.checkBox_LogToFile.Location = new System.Drawing.Point(24, 324);
            this.checkBox_LogToFile.Name = "checkBox_LogToFile";
            this.checkBox_LogToFile.Size = new System.Drawing.Size(72, 17);
            this.checkBox_LogToFile.TabIndex = 327;
            this.checkBox_LogToFile.Text = "Log to file";
            this.checkBox_LogToFile.UseVisualStyleBackColor = true;
            // 
            // label_ethminerDefaultBlockHeight
            // 
            this.label_ethminerDefaultBlockHeight.AutoSize = true;
            this.label_ethminerDefaultBlockHeight.Location = new System.Drawing.Point(217, 262);
            this.label_ethminerDefaultBlockHeight.Name = "label_ethminerDefaultBlockHeight";
            this.label_ethminerDefaultBlockHeight.Size = new System.Drawing.Size(142, 13);
            this.label_ethminerDefaultBlockHeight.TabIndex = 361;
            this.label_ethminerDefaultBlockHeight.Text = "ethminerDefaultBlockHeight:";
            // 
            // label_Language
            // 
            this.label_Language.AutoSize = true;
            this.label_Language.Location = new System.Drawing.Point(217, 17);
            this.label_Language.Name = "label_Language";
            this.label_Language.Size = new System.Drawing.Size(58, 13);
            this.label_Language.TabIndex = 358;
            this.label_Language.Text = "Language:";
            // 
            // comboBox_Language
            // 
            this.comboBox_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Location = new System.Drawing.Point(221, 37);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(190, 21);
            this.comboBox_Language.TabIndex = 328;
            // 
            // checkBox_NVIDIAP0State
            // 
            this.checkBox_NVIDIAP0State.AutoSize = true;
            this.checkBox_NVIDIAP0State.Location = new System.Drawing.Point(24, 302);
            this.checkBox_NVIDIAP0State.Name = "checkBox_NVIDIAP0State";
            this.checkBox_NVIDIAP0State.Size = new System.Drawing.Size(100, 17);
            this.checkBox_NVIDIAP0State.TabIndex = 326;
            this.checkBox_NVIDIAP0State.Text = "NVIDIAP0State";
            this.checkBox_NVIDIAP0State.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableWindowsErrorReporting
            // 
            this.checkBox_DisableWindowsErrorReporting.AutoSize = true;
            this.checkBox_DisableWindowsErrorReporting.Location = new System.Drawing.Point(24, 258);
            this.checkBox_DisableWindowsErrorReporting.Name = "checkBox_DisableWindowsErrorReporting";
            this.checkBox_DisableWindowsErrorReporting.Size = new System.Drawing.Size(173, 17);
            this.checkBox_DisableWindowsErrorReporting.TabIndex = 324;
            this.checkBox_DisableWindowsErrorReporting.Text = "DisableWindowsErrorReporting";
            this.checkBox_DisableWindowsErrorReporting.UseVisualStyleBackColor = true;
            // 
            // label_LogMaxFileSize
            // 
            this.label_LogMaxFileSize.AutoSize = true;
            this.label_LogMaxFileSize.Location = new System.Drawing.Point(217, 311);
            this.label_LogMaxFileSize.Name = "label_LogMaxFileSize";
            this.label_LogMaxFileSize.Size = new System.Drawing.Size(84, 13);
            this.label_LogMaxFileSize.TabIndex = 357;
            this.label_LogMaxFileSize.Text = "LogMaxFileSize:";
            // 
            // label_MinIdleSeconds
            // 
            this.label_MinIdleSeconds.AutoSize = true;
            this.label_MinIdleSeconds.Location = new System.Drawing.Point(394, 115);
            this.label_MinIdleSeconds.Name = "label_MinIdleSeconds";
            this.label_MinIdleSeconds.Size = new System.Drawing.Size(86, 13);
            this.label_MinIdleSeconds.TabIndex = 356;
            this.label_MinIdleSeconds.Text = "MinIdleSeconds:";
            // 
            // label_MinerAPIGraceSeconds
            // 
            this.label_MinerAPIGraceSeconds.AutoSize = true;
            this.label_MinerAPIGraceSeconds.Location = new System.Drawing.Point(218, 164);
            this.label_MinerAPIGraceSeconds.Name = "label_MinerAPIGraceSeconds";
            this.label_MinerAPIGraceSeconds.Size = new System.Drawing.Size(124, 13);
            this.label_MinerAPIGraceSeconds.TabIndex = 379;
            this.label_MinerAPIGraceSeconds.Text = "MinerAPIGraceSeconds:";
            // 
            // label_MinerRestartDelayMS
            // 
            this.label_MinerRestartDelayMS.AutoSize = true;
            this.label_MinerRestartDelayMS.Location = new System.Drawing.Point(565, 115);
            this.label_MinerRestartDelayMS.Name = "label_MinerRestartDelayMS";
            this.label_MinerRestartDelayMS.Size = new System.Drawing.Size(113, 13);
            this.label_MinerRestartDelayMS.TabIndex = 375;
            this.label_MinerRestartDelayMS.Text = "MinerRestartDelayMS:";
            // 
            // label_MinerAPIQueryInterval
            // 
            this.label_MinerAPIQueryInterval.AutoSize = true;
            this.label_MinerAPIQueryInterval.Location = new System.Drawing.Point(565, 164);
            this.label_MinerAPIQueryInterval.Name = "label_MinerAPIQueryInterval";
            this.label_MinerAPIQueryInterval.Size = new System.Drawing.Size(116, 13);
            this.label_MinerAPIQueryInterval.TabIndex = 376;
            this.label_MinerAPIQueryInterval.Text = "MinerAPIQueryInterval:";
            // 
            // label_SwitchMinSecondsDynamic
            // 
            this.label_SwitchMinSecondsDynamic.AutoSize = true;
            this.label_SwitchMinSecondsDynamic.Location = new System.Drawing.Point(394, 213);
            this.label_SwitchMinSecondsDynamic.Name = "label_SwitchMinSecondsDynamic";
            this.label_SwitchMinSecondsDynamic.Size = new System.Drawing.Size(142, 13);
            this.label_SwitchMinSecondsDynamic.TabIndex = 378;
            this.label_SwitchMinSecondsDynamic.Text = "SwitchMinSecondsDynamic:";
            // 
            // label_SwitchMinSecondsFixed
            // 
            this.label_SwitchMinSecondsFixed.AutoSize = true;
            this.label_SwitchMinSecondsFixed.Location = new System.Drawing.Point(217, 213);
            this.label_SwitchMinSecondsFixed.Name = "label_SwitchMinSecondsFixed";
            this.label_SwitchMinSecondsFixed.Size = new System.Drawing.Size(126, 13);
            this.label_SwitchMinSecondsFixed.TabIndex = 366;
            this.label_SwitchMinSecondsFixed.Text = "SwitchMinSecondsFixed:";
            // 
            // comboBox_ServiceLocation
            // 
            this.comboBox_ServiceLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ServiceLocation.FormattingEnabled = true;
            this.comboBox_ServiceLocation.Items.AddRange(new object[] {
            "Europe - Amsterdam",
            "USA - San Jose",
            "China - Hong Kong",
            "Japan - Tokyo"});
            this.comboBox_ServiceLocation.Location = new System.Drawing.Point(220, 136);
            this.comboBox_ServiceLocation.Name = "comboBox_ServiceLocation";
            this.comboBox_ServiceLocation.Size = new System.Drawing.Size(160, 21);
            this.comboBox_ServiceLocation.TabIndex = 330;
            // 
            // label_ServiceLocation
            // 
            this.label_ServiceLocation.AutoSize = true;
            this.label_ServiceLocation.Location = new System.Drawing.Point(217, 115);
            this.label_ServiceLocation.Name = "label_ServiceLocation";
            this.label_ServiceLocation.Size = new System.Drawing.Size(87, 13);
            this.label_ServiceLocation.TabIndex = 363;
            this.label_ServiceLocation.Text = "ServiceLocation:";
            // 
            // label_WorkerName
            // 
            this.label_WorkerName.AutoSize = true;
            this.label_WorkerName.Location = new System.Drawing.Point(565, 66);
            this.label_WorkerName.Name = "label_WorkerName";
            this.label_WorkerName.Size = new System.Drawing.Size(73, 13);
            this.label_WorkerName.TabIndex = 354;
            this.label_WorkerName.Text = "WorkerName:";
            // 
            // label_BitcoinAddress
            // 
            this.label_BitcoinAddress.AutoSize = true;
            this.label_BitcoinAddress.Location = new System.Drawing.Point(218, 66);
            this.label_BitcoinAddress.Name = "label_BitcoinAddress";
            this.label_BitcoinAddress.Size = new System.Drawing.Size(80, 13);
            this.label_BitcoinAddress.TabIndex = 355;
            this.label_BitcoinAddress.Text = "BitcoinAddress:";
            // 
            // checkBox_ShowDriverVersionWarning
            // 
            this.checkBox_ShowDriverVersionWarning.AutoSize = true;
            this.checkBox_ShowDriverVersionWarning.Location = new System.Drawing.Point(24, 236);
            this.checkBox_ShowDriverVersionWarning.Name = "checkBox_ShowDriverVersionWarning";
            this.checkBox_ShowDriverVersionWarning.Size = new System.Drawing.Size(156, 17);
            this.checkBox_ShowDriverVersionWarning.TabIndex = 323;
            this.checkBox_ShowDriverVersionWarning.Text = "ShowDriverVersionWarning";
            this.checkBox_ShowDriverVersionWarning.UseVisualStyleBackColor = true;
            // 
            // checkBox_StartMiningWhenIdle
            // 
            this.checkBox_StartMiningWhenIdle.AutoSize = true;
            this.checkBox_StartMiningWhenIdle.Location = new System.Drawing.Point(25, 214);
            this.checkBox_StartMiningWhenIdle.Name = "checkBox_StartMiningWhenIdle";
            this.checkBox_StartMiningWhenIdle.Size = new System.Drawing.Size(125, 17);
            this.checkBox_StartMiningWhenIdle.TabIndex = 322;
            this.checkBox_StartMiningWhenIdle.Text = "StartMiningWhenIdle";
            this.checkBox_StartMiningWhenIdle.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoScaleBTCValues
            // 
            this.checkBox_AutoScaleBTCValues.AutoSize = true;
            this.checkBox_AutoScaleBTCValues.Location = new System.Drawing.Point(25, 192);
            this.checkBox_AutoScaleBTCValues.Name = "checkBox_AutoScaleBTCValues";
            this.checkBox_AutoScaleBTCValues.Size = new System.Drawing.Size(128, 17);
            this.checkBox_AutoScaleBTCValues.TabIndex = 321;
            this.checkBox_AutoScaleBTCValues.Text = "AutoScaleBTCValues";
            this.checkBox_AutoScaleBTCValues.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionAMD
            // 
            this.checkBox_DisableDetectionAMD.AutoSize = true;
            this.checkBox_DisableDetectionAMD.Location = new System.Drawing.Point(25, 170);
            this.checkBox_DisableDetectionAMD.Name = "checkBox_DisableDetectionAMD";
            this.checkBox_DisableDetectionAMD.Size = new System.Drawing.Size(131, 17);
            this.checkBox_DisableDetectionAMD.TabIndex = 320;
            this.checkBox_DisableDetectionAMD.Text = "DisableDetectionAMD";
            this.checkBox_DisableDetectionAMD.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionNVidia2X
            // 
            this.checkBox_DisableDetectionNVidia2X.AutoSize = true;
            this.checkBox_DisableDetectionNVidia2X.Location = new System.Drawing.Point(25, 148);
            this.checkBox_DisableDetectionNVidia2X.Name = "checkBox_DisableDetectionNVidia2X";
            this.checkBox_DisableDetectionNVidia2X.Size = new System.Drawing.Size(151, 17);
            this.checkBox_DisableDetectionNVidia2X.TabIndex = 319;
            this.checkBox_DisableDetectionNVidia2X.Text = "DisableDetectionNVidia2X";
            this.checkBox_DisableDetectionNVidia2X.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionNVidia3X
            // 
            this.checkBox_DisableDetectionNVidia3X.AutoSize = true;
            this.checkBox_DisableDetectionNVidia3X.Location = new System.Drawing.Point(25, 126);
            this.checkBox_DisableDetectionNVidia3X.Name = "checkBox_DisableDetectionNVidia3X";
            this.checkBox_DisableDetectionNVidia3X.Size = new System.Drawing.Size(151, 17);
            this.checkBox_DisableDetectionNVidia3X.TabIndex = 318;
            this.checkBox_DisableDetectionNVidia3X.Text = "DisableDetectionNVidia3X";
            this.checkBox_DisableDetectionNVidia3X.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionNVidia5X
            // 
            this.checkBox_DisableDetectionNVidia5X.AutoSize = true;
            this.checkBox_DisableDetectionNVidia5X.Location = new System.Drawing.Point(25, 104);
            this.checkBox_DisableDetectionNVidia5X.Name = "checkBox_DisableDetectionNVidia5X";
            this.checkBox_DisableDetectionNVidia5X.Size = new System.Drawing.Size(151, 17);
            this.checkBox_DisableDetectionNVidia5X.TabIndex = 317;
            this.checkBox_DisableDetectionNVidia5X.Text = "DisableDetectionNVidia5X";
            this.checkBox_DisableDetectionNVidia5X.UseVisualStyleBackColor = true;
            // 
            // checkBox_MinimizeToTray
            // 
            this.checkBox_MinimizeToTray.AutoSize = true;
            this.checkBox_MinimizeToTray.Location = new System.Drawing.Point(25, 82);
            this.checkBox_MinimizeToTray.Name = "checkBox_MinimizeToTray";
            this.checkBox_MinimizeToTray.Size = new System.Drawing.Size(100, 17);
            this.checkBox_MinimizeToTray.TabIndex = 316;
            this.checkBox_MinimizeToTray.Text = "MinimizeToTray";
            this.checkBox_MinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // checkBox_HideMiningWindows
            // 
            this.checkBox_HideMiningWindows.AutoSize = true;
            this.checkBox_HideMiningWindows.Location = new System.Drawing.Point(25, 60);
            this.checkBox_HideMiningWindows.Name = "checkBox_HideMiningWindows";
            this.checkBox_HideMiningWindows.Size = new System.Drawing.Size(123, 17);
            this.checkBox_HideMiningWindows.TabIndex = 315;
            this.checkBox_HideMiningWindows.Text = "HideMiningWindows";
            this.checkBox_HideMiningWindows.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoStartMining
            // 
            this.checkBox_AutoStartMining.AutoSize = true;
            this.checkBox_AutoStartMining.Location = new System.Drawing.Point(25, 38);
            this.checkBox_AutoStartMining.Name = "checkBox_AutoStartMining";
            this.checkBox_AutoStartMining.Size = new System.Drawing.Size(101, 17);
            this.checkBox_AutoStartMining.TabIndex = 314;
            this.checkBox_AutoStartMining.Text = "AutoStartMining";
            this.checkBox_AutoStartMining.UseVisualStyleBackColor = true;
            // 
            // checkBox_DebugConsole
            // 
            this.checkBox_DebugConsole.AutoSize = true;
            this.checkBox_DebugConsole.Location = new System.Drawing.Point(25, 16);
            this.checkBox_DebugConsole.Name = "checkBox_DebugConsole";
            this.checkBox_DebugConsole.Size = new System.Drawing.Size(96, 17);
            this.checkBox_DebugConsole.TabIndex = 313;
            this.checkBox_DebugConsole.Text = "DebugConsole";
            this.checkBox_DebugConsole.UseVisualStyleBackColor = true;
            // 
            // tabPageDevices
            // 
            this.tabPageDevices.Controls.Add(this.splitContainerDevicesSettings);
            this.tabPageDevices.Location = new System.Drawing.Point(4, 22);
            this.tabPageDevices.Name = "tabPageDevices";
            this.tabPageDevices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDevices.Size = new System.Drawing.Size(966, 611);
            this.tabPageDevices.TabIndex = 1;
            this.tabPageDevices.Text = "Devices";
            this.tabPageDevices.UseVisualStyleBackColor = true;
            // 
            // splitContainerDevicesSettings
            // 
            this.splitContainerDevicesSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainerDevicesSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDevicesSettings.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerDevicesSettings.IsSplitterFixed = true;
            this.splitContainerDevicesSettings.Location = new System.Drawing.Point(3, 3);
            this.splitContainerDevicesSettings.Name = "splitContainerDevicesSettings";
            // 
            // splitContainerDevicesSettings.Panel1
            // 
            this.splitContainerDevicesSettings.Panel1.Controls.Add(this.devicesListView1);
            this.splitContainerDevicesSettings.Panel1.Controls.Add(this.deviceSettingsControl1);
            // 
            // splitContainerDevicesSettings.Panel2
            // 
            this.splitContainerDevicesSettings.Panel2.Controls.Add(this.label1);
            this.splitContainerDevicesSettings.Panel2.Controls.Add(this.algorithmsListView1);
            this.splitContainerDevicesSettings.Panel2.Controls.Add(this.algorithmSettingsControl1);
            this.splitContainerDevicesSettings.Size = new System.Drawing.Size(960, 605);
            this.splitContainerDevicesSettings.SplitterDistance = 488;
            this.splitContainerDevicesSettings.TabIndex = 0;
            // 
            // devicesListView1
            // 
            this.devicesListView1.Location = new System.Drawing.Point(3, 40);
            this.devicesListView1.Name = "devicesListView1";
            this.devicesListView1.Size = new System.Drawing.Size(452, 126);
            this.devicesListView1.TabIndex = 1;
            // 
            // deviceSettingsControl1
            // 
            this.deviceSettingsControl1.Enabled = false;
            this.deviceSettingsControl1.Location = new System.Drawing.Point(3, 172);
            this.deviceSettingsControl1.Name = "deviceSettingsControl1";
            this.deviceSettingsControl1.SelectedComputeDevice = null;
            this.deviceSettingsControl1.Size = new System.Drawing.Size(452, 400);
            this.deviceSettingsControl1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Algorithm settings for selected device:";
            // 
            // algorithmsListView1
            // 
            this.algorithmsListView1.ComunicationInterface = null;
            this.algorithmsListView1.Location = new System.Drawing.Point(6, 34);
            this.algorithmsListView1.Name = "algorithmsListView1";
            this.algorithmsListView1.Size = new System.Drawing.Size(455, 287);
            this.algorithmsListView1.TabIndex = 2;
            // 
            // algorithmSettingsControl1
            // 
            this.algorithmSettingsControl1.Location = new System.Drawing.Point(48, 327);
            this.algorithmSettingsControl1.Name = "algorithmSettingsControl1";
            this.algorithmSettingsControl1.Size = new System.Drawing.Size(413, 245);
            this.algorithmSettingsControl1.TabIndex = 1;
            // 
            // buttonSaveClose
            // 
            this.buttonSaveClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveClose.Location = new System.Drawing.Point(686, 10);
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.buttonSaveClose.Size = new System.Drawing.Size(110, 23);
            this.buttonSaveClose.TabIndex = 44;
            this.buttonSaveClose.Text = "&Save and Close";
            this.buttonSaveClose.UseVisualStyleBackColor = true;
            this.buttonSaveClose.Click += new System.EventHandler(this.buttonSaveClose_Click);
            // 
            // buttonDefaults
            // 
            this.buttonDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDefaults.Location = new System.Drawing.Point(605, 10);
            this.buttonDefaults.Name = "buttonDefaults";
            this.buttonDefaults.Size = new System.Drawing.Size(75, 23);
            this.buttonDefaults.TabIndex = 43;
            this.buttonDefaults.Text = "&Defaults";
            this.buttonDefaults.UseVisualStyleBackColor = true;
            this.buttonDefaults.Click += new System.EventHandler(this.buttonDefaults_Click);
            // 
            // buttonCloseNoSave
            // 
            this.buttonCloseNoSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCloseNoSave.Location = new System.Drawing.Point(802, 10);
            this.buttonCloseNoSave.Name = "buttonCloseNoSave";
            this.buttonCloseNoSave.Size = new System.Drawing.Size(135, 23);
            this.buttonCloseNoSave.TabIndex = 45;
            this.buttonCloseNoSave.Text = "&Close without Saving";
            this.buttonCloseNoSave.UseVisualStyleBackColor = true;
            this.buttonCloseNoSave.Click += new System.EventHandler(this.buttonCloseNoSave_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 698);
            this.Controls.Add(this.splitContainerTabControlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.splitContainerTabControlButtons.Panel1.ResumeLayout(false);
            this.splitContainerTabControlButtons.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTabControlButtons)).EndInit();
            this.splitContainerTabControlButtons.ResumeLayout(false);
            this.tabControlGeneral.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.groupBoxBenchmarkTimeLimits.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabPageDevices.ResumeLayout(false);
            this.splitContainerDevicesSettings.Panel1.ResumeLayout(false);
            this.splitContainerDevicesSettings.Panel2.ResumeLayout(false);
            this.splitContainerDevicesSettings.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDevicesSettings)).EndInit();
            this.splitContainerDevicesSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerTabControlButtons;
        private System.Windows.Forms.Button buttonSaveClose;
        private System.Windows.Forms.Button buttonDefaults;
        private System.Windows.Forms.Button buttonCloseNoSave;
        private System.Windows.Forms.TabControl tabControlGeneral;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.Label displayCurrencyLabel;
        private System.Windows.Forms.ComboBox currencyConverterCombobox;
        private System.Windows.Forms.TextBox textBox_MinerAPIGraceSecondsAMD;
        private System.Windows.Forms.TextBox textBox_SwitchMinSecondsAMD;
        private System.Windows.Forms.TextBox textBox_ethminerDefaultBlockHeight;
        private System.Windows.Forms.TextBox textBox_LogMaxFileSize;
        private System.Windows.Forms.TextBox textBox_MinIdleSeconds;
        private System.Windows.Forms.TextBox textBox_MinerAPIGraceSeconds;
        private System.Windows.Forms.TextBox textBox_MinerRestartDelayMS;
        private System.Windows.Forms.TextBox textBox_MinerAPIQueryInterval;
        private System.Windows.Forms.TextBox textBox_SwitchMinSecondsDynamic;
        private System.Windows.Forms.TextBox textBox_SwitchMinSecondsFixed;
        private System.Windows.Forms.TextBox textBox_WorkerName;
        private System.Windows.Forms.TextBox textBox_BitcoinAddress;
        private System.Windows.Forms.Label label_MinerAPIGraceSecondsAMD;
        private System.Windows.Forms.Label label_SwitchMinSecondsAMD;
        private System.Windows.Forms.CheckBox checkBox_LogToFile;
        private System.Windows.Forms.Label label_ethminerDefaultBlockHeight;
        private System.Windows.Forms.Label label_Language;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.CheckBox checkBox_NVIDIAP0State;
        private System.Windows.Forms.CheckBox checkBox_DisableWindowsErrorReporting;
        private System.Windows.Forms.Label label_LogMaxFileSize;
        private System.Windows.Forms.Label label_MinIdleSeconds;
        private System.Windows.Forms.Label label_MinerAPIGraceSeconds;
        private System.Windows.Forms.Label label_MinerRestartDelayMS;
        private System.Windows.Forms.Label label_MinerAPIQueryInterval;
        private System.Windows.Forms.Label label_SwitchMinSecondsDynamic;
        private System.Windows.Forms.Label label_SwitchMinSecondsFixed;
        private System.Windows.Forms.ComboBox comboBox_ServiceLocation;
        private System.Windows.Forms.Label label_ServiceLocation;
        private System.Windows.Forms.Label label_WorkerName;
        private System.Windows.Forms.Label label_BitcoinAddress;
        private System.Windows.Forms.CheckBox checkBox_ShowDriverVersionWarning;
        private System.Windows.Forms.CheckBox checkBox_StartMiningWhenIdle;
        private System.Windows.Forms.CheckBox checkBox_AutoScaleBTCValues;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionAMD;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionNVidia2X;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionNVidia3X;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionNVidia5X;
        private System.Windows.Forms.CheckBox checkBox_MinimizeToTray;
        private System.Windows.Forms.CheckBox checkBox_HideMiningWindows;
        private System.Windows.Forms.CheckBox checkBox_AutoStartMining;
        private System.Windows.Forms.CheckBox checkBox_DebugConsole;
        private System.Windows.Forms.TabPage tabPageDevices;
        private System.Windows.Forms.SplitContainer splitContainerDevicesSettings;
        private Components.DeviceSettingsControl deviceSettingsControl1;
        private System.Windows.Forms.Label label1;
        private Components.AlgorithmsListView algorithmsListView1;
        private Components.AlgorithmSettingsControl algorithmSettingsControl1;
        private System.Windows.Forms.GroupBox groupBoxBenchmarkTimeLimits;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Components.BenchmarkLimitControl benchmarkLimitControlCPU;
        private Components.BenchmarkLimitControl benchmarkLimitControlNVIDIA;
        private Components.BenchmarkLimitControl benchmarkLimitControlAMD;
        private System.Windows.Forms.ToolTip toolTip1;
        private Components.DevicesListView devicesListView1;

    }
}