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
            return IsDual() ? 2.0 : 1.0;
        }

        // the short form the miner uses for secondary algo in cmd line and log
        public string SecondaryShortName() {
            switch (SecondaryAlgorithmType) {
                case AlgorithmType.Decred:
                    return "dcr";
                case AlgorithmType.Lbry:
                    return "lbc";
                case AlgorithmType.Pascal:
                    return "pasc";
            }
            return "";
        }

        protected override string SecondaryLookForStart() {
            return (SecondaryShortName() + " - Total Speed:").ToLower();
        }

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 90 * 1000; // 1.5 minute max, whole waiting time 75seconds
        }

        private string GetStartCommand(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);

            string dualModeParams = "";
            if (!IsDual())
            {  // leave convenience param for non-dual entry
                foreach (var pair in MiningSetup.MiningPairs)
                {
                    if (pair.CurrentExtraLaunchParameters.Contains("-dual="))
                    {
                        AlgorithmType dual = AlgorithmType.NONE;
                        string coinP = "";
                        if (pair.CurrentExtraLaunchParameters.Contains("Decred")) {
                            dual = AlgorithmType.Decred;
                            coinP = " -dcoin dcr ";
                        }
                        if (pair.CurrentExtraLaunchParameters.Contains("Siacoin")) {
                            dual = AlgorithmType.Sia;
                        }
                        if (pair.CurrentExtraLaunchParameters.Contains("Lbry"))  {
                            dual = AlgorithmType.Lbry;
                            coinP = " -dcoin lbc ";
                        }
                        if (pair.CurrentExtraLaunchParameters.Contains("Pascal")) {
                            dual = AlgorithmType.Pascal;
                            coinP = " -dcoin pasc ";
                        }
                        if (dual != AlgorithmType.NONE)  {
                            string urlSecond = Globals.GetLocationURL(dual, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
                            dualModeParams = String.Format(" {0} -dpool {1} -dwal {2}", coinP, urlSecond, username);
                            break;
                        }
                    }
                }
            } else {
                string urlSecond = Globals.GetLocationURL(SecondaryAlgorithmType, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
                dualModeParams = String.Format(" -dcoin {0} -dpool {1} -dwal {2}", SecondaryShortName(), urlSecond, username);
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


            // network stub
            string url = Globals.GetLocationURL(algorithm.NiceHashID, Globals.MiningLocation[ConfigManager.GeneralConfig.ServiceLocation], this.ConectionType);
            // demo for benchmark
            string ret = GetStartCommand(url, Globals.DemoUser, ConfigManager.GeneralConfig.WorkerName.Trim());
            // local benhcmark
            if (!IsDual()) {
                benchmarkTimeWait = time;
                return ret + "  -benchmark 1";
            } else {
                benchmarkTimeWait = Math.Max(120, time);  // dual seems to stop mining after this time if redirect output is true
                return ret;  // benchmark 1 does not output secondary speeds
            }
        }

    }
}
