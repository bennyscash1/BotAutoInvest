using InvesAuto.Infra.DbService;
using System;
using System.Threading.Tasks;
using YahooFinanceApi;



public class YahooRequestService
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
                    Field.SharesOutstanding
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
                SharesOutstanding = security[Field.SharesOutstanding]?.ToString() ?? "Null"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ An error occurred: {ex.Message}, for symbol {companyName}");
            Console.WriteLine($"🔍 StackTrace: {ex.StackTrace}");
            return null;
        }
    }

}
