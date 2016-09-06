namespace NiceHashMiner.Forms.Components {
    partial class Field {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.labelFieldIndicator = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelFieldIndicator
            // 
            this.labelFieldIndicator.AutoSize = true;
            this.labelFieldIndicator.Location = new System.Drawing.Point(3, 6);
            this.labelFieldIndicator.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelFieldIndicator.Name = "labelFieldIndicator";
            this.labelFieldIndicator.Size = new System.Drawing.Size(98, 13);
            this.labelFieldIndicator.TabIndex = 6;
            this.labelFieldIndicator.Text = "Label field indicator";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(3, 24);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(214, 20);
            this.textBox.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NiceHashMiner.Properties.Resources.info_black_18;
            this.pictureBox1.Location = new System.Drawing.Point(197, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 19);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // Field
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelFieldIndicator);
            this.Controls.Add(this.textBox);
            this.Name = "Field";
            this.Size = new System.Drawing.Size(220, 48);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFieldIndicator;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.PictureBox pictureBox1;



    }
}
