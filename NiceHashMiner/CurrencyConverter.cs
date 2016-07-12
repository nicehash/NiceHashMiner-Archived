using Newtonsoft.Json;
using System;
using System.Net;

namespace NiceHashMiner.CurrencyConverter
{
    public static class CurrencyConverter
    {
        public static DateTime LastUpdate = DateTime.Now;
        public static CurrencyAPIResponse LastResponse;

        public static bool ConverterActive  {
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
                Helpers.ConsolePrint("CurrencyConverter", "Unable to retrieve update, Falling back to USD");
                return amount;
            }

            Helpers.ConsolePrint("CurrencyConverter", "Current Currency: " + Config.ConfigData.DisplayCurrency);
            switch (Config.ConfigData.DisplayCurrency)
            {
                case "AUD":
                    return amount * LastResponse.rates.AUD;
                case "BGN":
                    return amount * LastResponse.rates.BGN;
                case "BRL":
                    return amount * LastResponse.rates.BRL;
                case "CAD":
                    return amount * LastResponse.rates.CAD;
                case "CHF":
                    return amount * LastResponse.rates.CHF;
                case "CNY":
                    return amount * LastResponse.rates.CNY;
                case "CZK":
                    return amount * LastResponse.rates.CZK;
                case "DKK":
                    return amount * LastResponse.rates.DKK;
                case "GBP":
                    return amount * LastResponse.rates.GBP;
                case "HKD":
                    return amount * LastResponse.rates.HKD;
                case "HRK":
                    return amount * LastResponse.rates.HRK;
                case "HUF":
                    return amount * LastResponse.rates.HUF;
                case "IDR":
                    return amount * LastResponse.rates.IDR;
                case "ILS":
                    return amount * LastResponse.rates.ILS;
                case "INR":
                    return amount * LastResponse.rates.INR;
                case "JPY":
                    return amount * LastResponse.rates.JPY;
                case "KRW":
                    return amount * LastResponse.rates.KRW;
                case "MXN":
                    return amount * LastResponse.rates.MXN;
                case "MYR":
                    return amount * LastResponse.rates.MYR;
                case "NOK":
                    return amount * LastResponse.rates.NOK;
                case "NZD":
                    return amount * LastResponse.rates.NZD;
                case "PHP":
                    return amount * LastResponse.rates.PHP;
                case "PLN":
                    return amount * LastResponse.rates.PLN;
                case "RON":
                    return amount * LastResponse.rates.RON;
                case "RUB":
                    return amount * LastResponse.rates.RUB;
                case "SEK":
                    return amount * LastResponse.rates.SEK;
                case "SGD":
                    return amount * LastResponse.rates.SGD;
                case "THB":
                    return amount * LastResponse.rates.THB;
                case "TRY":
                    return amount * LastResponse.rates.TRY;
                case "ZAR":
                    return amount * LastResponse.rates.ZAR;
                case "EUR":
                    return amount * LastResponse.rates.EUR;
                default:
                    Helpers.ConsolePrint("CurrencyConverter", "Currency Converter. Unable to retrieve rates. Fallback to USD");
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
            catch (Exception E)
            {
                Helpers.ConsolePrint("CurrencyConverter", "Unable to update API: reverting to usd");
                Config.ConfigData.DisplayCurrency = "USD";
            }
        }
    }


    public class Rates
    {
        public double AUD { get; set; }
        public double BGN { get; set; }
        public double BRL { get; set; }
        public double CAD { get; set; }
        public double CHF { get; set; }
        public double CNY { get; set; }
        public double CZK { get; set; }
        public double DKK { get; set; }
        public double GBP { get; set; }
        public double HKD { get; set; }
        public double HRK { get; set; }
        public double HUF { get; set; }
        public double IDR { get; set; }
        public double ILS { get; set; }
        public double INR { get; set; }
        public double JPY { get; set; }
        public double KRW { get; set; }
        public double MXN { get; set; }
        public double MYR { get; set; }
        public double NOK { get; set; }
        public double NZD { get; set; }
        public double PHP { get; set; }
        public double PLN { get; set; }
        public double RON { get; set; }
        public double RUB { get; set; }
        public double SEK { get; set; }
        public double SGD { get; set; }
        public double THB { get; set; }
        public double TRY { get; set; }
        public double ZAR { get; set; }
        public double EUR { get; set; }
    }

    public class CurrencyAPIResponse
    {
        public string @base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }
    }
}
