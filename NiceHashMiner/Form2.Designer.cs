namespace NiceHashMiner
{
    partial class Form2
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonStartBenchmark = new System.Windows.Forms.Button();
            this.buttonStopBenchmark = new System.Windows.Forms.Button();
            this.radioButton_QuickBenchmark = new System.Windows.Forms.RadioButton();
            this.radioButton_StandardBenchmark = new System.Windows.Forms.RadioButton();
            this.radioButton_PreciseBenchmark = new System.Windows.Forms.RadioButton();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSubmitHardware = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCheckProfitability = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(1, 128);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(472, 354);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "Enabled";
            this.columnHeader0.Width = 63;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Device";
            this.columnHeader1.Width = 83;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Algorithm";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Speed";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 197;
            // 
            // buttonStartBenchmark
            // 
            this.buttonStartBenchmark.Location = new System.Drawing.Point(12, 12);
            this.buttonStartBenchmark.Name = "buttonStartBenchmark";
            this.buttonStartBenchmark.Size = new System.Drawing.Size(115, 23);
            this.buttonStartBenchmark.TabIndex = 0;
            this.buttonStartBenchmark.Text = "Start &benchmark";
            this.buttonStartBenchmark.UseVisualStyleBackColor = true;
            this.buttonStartBenchmark.Click += new System.EventHandler(this.buttonStartBenchmark_Click);
            // 
            // buttonStopBenchmark
            // 
            this.buttonStopBenchmark.Enabled = false;
            this.buttonStopBenchmark.Location = new System.Drawing.Point(12, 41);
            this.buttonStopBenchmark.Name = "buttonStopBenchmark";
            this.buttonStopBenchmark.Size = new System.Drawing.Size(115, 23);
            this.buttonStopBenchmark.TabIndex = 1;
            this.buttonStopBenchmark.Text = "St&op benchmark";
            this.buttonStopBenchmark.UseVisualStyleBackColor = true;
            this.buttonStopBenchmark.Click += new System.EventHandler(this.buttonStopBenchmark_Click);
            // 
            // radioButton_QuickBenchmark
            // 
            this.radioButton_QuickBenchmark.AutoSize = true;
            this.radioButton_QuickBenchmark.Location = new System.Drawing.Point(146, 15);
            this.radioButton_QuickBenchmark.Name = "radioButton_QuickBenchmark";
            this.radioButton_QuickBenchmark.Size = new System.Drawing.Size(204, 17);
            this.radioButton_QuickBenchmark.TabIndex = 6;
            this.radioButton_QuickBenchmark.Text = "&Quick benchmark (can be inaccurate)";
            this.radioButton_QuickBenchmark.UseVisualStyleBackColor = true;
            this.radioButton_QuickBenchmark.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton_StandardBenchmark
            // 
            this.radioButton_StandardBenchmark.AutoSize = true;
            this.radioButton_StandardBenchmark.Checked = true;
            this.radioButton_StandardBenchmark.Location = new System.Drawing.Point(146, 44);
            this.radioButton_StandardBenchmark.Name = "radioButton_StandardBenchmark";
            this.radioButton_StandardBenchmark.Size = new System.Drawing.Size(124, 17);
            this.radioButton_StandardBenchmark.TabIndex = 7;
            this.radioButton_StandardBenchmark.TabStop = true;
            this.radioButton_StandardBenchmark.Text = "&Standard benchmark";
            this.radioButton_StandardBenchmark.UseVisualStyleBackColor = true;
            this.radioButton_StandardBenchmark.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton_PreciseBenchmark
            // 
            this.radioButton_PreciseBenchmark.AutoSize = true;
            this.radioButton_PreciseBenchmark.Location = new System.Drawing.Point(146, 73);
            this.radioButton_PreciseBenchmark.Name = "radioButton_PreciseBenchmark";
            this.radioButton_PreciseBenchmark.Size = new System.Drawing.Size(195, 17);
            this.radioButton_PreciseBenchmark.TabIndex = 8;
            this.radioButton_PreciseBenchmark.Text = "&Precise benchmark (will take longer)";
            this.radioButton_PreciseBenchmark.UseVisualStyleBackColor = true;
            this.radioButton_PreciseBenchmark.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(12, 70);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(115, 23);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "&Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSubmitHardware
            // 
            this.buttonSubmitHardware.Location = new System.Drawing.Point(254, 99);
            this.buttonSubmitHardware.Name = "buttonSubmitHardware";
            this.buttonSubmitHardware.Size = new System.Drawing.Size(115, 23);
            this.buttonSubmitHardware.TabIndex = 5;
            this.buttonSubmitHardware.Text = "Submit &hardware";
            this.buttonSubmitHardware.UseVisualStyleBackColor = true;
            this.buttonSubmitHardware.Click += new System.EventHandler(this.buttonSubmitHardware_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(12, 99);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(115, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCheckProfitability
            // 
            this.buttonCheckProfitability.Location = new System.Drawing.Point(133, 99);
            this.buttonCheckProfitability.Name = "buttonCheckProfitability";
            this.buttonCheckProfitability.Size = new System.Drawing.Size(115, 23);
            this.buttonCheckProfitability.TabIndex = 4;
            this.buttonCheckProfitability.Text = "Check &profitability";
            this.buttonCheckProfitability.UseVisualStyleBackColor = true;
            this.buttonCheckProfitability.Click += new System.EventHandler(this.buttonCheckProfitability_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 484);
            this.Controls.Add(this.buttonCheckProfitability);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSubmitHardware);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.radioButton_PreciseBenchmark);
            this.Controls.Add(this.radioButton_StandardBenchmark);
            this.Controls.Add(this.radioButton_QuickBenchmark);
            this.Controls.Add(this.buttonStopBenchmark);
            this.Controls.Add(this.buttonStartBenchmark);
            this.Controls.Add(this.listView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(490, 200);
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Benchmark";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button buttonStartBenchmark;
        private System.Windows.Forms.Button buttonStopBenchmark;
        private System.Windows.Forms.RadioButton radioButton_QuickBenchmark;
        private System.Windows.Forms.RadioButton radioButton_StandardBenchmark;
        private System.Windows.Forms.RadioButton radioButton_PreciseBenchmark;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSubmitHardware;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCheckProfitability;
    }
}