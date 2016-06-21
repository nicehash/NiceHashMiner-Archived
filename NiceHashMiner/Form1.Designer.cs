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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_ServiceLocation = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label_BitcoinAddress = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_GlobalRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_BTCDay = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_Balance = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.linkLabel_CheckStats = new System.Windows.Forms.LinkLabel();
            this.label_WorkerName = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.linkLabel_VisitUs = new System.Windows.Forms.LinkLabel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonStopMining = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
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
            this.label_AMDOpenCL_Mining_Speed = new System.Windows.Forms.Label();
            this.label_RateAMD = new System.Windows.Forms.Label();
            this.label_RateAMDBTC = new System.Windows.Forms.Label();
            this.label_RateAMDDollar = new System.Windows.Forms.Label();
            this.label_RateNVIDIA2XDollar = new System.Windows.Forms.Label();
            this.label_RateNVIDIA2XBTC = new System.Windows.Forms.Label();
            this.label_RateNVIDIA2X = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.linkLabel_ChooseBTCWallet = new System.Windows.Forms.LinkLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStartMining
            // 
            this.buttonStartMining.Location = new System.Drawing.Point(444, 146);
            this.buttonStartMining.Name = "buttonStartMining";
            this.buttonStartMining.Size = new System.Drawing.Size(75, 23);
            this.buttonStartMining.TabIndex = 8;
            this.buttonStartMining.Text = "&Start";
            this.buttonStartMining.UseVisualStyleBackColor = true;
            this.buttonStartMining.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(104, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(237, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // label_ServiceLocation
            // 
            this.label_ServiceLocation.AutoSize = true;
            this.label_ServiceLocation.Location = new System.Drawing.Point(12, 15);
            this.label_ServiceLocation.Name = "label_ServiceLocation";
            this.label_ServiceLocation.Size = new System.Drawing.Size(86, 13);
            this.label_ServiceLocation.TabIndex = 2;
            this.label_ServiceLocation.Text = "Service location:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Europe - Amsterdam",
            "USA - San Jose",
            "China - Hong Kong",
            "Japan - Tokyo"});
            this.comboBox1.Location = new System.Drawing.Point(104, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // label_BitcoinAddress
            // 
            this.label_BitcoinAddress.AutoSize = true;
            this.label_BitcoinAddress.Location = new System.Drawing.Point(12, 42);
            this.label_BitcoinAddress.Name = "label_BitcoinAddress";
            this.label_BitcoinAddress.Size = new System.Drawing.Size(82, 13);
            this.label_BitcoinAddress.TabIndex = 4;
            this.label_BitcoinAddress.Text = "Bitcoin address:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_GlobalRate,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel_BTCDay,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel_Balance,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel8,
            this.toolStripStatusLabel10});
            this.statusStrip1.Location = new System.Drawing.Point(0, 267);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(530, 25);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_GlobalRate
            // 
            this.toolStripStatusLabel_GlobalRate.Name = "toolStripStatusLabel_GlobalRate";
            this.toolStripStatusLabel_GlobalRate.Size = new System.Drawing.Size(67, 20);
            this.toolStripStatusLabel_GlobalRate.Text = "Global rate:";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(73, 20);
            this.toolStripStatusLabel4.Text = "0.00000000";
            // 
            // toolStripStatusLabel_BTCDay
            // 
            this.toolStripStatusLabel_BTCDay.Name = "toolStripStatusLabel_BTCDay";
            this.toolStripStatusLabel_BTCDay.Size = new System.Drawing.Size(52, 20);
            this.toolStripStatusLabel_BTCDay.Text = "BTC/Day";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(31, 20);
            this.toolStripStatusLabel2.Text = "0.00";
            // 
            // toolStripStatusLabel_Balance
            // 
            this.toolStripStatusLabel_Balance.Name = "toolStripStatusLabel_Balance";
            this.toolStripStatusLabel_Balance.Size = new System.Drawing.Size(97, 20);
            this.toolStripStatusLabel_Balance.Text = "$/Day     Balance:";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(73, 20);
            this.toolStripStatusLabel6.Text = "0.00000000";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(27, 20);
            this.toolStripStatusLabel7.Text = "BTC";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(31, 20);
            this.toolStripStatusLabel3.Text = "0.00";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(16, 20);
            this.toolStripStatusLabel8.Text = "$ ";
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
            // linkLabel_CheckStats
            // 
            this.linkLabel_CheckStats.AutoSize = true;
            this.linkLabel_CheckStats.Location = new System.Drawing.Point(347, 42);
            this.linkLabel_CheckStats.Name = "linkLabel_CheckStats";
            this.linkLabel_CheckStats.Size = new System.Drawing.Size(113, 13);
            this.linkLabel_CheckStats.TabIndex = 5;
            this.linkLabel_CheckStats.TabStop = true;
            this.linkLabel_CheckStats.Text = "Check my stats online!";
            this.linkLabel_CheckStats.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label_WorkerName
            // 
            this.label_WorkerName.AutoSize = true;
            this.label_WorkerName.Location = new System.Drawing.Point(12, 68);
            this.label_WorkerName.Name = "label_WorkerName";
            this.label_WorkerName.Size = new System.Drawing.Size(74, 13);
            this.label_WorkerName.TabIndex = 10;
            this.label_WorkerName.Text = "Worker name:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(104, 65);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(60, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // linkLabel_VisitUs
            // 
            this.linkLabel_VisitUs.AutoSize = true;
            this.linkLabel_VisitUs.Location = new System.Drawing.Point(261, 9);
            this.linkLabel_VisitUs.Name = "linkLabel_VisitUs";
            this.linkLabel_VisitUs.Size = new System.Drawing.Size(150, 13);
            this.linkLabel_VisitUs.TabIndex = 4;
            this.linkLabel_VisitUs.TabStop = true;
            this.linkLabel_VisitUs.Text = "Visit us @ www.nicehash.com";
            this.linkLabel_VisitUs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(15, 91);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(423, 105);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
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
            this.buttonStopMining.Size = new System.Drawing.Size(75, 23);
            this.buttonStopMining.TabIndex = 9;
            this.buttonStopMining.Text = "St&op";
            this.buttonStopMining.UseVisualStyleBackColor = true;
            this.buttonStopMining.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "CPU Mining:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(130, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "0.000 kH/s";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(130, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "0.000 MH/s";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "GPU NVIDIA5/6 Mining:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(130, 225);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "0.000 MH/s";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 225);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "GPU NVIDIA3.x Mining:";
            // 
            // label_RateCPU
            // 
            this.label_RateCPU.AutoSize = true;
            this.label_RateCPU.Location = new System.Drawing.Point(312, 199);
            this.label_RateCPU.Name = "label_RateCPU";
            this.label_RateCPU.Size = new System.Drawing.Size(33, 13);
            this.label_RateCPU.TabIndex = 20;
            this.label_RateCPU.Text = "Rate:";
            // 
            // label_RateCPUBTC
            // 
            this.label_RateCPUBTC.AutoSize = true;
            this.label_RateCPUBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateCPUBTC.Location = new System.Drawing.Point(342, 199);
            this.label_RateCPUBTC.Name = "label_RateCPUBTC";
            this.label_RateCPUBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateCPUBTC.TabIndex = 21;
            this.label_RateCPUBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA5XBTC
            // 
            this.label_RateNVIDIA5XBTC.AutoSize = true;
            this.label_RateNVIDIA5XBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateNVIDIA5XBTC.Location = new System.Drawing.Point(342, 212);
            this.label_RateNVIDIA5XBTC.Name = "label_RateNVIDIA5XBTC";
            this.label_RateNVIDIA5XBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateNVIDIA5XBTC.TabIndex = 23;
            this.label_RateNVIDIA5XBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA5X
            // 
            this.label_RateNVIDIA5X.AutoSize = true;
            this.label_RateNVIDIA5X.Location = new System.Drawing.Point(312, 212);
            this.label_RateNVIDIA5X.Name = "label_RateNVIDIA5X";
            this.label_RateNVIDIA5X.Size = new System.Drawing.Size(33, 13);
            this.label_RateNVIDIA5X.TabIndex = 22;
            this.label_RateNVIDIA5X.Text = "Rate:";
            // 
            // label_RateNVIDIA3XBTC
            // 
            this.label_RateNVIDIA3XBTC.AutoSize = true;
            this.label_RateNVIDIA3XBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateNVIDIA3XBTC.Location = new System.Drawing.Point(342, 225);
            this.label_RateNVIDIA3XBTC.Name = "label_RateNVIDIA3XBTC";
            this.label_RateNVIDIA3XBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateNVIDIA3XBTC.TabIndex = 25;
            this.label_RateNVIDIA3XBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA3X
            // 
            this.label_RateNVIDIA3X.AutoSize = true;
            this.label_RateNVIDIA3X.Location = new System.Drawing.Point(312, 225);
            this.label_RateNVIDIA3X.Name = "label_RateNVIDIA3X";
            this.label_RateNVIDIA3X.Size = new System.Drawing.Size(33, 13);
            this.label_RateNVIDIA3X.TabIndex = 24;
            this.label_RateNVIDIA3X.Text = "Rate:";
            // 
            // buttonBenchmark
            // 
            this.buttonBenchmark.Location = new System.Drawing.Point(444, 91);
            this.buttonBenchmark.Name = "buttonBenchmark";
            this.buttonBenchmark.Size = new System.Drawing.Size(75, 23);
            this.buttonBenchmark.TabIndex = 6;
            this.buttonBenchmark.Text = "&Benchmark";
            this.buttonBenchmark.UseVisualStyleBackColor = true;
            this.buttonBenchmark.Click += new System.EventHandler(this.button3_Click);
            // 
            // label_RateCPUDollar
            // 
            this.label_RateCPUDollar.AutoSize = true;
            this.label_RateCPUDollar.Location = new System.Drawing.Point(460, 199);
            this.label_RateCPUDollar.Name = "label_RateCPUDollar";
            this.label_RateCPUDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateCPUDollar.TabIndex = 27;
            this.label_RateCPUDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA5XDollar
            // 
            this.label_RateNVIDIA5XDollar.AutoSize = true;
            this.label_RateNVIDIA5XDollar.Location = new System.Drawing.Point(460, 212);
            this.label_RateNVIDIA5XDollar.Name = "label_RateNVIDIA5XDollar";
            this.label_RateNVIDIA5XDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateNVIDIA5XDollar.TabIndex = 28;
            this.label_RateNVIDIA5XDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA3XDollar
            // 
            this.label_RateNVIDIA3XDollar.AutoSize = true;
            this.label_RateNVIDIA3XDollar.Location = new System.Drawing.Point(460, 225);
            this.label_RateNVIDIA3XDollar.Name = "label_RateNVIDIA3XDollar";
            this.label_RateNVIDIA3XDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateNVIDIA3XDollar.TabIndex = 29;
            this.label_RateNVIDIA3XDollar.Text = "0.00 $/Day";
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(444, 118);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSettings.TabIndex = 7;
            this.buttonSettings.Text = "S&ettings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.button4_Click);
            // 
            // label_AMDOpenCL_Mining_Text
            // 
            this.label_AMDOpenCL_Mining_Text.AutoSize = true;
            this.label_AMDOpenCL_Mining_Text.Location = new System.Drawing.Point(12, 251);
            this.label_AMDOpenCL_Mining_Text.Name = "label_AMDOpenCL_Mining_Text";
            this.label_AMDOpenCL_Mining_Text.Size = new System.Drawing.Size(110, 13);
            this.label_AMDOpenCL_Mining_Text.TabIndex = 30;
            this.label_AMDOpenCL_Mining_Text.Text = "AMD OpenCL Mining:";
            // 
            // label_AMDOpenCL_Mining_Speed
            // 
            this.label_AMDOpenCL_Mining_Speed.AutoSize = true;
            this.label_AMDOpenCL_Mining_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_AMDOpenCL_Mining_Speed.Location = new System.Drawing.Point(130, 251);
            this.label_AMDOpenCL_Mining_Speed.Name = "label_AMDOpenCL_Mining_Speed";
            this.label_AMDOpenCL_Mining_Speed.Size = new System.Drawing.Size(74, 13);
            this.label_AMDOpenCL_Mining_Speed.TabIndex = 31;
            this.label_AMDOpenCL_Mining_Speed.Text = "0.000 MH/s";
            // 
            // label_RateAMD
            // 
            this.label_RateAMD.AutoSize = true;
            this.label_RateAMD.Location = new System.Drawing.Point(312, 251);
            this.label_RateAMD.Name = "label_RateAMD";
            this.label_RateAMD.Size = new System.Drawing.Size(33, 13);
            this.label_RateAMD.TabIndex = 32;
            this.label_RateAMD.Text = "Rate:";
            // 
            // label_RateAMDBTC
            // 
            this.label_RateAMDBTC.AutoSize = true;
            this.label_RateAMDBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateAMDBTC.Location = new System.Drawing.Point(342, 251);
            this.label_RateAMDBTC.Name = "label_RateAMDBTC";
            this.label_RateAMDBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateAMDBTC.TabIndex = 33;
            this.label_RateAMDBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateAMDDollar
            // 
            this.label_RateAMDDollar.AutoSize = true;
            this.label_RateAMDDollar.Location = new System.Drawing.Point(460, 251);
            this.label_RateAMDDollar.Name = "label_RateAMDDollar";
            this.label_RateAMDDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateAMDDollar.TabIndex = 34;
            this.label_RateAMDDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA2XDollar
            // 
            this.label_RateNVIDIA2XDollar.AutoSize = true;
            this.label_RateNVIDIA2XDollar.Location = new System.Drawing.Point(460, 238);
            this.label_RateNVIDIA2XDollar.Name = "label_RateNVIDIA2XDollar";
            this.label_RateNVIDIA2XDollar.Size = new System.Drawing.Size(61, 13);
            this.label_RateNVIDIA2XDollar.TabIndex = 39;
            this.label_RateNVIDIA2XDollar.Text = "0.00 $/Day";
            // 
            // label_RateNVIDIA2XBTC
            // 
            this.label_RateNVIDIA2XBTC.AutoSize = true;
            this.label_RateNVIDIA2XBTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_RateNVIDIA2XBTC.Location = new System.Drawing.Point(342, 238);
            this.label_RateNVIDIA2XBTC.Name = "label_RateNVIDIA2XBTC";
            this.label_RateNVIDIA2XBTC.Size = new System.Drawing.Size(112, 13);
            this.label_RateNVIDIA2XBTC.TabIndex = 38;
            this.label_RateNVIDIA2XBTC.Text = "0.00000000 BTC/Day";
            // 
            // label_RateNVIDIA2X
            // 
            this.label_RateNVIDIA2X.AutoSize = true;
            this.label_RateNVIDIA2X.Location = new System.Drawing.Point(312, 238);
            this.label_RateNVIDIA2X.Name = "label_RateNVIDIA2X";
            this.label_RateNVIDIA2X.Size = new System.Drawing.Size(33, 13);
            this.label_RateNVIDIA2X.TabIndex = 37;
            this.label_RateNVIDIA2X.Text = "Rate:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label22.Location = new System.Drawing.Point(130, 238);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(74, 13);
            this.label22.TabIndex = 36;
            this.label22.Text = "0.000 MH/s";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(12, 238);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(121, 13);
            this.label23.TabIndex = 35;
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
            this.buttonHelp.TabIndex = 40;
            this.buttonHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.buttonHelp.UseMnemonic = false;
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // linkLabel_ChooseBTCWallet
            // 
            this.linkLabel_ChooseBTCWallet.AutoSize = true;
            this.linkLabel_ChooseBTCWallet.Location = new System.Drawing.Point(170, 68);
            this.linkLabel_ChooseBTCWallet.Name = "linkLabel_ChooseBTCWallet";
            this.linkLabel_ChooseBTCWallet.Size = new System.Drawing.Size(165, 13);
            this.linkLabel_ChooseBTCWallet.TabIndex = 41;
            this.linkLabel_ChooseBTCWallet.TabStop = true;
            this.linkLabel_ChooseBTCWallet.Text = "Help me choose my Bitcoin wallet";
            this.linkLabel_ChooseBTCWallet.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Choose_BTC_Wallet_LinkClicked);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 292);
            this.Controls.Add(this.linkLabel_ChooseBTCWallet);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.label_RateNVIDIA2XDollar);
            this.Controls.Add(this.label_RateNVIDIA2XBTC);
            this.Controls.Add(this.label_RateNVIDIA2X);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label_RateAMDDollar);
            this.Controls.Add(this.label_RateAMDBTC);
            this.Controls.Add(this.label_RateAMD);
            this.Controls.Add(this.label_AMDOpenCL_Mining_Speed);
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
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonStopMining);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.linkLabel_VisitUs);
            this.Controls.Add(this.label_WorkerName);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.linkLabel_CheckStats);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label_BitcoinAddress);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label_ServiceLocation);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.Label label_ServiceLocation;
        private System.Windows.Forms.Label label_BitcoinAddress;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.LinkLabel linkLabel_CheckStats;
        private System.Windows.Forms.Label label_WorkerName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Balance;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.LinkLabel linkLabel_VisitUs;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_GlobalRate;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button buttonStopMining;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_BTCDay;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label label_AMDOpenCL_Mining_Text;
        private System.Windows.Forms.Label label_AMDOpenCL_Mining_Speed;
        private System.Windows.Forms.Label label_RateAMD;
        private System.Windows.Forms.Label label_RateAMDBTC;
        private System.Windows.Forms.Label label_RateAMDDollar;
        private System.Windows.Forms.Label label_RateNVIDIA2XDollar;
        private System.Windows.Forms.Label label_RateNVIDIA2XBTC;
        private System.Windows.Forms.Label label_RateNVIDIA2X;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.LinkLabel linkLabel_ChooseBTCWallet;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.ComboBox comboBox1;
        public System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.ListView listView1;
    }
}



