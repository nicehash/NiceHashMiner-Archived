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
    public partial class Form1 : Form
    {   
        private static string[] Executables = { "ccminer.exe", "sgminer.exe" };
        private static int[] APIPorts = { 4048, 0 };
        private static string[] MiningURL = { "eu", "usa", "hk", "jp" };
        private static double[] SpeedMulti = { 1, 0.001, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1000000, 1 };

        private static Process CPUMiner;
        private static Timer T;
        private static Boolean FirstTime;

        private static Timer UpdateCheck;
        private static string VisitURL = "http://www.nicehash.com";


        public Form1()
        {
            InitializeComponent();

            Text += " v" + Application.ProductVersion;

            T = new Timer();
            T.Tick += T_Tick;
            T.Interval = 60000;

            FirstTime = false;

            comboBox1.SelectedIndex = Config.ConfigData.Location;
            textBox1.Text = Config.ConfigData.BitcoinAddress;
            textBox2.Text = Config.ConfigData.WorkerName;

            // for debugging
            T_Tick(null, null);
            //

            UpdateCheck = new Timer();
            UpdateCheck.Tick += UpdateCheck_Tick;
            UpdateCheck.Interval = 1000 * 3600; // every 1 hour
            UpdateCheck.Start();
            UpdateCheck_Tick(null, null);
        }


        void UpdateCheck_Tick(object sender, EventArgs e)
        {
            string ver = NiceHashStats.GetVersion();
            if (ver == null) return;

            if (ver != Application.ProductVersion)
            {
                linkLabel2.Text = "IMPORTANT! New version v" + ver + " has\r\nbeen released. Click here to download it.";
                VisitURL = "https://github.com/nicehash/NiceHashMiner/releases/tag/" + ver;
                UpdateCheck.Stop();
            }
        }


        private void T_Tick(object sender, EventArgs e)
        {
            if (!VerifyMiningAddress()) return;

            APIData AD = APIAccess.GetSummaryccminer(APIPorts[0]);
            if (AD == null) return;

            Algorithm Algo = Algorithm.GetFromccminer(AD.AlgorithmName);

            double Rate = NiceHashStats.GetAlgorithmRate(Algo.NiceHashID);
            double Balance = NiceHashStats.GetBalance(textBox1.Text.Trim());

            double Paying = Rate * AD.Speed * SpeedMulti[Algo.NiceHashID] * 0.000001;

            toolStripStatusLabel8.Text = Algo.NiceHashName;
            toolStripStatusLabel2.Text = (AD.Speed * 0.001).ToString("F4");
            toolStripStatusLabel4.Text = Paying.ToString("F8");
            toolStripStatusLabel6.Text = Balance.ToString("F8");
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!VerifyMiningAddress()) return;

            int location = comboBox1.SelectedIndex;
            if (location > 1) location = 1;

            System.Diagnostics.Process.Start("http://www.nicehash.com/index.jsp?p=miners&addr=" + textBox1.Text.Trim());
        }


        private bool VerifyMiningAddress()
        {
            if (!BitcoinAddress.ValidateBitcoinAddress(textBox1.Text.Trim()))
            {
                MessageBox.Show("Invalid Bitcoin address!\n\nPlease, enter valid Bitcoin address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBox1.Focus();
                return false;
            }

            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (CPUMiner != null)
            {
                // stop miner
                CPUMiner.Kill();
                CPUMiner.Close();
                CPUMiner = null;

                ResetButtonText();
                return;
            }

            if (!VerifyMiningAddress()) return;

            button1.Text = "Please wait...";
            button1.Update();

            string Worker = textBox2.Text.Trim();
            if (Worker.Length > 0)
                Worker = textBox1.Text.Trim() + "." + Worker;
            else
                Worker = textBox1.Text.Trim();

            string FileName = "bin\\" + Executables[0];

            string CommandLine = "--url=sumplemultialgo+NiceHash+" + MiningURL[comboBox1.SelectedIndex] + " --userpass=" + Worker;

            CPUMiner = Process.Start(FileName, CommandLine);
            CPUMiner.EnableRaisingEvents = true;
            CPUMiner.Exited += CPUMiner_Exited;

            if (!FirstTime)
            {
                T.Start();
                FirstTime = true;
                T_Tick(null, null);
            }

            Config.ConfigData.BitcoinAddress = textBox1.Text.Trim();
            Config.ConfigData.WorkerName = textBox2.Text.Trim();
            Config.ConfigData.Location = comboBox1.SelectedIndex;
            Config.Commit();

            button1.Text = "Stop";
        }


        private void CPUMiner_Exited(object sender, EventArgs e)
        {
            CPUMiner.Close();
            CPUMiner = null;

            ResetButtonText();
        }


        delegate void ResetButtonTextCallback();

        private void ResetButtonText()
        {
            if (this.button1.InvokeRequired)
            {
                ResetButtonTextCallback d = new ResetButtonTextCallback(ResetButtonText);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.button1.Text = "Start";
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(VisitURL);
        }
    }
}
