using NiceHashMiner.Devices;
using NiceHashMiner.Enums;
using NiceHashMiner.Miners.Equihash;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners {
    public class MinerFactory {

        private static Miner CreateEthminer(DeviceType deviceType) {
            if (DeviceType.AMD == deviceType) {
                return new MinerEtherumOCL(/*minersConfig*/);
            } else if(DeviceType.NVIDIA == deviceType) {
                return new MinerEtherumCUDA(/*minersConfig*/);
            }
            return null;
        }

        private static Miner CreateClaymore(AlgorithmType algorithmType) {
            if (AlgorithmType.Equihash == algorithmType) {
                return new ClaymoreZcashMiner(/*minersConfig*/);
            } else if (AlgorithmType.CryptoNight == algorithmType) {
                return new ClaymoreCryptoNightMiner(/*minersConfig*/);
            }
            return null;
        }

        public static Miner CreateMiner(DeviceType deviceType, AlgorithmType algorithmType, MinerBaseType minerBaseType /*, /*minersConfig: minersConfig*/) {
            switch (minerBaseType) {
                case MinerBaseType.cpuminer:
                    return new cpuminer(/*minersConfig*/);
                case MinerBaseType.ccminer:
                    return new ccminer(/*minersConfig*/);
                case MinerBaseType.sgminer:
                    return new sgminer(/*minersConfig*/);
                case MinerBaseType.nheqminer:
                    return new nheqminer(/*minersConfig*/);
                case MinerBaseType.eqm:
                    return new eqm(/*minersConfig*/);
                case MinerBaseType.ethminer:
                    return CreateEthminer(deviceType);
                case MinerBaseType.ClaymoreAMD:
                    return CreateClaymore(algorithmType);
                case MinerBaseType.OptiminerAMD:
                    return new OptiminerZcashMiner(/*minersConfig*/);
                case MinerBaseType.excavator:
                    return new excavator();
            }
            return null;
        }

        // create miner creates new miners based on device type and algorithm/miner path
        public static Miner CreateMiner(ComputeDevice device, Algorithm algorithm) {
            if (device != null && algorithm != null) {
                return CreateMiner(device.DeviceType, algorithm.NiceHashID, algorithm.MinerBaseType);
            }
            return null;
        }
    }
}
