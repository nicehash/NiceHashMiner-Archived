using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner.Utils {
    public class DownloadSetup {
        public DownloadSetup(string url, string dlName, string inFolderName) {
            BinsDownloadURL = url;
            BinsZipLocation = dlName;
            ZipedFolderName = inFolderName;
        }
        public readonly string BinsDownloadURL;
        public readonly string BinsZipLocation;
        public readonly string ZipedFolderName;
    }
}
