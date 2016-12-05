using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.IO.Compression;
using System.Windows.Forms;
using NiceHashMiner.Interfaces;
using System.Threading;
using NiceHashMiner.Configs;
using NiceHashMiner.Devices;

namespace NiceHashMiner.Utils {
    public static class MinersDownloadManager {
        public static DownloadSetup StandardDlSetup = new DownloadSetup(
            "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_v1.7.3.10.7z",
            "bins.7z",
            "bin");

        public static DownloadSetup ThirdPartyDlSetup = new DownloadSetup(
            "https://github.com/nicehash/NiceHashMiner/releases/download/1.7.0.0-dev/bin_3rdparty_v1.7.3.10.7z",
            "bins_3rdparty.7z",
            "bin_3rdparty");

        // #1 check if miners exits
        public static bool IsMinerBinFolder() {
            return Directory.Exists(StandardDlSetup.ZipedFolderName);
        }

        static bool IsMinerBinZip() {
            return File.Exists(StandardDlSetup.BinsZipLocation);
        }

    }
}
