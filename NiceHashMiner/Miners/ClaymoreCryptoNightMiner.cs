using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Grouping;
using NiceHashMiner.Miners.Parsing;
using NiceHashMiner.Net20_backport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NiceHashMiner.Miners {
    public class ClaymoreCryptoNightMiner : ClaymoreBaseMiner {

        const string _LOOK_FOR_START = "hashrate =";
        public ClaymoreCryptoNightMiner()
            : base("ClaymoreCryptoNightMiner", MinerPaths.ClaymoreCryptoNightMiner, "NsGpuCNMiner", _LOOK_FOR_START) {
        }

        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = " " + GetDevicesCommandString() + " -mport -" + APIPort + " -o " + url + " -u " + username + " -p x -dbg -1";
            ProcessHandle = _Start();
        }
        
        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // clean old logs
            CleanAllOldLogs();
            //benchmarkTimeWait = time;
            ////benchmarkTimeWait = 180;
            ////benchmarkTimeWait = 30; // when debugging

            // network workaround
            var nhAlgorithmData = Globals.NiceHashData[algorithm.NiceHashID];
            string url = "stratum+tcp://" + nhAlgorithmData.name + "." +
                         Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation] + ".nicehash.com:" +
                         nhAlgorithmData.port;
            // demo for benchmark
            string username = Globals.DemoUser;
            if (ConfigManager.GeneralConfig.WorkerName.Length > 0)
                username += "." + ConfigManager.GeneralConfig.WorkerName.Trim();

            string ret = " " + GetDevicesCommandString() + " -mport -" + APIPort + " -o " + url + " -u " + username + " -p x";
            return ret;
        }

    }
}
