using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using NiceHashMiner.Configs;

namespace NiceHashMiner
{
    public static class CurrencyConverter
    {
        private static DateTime LastUpdate = DateTime.Now;
        //private static CurrencyAPIResponse LastResponse;
        private static CurrencyAPIResponse CurrencyAPIResponse;
        // after first successful request don't fallback to USD
        private static bool IsCurrencyConverterInit = false;
        public static string ActiveDisplayCurrency = "USD";

        private static bool ConverterActive {
            get { return ConfigManager.GeneralConfig.DisplayCurrency != "USD"; }
        }


        public static double ConvertToActiveCurrency(double amount)
        {
            if (!ConverterActive)
                return amount;

            if (!IsCurrencyConverterInit || DateTime.Now - LastUpdate > TimeSpan.FromMinutes(10))
            {
                UpdateAPI();
            }

            // if we are still null after an update something went wrong. just use USD hopefully itll update next tick
            if (CurrencyAPIResponse == null || ActiveDisplayCurrency == "USD")
            {
                Helpers.ConsolePrint("CurrencyConverter", "Unable to retrieve update, Falling back to USD");
                return amount;
            }

            //Helpers.ConsolePrint("CurrencyConverter", "Current Currency: " + ConfigManager.Instance.GeneralConfig.DisplayCurrency);
            double usdExchangeRate = 1.0;
            if (CurrencyAPIResponse.rates.TryGetValue(ActiveDisplayCurrency, out usdExchangeRate))
                return amount * usdExchangeRate;
            else
            {
                Helpers.ConsolePrint("CurrencyConverter", "Unknown Currency Tag: " + ActiveDisplayCurrency + " falling back to USD rates");
                ActiveDisplayCurrency = "USD";
                return amount;
            }
        }

        private static void UpdateAPI()
        {
            try {
                var Client = new WebClient();
                var Response = Client.DownloadString("http://api.fixer.io/latest?base=USD");
                var LastResponse = JsonConvert.DeserializeObject<CurrencyAPIResponse>(Response, Globals.JsonSettings);
                LastUpdate = DateTime.Now;
                // set that we have a response
                if (LastResponse != null) {
                    IsCurrencyConverterInit = true;
                    CurrencyAPIResponse = LastResponse;
                    ActiveDisplayCurrency = ConfigManager.GeneralConfig.DisplayCurrency;
                }
            }
            catch (Exception E)
            {
                if (!IsCurrencyConverterInit) {
                    Helpers.ConsolePrint("CurrencyConverter", E.Message);
                    Helpers.ConsolePrint("CurrencyConverter", "Unable to update API: reverting to usd");
                    ActiveDisplayCurrency = "USD";
                }
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
