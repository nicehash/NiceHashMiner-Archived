# NiceHashMiner
NiceHash easy to use CPU&GPU Miner for Windows.

Currently, miner is in beta testing. Only CPU mining supported for now.

For CPU mining our tpruvot's forked cpuminer has been used from here: https://github.com/nicehash/cpuminer-multi (compiled with MingW64).

- [Features](#features)
- [How to run?](#run)
- [Additional options](#options)

![Alt text](/newminer.png?raw=true)

# <a name="features"></a> Features

- Easy one-click CPU mining for CPUs that support at least SSE2 (x64 OS).
- Support for multiple CPUs on multiple NUMAs with affinity adjustments to maximize mining speed.
- Integrated support for Simple Multi-Algorithm. Always mine most profitable algorithm.
- Integrated benchmarking tool. Run it only once before you start mining and after every hardware upgrade.
- Optimized algorithms for AVX2 and AVX.
- Watch-feature - automatically restart miner if crashed.
- Display current rate and your balance in real time.
- Auto update notifications.

# <a name="run"></a> Instructions on how to run

- Download binaries from here: https://github.com/nicehash/NiceHashMiner/releases
- Extract zip archive
- Run NiceHashMiner.exe
- After first run, start benchmark test, otherwise Multi-Algorithm mining will not work properly.
- Note: .NET Framework 2.0 or higher is required. No additional installations are needed if you use Windows 7 or later.

# <a name="options"></a> Additional options

After first launch, config.json file is created. Additional options are exposed if you decide to manually modify this file.

Parameter        | Range    | Description
-----------------|----------|-------------------
DebugConsole     | 0 or 1   | When set to 1, it displays debug console.
LessThreads      | 0 .. 64  | Reduce number of threads used on each CPU by LessThreads. Example: if you have 8 cores and LessThreads = 1 then CPU miner will work with 7 threads.
BenchmarkSpeeds  | double   | Fine tune algorithm ratios by manually setting benchmark speeds for each algorithm.
ExtraLaunchParameters | string | Additional launch parameters when launching miner.
