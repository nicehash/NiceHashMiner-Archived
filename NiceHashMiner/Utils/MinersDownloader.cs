using NiceHashMiner.Interfaces;
using SharpCompress.Archive;
using SharpCompress.Archive.SevenZip;
using SharpCompress.Common;
using SharpCompress.Reader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;

namespace NiceHashMiner.Utils {
    public class MinersDownloader {
        private const string TAG = "MinersDownloader";

        DownloadSetup _downloadSetup;

        private WebClient _webClient;
        private Stopwatch _stopwatch;
        Thread _UnzipThread = null;

        bool isDownloadSizeInit = false;

        IMinerUpdateIndicator _minerUpdateIndicator;

        public MinersDownloader(DownloadSetup downloadSetup) {
            _downloadSetup = downloadSetup;
        }

        public void Start(IMinerUpdateIndicator minerUpdateIndicator) {
            _minerUpdateIndicator = minerUpdateIndicator;

            // if something not right delete previous and download new
            try {
                if (File.Exists(_downloadSetup.BinsZipLocation)) {
                    File.Delete(_downloadSetup.BinsZipLocation);
                }
                if (Directory.Exists(_downloadSetup.ZipedFolderName)) {
                    Directory.Delete(_downloadSetup.ZipedFolderName, true);
                }
            } catch { }
            Downlaod();
        }

        // #2 download the file
        private void Downlaod() {
            _minerUpdateIndicator.SetTitle(International.GetText("MinersDownloadManager_Title_Downloading"));
            _stopwatch = new Stopwatch();
            using (_webClient = new WebClient()) {
                _webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                _webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);

                Uri downloadURL = new Uri(_downloadSetup.BinsDownloadURL);

                _stopwatch.Start();
                try {
                    _webClient.DownloadFileAsync(downloadURL, _downloadSetup.BinsZipLocation);
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
                while (!File.Exists(_downloadSetup.BinsZipLocation) && try_count > 0) { --try_count; }

                UnzipStart();
            }
        }

        #endregion Download delegates


        private void UnzipStart() {
            _minerUpdateIndicator.SetTitle(International.GetText("MinersDownloadManager_Title_Settup"));
            _UnzipThread = new Thread(UnzipThreadRoutine);
            _UnzipThread.Start();
        }

        private void UnzipThreadRoutine() {
            try {
                if (File.Exists(_downloadSetup.BinsZipLocation)) {
                    
                    Helpers.ConsolePrint(TAG, _downloadSetup.BinsZipLocation + " already downloaded");
                    Helpers.ConsolePrint(TAG, "unzipping");

                    // if using other formats as zip are returning 0
                    FileInfo fileArchive = new FileInfo(_downloadSetup.BinsZipLocation);
                    var archive = ArchiveFactory.Open(_downloadSetup.BinsZipLocation);
                    _minerUpdateIndicator.SetMaxProgressValue(100);
                    long SizeCount = 0;
                    foreach (var entry in archive.Entries) {
                        if (!entry.IsDirectory) {
                            SizeCount += entry.CompressedSize;
                            Helpers.ConsolePrint(TAG, entry.Key);
                            entry.WriteToDirectory("", ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);

                            double prog = ((double)(SizeCount) / (double)(fileArchive.Length) * 100);
                            _minerUpdateIndicator.SetProgressValueAndMsg((int)prog, String.Format(International.GetText("MinersDownloadManager_Title_Settup_Unzipping"), prog.ToString("F2")));
                        }
                    }
                    archive.Dispose();
                    archive = null;
                    // after unzip stuff
                    _minerUpdateIndicator.FinishMsg(true);
                    // remove bins zip
                    try {
                        if (File.Exists(_downloadSetup.BinsZipLocation)) {
                            File.Delete(_downloadSetup.BinsZipLocation);
                        }
                    } catch (Exception e) {
                        Helpers.ConsolePrint("MinersDownloader.UnzipThreadRoutine", "Cannot delete exception: " + e.Message);
                    }
                } else {
                    Helpers.ConsolePrint(TAG, String.Format("UnzipThreadRoutine {0} file not found", _downloadSetup.BinsZipLocation));
                }
            } catch (Exception e) {
                Helpers.ConsolePrint(TAG, "UnzipThreadRoutine has encountered an error: " + e.Message);
            }
        }
    }
}
