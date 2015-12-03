# NiceHashMiner

- [Introduction](#introduction)
- [What are the benefits?](#benefits)
- [Features](#features)
- [How to get&run it?](#run)
- [Where is the profit coming from?](#profit)
- [Additional options](#options)
- [Troubleshooting](#troubleshooting)
- [References](#references)

# <a name="introduction"></a> Introduction

NiceHash Miner is an **easy to use CPU & GPU cryptocurrency miner for Windows**. With a simple an intuitive graphical user interface it allows you to quickly turn your PC, workstation or server into **money-making cryptocurrency mining machine**. Why leave you computer idle, whereas it could **earn you Bitcoins with just a few clicks**?

# <a name="benefits"></a> What are the benefits?

NiceHash Miner is essentially the only tool a miner needs. No need to go through tons of configuration files, various mining software versions, configuration tuning or cryptocurrency coins market analysis. **Auto-tuning for best performance and efficiency**, automatic selection and runtime **automatic switching to most profitable cryptocurrency algorithm** are all integrated into NiceHash Miner and will enable you seamless, joyful and **profitable mining experience**.

# <a name="features"></a> Features

- Easy one-click CPU mining for CPUs that support at least SSE2 (only works on Windows x64).
- Easy one-click GPU mining for NVIDIA GPUs using microarchitecture (compute capability) SM 2.1/3.x/5.x.
- Easy one-click GPU mining for AMD GPUs using any AMD GPU devices that supports OpenCL.
- Support for multiple CPUs on multiple NUMAs with affinity adjustments to maximize mining speed.
- Integrated support for Simple Multi-Algorithm. Always mine most profitable algorithm.
- Integrated benchmarking tool. Run it only once before you start mining and after every hardware/driver/software upgrade.
- Optimized algorithms for AVX2 and AVX (CPU mining).
- Watch-feature - automatically restart miner if crashed or hanged.
- Display current rate and your balance in real time.
- Auto update notifications.
- Much more...

# <a name="run"></a> How to get&run it?

All you have to do is download, extract and run the miner (no installation needed), choose the server location that is the **closest to your location**, run built-in benchmark and enter your Bitcoin wallet address where you want to get your coins sent at - and you are ready to start mining and maximizing your profit.

<i>**Note**: .NET Framework 2.0 or higher is required. No additional installations should be needed if you use Windows 7 or later. However if you encounter any issues when starting application (application would fail to start or errors/warnings about missing DLL files are displayed) you should download and install Microsoft **.NET Framework 2.0** and/or **Microsoft Visual C++ Redistributable**.</i>

Detailed instructions:
- Download binaries from here: https://github.com/nicehash/NiceHashMiner/releases
- Extract zip archive
- Run NiceHashMiner.exe
- After first run, start benchmark test, otherwise Multi-Algorithm mining will not work properly; for AMD GPUs we suggest you to run **Precise benchmark**
- Make sure you select your own personal Bitcoin wallet to receive payments, see **Bitcoin wallet guidelines and instructions** here: https://www.nicehash.com/index.jsp?p=faq#faqs15.
- You will recieve Bitcoin payments according to our payments schedule: https://www.nicehash.com/index.jsp?p=faq#faqs6

# <a name="profit"></a> Where is the profit coming from?

As a back-end NiceHash Miner relies on the <a href="https://www.nicehash.com" target="_blank">NiceHash.com</a> service. By running NiceHash Miner you're essentially selling the hashing power of your CPUs & GPUs to hashing power buyers. Those are using the hashing power to mine various cryptocurrency coins and support decentralized blockchain networks - similar to cloud computing - only that by running NiceHash Miner you're actually being a provider for the cryptocurrency mining hashing power. You are being part of a global compute power network, **empowering decentralized digital currencies**.

# <a name="options"></a> Additional options

Click 'Settings' button. NiceHash Miner will be relaunched with the ability to modify configs. Alternatively, you can manually modify config.json file (close NiceHash Miner first).

Parameter | Range | Description
-----------------|----------|-------------------
DebugConsole | true or false | When set to true, it displays debug console.
LessThreads | 0 .. 64 | Reduce number of threads used on each CPU by LessThreads.
ForceCPUExtension | 0, 1, 2 or 3 | Force certain CPU extension miner. 0 is automatic, 1 for SSE2, 2 for AVX and 3 for AVX2.
AutoStartMining | true or false | When set to true, NiceHashMiner will automatically start mining when launched.
HideMiningWindows | true or false | When set to true, sgminer, ccminer and cpuminer console windows will be hidden.
StartMiningWhenIdle | true or false | Automatically start mining when computer is idle and stop mining when computer is being used.
MinIdleSeconds | number | When StartMiningWhenIdle is set to true, MinIdleSeconds tells how many secunds computer has to be idle before mining starts.
SwitchMinSecondsFixed | number | Fixed part of minimal time (in seconds) before miner switches algorithm. Total time is SwitchMinSecondsFixed + SwitchMinSecondsDynamic.
SwitchMinSecondsDynamic | number | Random part of minimal time (in seconds) before miner switches algorithm. Total time is SwitchMinSecondsFixed + SwitchMinSecondsDynamic. Random part is used to prevent all world-wide NiceHash Miner users to have the exact same switching pattern.
Groups\ExtraLaunchParameters | text | Additional launch parameters when launching miner.
Groups\Algorithms\ExtraLaunchParameters | text | Additional launch parameters when launching miner and this algorithm.
Groups\Algorithms\BenchmarkSpeed | number | Fine tune algorithm ratios by manually setting benchmark speeds for each algorithm.
Groups\UsePassword | text or null | Use this password when launching miner. If null, default password 'x' is used.
Groups\Algorithms\UsePassword | text or null | Use this password when launching miner and this algorithm. If null, Groups\UsePassword is used.

Do not change any 'Name' parameters - changing them will not have any effect. 'Name' parameters are there only for easier config management.

Examples:
--------
If your CPU has 8 virtual cores and you would like to mine only with 7:
> Set LessThreads to 1.

If you would like to set lower starting difficulty for ScryptJaneNf16 algorithm because your CPU is slow:
> Set UsePassword to "d=0.1" under Algorithms item that has Name 
"scryptjanenf16" for all groups with Name "CPUx".

If you would like to manually configure intensity parameters for your three NVIDIA video cards (Quark algorithm):
> Set ExtraLaunchParameters to "-i 19,19,19" under Algorithms item that 
has Name "quark" for group with Name "NVIDIA5.x" or "NVIDIA3.x".

# <a name="troubleshooting"></a> Troubleshooting

My NVIDIA video card(s) is/are not detected.
> Make sure to install latest official NVIDIA drivers from here: 
http://www.nvidia.com/Download/index.aspx
> Also check weather your card has Compute capability (version) 2.1, 3.x or 5.x, check here: https://en.wikipedia.org/wiki/CUDA#Supported_GPUs

My AMD video card(s) is/are not detected.
> Make sure to install latest official AMD drivers from here:
http://support.amd.com/en-us/download
> Also check weather your card supports OpenCL, check "OpenCL" column here: https://en.wikipedia.org/wiki/List_of_AMD_graphics_processing_units

# <a name="references"></a> References

- For CPU mining our tpruvot's forked cpuminer has been used from here: https://github.com/nicehash/cpuminer-multi (compiled with MingW64).
- For NVIDIA 5.x cards, sp's fork of ccminer has been used from here: https://github.com/sp-hash/ccminer.
- For NVIDIA 2.1 and 3.x (older cards), tpruvot's fork of ccminer has been used from here: https://github.com/tpruvot/ccminer.
- For AMD cards, sgminer has been used from here: https://github.com/sgminer-dev/sgminer.
