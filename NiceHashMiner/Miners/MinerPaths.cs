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
        public const string ccminer_neoscrypt =         _bin + @"\ccminer_neoscrypt.exe";
        public const string ccminer_sp =                _bin + @"\ccminer_sp.exe";
        public const string ccminer_tpruvot =           _bin + @"\ccminer_tpruvot.exe";
        public const string ccminer_cryptonight =       _bin + @"\ccminer_cryptonight.exe";
        /// <summary>
        /// cpuminers opt new
        /// </summary>
        public const string cpuminer_opt_AVX2_AES =     _bin + @"\cpuminer_opt_AVX2_AES.exe";    
        public const string cpuminer_opt_AVX2 =         _bin + @"\cpuminer_opt_AVX2.exe";
        public const string cpuminer_opt_AVX_AES =      _bin + @"\cpuminer_opt_AVX_AES.exe";    
        public const string cpuminer_opt_AVX =          _bin + @"\cpuminer_opt_AVX.exe";
        public const string cpuminer_opt_AES =          _bin + @"\cpuminer_opt_AES.exe";
        public const string cpuminer_opt_SSE2 =         _bin + @"\cpuminer_opt_SSE2.exe";
        /// <summary>
        /// ethminers
        /// </summary>
        public const string ethminer = _bin + @"\ethminer.exe";
                
        /// <summary>
        /// sgminers
        /// </summary>
        public const string sgminer_5_1_0_optimized =   _bin + @"\sgminer-5-1-0-optimized\sgminer.exe";
        public const string sgminer_5_1_1_optimized =   _bin + @"\sgminer-5-1-1-optimized\sgminer.exe";
        public const string sgminer_5_5_0_general =     _bin + @"\sgminer-5-5-0-general\sgminer.exe";
        public const string sgminer_5_4_0_tweaked =     _bin + @"\sgminer-5-4-0-tweaked\sgminer.exe";

    }
}
