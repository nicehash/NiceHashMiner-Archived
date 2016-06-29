namespace NiceHashMiner
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonStartMining = new System.Windows.Forms.Button();
            this.textBoxBTCAddress = new System.Windows.Forms.TextBox();
            this.labelServiceLocation = new System.Windows.Forms.Label();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.labelBitcoinAddress = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelGlobalRateText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelGlobalRateValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBTCDayText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBTCDayValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBalanceText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBalanceBTCValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBalanceBTCCode = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBalanceDollarText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelBalanceDollarValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.linkLabelCheckStats = new System.Windows.Forms.LinkLabel();
            this.labelWorkerName = new System.Windows.Forms.Label();
            this.textBoxWorkerName = new System.Windows.Forms.TextBox();
            this.linkLabelVisitUs = new System.Windows.Forms.LinkLabel();
            this.listViewDevices = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonStopMining = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCPU_Mining_Speed = new System.Windows.Forms.Label();
            this.labelNVIDIA5X_Mining_Speed = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelNVIDIA3X_Mining_Speed = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label_RateCPU = new System.Windows.Forms.Label();
            this.label_RateCPUBTC = new System.Windows.Forms.Label();
            this.label_RateNVIDIA5XBTC = new System.Windows.Forms.Label();
            this.label_RateNVIDIA5X = new System.Windows.Forms.Label();
            this.label_RateNVIDIA3XBTC = new System.Windows.Forms.Label();
            this.label_RateNVIDIA3X = new System.Windows.Forms.Label();
            this.buttonBenchmark = new System.Windows.Forms.Button();
            this.label_RateCPUDollar = new System.Windows.Forms.Label();
            this.label_RateNVIDIA5XDollar = new System.Windows.Forms.Label();
            this.label_RateNVIDIA3XDollar = new System.Windows.Forms.Label();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.label_AMDOpenCL_Mining_Text = new System.Windows.Forms.Label();
            this.labelAMDOpenCL_Mining_Speed = new System.Windows.Forms.Label();
            this.label_RateAMD = new System.Windows.Forms.Label();
            this.label_RateAMDBTC = new System.Windows.Forms.Label();
            this.label_RateAMDDollar = new System.Windows.Forms.Label();
            this.label_RateNVIDIA2XDollar = new System.Windows.Forms.Label();
            this.label_RateNVIDIA2XBTC = new System.Windows.Forms.Label();
            this.label_RateNVIDIA2X = new System.Windows.Forms.Label();
            this.labelNVIDIA2X_Mining_Speed = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.linkLabelChooseBTCWallet = new System.Windows.Forms.LinkLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartMining
            // 
            this.buttonStartMining.Location = new System.Drawing.Point(444, 146);
            this.buttonStartMining.Name = "buttonStartMining";
            this.buttonStartMining.Size = new System.Drawing.Size(89, 23);
            this.buttonStartMining.TabIndex = 6;
            this.buttonStartMining.Text = "&Start";
            this.buttonStartMining.UseVisualStyleBackColor = true;
            this.buttonStartMining.Click += new System.EventHandler(this.buttonStartMining_Click);
            // 
            // textBoxBTCAddress
            // 
            this.textBoxBTCAddress.Location = new System.Drawing.Point(113, 39);
            this.textBoxBTCAddress.Name = "textBoxBTCAddress";
            this.textBoxBTCAddress.Size = new System.Drawing.Size(237, 20);
            this.textBoxBTCAddress.TabIndex = 1;
            this.textBoxBTCAddress.Leave += new System.EventHandler(this.textBoxCheckBoxMain_Leave);
            // 
            // labelServiceLocation
            // 
            this.labelServiceLocation.AutoSize = true;
            this.labelServiceLocation.Location = new System.Drawing.Point(8, 15);
            this.labelServiceLocation.Name = "labelServiceLocation";
            this.labelServiceLocation.Size = new System.Drawing.Size(86, 13);
            this.labelServiceLocation.TabIndex = 99;
            this.labelServiceLocation.Text = "Service location:";
            // 
            // comboBoxLocation
            // 
            this.comboBoxLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLocation.FormattingEnabled = true;
            this.comboBoxLocation.Items.AddRange(new object[] {
            "Europe - Amsterdam",
            "USA - San Jose",
            "China - Hong Kong",
            "Japan - Tokyo"});
            this.comboBoxLocation.Location = new System.Drawing.Point(113, 12);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(121, 21);
            this.comboBoxLocation.TabIndex = 0;
            this.comboBoxLocation.Leave += new System.EventHandler(this.textBoxCheckBoxMain_Leave);
            // 
            // labelBitcoinAddress
            // 
            this.labelBitcoinAddress.AutoSize = true;
            this.labelBitcoinAddress.Location = new System.Drawing.Point(8, 42);
            this.labelBitcoinAddress.Name = "labelBitcoinAddress";
            this.labelBitcoinAddress.Size = new System.Drawing.Size(82, 13);
            this.labelBitcoinAddress.TabIndex = 99;
            this.labelBitcoinAddress.Text = "Bitcoin address:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelGlobalRateText,
            this.toolStripStatusLabelGlobalRateValue,
            this.toolStripStatusLabelBTCDayText,
            this.toolStripStatusLabelBTCDayValue,
            this.toolStripStatusLabelBalanceText,
            this.toolStripStatusLabelBalanceBTCValue,
            this.toolStripStatusLabelBalanceBTCCode,
            this.toolStripStatusLabelBalanceDollarText,
            this.toolStripStatusLabelBalanceDollarValue,
            this.toolStripStatusLabel10});
            this.statusStrip1.Location = new System.Drawing.Point(0, 267);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(544, 25);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelGlobalRateText
            // 
            this.toolStripStatusLabelGlobalRateText.Name = "toolStripStatusLabelGlobalRateText";
            this.toolStripStatusLabelGlobalRateText.Size = new System.Drawing.Size(67, 20);
            this.toolStripStatusLabelGlobalRateText.Text = "Global rate:";
            // 
            // toolStripStatusLabelGlobalRateValue
            // 
            this.toolStripStatusLabelGlobalRateValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelGlobalRateValue.Name = "toolStripStatusLabelGlobalRateValue";
            this.toolStripStatusLabelGlobalRateValue.Size = new System.Drawing.Size(73, 20);
            this.toolStripStatusLabelGlobalRateValue.Text = "0.00000000";
            // 
            // toolStripStatusLabelBTCDayText
            // 
            this.toolStripStatusLabelBTCDayText.Name = "toolStripStatusLabelBTCDayText";
            this.toolStripStatusLabelBTCDayText.Size = new System.Drawing.Size(54, 20);
            this.toolStripStatusLabelBTCDayText.Text = "BTC/Day";
            // 
            // toolStripStatusLabelBTCDayValue
            // 
            this.toolStripStatusLabelBTCDayValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelBTCDayValue.Name = "toolStripStatusLabelBTCDayValue";
            this.toolStripStatusLabelBTCDayValue.Size = new System.Drawing.Size(31, 20);
            this.toolStripStatusLabelBTCDayValue.Text = "0.00";
            // 
            // toolStripStatusLabelBalanceText
            // 
            this.toolStripStatusLabelBalanceText.Name = "toolStripStatusLabelBalanceText";
            this.toolStripStatusLabelBalanceText.Size = new System.Drawing.Size(97, 20);
            this.toolStripStatusLabelBalanceText.Text = "$/Day     Balance:";
            // 
            // toolStripStatusLabelBalanceBTCValue
            // 
            this.toolStripStatusLabelBalanceBTCValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelBalanceBTCValue.Name = "toolStripStatusLabelBalanceBTCValue";
            this.toolStripStatusLabelBalanceBTCValue.Size = new System.Drawing.Size(73, 20);
            this.toolStripStatusLabelBalanceBTCValue.Text = "0.00000000";
            // 
            // toolStripStatusLabelBalanceBTCCode
            // 
            this.toolStripStatusLabelBalanceBTCCode.Name = "toolStripStatusLabelBalanceBTCCode";
            this.toolStripStatusLabelBalanceBTCCode.Size = new System.Drawing.Size(29, 20);
            this.toolStripStatusLabelBalanceBTCCode.Text = "BTC";
            // 
            // toolStripStatusLabelBalanceDollarText
            // 
            this.toolStripStatusLabelBalanceDollarText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabelBalanceDollarText.Name = "toolStripStatusLabelBalanceDollarText";
            this.toolStripStatusLabelBalanceDollarText.Size = new System.Drawing.Size(31, 20);
            this.toolStripStatusLabelBalanceDollarText.Text = "0.00";
            // 
            // toolStripStatusLabelBalanceDollarValue
            // 
            this.toolStripStatusLabelBalanceDollarValue.Name = "toolStripStatusLabelBalanceDollarValue";
            this.toolStripStatusLabelBalanceDollarValue.Size = new System.Drawing.Size(16, 20);
            this.toolStripStatusLabelBalanceDollarValue.Text = "$ ";
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Image = global::NiceHashMiner.Properties.Resources.NHM_Cash_Register_Bitcoin_transparent;
            this.toolStripStatusLabel10.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(35, 20);
            this.toolStripStatusLabel10.Click += new System.EventHandler(this.toolStripStatusLabel10_Click);
            this.toolStripStatusLabel10.MouseLeave += new System.EventHandler(this.toolStripStatusLabel10_MouseLeave);
            this.toolStripStatusLabel10.MouseHover += new System.EventHandler(this.toolStripStatusLabel10_MouseHover);
            // 
            // linkLabelCheckStats
            // 
            this.linkLabelCheckStats.AutoSize = true;
            this.linkLabelCheckStats.Location = new System.Drawing.Point(356, 42);
            this.linkLabelCheckStats.Name = "linkLabelCheckStats";
            this.linkLabelCheckStats.Size = new System.Drawing.Size(113, 13);
            this.linkLabelCheckStats.TabIndex = 9;
            this.linkLabelCheckStats.TabStop = true;
            this.linkLabelCheckStats.Text = "Check my stats online!";
            this.linkLabelCheckStats.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCheckStats_LinkClicked);
            // 
            // labelWorkerName
            // 
            this.labelWorkerName.AutoSize = true;
            this.labelWorkerName.Location = new System.Drawing.Point(8, 68);
            this.labelWorkerName.Name = "labelWorkerName";
            this.labelWorkerName.Size = new System.Drawing.Size(74, 13);
            this.labelWorkerName.TabIndex = 99;
            this.labelWorkerName.Text = "Worker name:";
            // 
            // textBoxWorkerName
            // 
            this.textBoxWorkerName.Location = new System.Drawing.Point(113, 65);
            this.textBoxWorkerName.Name = "textBoxWorkerName";
            this.textBoxWorkerName.Size = new System.Drawing.Size(60, 20);
            this.textBoxWorkerName.TabIndex = 2;
            this.textBoxWorkerName.Leave += new System.EventHandler(this.textBoxCheckBoxMain_Leave);
            // 
            // linkLabelVisitUs
            // 
            this.linkLabelVisitUs.AutoSize = true;
            this.linkLabelVisitUs.Location = new System.Drawing.Point(270, 9);
            this.linkLabelVisitUs.Name = "linkLabelVisitUs";
            this.linkLabelVisitUs.Size = new System.Drawing.Size(150, 13);
            this.linkLabelVisitUs.TabIndex = 8;
            this.linkLabelVisitUs.TabStop = true;
            this.linkLabelVisitUs.Text = "Visit us @ www.nicehash.com";
            this.linkLabelVisitUs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelVisitUs_LinkClicked);
            // 
            // listViewDevices
            // 
            this.listViewDevices.CheckBoxes = true;
            this.listViewDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewDevices.FullRowSelect = true;
            this.listViewDevices.GridLines = true;
            this.listViewDevices.Location = new System.Drawing.Point(11, 91);
            this.listViewDevices.Name = "listViewDevices";
            this.listViewDevices.Size = new System.Drawing.Size(427, 105);
            this.listViewDevices.TabIndex = 3;
            this.listViewDevices.UseCompatibleStateImageBehavior = false;
            this.listViewDevices.View = System.Windows.Forms.View.Details;
            this.listViewDevices.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "Enabled";
            this.columnHeader0.Width = 53;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Group";
            this.columnHeader2.Width = 97;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Device";
            this.columnHeader3.Width = 245;
            // 
            // buttonStopMining
            // 
            this.buttonStopMining.Enabled = false;
            this.buttonStopMining.Location = new System.Drawing.Point(444, 173);
            this.buttonStopMining.Name = "buttonStopMining";
            this.buttonStopMining.Size = new System.Drawing.Size(89, 23);
            this.buttonStopMining.TabIndex = 7;
            this.buttonStopMining.Text = "St&op";
            this.buttonStopMining.UseVisualStyleBackColor = true;
            this.buttonStopMining.Click += new System.EventHandler(this.buttonStopMining_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 99;
            this.label3.Text = "CPU Mining:";
            // 
            // labelCPU_Mining_Speed
            // 
            this.labelCPU_Mining_Speed.AutoSize = true;
            this.labelCPU_Mining_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCPU_Mining_Speed.Location = new System.Drawing.Point(126, 199);
            this.labelCPU_Mining_Speed.Name = "labelCPU_Mining_Speed";
            this.labelCPU_Mining_Speed.Size = new System.Drawing.Size(71, 13);
            this.labelCPU_Mining_Speed.TabIndex = 99;
            this.labelCPU_Mining_Speed.Text = "0.000 kH/s";
            // 
            // labelNVIDIA5X_Mining_Speed
            // 
            this.labelNVIDIA5X_Mining_Speed.AutoSize = true;
            this.labelNVIDIA5X_Mining_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelNVIDIA5X_Mining_Speed.Location = new System.Drawing.Point(126, 212);
            this.labelNVIDIA5X_Mining_Speed.Name = "labelNVIDIA5X_Mining_Speed";
            this.labelNVIDIA5X_Mining_Speed.Size = new System.Drawing.Size(74, 13);
            this.labelNVIDIA5X_Mining_Speed.TabIndex = 99;
            this.labelNVIDIA5X_Mining_Speed.Text = "0.000 MH/s";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 99;
            this.label7.Text = "GPU NVIDIA5/6 Mining:";
            // 
            // labelNVIDIA3X_Mining_Speed
            // 
            this.labelNVIDIA3X_Mining_Speed.AutoSize = true;
            this.labelNVIDIA3X_Mining_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelNVIDIA3X_Mining_Speed.Location = new System.Drawing.Point(126, 225);
            this.labelNVIDIA3X_Mining_Speed.Name = "labelNVIDIA3X_Mining_Speed";
            this.labelNVIDIA3X_Mining_Speed.Size = new System.Drawing.Size(74, 13);
            this.labelNVIDIA3X_Mining_Speed.TabIndex = 99;
            this.labelNVIDIA3X_Mining_Speed.Text = "0.000 MH/s";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 225);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 13);
            this.label9.TabIndex = 99;
            this.label9.Text = "GPU NVIDIA3.x Mining:";
            // 
            // label_RateCPU
            // 
            this.label_RateCPU.AutoSize = true;
            this.label_RateCPU.Location = new System.Drawing.Point(305, 199);
            this.label_RateCPU.Name = "label_RateCPU";
            this.label_RateCPU.Size = new System.Drawing.Size(33, 13);
            this.label_RateCPU.TabIndex = 99;
            this.label_RateCPU.Text = "Rate:";
            // 
            // label_RateCPUBTC
            // 
            this.label_RateCPUBTC.AutoSize = true;
            this.label_RateCPUBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateCPUBTC.Location = new System.Drawing.Point(346, 199);
            this.label_RateCPUBTC.Name = "label_RateCPUBTC";
            this.label_RateCPUBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateCPUBTC.TabIndex = 99;
            this.label_RateCPUBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA5XBTC
            // 
            this.label_RateNVIDIA5XBTC.AutoSize = true;
            this.label_RateNVIDIA5XBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateNVIDIA5XBTC.Location = new System.Drawing.Point(346, 212);
            this.label_RateNVIDIA5XBTC.Name = "label_RateNVIDIA5XBTC";
            this.label_RateNVIDIA5XBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateNVIDIA5XBTC.TabIndex = 99;
            this.label_RateNVIDIA5XBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA5X
            // 
            this.label_RateNVIDIA5X.AutoSize = true;
            this.label_RateNVIDIA5X.Location = new System.Drawing.Point(305, 212);
            this.label_RateNVIDIA5X.Name = "label_RateNVIDIA5X";
            this.label_RateNVIDIA5X.Size = new System.Drawing.Size(33, 13);
            this.label_RateNVIDIA5X.TabIndex = 99;
            this.label_RateNVIDIA5X.Text = "Rate:";
            // 
            // label_RateNVIDIA3XBTC
            // 
            this.label_RateNVIDIA3XBTC.AutoSize = true;
            this.label_RateNVIDIA3XBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateNVIDIA3XBTC.Location = new System.Drawing.Point(346, 225);
            this.label_RateNVIDIA3XBTC.Name = "label_RateNVIDIA3XBTC";
            this.label_RateNVIDIA3XBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateNVIDIA3XBTC.TabIndex = 99;
            this.label_RateNVIDIA3XBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA3X
            // 
            this.label_RateNVIDIA3X.AutoSize = true;
            this.label_RateNVIDIA3X.Location = new System.Drawing.Point(305, 225);
            this.label_RateNVIDIA3X.Name = "label_RateNVIDIA3X";
            this.label_RateNVIDIA3X.Size = new System.Drawing.Size(33, 13);
            this.label_RateNVIDIA3X.TabIndex = 99;
            this.label_RateNVIDIA3X.Text = "Rate:";
            // 
            // buttonBenchmark
            // 
            this.buttonBenchmark.Location = new System.Drawing.Point(444, 91);
            this.buttonBenchmark.Name = "buttonBenchmark";
            this.buttonBenchmark.Size = new System.Drawing.Size(89, 23);
            this.buttonBenchmark.TabIndex = 4;
            this.buttonBenchmark.Text = "&Benchmark";
            this.buttonBenchmark.UseVisualStyleBackColor = true;
            this.buttonBenchmark.Click += new System.EventHandler(this.buttonBenchmark_Click);
            // 
            // label_RateCPUDollar
            // 
            this.label_RateCPUDollar.AutoSize = true;
            this.label_RateCPUDollar.Location = new System.Drawing.Point(468, 199);
            this.label_RateCPUDollar.Name = "label_RateCPUDollar";
            this.label_RateCPUDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateCPUDollar.TabIndex = 99;
            this.label_RateCPUDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA5XDollar
            // 
            this.label_RateNVIDIA5XDollar.AutoSize = true;
            this.label_RateNVIDIA5XDollar.Location = new System.Drawing.Point(468, 212);
            this.label_RateNVIDIA5XDollar.Name = "label_RateNVIDIA5XDollar";
            this.label_RateNVIDIA5XDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateNVIDIA5XDollar.TabIndex = 99;
            this.label_RateNVIDIA5XDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA3XDollar
            // 
            this.label_RateNVIDIA3XDollar.AutoSize = true;
            this.label_RateNVIDIA3XDollar.Location = new System.Drawing.Point(468, 225);
            this.label_RateNVIDIA3XDollar.Name = "label_RateNVIDIA3XDollar";
            this.label_RateNVIDIA3XDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateNVIDIA3XDollar.TabIndex = 99;
            this.label_RateNVIDIA3XDollar.Text = "0.00 $/Day";
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(444, 118);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(89, 23);
            this.buttonSettings.TabIndex = 5;
            this.buttonSettings.Text = "S&ettings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // label_AMDOpenCL_Mining_Text
            // 
            this.label_AMDOpenCL_Mining_Text.AutoSize = true;
            this.label_AMDOpenCL_Mining_Text.Location = new System.Drawing.Point(8, 251);
            this.label_AMDOpenCL_Mining_Text.Name = "label_AMDOpenCL_Mining_Text";
            this.label_AMDOpenCL_Mining_Text.Size = new System.Drawing.Size(110, 13);
            this.label_AMDOpenCL_Mining_Text.TabIndex = 99;
            this.label_AMDOpenCL_Mining_Text.Text = "AMD OpenCL Mining:";
            // 
            // labelAMDOpenCL_Mining_Speed
            // 
            this.labelAMDOpenCL_Mining_Speed.AutoSize = true;
            this.labelAMDOpenCL_Mining_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAMDOpenCL_Mining_Speed.Location = new System.Drawing.Point(126, 251);
            this.labelAMDOpenCL_Mining_Speed.Name = "labelAMDOpenCL_Mining_Speed";
            this.labelAMDOpenCL_Mining_Speed.Size = new System.Drawing.Size(74, 13);
            this.labelAMDOpenCL_Mining_Speed.TabIndex = 99;
            this.labelAMDOpenCL_Mining_Speed.Text = "0.000 MH/s";
            // 
            // label_RateAMD
            // 
            this.label_RateAMD.AutoSize = true;
            this.label_RateAMD.Location = new System.Drawing.Point(305, 251);
            this.label_RateAMD.Name = "label_RateAMD";
            this.label_RateAMD.Size = new System.Drawing.Size(33, 13);
            this.label_RateAMD.TabIndex = 99;
            this.label_RateAMD.Text = "Rate:";
            // 
            // label_RateAMDBTC
            // 
            this.label_RateAMDBTC.AutoSize = true;
            this.label_RateAMDBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateAMDBTC.Location = new System.Drawing.Point(346, 251);
            this.label_RateAMDBTC.Name = "label_RateAMDBTC";
            this.label_RateAMDBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateAMDBTC.TabIndex = 99;
            this.label_RateAMDBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateAMDDollar
            // 
            this.label_RateAMDDollar.AutoSize = true;
            this.label_RateAMDDollar.Location = new System.Drawing.Point(468, 251);
            this.label_RateAMDDollar.Name = "label_RateAMDDollar";
            this.label_RateAMDDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateAMDDollar.TabIndex = 99;
            this.label_RateAMDDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA2XDollar
            // 
            this.label_RateNVIDIA2XDollar.AutoSize = true;
            this.label_RateNVIDIA2XDollar.Location = new System.Drawing.Point(468, 238);
            this.label_RateNVIDIA2XDollar.Name = "label_RateNVIDIA2XDollar";
            this.label_RateNVIDIA2XDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateNVIDIA2XDollar.TabIndex = 99;
            this.label_RateNVIDIA2XDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA2XBTC
            // 
            this.label_RateNVIDIA2XBTC.AutoSize = true;
            this.label_RateNVIDIA2XBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateNVIDIA2XBTC.Location = new System.Drawing.Point(346, 238);
            this.label_RateNVIDIA2XBTC.Name = "label_RateNVIDIA2XBTC";
            this.label_RateNVIDIA2XBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateNVIDIA2XBTC.TabIndex = 99;
            this.label_RateNVIDIA2XBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA2X
            // 
            this.label_RateNVIDIA2X.AutoSize = true;
            this.label_RateNVIDIA2X.Location = new System.Drawing.Point(305, 238);
            this.label_RateNVIDIA2X.Name = "label_RateNVIDIA2X";
            this.label_RateNVIDIA2X.Size = new System.Drawing.Size(33, 13);
            this.label_RateNVIDIA2X.TabIndex = 99;
            this.label_RateNVIDIA2X.Text = "Rate:";
            // 
            // labelNVIDIA2X_Mining_Speed
            // 
            this.labelNVIDIA2X_Mining_Speed.AutoSize = true;
            this.labelNVIDIA2X_Mining_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelNVIDIA2X_Mining_Speed.Location = new System.Drawing.Point(126, 238);
            this.labelNVIDIA2X_Mining_Speed.Name = "labelNVIDIA2X_Mining_Speed";
            this.labelNVIDIA2X_Mining_Speed.Size = new System.Drawing.Size(74, 13);
            this.labelNVIDIA2X_Mining_Speed.TabIndex = 99;
            this.labelNVIDIA2X_Mining_Speed.Text = "0.000 MH/s";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(8, 238);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(121, 13);
            this.label23.TabIndex = 99;
            this.label23.Text = "GPU NVIDIA2.1 Mining:";
            // 
            // buttonHelp
            // 
            this.buttonHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonHelp.FlatAppearance.BorderSize = 0;
            this.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHelp.Image = global::NiceHashMiner.Properties.Resources.NHM_help_50px;
            this.buttonHelp.Location = new System.Drawing.Point(473, 0);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(52, 60);
            this.buttonHelp.TabIndex = 11;
            this.buttonHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.buttonHelp.UseMnemonic = false;
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // linkLabelChooseBTCWallet
            // 
            this.linkLabelChooseBTCWallet.AutoSize = true;
            this.linkLabelChooseBTCWallet.Location = new System.Drawing.Point(179, 68);
            this.linkLabelChooseBTCWallet.Name = "linkLabelChooseBTCWallet";
            this.linkLabelChooseBTCWallet.Size = new System.Drawing.Size(165, 13);
            this.linkLabelChooseBTCWallet.TabIndex = 10;
            this.linkLabelChooseBTCWallet.TabStop = true;
            this.linkLabelChooseBTCWallet.Text = "Help me choose my Bitcoin wallet";
            this.linkLabelChooseBTCWallet.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelChooseBTCWallet_LinkClicked);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 292);
            this.Controls.Add(this.linkLabelChooseBTCWallet);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.label_RateNVIDIA2XDollar);
            this.Controls.Add(this.label_RateNVIDIA2XBTC);
            this.Controls.Add(this.label_RateNVIDIA2X);
            this.Controls.Add(this.labelNVIDIA2X_Mining_Speed);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label_RateAMDDollar);
            this.Controls.Add(this.label_RateAMDBTC);
            this.Controls.Add(this.label_RateAMD);
            this.Controls.Add(this.labelAMDOpenCL_Mining_Speed);
            this.Controls.Add(this.label_AMDOpenCL_Mining_Text);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.label_RateNVIDIA3XDollar);
            this.Controls.Add(this.label_RateNVIDIA5XDollar);
            this.Controls.Add(this.label_RateCPUDollar);
            this.Controls.Add(this.buttonBenchmark);
            this.Controls.Add(this.label_RateNVIDIA3XBTC);
            this.Controls.Add(this.label_RateNVIDIA3X);
            this.Controls.Add(this.label_RateNVIDIA5XBTC);
            this.Controls.Add(this.label_RateNVIDIA5X);
            this.Controls.Add(this.label_RateCPUBTC);
            this.Controls.Add(this.label_RateCPU);
            this.Controls.Add(this.labelNVIDIA3X_Mining_Speed);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelNVIDIA5X_Mining_Speed);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelCPU_Mining_Speed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonStopMining);
            this.Controls.Add(this.listViewDevices);
            this.Controls.Add(this.linkLabelVisitUs);
            this.Controls.Add(this.labelWorkerName);
            this.Controls.Add(this.textBoxWorkerName);
            this.Controls.Add(this.linkLabelCheckStats);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.labelBitcoinAddress);
            this.Controls.Add(this.comboBoxLocation);
            this.Controls.Add(this.labelServiceLocation);
            this.Controls.Add(this.textBoxBTCAddress);
            this.Controls.Add(this.buttonStartMining);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NiceHash Miner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartMining;
        private System.Windows.Forms.Label labelServiceLocation;
        private System.Windows.Forms.Label labelBitcoinAddress;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.LinkLabel linkLabelCheckStats;
        private System.Windows.Forms.Label labelWorkerName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGlobalRateValue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBalanceText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBalanceBTCValue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBalanceBTCCode;
        private System.Windows.Forms.LinkLabel linkLabelVisitUs;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGlobalRateText;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button buttonStopMining;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCPU_Mining_Speed;
        private System.Windows.Forms.Label labelNVIDIA5X_Mining_Speed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelNVIDIA3X_Mining_Speed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label_RateCPU;
        private System.Windows.Forms.Label label_RateCPUBTC;
        private System.Windows.Forms.Label label_RateNVIDIA5XBTC;
        private System.Windows.Forms.Label label_RateNVIDIA5X;
        private System.Windows.Forms.Label label_RateNVIDIA3XBTC;
        private System.Windows.Forms.Label label_RateNVIDIA3X;
        private System.Windows.Forms.Button buttonBenchmark;
        private System.Windows.Forms.Label label_RateCPUDollar;
        private System.Windows.Forms.Label label_RateNVIDIA5XDollar;
        private System.Windows.Forms.Label label_RateNVIDIA3XDollar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBTCDayText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBTCDayValue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBalanceDollarText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBalanceDollarValue;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label label_AMDOpenCL_Mining_Text;
        private System.Windows.Forms.Label labelAMDOpenCL_Mining_Speed;
        private System.Windows.Forms.Label label_RateAMD;
        private System.Windows.Forms.Label label_RateAMDBTC;
        private System.Windows.Forms.Label label_RateAMDDollar;
        private System.Windows.Forms.Label label_RateNVIDIA2XDollar;
        private System.Windows.Forms.Label label_RateNVIDIA2XBTC;
        private System.Windows.Forms.Label label_RateNVIDIA2X;
        private System.Windows.Forms.Label labelNVIDIA2X_Mining_Speed;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.TextBox textBoxBTCAddress;
        public System.Windows.Forms.ComboBox comboBoxLocation;
        public System.Windows.Forms.TextBox textBoxWorkerName;
        public System.Windows.Forms.ListView listViewDevices;
        private System.Windows.Forms.LinkLabel linkLabelChooseBTCWallet;
    }
}



