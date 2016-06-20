namespace NiceHashMiner
{
    partial class Form_Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Settings));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_CPU0_ExtraLaunchParameters = new System.Windows.Forms.TextBox();
            this.label_CPU0_ExtraLaunchParameters = new System.Windows.Forms.Label();
            this.tabControl_CPU0 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.textBox_CPU0_lyra2re_ExtraLaunchParameters = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.checkBox_CPU0_lyra2re_Skip = new System.Windows.Forms.CheckBox();
            this.textBox_CPU0_lyra2re_BenchmarkSpeed = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.textBox_CPU0_lyra2re_UsePassword = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.textBox_CPU0_APIBindPort = new System.Windows.Forms.TextBox();
            this.label_CPU0_APIBindPort = new System.Windows.Forms.Label();
            this.textBox_CPU0_LessThreads = new System.Windows.Forms.TextBox();
            this.label_CPU0_LessThreads = new System.Windows.Forms.Label();
            this.comboBox_CPU0_ForceCPUExtension = new System.Windows.Forms.ComboBox();
            this.label_CPU0_ForceCPUExtension = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_NVIDIAP0State = new System.Windows.Forms.CheckBox();
            this.checkBox_DisableWindowsErrorReporting = new System.Windows.Forms.CheckBox();
            this.checkBox_UseNewSettingsPage = new System.Windows.Forms.CheckBox();
            this.textBox_LogMaxFileSize = new System.Windows.Forms.TextBox();
            this.label_LogMaxFileSize = new System.Windows.Forms.Label();
            this.textBox_LogLevel = new System.Windows.Forms.TextBox();
            this.label_LogLevel = new System.Windows.Forms.Label();
            this.textBox_MinIdleSeconds = new System.Windows.Forms.TextBox();
            this.label_MinIdleSeconds = new System.Windows.Forms.Label();
            this.label_BenchmarkTimeLimitsAMD_Precise = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsAMD_Precise = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsAMD_Standard = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsAMD_Standard = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsAMD_Quick = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsAMD_Quick = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label_BenchmarkTimeLimitsNVIDIA_Precise = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsNVIDIA_Standard = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsNVIDIA_Quick = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label_BenchmarkTimeLimitsCPU_Precise = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsCPU_Precise = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsCPU_Standard = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsCPU_Standard = new System.Windows.Forms.TextBox();
            this.label_BenchmarkTimeLimitsCPU_Quick = new System.Windows.Forms.Label();
            this.textBox_BenchmarkTimeLimitsCPU_Quick = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox_MinerAPIGraceMinutes = new System.Windows.Forms.TextBox();
            this.label_MinerAPIGraceMinutes = new System.Windows.Forms.Label();
            this.textBox_MinerRestartDelayMS = new System.Windows.Forms.TextBox();
            this.label_MinerRestartDelayMS = new System.Windows.Forms.Label();
            this.textBox_MinerAPIQueryInterval = new System.Windows.Forms.TextBox();
            this.label_MinerAPIQueryInterval = new System.Windows.Forms.Label();
            this.textBox_SwitchMinSecondsDynamic = new System.Windows.Forms.TextBox();
            this.label_SwitchMinSecondsDynamic = new System.Windows.Forms.Label();
            this.label_SwitchMinSecondsFixed = new System.Windows.Forms.Label();
            this.textBox_SwitchMinSecondsFixed = new System.Windows.Forms.TextBox();
            this.comboBox_Location = new System.Windows.Forms.ComboBox();
            this.label_Location = new System.Windows.Forms.Label();
            this.textBox_WorkerName = new System.Windows.Forms.TextBox();
            this.textBox_BitcoinAddress = new System.Windows.Forms.TextBox();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_NVIDIA5X = new System.Windows.Forms.TabPage();
            this.textBox_NVIDIA5X_MinimumProfit = new System.Windows.Forms.TextBox();
            this.label_NVIDIA5X_MinimumProfit = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.tabControl_NVIDIA5X_Algos = new System.Windows.Forms.TabControl();
            this.label_NVIDIA5X_ExtraLaunchParameters = new System.Windows.Forms.Label();
            this.textBox_NVIDIA5X_ExtraLaunchParameters = new System.Windows.Forms.TextBox();
            this.textBox_NVIDIA5X_UsePassword = new System.Windows.Forms.TextBox();
            this.label_NVIDIA5X_UsePassword = new System.Windows.Forms.Label();
            this.textBox_NVIDIA5X_APIBindPort = new System.Windows.Forms.TextBox();
            this.label_NVIDIA5X_APIBindPort = new System.Windows.Forms.Label();
            this.tabPage_NVIDIA3X = new System.Windows.Forms.TabPage();
            this.textBox_NVIDIA3X_MinimumProfit = new System.Windows.Forms.TextBox();
            this.label_NVIDIA3X_MinimumProfit = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.tabControl_NVIDIA3X_Algos = new System.Windows.Forms.TabControl();
            this.label_NVIDIA3X_ExtraLaunchParameters = new System.Windows.Forms.Label();
            this.textBox_NVIDIA3X_ExtraLaunchParameters = new System.Windows.Forms.TextBox();
            this.textBox_NVIDIA3X_UsePassword = new System.Windows.Forms.TextBox();
            this.label_NVIDIA3X_UsePassword = new System.Windows.Forms.Label();
            this.textBox_NVIDIA3X_APIBindPort = new System.Windows.Forms.TextBox();
            this.label_NVIDIA3X_APIBindPort = new System.Windows.Forms.Label();
            this.tabPage_NVIDIA2X = new System.Windows.Forms.TabPage();
            this.textBox_NVIDIA2X_MinimumProfit = new System.Windows.Forms.TextBox();
            this.label_NVIDIA2X_MinimumProfit = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.tabControl_NVIDIA2X_Algos = new System.Windows.Forms.TabControl();
            this.tabPage38 = new System.Windows.Forms.TabPage();
            this.textBox54 = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.textBox55 = new System.Windows.Forms.TextBox();
            this.label60 = new System.Windows.Forms.Label();
            this.textBox56 = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.label_NVIDIA2X_ExtraLaunchParameters = new System.Windows.Forms.Label();
            this.textBox_NVIDIA2X_ExtraLaunchParameters = new System.Windows.Forms.TextBox();
            this.textBox_NVIDIA2X_UsePassword = new System.Windows.Forms.TextBox();
            this.label_NVIDIA2X_UsePassword = new System.Windows.Forms.Label();
            this.textBox_NVIDIA2X_APIBindPort = new System.Windows.Forms.TextBox();
            this.label_NVIDIA2X_APIBindPort = new System.Windows.Forms.Label();
            this.tabPage_AMD = new System.Windows.Forms.TabPage();
            this.label38 = new System.Windows.Forms.Label();
            this.textBox_AMD_MinimumProfit = new System.Windows.Forms.TextBox();
            this.label_AMD_MinimumProfit = new System.Windows.Forms.Label();
            this.tabControl_AMD_Algos = new System.Windows.Forms.TabControl();
            this.tabPage52 = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label39 = new System.Windows.Forms.Label();
            this.textBox57 = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.textBox58 = new System.Windows.Forms.TextBox();
            this.label63 = new System.Windows.Forms.Label();
            this.textBox59 = new System.Windows.Forms.TextBox();
            this.label64 = new System.Windows.Forms.Label();
            this.label_AMD_ExtraLaunchParameters = new System.Windows.Forms.Label();
            this.textBox_AMD_ExtraLaunchParameters = new System.Windows.Forms.TextBox();
            this.textBox_AMD_UsePassword = new System.Windows.Forms.TextBox();
            this.label_AMD_UsePassword = new System.Windows.Forms.Label();
            this.textBox_AMD_APIBindPort = new System.Windows.Forms.TextBox();
            this.label_AMD_APIBindPort = new System.Windows.Forms.Label();
            this.checkBox_AMD_DisableAMDTempControl = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button_Close = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.comboBox_Language = new System.Windows.Forms.ComboBox();
            this.label_Language = new System.Windows.Forms.Label();
            this.label_ethminerAPIPortNvidia = new System.Windows.Forms.Label();
            this.label_ethminerAPIPortAMD = new System.Windows.Forms.Label();
            this.label_ethminerDefaultBlockHeight = new System.Windows.Forms.Label();
            this.textBox_ethminerAPIPortNvidia = new System.Windows.Forms.TextBox();
            this.textBox_ethminerAPIPortAMD = new System.Windows.Forms.TextBox();
            this.textBox_ethminerDefaultBlockHeight = new System.Windows.Forms.TextBox();
            this.tabPage2.SuspendLayout();
            this.tabControl_CPU0.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_NVIDIA5X.SuspendLayout();
            this.tabPage_NVIDIA3X.SuspendLayout();
            this.tabPage_NVIDIA2X.SuspendLayout();
            this.tabControl_NVIDIA2X_Algos.SuspendLayout();
            this.tabPage38.SuspendLayout();
            this.tabPage_AMD.SuspendLayout();
            this.tabControl_AMD_Algos.SuspendLayout();
            this.tabPage52.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox_CPU0_ExtraLaunchParameters);
            this.tabPage2.Controls.Add(this.label_CPU0_ExtraLaunchParameters);
            this.tabPage2.Controls.Add(this.tabControl_CPU0);
            this.tabPage2.Controls.Add(this.textBox_CPU0_APIBindPort);
            this.tabPage2.Controls.Add(this.label_CPU0_APIBindPort);
            this.tabPage2.Controls.Add(this.textBox_CPU0_LessThreads);
            this.tabPage2.Controls.Add(this.label_CPU0_LessThreads);
            this.tabPage2.Controls.Add(this.comboBox_CPU0_ForceCPUExtension);
            this.tabPage2.Controls.Add(this.label_CPU0_ForceCPUExtension);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(823, 320);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CPU0";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox_CPU0_ExtraLaunchParameters
            // 
            this.textBox_CPU0_ExtraLaunchParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CPU0_ExtraLaunchParameters.Location = new System.Drawing.Point(135, 33);
            this.textBox_CPU0_ExtraLaunchParameters.Name = "textBox_CPU0_ExtraLaunchParameters";
            this.textBox_CPU0_ExtraLaunchParameters.Size = new System.Drawing.Size(678, 20);
            this.textBox_CPU0_ExtraLaunchParameters.TabIndex = 4;
            // 
            // label_CPU0_ExtraLaunchParameters
            // 
            this.label_CPU0_ExtraLaunchParameters.AutoSize = true;
            this.label_CPU0_ExtraLaunchParameters.Location = new System.Drawing.Point(6, 36);
            this.label_CPU0_ExtraLaunchParameters.Name = "label_CPU0_ExtraLaunchParameters";
            this.label_CPU0_ExtraLaunchParameters.Size = new System.Drawing.Size(123, 13);
            this.label_CPU0_ExtraLaunchParameters.TabIndex = 99;
            this.label_CPU0_ExtraLaunchParameters.Text = "ExtraLaunchParameters:";
            // 
            // tabControl_CPU0
            // 
            this.tabControl_CPU0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_CPU0.Controls.Add(this.tabPage7);
            this.tabControl_CPU0.Location = new System.Drawing.Point(9, 59);
            this.tabControl_CPU0.Name = "tabControl_CPU0";
            this.tabControl_CPU0.SelectedIndex = 0;
            this.tabControl_CPU0.Size = new System.Drawing.Size(808, 258);
            this.tabControl_CPU0.TabIndex = 5;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.textBox_CPU0_lyra2re_ExtraLaunchParameters);
            this.tabPage7.Controls.Add(this.label31);
            this.tabPage7.Controls.Add(this.checkBox_CPU0_lyra2re_Skip);
            this.tabPage7.Controls.Add(this.textBox_CPU0_lyra2re_BenchmarkSpeed);
            this.tabPage7.Controls.Add(this.label32);
            this.tabPage7.Controls.Add(this.textBox_CPU0_lyra2re_UsePassword);
            this.tabPage7.Controls.Add(this.label40);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(800, 232);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "lyra2re";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // textBox_CPU0_lyra2re_ExtraLaunchParameters
            // 
            this.textBox_CPU0_lyra2re_ExtraLaunchParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CPU0_lyra2re_ExtraLaunchParameters.Location = new System.Drawing.Point(9, 48);
            this.textBox_CPU0_lyra2re_ExtraLaunchParameters.Name = "textBox_CPU0_lyra2re_ExtraLaunchParameters";
            this.textBox_CPU0_lyra2re_ExtraLaunchParameters.Size = new System.Drawing.Size(785, 20);
            this.textBox_CPU0_lyra2re_ExtraLaunchParameters.TabIndex = 18;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 32);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(123, 13);
            this.label31.TabIndex = 17;
            this.label31.Text = "ExtraLaunchParameters:";
            // 
            // checkBox_CPU0_lyra2re_Skip
            // 
            this.checkBox_CPU0_lyra2re_Skip.AutoSize = true;
            this.checkBox_CPU0_lyra2re_Skip.Location = new System.Drawing.Point(9, 9);
            this.checkBox_CPU0_lyra2re_Skip.Name = "checkBox_CPU0_lyra2re_Skip";
            this.checkBox_CPU0_lyra2re_Skip.Size = new System.Drawing.Size(47, 17);
            this.checkBox_CPU0_lyra2re_Skip.TabIndex = 16;
            this.checkBox_CPU0_lyra2re_Skip.Text = "Skip";
            this.checkBox_CPU0_lyra2re_Skip.UseVisualStyleBackColor = true;
            // 
            // textBox_CPU0_lyra2re_BenchmarkSpeed
            // 
            this.textBox_CPU0_lyra2re_BenchmarkSpeed.Location = new System.Drawing.Point(356, 7);
            this.textBox_CPU0_lyra2re_BenchmarkSpeed.Name = "textBox_CPU0_lyra2re_BenchmarkSpeed";
            this.textBox_CPU0_lyra2re_BenchmarkSpeed.Size = new System.Drawing.Size(100, 20);
            this.textBox_CPU0_lyra2re_BenchmarkSpeed.TabIndex = 15;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(258, 10);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(95, 13);
            this.label32.TabIndex = 14;
            this.label32.Text = "BenchmarkSpeed:";
            // 
            // textBox_CPU0_lyra2re_UsePassword
            // 
            this.textBox_CPU0_lyra2re_UsePassword.Location = new System.Drawing.Point(140, 7);
            this.textBox_CPU0_lyra2re_UsePassword.Name = "textBox_CPU0_lyra2re_UsePassword";
            this.textBox_CPU0_lyra2re_UsePassword.Size = new System.Drawing.Size(100, 20);
            this.textBox_CPU0_lyra2re_UsePassword.TabIndex = 13;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(62, 10);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(75, 13);
            this.label40.TabIndex = 12;
            this.label40.Text = "UsePassword:";
            // 
            // textBox_CPU0_APIBindPort
            // 
            this.textBox_CPU0_APIBindPort.Location = new System.Drawing.Point(497, 6);
            this.textBox_CPU0_APIBindPort.Name = "textBox_CPU0_APIBindPort";
            this.textBox_CPU0_APIBindPort.Size = new System.Drawing.Size(100, 20);
            this.textBox_CPU0_APIBindPort.TabIndex = 3;
            // 
            // label_CPU0_APIBindPort
            // 
            this.label_CPU0_APIBindPort.AutoSize = true;
            this.label_CPU0_APIBindPort.Location = new System.Drawing.Point(427, 9);
            this.label_CPU0_APIBindPort.Name = "label_CPU0_APIBindPort";
            this.label_CPU0_APIBindPort.Size = new System.Drawing.Size(67, 13);
            this.label_CPU0_APIBindPort.TabIndex = 99;
            this.label_CPU0_APIBindPort.Text = "APIBindPort:";
            // 
            // textBox_CPU0_LessThreads
            // 
            this.textBox_CPU0_LessThreads.Location = new System.Drawing.Point(318, 6);
            this.textBox_CPU0_LessThreads.Name = "textBox_CPU0_LessThreads";
            this.textBox_CPU0_LessThreads.Size = new System.Drawing.Size(100, 20);
            this.textBox_CPU0_LessThreads.TabIndex = 2;
            // 
            // label_CPU0_LessThreads
            // 
            this.label_CPU0_LessThreads.AutoSize = true;
            this.label_CPU0_LessThreads.Location = new System.Drawing.Point(244, 9);
            this.label_CPU0_LessThreads.Name = "label_CPU0_LessThreads";
            this.label_CPU0_LessThreads.Size = new System.Drawing.Size(71, 13);
            this.label_CPU0_LessThreads.TabIndex = 99;
            this.label_CPU0_LessThreads.Text = "LessThreads:";
            // 
            // comboBox_CPU0_ForceCPUExtension
            // 
            this.comboBox_CPU0_ForceCPUExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CPU0_ForceCPUExtension.FormattingEnabled = true;
            this.comboBox_CPU0_ForceCPUExtension.Items.AddRange(new object[] {
            "Automatic",
            "SSE2",
            "AVX",
            "AVX2"});
            this.comboBox_CPU0_ForceCPUExtension.Location = new System.Drawing.Point(114, 6);
            this.comboBox_CPU0_ForceCPUExtension.Name = "comboBox_CPU0_ForceCPUExtension";
            this.comboBox_CPU0_ForceCPUExtension.Size = new System.Drawing.Size(121, 21);
            this.comboBox_CPU0_ForceCPUExtension.TabIndex = 1;
            // 
            // label_CPU0_ForceCPUExtension
            // 
            this.label_CPU0_ForceCPUExtension.AutoSize = true;
            this.label_CPU0_ForceCPUExtension.Location = new System.Drawing.Point(6, 9);
            this.label_CPU0_ForceCPUExtension.Name = "label_CPU0_ForceCPUExtension";
            this.label_CPU0_ForceCPUExtension.Size = new System.Drawing.Size(105, 13);
            this.label_CPU0_ForceCPUExtension.TabIndex = 99;
            this.label_CPU0_ForceCPUExtension.Text = "ForceCPUExtension:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox_ethminerDefaultBlockHeight);
            this.tabPage1.Controls.Add(this.textBox_ethminerAPIPortAMD);
            this.tabPage1.Controls.Add(this.textBox_ethminerAPIPortNvidia);
            this.tabPage1.Controls.Add(this.label_ethminerDefaultBlockHeight);
            this.tabPage1.Controls.Add(this.label_ethminerAPIPortAMD);
            this.tabPage1.Controls.Add(this.label_ethminerAPIPortNvidia);
            this.tabPage1.Controls.Add(this.label_Language);
            this.tabPage1.Controls.Add(this.comboBox_Language);
            this.tabPage1.Controls.Add(this.checkBox_NVIDIAP0State);
            this.tabPage1.Controls.Add(this.checkBox_DisableWindowsErrorReporting);
            this.tabPage1.Controls.Add(this.checkBox_UseNewSettingsPage);
            this.tabPage1.Controls.Add(this.textBox_LogMaxFileSize);
            this.tabPage1.Controls.Add(this.label_LogMaxFileSize);
            this.tabPage1.Controls.Add(this.textBox_LogLevel);
            this.tabPage1.Controls.Add(this.label_LogLevel);
            this.tabPage1.Controls.Add(this.textBox_MinIdleSeconds);
            this.tabPage1.Controls.Add(this.label_MinIdleSeconds);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Precise);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsAMD_Precise);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Standard);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsAMD_Standard);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsAMD_Quick);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsAMD_Quick);
            this.tabPage1.Controls.Add(this.label22);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Precise);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsNVIDIA_Precise);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Standard);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsNVIDIA_Standard);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsNVIDIA_Quick);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsNVIDIA_Quick);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Precise);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsCPU_Precise);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Standard);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsCPU_Standard);
            this.tabPage1.Controls.Add(this.label_BenchmarkTimeLimitsCPU_Quick);
            this.tabPage1.Controls.Add(this.textBox_BenchmarkTimeLimitsCPU_Quick);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.textBox_MinerAPIGraceMinutes);
            this.tabPage1.Controls.Add(this.label_MinerAPIGraceMinutes);
            this.tabPage1.Controls.Add(this.textBox_MinerRestartDelayMS);
            this.tabPage1.Controls.Add(this.label_MinerRestartDelayMS);
            this.tabPage1.Controls.Add(this.textBox_MinerAPIQueryInterval);
            this.tabPage1.Controls.Add(this.label_MinerAPIQueryInterval);
            this.tabPage1.Controls.Add(this.textBox_SwitchMinSecondsDynamic);
            this.tabPage1.Controls.Add(this.label_SwitchMinSecondsDynamic);
            this.tabPage1.Controls.Add(this.label_SwitchMinSecondsFixed);
            this.tabPage1.Controls.Add(this.textBox_SwitchMinSecondsFixed);
            this.tabPage1.Controls.Add(this.comboBox_Location);
            this.tabPage1.Controls.Add(this.label_Location);
            this.tabPage1.Controls.Add(this.textBox_WorkerName);
            this.tabPage1.Controls.Add(this.textBox_BitcoinAddress);
            this.tabPage1.Controls.Add(this.label_WorkerName);
            this.tabPage1.Controls.Add(this.label_BitcoinAddress);
            this.tabPage1.Controls.Add(this.checkBox_ShowDriverVersionWarning);
            this.tabPage1.Controls.Add(this.checkBox_StartMiningWhenIdle);
            this.tabPage1.Controls.Add(this.checkBox_AutoScaleBTCValues);
            this.tabPage1.Controls.Add(this.checkBox_DisableDetectionAMD);
            this.tabPage1.Controls.Add(this.checkBox_DisableDetectionNVidia2X);
            this.tabPage1.Controls.Add(this.checkBox_DisableDetectionNVidia3X);
            this.tabPage1.Controls.Add(this.checkBox_DisableDetectionNVidia5X);
            this.tabPage1.Controls.Add(this.checkBox_MinimizeToTray);
            this.tabPage1.Controls.Add(this.checkBox_HideMiningWindows);
            this.tabPage1.Controls.Add(this.checkBox_AutoStartMining);
            this.tabPage1.Controls.Add(this.checkBox_DebugConsole);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(823, 320);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_NVIDIAP0State
            // 
            this.checkBox_NVIDIAP0State.AutoSize = true;
            this.checkBox_NVIDIAP0State.Location = new System.Drawing.Point(6, 293);
            this.checkBox_NVIDIAP0State.Name = "checkBox_NVIDIAP0State";
            this.checkBox_NVIDIAP0State.Size = new System.Drawing.Size(100, 17);
            this.checkBox_NVIDIAP0State.TabIndex = 64;
            this.checkBox_NVIDIAP0State.Text = "NVIDIAP0State";
            this.checkBox_NVIDIAP0State.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableWindowsErrorReporting
            // 
            this.checkBox_DisableWindowsErrorReporting.AutoSize = true;
            this.checkBox_DisableWindowsErrorReporting.Location = new System.Drawing.Point(6, 249);
            this.checkBox_DisableWindowsErrorReporting.Name = "checkBox_DisableWindowsErrorReporting";
            this.checkBox_DisableWindowsErrorReporting.Size = new System.Drawing.Size(173, 17);
            this.checkBox_DisableWindowsErrorReporting.TabIndex = 63;
            this.checkBox_DisableWindowsErrorReporting.Text = "DisableWindowsErrorReporting";
            this.checkBox_DisableWindowsErrorReporting.UseVisualStyleBackColor = true;
            // 
            // checkBox_UseNewSettingsPage
            // 
            this.checkBox_UseNewSettingsPage.AutoSize = true;
            this.checkBox_UseNewSettingsPage.Location = new System.Drawing.Point(6, 271);
            this.checkBox_UseNewSettingsPage.Name = "checkBox_UseNewSettingsPage";
            this.checkBox_UseNewSettingsPage.Size = new System.Drawing.Size(130, 17);
            this.checkBox_UseNewSettingsPage.TabIndex = 62;
            this.checkBox_UseNewSettingsPage.Text = "UseNewSettingsPage";
            this.checkBox_UseNewSettingsPage.UseVisualStyleBackColor = true;
            // 
            // textBox_LogMaxFileSize
            // 
            this.textBox_LogMaxFileSize.Location = new System.Drawing.Point(501, 170);
            this.textBox_LogMaxFileSize.Name = "textBox_LogMaxFileSize";
            this.textBox_LogMaxFileSize.Size = new System.Drawing.Size(139, 20);
            this.textBox_LogMaxFileSize.TabIndex = 61;
            // 
            // label_LogMaxFileSize
            // 
            this.label_LogMaxFileSize.AutoSize = true;
            this.label_LogMaxFileSize.Location = new System.Drawing.Point(498, 150);
            this.label_LogMaxFileSize.Name = "label_LogMaxFileSize";
            this.label_LogMaxFileSize.Size = new System.Drawing.Size(84, 13);
            this.label_LogMaxFileSize.TabIndex = 60;
            this.label_LogMaxFileSize.Text = "LogMaxFileSize:";
            // 
            // textBox_LogLevel
            // 
            this.textBox_LogLevel.Location = new System.Drawing.Point(501, 122);
            this.textBox_LogLevel.Name = "textBox_LogLevel";
            this.textBox_LogLevel.Size = new System.Drawing.Size(139, 20);
            this.textBox_LogLevel.TabIndex = 58;
            // 
            // label_LogLevel
            // 
            this.label_LogLevel.AutoSize = true;
            this.label_LogLevel.Location = new System.Drawing.Point(498, 102);
            this.label_LogLevel.Name = "label_LogLevel";
            this.label_LogLevel.Size = new System.Drawing.Size(54, 13);
            this.label_LogLevel.TabIndex = 57;
            this.label_LogLevel.Text = "LogLevel:";
            // 
            // textBox_MinIdleSeconds
            // 
            this.textBox_MinIdleSeconds.Location = new System.Drawing.Point(501, 75);
            this.textBox_MinIdleSeconds.Name = "textBox_MinIdleSeconds";
            this.textBox_MinIdleSeconds.Size = new System.Drawing.Size(139, 20);
            this.textBox_MinIdleSeconds.TabIndex = 56;
            // 
            // label_MinIdleSeconds
            // 
            this.label_MinIdleSeconds.AutoSize = true;
            this.label_MinIdleSeconds.Location = new System.Drawing.Point(498, 55);
            this.label_MinIdleSeconds.Name = "label_MinIdleSeconds";
            this.label_MinIdleSeconds.Size = new System.Drawing.Size(86, 13);
            this.label_MinIdleSeconds.TabIndex = 55;
            this.label_MinIdleSeconds.Text = "MinIdleSeconds:";
            // 
            // label_BenchmarkTimeLimitsAMD_Precise
            // 
            this.label_BenchmarkTimeLimitsAMD_Precise.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Precise.Location = new System.Drawing.Point(760, 276);
            this.label_BenchmarkTimeLimitsAMD_Precise.Name = "label_BenchmarkTimeLimitsAMD_Precise";
            this.label_BenchmarkTimeLimitsAMD_Precise.Size = new System.Drawing.Size(42, 13);
            this.label_BenchmarkTimeLimitsAMD_Precise.TabIndex = 49;
            this.label_BenchmarkTimeLimitsAMD_Precise.Text = "Precise";
            // 
            // textBox_BenchmarkTimeLimitsAMD_Precise
            // 
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Location = new System.Drawing.Point(654, 273);
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Name = "textBox_BenchmarkTimeLimitsAMD_Precise";
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsAMD_Precise.TabIndex = 48;
            // 
            // label_BenchmarkTimeLimitsAMD_Standard
            // 
            this.label_BenchmarkTimeLimitsAMD_Standard.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Standard.Location = new System.Drawing.Point(760, 251);
            this.label_BenchmarkTimeLimitsAMD_Standard.Name = "label_BenchmarkTimeLimitsAMD_Standard";
            this.label_BenchmarkTimeLimitsAMD_Standard.Size = new System.Drawing.Size(50, 13);
            this.label_BenchmarkTimeLimitsAMD_Standard.TabIndex = 47;
            this.label_BenchmarkTimeLimitsAMD_Standard.Text = "Standard";
            // 
            // textBox_BenchmarkTimeLimitsAMD_Standard
            // 
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Location = new System.Drawing.Point(654, 248);
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Name = "textBox_BenchmarkTimeLimitsAMD_Standard";
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsAMD_Standard.TabIndex = 46;
            // 
            // label_BenchmarkTimeLimitsAMD_Quick
            // 
            this.label_BenchmarkTimeLimitsAMD_Quick.AutoSize = true;
            this.label_BenchmarkTimeLimitsAMD_Quick.Location = new System.Drawing.Point(760, 226);
            this.label_BenchmarkTimeLimitsAMD_Quick.Name = "label_BenchmarkTimeLimitsAMD_Quick";
            this.label_BenchmarkTimeLimitsAMD_Quick.Size = new System.Drawing.Size(35, 13);
            this.label_BenchmarkTimeLimitsAMD_Quick.TabIndex = 45;
            this.label_BenchmarkTimeLimitsAMD_Quick.Text = "Quick";
            // 
            // textBox_BenchmarkTimeLimitsAMD_Quick
            // 
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Location = new System.Drawing.Point(654, 223);
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Name = "textBox_BenchmarkTimeLimitsAMD_Quick";
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsAMD_Quick.TabIndex = 44;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(651, 205);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(137, 13);
            this.label22.TabIndex = 43;
            this.label22.Text = "BenchmarkTimeLimitsAMD:";
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Precise
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Location = new System.Drawing.Point(760, 177);
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Name = "label_BenchmarkTimeLimitsNVIDIA_Precise";
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Size = new System.Drawing.Size(42, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.TabIndex = 42;
            this.label_BenchmarkTimeLimitsNVIDIA_Precise.Text = "Precise";
            // 
            // textBox_BenchmarkTimeLimitsNVIDIA_Precise
            // 
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Location = new System.Drawing.Point(654, 174);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Name = "textBox_BenchmarkTimeLimitsNVIDIA_Precise";
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.TabIndex = 41;
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Standard
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Location = new System.Drawing.Point(760, 152);
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Name = "label_BenchmarkTimeLimitsNVIDIA_Standard";
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Size = new System.Drawing.Size(50, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.TabIndex = 40;
            this.label_BenchmarkTimeLimitsNVIDIA_Standard.Text = "Standard";
            // 
            // textBox_BenchmarkTimeLimitsNVIDIA_Standard
            // 
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Location = new System.Drawing.Point(654, 149);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Name = "textBox_BenchmarkTimeLimitsNVIDIA_Standard";
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.TabIndex = 39;
            // 
            // label_BenchmarkTimeLimitsNVIDIA_Quick
            // 
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.AutoSize = true;
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Location = new System.Drawing.Point(760, 127);
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Name = "label_BenchmarkTimeLimitsNVIDIA_Quick";
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Size = new System.Drawing.Size(35, 13);
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.TabIndex = 38;
            this.label_BenchmarkTimeLimitsNVIDIA_Quick.Text = "Quick";
            // 
            // textBox_BenchmarkTimeLimitsNVIDIA_Quick
            // 
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Location = new System.Drawing.Point(654, 124);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Name = "textBox_BenchmarkTimeLimitsNVIDIA_Quick";
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.TabIndex = 37;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(651, 106);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(149, 13);
            this.label14.TabIndex = 36;
            this.label14.Text = "BenchmarkTimeLimitsNVIDIA:";
            // 
            // label_BenchmarkTimeLimitsCPU_Precise
            // 
            this.label_BenchmarkTimeLimitsCPU_Precise.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Precise.Location = new System.Drawing.Point(760, 78);
            this.label_BenchmarkTimeLimitsCPU_Precise.Name = "label_BenchmarkTimeLimitsCPU_Precise";
            this.label_BenchmarkTimeLimitsCPU_Precise.Size = new System.Drawing.Size(42, 13);
            this.label_BenchmarkTimeLimitsCPU_Precise.TabIndex = 35;
            this.label_BenchmarkTimeLimitsCPU_Precise.Text = "Precise";
            // 
            // textBox_BenchmarkTimeLimitsCPU_Precise
            // 
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Location = new System.Drawing.Point(654, 75);
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Name = "textBox_BenchmarkTimeLimitsCPU_Precise";
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsCPU_Precise.TabIndex = 34;
            // 
            // label_BenchmarkTimeLimitsCPU_Standard
            // 
            this.label_BenchmarkTimeLimitsCPU_Standard.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Standard.Location = new System.Drawing.Point(760, 53);
            this.label_BenchmarkTimeLimitsCPU_Standard.Name = "label_BenchmarkTimeLimitsCPU_Standard";
            this.label_BenchmarkTimeLimitsCPU_Standard.Size = new System.Drawing.Size(50, 13);
            this.label_BenchmarkTimeLimitsCPU_Standard.TabIndex = 33;
            this.label_BenchmarkTimeLimitsCPU_Standard.Text = "Standard";
            // 
            // textBox_BenchmarkTimeLimitsCPU_Standard
            // 
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Location = new System.Drawing.Point(654, 50);
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Name = "textBox_BenchmarkTimeLimitsCPU_Standard";
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsCPU_Standard.TabIndex = 32;
            // 
            // label_BenchmarkTimeLimitsCPU_Quick
            // 
            this.label_BenchmarkTimeLimitsCPU_Quick.AutoSize = true;
            this.label_BenchmarkTimeLimitsCPU_Quick.Location = new System.Drawing.Point(760, 28);
            this.label_BenchmarkTimeLimitsCPU_Quick.Name = "label_BenchmarkTimeLimitsCPU_Quick";
            this.label_BenchmarkTimeLimitsCPU_Quick.Size = new System.Drawing.Size(35, 13);
            this.label_BenchmarkTimeLimitsCPU_Quick.TabIndex = 31;
            this.label_BenchmarkTimeLimitsCPU_Quick.Text = "Quick";
            // 
            // textBox_BenchmarkTimeLimitsCPU_Quick
            // 
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Location = new System.Drawing.Point(654, 25);
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Name = "textBox_BenchmarkTimeLimitsCPU_Quick";
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Size = new System.Drawing.Size(100, 20);
            this.textBox_BenchmarkTimeLimitsCPU_Quick.TabIndex = 30;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(651, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(135, 13);
            this.label18.TabIndex = 29;
            this.label18.Text = "BenchmarkTimeLimitsCPU:";
            // 
            // textBox_MinerAPIGraceMinutes
            // 
            this.textBox_MinerAPIGraceMinutes.Location = new System.Drawing.Point(351, 122);
            this.textBox_MinerAPIGraceMinutes.Name = "textBox_MinerAPIGraceMinutes";
            this.textBox_MinerAPIGraceMinutes.Size = new System.Drawing.Size(131, 20);
            this.textBox_MinerAPIGraceMinutes.TabIndex = 26;
            // 
            // label_MinerAPIGraceMinutes
            // 
            this.label_MinerAPIGraceMinutes.AutoSize = true;
            this.label_MinerAPIGraceMinutes.Location = new System.Drawing.Point(348, 102);
            this.label_MinerAPIGraceMinutes.Name = "label_MinerAPIGraceMinutes";
            this.label_MinerAPIGraceMinutes.Size = new System.Drawing.Size(119, 13);
            this.label_MinerAPIGraceMinutes.TabIndex = 25;
            this.label_MinerAPIGraceMinutes.Text = "MinerAPIGraceMinutes:";
            // 
            // textBox_MinerRestartDelayMS
            // 
            this.textBox_MinerRestartDelayMS.Location = new System.Drawing.Point(351, 75);
            this.textBox_MinerRestartDelayMS.Name = "textBox_MinerRestartDelayMS";
            this.textBox_MinerRestartDelayMS.Size = new System.Drawing.Size(131, 20);
            this.textBox_MinerRestartDelayMS.TabIndex = 24;
            // 
            // label_MinerRestartDelayMS
            // 
            this.label_MinerRestartDelayMS.AutoSize = true;
            this.label_MinerRestartDelayMS.Location = new System.Drawing.Point(348, 55);
            this.label_MinerRestartDelayMS.Name = "label_MinerRestartDelayMS";
            this.label_MinerRestartDelayMS.Size = new System.Drawing.Size(113, 13);
            this.label_MinerRestartDelayMS.TabIndex = 23;
            this.label_MinerRestartDelayMS.Text = "MinerRestartDelayMS:";
            // 
            // textBox_MinerAPIQueryInterval
            // 
            this.textBox_MinerAPIQueryInterval.Location = new System.Drawing.Point(351, 170);
            this.textBox_MinerAPIQueryInterval.Name = "textBox_MinerAPIQueryInterval";
            this.textBox_MinerAPIQueryInterval.Size = new System.Drawing.Size(131, 20);
            this.textBox_MinerAPIQueryInterval.TabIndex = 22;
            // 
            // label_MinerAPIQueryInterval
            // 
            this.label_MinerAPIQueryInterval.AutoSize = true;
            this.label_MinerAPIQueryInterval.Location = new System.Drawing.Point(348, 150);
            this.label_MinerAPIQueryInterval.Name = "label_MinerAPIQueryInterval";
            this.label_MinerAPIQueryInterval.Size = new System.Drawing.Size(116, 13);
            this.label_MinerAPIQueryInterval.TabIndex = 21;
            this.label_MinerAPIQueryInterval.Text = "MinerAPIQueryInterval:";
            // 
            // textBox_SwitchMinSecondsDynamic
            // 
            this.textBox_SwitchMinSecondsDynamic.Location = new System.Drawing.Point(189, 217);
            this.textBox_SwitchMinSecondsDynamic.Name = "textBox_SwitchMinSecondsDynamic";
            this.textBox_SwitchMinSecondsDynamic.Size = new System.Drawing.Size(147, 20);
            this.textBox_SwitchMinSecondsDynamic.TabIndex = 20;
            // 
            // label_SwitchMinSecondsDynamic
            // 
            this.label_SwitchMinSecondsDynamic.AutoSize = true;
            this.label_SwitchMinSecondsDynamic.Location = new System.Drawing.Point(186, 197);
            this.label_SwitchMinSecondsDynamic.Name = "label_SwitchMinSecondsDynamic";
            this.label_SwitchMinSecondsDynamic.Size = new System.Drawing.Size(142, 13);
            this.label_SwitchMinSecondsDynamic.TabIndex = 19;
            this.label_SwitchMinSecondsDynamic.Text = "SwitchMinSecondsDynamic:";
            // 
            // label_SwitchMinSecondsFixed
            // 
            this.label_SwitchMinSecondsFixed.AutoSize = true;
            this.label_SwitchMinSecondsFixed.Location = new System.Drawing.Point(186, 150);
            this.label_SwitchMinSecondsFixed.Name = "label_SwitchMinSecondsFixed";
            this.label_SwitchMinSecondsFixed.Size = new System.Drawing.Size(126, 13);
            this.label_SwitchMinSecondsFixed.TabIndex = 18;
            this.label_SwitchMinSecondsFixed.Text = "SwitchMinSecondsFixed:";
            // 
            // textBox_SwitchMinSecondsFixed
            // 
            this.textBox_SwitchMinSecondsFixed.Location = new System.Drawing.Point(189, 170);
            this.textBox_SwitchMinSecondsFixed.Name = "textBox_SwitchMinSecondsFixed";
            this.textBox_SwitchMinSecondsFixed.Size = new System.Drawing.Size(147, 20);
            this.textBox_SwitchMinSecondsFixed.TabIndex = 17;
            // 
            // comboBox_Location
            // 
            this.comboBox_Location.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Location.FormattingEnabled = true;
            this.comboBox_Location.Items.AddRange(new object[] {
            "Europe - Amsterdam",
            "USA - San Jose",
            "China - Hong Kong",
            "Japan - Tokyo"});
            this.comboBox_Location.Location = new System.Drawing.Point(189, 122);
            this.comboBox_Location.Name = "comboBox_Location";
            this.comboBox_Location.Size = new System.Drawing.Size(147, 21);
            this.comboBox_Location.TabIndex = 16;
            // 
            // label_Location
            // 
            this.label_Location.AutoSize = true;
            this.label_Location.Location = new System.Drawing.Point(186, 102);
            this.label_Location.Name = "label_Location";
            this.label_Location.Size = new System.Drawing.Size(51, 13);
            this.label_Location.TabIndex = 15;
            this.label_Location.Text = "Location:";
            // 
            // textBox_WorkerName
            // 
            this.textBox_WorkerName.Location = new System.Drawing.Point(501, 27);
            this.textBox_WorkerName.Name = "textBox_WorkerName";
            this.textBox_WorkerName.Size = new System.Drawing.Size(139, 20);
            this.textBox_WorkerName.TabIndex = 14;
            // 
            // textBox_BitcoinAddress
            // 
            this.textBox_BitcoinAddress.Location = new System.Drawing.Point(189, 28);
            this.textBox_BitcoinAddress.Name = "textBox_BitcoinAddress";
            this.textBox_BitcoinAddress.Size = new System.Drawing.Size(293, 20);
            this.textBox_BitcoinAddress.TabIndex = 13;
            // 
            // label_WorkerName
            // 
            this.label_WorkerName.AutoSize = true;
            this.label_WorkerName.Location = new System.Drawing.Point(498, 8);
            this.label_WorkerName.Name = "label_WorkerName";
            this.label_WorkerName.Size = new System.Drawing.Size(73, 13);
            this.label_WorkerName.TabIndex = 12;
            this.label_WorkerName.Text = "WorkerName:";
            // 
            // label_BitcoinAddress
            // 
            this.label_BitcoinAddress.AutoSize = true;
            this.label_BitcoinAddress.Location = new System.Drawing.Point(186, 8);
            this.label_BitcoinAddress.Name = "label_BitcoinAddress";
            this.label_BitcoinAddress.Size = new System.Drawing.Size(80, 13);
            this.label_BitcoinAddress.TabIndex = 11;
            this.label_BitcoinAddress.Text = "BitcoinAddress:";
            // 
            // checkBox_ShowDriverVersionWarning
            // 
            this.checkBox_ShowDriverVersionWarning.AutoSize = true;
            this.checkBox_ShowDriverVersionWarning.Location = new System.Drawing.Point(6, 227);
            this.checkBox_ShowDriverVersionWarning.Name = "checkBox_ShowDriverVersionWarning";
            this.checkBox_ShowDriverVersionWarning.Size = new System.Drawing.Size(156, 17);
            this.checkBox_ShowDriverVersionWarning.TabIndex = 10;
            this.checkBox_ShowDriverVersionWarning.Text = "ShowDriverVersionWarning";
            this.checkBox_ShowDriverVersionWarning.UseVisualStyleBackColor = true;
            // 
            // checkBox_StartMiningWhenIdle
            // 
            this.checkBox_StartMiningWhenIdle.AutoSize = true;
            this.checkBox_StartMiningWhenIdle.Location = new System.Drawing.Point(7, 205);
            this.checkBox_StartMiningWhenIdle.Name = "checkBox_StartMiningWhenIdle";
            this.checkBox_StartMiningWhenIdle.Size = new System.Drawing.Size(125, 17);
            this.checkBox_StartMiningWhenIdle.TabIndex = 9;
            this.checkBox_StartMiningWhenIdle.Text = "StartMiningWhenIdle";
            this.checkBox_StartMiningWhenIdle.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoScaleBTCValues
            // 
            this.checkBox_AutoScaleBTCValues.AutoSize = true;
            this.checkBox_AutoScaleBTCValues.Location = new System.Drawing.Point(7, 183);
            this.checkBox_AutoScaleBTCValues.Name = "checkBox_AutoScaleBTCValues";
            this.checkBox_AutoScaleBTCValues.Size = new System.Drawing.Size(128, 17);
            this.checkBox_AutoScaleBTCValues.TabIndex = 8;
            this.checkBox_AutoScaleBTCValues.Text = "AutoScaleBTCValues";
            this.checkBox_AutoScaleBTCValues.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionAMD
            // 
            this.checkBox_DisableDetectionAMD.AutoSize = true;
            this.checkBox_DisableDetectionAMD.Location = new System.Drawing.Point(7, 161);
            this.checkBox_DisableDetectionAMD.Name = "checkBox_DisableDetectionAMD";
            this.checkBox_DisableDetectionAMD.Size = new System.Drawing.Size(131, 17);
            this.checkBox_DisableDetectionAMD.TabIndex = 7;
            this.checkBox_DisableDetectionAMD.Text = "DisableDetectionAMD";
            this.checkBox_DisableDetectionAMD.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionNVidia2X
            // 
            this.checkBox_DisableDetectionNVidia2X.AutoSize = true;
            this.checkBox_DisableDetectionNVidia2X.Location = new System.Drawing.Point(7, 139);
            this.checkBox_DisableDetectionNVidia2X.Name = "checkBox_DisableDetectionNVidia2X";
            this.checkBox_DisableDetectionNVidia2X.Size = new System.Drawing.Size(151, 17);
            this.checkBox_DisableDetectionNVidia2X.TabIndex = 6;
            this.checkBox_DisableDetectionNVidia2X.Text = "DisableDetectionNVidia2X";
            this.checkBox_DisableDetectionNVidia2X.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionNVidia3X
            // 
            this.checkBox_DisableDetectionNVidia3X.AutoSize = true;
            this.checkBox_DisableDetectionNVidia3X.Location = new System.Drawing.Point(7, 117);
            this.checkBox_DisableDetectionNVidia3X.Name = "checkBox_DisableDetectionNVidia3X";
            this.checkBox_DisableDetectionNVidia3X.Size = new System.Drawing.Size(151, 17);
            this.checkBox_DisableDetectionNVidia3X.TabIndex = 5;
            this.checkBox_DisableDetectionNVidia3X.Text = "DisableDetectionNVidia3X";
            this.checkBox_DisableDetectionNVidia3X.UseVisualStyleBackColor = true;
            // 
            // checkBox_DisableDetectionNVidia5X
            // 
            this.checkBox_DisableDetectionNVidia5X.AutoSize = true;
            this.checkBox_DisableDetectionNVidia5X.Location = new System.Drawing.Point(7, 95);
            this.checkBox_DisableDetectionNVidia5X.Name = "checkBox_DisableDetectionNVidia5X";
            this.checkBox_DisableDetectionNVidia5X.Size = new System.Drawing.Size(151, 17);
            this.checkBox_DisableDetectionNVidia5X.TabIndex = 4;
            this.checkBox_DisableDetectionNVidia5X.Text = "DisableDetectionNVidia5X";
            this.checkBox_DisableDetectionNVidia5X.UseVisualStyleBackColor = true;
            // 
            // checkBox_MinimizeToTray
            // 
            this.checkBox_MinimizeToTray.AutoSize = true;
            this.checkBox_MinimizeToTray.Location = new System.Drawing.Point(7, 73);
            this.checkBox_MinimizeToTray.Name = "checkBox_MinimizeToTray";
            this.checkBox_MinimizeToTray.Size = new System.Drawing.Size(100, 17);
            this.checkBox_MinimizeToTray.TabIndex = 3;
            this.checkBox_MinimizeToTray.Text = "MinimizeToTray";
            this.checkBox_MinimizeToTray.UseVisualStyleBackColor = true;
            // 
            // checkBox_HideMiningWindows
            // 
            this.checkBox_HideMiningWindows.AutoSize = true;
            this.checkBox_HideMiningWindows.Location = new System.Drawing.Point(7, 51);
            this.checkBox_HideMiningWindows.Name = "checkBox_HideMiningWindows";
            this.checkBox_HideMiningWindows.Size = new System.Drawing.Size(123, 17);
            this.checkBox_HideMiningWindows.TabIndex = 2;
            this.checkBox_HideMiningWindows.Text = "HideMiningWindows";
            this.checkBox_HideMiningWindows.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoStartMining
            // 
            this.checkBox_AutoStartMining.AutoSize = true;
            this.checkBox_AutoStartMining.Location = new System.Drawing.Point(7, 29);
            this.checkBox_AutoStartMining.Name = "checkBox_AutoStartMining";
            this.checkBox_AutoStartMining.Size = new System.Drawing.Size(101, 17);
            this.checkBox_AutoStartMining.TabIndex = 1;
            this.checkBox_AutoStartMining.Text = "AutoStartMining";
            this.checkBox_AutoStartMining.UseVisualStyleBackColor = true;
            // 
            // checkBox_DebugConsole
            // 
            this.checkBox_DebugConsole.AutoSize = true;
            this.checkBox_DebugConsole.Location = new System.Drawing.Point(7, 7);
            this.checkBox_DebugConsole.Name = "checkBox_DebugConsole";
            this.checkBox_DebugConsole.Size = new System.Drawing.Size(96, 17);
            this.checkBox_DebugConsole.TabIndex = 0;
            this.checkBox_DebugConsole.Text = "DebugConsole";
            this.checkBox_DebugConsole.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage_NVIDIA5X);
            this.tabControl1.Controls.Add(this.tabPage_NVIDIA3X);
            this.tabControl1.Controls.Add(this.tabPage_NVIDIA2X);
            this.tabControl1.Controls.Add(this.tabPage_AMD);
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(831, 346);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_NVIDIA5X
            // 
            this.tabPage_NVIDIA5X.Controls.Add(this.textBox_NVIDIA5X_MinimumProfit);
            this.tabPage_NVIDIA5X.Controls.Add(this.label_NVIDIA5X_MinimumProfit);
            this.tabPage_NVIDIA5X.Controls.Add(this.label46);
            this.tabPage_NVIDIA5X.Controls.Add(this.tabControl_NVIDIA5X_Algos);
            this.tabPage_NVIDIA5X.Controls.Add(this.label_NVIDIA5X_ExtraLaunchParameters);
            this.tabPage_NVIDIA5X.Controls.Add(this.textBox_NVIDIA5X_ExtraLaunchParameters);
            this.tabPage_NVIDIA5X.Controls.Add(this.textBox_NVIDIA5X_UsePassword);
            this.tabPage_NVIDIA5X.Controls.Add(this.label_NVIDIA5X_UsePassword);
            this.tabPage_NVIDIA5X.Controls.Add(this.textBox_NVIDIA5X_APIBindPort);
            this.tabPage_NVIDIA5X.Controls.Add(this.label_NVIDIA5X_APIBindPort);
            this.tabPage_NVIDIA5X.Location = new System.Drawing.Point(4, 22);
            this.tabPage_NVIDIA5X.Name = "tabPage_NVIDIA5X";
            this.tabPage_NVIDIA5X.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_NVIDIA5X.Size = new System.Drawing.Size(823, 320);
            this.tabPage_NVIDIA5X.TabIndex = 2;
            this.tabPage_NVIDIA5X.Text = "NVIDIA5X";
            this.tabPage_NVIDIA5X.UseVisualStyleBackColor = true;
            // 
            // textBox_NVIDIA5X_MinimumProfit
            // 
            this.textBox_NVIDIA5X_MinimumProfit.Location = new System.Drawing.Point(487, 6);
            this.textBox_NVIDIA5X_MinimumProfit.Name = "textBox_NVIDIA5X_MinimumProfit";
            this.textBox_NVIDIA5X_MinimumProfit.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA5X_MinimumProfit.TabIndex = 3;
            // 
            // label_NVIDIA5X_MinimumProfit
            // 
            this.label_NVIDIA5X_MinimumProfit.AutoSize = true;
            this.label_NVIDIA5X_MinimumProfit.Location = new System.Drawing.Point(372, 9);
            this.label_NVIDIA5X_MinimumProfit.Name = "label_NVIDIA5X_MinimumProfit";
            this.label_NVIDIA5X_MinimumProfit.Size = new System.Drawing.Size(112, 13);
            this.label_NVIDIA5X_MinimumProfit.TabIndex = 99;
            this.label_NVIDIA5X_MinimumProfit.Text = "MinimumProfit ($/day):";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(6, 33);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(90, 13);
            this.label46.TabIndex = 99;
            this.label46.Text = "DisabledDevices:";
            // 
            // tabControl_NVIDIA5X_Algos
            // 
            this.tabControl_NVIDIA5X_Algos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_NVIDIA5X_Algos.Location = new System.Drawing.Point(9, 81);
            this.tabControl_NVIDIA5X_Algos.Name = "tabControl_NVIDIA5X_Algos";
            this.tabControl_NVIDIA5X_Algos.SelectedIndex = 0;
            this.tabControl_NVIDIA5X_Algos.Size = new System.Drawing.Size(808, 233);
            this.tabControl_NVIDIA5X_Algos.TabIndex = 99;
            // 
            // label_NVIDIA5X_ExtraLaunchParameters
            // 
            this.label_NVIDIA5X_ExtraLaunchParameters.AutoSize = true;
            this.label_NVIDIA5X_ExtraLaunchParameters.Location = new System.Drawing.Point(6, 58);
            this.label_NVIDIA5X_ExtraLaunchParameters.Name = "label_NVIDIA5X_ExtraLaunchParameters";
            this.label_NVIDIA5X_ExtraLaunchParameters.Size = new System.Drawing.Size(123, 13);
            this.label_NVIDIA5X_ExtraLaunchParameters.TabIndex = 99;
            this.label_NVIDIA5X_ExtraLaunchParameters.Text = "ExtraLaunchParameters:";
            // 
            // textBox_NVIDIA5X_ExtraLaunchParameters
            // 
            this.textBox_NVIDIA5X_ExtraLaunchParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_NVIDIA5X_ExtraLaunchParameters.Location = new System.Drawing.Point(135, 55);
            this.textBox_NVIDIA5X_ExtraLaunchParameters.Name = "textBox_NVIDIA5X_ExtraLaunchParameters";
            this.textBox_NVIDIA5X_ExtraLaunchParameters.Size = new System.Drawing.Size(682, 20);
            this.textBox_NVIDIA5X_ExtraLaunchParameters.TabIndex = 99;
            // 
            // textBox_NVIDIA5X_UsePassword
            // 
            this.textBox_NVIDIA5X_UsePassword.Location = new System.Drawing.Point(263, 6);
            this.textBox_NVIDIA5X_UsePassword.Name = "textBox_NVIDIA5X_UsePassword";
            this.textBox_NVIDIA5X_UsePassword.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA5X_UsePassword.TabIndex = 2;
            // 
            // label_NVIDIA5X_UsePassword
            // 
            this.label_NVIDIA5X_UsePassword.AutoSize = true;
            this.label_NVIDIA5X_UsePassword.Location = new System.Drawing.Point(185, 9);
            this.label_NVIDIA5X_UsePassword.Name = "label_NVIDIA5X_UsePassword";
            this.label_NVIDIA5X_UsePassword.Size = new System.Drawing.Size(75, 13);
            this.label_NVIDIA5X_UsePassword.TabIndex = 99;
            this.label_NVIDIA5X_UsePassword.Text = "UsePassword:";
            // 
            // textBox_NVIDIA5X_APIBindPort
            // 
            this.textBox_NVIDIA5X_APIBindPort.Location = new System.Drawing.Point(76, 6);
            this.textBox_NVIDIA5X_APIBindPort.Name = "textBox_NVIDIA5X_APIBindPort";
            this.textBox_NVIDIA5X_APIBindPort.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA5X_APIBindPort.TabIndex = 1;
            // 
            // label_NVIDIA5X_APIBindPort
            // 
            this.label_NVIDIA5X_APIBindPort.AutoSize = true;
            this.label_NVIDIA5X_APIBindPort.Location = new System.Drawing.Point(6, 9);
            this.label_NVIDIA5X_APIBindPort.Name = "label_NVIDIA5X_APIBindPort";
            this.label_NVIDIA5X_APIBindPort.Size = new System.Drawing.Size(67, 13);
            this.label_NVIDIA5X_APIBindPort.TabIndex = 99;
            this.label_NVIDIA5X_APIBindPort.Text = "APIBindPort:";
            // 
            // tabPage_NVIDIA3X
            // 
            this.tabPage_NVIDIA3X.Controls.Add(this.textBox_NVIDIA3X_MinimumProfit);
            this.tabPage_NVIDIA3X.Controls.Add(this.label_NVIDIA3X_MinimumProfit);
            this.tabPage_NVIDIA3X.Controls.Add(this.label45);
            this.tabPage_NVIDIA3X.Controls.Add(this.tabControl_NVIDIA3X_Algos);
            this.tabPage_NVIDIA3X.Controls.Add(this.label_NVIDIA3X_ExtraLaunchParameters);
            this.tabPage_NVIDIA3X.Controls.Add(this.textBox_NVIDIA3X_ExtraLaunchParameters);
            this.tabPage_NVIDIA3X.Controls.Add(this.textBox_NVIDIA3X_UsePassword);
            this.tabPage_NVIDIA3X.Controls.Add(this.label_NVIDIA3X_UsePassword);
            this.tabPage_NVIDIA3X.Controls.Add(this.textBox_NVIDIA3X_APIBindPort);
            this.tabPage_NVIDIA3X.Controls.Add(this.label_NVIDIA3X_APIBindPort);
            this.tabPage_NVIDIA3X.Location = new System.Drawing.Point(4, 22);
            this.tabPage_NVIDIA3X.Name = "tabPage_NVIDIA3X";
            this.tabPage_NVIDIA3X.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_NVIDIA3X.Size = new System.Drawing.Size(823, 320);
            this.tabPage_NVIDIA3X.TabIndex = 3;
            this.tabPage_NVIDIA3X.Text = "NVIDIA3X";
            this.tabPage_NVIDIA3X.UseVisualStyleBackColor = true;
            // 
            // textBox_NVIDIA3X_MinimumProfit
            // 
            this.textBox_NVIDIA3X_MinimumProfit.Location = new System.Drawing.Point(487, 6);
            this.textBox_NVIDIA3X_MinimumProfit.Name = "textBox_NVIDIA3X_MinimumProfit";
            this.textBox_NVIDIA3X_MinimumProfit.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA3X_MinimumProfit.TabIndex = 3;
            // 
            // label_NVIDIA3X_MinimumProfit
            // 
            this.label_NVIDIA3X_MinimumProfit.AutoSize = true;
            this.label_NVIDIA3X_MinimumProfit.Location = new System.Drawing.Point(372, 9);
            this.label_NVIDIA3X_MinimumProfit.Name = "label_NVIDIA3X_MinimumProfit";
            this.label_NVIDIA3X_MinimumProfit.Size = new System.Drawing.Size(112, 13);
            this.label_NVIDIA3X_MinimumProfit.TabIndex = 99;
            this.label_NVIDIA3X_MinimumProfit.Text = "MinimumProfit ($/day):";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(6, 33);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(90, 13);
            this.label45.TabIndex = 99;
            this.label45.Text = "DisabledDevices:";
            // 
            // tabControl_NVIDIA3X_Algos
            // 
            this.tabControl_NVIDIA3X_Algos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_NVIDIA3X_Algos.Location = new System.Drawing.Point(9, 81);
            this.tabControl_NVIDIA3X_Algos.Name = "tabControl_NVIDIA3X_Algos";
            this.tabControl_NVIDIA3X_Algos.SelectedIndex = 0;
            this.tabControl_NVIDIA3X_Algos.Size = new System.Drawing.Size(808, 233);
            this.tabControl_NVIDIA3X_Algos.TabIndex = 99;
            // 
            // label_NVIDIA3X_ExtraLaunchParameters
            // 
            this.label_NVIDIA3X_ExtraLaunchParameters.AutoSize = true;
            this.label_NVIDIA3X_ExtraLaunchParameters.Location = new System.Drawing.Point(6, 58);
            this.label_NVIDIA3X_ExtraLaunchParameters.Name = "label_NVIDIA3X_ExtraLaunchParameters";
            this.label_NVIDIA3X_ExtraLaunchParameters.Size = new System.Drawing.Size(123, 13);
            this.label_NVIDIA3X_ExtraLaunchParameters.TabIndex = 99;
            this.label_NVIDIA3X_ExtraLaunchParameters.Text = "ExtraLaunchParameters:";
            // 
            // textBox_NVIDIA3X_ExtraLaunchParameters
            // 
            this.textBox_NVIDIA3X_ExtraLaunchParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_NVIDIA3X_ExtraLaunchParameters.Location = new System.Drawing.Point(135, 55);
            this.textBox_NVIDIA3X_ExtraLaunchParameters.Name = "textBox_NVIDIA3X_ExtraLaunchParameters";
            this.textBox_NVIDIA3X_ExtraLaunchParameters.Size = new System.Drawing.Size(658, 20);
            this.textBox_NVIDIA3X_ExtraLaunchParameters.TabIndex = 99;
            // 
            // textBox_NVIDIA3X_UsePassword
            // 
            this.textBox_NVIDIA3X_UsePassword.Location = new System.Drawing.Point(263, 6);
            this.textBox_NVIDIA3X_UsePassword.Name = "textBox_NVIDIA3X_UsePassword";
            this.textBox_NVIDIA3X_UsePassword.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA3X_UsePassword.TabIndex = 2;
            // 
            // label_NVIDIA3X_UsePassword
            // 
            this.label_NVIDIA3X_UsePassword.AutoSize = true;
            this.label_NVIDIA3X_UsePassword.Location = new System.Drawing.Point(185, 9);
            this.label_NVIDIA3X_UsePassword.Name = "label_NVIDIA3X_UsePassword";
            this.label_NVIDIA3X_UsePassword.Size = new System.Drawing.Size(75, 13);
            this.label_NVIDIA3X_UsePassword.TabIndex = 99;
            this.label_NVIDIA3X_UsePassword.Text = "UsePassword:";
            // 
            // textBox_NVIDIA3X_APIBindPort
            // 
            this.textBox_NVIDIA3X_APIBindPort.Location = new System.Drawing.Point(76, 6);
            this.textBox_NVIDIA3X_APIBindPort.Name = "textBox_NVIDIA3X_APIBindPort";
            this.textBox_NVIDIA3X_APIBindPort.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA3X_APIBindPort.TabIndex = 1;
            // 
            // label_NVIDIA3X_APIBindPort
            // 
            this.label_NVIDIA3X_APIBindPort.AutoSize = true;
            this.label_NVIDIA3X_APIBindPort.Location = new System.Drawing.Point(6, 9);
            this.label_NVIDIA3X_APIBindPort.Name = "label_NVIDIA3X_APIBindPort";
            this.label_NVIDIA3X_APIBindPort.Size = new System.Drawing.Size(67, 13);
            this.label_NVIDIA3X_APIBindPort.TabIndex = 99;
            this.label_NVIDIA3X_APIBindPort.Text = "APIBindPort:";
            // 
            // tabPage_NVIDIA2X
            // 
            this.tabPage_NVIDIA2X.Controls.Add(this.textBox_NVIDIA2X_MinimumProfit);
            this.tabPage_NVIDIA2X.Controls.Add(this.label_NVIDIA2X_MinimumProfit);
            this.tabPage_NVIDIA2X.Controls.Add(this.label44);
            this.tabPage_NVIDIA2X.Controls.Add(this.tabControl_NVIDIA2X_Algos);
            this.tabPage_NVIDIA2X.Controls.Add(this.label_NVIDIA2X_ExtraLaunchParameters);
            this.tabPage_NVIDIA2X.Controls.Add(this.textBox_NVIDIA2X_ExtraLaunchParameters);
            this.tabPage_NVIDIA2X.Controls.Add(this.textBox_NVIDIA2X_UsePassword);
            this.tabPage_NVIDIA2X.Controls.Add(this.label_NVIDIA2X_UsePassword);
            this.tabPage_NVIDIA2X.Controls.Add(this.textBox_NVIDIA2X_APIBindPort);
            this.tabPage_NVIDIA2X.Controls.Add(this.label_NVIDIA2X_APIBindPort);
            this.tabPage_NVIDIA2X.Location = new System.Drawing.Point(4, 22);
            this.tabPage_NVIDIA2X.Name = "tabPage_NVIDIA2X";
            this.tabPage_NVIDIA2X.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_NVIDIA2X.Size = new System.Drawing.Size(823, 320);
            this.tabPage_NVIDIA2X.TabIndex = 4;
            this.tabPage_NVIDIA2X.Text = "NVIDIA2X";
            this.tabPage_NVIDIA2X.UseVisualStyleBackColor = true;
            // 
            // textBox_NVIDIA2X_MinimumProfit
            // 
            this.textBox_NVIDIA2X_MinimumProfit.Location = new System.Drawing.Point(487, 6);
            this.textBox_NVIDIA2X_MinimumProfit.Name = "textBox_NVIDIA2X_MinimumProfit";
            this.textBox_NVIDIA2X_MinimumProfit.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA2X_MinimumProfit.TabIndex = 3;
            // 
            // label_NVIDIA2X_MinimumProfit
            // 
            this.label_NVIDIA2X_MinimumProfit.AutoSize = true;
            this.label_NVIDIA2X_MinimumProfit.Location = new System.Drawing.Point(372, 9);
            this.label_NVIDIA2X_MinimumProfit.Name = "label_NVIDIA2X_MinimumProfit";
            this.label_NVIDIA2X_MinimumProfit.Size = new System.Drawing.Size(112, 13);
            this.label_NVIDIA2X_MinimumProfit.TabIndex = 99;
            this.label_NVIDIA2X_MinimumProfit.Text = "MinimumProfit ($/day):";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(6, 33);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(90, 13);
            this.label44.TabIndex = 99;
            this.label44.Text = "DisabledDevices:";
            // 
            // tabControl_NVIDIA2X_Algos
            // 
            this.tabControl_NVIDIA2X_Algos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_NVIDIA2X_Algos.Controls.Add(this.tabPage38);
            this.tabControl_NVIDIA2X_Algos.Location = new System.Drawing.Point(9, 81);
            this.tabControl_NVIDIA2X_Algos.Name = "tabControl_NVIDIA2X_Algos";
            this.tabControl_NVIDIA2X_Algos.SelectedIndex = 0;
            this.tabControl_NVIDIA2X_Algos.Size = new System.Drawing.Size(808, 233);
            this.tabControl_NVIDIA2X_Algos.TabIndex = 99;
            // 
            // tabPage38
            // 
            this.tabPage38.Controls.Add(this.textBox54);
            this.tabPage38.Controls.Add(this.label59);
            this.tabPage38.Controls.Add(this.checkBox18);
            this.tabPage38.Controls.Add(this.textBox55);
            this.tabPage38.Controls.Add(this.label60);
            this.tabPage38.Controls.Add(this.textBox56);
            this.tabPage38.Controls.Add(this.label61);
            this.tabPage38.Location = new System.Drawing.Point(4, 22);
            this.tabPage38.Name = "tabPage38";
            this.tabPage38.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage38.Size = new System.Drawing.Size(800, 207);
            this.tabPage38.TabIndex = 0;
            this.tabPage38.Text = "x11";
            this.tabPage38.UseVisualStyleBackColor = true;
            // 
            // textBox54
            // 
            this.textBox54.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox54.Location = new System.Drawing.Point(6, 45);
            this.textBox54.Name = "textBox54";
            this.textBox54.Size = new System.Drawing.Size(788, 20);
            this.textBox54.TabIndex = 25;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(3, 29);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(123, 13);
            this.label59.TabIndex = 99;
            this.label59.Text = "ExtraLaunchParameters:";
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Location = new System.Drawing.Point(6, 6);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(47, 17);
            this.checkBox18.TabIndex = 23;
            this.checkBox18.Text = "Skip";
            this.checkBox18.UseVisualStyleBackColor = true;
            // 
            // textBox55
            // 
            this.textBox55.Location = new System.Drawing.Point(353, 4);
            this.textBox55.Name = "textBox55";
            this.textBox55.Size = new System.Drawing.Size(100, 20);
            this.textBox55.TabIndex = 22;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(255, 7);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(95, 13);
            this.label60.TabIndex = 99;
            this.label60.Text = "BenchmarkSpeed:";
            // 
            // textBox56
            // 
            this.textBox56.Location = new System.Drawing.Point(137, 4);
            this.textBox56.Name = "textBox56";
            this.textBox56.Size = new System.Drawing.Size(100, 20);
            this.textBox56.TabIndex = 20;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(59, 7);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(75, 13);
            this.label61.TabIndex = 99;
            this.label61.Text = "UsePassword:";
            // 
            // label_NVIDIA2X_ExtraLaunchParameters
            // 
            this.label_NVIDIA2X_ExtraLaunchParameters.AutoSize = true;
            this.label_NVIDIA2X_ExtraLaunchParameters.Location = new System.Drawing.Point(6, 58);
            this.label_NVIDIA2X_ExtraLaunchParameters.Name = "label_NVIDIA2X_ExtraLaunchParameters";
            this.label_NVIDIA2X_ExtraLaunchParameters.Size = new System.Drawing.Size(123, 13);
            this.label_NVIDIA2X_ExtraLaunchParameters.TabIndex = 99;
            this.label_NVIDIA2X_ExtraLaunchParameters.Text = "ExtraLaunchParameters:";
            // 
            // textBox_NVIDIA2X_ExtraLaunchParameters
            // 
            this.textBox_NVIDIA2X_ExtraLaunchParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_NVIDIA2X_ExtraLaunchParameters.Location = new System.Drawing.Point(135, 55);
            this.textBox_NVIDIA2X_ExtraLaunchParameters.Name = "textBox_NVIDIA2X_ExtraLaunchParameters";
            this.textBox_NVIDIA2X_ExtraLaunchParameters.Size = new System.Drawing.Size(682, 20);
            this.textBox_NVIDIA2X_ExtraLaunchParameters.TabIndex = 99;
            // 
            // textBox_NVIDIA2X_UsePassword
            // 
            this.textBox_NVIDIA2X_UsePassword.Location = new System.Drawing.Point(263, 6);
            this.textBox_NVIDIA2X_UsePassword.Name = "textBox_NVIDIA2X_UsePassword";
            this.textBox_NVIDIA2X_UsePassword.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA2X_UsePassword.TabIndex = 2;
            // 
            // label_NVIDIA2X_UsePassword
            // 
            this.label_NVIDIA2X_UsePassword.AutoSize = true;
            this.label_NVIDIA2X_UsePassword.Location = new System.Drawing.Point(185, 9);
            this.label_NVIDIA2X_UsePassword.Name = "label_NVIDIA2X_UsePassword";
            this.label_NVIDIA2X_UsePassword.Size = new System.Drawing.Size(75, 13);
            this.label_NVIDIA2X_UsePassword.TabIndex = 99;
            this.label_NVIDIA2X_UsePassword.Text = "UsePassword:";
            // 
            // textBox_NVIDIA2X_APIBindPort
            // 
            this.textBox_NVIDIA2X_APIBindPort.Location = new System.Drawing.Point(76, 6);
            this.textBox_NVIDIA2X_APIBindPort.Name = "textBox_NVIDIA2X_APIBindPort";
            this.textBox_NVIDIA2X_APIBindPort.Size = new System.Drawing.Size(100, 20);
            this.textBox_NVIDIA2X_APIBindPort.TabIndex = 1;
            // 
            // label_NVIDIA2X_APIBindPort
            // 
            this.label_NVIDIA2X_APIBindPort.AutoSize = true;
            this.label_NVIDIA2X_APIBindPort.Location = new System.Drawing.Point(6, 9);
            this.label_NVIDIA2X_APIBindPort.Name = "label_NVIDIA2X_APIBindPort";
            this.label_NVIDIA2X_APIBindPort.Size = new System.Drawing.Size(67, 13);
            this.label_NVIDIA2X_APIBindPort.TabIndex = 99;
            this.label_NVIDIA2X_APIBindPort.Text = "APIBindPort:";
            // 
            // tabPage_AMD
            // 
            this.tabPage_AMD.Controls.Add(this.label38);
            this.tabPage_AMD.Controls.Add(this.textBox_AMD_MinimumProfit);
            this.tabPage_AMD.Controls.Add(this.label_AMD_MinimumProfit);
            this.tabPage_AMD.Controls.Add(this.tabControl_AMD_Algos);
            this.tabPage_AMD.Controls.Add(this.label_AMD_ExtraLaunchParameters);
            this.tabPage_AMD.Controls.Add(this.textBox_AMD_ExtraLaunchParameters);
            this.tabPage_AMD.Controls.Add(this.textBox_AMD_UsePassword);
            this.tabPage_AMD.Controls.Add(this.label_AMD_UsePassword);
            this.tabPage_AMD.Controls.Add(this.textBox_AMD_APIBindPort);
            this.tabPage_AMD.Controls.Add(this.label_AMD_APIBindPort);
            this.tabPage_AMD.Controls.Add(this.checkBox_AMD_DisableAMDTempControl);
            this.tabPage_AMD.Location = new System.Drawing.Point(4, 22);
            this.tabPage_AMD.Name = "tabPage_AMD";
            this.tabPage_AMD.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_AMD.Size = new System.Drawing.Size(823, 320);
            this.tabPage_AMD.TabIndex = 5;
            this.tabPage_AMD.Text = "AMD_OpenCL";
            this.tabPage_AMD.UseVisualStyleBackColor = true;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 33);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(90, 13);
            this.label38.TabIndex = 99;
            this.label38.Text = "DisabledDevices:";
            // 
            // textBox_AMD_MinimumProfit
            // 
            this.textBox_AMD_MinimumProfit.Location = new System.Drawing.Point(487, 6);
            this.textBox_AMD_MinimumProfit.Name = "textBox_AMD_MinimumProfit";
            this.textBox_AMD_MinimumProfit.Size = new System.Drawing.Size(100, 20);
            this.textBox_AMD_MinimumProfit.TabIndex = 3;
            // 
            // label_AMD_MinimumProfit
            // 
            this.label_AMD_MinimumProfit.AutoSize = true;
            this.label_AMD_MinimumProfit.Location = new System.Drawing.Point(372, 9);
            this.label_AMD_MinimumProfit.Name = "label_AMD_MinimumProfit";
            this.label_AMD_MinimumProfit.Size = new System.Drawing.Size(112, 13);
            this.label_AMD_MinimumProfit.TabIndex = 100;
            this.label_AMD_MinimumProfit.Text = "MinimumProfit ($/day):";
            // 
            // tabControl_AMD_Algos
            // 
            this.tabControl_AMD_Algos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_AMD_Algos.Controls.Add(this.tabPage52);
            this.tabControl_AMD_Algos.Location = new System.Drawing.Point(9, 81);
            this.tabControl_AMD_Algos.Name = "tabControl_AMD_Algos";
            this.tabControl_AMD_Algos.SelectedIndex = 0;
            this.tabControl_AMD_Algos.Size = new System.Drawing.Size(808, 233);
            this.tabControl_AMD_Algos.TabIndex = 6;
            // 
            // tabPage52
            // 
            this.tabPage52.Controls.Add(this.checkBox1);
            this.tabPage52.Controls.Add(this.label39);
            this.tabPage52.Controls.Add(this.textBox57);
            this.tabPage52.Controls.Add(this.label62);
            this.tabPage52.Controls.Add(this.checkBox19);
            this.tabPage52.Controls.Add(this.textBox58);
            this.tabPage52.Controls.Add(this.label63);
            this.tabPage52.Controls.Add(this.textBox59);
            this.tabPage52.Controls.Add(this.label64);
            this.tabPage52.Location = new System.Drawing.Point(4, 22);
            this.tabPage52.Name = "tabPage52";
            this.tabPage52.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage52.Size = new System.Drawing.Size(800, 207);
            this.tabPage52.TabIndex = 0;
            this.tabPage52.Text = "x11";
            this.tabPage52.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(99, 31);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 27;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(3, 32);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(90, 13);
            this.label39.TabIndex = 26;
            this.label39.Text = "DisabledDevices:";
            // 
            // textBox57
            // 
            this.textBox57.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox57.Location = new System.Drawing.Point(6, 71);
            this.textBox57.Name = "textBox57";
            this.textBox57.Size = new System.Drawing.Size(788, 20);
            this.textBox57.TabIndex = 25;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(3, 55);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(123, 13);
            this.label62.TabIndex = 24;
            this.label62.Text = "ExtraLaunchParameters:";
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Location = new System.Drawing.Point(6, 6);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(47, 17);
            this.checkBox19.TabIndex = 23;
            this.checkBox19.Text = "Skip";
            this.checkBox19.UseVisualStyleBackColor = true;
            // 
            // textBox58
            // 
            this.textBox58.Location = new System.Drawing.Point(353, 4);
            this.textBox58.Name = "textBox58";
            this.textBox58.Size = new System.Drawing.Size(100, 20);
            this.textBox58.TabIndex = 22;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(255, 7);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(95, 13);
            this.label63.TabIndex = 21;
            this.label63.Text = "BenchmarkSpeed:";
            // 
            // textBox59
            // 
            this.textBox59.Location = new System.Drawing.Point(137, 4);
            this.textBox59.Name = "textBox59";
            this.textBox59.Size = new System.Drawing.Size(100, 20);
            this.textBox59.TabIndex = 20;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(59, 7);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(75, 13);
            this.label64.TabIndex = 19;
            this.label64.Text = "UsePassword:";
            // 
            // label_AMD_ExtraLaunchParameters
            // 
            this.label_AMD_ExtraLaunchParameters.AutoSize = true;
            this.label_AMD_ExtraLaunchParameters.Location = new System.Drawing.Point(6, 58);
            this.label_AMD_ExtraLaunchParameters.Name = "label_AMD_ExtraLaunchParameters";
            this.label_AMD_ExtraLaunchParameters.Size = new System.Drawing.Size(123, 13);
            this.label_AMD_ExtraLaunchParameters.TabIndex = 99;
            this.label_AMD_ExtraLaunchParameters.Text = "ExtraLaunchParameters:";
            // 
            // textBox_AMD_ExtraLaunchParameters
            // 
            this.textBox_AMD_ExtraLaunchParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_AMD_ExtraLaunchParameters.Location = new System.Drawing.Point(135, 55);
            this.textBox_AMD_ExtraLaunchParameters.Name = "textBox_AMD_ExtraLaunchParameters";
            this.textBox_AMD_ExtraLaunchParameters.Size = new System.Drawing.Size(682, 20);
            this.textBox_AMD_ExtraLaunchParameters.TabIndex = 5;
            // 
            // textBox_AMD_UsePassword
            // 
            this.textBox_AMD_UsePassword.Location = new System.Drawing.Point(263, 6);
            this.textBox_AMD_UsePassword.Name = "textBox_AMD_UsePassword";
            this.textBox_AMD_UsePassword.Size = new System.Drawing.Size(100, 20);
            this.textBox_AMD_UsePassword.TabIndex = 2;
            // 
            // label_AMD_UsePassword
            // 
            this.label_AMD_UsePassword.AutoSize = true;
            this.label_AMD_UsePassword.Location = new System.Drawing.Point(185, 9);
            this.label_AMD_UsePassword.Name = "label_AMD_UsePassword";
            this.label_AMD_UsePassword.Size = new System.Drawing.Size(75, 13);
            this.label_AMD_UsePassword.TabIndex = 99;
            this.label_AMD_UsePassword.Text = "UsePassword:";
            // 
            // textBox_AMD_APIBindPort
            // 
            this.textBox_AMD_APIBindPort.Location = new System.Drawing.Point(76, 6);
            this.textBox_AMD_APIBindPort.Name = "textBox_AMD_APIBindPort";
            this.textBox_AMD_APIBindPort.Size = new System.Drawing.Size(100, 20);
            this.textBox_AMD_APIBindPort.TabIndex = 1;
            // 
            // label_AMD_APIBindPort
            // 
            this.label_AMD_APIBindPort.AutoSize = true;
            this.label_AMD_APIBindPort.Location = new System.Drawing.Point(6, 9);
            this.label_AMD_APIBindPort.Name = "label_AMD_APIBindPort";
            this.label_AMD_APIBindPort.Size = new System.Drawing.Size(67, 13);
            this.label_AMD_APIBindPort.TabIndex = 99;
            this.label_AMD_APIBindPort.Text = "APIBindPort:";
            // 
            // checkBox_AMD_DisableAMDTempControl
            // 
            this.checkBox_AMD_DisableAMDTempControl.AutoSize = true;
            this.checkBox_AMD_DisableAMDTempControl.Location = new System.Drawing.Point(596, 8);
            this.checkBox_AMD_DisableAMDTempControl.Name = "checkBox_AMD_DisableAMDTempControl";
            this.checkBox_AMD_DisableAMDTempControl.Size = new System.Drawing.Size(145, 17);
            this.checkBox_AMD_DisableAMDTempControl.TabIndex = 4;
            this.checkBox_AMD_DisableAMDTempControl.Text = "DisableAMDTempControl";
            this.checkBox_AMD_DisableAMDTempControl.UseVisualStyleBackColor = true;
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(760, 358);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(75, 23);
            this.button_Close.TabIndex = 1;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // comboBox_Language
            // 
            this.comboBox_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Location = new System.Drawing.Point(190, 75);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(147, 21);
            this.comboBox_Language.TabIndex = 65;
            // 
            // label_Language
            // 
            this.label_Language.AutoSize = true;
            this.label_Language.Location = new System.Drawing.Point(186, 55);
            this.label_Language.Name = "label_Language";
            this.label_Language.Size = new System.Drawing.Size(58, 13);
            this.label_Language.TabIndex = 66;
            this.label_Language.Text = "Language:";
            // 
            // label_ethminerAPIPortNvidia
            // 
            this.label_ethminerAPIPortNvidia.AutoSize = true;
            this.label_ethminerAPIPortNvidia.Location = new System.Drawing.Point(348, 197);
            this.label_ethminerAPIPortNvidia.Name = "label_ethminerAPIPortNvidia";
            this.label_ethminerAPIPortNvidia.Size = new System.Drawing.Size(116, 13);
            this.label_ethminerAPIPortNvidia.TabIndex = 67;
            this.label_ethminerAPIPortNvidia.Text = "ethminerAPIPortNvidia:";
            // 
            // label_ethminerAPIPortAMD
            // 
            this.label_ethminerAPIPortAMD.AutoSize = true;
            this.label_ethminerAPIPortAMD.Location = new System.Drawing.Point(348, 244);
            this.label_ethminerAPIPortAMD.Name = "label_ethminerAPIPortAMD";
            this.label_ethminerAPIPortAMD.Size = new System.Drawing.Size(110, 13);
            this.label_ethminerAPIPortAMD.TabIndex = 68;
            this.label_ethminerAPIPortAMD.Text = "ethminerAPIPortAMD:";
            // 
            // label_ethminerDefaultBlockHeight
            // 
            this.label_ethminerDefaultBlockHeight.AutoSize = true;
            this.label_ethminerDefaultBlockHeight.Location = new System.Drawing.Point(186, 244);
            this.label_ethminerDefaultBlockHeight.Name = "label_ethminerDefaultBlockHeight";
            this.label_ethminerDefaultBlockHeight.Size = new System.Drawing.Size(142, 13);
            this.label_ethminerDefaultBlockHeight.TabIndex = 69;
            this.label_ethminerDefaultBlockHeight.Text = "ethminerDefaultBlockHeight:";
            // 
            // textBox_ethminerAPIPortNvidia
            // 
            this.textBox_ethminerAPIPortNvidia.Location = new System.Drawing.Point(351, 217);
            this.textBox_ethminerAPIPortNvidia.Name = "textBox_ethminerAPIPortNvidia";
            this.textBox_ethminerAPIPortNvidia.Size = new System.Drawing.Size(131, 20);
            this.textBox_ethminerAPIPortNvidia.TabIndex = 70;
            // 
            // textBox_ethminerAPIPortAMD
            // 
            this.textBox_ethminerAPIPortAMD.Location = new System.Drawing.Point(351, 264);
            this.textBox_ethminerAPIPortAMD.Name = "textBox_ethminerAPIPortAMD";
            this.textBox_ethminerAPIPortAMD.Size = new System.Drawing.Size(131, 20);
            this.textBox_ethminerAPIPortAMD.TabIndex = 71;
            // 
            // textBox_ethminerDefaultBlockHeight
            // 
            this.textBox_ethminerDefaultBlockHeight.Location = new System.Drawing.Point(190, 264);
            this.textBox_ethminerDefaultBlockHeight.Name = "textBox_ethminerDefaultBlockHeight";
            this.textBox_ethminerDefaultBlockHeight.Size = new System.Drawing.Size(139, 20);
            this.textBox_ethminerDefaultBlockHeight.TabIndex = 72;
            // 
            // Form_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 390);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(836, 371);
            this.Name = "Form_Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Settings_FormClosing);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl_CPU0.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_NVIDIA5X.ResumeLayout(false);
            this.tabPage_NVIDIA5X.PerformLayout();
            this.tabPage_NVIDIA3X.ResumeLayout(false);
            this.tabPage_NVIDIA3X.PerformLayout();
            this.tabPage_NVIDIA2X.ResumeLayout(false);
            this.tabPage_NVIDIA2X.PerformLayout();
            this.tabControl_NVIDIA2X_Algos.ResumeLayout(false);
            this.tabPage38.ResumeLayout(false);
            this.tabPage38.PerformLayout();
            this.tabPage_AMD.ResumeLayout(false);
            this.tabPage_AMD.PerformLayout();
            this.tabControl_AMD_Algos.ResumeLayout(false);
            this.tabPage52.ResumeLayout(false);
            this.tabPage52.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionNVidia3X;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionNVidia5X;
        private System.Windows.Forms.CheckBox checkBox_MinimizeToTray;
        private System.Windows.Forms.CheckBox checkBox_HideMiningWindows;
        private System.Windows.Forms.CheckBox checkBox_AutoStartMining;
        private System.Windows.Forms.CheckBox checkBox_DebugConsole;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionNVidia2X;
        private System.Windows.Forms.Label label_Location;
        private System.Windows.Forms.TextBox textBox_WorkerName;
        private System.Windows.Forms.TextBox textBox_BitcoinAddress;
        private System.Windows.Forms.Label label_WorkerName;
        private System.Windows.Forms.Label label_BitcoinAddress;
        private System.Windows.Forms.CheckBox checkBox_ShowDriverVersionWarning;
        private System.Windows.Forms.CheckBox checkBox_StartMiningWhenIdle;
        private System.Windows.Forms.CheckBox checkBox_AutoScaleBTCValues;
        private System.Windows.Forms.CheckBox checkBox_DisableDetectionAMD;
        private System.Windows.Forms.TabPage tabPage_NVIDIA5X;
        private System.Windows.Forms.TabPage tabPage_NVIDIA3X;
        private System.Windows.Forms.TabPage tabPage_NVIDIA2X;
        private System.Windows.Forms.TabPage tabPage_AMD;
        private System.Windows.Forms.CheckBox checkBox_AMD_DisableAMDTempControl;
        private System.Windows.Forms.ComboBox comboBox_CPU0_ForceCPUExtension;
        private System.Windows.Forms.Label label_CPU0_ForceCPUExtension;
        private System.Windows.Forms.Label label_SwitchMinSecondsFixed;
        private System.Windows.Forms.TextBox textBox_SwitchMinSecondsFixed;
        private System.Windows.Forms.ComboBox comboBox_Location;
        private System.Windows.Forms.TextBox textBox_MinerAPIQueryInterval;
        private System.Windows.Forms.Label label_MinerAPIQueryInterval;
        private System.Windows.Forms.TextBox textBox_SwitchMinSecondsDynamic;
        private System.Windows.Forms.Label label_SwitchMinSecondsDynamic;
        private System.Windows.Forms.TextBox textBox_MinerAPIGraceMinutes;
        private System.Windows.Forms.Label label_MinerAPIGraceMinutes;
        private System.Windows.Forms.TextBox textBox_MinerRestartDelayMS;
        private System.Windows.Forms.Label label_MinerRestartDelayMS;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Precise;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsAMD_Precise;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Standard;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsAMD_Standard;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsAMD_Quick;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsAMD_Quick;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Precise;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsNVIDIA_Precise;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Standard;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsNVIDIA_Standard;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsNVIDIA_Quick;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsNVIDIA_Quick;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Precise;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsCPU_Precise;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Standard;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsCPU_Standard;
        private System.Windows.Forms.Label label_BenchmarkTimeLimitsCPU_Quick;
        private System.Windows.Forms.TextBox textBox_BenchmarkTimeLimitsCPU_Quick;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabControl_CPU0;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.TextBox textBox_CPU0_APIBindPort;
        private System.Windows.Forms.Label label_CPU0_APIBindPort;
        private System.Windows.Forms.TextBox textBox_CPU0_LessThreads;
        private System.Windows.Forms.Label label_CPU0_LessThreads;
        private System.Windows.Forms.TextBox textBox_LogMaxFileSize;
        private System.Windows.Forms.Label label_LogMaxFileSize;
        private System.Windows.Forms.TextBox textBox_LogLevel;
        private System.Windows.Forms.Label label_LogLevel;
        private System.Windows.Forms.TextBox textBox_MinIdleSeconds;
        private System.Windows.Forms.Label label_MinIdleSeconds;
        private System.Windows.Forms.TextBox textBox_CPU0_ExtraLaunchParameters;
        private System.Windows.Forms.Label label_CPU0_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_CPU0_lyra2re_ExtraLaunchParameters;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.CheckBox checkBox_CPU0_lyra2re_Skip;
        private System.Windows.Forms.TextBox textBox_CPU0_lyra2re_BenchmarkSpeed;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox textBox_CPU0_lyra2re_UsePassword;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TabControl tabControl_NVIDIA5X_Algos;
        private System.Windows.Forms.Label label_NVIDIA5X_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_NVIDIA5X_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_NVIDIA5X_UsePassword;
        private System.Windows.Forms.Label label_NVIDIA5X_UsePassword;
        private System.Windows.Forms.TextBox textBox_NVIDIA5X_APIBindPort;
        private System.Windows.Forms.Label label_NVIDIA5X_APIBindPort;
        private System.Windows.Forms.Label label_NVIDIA3X_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_NVIDIA3X_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_NVIDIA3X_UsePassword;
        private System.Windows.Forms.Label label_NVIDIA3X_UsePassword;
        private System.Windows.Forms.TextBox textBox_NVIDIA3X_APIBindPort;
        private System.Windows.Forms.Label label_NVIDIA3X_APIBindPort;
        private System.Windows.Forms.TabControl tabControl_NVIDIA2X_Algos;
        private System.Windows.Forms.TabPage tabPage38;
        private System.Windows.Forms.TextBox textBox54;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.TextBox textBox55;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.TextBox textBox56;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label_NVIDIA2X_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_NVIDIA2X_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_NVIDIA2X_UsePassword;
        private System.Windows.Forms.Label label_NVIDIA2X_UsePassword;
        private System.Windows.Forms.TextBox textBox_NVIDIA2X_APIBindPort;
        private System.Windows.Forms.Label label_NVIDIA2X_APIBindPort;
        private System.Windows.Forms.TabControl tabControl_AMD_Algos;
        private System.Windows.Forms.TabPage tabPage52;
        private System.Windows.Forms.TextBox textBox57;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.TextBox textBox58;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.TextBox textBox59;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label_AMD_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_AMD_ExtraLaunchParameters;
        private System.Windows.Forms.TextBox textBox_AMD_UsePassword;
        private System.Windows.Forms.Label label_AMD_UsePassword;
        private System.Windows.Forms.TextBox textBox_AMD_APIBindPort;
        private System.Windows.Forms.Label label_AMD_APIBindPort;
        private System.Windows.Forms.TabControl tabControl_NVIDIA3X_Algos;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.TextBox textBox_NVIDIA5X_MinimumProfit;
        private System.Windows.Forms.Label label_NVIDIA5X_MinimumProfit;
        private System.Windows.Forms.TextBox textBox_NVIDIA3X_MinimumProfit;
        private System.Windows.Forms.Label label_NVIDIA3X_MinimumProfit;
        private System.Windows.Forms.TextBox textBox_NVIDIA2X_MinimumProfit;
        private System.Windows.Forms.Label label_NVIDIA2X_MinimumProfit;
        private System.Windows.Forms.TextBox textBox_AMD_MinimumProfit;
        private System.Windows.Forms.Label label_AMD_MinimumProfit;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.CheckBox checkBox_UseNewSettingsPage;
        private System.Windows.Forms.CheckBox checkBox_DisableWindowsErrorReporting;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.CheckBox checkBox_NVIDIAP0State;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label_Language;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.Label label_ethminerAPIPortNvidia;
        private System.Windows.Forms.Label label_ethminerAPIPortAMD;
        private System.Windows.Forms.TextBox textBox_ethminerDefaultBlockHeight;
        private System.Windows.Forms.TextBox textBox_ethminerAPIPortAMD;
        private System.Windows.Forms.TextBox textBox_ethminerAPIPortNvidia;
        private System.Windows.Forms.Label label_ethminerDefaultBlockHeight;
    }
}