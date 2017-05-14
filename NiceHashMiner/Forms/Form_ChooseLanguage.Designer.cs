namespace NiceHashMiner.Forms {
    partial class Form_ChooseLanguage {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ChooseLanguage));
            this.label_Instruction = new System.Windows.Forms.Label();
            this.comboBox_Languages = new System.Windows.Forms.ComboBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.checkBox_TOS = new System.Windows.Forms.CheckBox();
            this.textBox_TOS = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label_Instruction
            // 
            this.label_Instruction.AutoSize = true;
            this.label_Instruction.Location = new System.Drawing.Point(10, 367);
            this.label_Instruction.Name = "label_Instruction";
            this.label_Instruction.Size = new System.Drawing.Size(231, 13);
            this.label_Instruction.TabIndex = 0;
            this.label_Instruction.Text = "Choose a default language for NiceHash Miner:";
            // 
            // comboBox_Languages
            // 
            this.comboBox_Languages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Languages.Enabled = false;
            this.comboBox_Languages.FormattingEnabled = true;
            this.comboBox_Languages.Location = new System.Drawing.Point(15, 384);
            this.comboBox_Languages.Name = "comboBox_Languages";
            this.comboBox_Languages.Size = new System.Drawing.Size(195, 21);
            this.comboBox_Languages.TabIndex = 1;
            // 
            // button_OK
            // 
            this.button_OK.Enabled = false;
            this.button_OK.Location = new System.Drawing.Point(216, 382);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(106, 23);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // checkBox_TOS
            // 
            this.checkBox_TOS.AutoSize = true;
            this.checkBox_TOS.Location = new System.Drawing.Point(292, 359);
            this.checkBox_TOS.Name = "checkBox_TOS";
            this.checkBox_TOS.Size = new System.Drawing.Size(151, 17);
            this.checkBox_TOS.TabIndex = 3;
            this.checkBox_TOS.Text = "I accept the Terms Of Use";
            this.checkBox_TOS.UseVisualStyleBackColor = true;
            this.checkBox_TOS.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBox_TOS
            // 
            this.textBox_TOS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_TOS.Location = new System.Drawing.Point(13, 13);
            this.textBox_TOS.Multiline = true;
            this.textBox_TOS.Name = "textBox_TOS";
            this.textBox_TOS.ReadOnly = true;
            this.textBox_TOS.Size = new System.Drawing.Size(430, 340);
            this.textBox_TOS.TabIndex = 4;
            this.textBox_TOS.TabStop = false;
            this.textBox_TOS.Text = resources.GetString("textBox_TOS.Text");
            // 
            // Form_ChooseLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 427);
            this.Controls.Add(this.textBox_TOS);
            this.Controls.Add(this.checkBox_TOS);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.comboBox_Languages);
            this.Controls.Add(this.label_Instruction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ChooseLanguage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NiceHash Miner EULA  / Choose Language";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Instruction;
        private System.Windows.Forms.ComboBox comboBox_Languages;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.CheckBox checkBox_TOS;
        private System.Windows.Forms.TextBox textBox_TOS;
    }
}