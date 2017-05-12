using Newtonsoft.Json;
using NiceHashMiner.Configs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NiceHashMiner {
    class ExchangeRateAPI {
        public class Result {
            public Object algorithms { get; set; }
            public Object servers { get; set; }
            public Object idealratios { get; set; }
            public List<Dictionary<string, string>> exchanges { get; set; }
            public Dictionary<string, double> exchanges_fiat { get; set; }
        }

        public class ExchangeRateJSON {
            public Result result { get; set; }
            public string method { get; set; }
        }

        const string apiUrl = "https://api.nicehash.com/api?method=nicehash.service.info";

        private static Dictionary<string, double> exchanges_fiat = null;
        private static double USD_BTC_rate = -1;
        public static string ActiveDisplayCurrency = "USD";

        private static bool ConverterActive {
            get { return ConfigManager.GeneralConfig.DisplayCurrency != "USD"; }
        }


        public static double ConvertToActiveCurrency(double amount) {
            if (!ConverterActive) {
                return amount;
            }

            // if we are still null after an update something went wrong. just use USD hopefully itll update next tick
            if (exchanges_fiat == null || ActiveDisplayCurrency == "USD") {
                Helpers.ConsolePrint("CurrencyConverter", "Unable to retrieve update, Falling back to USD");
                return amount;
            }

            //Helpers.ConsolePrint("CurrencyConverter", "Current Currency: " + ConfigManager.Instance.GeneralConfig.DisplayCurrency);
            double usdExchangeRate = 1.0;
            if (exchanges_fiat.TryGetValue(ActiveDisplayCurrency, out usdExchangeRate))
                return amount * usdExchangeRate;
            else {
                Helpers.ConsolePrint("CurrencyConverter", "Unknown Currency Tag: " + ActiveDisplayCurrency + " falling back to USD rates");
                ActiveDisplayCurrency = "USD";
                return amount;
            }
        }

        public static double GetUSDExchangeRate() {
            if (USD_BTC_rate > 0) {
                return USD_BTC_rate;
            }
            return 0.0;
        }

        public static void UpdateAPI(string worker) {
            string resp = NiceHashStats.GetNiceHashAPIData(apiUrl, worker);
            if (resp != null) {
                try {
                    var LastResponse = JsonConvert.DeserializeObject<ExchangeRateJSON>(resp, Globals.JsonSettings);
                    // set that we have a response
                    if (LastResponse != null) {
                        Result last_result = LastResponse.result;
                        ActiveDisplayCurrency = ConfigManager.GeneralConfig.DisplayCurrency;
                        exchanges_fiat = last_result.exchanges_fiat;
                        // ActiveDisplayCurrency = "USD";
                        // check if currency avaliable and fill currency list
                        foreach (var pair in last_result.exchanges) {
                            if (pair.ContainsKey("USD") && pair.ContainsKey("coin") && pair["coin"] == "BTC" && pair["USD"] != null) {
                                USD_BTC_rate = Helpers.ParseDouble(pair["USD"]);
                                break;
                            }
                        }
                    }
                } catch(Exception e) {
                    Helpers.ConsolePrint("ExchangeRateAPI", "UpdateAPI got Exception: " + e.Message);
                }
            } else {
                Helpers.ConsolePrint("ExchangeRateAPI", "UpdateAPI got NULL");
            }
        }

    }
}
