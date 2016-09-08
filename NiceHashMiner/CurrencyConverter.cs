using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using NiceHashMiner.Configs;

namespace NiceHashMiner.CurrencyConverter
{
    public static class CurrencyConverter
    {
        private static DateTime LastUpdate = DateTime.Now;
        private static CurrencyAPIResponse LastResponse;

        private static bool ConverterActive  {
            get { return ConfigManager.Instance.GeneralConfig.DisplayCurrency != "USD"; }
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
                Helpers.ConsolePrint("CurrencyConverter", "Unable to retrieve update, Falling back to USD");
                return amount;
            }

            //Helpers.ConsolePrint("CurrencyConverter", "Current Currency: " + ConfigManager.Instance.GeneralConfig.DisplayCurrency);
            double usdExchangeRate = 1.0;
            if (LastResponse.rates.TryGetValue(ConfigManager.Instance.GeneralConfig.DisplayCurrency, out usdExchangeRate))
                return amount * usdExchangeRate;
            else
            {
                Helpers.ConsolePrint("CurrencyConverter", "Unknown Currency Tag: " + ConfigManager.Instance.GeneralConfig.DisplayCurrency + " falling back to USD rates");
                ConfigManager.Instance.GeneralConfig.DisplayCurrency = "USD";
                return amount;
            }
        }

        private static void UpdateAPI()
        {
            try {
                var Client = new WebClient();
                var Response = Client.DownloadString("http://api.fixer.io/latest?base=USD");
                LastResponse = JsonConvert.DeserializeObject<CurrencyAPIResponse>(Response, Globals.JsonSettings);
                LastUpdate = DateTime.Now;
            }
            catch (Exception E)
            {
                Helpers.ConsolePrint("CurrencyConverter", E.Message);
                Helpers.ConsolePrint("CurrencyConverter", "Unable to update API: reverting to usd");
                ConfigManager.Instance.GeneralConfig.DisplayCurrency = "USD";
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
