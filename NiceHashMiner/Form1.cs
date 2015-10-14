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
        private static Miner[] Miners = new Miner[1];
        private static string[] MiningURL = { "eu", "usa", "hk", "jp" };
        private static string VisitURL = "http://www.nicehash.com";

        private Timer T;
        private int Tcheck;

        private Timer UpdateCheck;

        private double[] PayRates = null;

        public Form1()
        {
            InitializeComponent();

            Miners[0] = new ccminer();

            Text += " v" + Application.ProductVersion;

            T = new Timer();
            T.Tick += T_Tick;
            T.Interval = 5000;

            comboBox1.SelectedIndex = Config.ConfigData.Location;
            textBox1.Text = Config.ConfigData.BitcoinAddress;
            textBox2.Text = Config.ConfigData.WorkerName;

            foreach (GPUData G in Miners[0].GPUs)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(G.ID.ToString());
                lvi.SubItems.Add("NVIDIA");
                lvi.SubItems.Add(G.Name);
                lvi.Checked = G.Enabled;
                lvi.Tag = G;
                listView1.Items.Add(lvi);
            }

            // for debugging
            //T_Tick(null, null);
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
            Tcheck++;
            if (!VerifyMiningAddress()) return;

            APIData AD = Miners[0].GetSummary();
            if (AD == null) return;

            if (Tcheck % 12 == 0)
            {
                PayRates = NiceHashStats.GetAlgorithmRates();
                double Balance = NiceHashStats.GetBalance(textBox1.Text.Trim());
                toolStripStatusLabel6.Text = Balance.ToString("F8");
            }

            if (PayRates != null)
            {
                double Paying = PayRates[AD.AlgorithmID] * AD.Speed * 0.000000001;
                SetStats(AD.AlgorithmName, AD.Speed * 0.000001, Paying);
            }
        }


        private void SetStats(string aname, double speed, double paying)
        {
            toolStripStatusLabel8.Text = aname;
            toolStripStatusLabel2.Text = speed.ToString("F4");
            toolStripStatusLabel4.Text = paying.ToString("F8");
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
            if (button1.Text == "Stop")
            {
                Miners[0].Stop();
                T.Stop();
                button1.Text = "Start";

                SetStats("-", 0, 0);

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

            Miners[0].Start(MiningURL[comboBox1.SelectedIndex], Worker);

            Config.ConfigData.BitcoinAddress = textBox1.Text.Trim();
            Config.ConfigData.WorkerName = textBox2.Text.Trim();
            Config.ConfigData.Location = comboBox1.SelectedIndex;
            Config.Commit();

            button1.Text = "Stop";

            Tcheck = 11;
            T.Start();
        }


        //delegate void ResetButtonTextCallback();

        //private void ResetButtonText()
        //{
        //    if (this.button1.InvokeRequired)
        //    {
        //        ResetButtonTextCallback d = new ResetButtonTextCallback(ResetButtonText);
        //        this.Invoke(d, new object[] { });
        //    }
        //    else
        //    {
        //        this.button1.Text = "Start";
        //    }
        //}

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(VisitURL);
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            GPUData G = e.Item.Tag as GPUData;
            G.Enabled = e.Item.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Miners[0].Stop();
            T.Stop();
        }
    }
}
