using NiceHashMiner.Configs;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners {
    public class ClaymoreDual : ClaymoreBaseMiner {

        const string _LOOK_FOR_START = "ETH - Total Speed:";
        public ClaymoreDual(AlgorithmType secondaryAlgorithmType)
            : base("ClaymoreDual", _LOOK_FOR_START) {
            ignoreZero = true;
            api_read_mult = 1000;
            ConectionType = NHMConectionType.STRATUM_TCP;
            SecondaryAlgorithmType = secondaryAlgorithmType;
        }

        // eth-only: 1%
        // eth-dual-mine: 2%
        protected override double DevFee() {
            return (SecondaryAlgorithmType == AlgorithmType.NONE) ? 1.0 : 2.0;
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 90 * 1000; // 1.5 minute max, whole waiting time 75seconds
        }

        private string GetStartCommand(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);

            string dualModeParams = "";
            AlgorithmType dual = SecondaryAlgorithmType;
            if (dual == AlgorithmType.NONE) {  // leave convenience param for non-dual entry
                foreach (var pair in MiningSetup.MiningPairs)
                {
                    if (pair.CurrentExtraLaunchParameters.Contains("-dual="))
                    {
                        if (pair.CurrentExtraLaunchParameters.Contains("Decred"))
                        {
                            dual = AlgorithmType.Decred;
                        }
                        //if (pair.CurrentExtraLaunchParameters.Contains("Siacoin")) {
                        //    dual = AlgorithmType.;
                        //}
                        if (pair.CurrentExtraLaunchParameters.Contains("Lbry"))
                        {
                            dual = AlgorithmType.Lbry;
                        }
                        if (pair.CurrentExtraLaunchParameters.Contains("Pascal"))
                        {
                            dual = AlgorithmType.Pascal;
                        }
                        if (dual != AlgorithmType.NONE)
                        {
                            break;
                        }
                    }
                }
            }

            if (dual != AlgorithmType.NONE)
            {
                string coinP = "";
                if (dual == AlgorithmType.Decred) {
                    coinP = " -dcoin dcr ";
                }
                else if (dual == AlgorithmType.Lbry) {
                    coinP = " -dcoin lbc ";
                }
                else if (dual == AlgorithmType.Pascal) {
                    coinP = " -dcoin pasc ";
                }
                string urlSecond = Globals.GetLocationURL(dual, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
                dualModeParams = String.Format(" {0} -dpool {1} -dwal {2}", coinP, urlSecond, username);
            }

            return " "
                + GetDevicesCommandString()
                + String.Format("  -epool {0} -ewal {1} -mport 127.0.0.1:{2} -esm 3 -epsw x -allpools 1", url, username, APIPort)
                + dualModeParams;
        }

        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = GetStartCommand(url, btcAdress, worker) + " -dbg -1";
            ProcessHandle = _Start();
        }
        
        // benchmark stuff

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            // clean old logs
            CleanAllOldLogs();

            benchmarkTimeWait = time;

            // network stub
            string url = Globals.GetLocationURL(algorithm.NiceHashID, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
            // demo for benchmark
            string ret = GetStartCommand(url, Globals.DemoUser, ConfigManager.GeneralConfig.WorkerName.Trim());
            // local benhcmark
            return ret + "  -benchmark 1";
        }

    }
}
