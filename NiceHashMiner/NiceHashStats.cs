using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
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
            public double accepted_speed;
            public double rejected_speed;
            public int algo;
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
#pragma warning restore 649

        public static double GetAlgorithmRate(int algo)
        {
            string r1 = GetNiceHashAPIData("https://www.nicehash.com/api?method=stats.global.current");
            if (r1 == null) return 0;

            nicehash_json<nicehash_global_stats> nhjson_current;
            try
            {
                nhjson_current = JsonConvert.DeserializeObject<nicehash_json<nicehash_global_stats>>(r1);
                for (int i = 0; i < nhjson_current.result.stats.Length; i++)
                {
                    if (nhjson_current.result.stats[i].algo == algo)
                        return nhjson_current.result.stats[i].price;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static nicehash_stats GetStats(string btc, int location, int algo)
        {
            if (location > 1) location = 1;
            string r1 = GetNiceHashAPIData("https://www.nicehash.com/api?method=stats.provider&location=" + location.ToString() + "&addr=" + btc);
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

        private static string GetNiceHashAPIData(string URL)
        {
            string ResponseFromServer;
            try
            {
                HttpWebRequest WR = (HttpWebRequest)WebRequest.Create(URL);
                WR.Timeout = 5000;
                WebResponse Response = WR.GetResponse();
                Stream SS = Response.GetResponseStream();
                SS.ReadTimeout = 5000;
                StreamReader Reader = new StreamReader(SS);
                ResponseFromServer = Reader.ReadToEnd();
                if (ResponseFromServer.Length == 0 || ResponseFromServer[0] != '{')
                    throw new Exception("Not JSON!");
                Reader.Close();
                Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[" + DateTime.Now.ToString() + "] JSONStats - " + ex.Message);
                return null;
            }

            return ResponseFromServer;
        }
    }
}
