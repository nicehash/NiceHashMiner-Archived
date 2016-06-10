using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace NiceHashMiner
{
    public partial class Form5 : Form
    {
        private Process EthMinerProcess;
        private Timer CheckTimer;
        private EthminerReader EthReader;
        private string Worker;

        public bool Success;
        public string Error;

        public Form5(string worker, Process eprocess)
        {
            InitializeComponent();

            Success = false;

            Worker = worker;
            EthMinerProcess = eprocess;
            EthMinerProcess.Start();

            EthReader = new EthminerReader(37450);
            EthReader.Start();

            CheckTimer = new Timer();
            CheckTimer.Interval = 1000;
            CheckTimer.Tick += CheckTimer_Tick;
            CheckTimer.Start();
        }

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            uint prog = EthReader.GetDAGprogress();

            if (progressBar1.Value != (int)prog)
            {
                Helpers.ConsolePrint(Worker, "DAG progress: " + prog.ToString() + "%");
                progressBar1.Value = (int)prog;
                // todo: display % on progress bar... some GUI work
            }

            if (EthMinerProcess.HasExited)
            {
                System.Threading.Thread.Sleep(500);
                if (EthReader.GetDAGprogress() == 100)
                {
                    Success = true;
                    Terminate(null);
                }
                else
                {
                    Terminate("ethminer crashed");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EthMinerProcess.Kill();
            Terminate("User terminated");
        }

        private void Terminate(string err)
        {
            CheckTimer.Stop();
            EthMinerProcess.Close();
            EthReader.Stop();
            Error = err;
            Close();
        }
    }
}
