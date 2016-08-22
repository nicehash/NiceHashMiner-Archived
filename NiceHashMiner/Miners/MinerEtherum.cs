using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {

    /// <summary>
    /// For now used only for daggerhashimoto
    /// </summary>
    public abstract class MinerEtherum : Miner {
        
        protected ethminerAPI ethminerLink;

        //ComputeDevice
        public ComputeDevice DaggerHashimotoGenerateDevice;

        public MinerEtherum() : base() {
            Path = Ethereum.EtherMinerPath;
            _isEthMinerExit = true;
        }

        protected override void InitSupportedMinerAlgorithms() {
            _supportedMinerAlgorithms = new AlgorithmType[] { AlgorithmType.DaggerHashimoto };
        }

        protected abstract string GetStartCommandStringPart(Algorithm miningAlgorithm, string url, string username);
        protected abstract string GetBenchmarkCommandStringPart(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm);

        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            List<string> ids = new List<string>();
            foreach (var cdev in CDevs) {
                ids.Add(cdev.ID.ToString());
            }
            deviceStringCommand += string.Join(" ", ids);
            deviceStringCommand += " --dag-load-mode singlekeep " + DaggerHashimotoGenerateDevice.ID.ToString();
            return deviceStringCommand;
        }

        public override void Start(Algorithm miningAlgorithm, string url, string username) {
            //if (ProcessHandle != null) return; // ignore, already running 

            CurrentMiningAlgorithm = miningAlgorithm;
            if (miningAlgorithm == null && miningAlgorithm.NiceHashID != AlgorithmType.DaggerHashimoto) {
                Helpers.ConsolePrint("MinerEtherum", "Algorithm is null or not DaggerHashimoto");
                return;
            }

            LastCommandLine = GetStartCommandStringPart(miningAlgorithm, url, username) + GetDevicesCommandString();

            ProcessHandle = _Start();
        }

        protected override string BenchmarkCreateCommandLine(DeviceBenchmarkConfig benchmarkConfig, Algorithm algorithm, int time) {
            string CommandLine = GetBenchmarkCommandStringPart(benchmarkConfig, algorithm) + GetDevicesCommandString();
            // TODO fix miner block
            Ethereum.GetCurrentBlock(MinerDeviceName);
            CommandLine += " --benchmark " + Ethereum.CurrentBlockNum;

            return CommandLine;
        }

        public override void SetCDevs(string[] deviceUUIDs) {
            base.SetCDevs(deviceUUIDs);
            // now find the fastest for DAG generation
            double fastestSpeed = double.MinValue;
            foreach (var cdev in CDevs) {
                double compareSpeed = DeviceBenchmarkConfigManager.Instance
                    .GetConfig(cdev.Name).AlgorithmSettings[AlgorithmType.DaggerHashimoto].BenchmarkSpeed;
                if (fastestSpeed < compareSpeed) {
                    DaggerHashimotoGenerateDevice = cdev;
                    fastestSpeed = compareSpeed;
                }
            }
        }

        public override APIData GetSummary() {
            APIData ad = new APIData();

            FillAlgorithm("daggerhashimoto", ref ad);

            bool ismining;
            if (!ethminerLink.GetSpeed(out ismining, out ad.Speed)) {
                if (NumRetries > 0) {
                    NumRetries--;
                    ad.Speed = 0;
                    return ad;
                }

                Helpers.ConsolePrint(MinerDeviceName, "ethminer is not running.. restarting..");
                Stop(false);
                _Start();
                ad.Speed = 0;
                return ad;
            } else if (!ismining) {
                // resend start mining command
                ethminerLink.StartMining();
            }
            ad.Speed *= 1000 * 1000;
            return ad;
        }

        protected override NiceHashProcess _Start() {
            // check if dagger already running
            if (AlgorithmType.DaggerHashimoto == CurrentAlgorithmType && ProcessHandle != null) {
                Helpers.ConsolePrint(MinerDeviceName, "Resuming ethminer..");
                ethminerLink.StartMining();
                IsRunning = true;
                return null;
            }
            ethminerLink = new ethminerAPI(APIPort);
            var P = base._Start();
            ProcessHandle = P;
            return P;
        }

        protected override void _Stop(bool willswitch) {
            if (willswitch) {
                // daggerhashimoto - we only "pause" mining
                Helpers.ConsolePrint(MinerDeviceName, "Pausing ethminer..");
                ethminerLink.StopMining();
                return;
            }

            Helpers.ConsolePrint(MinerDeviceName, "Shutting down miner");

            if (!willswitch && ProcessHandle != null) {
                try { ProcessHandle.Kill(); } catch { }
                ProcessHandle.Close();
                ProcessHandle = null;
            }
        }

        public override string GetOptimizedMinerPath(AlgorithmType algorithmType, string devCodename, bool isOptimized) {
            return Ethereum.EtherMinerPath;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            if (outdata.Contains("min/mean/max:")) {
                string[] splt = outdata.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                int index = Array.IndexOf(splt, "mean");
                double avg_spd = Convert.ToDouble(splt[index + 2]);
                Helpers.ConsolePrint("BENCHMARK", "Final Speed: " + avg_spd + "H/s");

                BenchmarkAlgorithm.BenchmarkSpeed = avg_spd;
                return true;
            }

            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

    }
}
