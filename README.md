# Bitcoin Phiber Miner

- [Introduction](#introduction)
- [What are the benefits?](#benefits)
- [Features](#features)
- [Requirements](#requirements)
- [How to get&run it?](#run)
- [Additional options](#options)
- [Troubleshooting](#troubleshooting)

# <a name="introduction"></a> Introduction

Bitcoin Phiber Miner is an **easy to use CPU & GPU cryptocurrency miner for Windows**. With a simple an intuitive graphical user interface it allows you to quickly turn your PC, workstation or server into **money-making cryptocurrency mining machine**. Why leave your computer idle, whereas it could **earn you Bitcoin Phiber with just a few clicks**?

We support NVIDIA GPUs only at this time.

<img src="https://Bitcoin Phiber.com/imgs/NHM_v1610.png" />

Please follow us on Twitter <a href="https://twitter.com/Bitcoin PhiberMining" target="_blank">@Bitcoin PhiberMining</a> for updates on new versions and other important information.

# <a name="benefits"></a> What are the benefits?

Bitcoin Phiber Miner is essentially the only tool a miner needs. No need to go through tons of configuration files, various mining software versions, configuration tuning or cryptocurrency coins market analysis. **Auto-tuning for best performance and efficiency**, automatic selection and runtime **automatic switching to most profitable cryptocurrency algorithm** are all integrated into Bitcoin Phiber Miner and will enable you seamless, joyful and **profitable mining experience**.

# <a name="features"></a> Features

- Easy one-click GPU mining for NVIDIA GPUs using microarchitecture (compute capability) SM 2.1/3.x/5.x/6.x.
- Integrated benchmarking tool. Run it only once before you start mining and after every hardware/driver/software upgrade.
- Watch-feature - automatically restart miner if crashed or hanged.
- Auto update notifications.
- Much more...

# <a name="requirements"></a> Requirements

- **Windows** 7 or newer operating system **64-bit**
- For NVIDIA mining any NVIDIA GPU with Compute capability (SM) 2.1 or newer
- **up-to-date drivers** for all GPUs
- **Reliable** internet connectivity
- Personal **Bitcoin wallet**: https://www.Bitcoin Phiber.com/index.jsp?p=faq#faqs15

# <a name="run"></a> How to get&run it?

All you have to do is download, extract and run the miner (no installation needed), choose the server location that is the **closest to your location**, run built-in benchmark and enter your Bitcoin wallet address where you want to get your coins sent at - and you are ready to start mining and maximizing your profit.

<i>**Note**: .NET Framework 2.0 or higher and Microsoft Visual C++ Redistributable 2013 is required. No additional installations should be needed if you use Windows 7 or later. However, if you encounter any issues when starting application (application would fail to start or errors/warnings about missing DLL files are displayed) you should download and install <a href="https://www.microsoft.com/en-us/download/details.aspx?id=30653" target="_blank">Microsoft **.NET Framework 2.0**</a> and <a href="https://www.microsoft.com/en-us/download/details.aspx?id=40784" target="_blank">Microsoft **Visual C++ Redistributable 2013 (vcredist_x64.exe)**</a> (after installation a reboot might be required).</i>

Detailed instructions:
- Download binaries 
- Extract zip archive
- Run BitcoinPhiberMiner.exe
- Make sure you select your own personal Bitcoin Phiber wallet to receive payments.

# <a name="options"></a> Additional options

Click 'Settings' button. Bitcoin Phiber Miner will be relaunched with the ability to modify configs. Alternatively, you can manually modify \configs\General.json for general settings and \configs\benchmark_XXX.json (XXX is your device UUID) files for device benchmark settings (close Bitcoin Phiber Miner first).

## General settings
Parameter | Range | Description
-----------------|----------|-------------------
ConfigFileVersion | Version | This is to identify which version of Bitcoin PhiberMiner did the config file is made from.
Language | number | Language selection for Bitcoin PhiberMiner GUI.
DisplayCurrency | valid 3 letter code | Converts to selected currency via http://fixer.io valid options are any supported via fixer.
DebugConsole | true or false | When set to true, it displays debug console.
BitcoinAddress | valid BTC address | The address that Bitcoin PhiberMiner will mine to.
WorkerName | text | To identify the computer on Bitcoin Phiber web UI.
ServiceLocation | number | Used to select the location of the mining server.
HideMiningWindows | true or false | When set to true, sgminer, ccminer and cpuminer console windows will be hidden.
MinimizeToTray | true or false | When set to true, Bitcoin PhiberMiner will minimize to the system tray.
ForceCPUExtension | 0, 1, 2, 3 or 4 | Force certain CPU extension miner. 0 is automatic, 1 for AVX2, 2 for AVX, 3 for AES and  4 for SSE2.
SwitchMinSecondsFixed | number | Fixed part of minimal time (in seconds) before miner switches algorithm. Total time is SwitchMinSecondsFixed + SwitchMinSecondsDynamic.
SwitchMinSecondsDynamic | number | Random part of minimal time (in seconds) before miner switches algorithm. Total time is SwitchMinSecondsFixed + SwitchMinSecondsDynamic. Random part is used to prevent all world-wide Bitcoin Phiber Miner users to have the exact same switching pattern.
SwitchMinSecondsAMD | number | Fixed part of minimal time (in seconds) before miner switches algorithm (additional time for AMD GPUs). Total time is SwitchMinSecondsFixed + SwitchMinSecondsAMD + SwitchMinSecondsDynamic.
MinerAPIQueryInterval | number | Amount of time between each API call to get the latest stats from miner.
MinerRestartDelayMS | number | Amount of time to delay before trying to restart the miner.
BenchmarkTimeLimits\CPU | numbers | List of benchmarking time (in seconds). The first one is for "Quick benchmark", second one is for "Standard benchmark" and third one is for "Precise benchmark".
BenchmarkTimeLimits\NVIDIA | numbers | List of benchmarking time (in seconds). The first one is for "Quick benchmark", second one is for "Standard benchmark" and third one is for "Precise benchmark".
BenchmarkTimeLimits\AMD | numbers | List of benchmarking time (in seconds). The first one is for "Quick benchmark", second one is for "Standard benchmark" and third one is for "Precise benchmark".
DeviceDetection\DisableDetectionNVidia6X | true or false | Set it to true if you would like to skip the detection of NVidia6.X GPUs.
DeviceDetection\DisableDetectionNVidia5X | true or false | Set it to true if you would like to skip the detection of NVidia5.X GPUs.
DeviceDetection\DisableDetectionNVidia3X | true or false | Set it to true if you would like to skip the detection of NVidia3.X GPUs.
DeviceDetection\DisableDetectionNVidia2X | true or false | Set it to true if you would like to skip the detection of NVidia2.X GPUs.
DeviceDetection\DisableDetectionAMD | true or false | Set it to true if you would like to skip the detection of AMD GPUs.
AutoScaleBTCValues | true or false | Set it to true if you wish to see the BTC values autoscale to the appropriate scale.
StartMiningWhenIdle | true or false | Automatically start mining when computer is idle and stop mining when computer is being used.
MinIdleSeconds | number | When StartMiningWhenIdle is set to true, MinIdleSeconds tells how many seconds computer has to be idle before mining starts.
LogToFile | true or false | Set it to true if you would like Bitcoin PhiberMiner to log to a file.
LogMaxFileSize | number | The maximum size (in bytes) of the log file before roll over.
ShowDriverVersionWarning | true or false | Set to true if you would like to get a warning if less than ideal driver for mining is detected.
DisableWindowsErrorReporting | true or false | Set it to true if you would like to disable windows error reporting. This will allow Bitcoin PhiberMiner to restart the miner in the case of the miner crashes.
UseNewSettingsPage | true or false | Set to true if you would like to use the new Settings form.
NVIDIAP0State | true or false | When set to true, Bitcoin PhiberMiner would change all supported NVidia GPUs to P0 state. This will increase some performance on certain algorithms.
ethminerDefaultBlockHeight | number | A fallback number that will be used if API call fails. This is only used for benchmarking.
EthminerDagGenerationType | 0, 1, 2, 3 | Set ethminer DAG mode generation 0 - SingleKeep, 1 - Single, 2 - Sequential, 3 - Parallel.
ApiBindPortPoolStart | number | Set the starting value (default is 5100) for miners API ports. When a new miner is created it will use an avaliable API port starting from the ApiBindPortPoolStart and higher.
MinimumProfit | number | If set to any value more than 0 (USD), Bitcoin PhiberMiner will stop mining if the calculated profit falls below the set amount.
LastDevicesSettup | device settup list | This list is used for setting if a device is enabled or disabled.
LastDevicesSettup\Enabled | true or false | Set to false if you would like to disable this device for benchmarking and mining by Bitcoin PhiberMiner.
LastDevicesSettup\UUID | text | Used for unique identification purposes in the config file (**DO NOT EDIT**)
LastDevicesSettup\Name | text | Used for identification purposes in the config file (**DO NOT EDIT**)

## Benchmark settings (per device)
Parameter | Range | Description
-----------------|----------|-------------------
DeviceUUID | text | Used for unique identification purposes in the config file (**DO NOT EDIT**)
DeviceName | text | Used for identification purposes in the config file (**DO NOT EDIT**)
AlgorithmSettings | dictionary {key: text, value: Algorithm } | Key value paired dictionary with avaliable device algorithms settings. Keys should not be edited only Algorithm data.
AlgorithmSettings\Algorithm\Bitcoin PhiberID | number | Algorithm ID (**DO NOT EDIT**)
AlgorithmSettings\Algorithm\MinerName | text | specific miner name setting (**DO NOT EDIT**)
AlgorithmSettings\Algorithm\BenchmarkSpeed | number | Fine tune algorithm ratios by manually setting benchmark speeds for each algorithm.
AlgorithmSettings\Algorithm\ExtraLaunchParameters | text | Additional launch parameters when launching miner and this algorithm.
AlgorithmSettings\Algorithm\Intensity | number | Set algorithm Intensity setting for this algorithm (**Setting works only for supported NVIDIA miners**).
AlgorithmSettings\Algorithm\LessThreads | 0 .. 64 | Reduce number of threads used on CPU by LessThreads (**Setting works only on CPU miners**).
AlgorithmSettings\Algorithm\Skip | true or false | Set to true if you would like to skip & disable a particular algorithm. Benchmarking as well as actual mining will be disabled for this particular algorithm. That said, auto-switching will skip this algorithm when mining will be running.

Examples:
--------

# <a name="troubleshooting"></a> Troubleshooting

My NVIDIA video card(s) is/are not detected.
> Make sure to install latest official NVIDIA drivers from here: 
http://www.nvidia.com/Download/index.aspx
> Also check whether your card has Compute capability (version) 2.1, 3.x or 5.x, check here: https://en.wikipedia.org/wiki/CUDA#Supported_GPUs

I'm getting "Always ask before opening this file" when running Bitcoin Phiber Miner
> Make sure you un-check the checkbox "Always ask before opening this file" when Bitcoin Phiber Miner is starting cpuminer, ccminer or sgminer back-end programs. This is needed because back-end programs will be executed several times while Bitcoin Phiber Miner is running (auto-switching according to profitability and in case programs hangs) and you have to make sure these programs will be to executed automatically without your intervention.

My anti-virus is blocking the application
> Some anti-virus software might block Bitcoin Phiber Miner as well as supporting back-end programs (cpuminer, ccminer, sgminer) due to false-positive matches. All software, included into Bitcoin Phiber Miner has been verified and checked by our team and is absolutely virus/trojan free. Our service is well established and trusted among users, therefore you can fully trust software releases that are downloaded from our GitHub repository: https://github.com/Bitcoin Phiber/Bitcoin PhiberMiner/releases. However, make sure you **never download and run any files from other unknown sources**! If you downloaded the software package from our GitHub repository you can simply resolve the issues with false-positives by adding the files sgminer.exe, ccminer_sp.exe, ccminer_tpruvot.exe, cpuminer_x64_AVX.exe, cpuminer_x64_AVX2.exe and cpuminer_x64_SSE2.exe to anti-virus exception list.

My benchmarking results are not accurate
> Any kind of automation can only be done up to a particular level. We've spent significant effort to make benchmarking as good as possible, but it can't be made ideal. First of all, make sure to run Precise benchmark if Standard benchmark is not giving you satisfactory results. If you still see a deviation of actual mining speed from the one, calculated from benchmark, then you should manually enter these observed speed numbers from actual mining into config.json file or set them via the "Settings" button.

