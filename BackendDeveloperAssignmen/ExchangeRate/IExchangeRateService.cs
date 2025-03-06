namespace ExchangeRate
{
    public interface IExchangeRateService
    {
        List<ExchangeRate> GetAllExchangeRates();
        ExchangeRate GetExchangeRate(string fromCurrency, string toCurrency);
        void StartTimer();
    }
}
