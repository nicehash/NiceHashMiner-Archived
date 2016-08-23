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

namespace NiceHashMiner.Utils {
    public class MinersDownloadManager : BaseLazySingleton<MinersDownloadManager> {

        private readonly string TAG;

        private WebClient _webClient;
        private Stopwatch _stopwatch;

        const string d_01 = "https://github.com/nicehash/NiceHashMiner/releases/download/1.6.1.2/NiceHashMiner_v1.6.1.2.zip";
        public string BinsDownloadURL = d_01;
        public string BinsZipLocation = "bin.zip";

        bool isDownloadSizeInit = false;
        Form_Loading _downloadForm;
        Form_Loading _unzipForm;

        protected MinersDownloadManager() {
            TAG = this.GetType().Name;
        }

        public void Start(ref Form_Loading downloadForm, Form_Loading unzipForm) {
            _downloadForm = downloadForm;
            _unzipForm = unzipForm;
            _downloadForm.Show();
            // #1 check bin folder
            if (!IsMinerBinFolder()) {
                Helpers.ConsolePrint(TAG, "miner bin folder NOT found");
                Helpers.ConsolePrint(TAG, "Downloading " + BinsDownloadURL);
                Downlaod();
            }
        }

        // #1 check if miners exits 
        // TODO
        bool IsMinerBinFolder() {
            return Directory.Exists("bin");
        }

        // #2 download the file
        private void Downlaod() {
            _stopwatch = new Stopwatch();
            using (_webClient = new WebClient()) {
                _webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                _webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);

                Uri downloadURL = new Uri(BinsDownloadURL);

                _stopwatch.Start();
                try {
                    _webClient.DownloadFileAsync(downloadURL, BinsZipLocation);
                    //_webClient.DownloadFile(downloadURL, BinsZipLocation);
                } catch (Exception ex) {
                    Helpers.ConsolePrint("MinersDownloadManager", ex.Message);
                }
            }
        }

        #region Download delegates

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            if (!isDownloadSizeInit) {
                isDownloadSizeInit = true;
                _downloadForm.SetProgressMaxValue((int)(e.TotalBytesToReceive / 1024));
            }

            // Calculate download speed and output it to labelSpeed.
            var speedString = string.Format("{0} kb/s", (e.BytesReceived / 1024d / _stopwatch.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            //progressBar.Value = e.ProgressPercentage;

            // Show the percentage on our label.
            var percString = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            var labelDownloaded = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));

            _downloadForm.SetValueAndMsg((int)(e.BytesReceived / 1024d),
                speedString + "   " + percString + "   " + labelDownloaded);

            Helpers.ConsolePrint(TAG, speedString + "   " + percString + "   " + labelDownloaded);

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
            }
        }

        #endregion Download delegates


    }
}
