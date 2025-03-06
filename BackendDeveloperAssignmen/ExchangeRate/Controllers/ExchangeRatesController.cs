using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.Controllers
{
    [ApiController]
    [Route("api/exchange-rates")]
    public class ExchangeRatesController : Controller
    {
        private readonly IExchangeRateService _exchangeRateService;
        public ExchangeRatesController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }
        [HttpGet]
        public ActionResult<List<ExchangeRate>> GetAllExchangeRates()
        {
            var exchangeRates = _exchangeRateService.GetAllExchangeRates();
            if (exchangeRates != null) 
                return Ok(exchangeRates);
            return NotFound();
        }

        [HttpGet("{fromCurrency}/{toCurrency}")]
        public ActionResult<ExchangeRate> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var exchangeRate = _exchangeRateService.GetExchangeRate(fromCurrency, toCurrency);
            if (exchangeRate != null)
                return Ok(exchangeRate);
            return NotFound();
        }
    }
}
