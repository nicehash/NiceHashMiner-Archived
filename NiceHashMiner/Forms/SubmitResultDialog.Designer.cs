namespace NiceHashMiner
{
    partial class SubmitResultDialog
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
            this.CloseBtn = new System.Windows.Forms.Button();
            this.StartStopBtn = new System.Windows.Forms.Button();
            this.DevicesListView = new System.Windows.Forms.ListView();
            this.Group = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BenchmarkProgressBar = new System.Windows.Forms.ProgressBar();
            this.labelInstruction = new System.Windows.Forms.Label();
            this.LabelProgressPercentage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(472, 220);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 2;
            this.CloseBtn.Text = "&Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // StartStopBtn
            // 
            this.StartStopBtn.Location = new System.Drawing.Point(391, 220);
            this.StartStopBtn.Name = "StartStopBtn";
            this.StartStopBtn.Size = new System.Drawing.Size(75, 23);
            this.StartStopBtn.TabIndex = 1;
            this.StartStopBtn.Text = "&Start";
            this.StartStopBtn.UseVisualStyleBackColor = true;
            this.StartStopBtn.Click += new System.EventHandler(this.BenchmarkBtn_Click);
            // 
            // DevicesListView
            // 
            this.DevicesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Group,
            this.DeviceName});
            this.DevicesListView.FullRowSelect = true;
            this.DevicesListView.GridLines = true;
            this.DevicesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.DevicesListView.Location = new System.Drawing.Point(16, 46);
            this.DevicesListView.MultiSelect = false;
            this.DevicesListView.Name = "DevicesListView";
            this.DevicesListView.Size = new System.Drawing.Size(531, 136);
            this.DevicesListView.TabIndex = 0;
            this.DevicesListView.UseCompatibleStateImageBehavior = false;
            this.DevicesListView.View = System.Windows.Forms.View.Details;
            // 
            // Group
            // 
            this.Group.Text = "Group";
            this.Group.Width = 137;
            // 
            // DeviceName
            // 
            this.DeviceName.Text = "Device Name";
            this.DeviceName.Width = 351;
            // 
            // BenchmarkProgressBar
            // 
            this.BenchmarkProgressBar.Location = new System.Drawing.Point(16, 191);
            this.BenchmarkProgressBar.Maximum = 10;
            this.BenchmarkProgressBar.Name = "BenchmarkProgressBar";
            this.BenchmarkProgressBar.Size = new System.Drawing.Size(531, 23);
            this.BenchmarkProgressBar.Step = 1;
            this.BenchmarkProgressBar.TabIndex = 99;
            // 
            // labelInstruction
            // 
            this.labelInstruction.AutoSize = true;
            this.labelInstruction.Location = new System.Drawing.Point(13, 13);
            this.labelInstruction.Name = "labelInstruction";
            this.labelInstruction.Size = new System.Drawing.Size(447, 26);
            this.labelInstruction.TabIndex = 99;
            this.labelInstruction.Text = "Please select a device to be benchmarked and _benchmarkOnce it is done, it will automatiall" +
    "y open a new\r\nweb page to display the profitability calculator on the NiceHash\'s" +
    " website.";
            // 
            // LabelProgressPercentage
            // 
            this.LabelProgressPercentage.BackColor = System.Drawing.SystemColors.Control;
            this.LabelProgressPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelProgressPercentage.Location = new System.Drawing.Point(16, 223);
            this.LabelProgressPercentage.Name = "LabelProgressPercentage";
            this.LabelProgressPercentage.Size = new System.Drawing.Size(293, 23);
            this.LabelProgressPercentage.TabIndex = 99;
            this.LabelProgressPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SubmitResultDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 254);
            this.Controls.Add(this.LabelProgressPercentage);
            this.Controls.Add(this.labelInstruction);
            this.Controls.Add(this.DevicesListView);
            this.Controls.Add(this.StartStopBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.BenchmarkProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubmitResultDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Submit Benchmark Result";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button StartStopBtn;
        private System.Windows.Forms.ListView DevicesListView;
        private System.Windows.Forms.ProgressBar BenchmarkProgressBar;
        private System.Windows.Forms.ColumnHeader DeviceName;
        private System.Windows.Forms.ColumnHeader Group;
        private System.Windows.Forms.Label labelInstruction;
        private System.Windows.Forms.Label LabelProgressPercentage;
    }
}