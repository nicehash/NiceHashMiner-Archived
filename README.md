# NiceHashMiner

- [Introduction](#introduction)
- [Features](#features)
- [How to get it and run](#run)
- [Additional options](#options)

# <a name="introduction"></a> Introduction

NiceHash Miner is easy to use CPU&GPU Miner for Windows with Multi-Algorithm mining support. It is the only tool a miner needs. You can quickly turn your PC into a miner. No need to go through tons of config files, various mining software forks, config tuning or algorithm-cryptocoin market analysis. With this miner, everything above is already taken care of by us. Just run it and enjoy best possible profit being squeezed out of your PC.

Currently, miner is in public beta testing. Please, be patient as there may be some bugs. Reporting them to us would greatly help us. Thank you for participating in beta testing!

- For CPU mining our tpruvot's forked cpuminer has been used from here: https://github.com/nicehash/cpuminer-multi (compiled with MingW64).
- For NVIDIA Maxwell mining, sp's fork of ccminer has been used from here: https://github.com/sp-hash/ccminer (compiled with VS 2013 32bit, CUDA 6.5).
- For NVIDIA mining (older cards), tpruvot's fork of ccminer has been used from here: https://github.com/tpruvot/ccminer (compiled with VS 2013 32bit, CUDA 6.5).

# <a name="features"></a> Features

- Easy one-click CPU mining for CPUs that support at least SSE2 (only works on Windows x64).
- Easy one-click GPU mining for NVIDIA GPUs using microarchitecture SM 3.x/5.x.
- Support for multiple CPUs on multiple NUMAs with affinity adjustments to maximize mining speed.
- Integrated support for Simple Multi-Algorithm. Always mine most profitable algorithm.
- Integrated benchmarking tool. Run it only once before you start mining and after every hardware upgrade.
- Optimized algorithms for AVX2 and AVX (CPU mining).
- Watch-feature - automatically restart miner if crashed.
- Display current rate and your balance in real time.
- Auto update notifications.
- Much more...

# <a name="run"></a> How to get it and run

- Download binaries from here: https://github.com/nicehash/NiceHashMiner/releases
- Extract zip archive
- Run NiceHashMiner.exe
- After first run, start benchmark test, otherwise Multi-Algorithm mining will not work properly. Benchmark may sometimes take longer time or cause NiceHash Miner to become unresponsive. This is normal.

<i>Note: .NET Framework 2.0 or higher is required. No additional installations are needed if you use Windows 7 or later.</i>

# <a name="options"></a> Additional options

After first launch, config.json file is created. Additional options are exposed if you decide to manually modify this file.

Parameter        | Range    | Description
-----------------|----------|-------------------
DebugConsole     | 0 or 1   | When set to 1, it displays debug console.
LessThreads      | 0 .. 64  | Reduce number of threads used on each CPU by LessThreads.
SwitchMinSecondsFixed | number | Fixed part of minimal time (in seconds) before miner switches algorithm. Total time is SwitchMinSecondsFixed + SwitchMinSecondsDynamic.
SwitchMinSecondsDynamic | number | Random part of minimal time (in seconds) before miner switches algorithm. Total time is SwitchMinSecondsFixed + SwitchMinSecondsDynamic. Random part is used to prevent all world-wide NiceHash Miner users to have the exact same switching pattern.
Groups\ExtraLaunchParameters | text | Additional launch parameters when launching miner.
Groups\Algorithms\ExtraLaunchParameters | text | Additional launch parameters when launching miner and this algorithm.
Groups\Algorithms\BenchmarkSpeed   | number   | Fine tune algorithm ratios by manually setting benchmark speeds for each algorithm.
Groups\UsePassword | text or null | Use this password when launching miner. If null, default password 'x' is used.
Groups\Algorithms\UsePassword | text or null | Use this password when launching miner and this algorithm. If null, Groups\UsePassword is used.

Do not change any 'Name' parameters - changin them will not have any effect. 'Name' parameters are there only for easier config management. Eventually, we will make all these config properties configurable over GUI.

Examples:
--------
If your CPU has 8 virtual cores and you would like to mine only with 7:
> Set LessThreads to 1.

If you would like to set lower starting difficulty for ScryptJaneNf16 algorithm because your CPU is slow:
> Set UsePassword to "d=0.1" under Algorithms item that has Name "scryptjanenf16" for all groups with Name "CPUx".

If you would like to manually configure intensity parameters for your three video cards (Quark algorithm):
> Set ExtraLaunchParameters to "-i 19,19,19" under Algorithms item that has Name "quark" for group with Name "NVIDIA5.x" or "NVIDIA3.x".
