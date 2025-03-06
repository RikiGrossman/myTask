using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRate
{
    public class Program
    {
        //builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();


        private static readonly string apiKey = "3557db96fcfb65387838263e";
        private static readonly List<FromCurrencyAndToCurrencyDto> listOfCurrency = new List<FromCurrencyAndToCurrencyDto>()
        {
            new FromCurrencyAndToCurrencyDto() {FromCurrency = "USD", ToCurrency = "ILS" },
            new FromCurrencyAndToCurrencyDto() {FromCurrency = "EUR", ToCurrency = "ILS" },
            new FromCurrencyAndToCurrencyDto() {FromCurrency = "GBP", ToCurrency = "ILS" },
            new FromCurrencyAndToCurrencyDto() {FromCurrency = "EUR", ToCurrency = "USD" },
            new FromCurrencyAndToCurrencyDto() {FromCurrency = "EUR", ToCurrency = "GBP" }
        };
        private static Timer timer;
        private static int timerInterval;
        private static string filePath = @"C:\exchange_rates.json";


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IExchangeRateService, ExchangeRateService>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            var exchangeRateService = app.Services.GetRequiredService<IExchangeRateService>();
            exchangeRateService.StartTimer();

            app.Run();
        }    
    }
}

