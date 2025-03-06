# Exchange Rate Service

## Overview
This Exchange Rate Service is a C# application that automatically fetches and stores currency exchange rates from the ExchangeRate API. It periodically retrieves rates for predefined currency pairs and saves them to a local JSON file for easy access.

## Getting Started

### Prerequisites
- .NET Framework or .NET Core runtime
- Visual Studio 2019 or later

### Running the Project
1. Clone the repository from GitHub
2. Open the solution file in Visual Studio
3. Build the solution (press F6 or use Build > Build Solution)
4. Run the project (press F5 or use Debug > Start Debugging)

## How to Use the Service

### Using the API Endpoints

#### Get All Exchange Rates
To retrieve all available exchange rates:
```
https://localhost:44379/api/exchange-rates
```

Open this URL in your browser to see all current exchange rates in JSON format.

#### Get a Specific Currency Pair
To retrieve the exchange rate for a specific currency pair:
```
https://localhost:44379/api/exchange-rates/USD/ILS
```

Replace `USD` and `ILS` with your desired currency codes. For example:
- EUR/USD: `https://localhost:44379/api/exchange-rates/EUR/USD`
- GBP/ILS: `https://localhost:44379/api/exchange-rates/GBP/ILS`

## Available Currency Pairs
The service is configured with the following currency pairs:
- USD to ILS (US Dollar to Israeli Shekel)
- EUR to ILS (Euro to Israeli Shekel)
- GBP to ILS (British Pound to Israeli Shekel)
- EUR to USD (Euro to US Dollar)
- EUR to GBP (Euro to British Pound)
