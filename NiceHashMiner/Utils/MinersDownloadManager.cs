using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Windows.Forms;
using NiceHashMiner.Interfaces;
using System.Threading;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Utils {
    public partial class MinersDownloadManager : BaseLazySingleton<MinersDownloadManager> {

        private readonly string TAG;

        private WebClient _webClient;
        private Stopwatch _stopwatch;

        const string d_01 = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin.zip";
        const string d_v1_7_0_4 = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_1_7_0_4.zip";
        const string d_v1_7_1_3 = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_1_7_1_3.zip";
        const string d_v1_7_1_4 = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_1_7_1_4.zip";
        const string d_v1_7_2_0 = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_1_7_2_0.zip";
        public string BinsDownloadURL = d_v1_7_2_0;
        public string BinsZipLocation = "bins.zip";

        private class DownloadData {
            public string url { get; set; }
            public string zipLocation { get; set; }
            public bool shouldDownload { get; set; }
        }
        // shared
        DownloadData dl_shared = new DownloadData() {
            url = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_cpu_shared.zip",
            zipLocation = "bins_shared.zip"
        };
        // nvidia
        DownloadData dl_nvidia = new DownloadData() {
            url = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_nvidia.zip",
            zipLocation = "bins_nvidia.zip"
        };
        // amd
        DownloadData dl_amd = new DownloadData() {
            url = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_amd.zip",
            zipLocation = "bins_amd.zip"
        };
        // all
        DownloadData dl_all = new DownloadData() {
            url = "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_all.zip",
            zipLocation = "bins_all.zip"
        };

        List<DownloadData> _downloadsData;

        bool isDownloadSizeInit = false;

        IMinerUpdateIndicator _minerUpdateIndicator;

        protected MinersDownloadManager() {
            TAG = this.GetType().Name;
        }

        public void InitDownloadPaths() {
            _downloadsData = new List<DownloadData>();
            dl_shared.shouldDownload = !IsMinersBins_SHARED_Init();
            dl_nvidia.shouldDownload = !IsMinersBins_NVIDIA_Init() && ComputeDeviceQueryManager.Instance.HasNVIDIA;
            dl_amd.shouldDownload = !IsMinersBins_AMD_Init() && ComputeDeviceQueryManager.Instance.HasAMD;
            _downloadsData.Add(dl_shared);
            _downloadsData.Add(dl_nvidia);
            _downloadsData.Add(dl_amd);
        }

        public void Start(IMinerUpdateIndicator minerUpdateIndicator) {
            _minerUpdateIndicator = minerUpdateIndicator;

            // if something not right delete previous and download new
            try {
                if (File.Exists(BinsZipLocation)) {
                    File.Delete(BinsZipLocation);
                }
                if (Directory.Exists("bin")) {
                    Directory.Delete("bin", true);
                }
            } catch { }
            Downlaod();
        }

        // #1 check if miners exits
        public bool IsMinerBinFolder() {
            return Directory.Exists("bin");
        }

        bool IsMinerBinZip() {
            return File.Exists(BinsZipLocation);
        }

        // #2 download the file
        private void Downlaod() {
            _minerUpdateIndicator.SetTitle(International.GetText("MinersDownloadManager_Title_Downloading"));
            _stopwatch = new Stopwatch();
            using (_webClient = new WebClient()) {
                _webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                _webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);

                Uri downloadURL = new Uri(BinsDownloadURL);

                _stopwatch.Start();
                try {
                    _webClient.DownloadFileAsync(downloadURL, BinsZipLocation);
                } catch (Exception ex) {
                    Helpers.ConsolePrint("MinersDownloadManager", ex.Message);
                }
            }
        }

        #region Download delegates

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            if (!isDownloadSizeInit) {
                isDownloadSizeInit = true;
                _minerUpdateIndicator.SetMaxProgressValue((int)(e.TotalBytesToReceive / 1024));
            }

            // Calculate download speed and output it to labelSpeed.
            var speedString = string.Format("{0} kb/s", (e.BytesReceived / 1024d / _stopwatch.Elapsed.TotalSeconds).ToString("0.00"));

            // Show the percentage on our label.
            var percString = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            var labelDownloaded = string.Format("{0} MB / {1} MB",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));

            _minerUpdateIndicator.SetProgressValueAndMsg(
                (int)(e.BytesReceived / 1024d),
                String.Format("{0}   {1}   {2}", speedString, percString,labelDownloaded));

        }

        // The event that will trigger when the WebClient is completed
        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e) {
            _stopwatch.Stop();
            _stopwatch = null;

            if (e.Cancelled == true) {
                // TODO handle Cancelled
                Helpers.ConsolePrint(TAG, "DownloadCompleted Cancelled");
            } else {
                // TODO handle Success
                Helpers.ConsolePrint(TAG, "DownloadCompleted Success");
                // wait one second for binary to exist
                System.Threading.Thread.Sleep(1000);
                // extra check dirty
                int try_count = 50;
                while (!File.Exists(BinsZipLocation) && try_count > 0) { --try_count; }

                UnzipStart();
            }
        }

        #endregion Download delegates


        private void UnzipStart() {
            _minerUpdateIndicator.SetTitle(International.GetText("MinersDownloadManager_Title_Settup"));
            Thread BenchmarkThread = new Thread(UnzipThreadRoutine);
            BenchmarkThread.Start();
        }

        private void UnzipThreadRoutine() {
            if (File.Exists(BinsZipLocation)) {
                Helpers.ConsolePrint(TAG, BinsZipLocation + " already downloaded");
                Helpers.ConsolePrint(TAG, "unzipping");
                using (ZipArchive archive = ZipFile.Open(BinsZipLocation, ZipArchiveMode.Read)) {
                    //archive.ExtractToDirectory("bin");
                    _minerUpdateIndicator.SetMaxProgressValue(archive.Entries.Count);
                    int prog = 0;
                    // first create dirs
                    foreach (ZipArchiveEntry entry in archive.Entries) {
                        if (entry.Length == 0) {
                            Helpers.ConsolePrint("ZipArchiveEntry", entry.FullName);
                            Helpers.ConsolePrint("ZipArchiveEntry", entry.Length.ToString());
                            Directory.CreateDirectory(entry.FullName);
                            _minerUpdateIndicator.SetProgressValueAndMsg(prog++, String.Format(International.GetText("MinersDownloadManager_Title_Settup_Unzipping"), ((double)(prog) / (double)(archive.Entries.Count) * 100).ToString("F2")));
                        }
                    }
                    // unzip files
                    foreach (ZipArchiveEntry entry in archive.Entries) {
                        if (entry.Length > 0) {
                            Helpers.ConsolePrint("ZipArchiveEntry", entry.FullName);
                            Helpers.ConsolePrint("ZipArchiveEntry", entry.Length.ToString());
                            entry.ExtractToFile(entry.FullName);
                            _minerUpdateIndicator.SetProgressValueAndMsg(prog++, String.Format(International.GetText("MinersDownloadManager_Title_Settup_Unzipping"), ((double)(prog) / (double)(archive.Entries.Count) * 100).ToString("F2")));
                        }
                    }
                }
                // after unzip stuff
                ConfigManager.Instance.GeneralConfig.DownloadInit = true;
                ConfigManager.Instance.GeneralConfig.Commit();
                _minerUpdateIndicator.FinishMsg(IsMinersBinsInit());
                // remove bins zip
                try {
                    if (File.Exists(BinsZipLocation)) {
                        File.Delete(BinsZipLocation);
                    }
                } catch { }
            } else {
                Helpers.ConsolePrint(TAG, "UnzipThreadRoutine bin.zip file not found");
            }
        }


        public bool IsMinersBins_ALL_Init() {
            foreach (var filePath in ALL_FILES_BINS) {
                if (!File.Exists(String.Format("bin{0}", filePath))) {
                    return false;
                }
            }
            return true;
        }

        // this one is mandatory download it regardles of CPU avaliability
        private bool IsMinersBins_SHARED_Init() {
            foreach (var filePath in ALL_FILES_BINS_SHARED) {
                if (!File.Exists(String.Format("bin{0}", filePath))) {
                    return false;
                }
            }
            return true;
        }

        private bool IsMinersBins_NVIDIA_Init() {
            foreach (var filePath in ALL_FILES_BINS_NVIDIA) {
                if (!File.Exists(String.Format("bin{0}", filePath))) {
                    return false;
                }
            }
            return true;
        }

        private bool IsMinersBins_AMD_Init() {
            foreach (var filePath in ALL_FILES_BINS_AMD) {
                if (!File.Exists(String.Format("bin{0}", filePath))) {
                    return false;
                }
            }
            return true;
        }

        public bool IsMinersBinsInit() {
            // in the future or 2.0
            //bool isOk = IsMinersBins_SHARED_Init();
            //if (isOk && ComputeDeviceQueryManager.Instance.HasNVIDIA) {
            //    isOk = IsMinersBins_NVIDIA_Init();
            //}
            //if (isOk && ComputeDeviceQueryManager.Instance.HasAMD) {
            //    isOk = IsMinersBins_AMD_Init();
            //}

            //return isOk;
            return IsMinersBins_ALL_Init();
        }

    }
}
