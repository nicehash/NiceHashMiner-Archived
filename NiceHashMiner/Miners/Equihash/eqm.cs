using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner.Miners {
    public class eqm : nheqBase {
        public eqm()
            : base(DeviceType.NVIDIA_CPU, "eqm") {
            Path = MinerPaths.eqm;
            WorkingDirectory = MinerPaths.eqm.Replace("eqm.exe", "");
            IsNHLocked = true;
        }


        // TODO fix extra paramteter parsing
        protected override string GetDevicesCommandString() {
            string deviceStringCommand = " ";

            if (CPUs.Count > 0) {
                deviceStringCommand += "-p " + CPUs.Count;
                if (CPUs[0].MostProfitableAlgorithm.LessThreads > 0 || !string.IsNullOrEmpty(CPUs[0].MostProfitableAlgorithm.ExtraLaunchParameters)) {
                    deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(CPUs, AlgorithmType.Equihash, DeviceType.CPU, Path);
                }
            } else {
                // disable CPU
                deviceStringCommand += " -t 0 ";
            }

            if (NVIDIAs.Count > 0) {
                deviceStringCommand += " -cd ";
                foreach (var nvidia in NVIDIAs) {
                    for (int i = 0; i < ExtraLaunchParametersParser.GetEqmThreadCount(nvidia); ++i) {
                        deviceStringCommand += nvidia.ID + " ";
                    }
                }
                // no extra launch params
                //deviceStringCommand += " " + ExtraLaunchParametersParser.ParseForCDevs(NVIDIAs, AlgorithmType.Equihash, DeviceType.NVIDIA, Path);
            }

            return deviceStringCommand;
        }

        // benchmark stuff
        protected override bool BenchmarkParseLine(string outdata) {

            if (outdata.Contains(Iter_PER_SEC)) {
                curSpeed = getNumber(outdata, "Measured: ", Iter_PER_SEC) * SolMultFactor;
            }
            if (outdata.Contains(Sols_PER_SEC)) {
                var sols = getNumber(outdata, "Measured: ", Sols_PER_SEC);
                if (sols > 0) {
                    BenchmarkAlgorithm.BenchmarkSpeed = curSpeed;
                    return true;
                }
            }
            return false;
        }
    }
}
