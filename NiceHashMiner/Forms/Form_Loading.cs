using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public partial class Form_Loading : Form
    {
        public interface AfterInitializationCaller {
            void AfterLoadComplete();
        }

        private int LoadCounter = 0;
        // TODO find out what are the 13 loading steps, think if this should really be hardcoded
        private int TotalLoadSteps = 13;
        private AfterInitializationCaller AfterInitCaller;

        public Form_Loading(AfterInitializationCaller initCaller, string startInfoMsg)
        {
            InitializeComponent();

            label_LoadingText.Text = International.GetText("form3_label_LoadingText");
            label_LoadingText.Location = new Point((this.Size.Width - label_LoadingText.Size.Width) / 2, label_LoadingText.Location.Y);

            // TODO assert that this is never null
            AfterInitCaller = initCaller;

            this.progressBar1.Maximum = TotalLoadSteps;
            this.progressBar1.Value = 0;

            SetInfoMsg(startInfoMsg);
        }

        public void IncreaseLoadCounterAndMessage(string infoMsg) {
            SetInfoMsg(infoMsg);
            IncreaseLoadCounter();
        }

        // TODO maybe remove this method
        public void SetProgressMaxValue(int maxValue) {
            this.progressBar1.Maximum = maxValue;
        }
        public void SetInfoMsg(string infoMsg) {
            this.LoadText.Text = infoMsg;
        }

        public void IncreaseLoadCounter() {
            LoadCounter++;
            this.progressBar1.Value = LoadCounter;
            this.Update();
            if (LoadCounter >= TotalLoadSteps) {
                AfterInitCaller.AfterLoadComplete();
                this.Close();
                this.Dispose();
            }
        }
    }
}
