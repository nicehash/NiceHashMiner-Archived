# Algorithm ExtraLaunchParameters
## How does it work:
If you are an advanced user that wants to tweak the performance of your GPUs or CPUs you can set **supported** options in the **ExtraLaunchParameters** for selected Device and Algorithm.

If you have 3 AMD devices with the following **ExtraLaunchParameters** settings for algorithm A and B:
  - **device1** --xintensity 1024 --worksize 64 --gputhreads 2
  - **device2** --xintensity 512 --worksize 128 --gputhreads 2
  - **device3** --xintensity 512 --worksize 64 --gputhreads 4

If **algorithm A** is most profitable for device1 and device2 and **algorithm B** for device3, NiceHashMiner will run two sgminers for A and B like so:
  - sgminer .. --xintensity 1024,512 --worksize 64,128 --gputhreads 2,2 .. (device1 and device2)
  - sgminer .. --xintensity 512 --worksize 64 --gputhreads 4 .. (device3)

If **algorithm A** is most profitable for all three devices, NiceHashMiner will run two sgminers for A like so:
  - sgminer .. --xintensity 1024,512,512 --worksize 64,128,64 --gputhreads 2,2,4 .. (device1, device2, device3)

So when setting **ExtraLaunchParameters** set them **per device and algorithm** NiceHashMiner will group them accordingly.
If you leave **ExtraLaunchParameters** empty the defaults will be used or ignored if no parameters have been set.

## Supported options
### NVIDIA ccminers
  - **--intensity=** or **-i** (if not set default 0 or ignored if unused)

### NVIDIA ccminer CryptoNight
  - **--launch=** or **-l** (if not set default 8x40 or ignored if unused)
  - **--bfactor=** (if not set default 0 or ignored if unused)
  - **--bsleep=** (if not set default 0 or ignored if unused)

### NVIDIA ethminer DaggerHashimoto
  - **--cuda-block-size** (if not set default 128 or ignored if unused)
  - **--cuda-grid-size** (if not set default 8192 or ignored if unused)

### AMD sgminer
  - **--keccak-unroll** (if not set default 0 or ignored if unused)
  - **--hamsi-expand-big** (if not set default 4 or ignored if unused)
  - **--nfactor** (if not set default 10 or ignored if unused)
  - **--intensity** (if not set default d or ignored if unused)
  - **--xintensity** (if not set default -1 or ignored if unused, overrides --intensity)
  - **--rawintensity** (if not set default -1 or ignored if unused, overrides --xintensity)
  - **--thread-concurrency** (if not set default -1 or ignored if unused)
  - **--worksize** (if not set default -1 or ignored if unused)
  - **--gpu-threads** (if not set default -1 or ignored if unused)
  - **--lookup-gap** (if not set default -1 or ignored if unused)

#### AMD sgminer Temperature Control (if Temperature Control disabled all options will be ignored)
  - **--gpu-fan** (if not set default 30-60)
  - **--temp-cutoff** (if not set default 95)
  - **--temp-overheat** (if not set default 85)
  - **--temp-target** (if not set default 75)
  - **--auto-fan** (if not set default it means it will not be used)
  - **--auto-gpu** (if not set default it means it will not be used)

### AMD ethminer DaggerHashimoto
  - **--cl-local-work** (if not set default  64 or ignored if unused)
  - **--cl-global-work** (if not set default 4096 * 64 or ignored if unused)

### CPU cpuminer
  - **--threads=** (if not set default it will use all avaliable virtual threads)
  - **--cpu-affinity** (if not set option will be ignored)
  - **--cpu-priority** (if not set option will be ignored)