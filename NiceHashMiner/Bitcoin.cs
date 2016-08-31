using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    class Bitcoin
    {
#pragma warning disable 649
        class CoinbaseResponse
        {
            public double amount;
            public string currency;
        }
#pragma warning restore 649

        public static double GetUSDExchangeRate()
        {
            string jsondata = null;

            for (int i = 0; i < 2; i++)
            {
                jsondata = GetCoinbaseAPIData("https://api.coinbase.com/v1/prices/spot_rate");
                if (jsondata != null) break;
            }

            if (jsondata == null) return 0;

            try
            {
                CoinbaseResponse cbr = JsonConvert.DeserializeObject<CoinbaseResponse>(jsondata, Globals.JsonSettings);
                return cbr.amount;
            }
            catch
            {
                return 0;
            }
        }

        private static string GetCoinbaseAPIData(string URL)
        {
            string ResponseFromServer;
            try
            {
                HttpWebRequest WR = (HttpWebRequest)WebRequest.Create(URL);
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
                Helpers.ConsolePrint("COINBASE", ex.Message);
                return null;
            }

            return ResponseFromServer;
        }
    }
}
