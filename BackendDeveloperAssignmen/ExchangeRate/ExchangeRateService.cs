using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRate
{
    public class ExchangeRateService : IExchangeRateService
    {
        private static readonly string apiKey = "3557db96fcfb65387838263e";
        private static readonly List<FromCurrencyAndToCurrencyDto> listOfCurrency = new List<FromCurrencyAndToCurrencyDto>()
    {
        new FromCurrencyAndToCurrencyDto() {FromCurrency = "USD", ToCurrency = "ILS" },
        new FromCurrencyAndToCurrencyDto() {FromCurrency = "EUR", ToCurrency = "ILS" },
        new FromCurrencyAndToCurrencyDto() {FromCurrency = "GBP", ToCurrency = "ILS" },
        new FromCurrencyAndToCurrencyDto() {FromCurrency = "EUR", ToCurrency = "USD" },
        new FromCurrencyAndToCurrencyDto() {FromCurrency = "EUR", ToCurrency = "GBP" }
    };
        private Timer timer;
        private readonly string filePath = @"C:\exchange_rates.json";

        public void StartTimer()
        {
            int timerInterval = 10000;
            timer = new Timer(TimerCallback, null, 0, timerInterval);
        }

        private void TimerCallback(object state)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            foreach (var item in listOfCurrency)
            {
                decimal amount = 1;
                decimal? rate = GetExchangeRateFromApi(item.FromCurrency, item.ToCurrency);

                if (rate.HasValue && rate.Value > 0)
                {
                    decimal convertedAmount = amount * rate.Value;
                    exchangeRates.Add(new ExchangeRate()
                    {
                        PairName = item.FromCurrency + "/" + item.ToCurrency,
                        LastUpdated = DateTime.Now,
                        Rate = convertedAmount
                    });
                }
                else
                {
                    Console.WriteLine("Failed to convert amount");
                }
            }

            string json = JsonConvert.SerializeObject(exchangeRates, Formatting.Indented);
            WriteJsonToFile(filePath, json);
        }

        private decimal? GetExchangeRateFromApi(string fromCurrency, string toCurrency)
        {
            // Your existing GetExchangeRate method code
            string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/pair/{fromCurrency}/{toCurrency}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string json = client.GetStringAsync(url).Result;
                    if (json != null)
                    {
                        try
                        {
                            JObject data = JObject.Parse(json);
                            if (data["result"].ToString() == "success")
                            {
                                decimal rate = (decimal)data["conversion_rate"];
                                return rate;
                            }
                            else
                            {
                                Console.WriteLine($"Exception: {data["error-type"]}");
                                return null;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failed to convert json");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The service GetStringAsync did not return anything");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private void WriteJsonToFile(string filePath, string json)
        {
            try
            {
                if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
                {
                    File.WriteAllText(filePath, string.Empty);
                }

                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<ExchangeRate> GetAllExchangeRates()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    if (!string.IsNullOrEmpty(json))
                    {
                        return JsonConvert.DeserializeObject<List<ExchangeRate>>(json);
                    }
                }
                return new List<ExchangeRate>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ExchangeRate>();
            }
        }

        public ExchangeRate GetExchangeRate(string fromCurrency, string toCurrency)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var listOfExchangeRate = JsonConvert.DeserializeObject<List<ExchangeRate>>(json);
                        return listOfExchangeRate?.FirstOrDefault(rate =>
                            rate.PairName == string.Format(@"{0}/{1}", fromCurrency, toCurrency));
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
