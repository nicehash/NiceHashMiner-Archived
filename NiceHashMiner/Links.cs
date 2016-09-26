using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceHashMiner {
    public static class Links {
        public static string VisitURL = "http://www.nicehash.com?utm_source=NHM";
        // add version
        public static string VisitURLNew = "https://github.com/nicehash/NiceHashMiner/releases/tag/";
        // add btc adress as parameter
        public static string CheckStats = "http://www.nicehash.com/index.jsp?utm_source=NHM&p=miners&addr=";
        // help and faq
        public static string NHM_Help = "https://github.com/nicehash/NiceHashMiner";
        // faq
        public static string NHM_BTC_Wallet_Faq = "https://www.nicehash.com/index.jsp?utm_source=NHM&p=faq#faqs15";
        public static string NHM_Paying_Faq = "https://www.nicehash.com/index.jsp?utm_source=NHM&p=faq#faqs6";
        // API
        // btc adress as parameter
        public static string NHM_API_stats = "https://www.nicehash.com/api?method=stats.provider&addr=";
        public static string NHM_API_info = "https://www.nicehash.com/api?method=simplemultialgo.info";
        public static string NHM_API_version = "https://www.nicehash.com/nicehashminer?method=version";
        // device profits
        public static string NHM_Profit_Check = "https://www.nicehash.com/?utm_source=NHM&p=calc&name=";
    }
}
