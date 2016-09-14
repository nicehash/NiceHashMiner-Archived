
namespace NiceHashMiner.Utils {
    public partial class MinersDownloadManager : BaseLazySingleton<MinersDownloadManager> {
    #region CODE_GEN STUFF // listFiles.py
private static string[] ALL_FILES_BINS = {
@"\ccminer_cryptonight.exe",
@"\ccminer_decred.exe",
@"\ccminer_nanashi.exe",
@"\ccminer_neoscrypt.exe",
@"\ccminer_sp.exe",
@"\ccminer_tpruvot.exe",
@"\cpuminer_opt_AES.exe",
@"\cpuminer_opt_AVX.exe",
@"\cpuminer_opt_AVX2.exe",
@"\cpuminer_opt_SSE2.exe",
@"\cudart32_80.dll",
@"\cudart64_80.dll",
@"\ethminer.exe",
@"\libcrypto-1.0.0.dll",
@"\libcurl-4.dll",
@"\libcurl.dll",
@"\libgcc_s_seh-1.dll",
@"\libgmp-10.dll",
@"\libgmpxx-4.dll",
@"\libjansson-4.dll",
@"\libjson-c-2.dll",
@"\libmicrohttpd-dll.dll",
@"\libsigc-2.0-0.dll",
@"\libssl-1.0.0.dll",
@"\libstdc++-6.dll",
@"\libwinpthread-1.dll",
@"\libz-1.dll",
@"\msvcr120.dll",
@"\OpenCL.dll",
@"\zlib1.dll",
@"\sgminer-5-1-0-optimized\darkcoin-modHawaiigw64l4.bin",
@"\sgminer-5-1-0-optimized\darkcoin-modPitcairngw64l4.bin",
@"\sgminer-5-1-0-optimized\darkcoin-modPitcairngw64l4ku0.bin",
@"\sgminer-5-1-0-optimized\darkcoin-modTahitigw64l4.bin",
@"\sgminer-5-1-0-optimized\darkcoin-modTongagw64l4.bin",
@"\sgminer-5-1-0-optimized\libcurl-4.dll",
@"\sgminer-5-1-0-optimized\libcurl.dll",
@"\sgminer-5-1-0-optimized\libeay32.dll",
@"\sgminer-5-1-0-optimized\libgcc_s_dw2-1.dll",
@"\sgminer-5-1-0-optimized\libidn-11.dll",
@"\sgminer-5-1-0-optimized\libpdcurses.dll",
@"\sgminer-5-1-0-optimized\Lyra2REv2Hawaiigw64l4.bin",
@"\sgminer-5-1-0-optimized\Lyra2REv2Pitcairngw64l4.bin",
@"\sgminer-5-1-0-optimized\Lyra2REv2Tahitigw64l4.bin",
@"\sgminer-5-1-0-optimized\Lyra2REv2Tongagw64l4.bin",
@"\sgminer-5-1-0-optimized\pthreadGC2.dll",
@"\sgminer-5-1-0-optimized\quarkcoinHawaiigw256l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinHawaiigw64l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinPitcairngw256l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinPitcairngw64l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinTahitigw256l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinTahitigw64l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinTongagw256l4ku0.bin",
@"\sgminer-5-1-0-optimized\quarkcoinTongagw64l4ku0.bin",
@"\sgminer-5-1-0-optimized\runme.bat",
@"\sgminer-5-1-0-optimized\sgminer-fixed.conf",
@"\sgminer-5-1-0-optimized\sgminer.exe",
@"\sgminer-5-1-0-optimized\ssleay32.dll",
@"\sgminer-5-1-0-optimized\zlib1.dll",
@"\sgminer-5-1-0-optimized\kernel\aes_helper.cl",
@"\sgminer-5-1-0-optimized\kernel\alexkarnew.cl",
@"\sgminer-5-1-0-optimized\kernel\alexkarold.cl",
@"\sgminer-5-1-0-optimized\kernel\animecoin.cl",
@"\sgminer-5-1-0-optimized\kernel\arebyp.cl",
@"\sgminer-5-1-0-optimized\kernel\bitblock.cl",
@"\sgminer-5-1-0-optimized\kernel\bitblockold.cl",
@"\sgminer-5-1-0-optimized\kernel\blake.cl",
@"\sgminer-5-1-0-optimized\kernel\blake256.cl",
@"\sgminer-5-1-0-optimized\kernel\bmw.cl",
@"\sgminer-5-1-0-optimized\kernel\bmw256.cl",
@"\sgminer-5-1-0-optimized\kernel\bufius.cl",
@"\sgminer-5-1-0-optimized\kernel\ckolivas.cl",
@"\sgminer-5-1-0-optimized\kernel\credits.cl",
@"\sgminer-5-1-0-optimized\kernel\cubehash.cl",
@"\sgminer-5-1-0-optimized\kernel\cubehash256.cl",
@"\sgminer-5-1-0-optimized\kernel\darkcoin-mod.cl",
@"\sgminer-5-1-0-optimized\kernel\darkcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\diamond.cl",
@"\sgminer-5-1-0-optimized\kernel\echo.cl",
@"\sgminer-5-1-0-optimized\kernel\fresh.cl",
@"\sgminer-5-1-0-optimized\kernel\fugue.cl",
@"\sgminer-5-1-0-optimized\kernel\fuguecoin.cl",
@"\sgminer-5-1-0-optimized\kernel\groestl.cl",
@"\sgminer-5-1-0-optimized\kernel\groestl256.cl",
@"\sgminer-5-1-0-optimized\kernel\groestlcoin-v1.cl",
@"\sgminer-5-1-0-optimized\kernel\groestlcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\hamsi.cl",
@"\sgminer-5-1-0-optimized\kernel\hamsi_helper.cl",
@"\sgminer-5-1-0-optimized\kernel\hamsi_helper_big.cl",
@"\sgminer-5-1-0-optimized\kernel\inkcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\jh.cl",
@"\sgminer-5-1-0-optimized\kernel\keccak.cl",
@"\sgminer-5-1-0-optimized\kernel\keccak1600.cl",
@"\sgminer-5-1-0-optimized\kernel\luffa.cl",
@"\sgminer-5-1-0-optimized\kernel\Lyra2.cl",
@"\sgminer-5-1-0-optimized\kernel\Lyra2RE.cl",
@"\sgminer-5-1-0-optimized\kernel\Lyra2REv2.cl",
@"\sgminer-5-1-0-optimized\kernel\Lyra2v2.cl",
@"\sgminer-5-1-0-optimized\kernel\marucoin-mod.cl",
@"\sgminer-5-1-0-optimized\kernel\marucoin-modold.cl",
@"\sgminer-5-1-0-optimized\kernel\marucoin.cl",
@"\sgminer-5-1-0-optimized\kernel\maxcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\myriadcoin-groestl.cl",
@"\sgminer-5-1-0-optimized\kernel\neoscrypt.cl",
@"\sgminer-5-1-0-optimized\kernel\panama.cl",
@"\sgminer-5-1-0-optimized\kernel\pluck.cl",
@"\sgminer-5-1-0-optimized\kernel\psw.cl",
@"\sgminer-5-1-0-optimized\kernel\quarkcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\qubitcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\shabal.cl",
@"\sgminer-5-1-0-optimized\kernel\shavite.cl",
@"\sgminer-5-1-0-optimized\kernel\sifcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\simd.cl",
@"\sgminer-5-1-0-optimized\kernel\skein.cl",
@"\sgminer-5-1-0-optimized\kernel\skein256.cl",
@"\sgminer-5-1-0-optimized\kernel\talkcoin-mod.cl",
@"\sgminer-5-1-0-optimized\kernel\twecoin.cl",
@"\sgminer-5-1-0-optimized\kernel\whirlcoin.cl",
@"\sgminer-5-1-0-optimized\kernel\whirlpool.cl",
@"\sgminer-5-1-0-optimized\kernel\x14.cl",
@"\sgminer-5-1-0-optimized\kernel\x14old.cl",
@"\sgminer-5-1-0-optimized\kernel\yescrypt-multi.cl",
@"\sgminer-5-1-0-optimized\kernel\yescrypt.cl",
@"\sgminer-5-1-0-optimized\kernel\yescrypt_essential.cl",
@"\sgminer-5-1-0-optimized\kernel\zuikkis.cl",
@"\sgminer-5-1-1-optimized\darkcoin-modHawaiigw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\darkcoin-modPitcairngw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\darkcoin-modTahitigw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\darkcoin-modTongagw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\libcurl.dll",
@"\sgminer-5-1-1-optimized\libeay32.dll",
@"\sgminer-5-1-1-optimized\libgcc_s_dw2-1.dll",
@"\sgminer-5-1-1-optimized\libidn-11.dll",
@"\sgminer-5-1-1-optimized\libpdcurses.dll",
@"\sgminer-5-1-1-optimized\pthreadGC2.dll",
@"\sgminer-5-1-1-optimized\quarkcoinHawaiigw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\quarkcoinPitcairngw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\quarkcoinTahitigw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\quarkcoinTongagw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\qubitcoinHawaiigw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\qubitcoinPitcairngw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\qubitcoinTahitigw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\qubitcoinTongagw64l4ku0.bin",
@"\sgminer-5-1-1-optimized\runme.bat",
@"\sgminer-5-1-1-optimized\sgminer-fixed.conf",
@"\sgminer-5-1-1-optimized\sgminer.exe",
@"\sgminer-5-1-1-optimized\ssleay32.dll",
@"\sgminer-5-1-1-optimized\zlib1.dll",
@"\sgminer-5-1-1-optimized\kernel\aes_helper.cl",
@"\sgminer-5-1-1-optimized\kernel\alexkarnew.cl",
@"\sgminer-5-1-1-optimized\kernel\alexkarold.cl",
@"\sgminer-5-1-1-optimized\kernel\animecoin.cl",
@"\sgminer-5-1-1-optimized\kernel\arebyp.cl",
@"\sgminer-5-1-1-optimized\kernel\bitblock.cl",
@"\sgminer-5-1-1-optimized\kernel\bitblockold.cl",
@"\sgminer-5-1-1-optimized\kernel\blake.cl",
@"\sgminer-5-1-1-optimized\kernel\blake256.cl",
@"\sgminer-5-1-1-optimized\kernel\bmw.cl",
@"\sgminer-5-1-1-optimized\kernel\bufius.cl",
@"\sgminer-5-1-1-optimized\kernel\ckolivas.cl",
@"\sgminer-5-1-1-optimized\kernel\cubehash.cl",
@"\sgminer-5-1-1-optimized\kernel\darkcoin-mod.cl",
@"\sgminer-5-1-1-optimized\kernel\darkcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\diamond.cl",
@"\sgminer-5-1-1-optimized\kernel\echo.cl",
@"\sgminer-5-1-1-optimized\kernel\fresh.cl",
@"\sgminer-5-1-1-optimized\kernel\fugue.cl",
@"\sgminer-5-1-1-optimized\kernel\fuguecoin.cl",
@"\sgminer-5-1-1-optimized\kernel\groestl.cl",
@"\sgminer-5-1-1-optimized\kernel\groestl256.cl",
@"\sgminer-5-1-1-optimized\kernel\groestlcoin-v1.cl",
@"\sgminer-5-1-1-optimized\kernel\groestlcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\hamsi.cl",
@"\sgminer-5-1-1-optimized\kernel\hamsi_helper.cl",
@"\sgminer-5-1-1-optimized\kernel\hamsi_helper_big.cl",
@"\sgminer-5-1-1-optimized\kernel\inkcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\jh.cl",
@"\sgminer-5-1-1-optimized\kernel\keccak.cl",
@"\sgminer-5-1-1-optimized\kernel\keccak1600.cl",
@"\sgminer-5-1-1-optimized\kernel\luffa.cl",
@"\sgminer-5-1-1-optimized\kernel\lyra2.cl",
@"\sgminer-5-1-1-optimized\kernel\lyra2re.cl",
@"\sgminer-5-1-1-optimized\kernel\marucoin-mod.cl",
@"\sgminer-5-1-1-optimized\kernel\marucoin-modold.cl",
@"\sgminer-5-1-1-optimized\kernel\marucoin.cl",
@"\sgminer-5-1-1-optimized\kernel\maxcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\myriadcoin-groestl.cl",
@"\sgminer-5-1-1-optimized\kernel\neoscrypt.cl",
@"\sgminer-5-1-1-optimized\kernel\panama.cl",
@"\sgminer-5-1-1-optimized\kernel\pluck.cl",
@"\sgminer-5-1-1-optimized\kernel\psw.cl",
@"\sgminer-5-1-1-optimized\kernel\quarkcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\qubitcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\shabal.cl",
@"\sgminer-5-1-1-optimized\kernel\shavite.cl",
@"\sgminer-5-1-1-optimized\kernel\sifcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\simd.cl",
@"\sgminer-5-1-1-optimized\kernel\skein.cl",
@"\sgminer-5-1-1-optimized\kernel\skein256.cl",
@"\sgminer-5-1-1-optimized\kernel\talkcoin-mod.cl",
@"\sgminer-5-1-1-optimized\kernel\twecoin.cl",
@"\sgminer-5-1-1-optimized\kernel\whirlcoin.cl",
@"\sgminer-5-1-1-optimized\kernel\whirlpool.cl",
@"\sgminer-5-1-1-optimized\kernel\whirlpoolx.cl",
@"\sgminer-5-1-1-optimized\kernel\x14.cl",
@"\sgminer-5-1-1-optimized\kernel\x14old.cl",
@"\sgminer-5-1-1-optimized\kernel\zuikkis.cl",
@"\sgminer-5-4-0-general\bitblockFijigw64l4ku0big4.bin",
@"\sgminer-5-4-0-general\bitblockHawaiigw64l4ku0big4.bin",
@"\sgminer-5-4-0-general\bitblockTongagw64l4ku0big4.bin",
@"\sgminer-5-4-0-general\blake256r8Fijigw128l4.bin",
@"\sgminer-5-4-0-general\blake256r8Hawaiigw128l4.bin",
@"\sgminer-5-4-0-general\blake256r8Tongagw128l4.bin",
@"\sgminer-5-4-0-general\decredFijigw64l4tc24512.bin",
@"\sgminer-5-4-0-general\decredHawaiigw64l4tc24512.bin",
@"\sgminer-5-4-0-general\decredTongagw64l4tc24512.bin",
@"\sgminer-5-4-0-general\marucoin-modFijigw64l4ku0big4.bin",
@"\sgminer-5-4-0-general\marucoin-modHawaiigw64l4ku0big4.bin",
@"\sgminer-5-4-0-general\marucoin-modTongagw64l4ku0big4.bin",
@"\sgminer-5-4-0-general\maxcoinFijigw64l4.bin",
@"\sgminer-5-4-0-general\maxcoinHawaiigw64l4.bin",
@"\sgminer-5-4-0-general\maxcoinTongagw64l4.bin",
@"\sgminer-5-4-0-general\neoscryptFijigw64l4lgtc8192.bin",
@"\sgminer-5-4-0-general\neoscryptHawaiigw64l4lgtc8192.bin",
@"\sgminer-5-4-0-general\neoscryptTongagw64l4lgtc8192.bin",
@"\sgminer-5-4-0-general\runme.bat",
@"\sgminer-5-4-0-general\sgminer-fixed.conf",
@"\sgminer-5-4-0-general\sgminer.exe",
@"\sgminer-5-4-0-general\talkcoin-modFijigw64l4ku0.bin",
@"\sgminer-5-4-0-general\talkcoin-modHawaiigw64l4ku0.bin",
@"\sgminer-5-4-0-general\talkcoin-modTongagw64l4ku0.bin",
@"\sgminer-5-4-0-general\vanillaFijigw128l4.bin",
@"\sgminer-5-4-0-general\vanillaHawaiigw128l4.bin",
@"\sgminer-5-4-0-general\vanillaTongagw128l4.bin",
@"\sgminer-5-4-0-general\kernel\aes_helper.cl",
@"\sgminer-5-4-0-general\kernel\alexkarnew.cl",
@"\sgminer-5-4-0-general\kernel\alexkarold.cl",
@"\sgminer-5-4-0-general\kernel\animecoin.cl",
@"\sgminer-5-4-0-general\kernel\arebyp.cl",
@"\sgminer-5-4-0-general\kernel\bitblock.cl",
@"\sgminer-5-4-0-general\kernel\bitblockold.cl",
@"\sgminer-5-4-0-general\kernel\blake.cl",
@"\sgminer-5-4-0-general\kernel\blake256.cl",
@"\sgminer-5-4-0-general\kernel\blake256r14.cl",
@"\sgminer-5-4-0-general\kernel\blake256r8.cl",
@"\sgminer-5-4-0-general\kernel\bmw.cl",
@"\sgminer-5-4-0-general\kernel\bmw256.cl",
@"\sgminer-5-4-0-general\kernel\bufius.cl",
@"\sgminer-5-4-0-general\kernel\ckolivas.cl",
@"\sgminer-5-4-0-general\kernel\credits.cl",
@"\sgminer-5-4-0-general\kernel\cubehash.cl",
@"\sgminer-5-4-0-general\kernel\cubehash256.cl",
@"\sgminer-5-4-0-general\kernel\darkcoin-mod.cl",
@"\sgminer-5-4-0-general\kernel\darkcoin.cl",
@"\sgminer-5-4-0-general\kernel\decred.cl",
@"\sgminer-5-4-0-general\kernel\diamond.cl",
@"\sgminer-5-4-0-general\kernel\echo.cl",
@"\sgminer-5-4-0-general\kernel\fresh.cl",
@"\sgminer-5-4-0-general\kernel\fugue.cl",
@"\sgminer-5-4-0-general\kernel\fuguecoin.cl",
@"\sgminer-5-4-0-general\kernel\groestl.cl",
@"\sgminer-5-4-0-general\kernel\groestl256.cl",
@"\sgminer-5-4-0-general\kernel\groestlcoin-v1.cl",
@"\sgminer-5-4-0-general\kernel\groestlcoin.cl",
@"\sgminer-5-4-0-general\kernel\hamsi.cl",
@"\sgminer-5-4-0-general\kernel\hamsi_helper.cl",
@"\sgminer-5-4-0-general\kernel\hamsi_helper_big.cl",
@"\sgminer-5-4-0-general\kernel\inkcoin.cl",
@"\sgminer-5-4-0-general\kernel\jh.cl",
@"\sgminer-5-4-0-general\kernel\keccak.cl",
@"\sgminer-5-4-0-general\kernel\keccak1600.cl",
@"\sgminer-5-4-0-general\kernel\luffa.cl",
@"\sgminer-5-4-0-general\kernel\lyra2.cl",
@"\sgminer-5-4-0-general\kernel\lyra2re.cl",
@"\sgminer-5-4-0-general\kernel\lyra2rev2.cl",
@"\sgminer-5-4-0-general\kernel\lyra2v2.cl",
@"\sgminer-5-4-0-general\kernel\marucoin-mod.cl",
@"\sgminer-5-4-0-general\kernel\marucoin-modold.cl",
@"\sgminer-5-4-0-general\kernel\marucoin.cl",
@"\sgminer-5-4-0-general\kernel\maxcoin.cl",
@"\sgminer-5-4-0-general\kernel\myriadcoin-groestl.cl",
@"\sgminer-5-4-0-general\kernel\neoscrypt.cl",
@"\sgminer-5-4-0-general\kernel\panama.cl",
@"\sgminer-5-4-0-general\kernel\pluck.cl",
@"\sgminer-5-4-0-general\kernel\psw.cl",
@"\sgminer-5-4-0-general\kernel\quarkcoin.cl",
@"\sgminer-5-4-0-general\kernel\qubitcoin.cl",
@"\sgminer-5-4-0-general\kernel\shabal.cl",
@"\sgminer-5-4-0-general\kernel\shavite.cl",
@"\sgminer-5-4-0-general\kernel\sifcoin.cl",
@"\sgminer-5-4-0-general\kernel\simd.cl",
@"\sgminer-5-4-0-general\kernel\skein.cl",
@"\sgminer-5-4-0-general\kernel\skein256.cl",
@"\sgminer-5-4-0-general\kernel\talkcoin-mod.cl",
@"\sgminer-5-4-0-general\kernel\twecoin.cl",
@"\sgminer-5-4-0-general\kernel\vanilla.cl",
@"\sgminer-5-4-0-general\kernel\whirlcoin.cl",
@"\sgminer-5-4-0-general\kernel\whirlpool.cl",
@"\sgminer-5-4-0-general\kernel\whirlpoolx.cl",
@"\sgminer-5-4-0-general\kernel\x14.cl",
@"\sgminer-5-4-0-general\kernel\x14old.cl",
@"\sgminer-5-4-0-general\kernel\yescrypt-multi.cl",
@"\sgminer-5-4-0-general\kernel\yescrypt.cl",
@"\sgminer-5-4-0-general\kernel\yescrypt_essential.cl",
@"\sgminer-5-4-0-general\kernel\zuikkis.cl",
@"\sgminer-5-4-0-tweaked\darkcoin-modBonairegw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\darkcoin-modFijigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\darkcoin-modHawaiigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\darkcoin-modPitcairngw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\darkcoin-modTahitigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\darkcoin-modTongagw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\lyra2rev2Bonairegw64l4.bin",
@"\sgminer-5-4-0-tweaked\lyra2rev2Fijigw64l4.bin",
@"\sgminer-5-4-0-tweaked\lyra2rev2Hawaiigw64l4.bin",
@"\sgminer-5-4-0-tweaked\lyra2rev2Pitcairngw64l4.bin",
@"\sgminer-5-4-0-tweaked\lyra2rev2Tahitigw64l4.bin",
@"\sgminer-5-4-0-tweaked\lyra2rev2Tongagw64l4.bin",
@"\sgminer-5-4-0-tweaked\quarkcoinBonairegw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\quarkcoinFijigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\quarkcoinHawaiigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\quarkcoinPitcairngw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\quarkcoinTahitigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\quarkcoinTongagw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\qubitcoinBonairegw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\qubitcoinFijigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\qubitcoinHawaiigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\qubitcoinPitcairngw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\qubitcoinTahitigw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\qubitcoinTongagw64l4ku0.bin",
@"\sgminer-5-4-0-tweaked\sgminer.exe",
@"\sgminer-5-4-0-tweaked\kernel\aes_helper.cl",
@"\sgminer-5-4-0-tweaked\kernel\alexkarnew.cl",
@"\sgminer-5-4-0-tweaked\kernel\alexkarold.cl",
@"\sgminer-5-4-0-tweaked\kernel\animecoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\arebyp.cl",
@"\sgminer-5-4-0-tweaked\kernel\bitblock.cl",
@"\sgminer-5-4-0-tweaked\kernel\bitblockold.cl",
@"\sgminer-5-4-0-tweaked\kernel\blake.cl",
@"\sgminer-5-4-0-tweaked\kernel\blake256.cl",
@"\sgminer-5-4-0-tweaked\kernel\blake256r14.cl",
@"\sgminer-5-4-0-tweaked\kernel\blake256r8.cl",
@"\sgminer-5-4-0-tweaked\kernel\bmw.cl",
@"\sgminer-5-4-0-tweaked\kernel\bmw256.cl",
@"\sgminer-5-4-0-tweaked\kernel\bufius.cl",
@"\sgminer-5-4-0-tweaked\kernel\ckolivas.cl",
@"\sgminer-5-4-0-tweaked\kernel\credits.cl",
@"\sgminer-5-4-0-tweaked\kernel\cubehash.cl",
@"\sgminer-5-4-0-tweaked\kernel\cubehash256.cl",
@"\sgminer-5-4-0-tweaked\kernel\darkcoin-mod.cl",
@"\sgminer-5-4-0-tweaked\kernel\darkcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\decred.cl",
@"\sgminer-5-4-0-tweaked\kernel\diamond.cl",
@"\sgminer-5-4-0-tweaked\kernel\echo.cl",
@"\sgminer-5-4-0-tweaked\kernel\fresh.cl",
@"\sgminer-5-4-0-tweaked\kernel\fugue.cl",
@"\sgminer-5-4-0-tweaked\kernel\fuguecoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\groestl.cl",
@"\sgminer-5-4-0-tweaked\kernel\groestl256.cl",
@"\sgminer-5-4-0-tweaked\kernel\groestlcoin-v1.cl",
@"\sgminer-5-4-0-tweaked\kernel\groestlcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\hamsi.cl",
@"\sgminer-5-4-0-tweaked\kernel\hamsi_helper.cl",
@"\sgminer-5-4-0-tweaked\kernel\hamsi_helper_big.cl",
@"\sgminer-5-4-0-tweaked\kernel\inkcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\jh.cl",
@"\sgminer-5-4-0-tweaked\kernel\keccak.cl",
@"\sgminer-5-4-0-tweaked\kernel\keccak1600.cl",
@"\sgminer-5-4-0-tweaked\kernel\luffa.cl",
@"\sgminer-5-4-0-tweaked\kernel\lyra2.cl",
@"\sgminer-5-4-0-tweaked\kernel\lyra2re.cl",
@"\sgminer-5-4-0-tweaked\kernel\lyra2rev2.cl",
@"\sgminer-5-4-0-tweaked\kernel\lyra2v2.cl",
@"\sgminer-5-4-0-tweaked\kernel\marucoin-mod.cl",
@"\sgminer-5-4-0-tweaked\kernel\marucoin-modold.cl",
@"\sgminer-5-4-0-tweaked\kernel\marucoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\maxcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\myriadcoin-groestl.cl",
@"\sgminer-5-4-0-tweaked\kernel\neoscrypt.cl",
@"\sgminer-5-4-0-tweaked\kernel\panama.cl",
@"\sgminer-5-4-0-tweaked\kernel\pluck.cl",
@"\sgminer-5-4-0-tweaked\kernel\psw.cl",
@"\sgminer-5-4-0-tweaked\kernel\quarkcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\qubitcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\shabal.cl",
@"\sgminer-5-4-0-tweaked\kernel\shavite.cl",
@"\sgminer-5-4-0-tweaked\kernel\sifcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\simd.cl",
@"\sgminer-5-4-0-tweaked\kernel\skein.cl",
@"\sgminer-5-4-0-tweaked\kernel\skein256.cl",
@"\sgminer-5-4-0-tweaked\kernel\talkcoin-mod.cl",
@"\sgminer-5-4-0-tweaked\kernel\twecoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\vanilla.cl",
@"\sgminer-5-4-0-tweaked\kernel\whirlcoin.cl",
@"\sgminer-5-4-0-tweaked\kernel\whirlpool.cl",
@"\sgminer-5-4-0-tweaked\kernel\whirlpoolx.cl",
@"\sgminer-5-4-0-tweaked\kernel\x14.cl",
@"\sgminer-5-4-0-tweaked\kernel\x14old.cl",
@"\sgminer-5-4-0-tweaked\kernel\yescrypt-multi.cl",
@"\sgminer-5-4-0-tweaked\kernel\yescrypt.cl",
@"\sgminer-5-4-0-tweaked\kernel\yescrypt_essential.cl",
@"\sgminer-5-4-0-tweaked\kernel\zuikkis.cl",
};
#endregion //CODE_GEN STUFF // listFiles.py
}
}

