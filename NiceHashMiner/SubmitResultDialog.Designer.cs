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
            this.BenchmarkBtn = new System.Windows.Forms.Button();
            this.DevicesListView = new System.Windows.Forms.ListView();
            this.Group = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BenchmarkProgressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.LabelProgressPercentage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(396, 223);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 0;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // BenchmarkBtn
            // 
            this.BenchmarkBtn.Location = new System.Drawing.Point(315, 223);
            this.BenchmarkBtn.Name = "BenchmarkBtn";
            this.BenchmarkBtn.Size = new System.Drawing.Size(75, 23);
            this.BenchmarkBtn.TabIndex = 2;
            this.BenchmarkBtn.Text = "Start";
            this.BenchmarkBtn.UseVisualStyleBackColor = true;
            this.BenchmarkBtn.Click += new System.EventHandler(this.BenchmarkBtn_Click);
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
            this.DevicesListView.Size = new System.Drawing.Size(455, 136);
            this.DevicesListView.TabIndex = 1;
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
            this.DeviceName.Width = 309;
            // 
            // BenchmarkProgressBar
            // 
            this.BenchmarkProgressBar.Location = new System.Drawing.Point(16, 191);
            this.BenchmarkProgressBar.Maximum = 10;
            this.BenchmarkProgressBar.Name = "BenchmarkProgressBar";
            this.BenchmarkProgressBar.Size = new System.Drawing.Size(374, 23);
            this.BenchmarkProgressBar.Step = 1;
            this.BenchmarkProgressBar.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(447, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please select a device to be benchmarked and once it is done, it will automatiall" +
    "y open a new\r\nweb page to display the profitability calculator on the NiceHash\'s" +
    " website.";
            // 
            // LabelProgressPercentage
            // 
            this.LabelProgressPercentage.BackColor = System.Drawing.SystemColors.Control;
            this.LabelProgressPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelProgressPercentage.Location = new System.Drawing.Point(397, 191);
            this.LabelProgressPercentage.Name = "LabelProgressPercentage";
            this.LabelProgressPercentage.Size = new System.Drawing.Size(74, 23);
            this.LabelProgressPercentage.TabIndex = 5;
            this.LabelProgressPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SubmitResultDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 254);
            this.Controls.Add(this.LabelProgressPercentage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesListView);
            this.Controls.Add(this.BenchmarkBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.BenchmarkProgressBar);
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
        private System.Windows.Forms.Button BenchmarkBtn;
        private System.Windows.Forms.ListView DevicesListView;
        private System.Windows.Forms.ProgressBar BenchmarkProgressBar;
        private System.Windows.Forms.ColumnHeader DeviceName;
        private System.Windows.Forms.ColumnHeader Group;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LabelProgressPercentage;
    }
}