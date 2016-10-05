using NiceHashMiner.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Devices {

    /// <summary>
    /// GroupAlgorithms creates defaults supported algorithms. Currently based in Miner implementation
    /// </summary>
    public static class GroupAlgorithms {

        public static Dictionary<AlgorithmType, Algorithm> CreateDefaultsForGroup(DeviceGroupType deviceGroupType) {
            if (DeviceGroupType.CPU == deviceGroupType) {
                return new Dictionary<AlgorithmType, Algorithm>() {
                { AlgorithmType.Lyra2RE, new Algorithm(AlgorithmType.Lyra2RE, "lyra2") },
                { AlgorithmType.Hodl, new Algorithm(AlgorithmType.Hodl, "hodl") },
                { AlgorithmType.CryptoNight, new Algorithm(AlgorithmType.CryptoNight, "cryptonight") }
                };
            }
            if (DeviceGroupType.AMD_OpenCL == deviceGroupType) {
                // DisableAMDTempControl = false; so temperature params are enabled by default, not sure if TemperatureParam should be lastly appended
                string DefaultParam = AmdGpuDevice.DefaultParam;
                return new Dictionary<AlgorithmType, Algorithm>() { 
                { AlgorithmType.X13 , new Algorithm(AlgorithmType.X13,  "x13")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize  64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Keccak , new Algorithm(AlgorithmType.Keccak,  "keccak")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity  300 --thread-concurrency    0 --worksize  64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.X15 , new Algorithm(AlgorithmType.X15,  "x15")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize  64 --gpu-threads 2"  + AmdGpuDevice.TemperatureParam} },
                { AlgorithmType.Nist5 , new Algorithm(AlgorithmType.Nist5,  "nist5")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity   16 --thread-concurrency    0 --worksize  64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.NeoScrypt , new Algorithm(AlgorithmType.NeoScrypt, "neoscrypt")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity    2 --thread-concurrency 8192 --worksize  64 --gpu-threads 4" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.WhirlpoolX , new Algorithm(AlgorithmType.WhirlpoolX, "whirlpoolx")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity   64 --thread-concurrency    0 --worksize 128 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Qubit , new Algorithm(AlgorithmType.Qubit,  "qubitcoin")
                    { ExtraLaunchParameters = DefaultParam + "--intensity 18 --worksize 64 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Quark , new Algorithm(AlgorithmType.Quark,  "quarkcoin")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity 1024 --thread-concurrency    0 --worksize  64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Lyra2REv2 , new Algorithm(AlgorithmType.Lyra2REv2,  "Lyra2REv2")
                    { ExtraLaunchParameters = DefaultParam + "--nfactor 10 --xintensity  160 --thread-concurrency    0 --worksize  64 --gpu-threads 1" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Blake256r8 , new Algorithm(AlgorithmType.Blake256r8, "blakecoin")
                    { ExtraLaunchParameters = DefaultParam + "--intensity  24 --worksize 128 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Blake256r14 , new Algorithm(AlgorithmType.Blake256r14, "blake")
                    { ExtraLaunchParameters = DefaultParam + "--intensity  24 --worksize 128 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Blake256r8vnl , new Algorithm(AlgorithmType.Blake256r8vnl, "vanilla")
                    { ExtraLaunchParameters = DefaultParam + "--intensity  24 --worksize 128 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.DaggerHashimoto , new Algorithm(AlgorithmType.DaggerHashimoto, "daggerhashimoto") },
                { AlgorithmType.Decred , new Algorithm(AlgorithmType.Decred, "decred")
                    { ExtraLaunchParameters = "--gpu-threads 1 --remove-disabled --xintensity 256 --lookup-gap 2 --worksize 64" + AmdGpuDevice.TemperatureParam } },
                { AlgorithmType.Lbry, new Algorithm(AlgorithmType.Lbry, "lbry") 
                    { ExtraLaunchParameters = DefaultParam + "--xintensity 512 --worksize 128 --gpu-threads 2" + AmdGpuDevice.TemperatureParam } },
                };
            }
            // NVIDIA
            if (DeviceGroupType.NVIDIA_2_1 == deviceGroupType || DeviceGroupType.NVIDIA_3_x == deviceGroupType || DeviceGroupType.NVIDIA_5_x == deviceGroupType || DeviceGroupType.NVIDIA_6_x == deviceGroupType) {
                var ret = new Dictionary<AlgorithmType, Algorithm> {
                { AlgorithmType.X13 , new Algorithm(AlgorithmType.X13, "x13") },
                { AlgorithmType.Keccak , new Algorithm(AlgorithmType.Keccak, "keccak") },
                { AlgorithmType.X15 , new Algorithm(AlgorithmType.X15, "x15") },
                { AlgorithmType.Nist5 , new Algorithm(AlgorithmType.Nist5, "nist5") },
                { AlgorithmType.NeoScrypt , new Algorithm(AlgorithmType.NeoScrypt, "neoscrypt") },
                { AlgorithmType.WhirlpoolX , new Algorithm(AlgorithmType.WhirlpoolX, "whirlpoolx") },
                { AlgorithmType.Qubit , new Algorithm(AlgorithmType.Qubit, "qubit") },
                { AlgorithmType.Quark , new Algorithm(AlgorithmType.Quark, "quark") },
                { AlgorithmType.Lyra2RE , new Algorithm(AlgorithmType.Lyra2RE, "lyra2") },
                { AlgorithmType.Lyra2REv2 , new Algorithm(AlgorithmType.Lyra2REv2, "lyra2v2") },
                { AlgorithmType.Blake256r8 , new Algorithm(AlgorithmType.Blake256r8, "blakecoin") },
                { AlgorithmType.Blake256r14 , new Algorithm(AlgorithmType.Blake256r14, "blake") },
                { AlgorithmType.Blake256r8vnl , new Algorithm(AlgorithmType.Blake256r8vnl, "vanilla") },
                { AlgorithmType.DaggerHashimoto , new Algorithm(AlgorithmType.DaggerHashimoto, "daggerhashimoto") },
                { AlgorithmType.Decred , new Algorithm(AlgorithmType.Decred, "decred") },
                { AlgorithmType.CryptoNight, new Algorithm(AlgorithmType.CryptoNight, "cryptonight") },
                { AlgorithmType.Lbry, new Algorithm(AlgorithmType.Lbry, "lbry") },
                };
                if(DeviceGroupType.NVIDIA_2_1 == deviceGroupType) {
                    // minerName change => "whirlpoolx" => "whirlpool"
                    ret[AlgorithmType.WhirlpoolX] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpool");     // Needed for new tpruvot's ccminer
                    // disable/remove neoscrypt, daggerhashimoto
                    ret.Remove(AlgorithmType.NeoScrypt);
                    ret.Remove(AlgorithmType.DaggerHashimoto);

                    ret.Remove(AlgorithmType.Lyra2RE);
                    ret.Remove(AlgorithmType.Lyra2REv2);
                    //ret.Remove(AlgorithmType.CryptoNight);
                }
                if (DeviceGroupType.NVIDIA_3_x == deviceGroupType) {
                    // minerName change => "whirlpoolx" => "whirlpool"
                    ret[AlgorithmType.WhirlpoolX] = new Algorithm(AlgorithmType.WhirlpoolX, "whirlpool");     // Needed for new tpruvot's ccminer
                    // disable/remove neoscrypt
                    ret.Remove(AlgorithmType.NeoScrypt);
                    ret.Remove(AlgorithmType.Lyra2RE);
                    ret.Remove(AlgorithmType.Lyra2REv2);
                    //ret.Remove(AlgorithmType.CryptoNight);
                }
                return ret;
            }

            return null;
        }


        public static List<AlgorithmType> GetAlgorithmKeysForGroup(DeviceGroupType deviceGroupType) {
            var ret = CreateDefaultsForGroup(deviceGroupType);
            if (ret != null) {
                return new List <AlgorithmType>(ret.Keys);
            }
            return null;
        }

    }
}
