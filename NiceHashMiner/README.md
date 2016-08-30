# NiceHash Miner
## Testing build flags:

Define **SWITCH_TESTING** in Properties/Build/Conditional compilation symbols to enable miner switch testing. Aditional options are in **SwitchTesting.cs**.

## Aditional build steps:
Make sure to copy OpenCLDeviceDetection.exe, CudaDeviceDetection.exe and nvml.dll to NHM.exe folder.

## Bins Paths codegen
Use /codegen/genBins.py ('python genBins.py > BINS_CODEGEN.cs') to generate miner bins BINS_CODEGEN.cs file. Copy BINS_CODEGEN.cs to NiceHashMiner/Utils folder.