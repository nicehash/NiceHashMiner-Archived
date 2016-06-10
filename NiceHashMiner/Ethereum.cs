using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace NiceHashMiner
{
    public class EthminerReader
    {
        /// <summary>
        /// Initialize ethminer instance that listens on certain UDP port for speed and DAG progress reports. You may have multiple instances,
        /// but each one must be listening on another port!
        /// </summary>
        /// <param name="port">UDP listening port.</param>
        public EthminerReader(int port)
        {
            bindPort = port;
        }

        /// <summary>
        /// Start listening.
        /// </summary>
        public void Start()
        {
            isRunning = true;
            speed = 0;
            DAGprogress = 0;
            lastActiveTime = DateTime.Now;
            ipep = new IPEndPoint(IPAddress.Any, bindPort);
            client = new UdpClient(ipep);
            workerTimer = new Timer();
            workerTimer.Tick += workerTimer_Tick;
            workerTimer.Interval = 50;
            workerTimer.Start();
        }

        /// <summary>
        /// Stop listening. Call this before application exits or if you are about to restart ethminer reader.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            speed = 0;
            DAGprogress = 0;
            workerTimer.Stop();
            client.Close();
            client = null;
        }

        /// <summary>
        /// Get if miner is still running.
        /// </summary>
        /// <returns>State of miner.</returns>
        public bool GetIsRunning()
        {
            return isRunning;
        }

        /// <summary>
        /// Get speed of miner in MH/s.
        /// </summary>
        /// <returns>Speed of miner in MH/s.</returns>
        public double GetSpeed()
        {
            return speed;
        }

        /// <summary>
        /// Get DAG progress in %. It only reports correct progress if ethminer is launched with -D parameter.
        /// </summary>
        /// <returns>DAG creation progress in %. When mining, this value is 100.</returns>
        public uint GetDAGprogress()
        {
            return DAGprogress;
        }

        /// <summary>
        /// Get DateTime when miner sent UDP status packet.
        /// </summary>
        /// <returns>DateTime of last active time.</returns>
        public DateTime GetLastActiveTime()
        {
            return lastActiveTime;
        }

        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba) hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }

        #region PRIVATE_PROPERTIES
        private IPEndPoint ipep;
        private UdpClient client;
        private double speed;
        private uint DAGprogress;
        private int bindPort;
        private bool isRunning;
        private Timer workerTimer;
        private DateTime lastActiveTime;
        #endregion

        #region PRIVATE_METHODS
        private void workerTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                client.Client.Blocking = false;

                if (isRunning && client.Available > 0)
                {
                    byte[] data = client.Receive(ref ipep);
                    //Helpers.ConsolePrint("DEBUG", "ByteArray: " + ByteArrayToString(data));
                    speed = BitConverter.ToDouble(data, 0);
                    DAGprogress = BitConverter.ToUInt32(data, 8);
                    lastActiveTime = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint("EthminerReader", "Error: " + ex.Message);
            }
        }

        #endregion
    }

    public static class Ethereum
    {
        public static string EtherMinerPath;
        public static string CurrentBlockNum;

        static Ethereum()
        {
            EtherMinerPath = "bin\\ethereum\\ethminer.exe";
            CurrentBlockNum = "";
        }

        public static bool CreateDAGFile(bool HideWindow, string worker, out string err)
        {
            err = null;

            try
            {
                if (!GetCurrentBlock(worker)) throw new Exception("GetCurrentBlock returns null..");

                // Check if dag-dir exist to avoid ethminer from crashing
                Helpers.ConsolePrint(worker, "Creating DAG directory for " + worker + "..");
                if (!CreateDAGDirectory(worker)) throw new Exception("[" + worker + "] Cannot create directory for DAG files.");

                if (worker.Equals("NVIDIA5.x"))
                {
                    CopyDAGFiles("NVIDIA3.x", worker);
                    CopyDAGFiles("AMD_OpenCL", worker);
                }
                else if (worker.Equals("NVIDIA3.x"))
                {
                    CopyDAGFiles("NVIDIA5.x", worker);
                    CopyDAGFiles("AMD_OpenCL", worker);
                }
                else if (worker.Equals("AMD_OpenCL"))
                {
                    CopyDAGFiles("NVIDIA5.x", worker);
                    CopyDAGFiles("NVIDIA3.x", worker);
                }

                Helpers.ConsolePrint(worker, "Creating DAG file for " + worker + "..");
                
                Process P = new Process();
                P.StartInfo.FileName = EtherMinerPath;
                P.StartInfo.Arguments = " --dag-dir " + Config.ConfigData.DAGDirectory + "\\" + worker + " --create-dag " + CurrentBlockNum;
                Helpers.ConsolePrint(worker, "CreateDAGFile Arguments: " + P.StartInfo.Arguments);
                P.StartInfo.CreateNoWindow = HideWindow;
                P.StartInfo.UseShellExecute = !HideWindow;

                Form5 f = new Form5(worker, P);
                f.ShowDialog();

                if (f.Success)
                    return true;
                else
                    throw new Exception(f.Error);
                //P.Start();
                
                //P.WaitForExit();

                //P.Close();
                //P = null;
            }
            catch (Exception e)
            {
                err = e.Message;
                Helpers.ConsolePrint(worker, "Exception: " + e.Message);
                return false;
            }

            //return true;
        }

        public static bool CreateDAGDirectory(string worker)
        {
            try
            {
                if (!Directory.Exists(Config.ConfigData.DAGDirectory + "\\" + worker))
                    Directory.CreateDirectory(Config.ConfigData.DAGDirectory + "\\" + worker);
            }
            catch { return false; }

            return true;
        }
        
        public static bool GetCurrentBlock(string worker)
        {
            string ret = NiceHashStats.GetNiceHashAPIData("https://etherchain.org/api/blocks/count", worker);
            if (ret == null) return false;
            ret = ret.Substring(ret.LastIndexOf("count") + 7);
            CurrentBlockNum = ret.Substring(0, ret.Length - 3);

            return true;
        }

        private static void CopyDAGFiles(string from, string to)
        {
            if (Directory.Exists(Config.ConfigData.DAGDirectory + "\\" + from))
            {
                string src = Config.ConfigData.DAGDirectory + "\\" + from;
                foreach (var file in Directory.GetFiles(src))
                {
                    string dest = Path.Combine(Config.ConfigData.DAGDirectory + "\\" + to, Path.GetFileName(file));
                    if (file.Contains("full") && !File.Exists(dest)) File.Copy(file, dest, false);
                }
            }
        }
    }
}
