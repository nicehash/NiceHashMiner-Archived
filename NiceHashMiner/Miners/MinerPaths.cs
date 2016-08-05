using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Miners
{
    /// <summary>
    /// MinerPaths, used just to store miners paths strings. Only one instance needed
    /// </summary>
    public static class MinerPaths
    {
        // root binary folder
        private const string _bin = @"bin";
        /// <summary>
        /// ccminers
        /// </summary>
        public const string ccminer_decred =            _bin + @"\ccminer_decred.exe";
        public const string ccminer_nanashi =           _bin + @"\ccminer_nanashi.exe";
        //public const string ccminer_nanashi_lyra2rev2 = _bin + @"\ccminer_nanashi_lyra2rev2.exe";
        public const string ccminer_neoscrypt =         _bin + @"\ccminer_neoscrypt.exe";
        public const string ccminer_sp =                _bin + @"\ccminer_sp.exe";
        public const string ccminer_tpruvot =           _bin + @"\ccminer_tpruvot.exe";
        /// <summary>
        /// cpuminers
        /// </summary>
        public const string cpuminer_x64_AVX =  _bin + @"\cpuminer_x64_AVX.exe";
        public const string cpuminer_x64_AVX2 = _bin + @"\cpuminer_x64_AVX2.exe";
        public const string cpuminer_x64_SSE2 = _bin + @"\cpuminer_x64_SSE2.exe";
        /// <summary>
        /// ethminers
        /// </summary>
        public const string ethminer = _bin + @"\ethminer.exe";
        /// <summary>
        /// hodlminer also CPU
        /// </summary>
        public const string hodlminer_core_avx2 =   _bin + @"\hodlminer\hodlminer_core_avx2.exe";
        public const string hodlminer_core2 =       _bin + @"\hodlminer\hodlminer_core2.exe";
        public const string hodlminer_corei7_avx =  _bin + @"\hodlminer\hodlminer_corei7_avx.exe";
        /// <summary>
        /// sgminers
        /// </summary>
        public const string sgminer_5_1_0_optimized =   _bin + @"\sgminer-5-1-0-optimized\sgminer.exe";
        public const string sgminer_5_1_1_optimized =   _bin + @"\sgminer-5-1-1-optimized\sgminer.exe";
        public const string sgminer_5_4_0_general =     _bin + @"\sgminer-5-4-0-general\sgminer.exe";
        public const string sgminer_5_4_0_tweaked =     _bin + @"\sgminer-5-4-0-tweaked\sgminer.exe";

    }
}
