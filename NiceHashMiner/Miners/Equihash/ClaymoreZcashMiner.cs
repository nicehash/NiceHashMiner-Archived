using Newtonsoft.Json;
using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NiceHashMiner.Miners {
    public class ClaymoreZcashMiner : ClaymoreBaseMiner {

        const string _LOOK_FOR_START = "ZEC - Total Speed:";
        public ClaymoreZcashMiner()
            : base("ClaymoreZcashMiner", _LOOK_FOR_START) {
                ignoreZero = true;
        }

        protected override double DevFee() {
            return 2.0;
        }

        
        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = " " + GetDevicesCommandString() + " -mport 127.0.0.1:" + APIPort + " -zpool " + url + " -zwal " + username + " -zpsw x -dbg -1";
            ProcessHandle = _Start();
        }

        // benchmark stuff
        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // clean old logs
            CleanAllOldLogs();
            benchmarkTimeWait = time / 3; // 3 times faster than sgminer

            string ret =  " -mport 127.0.0.1:" + APIPort + " -benchmark 1 " + GetDevicesCommandString();
            return ret;
        }
    }
}
