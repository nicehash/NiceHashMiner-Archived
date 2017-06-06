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
                return new MinerEtherumOCL();
            } else if(DeviceType.NVIDIA == deviceType) {
                return new MinerEtherumCUDA();
            }
            return null;
        }

        private static Miner CreateClaymore(AlgorithmType algorithmType, AlgorithmType secondaryAlgorithmType) {
            if (AlgorithmType.Equihash == algorithmType) {
                return new ClaymoreZcashMiner();
            } else if (AlgorithmType.CryptoNight == algorithmType) {
                return new ClaymoreCryptoNightMiner();
            } else if (AlgorithmType.DaggerHashimoto == algorithmType) {
                return new ClaymoreDual(secondaryAlgorithmType);
            }
            return null;
        }

        private static Miner CreateExperimental(DeviceType deviceType, AlgorithmType algorithmType) {
            if (AlgorithmType.NeoScrypt == algorithmType && DeviceType.NVIDIA == deviceType) {
                return new ccminer();
            }
            return null;
        }

        public static Miner CreateMiner(DeviceType deviceType, AlgorithmType algorithmType, MinerBaseType minerBaseType, AlgorithmType secondaryAlgorithmType=AlgorithmType.NONE) {
            switch (minerBaseType) {
                case MinerBaseType.ccminer:
                    return new ccminer();
                case MinerBaseType.sgminer:
                    return new sgminer();
                case MinerBaseType.nheqminer:
                    return new nheqminer();
                case MinerBaseType.ethminer:
                    return CreateEthminer(deviceType);
                case MinerBaseType.ClaymoreAMD:
                    return CreateClaymore(algorithmType, secondaryAlgorithmType);
                case MinerBaseType.OptiminerAMD:
                    return new OptiminerZcashMiner();
                case MinerBaseType.excavator:
                    return new excavator();
                case MinerBaseType.XmrStackCPU:
                    return new XmrStackCPUMiner();
                case MinerBaseType.ccminer_alexis:
                    return new ccminer();
                case MinerBaseType.experimental:
                    return CreateExperimental(deviceType, algorithmType);
            }
            return null;
        }

        // create miner creates new miners based on device type and algorithm/miner path
        public static Miner CreateMiner(ComputeDevice device, Algorithm algorithm) {
            if (device != null && algorithm != null) {
                return CreateMiner(device.DeviceType, algorithm.NiceHashID, algorithm.MinerBaseType, algorithm.SecondaryNiceHashID);
            }
            return null;
        }
    }
}
