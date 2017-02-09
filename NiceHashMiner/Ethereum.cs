using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using NiceHashMiner.Configs;
using NiceHashMiner.Miners;
using NiceHashMiner.Miners.Grouping;

namespace NiceHashMiner
{
    //public class EthminerReader
    //{
    //    /// <summary>
    //    /// Initialize ethminer instance that listens on certain UDP port for speed and DAG progress reports. You may have multiple instances,
    //    /// but each one must be listening on another port!
    //    /// </summary>
    //    /// <param name="port">UDP listening port.</param>
    //    public EthminerReader(int port)
    //    {
    //        bindPort = port;
    //    }

    //    /// <summary>
    //    /// Start listening.
    //    /// </summary>
    //    public void Start()
    //    {
    //        isRunning = true;
    //        speed = 0;
    //        DAGprogress = 0;
    //        lastActiveTime = DateTime.Now;
    //        ipep = new IPEndPoint(IPAddress.Loopback, bindPort);
    //        client = new UdpClient(ipep);
    //        workerTimer = new Timer();
    //        workerTimer.Tick += workerTimer_Tick;
    //        workerTimer.Interval = 50;
    //        workerTimer.Start();
    //    }

    //    /// <summary>
    //    /// Stop listening. Call this before application exits or if you are about to restart ethminer reader.
    //    /// </summary>
    //    public void Stop()
    //    {
    //        isRunning = false;
    //        speed = 0;
    //        DAGprogress = 0;
    //        workerTimer.Stop();
    //        client.Close();
    //        client = null;
    //    }

    //    /// <summary>
    //    /// Get if miner is still running.
    //    /// </summary>
    //    /// <returns>State of miner.</returns>
    //    public bool GetIsRunning()
    //    {
    //        return isRunning;
    //    }

    //    /// <summary>
    //    /// Get speed of miner in MH/s.
    //    /// </summary>
    //    /// <returns>Speed of miner in MH/s.</returns>
    //    public double GetSpeed()
    //    {
    //        return speed;
    //    }

    //    /// <summary>
    //    /// Get DAG progress in %. It only reports correct progress if ethminer is launched with -D parameter.
    //    /// </summary>
    //    /// <returns>DAG creation progress in %. When mining, this value is 100.</returns>
    //    public uint GetDAGprogress()
    //    {
    //        return DAGprogress;
    //    }

    //    /// <summary>
    //    /// Get DateTime when miner sent UDP status packet.
    //    /// </summary>
    //    /// <returns>DateTime of last active time.</returns>
    //    public DateTime GetLastActiveTime()
    //    {
    //        return lastActiveTime;
    //    }

    //    public string ByteArrayToString(byte[] ba)
    //    {
    //        StringBuilder hex = new StringBuilder(ba.Length * 2);
    //        foreach (byte b in ba) hex.AppendFormat("{0:x2} ", b);
    //        return hex.ToString();
    //    }

    //    #region PRIVATE_PROPERTIES
    //    private IPEndPoint ipep;
    //    private UdpClient client;
    //    private double speed;
    //    private uint DAGprogress;
    //    private int bindPort;
    //    private bool isRunning;
    //    private Timer workerTimer;
    //    private DateTime lastActiveTime;
    //    #endregion

    //    #region PRIVATE_METHODS
    //    private void workerTimer_Tick(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            client.Client.Blocking = false;

    //            if (isRunning && client.Available > 0)
    //            {
    //                byte[] data = client.Receive(ref ipep);
    //                //Helpers.ConsolePrint("DEBUG", "ByteArray: " + ByteArrayToString(data));
    //                speed = BitConverter.ToDouble(data, 0);
    //                DAGprogress = BitConverter.ToUInt32(data, 8);
    //                lastActiveTime = DateTime.Now;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Helpers.ConsolePrint("EthminerReader", "Error: " + ex.Message);
    //        }
    //    }

    //    #endregion
    //}

    public static class Ethereum
    {
        //public static string EtherMinerPath;
        public static string CurrentBlockNum;

        static Ethereum()
        {
            CurrentBlockNum = "";
        }

        public static void GetCurrentBlock(string worker)
        {
            string ret = NiceHashStats.GetNiceHashAPIData("https://etherchain.org/api/blocks/count", worker);
            
            if (ret == null)
            {
                Helpers.ConsolePrint(worker, String.Format("Failed to obtain current block, using default {0}.", ConfigManager.GeneralConfig.ethminerDefaultBlockHeight));
                CurrentBlockNum = ConfigManager.GeneralConfig.ethminerDefaultBlockHeight.ToString();
            }
            else
            {
                ret = ret.Substring(ret.LastIndexOf("count") + 7);
                CurrentBlockNum = ret.Substring(0, ret.Length - 3);
            }
        }
    }
}
