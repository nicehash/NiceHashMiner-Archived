# NiceHashMiner
NiceHash easy to use CPU&GPU Miner for Windows x64.

Currently, miner is in beta testing. Only CPU mining supported for now.

For CPU mining our tpruvot's forked cpuminer has been used from here: https://github.com/nicehash/cpuminer-multi (compiled with MingW64).

- [Features](#features)
- [How to run?](#run)

# <a name="features"></a> Features

- Easy one-click CPU mining for CPUs that support at least SSE2.
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

![Alt text](/newminer.png?raw=true)