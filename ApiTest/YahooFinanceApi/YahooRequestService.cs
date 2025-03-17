using InvesAuto.ApiTest.ApiService;
using InvesAuto.Infra.DbService;
using System;
using System.Globalization;
using System.Threading.Tasks;
using YahooFinanceApi;



public class YahooRequestService : InfraApiService
{


    public static async Task<StockSymbolDataDto> GetStockDataAsync(string companyName)
    {
        try
        {
            var securities = await Yahoo.Symbols(companyName)
                .Fields(
                    Field.RegularMarketPrice,
                    Field.RegularMarketVolume,
                    Field.FiftyDayAverage,
                    Field.TwoHundredDayAverage,
                    Field.FiftyTwoWeekHigh,
                    Field.FiftyTwoWeekLow,
                    Field.EpsTrailingTwelveMonths,
                    Field.RegularMarketTime,
                    Field.MarketCap,
                    Field.SharesOutstanding,
                    Field.AverageDailyVolume3Month,
                    Field.TrailingAnnualDividendRate
                )
                .QueryAsync();

            if (!securities.ContainsKey(companyName))
                return null;

            var security = securities[companyName];

            return new StockSymbolDataDto
            {
                CompanyName = companyName,
                Price = security[Field.RegularMarketPrice]?.ToString() ?? "Null",
                Volume = security[Field.RegularMarketVolume]?.ToString() ?? "Null",
                MovingAvg50 = security[Field.FiftyDayAverage]?.ToString() ?? "Null",
                MovingAvg200 = security[Field.TwoHundredDayAverage]?.ToString() ?? "Null",
                High52Week = security[Field.FiftyTwoWeekHigh]?.ToString() ?? "Null",
                Low52Week = security[Field.FiftyTwoWeekLow]?.ToString() ?? "Null",
                EPS = security[Field.EpsTrailingTwelveMonths]?.ToString() ?? "Null",
                MarketTime = security[Field.RegularMarketTime]?.ToString() ?? "Null",
                MarketCap = security[Field.MarketCap]?.ToString() ?? "Null",
                SharesOutstanding = security[Field.SharesOutstanding]?.ToString() ?? "Null",
                AverageDailyVolume3Month = security[Field.AverageDailyVolume3Month]?.ToString() ?? "Null",
                TrailingAnnualDividendRate = security[Field.TrailingAnnualDividendRate]?.ToString() ?? "Null"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ An error occurred: {ex.Message}, for symbol {companyName}");
            Console.WriteLine($"🔍 StackTrace: {ex.StackTrace}");
            return null;
        }
    }
    public static bool IsMarketCapHaveALargeThreshold(string marketCapStr, long threshold = 2_000_000_000)
    {
        if (long.TryParse(marketCapStr, out long marketCap))
        {
            return marketCap > threshold;
        }
        return false; // Return false if conversion fails
    }
    public bool isSharesOutstandingDiffFromaverageDailyVolume3Month(
      string sharesOutstanding,
      string averageDailyVolume3Month,
      int diffrentPercentage = 2)
    {
        if (!double.TryParse(sharesOutstanding, out double sharesOutstandingVal) ||
            !double.TryParse(averageDailyVolume3Month, out double averageDailyVolumeVal))
        {
            Console.WriteLine($"Invalid or missing input. sharesOutstanding: {sharesOutstanding}, averageDailyVolume3Month: {averageDailyVolume3Month}");
            return false;
        }

        double difference = Math.Abs(sharesOutstandingVal - averageDailyVolumeVal);
        double percentageDifference = (difference / sharesOutstandingVal) * 100;

        return percentageDifference >= diffrentPercentage;
    }

    public bool isTrailingAnnualDividendRateIsBig(string dayes50MovingAvg, string dayes200MovingAvg,
        string trailingAnnualDividendRate, int defultDividendRate =3)
    {
        if (string.IsNullOrEmpty(dayes50MovingAvg) || string.IsNullOrEmpty(dayes200MovingAvg) ||
            string.IsNullOrEmpty(trailingAnnualDividendRate))
        {
            Console.WriteLine($"dayes50MovingAvg value: {dayes50MovingAvg}, dayes200MovingAvg value: {dayes200MovingAvg}, trailingAnnualDividendRate value: {trailingAnnualDividendRate}");
            return false;
        }
        decimal dayes50MovingNumber = decimal.Parse(dayes50MovingAvg, CultureInfo.InvariantCulture);
        decimal dayes200MovingNumber = decimal.Parse(dayes200MovingAvg, CultureInfo.InvariantCulture);
        decimal intDividendRate = decimal.Parse(trailingAnnualDividendRate, CultureInfo.InvariantCulture);

        if (dayes50MovingNumber > dayes200MovingNumber)
        {
            return true;
        }
        else
        {
            if (intDividendRate >= defultDividendRate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
