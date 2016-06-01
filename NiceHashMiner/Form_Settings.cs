using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace NiceHashMiner
{
    public partial class Form_Settings : Form
    {
        private int numCPUs;

        public Form_Settings()
        {
            InitializeComponent();
            numCPUs = CPUID.GetPhysicalProcessorCount();

            SetupGeneralTab();
            SetupCPUTab();
            SetupNVIDIA5XTab();
            SetupNVIDIA3XTab();
            SetupNVIDIA2XTab();
            SetupAMDTab();
        }

        private void SetupGeneralTab()
        {
            // Checkboxes
            checkBox_DebugConsole.Checked = Config.ConfigData.DebugConsole;
            checkBox_AutoStartMining.Checked = Config.ConfigData.AutoStartMining;
            checkBox_HideMiningWindows.Checked = Config.ConfigData.HideMiningWindows;
            checkBox_MinimizeToTray.Checked = Config.ConfigData.MinimizeToTray;
            checkBox_DisableDetectionNVidia5X.Checked = Config.ConfigData.DisableDetectionNVidia5X;
            checkBox_DisableDetectionNVidia3X.Checked = Config.ConfigData.DisableDetectionNVidia3X;
            checkBox_DisableDetectionNVidia2X.Checked = Config.ConfigData.DisableDetectionNVidia2X;
            checkBox_DisableDetectionAMD.Checked = Config.ConfigData.DisableDetectionAMD;
            checkBox_AutoScaleBTCValues.Checked = Config.ConfigData.AutoScaleBTCValues;
            checkBox_StartMiningWhenIdle.Checked = Config.ConfigData.StartMiningWhenIdle;
            checkBox_ShowDriverVersionWarning.Checked = Config.ConfigData.ShowDriverVersionWarning;
            checkBox_DisableWindowsErrorReporting.Checked = Config.ConfigData.DisableWindowsErrorReporting;
            checkBox_UseNewSettingsPage.Checked = Config.ConfigData.UseNewSettingsPage;
            checkBox_NVIDIAP0State.Checked = Config.ConfigData.NVIDIAP0State;

            // Add EventHandler for all the general tab's checkboxes
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
            this.checkBox_UseNewSettingsPage.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);
            this.checkBox_NVIDIAP0State.CheckedChanged += new System.EventHandler(this.GeneralCheckBoxes_CheckedChanged);

            // Textboxes
            textBox_BitcoinAddress.Text = Config.ConfigData.BitcoinAddress;
            textBox_DAGDirectory.Text = Config.ConfigData.DAGDirectory;
            textBox_WorkerName.Text = Config.ConfigData.WorkerName;
            textBox_SwitchMinSecondsFixed.Text = Config.ConfigData.SwitchMinSecondsFixed.ToString();
            textBox_SwitchMinSecondsDynamic.Text = Config.ConfigData.SwitchMinSecondsDynamic.ToString();
            textBox_MinerAPIQueryInterval.Text = Config.ConfigData.MinerAPIQueryInterval.ToString();
            textBox_MinerRestartDelayMS.Text = Config.ConfigData.MinerRestartDelayMS.ToString();
            textBox_MinerAPIGraceMinutes.Text = Config.ConfigData.MinerAPIGraceMinutes.ToString();
            textBox_EthMinerAPIGraceMinutes.Text = Config.ConfigData.EthMinerAPIGraceMinutes.ToString();
            textBox_APIBindPortEthereumFrontEnd.Text = Config.ConfigData.APIBindPortEthereumFrontEnd.ToString();
            textBox_APIBindPortEthereumProxy.Text = Config.ConfigData.APIBindPortEthereumProxy.ToString();
            textBox_MinIdleSeconds.Text = Config.ConfigData.MinIdleSeconds.ToString();
            textBox_LogLevel.Text = Config.ConfigData.LogLevel.ToString();
            textBox_LogMaxFileSize.Text = Config.ConfigData.LogMaxFileSize.ToString();
            textBox_BenchmarkTimeLimitsCPU_Quick.Text = Config.ConfigData.BenchmarkTimeLimitsCPU[0].ToString();
            textBox_BenchmarkTimeLimitsCPU_Standard.Text = Config.ConfigData.BenchmarkTimeLimitsCPU[1].ToString();
            textBox_BenchmarkTimeLimitsCPU_Precise.Text = Config.ConfigData.BenchmarkTimeLimitsCPU[2].ToString();
            textBox_BenchmarkTimeLimitsNVIDIA_Quick.Text = Config.ConfigData.BenchmarkTimeLimitsNVIDIA[0].ToString();
            textBox_BenchmarkTimeLimitsNVIDIA_Standard.Text = Config.ConfigData.BenchmarkTimeLimitsNVIDIA[1].ToString();
            textBox_BenchmarkTimeLimitsNVIDIA_Precise.Text = Config.ConfigData.BenchmarkTimeLimitsNVIDIA[2].ToString();
            textBox_BenchmarkTimeLimitsAMD_Quick.Text = Config.ConfigData.BenchmarkTimeLimitsAMD[0].ToString();
            textBox_BenchmarkTimeLimitsAMD_Standard.Text = Config.ConfigData.BenchmarkTimeLimitsAMD[1].ToString();
            textBox_BenchmarkTimeLimitsAMD_Precise.Text = Config.ConfigData.BenchmarkTimeLimitsAMD[2].ToString();

            // Add EventHandler for all the general tab's textboxes
            this.textBox_BitcoinAddress.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_DAGDirectory.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_WorkerName.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_SwitchMinSecondsFixed.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_SwitchMinSecondsDynamic.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_MinerAPIQueryInterval.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_MinerRestartDelayMS.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_MinerAPIGraceMinutes.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_EthMinerAPIGraceMinutes.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_APIBindPortEthereumFrontEnd.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_APIBindPortEthereumProxy.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_MinIdleSeconds.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_LogLevel.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_LogMaxFileSize.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsCPU_Quick.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsCPU_Standard.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsCPU_Precise.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Quick.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Standard.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsNVIDIA_Precise.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsAMD_Quick.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsAMD_Standard.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);
            this.textBox_BenchmarkTimeLimitsAMD_Precise.Leave += new System.EventHandler(this.GeneralTextBoxes_Leave);

            // ComboBox
            comboBox_Location.SelectedIndex = Config.ConfigData.Location;

            // Add EventHandler for all the general tab's textboxes
            this.comboBox_Location.Leave += new System.EventHandler(this.GeneralComboBoxes_Leave);
        }

        // Currently it only supports for CPU0
        private void SetupCPUTab()
        {
            comboBox_CPU0_ForceCPUExtension.SelectedIndex = Config.ConfigData.ForceCPUExtension;
            comboBox_CPU0_ForceCPUExtension.SelectedIndexChanged += Main_ForceCPUExtension_SelectedIndexChanged;
            textBox_CPU0_LessThreads.Text = Config.ConfigData.LessThreads.ToString();
            textBox_CPU0_LessThreads.Leave += Main_LessThreads_Leave;
            textBox_CPU0_APIBindPort.Text = Config.ConfigData.Groups[0].APIBindPort.ToString();
            textBox_CPU0_APIBindPort.Tag = (int)0;
            textBox_CPU0_APIBindPort.Leave += Main_APIBindPort_Leave;
            textBox_CPU0_ExtraLaunchParameters.Text = Config.ConfigData.Groups[0].ExtraLaunchParameters;
            textBox_CPU0_ExtraLaunchParameters.Tag = (int)0;
            textBox_CPU0_ExtraLaunchParameters.Leave += Main_ExtraLaunchParameters_Leave;

            // Setup Algos
            int tabIndex = 6;
            ShowAlgoSettings(ref tabControl_CPU0, ref tabIndex, 0, "CPU0", 1);
            tabControl_CPU0.Controls.Remove(tabPage7);
            tabControl_CPU0.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
        }

        private void SetupNVIDIA5XTab()
        {
            int minerIndex = numCPUs;
            string minerName = Config.ConfigData.Groups[minerIndex].Name;
            int numDevices = Form1.Miners[minerIndex].CDevs.Count;
            int tabIndex = 3;

            // Main disabled devices
            DisabledDevice(true, ref tabPage_NVIDIA5X, ref tabIndex, minerIndex, -1, numDevices, 102, 32, minerName);
            textBox_NVIDIA5X_ExtraLaunchParameters.TabIndex = tabIndex++;
            // Add TabPages for all the algos
            tabControl_NVIDIA5X_Algos.TabIndex = tabIndex++;
            ShowAlgoSettings(ref tabControl_NVIDIA5X_Algos, ref tabIndex, minerIndex, minerName, numDevices);

            tabControl_NVIDIA5X_Algos.ResumeLayout(false);
            tabPage_NVIDIA5X.ResumeLayout(false);
            tabPage_NVIDIA5X.PerformLayout();

            textBox_NVIDIA5X_APIBindPort.Text = Config.ConfigData.Groups[minerIndex].APIBindPort.ToString();
            textBox_NVIDIA5X_APIBindPort.Tag = minerIndex;
            textBox_NVIDIA5X_APIBindPort.Leave += Main_APIBindPort_Leave;
            textBox_NVIDIA5X_UsePassword.Text = Config.ConfigData.Groups[minerIndex].UsePassword;
            textBox_NVIDIA5X_UsePassword.Tag = minerIndex;
            textBox_NVIDIA5X_UsePassword.Leave += Main_UsePassword_Leave;
            textBox_NVIDIA5X_MinimumProfit.Text = Config.ConfigData.Groups[minerIndex].MinimumProfit.ToString();
            textBox_NVIDIA5X_MinimumProfit.Tag = minerIndex;
            textBox_NVIDIA5X_MinimumProfit.Leave += Main_MinimumProfit_Leave;
            textBox_NVIDIA5X_ExtraLaunchParameters.Text = Config.ConfigData.Groups[minerIndex].ExtraLaunchParameters;
            textBox_NVIDIA5X_ExtraLaunchParameters.Tag = minerIndex;
            textBox_NVIDIA5X_ExtraLaunchParameters.Leave += Main_ExtraLaunchParameters_Leave;
        }

        private void SetupNVIDIA3XTab()
        {
            int minerIndex = numCPUs + 1;
            string minerName = Config.ConfigData.Groups[minerIndex].Name;
            int numDevices = Form1.Miners[minerIndex].CDevs.Count;
            int tabIndex = 3;

            // Main disabled devices
            DisabledDevice(true, ref tabPage_NVIDIA3X, ref tabIndex, minerIndex, -1, numDevices, 102, 32, minerName);
            textBox_NVIDIA3X_ExtraLaunchParameters.TabIndex = tabIndex++;
            // Add TabPages for all the algos
            tabControl_NVIDIA3X_Algos.TabIndex = tabIndex++;
            ShowAlgoSettings(ref tabControl_NVIDIA3X_Algos, ref tabIndex, minerIndex, minerName, numDevices);

            tabControl_NVIDIA3X_Algos.ResumeLayout(false);
            tabPage_NVIDIA3X.ResumeLayout(false);
            tabPage_NVIDIA3X.PerformLayout();

            textBox_NVIDIA3X_APIBindPort.Text = Config.ConfigData.Groups[minerIndex].APIBindPort.ToString();
            textBox_NVIDIA3X_APIBindPort.Tag = minerIndex;
            textBox_NVIDIA3X_APIBindPort.Leave += Main_APIBindPort_Leave;
            textBox_NVIDIA3X_UsePassword.Text = Config.ConfigData.Groups[minerIndex].UsePassword;
            textBox_NVIDIA3X_UsePassword.Tag = minerIndex;
            textBox_NVIDIA3X_UsePassword.Leave += Main_UsePassword_Leave;
            textBox_NVIDIA3X_MinimumProfit.Text = Config.ConfigData.Groups[minerIndex].MinimumProfit.ToString();
            textBox_NVIDIA3X_MinimumProfit.Tag = minerIndex;
            textBox_NVIDIA3X_MinimumProfit.Leave += Main_MinimumProfit_Leave;
            textBox_NVIDIA3X_ExtraLaunchParameters.Text = Config.ConfigData.Groups[minerIndex].ExtraLaunchParameters;
            textBox_NVIDIA3X_ExtraLaunchParameters.Tag = minerIndex;
            textBox_NVIDIA3X_ExtraLaunchParameters.Leave += Main_ExtraLaunchParameters_Leave;
        }

        private void SetupNVIDIA2XTab()
        {
            int minerIndex = numCPUs + 2;
            string minerName = Config.ConfigData.Groups[minerIndex].Name;
            int numDevices = Form1.Miners[minerIndex].CDevs.Count;
            int tabIndex = 3;

            // Main disabled devices
            DisabledDevice(true, ref tabPage_NVIDIA2X, ref tabIndex, minerIndex, -1, numDevices, 102, 32, minerName);
            textBox_NVIDIA2X_ExtraLaunchParameters.TabIndex = tabIndex;
            // Add TabPages for all the algos
            tabControl_NVIDIA2X_Algos.TabIndex = tabIndex++;
            ShowAlgoSettings(ref tabControl_NVIDIA2X_Algos, ref tabIndex, minerIndex, minerName, numDevices);
            
            tabControl_NVIDIA2X_Algos.Controls.Remove(tabPage38);
            tabControl_NVIDIA2X_Algos.ResumeLayout(false);
            tabPage_NVIDIA2X.ResumeLayout(false);
            tabPage_NVIDIA2X.PerformLayout();

            textBox_NVIDIA2X_APIBindPort.Text = Config.ConfigData.Groups[minerIndex].APIBindPort.ToString();
            textBox_NVIDIA2X_APIBindPort.Tag = minerIndex;
            textBox_NVIDIA2X_APIBindPort.Leave += Main_APIBindPort_Leave;
            textBox_NVIDIA2X_UsePassword.Text = Config.ConfigData.Groups[minerIndex].UsePassword;
            textBox_NVIDIA2X_UsePassword.Tag = minerIndex;
            textBox_NVIDIA2X_UsePassword.Leave += Main_UsePassword_Leave;
            textBox_NVIDIA2X_MinimumProfit.Text = Config.ConfigData.Groups[minerIndex].MinimumProfit.ToString();
            textBox_NVIDIA2X_MinimumProfit.Tag = minerIndex;
            textBox_NVIDIA2X_MinimumProfit.Leave += Main_MinimumProfit_Leave;
            textBox_NVIDIA2X_ExtraLaunchParameters.Text = Config.ConfigData.Groups[minerIndex].ExtraLaunchParameters;
            textBox_NVIDIA2X_ExtraLaunchParameters.Tag = minerIndex;
            textBox_NVIDIA2X_ExtraLaunchParameters.Leave += Main_ExtraLaunchParameters_Leave;
        }

        private void SetupAMDTab()
        {
            int minerIndex = numCPUs + 3;
            string minerName = Config.ConfigData.Groups[minerIndex].Name;
            int numDevices = Form1.Miners[minerIndex].CDevs.Count;
            int tabIndex = 4;

            // Main disabled devices
            DisabledDevice(true, ref tabPage_AMD, ref tabIndex, minerIndex, -1, numDevices, 102, 32, minerName);
            textBox_AMD_ExtraLaunchParameters.TabIndex = tabIndex++;
            // Add TabPages for all the algos
            tabControl_AMD_Algos.TabIndex = tabIndex++;
            ShowAlgoSettings(ref tabControl_AMD_Algos, ref tabIndex, minerIndex, minerName, numDevices);

            tabControl_AMD_Algos.Controls.Remove(tabPage52);
            tabControl_AMD_Algos.ResumeLayout(false);
            tabPage_AMD.ResumeLayout(false);
            tabPage_AMD.PerformLayout();

            checkBox_AMD_DisableAMDTempControl.Checked = Config.ConfigData.DisableAMDTempControl;
            checkBox_AMD_DisableAMDTempControl.CheckedChanged += checkBox_AMD_DisableAMDTempControl_CheckedChanged;

            textBox_AMD_APIBindPort.Text = Config.ConfigData.Groups[minerIndex].APIBindPort.ToString();
            textBox_AMD_APIBindPort.Tag = minerIndex;
            textBox_AMD_APIBindPort.Leave += Main_APIBindPort_Leave;
            textBox_AMD_UsePassword.Text = Config.ConfigData.Groups[minerIndex].UsePassword;
            textBox_AMD_UsePassword.Tag = minerIndex;
            textBox_AMD_UsePassword.Leave += Main_UsePassword_Leave;
            textBox_AMD_MinimumProfit.Text = Config.ConfigData.Groups[minerIndex].MinimumProfit.ToString();
            textBox_AMD_MinimumProfit.Tag = minerIndex;
            textBox_AMD_MinimumProfit.Leave += Main_MinimumProfit_Leave;
            textBox_AMD_ExtraLaunchParameters.Text = Config.ConfigData.Groups[minerIndex].ExtraLaunchParameters;
            textBox_AMD_ExtraLaunchParameters.Tag = minerIndex;
            textBox_AMD_ExtraLaunchParameters.Leave += Main_ExtraLaunchParameters_Leave;
        }

        private void ShowAlgoSettings(ref TabControl tabCtl, ref int tabIndex, int minerIndex, string minerName, int numDevices)
        {
            int numAlgos = Config.ConfigData.Groups[minerIndex].Algorithms.Length;
            TabPage[] algoTabPage = new TabPage[numAlgos];
            CheckBox[] Skip = new CheckBox[numAlgos];
            Label[] labelUsePassword = new Label[numAlgos];
            Label[] labelBenchmarkSpeed = new Label[numAlgos];
            Label[] labelBenchmarkSpeedUnit = new Label[numAlgos];
            Label[] labelDisabledDevices = new Label[numAlgos];
            Label[] labelExtraLaunchParameters = new Label[numAlgos];
            TextBox[] textboxUsePassword = new TextBox[numAlgos];
            TextBox[] textboxBenchmarkSpeed = new TextBox[numAlgos];
            TextBox[] textboxExtraLaunchParameters = new TextBox[numAlgos];

            for (int i = 0; i < numAlgos; i++)
            {
                int[] tag = { minerIndex, i };
                string algoName = Config.ConfigData.Groups[minerIndex].Algorithms[i].Name;

                // TabPages
                algoTabPage[i] = new TabPage();
                algoTabPage[i].SuspendLayout();

                // Skip CheckBoxes
                Skip[i] = new CheckBox();
                Skip[i].AutoSize = true;
                Skip[i].Location = new System.Drawing.Point(6, 9);
                Skip[i].Name = "checkBox_" + minerName + "_" + algoName + "_Skip";
                Skip[i].Size = new System.Drawing.Size(47, 17);
                Skip[i].TabIndex = tabIndex++;
                Skip[i].Text = "Skip";
                Skip[i].UseVisualStyleBackColor = true;
                Skip[i].Checked = Config.ConfigData.Groups[minerIndex].Algorithms[i].Skip;
                Skip[i].Tag = tag;
                Skip[i].CheckedChanged += Algo_Skip_CheckedChanged;

                // UsePassword
                labelUsePassword[i] = new Label();
                labelUsePassword[i].AutoSize = true;
                labelUsePassword[i].Location = new System.Drawing.Point(60, 10);
                labelUsePassword[i].Name = "label_" + minerName + "_" + algoName + "_UsePassword";
                labelUsePassword[i].Size = new System.Drawing.Size(75, 13);
                labelUsePassword[i].TabIndex = 99;
                labelUsePassword[i].Text = "UsePassword:";

                textboxUsePassword[i] = new TextBox();
                textboxUsePassword[i].Location = new System.Drawing.Point(137, 7);
                textboxUsePassword[i].Name = "textBox_" + minerName + "_" + algoName + "_UsePassword";
                textboxUsePassword[i].Size = new System.Drawing.Size(100, 20);
                textboxUsePassword[i].TabIndex = tabIndex++;
                textboxUsePassword[i].Text = Config.ConfigData.Groups[minerIndex].Algorithms[i].UsePassword;
                textboxUsePassword[i].Tag = tag;
                textboxUsePassword[i].Leave += Algo_UsePassword_Leave;

                // Benchmark Speed
                labelBenchmarkSpeed[i] = new Label();
                labelBenchmarkSpeed[i].AutoSize = true;
                labelBenchmarkSpeed[i].Location = new System.Drawing.Point(255, 10);
                labelBenchmarkSpeed[i].Name = "label_" + minerName + "_" + algoName + "_BenchmarkSpeed";
                labelBenchmarkSpeed[i].Size = new System.Drawing.Size(95, 13);
                labelBenchmarkSpeed[i].TabIndex = 99;
                labelBenchmarkSpeed[i].Text = "BenchmarkSpeed:";

                labelBenchmarkSpeedUnit[i] = new Label();
                labelBenchmarkSpeedUnit[i].AutoSize = true;
                labelBenchmarkSpeedUnit[i].Location = new System.Drawing.Point(456, 10);
                labelBenchmarkSpeedUnit[i].Name = "label_" + minerName + "_" + algoName + "_BenchmarkSpeedUnit";
                labelBenchmarkSpeedUnit[i].Size = new System.Drawing.Size(95, 13);
                labelBenchmarkSpeedUnit[i].TabIndex = 99;
                labelBenchmarkSpeedUnit[i].Text = "H/s";

                textboxBenchmarkSpeed[i] = new TextBox();
                textboxBenchmarkSpeed[i].Location = new System.Drawing.Point(353, 7);
                textboxBenchmarkSpeed[i].Name = "textBox_" + minerName + "_" + algoName + "_BenchmarkSpeed";
                textboxBenchmarkSpeed[i].Size = new System.Drawing.Size(100, 20);
                textboxBenchmarkSpeed[i].TabIndex = tabIndex++;
                textboxBenchmarkSpeed[i].Text = Config.ConfigData.Groups[minerIndex].Algorithms[i].BenchmarkSpeed.ToString();
                textboxBenchmarkSpeed[i].Tag = tag;
                textboxBenchmarkSpeed[i].Leave += Algo_BenchmarkSpeed_Leave;

                // Disabled Devices per Algo
                labelDisabledDevices[i] = new Label();
                labelDisabledDevices[i].AutoSize = true;
                labelDisabledDevices[i].Location = new System.Drawing.Point(3, 32);
                labelDisabledDevices[i].Name = "label39";
                labelDisabledDevices[i].Size = new System.Drawing.Size(90, 13);
                labelDisabledDevices[i].TabIndex = tabIndex++;
                labelDisabledDevices[i].Text = "DisabledDevices:";
                
                DisabledDevice(false, ref algoTabPage[i], ref tabIndex, minerIndex, i, numDevices, 99, 31, minerName);

                // Extra Launch Parameters
                labelExtraLaunchParameters[i] = new Label();
                labelExtraLaunchParameters[i].AutoSize = true;
                labelExtraLaunchParameters[i].Location = new System.Drawing.Point(3, 55);
                labelExtraLaunchParameters[i].Name = "label_" + minerName + "_" + algoName + "_ExtraLaunchParameters";
                labelExtraLaunchParameters[i].Size = new System.Drawing.Size(123, 13);
                labelExtraLaunchParameters[i].TabIndex = 99;
                labelExtraLaunchParameters[i].Text = "ExtraLaunchParameters:";

                textboxExtraLaunchParameters[i] = new TextBox();
                textboxExtraLaunchParameters[i].Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
                textboxExtraLaunchParameters[i].Location = new System.Drawing.Point(6, 71);
                textboxExtraLaunchParameters[i].Name = "textBox_" + minerName + "_" + algoName + "_ExtraLaunchParameters";
                textboxExtraLaunchParameters[i].Size = new System.Drawing.Size(764, 20);
                textboxExtraLaunchParameters[i].TabIndex = tabIndex++;
                textboxExtraLaunchParameters[i].Text = Config.ConfigData.Groups[minerIndex].Algorithms[i].ExtraLaunchParameters;
                textboxExtraLaunchParameters[i].Tag = tag;
                textboxExtraLaunchParameters[i].Leave += Algo_ExtraLaunchParameters_Leave;

                // Add TabPages
                algoTabPage[i].Controls.Add(Skip[i]);
                algoTabPage[i].Controls.Add(labelUsePassword[i]);
                algoTabPage[i].Controls.Add(textboxUsePassword[i]);
                algoTabPage[i].Controls.Add(labelBenchmarkSpeed[i]);
                algoTabPage[i].Controls.Add(textboxBenchmarkSpeed[i]);
                algoTabPage[i].Controls.Add(labelDisabledDevices[i]);
                algoTabPage[i].Controls.Add(labelExtraLaunchParameters[i]);
                algoTabPage[i].Controls.Add(textboxExtraLaunchParameters[i]);
                algoTabPage[i].Location = new System.Drawing.Point(4, 22);
                algoTabPage[i].Name = "tabPage_" + minerName + "_" + algoName;
                algoTabPage[i].Padding = new System.Windows.Forms.Padding(3);
                algoTabPage[i].Size = new System.Drawing.Size(776, 203);
                algoTabPage[i].TabIndex = 0;
                algoTabPage[i].Text = algoName;
                algoTabPage[i].UseVisualStyleBackColor = true;

                // Add algoTabPages to mainTabPages
                tabCtl.Controls.Add(algoTabPage[i]);

                algoTabPage[i].ResumeLayout(false);
                algoTabPage[i].PerformLayout();
            }
        }

        private void GeneralCheckBoxes_CheckedChanged(object sender, EventArgs e)
        {
            Config.ConfigData.DebugConsole = checkBox_DebugConsole.Checked;
            Config.ConfigData.AutoStartMining = checkBox_AutoStartMining.Checked;
            Config.ConfigData.HideMiningWindows = checkBox_HideMiningWindows.Checked;
            Config.ConfigData.MinimizeToTray = checkBox_MinimizeToTray.Checked;
            Config.ConfigData.DisableDetectionNVidia5X = checkBox_DisableDetectionNVidia5X.Checked;
            Config.ConfigData.DisableDetectionNVidia3X = checkBox_DisableDetectionNVidia3X.Checked;
            Config.ConfigData.DisableDetectionNVidia2X = checkBox_DisableDetectionNVidia2X.Checked;
            Config.ConfigData.DisableDetectionAMD = checkBox_DisableDetectionAMD.Checked;
            Config.ConfigData.AutoScaleBTCValues = checkBox_AutoScaleBTCValues.Checked;
            Config.ConfigData.StartMiningWhenIdle = checkBox_StartMiningWhenIdle.Checked;
            Config.ConfigData.ShowDriverVersionWarning = checkBox_ShowDriverVersionWarning.Checked;
            Config.ConfigData.DisableWindowsErrorReporting = checkBox_DisableWindowsErrorReporting.Checked;
            Config.ConfigData.UseNewSettingsPage = checkBox_UseNewSettingsPage.Checked;
            Config.ConfigData.NVIDIAP0State = checkBox_NVIDIAP0State.Checked;
        }

        private void GeneralTextBoxes_Leave(object sender, EventArgs e)
        {
            Config.ConfigData.BitcoinAddress = textBox_BitcoinAddress.Text.Trim();
            Config.ConfigData.DAGDirectory = textBox_DAGDirectory.Text.Trim();
            Config.ConfigData.WorkerName = textBox_WorkerName.Text.Trim();
            if (!ParseStringToInt32(ref textBox_SwitchMinSecondsFixed, ref Config.ConfigData.SwitchMinSecondsFixed)) return;
            if (!ParseStringToInt32(ref textBox_SwitchMinSecondsDynamic, ref Config.ConfigData.SwitchMinSecondsDynamic)) return;
            if (!ParseStringToInt32(ref textBox_MinerAPIQueryInterval, ref Config.ConfigData.MinerAPIQueryInterval)) return;
            if (!ParseStringToInt32(ref textBox_MinerRestartDelayMS, ref Config.ConfigData.MinerRestartDelayMS)) return;
            if (!ParseStringToInt32(ref textBox_MinerAPIGraceMinutes, ref Config.ConfigData.MinerAPIGraceMinutes)) return;
            if (!ParseStringToInt32(ref textBox_EthMinerAPIGraceMinutes, ref Config.ConfigData.EthMinerAPIGraceMinutes)) return;
            if (!ParseStringToInt32(ref textBox_APIBindPortEthereumFrontEnd, ref Config.ConfigData.APIBindPortEthereumFrontEnd)) return;
            if (!ParseStringToInt32(ref textBox_APIBindPortEthereumProxy, ref Config.ConfigData.APIBindPortEthereumProxy)) return;
            if (!ParseStringToInt32(ref textBox_MinIdleSeconds, ref Config.ConfigData.MinIdleSeconds)) return;
            if (!ParseStringToInt32(ref textBox_LogLevel, ref Config.ConfigData.LogLevel)) return;
            if (!ParseStringToInt64(ref textBox_LogMaxFileSize, ref Config.ConfigData.LogMaxFileSize)) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsCPU_Quick, ref Config.ConfigData.BenchmarkTimeLimitsCPU[0])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsCPU_Standard, ref Config.ConfigData.BenchmarkTimeLimitsCPU[1])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsCPU_Precise, ref Config.ConfigData.BenchmarkTimeLimitsCPU[2])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsNVIDIA_Quick, ref Config.ConfigData.BenchmarkTimeLimitsNVIDIA[0])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsNVIDIA_Standard, ref Config.ConfigData.BenchmarkTimeLimitsNVIDIA[1])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsNVIDIA_Precise, ref Config.ConfigData.BenchmarkTimeLimitsNVIDIA[2])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsAMD_Quick, ref Config.ConfigData.BenchmarkTimeLimitsAMD[0])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsAMD_Standard, ref Config.ConfigData.BenchmarkTimeLimitsAMD[1])) return;
            if (!ParseStringToInt32(ref textBox_BenchmarkTimeLimitsAMD_Precise, ref Config.ConfigData.BenchmarkTimeLimitsAMD[2])) return;
        }

        private void GeneralComboBoxes_Leave(object sender, EventArgs e)
        {
            Config.ConfigData.Location = comboBox_Location.SelectedIndex;
        }

        private void checkBox_AMD_DisableAMDTempControl_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;

            Config.ConfigData.DisableAMDTempControl = chkbox.Checked;
        }

        private void Main_LessThreads_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;

            int val;
            if (Int32.TryParse(txtbox.Text, out val))
                Config.ConfigData.LessThreads = val;
            else
            {
                MessageBox.Show("ERROR! Please input a valid number for LessThreads.", "Invalid number for LessThreads!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbox.Text = Config.ConfigData.LessThreads.ToString();
                txtbox.Focus();
            }
        }

        private void Main_ForceCPUExtension_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = (ComboBox)sender;

            Config.ConfigData.ForceCPUExtension = cmbbox.SelectedIndex;
        }

        private void Main_ExtraLaunchParameters_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int index = (int)txtbox.Tag;

            Config.ConfigData.Groups[index].ExtraLaunchParameters = txtbox.Text.Trim();
        }

        private void Main_MinimumProfit_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int index = (int)txtbox.Tag;

            double val;
            if (Double.TryParse(txtbox.Text, out val))
            {
                if (val >= 0)
                    Config.ConfigData.Groups[index].MinimumProfit = val;
                else
                {
                    MessageBox.Show("ERROR! Please input a minimum profit that is more than 0.", "Invalid minimum profit!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtbox.Text = Config.ConfigData.Groups[index].MinimumProfit.ToString();
                    txtbox.Focus();
                }
            }
            else
            {
                MessageBox.Show("ERROR! Please input a valid minimum profit.", "Invalid minimum profit!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbox.Text = Config.ConfigData.Groups[index].MinimumProfit.ToString();
                txtbox.Focus();
            }
        }

        private void Main_UsePassword_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int index = (int)txtbox.Tag;

            Config.ConfigData.Groups[index].UsePassword = txtbox.Text.Trim();
        }

        private void Main_APIBindPort_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int index = (int)txtbox.Tag;

            int val;
            if (Int32.TryParse(txtbox.Text, out val))
            {
                if (val < 65535)
                    Config.ConfigData.Groups[index].APIBindPort = val;
                else
                {
                    MessageBox.Show("ERROR! Please input a valid API port number.", "Invalid API port number!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtbox.Text = Config.ConfigData.Groups[index].APIBindPort.ToString();
                    txtbox.Focus();
                }
            }
            else
            {
                MessageBox.Show("ERROR! Please input a valid API port number.", "Invalid API port number!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbox.Text = Config.ConfigData.Groups[index].APIBindPort.ToString();
                txtbox.Focus();
            }
        }

        private void Algo_ExtraLaunchParameters_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int[] tag = (int[])txtbox.Tag;

            Config.ConfigData.Groups[tag[0]].Algorithms[tag[1]].ExtraLaunchParameters = txtbox.Text.Trim();
        }

        private void Algo_BenchmarkSpeed_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int[] tag = (int[])txtbox.Tag;

            double val;
            if (Double.TryParse(txtbox.Text, out val))
            {
                if (val >= 0)
                    Config.ConfigData.Groups[tag[0]].Algorithms[tag[1]].BenchmarkSpeed = val;
                else
                {
                    MessageBox.Show("ERROR! Please input a benchmark speed that is more or equals to 0H/s.", "Invalid benchmark speed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtbox.Text = Config.ConfigData.Groups[tag[0]].Algorithms[tag[1]].BenchmarkSpeed.ToString();
                    txtbox.Focus();
                }
            }
            else
            {
                MessageBox.Show("ERROR! Please input a valid benchmark speed.", "Invalid benchmark speed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtbox.Text = Config.ConfigData.Groups[tag[0]].Algorithms[tag[1]].BenchmarkSpeed.ToString();
                txtbox.Focus();
            }
        }

        private void Algo_UsePassword_Leave(object sender, EventArgs e)
        {
            TextBox txtbox = (TextBox)sender;
            int[] tag = (int[])txtbox.Tag;

            Config.ConfigData.Groups[tag[0]].Algorithms[tag[1]].UsePassword = txtbox.Text.Trim();
        }

        private void Algo_Skip_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            int[] tag = (int[])chkbox.Tag;

            Config.ConfigData.Groups[tag[0]].Algorithms[tag[1]].Skip = chkbox.Checked;
        }

        private void DisabledDevice(bool mainDD, ref TabPage tb, ref int tabIndex, int minerIndex, int algoIndex, int numDevices, int x, int y, string minerName)
        {
            if (numDevices > 0)
            {
                List<int> lst = new List<int>();
                if (mainDD)
                    for (int i = 0; i < Config.ConfigData.Groups[minerIndex].DisabledDevices.Length; i++)
                        lst.Add(Config.ConfigData.Groups[minerIndex].DisabledDevices[i]);

                CheckBox[] DisabledDevice = new CheckBox[numDevices];
                for (int i = 0; i < numDevices; i++)
                {
                    DisabledDevice[i] = new CheckBox();
                    DisabledDevice[i].AutoSize = true;
                    DisabledDevice[i].Location = new System.Drawing.Point(x, y);
                    DisabledDevice[i].Name = "checkBox_" + minerName + "_GPU" + i;
                    DisabledDevice[i].Size = new System.Drawing.Size(55, 17);
                    DisabledDevice[i].TabIndex = tabIndex++;
                    DisabledDevice[i].Text = "GPU" + i;
                    DisabledDevice[i].UseVisualStyleBackColor = true;
                    if (mainDD)
                    {
                        int[] tag = { minerIndex, i };
                        DisabledDevice[i].Tag = tag;
                        DisabledDevice[i].Checked = lst.Contains(i);
                        DisabledDevice[i].CheckedChanged += MainDisabledDevice_CheckedChanged;
                    }
                    else
                    {
                        int[] tag = { minerIndex, algoIndex, i };
                        DisabledDevice[i].Tag = tag;
                        DisabledDevice[i].Checked = Config.ConfigData.Groups[minerIndex].Algorithms[algoIndex].DisabledDevices[i];
                        DisabledDevice[i].CheckedChanged += AlgoDisabledDevice_CheckedChanged;
                    }

                    tb.Controls.Add(DisabledDevice[i]);
                    x += 61;
                }

                textBox_NVIDIA2X_ExtraLaunchParameters.TabIndex = tabIndex++;
                tabControl_NVIDIA2X_Algos.TabIndex = tabIndex++;
            }
            else
            {
                Label labelNoDevice = new Label();
                labelNoDevice.AutoSize = true;
                labelNoDevice.Location = new System.Drawing.Point(x, y + 1);
                labelNoDevice.Name = "labelNoDevice";
                labelNoDevice.Size = new System.Drawing.Size(92, 13);
                labelNoDevice.TabIndex = 99;
                labelNoDevice.Text = "No device found..";

                tb.Controls.Add(labelNoDevice);
            }
        }

        private void AlgoDisabledDevice_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            int[] tag = (int[])chkbox.Tag;
            int minerIndex = tag[0];
            int algoIndex = tag[1];
            int device = tag[2];

            Config.ConfigData.Groups[minerIndex].Algorithms[algoIndex].DisabledDevices[device] = chkbox.Checked;
        }

        private void MainDisabledDevice_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            int[] tag = (int[])chkbox.Tag;
            int minerIndex = tag[0];
            int device = tag[1];

            List<int> lst = new List<int>();
            for (int i = 0; i < Config.ConfigData.Groups[minerIndex].DisabledDevices.Length; i++)
                lst.Add(Config.ConfigData.Groups[minerIndex].DisabledDevices[i]);

            if (chkbox.Checked == true && lst.Contains(device) == false) lst.Add(device);
            if (chkbox.Checked == false && lst.Contains(device) == true) lst.Remove(device);

            Config.ConfigData.Groups[minerIndex].DisabledDevices = lst.ToArray();
        }

        private bool ParseStringToInt32(ref TextBox textBox, ref int configInt)
        {
            if (!Int32.TryParse(textBox.Text, out configInt))
            {
                MessageBox.Show("This input box only accepts numbers (0-9).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Focus();
                return false;
            }

            return true;
        }

        private bool ParseStringToInt64(ref TextBox textBox, ref long configInt)
        {
            if (!Int64.TryParse(textBox.Text, out configInt))
            {
                MessageBox.Show("This input box only accepts numbers (0-9).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.Focus();
                return false;
            }

            return true;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Commit();
        }
    }
}
