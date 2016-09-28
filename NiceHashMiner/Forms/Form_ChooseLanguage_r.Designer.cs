namespace NiceHashMiner.Forms {
    partial class Form_ChooseLanguage_r {
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
            this.label_Instruction = new System.Windows.Forms.Label();
            this.comboBox_Languages = new System.Windows.Forms.ComboBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_Instruction
            // 
            this.label_Instruction.AutoSize = true;
            this.label_Instruction.Location = new System.Drawing.Point(52, 9);
            this.label_Instruction.Name = "label_Instruction";
            this.label_Instruction.Size = new System.Drawing.Size(231, 13);
            this.label_Instruction.TabIndex = 0;
            this.label_Instruction.Text = "Choose a default language for NiceHash Miner:";
            // 
            // comboBox_Languages
            // 
            this.comboBox_Languages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Languages.FormattingEnabled = true;
            this.comboBox_Languages.Location = new System.Drawing.Point(66, 29);
            this.comboBox_Languages.Name = "comboBox_Languages";
            this.comboBox_Languages.Size = new System.Drawing.Size(195, 21);
            this.comboBox_Languages.TabIndex = 1;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(110, 56);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(106, 23);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // Form_ChooseLanguage_r
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 91);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.comboBox_Languages);
            this.Controls.Add(this.label_Instruction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ChooseLanguage_r";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Language";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Instruction;
        private System.Windows.Forms.ComboBox comboBox_Languages;
        private System.Windows.Forms.Button button_OK;
    }
}