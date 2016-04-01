using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NiceHashMiner
{
#pragma warning disable 649
    public class NiceHashSMA
    {
        public int port;
        public string name;
        public int algo;
        public double paying;
    }
#pragma warning restore 649

    class NiceHashStats
    {
#pragma warning disable 649
        class nicehash_global_stats
        {
            public double profitability_above_ltc;
            public double price;
            public double profitability_ltc;
            public int algo;
            public double speed;
        }

        public class nicehash_stats
        {
            public double balance;
            public double balance_unexchanged;
            public double balance_immature;
            public double balance_confirmed;
            public double accepted_speed;
            public double rejected_speed;
            public int algo;
        }

        public class nicehash_result_2
        {
            public NiceHashSMA[] simplemultialgo;
        }

        public class nicehash_json_2
        {
            public nicehash_result_2 result;
            public string method;
        }

        class nicehash_result<T>
        {
            public T[] stats;
        }
        
        class nicehash_json<T>
        {
            public nicehash_result<T> result;
            public string method;
        }

        class nicehash_error
        {
            public string error;
            public string method;
        }

        class nicehashminer_version
        {
            public string version;
        }
#pragma warning restore 649


        public static NiceHashSMA[] GetAlgorithmRates(string worker)
        {
            string r1 = GetNiceHashAPIData("https://www.nicehash.com/api?method=simplemultialgo.info", worker);
            if (r1 == null) return null;

            nicehash_json_2 nhjson_current;
            try
            {
                nhjson_current = JsonConvert.DeserializeObject<nicehash_json_2>(r1);
                return nhjson_current.result.simplemultialgo;
            }
            catch
            {
                return null;
            }
        }


        public static nicehash_stats GetStats(string btc, int location, int algo, string worker)
        {
            if (location > 1) location = 1;
            string r1 = GetNiceHashAPIData("https://www.nicehash.com/api?method=stats.provider&location=" + location.ToString() + "&addr=" + btc, worker);
            if (r1 == null) return null;

            nicehash_json<nicehash_stats> nhjson_current;
            try
            {
                nhjson_current = JsonConvert.DeserializeObject<nicehash_json<nicehash_stats>>(r1);
                for (int i = 0; i < nhjson_current.result.stats.Length; i++)
                {
                    if (nhjson_current.result.stats[i].algo == algo)
                        return nhjson_current.result.stats[i];
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        public static double GetBalance(string btc, string worker)
        {
            double balance = 0;

            for (int l = 0; l < 2; l++)
            {
                string r1 = GetNiceHashAPIData("https://www.nicehash.com/api?method=stats.provider&location=" + l.ToString() + "&addr=" + btc, worker);
                if (r1 == null) break;

                nicehash_json<nicehash_stats> nhjson_current;
                try
                {
                    nhjson_current = JsonConvert.DeserializeObject<nicehash_json<nicehash_stats>>(r1);
                    for (int i = 0; i < nhjson_current.result.stats.Length; i++)
                    {
                        if (nhjson_current.result.stats[i].algo != 999)
                        {
                            balance += nhjson_current.result.stats[i].balance;
                        }
                        else if (nhjson_current.result.stats[i].algo == 999 && l == 0)
                        {
                            balance += nhjson_current.result.stats[i].balance_confirmed;
                        }
                    }
                }
                catch
                {
                    break;
                }
            }

            return balance;
        }


        public static string GetVersion(string worker)
        {
            string r1 = GetNiceHashAPIData("https://www.nicehash.com/nicehashminer?method=version", worker);
            if (r1 == null) return null;

            nicehashminer_version nhjson;
            try
            {
                nhjson = JsonConvert.DeserializeObject<nicehashminer_version>(r1);
                return nhjson.version;
            }
            catch
            {
                return null;
            }
        }


        public static string GetNiceHashAPIData(string URL, string worker)
        {
            string ResponseFromServer;
            try
            {
                HttpWebRequest WR = (HttpWebRequest)WebRequest.Create(URL);
                WR.UserAgent = "NiceHashMiner/" + Application.ProductVersion;
                if (worker.Length > 64) worker = worker.Substring(0, 64);
                WR.Headers.Add("NiceHash-Worker-ID", worker);
                WR.Timeout = 10000;
                WebResponse Response = WR.GetResponse();
                Stream SS = Response.GetResponseStream();
                SS.ReadTimeout = 10000;
                StreamReader Reader = new StreamReader(SS);
                ResponseFromServer = Reader.ReadToEnd();
                if (ResponseFromServer.Length == 0 || ResponseFromServer[0] != '{')
                    throw new Exception("Not JSON!");
                Reader.Close();
                Response.Close();
            }
            catch (Exception ex)
            {
                Helpers.ConsolePrint("NICEHASH", ex.Message);
                return null;
            }

            return ResponseFromServer;
        }
    }
}
