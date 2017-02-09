using Newtonsoft.Json;
using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using NiceHashMiner.Miners.Grouping;
using System.Diagnostics;
using NiceHashMiner.Miners.Parsing;

namespace NiceHashMiner.Miners {
    public class nheqminer : nheqBase {
        public nheqminer()
            : base("nheqminer") {
                ConectionType = NHMConectionType.NONE;
        }

        // CPU aff set from NHM
        protected override NiceHashProcess _Start() {
            NiceHashProcess P = base._Start();
            if (CPU_Setup.IsInit && P != null) {
                var AffinityMask = CPU_Setup.MiningPairs[0].Device.AffinityMask;
                if (AffinityMask != 0) {
                    CPUID.AdjustAffinity(P.Id, AffinityMask);
                }
            }

            return P;
        }

        public override void Start(string url, string btcAdress, string worker) {
            string username = GetUsername(btcAdress, worker);
            LastCommandLine = GetDevicesCommandString() + " -a " + APIPort + " -l " + url + " -u " + username;
            ProcessHandle = _Start();
        }


        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            if (CPU_Setup.IsInit) {
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForMiningSetup(CPU_Setup, DeviceType.CPU);
            } else {
                // disable CPU
                deviceStringCommand += " -t 0 ";
            }

            if (NVIDIA_Setup.IsInit) {
                deviceStringCommand += " -cd ";
                foreach (var nvidia_pair in NVIDIA_Setup.MiningPairs) {
                    deviceStringCommand += nvidia_pair.Device.ID + " ";
                }
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForMiningSetup(NVIDIA_Setup, DeviceType.NVIDIA);
            }

            if (AMD_Setup.IsInit) {
                deviceStringCommand += " -op " + AMD_OCL_PLATFORM.ToString();
                deviceStringCommand += " -od ";
                foreach (var amd_pair in AMD_Setup.MiningPairs) {
                    deviceStringCommand += amd_pair.Device.ID + " ";
                }
                deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForMiningSetup(AMD_Setup, DeviceType.AMD);
            }

            return deviceStringCommand;
        }

        // benchmark stuff
        protected override Process BenchmarkStartProcess(string CommandLine) {
            Process BenchmarkHandle = base.BenchmarkStartProcess(CommandLine);

            if (CPU_Setup.IsInit && BenchmarkHandle != null) {
                var AffinityMask = CPU_Setup.MiningPairs[0].Device.AffinityMask;
                if (AffinityMask != 0) {
                    CPUID.AdjustAffinity(BenchmarkHandle.Id, AffinityMask);
                }
            }

            return BenchmarkHandle;
        }

        protected override bool BenchmarkParseLine(string outdata) {

            if (outdata.Contains(Iter_PER_SEC)) {
                curSpeed = getNumber(outdata, "Speed: ", Iter_PER_SEC) * SolMultFactor;
            }
            if (outdata.Contains(Sols_PER_SEC)) {
                var sols = getNumber(outdata, "Speed: ", Sols_PER_SEC);
                if (sols > 0) {
                    BenchmarkAlgorithm.BenchmarkSpeed = curSpeed;
                    return true;
                }
            }
            return false;
        }
        
    }
}
