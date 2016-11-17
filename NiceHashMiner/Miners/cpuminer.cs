using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;

namespace NiceHashMiner.Miners {
    public class cpuminer : Miner {
        //private int Threads;
        //private ulong AffinityMask;

        public cpuminer()
            : base(DeviceType.CPU, DeviceGroupType.CPU, "CPU") {
        }        

        protected override int GET_MAX_CooldownTimeInMilliseconds() {
            return 60 * 1000; // 1 minute max, whole waiting time 75seconds
        }

        protected override void InitSupportedMinerAlgorithms() {
            var allGroupSupportedList = GroupAlgorithms.GetAlgorithmKeysForGroup(DeviceGroupType.CPU);
            _supportedMinerAlgorithms = allGroupSupportedList.ToArray();
        }

        public override void Start(Algorithm miningAlgorithm, string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            CurrentMiningAlgorithm = miningAlgorithm;
            if (ProcessHandle != null) return; // ignore, already running

            if (CDevs.Count == 0 || !CDevs[0].Enabled) return;

            if (miningAlgorithm == null) return;

            Path = GetOptimizedMinerPath(miningAlgorithm.NiceHashID);

            LastCommandLine = "--algo=" + miningAlgorithm.MinerName +
                              " --url=" + url +
                              " --userpass=" + username + ":" + Algorithm.PasswordDefault +
                              ExtraLaunchParametersParser.ParseForCDevs(
                                                                CDevs,
                                                                CurrentMiningAlgorithm.NiceHashID,
                                                                DeviceType.CPU) +
                              " --api-bind=" + APIPort.ToString();

            ProcessHandle = _Start();
        }

        public override APIData GetSummary() {
            return GetSummaryCPU_CCMINER();
        }

        protected override void _Stop(MinerStopType willswitch) {
            Stop_cpu_ccminer_sgminer_nheqminer(willswitch);
        }

        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();

            if (CDevs[0].AffinityMask != 0 && P != null)
                CPUID.AdjustAffinity(P.Id, CDevs[0].AffinityMask);

            return P;
        }

        protected override bool UpdateBindPortCommand(int oldPort, int newPort) {
            return UpdateBindPortCommand_ccminer_cpuminer(oldPort, newPort);
        }

        // new decoupled benchmarking routines
        #region Decoupled benchmarking routines

        protected override string BenchmarkCreateCommandLine(Algorithm algorithm, int time) {
            Path = GetOptimizedMinerPath(algorithm.NiceHashID);

            return "--algo=" + algorithm.MinerName +
                         " --benchmark" +
                         ExtraLaunchParametersParser.ParseForCDevs(
                                                                CDevs,
                                                                algorithm.NiceHashID,
                                                                DeviceType.CPU) +
                         " --time-limit " + time.ToString();
        }

        protected override Process BenchmarkStartProcess(string CommandLine) {
            Process BenchmarkHandle = base.BenchmarkStartProcess(CommandLine);

            if (CDevs[0].AffinityMask != 0 && BenchmarkHandle != null)
                CPUID.AdjustAffinity(BenchmarkHandle.Id, CDevs[0].AffinityMask);

            return BenchmarkHandle;
        }

        protected override bool BenchmarkParseLine(string outdata) {
            double lastSpeed = 0;
            if (double.TryParse(outdata, out lastSpeed)) {
                BenchmarkAlgorithm.BenchmarkSpeed = lastSpeed;
                return true;
            }
            return false;
        }

        protected override void BenchmarkOutputErrorDataReceivedImpl(string outdata) {
            CheckOutdata(outdata);
        }

        #endregion // Decoupled benchmarking routines
    }
}
