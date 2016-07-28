namespace NiceHashMiner.Forms {
    partial class FormSettings_New {
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
            this.splitContainerTabControlButtons = new System.Windows.Forms.SplitContainer();
            this.buttonSaveClose = new System.Windows.Forms.Button();
            this.buttonDefaults = new System.Windows.Forms.Button();
            this.buttonCloseNoSave = new System.Windows.Forms.Button();
            this.tabPageDevices = new System.Windows.Forms.TabPage();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.checkBox_DebugConsole = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoStartMining = new System.Windows.Forms.CheckBox();
            this.checkBox_HideMiningWindows = new System.Windows.Forms.CheckBox();
            this.checkBox_MinimizeToTray = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionNVidia5X = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionNVidia3X = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionNVidia2X = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableDetectionAMD = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoScaleBTCValues = new System.Windows.Forms.CheckBox();
            this.checkBox_StartMiningWhenIdle = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowDriverVersionWarning = new System.Windows.Forms.CheckBox();
            this.label_BitcoinAddress = new System.Windows.Forms.Label();
            this.label_WorkerName = new System.Windows.Forms.Label();
            this.textBox_BitcoinAddress = new System.Windows.Forms.TextBox();
            this.textBox_WorkerName = new System.Windows.Forms.TextBox();
            this.label_ServiceLocation = new System.Windows.Forms.Label();
            this.comboBox_ServiceLocation = new System.Windows.Forms.ComboBox();
            this.textBox_SwitchMinSecondsFixed = new System.Windows.Forms.TextBox();
            this.label_SwitchMinSecondsFixed = new System.Windows.Forms.Label();
            this.label_SwitchMinSecondsDynamic = new System.Windows.Forms.Label();
            this.textBox_SwitchMinSecondsDynamic = new System.Windows.Forms.TextBox();
            this.label_MinerAPIQueryInterval = new System.Windows.Forms.Label();
            this.textBox_MinerAPIQueryInterval = new System.Windows.Forms.TextBox();
            this.label_MinerRestartDelayMS = new System.Windows.Forms.Label();
            this.textBox_MinerRestartDelayMS = new System.Windows.Forms.TextBox();
            this.label_MinerAPIGraceSeconds = new System.Windows.Forms.Label();
            this.textBox_MinerAPIGraceSeconds = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsCPU_Group = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsCPU_Quick = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsCPU_Quick = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsCPU_Standard = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsCPU_Standard = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsCPU_Precise = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsCPU_Precise = new System.Windows.Forms.Label();
            this.label_BenchmarkTimeLimitsNVIDIA_Group = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsNVIDIA_Quick = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsNVIDIA_Standard = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsNVIDIA_Precise = new System.Windows.Forms.Label();
            this.label_BenchmarkTimeLimitsAMD_Group = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsAMD_Quick = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsAMD_Quick = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsAMD_Standard = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsAMD_Standard = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsAMD_Precise = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsAMD_Precise = new System.Windows.Forms.Label();
            this.label_MinIdleSeconds = new System.Windows.Forms.Label();
            this.textBox_MinIdleSeconds = new System.Windows.Forms.TextBox();
            this.label_LogMaxFileSize = new System.Windows.Forms.Label();
            this.textBox_LogMaxFileSize = new System.Windows.Forms.TextBox();
            this.checkBox_UseNewSettingsPage = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableWindowsErrorReporting = new System.Windows.Forms.CheckBox();
            this.checkBox_NVIDIAP0State = new System.Windows.Forms.CheckBox();
            this.comboBox_Language = new System.Windows.Forms.ComboBox();
            this.label_Language = new System.Windows.Forms.Label();
            this.label_ethminerAPIPortNvidia = new System.Windows.Forms.Label();
            this.label_ethminerAPIPortAMD = new System.Windows.Forms.Label();
            this.label_ethminerDefaultBlockHeight = new System.Windows.Forms.Label();
            this.textBox_ethminerAPIPortNvidia = new System.Windows.Forms.TextBox();
            this.textBox_ethminerAPIPortAMD = new System.Windows.Forms.TextBox();
            this.textBox_ethminerDefaultBlockHeight = new System.Windows.Forms.TextBox();
            this.checkBox_LogToFile = new System.Windows.Forms.CheckBox();
            this.label_SwitchMinSecondsAMD = new System.Windows.Forms.Label();
            this.textBox_SwitchMinSecondsAMD = new System.Windows.Forms.TextBox();
            this.label_MinerAPIGraceSecondsAMD = new System.Windows.Forms.Label();
            this.textBox_MinerAPIGraceSecondsAMD = new System.Windows.Forms.TextBox();
            this.currencyConverterCombobox = new System.Windows.Forms.ComboBox();
            this.displayCurrencyLabel = new System.Windows.Forms.Label();
            this.tabControlGeneral = new System.Windows.Forms.TabControl();
            this.splitContainerDevicesSettings = new System.Windows.Forms.SplitContainer();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.devicesListView1 = new NiceHashMiner.Forms.Components.DevicesListView();
            this.deviceSettingsControl1 = new NiceHashMiner.Forms.Components.DeviceSettingsControl();
            this.label1 = new System.Windows.Forms.Label();
            this.algorithmsListView1 = new NiceHashMiner.Forms.Components.AlgorithmsListView();
            this.benchmarkAlgorithmSettup1 = new NiceHashMiner.Forms.Components.BenchmarkAlgorithmSettup();
            this.splitContainerTabControlButtons.Panel1.SuspendLayout();
            this.splitContainerTabControlButtons.Panel2.SuspendLayout();
            this.splitContainerTabControlButtons.SuspendLayout();
            this.tabPageDevices.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabControlGeneral.SuspendLayout();
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
            // buttonSaveClose
            // 
            this.buttonSaveClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveClose.Location = new System.Drawing.Point(686, 10);
            this.buttonSaveClose.Name = "buttonSaveClose";
            this.buttonSaveClose.Size = new System.Drawing.Size(110, 23);
            this.buttonSaveClose.TabIndex = 44;
            this.buttonSaveClose.Text = "&Save and Close";
            this.buttonSaveClose.UseVisualStyleBackColor = true;
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
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.displayCurrencyLabel);
            this.tabPageGeneral.Controls.Add(this.currencyConverterCombobox);
            this.tabPageGeneral.Controls.Add(this.textBox_MinerAPIGraceSecondsAMD);
            this.tabPageGeneral.Controls.Add(this.textBox_SwitchMinSecondsAMD);
            this.tabPageGeneral.Controls.Add(this.textBox_ethminerDefaultBlockHeight);
            this.tabPageGeneral.Controls.Add(this.textBox_ethminerAPIPortAMD);
            this.tabPageGeneral.Controls.Add(this.textBox_ethminerAPIPortNvidia);
            this.tabPageGeneral.Controls.Add(this.textBox_LogMaxFileSize);
            this.tabPageGeneral.Controls.Add(this.textBox_MinIdleSeconds);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsAMD_Precise);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsAMD_Standard);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsAMD_Quick);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsNVIDIA_Precise);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsNVIDIA_Standard);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsNVIDIA_Quick);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsCPU_Precise);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsCPU_Standard);
            this.tabPageGeneral.Controls.Add(this.textBox_BenchmarkTimeLimitsCPU_Quick);
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
            this.tabPageGeneral.Controls.Add(this.label_ethminerAPIPortAMD);
            this.tabPageGeneral.Controls.Add(this.label_ethminerAPIPortNvidia);
            this.tabPageGeneral.Controls.Add(this.label_Language);
            this.tabPageGeneral.Controls.Add(this.comboBox_Language);
            this.tabPageGeneral.Controls.Add(this.checkBox_NVIDIAP0State);
            this.tabPageGeneral.Controls.Add(this.checkBox_DisableWindowsErrorReporting);
            this.tabPageGeneral.Controls.Add(this.checkBox_UseNewSettingsPage);
            this.tabPageGeneral.Controls.Add(this.label_LogMaxFileSize);
            this.tabPageGeneral.Controls.Add(this.label_MinIdleSeconds);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Precise);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Standard);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Quick);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Group);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Precise);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Standard);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Quick);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Group);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Precise);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Standard);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Quick);
            this.tabPageGeneral.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Group);
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
            this.tabPageGeneral.Size = new System.Drawing.Size(929, 611);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
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
            // label_BitcoinAddress
            // 
            this.label_BitcoinAddress.AutoSize = true;
            this.label_BitcoinAddress.Location = new System.Drawing.Point(218, 66);
            this.label_BitcoinAddress.Name = "label_BitcoinAddress";
            this.label_BitcoinAddress.Size = new System.Drawing.Size(80, 13);
            this.label_BitcoinAddress.TabIndex = 355;
            this.label_BitcoinAddress.Text = "BitcoinAddress:";
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
            // textBox_BitcoinAddress
            // 
            this.textBox_BitcoinAddress.Location = new System.Drawing.Point(221, 86);
            this.textBox_BitcoinAddress.Name = "textBox_BitcoinAddress";
            this.textBox_BitcoinAddress.Size = new System.Drawing.Size(316, 20);
            this.textBox_BitcoinAddress.TabIndex = 329;
            // 
            // textBox_WorkerName
            // 
            this.textBox_WorkerName.Location = new System.Drawing.Point(568, 87);
            this.textBox_WorkerName.Name = "textBox_WorkerName";
            this.textBox_WorkerName.Size = new System.Drawing.Size(139, 20);
            this.textBox_WorkerName.TabIndex = 339;
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
            // textBox_SwitchMinSecondsFixed
            // 
            this.textBox_SwitchMinSecondsFixed.Location = new System.Drawing.Point(220, 234);
            this.textBox_SwitchMinSecondsFixed.Name = "textBox_SwitchMinSecondsFixed";
            this.textBox_SwitchMinSecondsFixed.Size = new System.Drawing.Size(160, 20);
            this.textBox_SwitchMinSecondsFixed.TabIndex = 332;
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
            // label_SwitchMinSecondsDynamic
            // 
            this.label_SwitchMinSecondsDynamic.AutoSize = true;
            this.label_SwitchMinSecondsDynamic.Location = new System.Drawing.Point(394, 213);
            this.label_SwitchMinSecondsDynamic.Name = "label_SwitchMinSecondsDynamic";
            this.label_SwitchMinSecondsDynamic.Size = new System.Drawing.Size(142, 13);
            this.label_SwitchMinSecondsDynamic.TabIndex = 378;
            this.label_SwitchMinSecondsDynamic.Text = "SwitchMinSecondsDynamic:";
            // 
            // textBox_SwitchMinSecondsDynamic
            // 
            this.textBox_SwitchMinSecondsDynamic.Location = new System.Drawing.Point(397, 234);
            this.textBox_SwitchMinSecondsDynamic.Name = "textBox_SwitchMinSecondsDynamic";
            this.textBox_SwitchMinSecondsDynamic.Size = new System.Drawing.Size(140, 20);
            this.textBox_SwitchMinSecondsDynamic.TabIndex = 337;
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
            // textBox_MinerAPIQueryInterval
            // 
            this.textBox_MinerAPIQueryInterval.Location = new System.Drawing.Point(568, 185);
            this.textBox_MinerAPIQueryInterval.Name = "textBox_MinerAPIQueryInterval";
            this.textBox_MinerAPIQueryInterval.Size = new System.Drawing.Size(140, 20);
            this.textBox_MinerAPIQueryInterval.TabIndex = 341;
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
            // textBox_MinerRestartDelayMS
            // 
            this.textBox_MinerRestartDelayMS.Location = new System.Drawing.Point(568, 136);
            this.textBox_MinerRestartDelayMS.Name = "textBox_MinerRestartDelayMS";
            this.textBox_MinerRestartDelayMS.Size = new System.Drawing.Size(139, 20);
            this.textBox_MinerRestartDelayMS.TabIndex = 340;
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
            // textBox_MinerAPIGraceSeconds
            // 
            this.textBox_MinerAPIGraceSeconds.Location = new System.Drawing.Point(221, 185);
            this.textBox_MinerAPIGraceSeconds.Name = "textBox_MinerAPIGraceSeconds";
            this.textBox_MinerAPIGraceSeconds.Size = new System.Drawing.Size(160, 20);
            this.textBox_MinerAPIGraceSeconds.TabIndex = 331;
            // 
            // label_BenchmarkTimeLimitsCPU_Group
            // 
            this.label_BenchmarkTimeLimitsCPU_Group.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Group.Location = new System.Drawing.Point(732, 16);
            this.label_BenchmarkTimeLimitsCPU_Group.Name = "label_BenchmarkTimeLimitsCPU_Group";
            this.label_BenchmarkTimeLimitsCPU_Group.Size = new System.Drawing.Size(135, 13);
            this.label_BenchmarkTimeLimitsCPU_Group.TabIndex = 373;
            this.label_BenchmarkTimeLimitsCPU_Group.Text = "BenchmarkTimeLimitsCPU:";
            // 
            // textBox_BenchmarkTimeLimitsCPU_Quick
            // 
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Location = new System.Drawing.Point(796, 34);
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Name = "textBox_BenchmarkTimeLimitsCPU_Quick";
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsCPU_Quick.TabIndex = 344;
            // 
            // label_BenchmarkTimeLimitsCPU_Quick
            // 
            this.label_BenchmarkTimeLimitsCPU_Quick.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Quick.Location = new System.Drawing.Point(733, 37);
            this.label_BenchmarkTimeLimitsCPU_Quick.Name = "label_BenchmarkTimeLimitsCPU_Quick";
            this.label_BenchmarkTimeLimitsCPU_Quick.Size = new System.Drawing.Size(35, 13);
            this.label_BenchmarkTimeLimitsCPU_Quick.TabIndex = 370;
            this.label_BenchmarkTimeLimitsCPU_Quick.Text = "Quick";
            // 
            // textBox_BenchmarkTimeLimitsCPU_Standard
            // 
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Location = new System.Drawing.Point(796, 60);
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Name = "textBox_BenchmarkTimeLimitsCPU_Standard";
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsCPU_Standard.TabIndex = 345;
            // 
            // label_BenchmarkTimeLimitsCPU_Standard
            // 
            this.label_BenchmarkTimeLimitsCPU_Standard.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Standard.Location = new System.Drawing.Point(733, 63);
            this.label_BenchmarkTimeLimitsCPU_Standard.Name = "label_BenchmarkTimeLimitsCPU_Standard";
            this.label_BenchmarkTimeLimitsCPU_Standard.Size = new System.Drawing.Size(50, 13);
            this.label_BenchmarkTimeLimitsCPU_Standard.TabIndex = 368;
            this.label_BenchmarkTimeLimitsCPU_Standard.Text = "Standard";
            // 
            // textBox_BenchmarkTimeLimitsCPU_Precise
            // 
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Location = new System.Drawing.Point(796, 86);
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Name = "textBox_BenchmarkTimeLimitsCPU_Precise";
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsCPU_Precise.TabIndex = 346;
            // 
            // label_BenchmarkTimeLimitsCPU_Precise
            // 
            this.label_BenchmarkTimeLimitsCPU_Precise.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Precise.Location = new System.Drawing.Point(733, 89);
            this.label_BenchmarkTimeLimitsCPU_Precise.Name = "label_BenchmarkTimeLimitsCPU_Precise";
            this.label_BenchmarkTimeLimitsCPU_Precise.Size = new System.Drawing.Size(42, 13);
            this.label_BenchmarkTimeLimitsCPU_Precise.TabIndex = 367;
            this.label_BenchmarkTimeLimitsCPU_Precise.Text = "Precise";
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Group
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Group.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Group.Location = new System.Drawing.Point(732, 115);
            this.label_BenchmarkTimeLimitsNVIDIA_Group.Name = "label_BenchmarkTimeLimitsNVIDIA_Group";
            this.label_BenchmarkTimeLimitsNVIDIA_Group.Size = new System.Drawing.Size(149, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Group.TabIndex = 365;
            this.label_BenchmarkTimeLimitsNVIDIA_Group.Text = "BenchmarkTimeLimitsNVIDIA:";
            // 
            // textBox_BenchmarkTimeLimitsNVIDIA_Quick
            // 
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Location = new System.Drawing.Point(796, 135);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Name = "textBox_BenchmarkTimeLimitsNVIDIA_Quick";
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.TabIndex = 347;
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Quick
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Location = new System.Drawing.Point(733, 138);
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Name = "label_BenchmarkTimeLimitsNVIDIA_Quick";
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Size = new System.Drawing.Size(35, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.TabIndex = 371;
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Text = "Quick";
            // 
            // textBox_BenchmarkTimeLimitsNVIDIA_Standard
            // 
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Location = new System.Drawing.Point(796, 160);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Name = "textBox_BenchmarkTimeLimitsNVIDIA_Standard";
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.TabIndex = 348;
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Standard
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Location = new System.Drawing.Point(733, 163);
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Name = "label_BenchmarkTimeLimitsNVIDIA_Standard";
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Size = new System.Drawing.Size(50, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.TabIndex = 369;
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Text = "Standard";
            // 
            // textBox_BenchmarkTimeLimitsNVIDIA_Precise
            // 
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Location = new System.Drawing.Point(796, 185);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Name = "textBox_BenchmarkTimeLimitsNVIDIA_Precise";
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.TabIndex = 349;
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Precise
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Location = new System.Drawing.Point(733, 188);
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Name = "label_BenchmarkTimeLimitsNVIDIA_Precise";
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Size = new System.Drawing.Size(42, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.TabIndex = 372;
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Text = "Precise";
            // 
            // label_BenchmarkTimeLimitsAMD_Group
            // 
            this.label_BenchmarkTimeLimitsAMD_Group.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Group.Location = new System.Drawing.Point(732, 216);
            this.label_BenchmarkTimeLimitsAMD_Group.Name = "label_BenchmarkTimeLimitsAMD_Group";
            this.label_BenchmarkTimeLimitsAMD_Group.Size = new System.Drawing.Size(137, 13);
            this.label_BenchmarkTimeLimitsAMD_Group.TabIndex = 374;
            this.label_BenchmarkTimeLimitsAMD_Group.Text = "BenchmarkTimeLimitsAMD:";
            // 
            // textBox_BenchmarkTimeLimitsAMD_Quick
            // 
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Location = new System.Drawing.Point(796, 234);
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Name = "textBox_BenchmarkTimeLimitsAMD_Quick";
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsAMD_Quick.TabIndex = 350;
            // 
            // label_BenchmarkTimeLimitsAMD_Quick
            // 
            this.label_BenchmarkTimeLimitsAMD_Quick.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Quick.Location = new System.Drawing.Point(733, 237);
            this.label_BenchmarkTimeLimitsAMD_Quick.Name = "label_BenchmarkTimeLimitsAMD_Quick";
            this.label_BenchmarkTimeLimitsAMD_Quick.Size = new System.Drawing.Size(35, 13);
            this.label_BenchmarkTimeLimitsAMD_Quick.TabIndex = 377;
            this.label_BenchmarkTimeLimitsAMD_Quick.Text = "Quick";
            // 
            // textBox_BenchmarkTimeLimitsAMD_Standard
            // 
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Location = new System.Drawing.Point(796, 259);
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Name = "textBox_BenchmarkTimeLimitsAMD_Standard";
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsAMD_Standard.TabIndex = 351;
            // 
            // label_BenchmarkTimeLimitsAMD_Standard
            // 
            this.label_BenchmarkTimeLimitsAMD_Standard.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Standard.Location = new System.Drawing.Point(733, 262);
            this.label_BenchmarkTimeLimitsAMD_Standard.Name = "label_BenchmarkTimeLimitsAMD_Standard";
            this.label_BenchmarkTimeLimitsAMD_Standard.Size = new System.Drawing.Size(50, 13);
            this.label_BenchmarkTimeLimitsAMD_Standard.TabIndex = 364;
            this.label_BenchmarkTimeLimitsAMD_Standard.Text = "Standard";
            // 
            // textBox_BenchmarkTimeLimitsAMD_Precise
            // 
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Location = new System.Drawing.Point(796, 284);
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Name = "textBox_BenchmarkTimeLimitsAMD_Precise";
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsAMD_Precise.TabIndex = 352;
            // 
            // label_BenchmarkTimeLimitsAMD_Precise
            // 
            this.label_BenchmarkTimeLimitsAMD_Precise.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Precise.Location = new System.Drawing.Point(733, 287);
            this.label_BenchmarkTimeLimitsAMD_Precise.Name = "label_BenchmarkTimeLimitsAMD_Precise";
            this.label_BenchmarkTimeLimitsAMD_Precise.Size = new System.Drawing.Size(42, 13);
            this.label_BenchmarkTimeLimitsAMD_Precise.TabIndex = 353;
            this.label_BenchmarkTimeLimitsAMD_Precise.Text = "Precise";
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
            // textBox_MinIdleSeconds
            // 
            this.textBox_MinIdleSeconds.Location = new System.Drawing.Point(397, 136);
            this.textBox_MinIdleSeconds.Name = "textBox_MinIdleSeconds";
            this.textBox_MinIdleSeconds.Size = new System.Drawing.Size(140, 20);
            this.textBox_MinIdleSeconds.TabIndex = 335;
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
            // textBox_LogMaxFileSize
            // 
            this.textBox_LogMaxFileSize.Location = new System.Drawing.Point(221, 332);
            this.textBox_LogMaxFileSize.Name = "textBox_LogMaxFileSize";
            this.textBox_LogMaxFileSize.Size = new System.Drawing.Size(160, 20);
            this.textBox_LogMaxFileSize.TabIndex = 334;
            // 
            // checkBox_UseNewSettingsPage
            // 
            this.checkBox_UseNewSettingsPage.AutoSize = true;
            this.checkBox_UseNewSettingsPage.Location = new System.Drawing.Point(24, 280);
            this.checkBox_UseNewSettingsPage.Name = "checkBox_UseNewSettingsPage";
            this.checkBox_UseNewSettingsPage.Size = new System.Drawing.Size(130, 17);
            this.checkBox_UseNewSettingsPage.TabIndex = 325;
            this.checkBox_UseNewSettingsPage.Text = "UseNewSettingsPage";
            this.checkBox_UseNewSettingsPage.UseVisualStyleBackColor = true;
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
            // comboBox_Language
            // 
            this.comboBox_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Location = new System.Drawing.Point(221, 37);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(190, 21);
            this.comboBox_Language.TabIndex = 328;
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
            // label_ethminerAPIPortNvidia
            // 
            this.label_ethminerAPIPortNvidia.AutoSize = true;
            this.label_ethminerAPIPortNvidia.Location = new System.Drawing.Point(394, 262);
            this.label_ethminerAPIPortNvidia.Name = "label_ethminerAPIPortNvidia";
            this.label_ethminerAPIPortNvidia.Size = new System.Drawing.Size(122, 13);
            this.label_ethminerAPIPortNvidia.TabIndex = 359;
            this.label_ethminerAPIPortNvidia.Text = "ethminerAPIPortNVIDIA:";
            // 
            // label_ethminerAPIPortAMD
            // 
            this.label_ethminerAPIPortAMD.AutoSize = true;
            this.label_ethminerAPIPortAMD.Location = new System.Drawing.Point(565, 262);
            this.label_ethminerAPIPortAMD.Name = "label_ethminerAPIPortAMD";
            this.label_ethminerAPIPortAMD.Size = new System.Drawing.Size(110, 13);
            this.label_ethminerAPIPortAMD.TabIndex = 360;
            this.label_ethminerAPIPortAMD.Text = "ethminerAPIPortAMD:";
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
            // textBox_ethminerAPIPortNvidia
            // 
            this.textBox_ethminerAPIPortNvidia.Location = new System.Drawing.Point(397, 283);
            this.textBox_ethminerAPIPortNvidia.Name = "textBox_ethminerAPIPortNvidia";
            this.textBox_ethminerAPIPortNvidia.Size = new System.Drawing.Size(140, 20);
            this.textBox_ethminerAPIPortNvidia.TabIndex = 338;
            // 
            // textBox_ethminerAPIPortAMD
            // 
            this.textBox_ethminerAPIPortAMD.Location = new System.Drawing.Point(568, 283);
            this.textBox_ethminerAPIPortAMD.Name = "textBox_ethminerAPIPortAMD";
            this.textBox_ethminerAPIPortAMD.Size = new System.Drawing.Size(139, 20);
            this.textBox_ethminerAPIPortAMD.TabIndex = 343;
            // 
            // textBox_ethminerDefaultBlockHeight
            // 
            this.textBox_ethminerDefaultBlockHeight.Location = new System.Drawing.Point(221, 283);
            this.textBox_ethminerDefaultBlockHeight.Name = "textBox_ethminerDefaultBlockHeight";
            this.textBox_ethminerDefaultBlockHeight.Size = new System.Drawing.Size(160, 20);
            this.textBox_ethminerDefaultBlockHeight.TabIndex = 333;
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
            // label_SwitchMinSecondsAMD
            // 
            this.label_SwitchMinSecondsAMD.AutoSize = true;
            this.label_SwitchMinSecondsAMD.Location = new System.Drawing.Point(565, 213);
            this.label_SwitchMinSecondsAMD.Name = "label_SwitchMinSecondsAMD";
            this.label_SwitchMinSecondsAMD.Size = new System.Drawing.Size(125, 13);
            this.label_SwitchMinSecondsAMD.TabIndex = 362;
            this.label_SwitchMinSecondsAMD.Text = "SwitchMinSecondsAMD:";
            // 
            // textBox_SwitchMinSecondsAMD
            // 
            this.textBox_SwitchMinSecondsAMD.Location = new System.Drawing.Point(568, 234);
            this.textBox_SwitchMinSecondsAMD.Name = "textBox_SwitchMinSecondsAMD";
            this.textBox_SwitchMinSecondsAMD.Size = new System.Drawing.Size(139, 20);
            this.textBox_SwitchMinSecondsAMD.TabIndex = 342;
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
            // textBox_MinerAPIGraceSecondsAMD
            // 
            this.textBox_MinerAPIGraceSecondsAMD.Location = new System.Drawing.Point(397, 185);
            this.textBox_MinerAPIGraceSecondsAMD.Name = "textBox_MinerAPIGraceSecondsAMD";
            this.textBox_MinerAPIGraceSecondsAMD.Size = new System.Drawing.Size(139, 20);
            this.textBox_MinerAPIGraceSecondsAMD.TabIndex = 336;
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
            this.splitContainerDevicesSettings.Panel1.Controls.Add(this.checkBox1);
            this.splitContainerDevicesSettings.Panel1.Controls.Add(this.devicesListView1);
            this.splitContainerDevicesSettings.Panel1.Controls.Add(this.deviceSettingsControl1);
            // 
            // splitContainerDevicesSettings.Panel2
            // 
            this.splitContainerDevicesSettings.Panel2.Controls.Add(this.label1);
            this.splitContainerDevicesSettings.Panel2.Controls.Add(this.algorithmsListView1);
            this.splitContainerDevicesSettings.Panel2.Controls.Add(this.benchmarkAlgorithmSettup1);
            this.splitContainerDevicesSettings.Size = new System.Drawing.Size(960, 605);
            this.splitContainerDevicesSettings.SplitterDistance = 488;
            this.splitContainerDevicesSettings.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(3, 11);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(195, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Use same settings for same devices";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // devicesListView1
            // 
            this.devicesListView1.Location = new System.Drawing.Point(3, 34);
            this.devicesListView1.Name = "devicesListView1";
            this.devicesListView1.Size = new System.Drawing.Size(452, 132);
            this.devicesListView1.TabIndex = 0;
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
            // benchmarkAlgorithmSettup1
            // 
            this.benchmarkAlgorithmSettup1.Location = new System.Drawing.Point(48, 327);
            this.benchmarkAlgorithmSettup1.Name = "benchmarkAlgorithmSettup1";
            this.benchmarkAlgorithmSettup1.Size = new System.Drawing.Size(413, 245);
            this.benchmarkAlgorithmSettup1.TabIndex = 1;
            // 
            // FormSettings_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 698);
            this.Controls.Add(this.splitContainerTabControlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings_New";
            this.Text = "FormSettings_New";
            this.splitContainerTabControlButtons.Panel1.ResumeLayout(false);
            this.splitContainerTabControlButtons.Panel2.ResumeLayout(false);
            this.splitContainerTabControlButtons.ResumeLayout(false);
            this.tabPageDevices.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabControlGeneral.ResumeLayout(false);
            this.splitContainerDevicesSettings.Panel1.ResumeLayout(false);
            this.splitContainerDevicesSettings.Panel1.PerformLayout();
            this.splitContainerDevicesSettings.Panel2.ResumeLayout(false);
            this.splitContainerDevicesSettings.Panel2.PerformLayout();
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
        private System.Windows.Forms.TextBox textBox_ethminerAPIPortAMD;
        private System.Windows.Forms.TextBox textBox_ethminerAPIPortNvidia;
        private System.Windows.Forms.TextBox textBox_LogMaxFileSize;
        private System.Windows.Forms.TextBox textBox_MinIdleSeconds;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsAMD_Precise;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsAMD_Standard;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsAMD_Quick;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsNVIDIA_Precise;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsNVIDIA_Standard;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsNVIDIA_Quick;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsCPU_Precise;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsCPU_Standard;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsCPU_Quick;
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
        private System.Windows.Forms.Label label_ethminerAPIPortAMD;
        private System.Windows.Forms.Label label_ethminerAPIPortNvidia;
        private System.Windows.Forms.Label label_Language;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.CheckBox checkBox_NVIDIAP0State;
        private System.Windows.Forms.CheckBox checkBox_DisableWindowsErrorReporting;
        private System.Windows.Forms.CheckBox checkBox_UseNewSettingsPage;
        private System.Windows.Forms.Label label_LogMaxFileSize;
        private System.Windows.Forms.Label label_MinIdleSeconds;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Precise;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Standard;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Quick;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Group;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Precise;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Standard;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Quick;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Group;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Precise;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Standard;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Quick;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Group;
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
        private System.Windows.Forms.CheckBox checkBox1;
        private Components.DevicesListView devicesListView1;
        private Components.DeviceSettingsControl deviceSettingsControl1;
        private System.Windows.Forms.Label label1;
        private Components.AlgorithmsListView algorithmsListView1;
        private Components.BenchmarkAlgorithmSettup benchmarkAlgorithmSettup1;

    }
}