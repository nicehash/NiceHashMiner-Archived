using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace NiceHashMiner.CurrencyConverter
{
    public static class CurrencyConverter
    {
        private static DateTime LastUpdate = DateTime.Now;
        private static CurrencyAPIResponse LastResponse;

        private static bool ConverterActive {
            get { return Config.ConfigData.DisplayCurrency != "USD"; }
        }


        public static double ConvertToActiveCurrency(double amount)
        {
            if (!ConverterActive)
                return amount;

            if(LastResponse == null || DateTime.Now - LastUpdate > TimeSpan.FromMinutes(10))
            {
                UpdateAPI();
            }

            // if we are still null after an update something went wrong. just use USD hopefully itll update next tick
            if (LastResponse == null)
            {
                //Helpers.ConsolePrint("CurrencyConverter", "Unable to retrieve update, Falling back to USD");
                return amount;
            }

            //Helpers.ConsolePrint("CurrencyConverter", "Current Currency: " + Config.ConfigData.DisplayCurrency);
            double usdExchangeRate = 1.0;
            if (LastResponse.rates.TryGetValue(Config.ConfigData.DisplayCurrency, out usdExchangeRate))
                return amount * usdExchangeRate;
            else
            {
                //Helpers.ConsolePrint("CurrencyConverter", "Unknown Currency Tag: " + Config.ConfigData.DisplayCurrency + " falling back to USD rates");
                Config.ConfigData.DisplayCurrency = "USD";
                return amount;
            }
        }

        private static void UpdateAPI()
        {
            try {
                var Client = new WebClient();
                var Response = Client.DownloadString("http://api.fixer.io/latest?base=USD");
                LastResponse = JsonConvert.DeserializeObject<CurrencyAPIResponse>(Response);
                LastUpdate = DateTime.Now;
            }
            catch
            {
                //Helpers.ConsolePrint("CurrencyConverter", "Unable to update API: reverting to usd");
                Config.ConfigData.DisplayCurrency = "USD";
            }
        }
    }


    

    public class CurrencyAPIResponse
    {
        public string @base { get; set; }
        public string date { get; set; }
        public Dictionary<string, double> rates { get; set; }
    }
}
