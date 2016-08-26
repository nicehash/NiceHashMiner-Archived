using NiceHashMiner.Interfaces;
using NiceHashMiner.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NiceHashMiner.Forms {
    public partial class DownloadMinersForm : Form, IMinerUpdateIndicator {
        public DownloadMinersForm() {
            InitializeComponent();

            Start();
        }

        public void Start() {
            MinersDownloadManager.Instance.Start(this);
        }

        public void SetMaxProgressValue(int max) {
            this.Invoke((MethodInvoker)delegate {
                this.progressBar1.Maximum = max;
                this.progressBar1.Value = 0;
            });
        }

        public void SetProgressValueAndMsg(int value, string msg) {
            if(value <= this.progressBar1.Maximum) {
                this.Invoke((MethodInvoker)delegate {
                    this.progressBar1.Value = value;
                    this.label1.Text = msg;
                    this.progressBar1.Invalidate();
                    this.label1.Invalidate();
                });
            }
        }
    }
}
