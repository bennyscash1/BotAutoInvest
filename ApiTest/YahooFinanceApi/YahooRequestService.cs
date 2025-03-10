using System;
using System.Threading.Tasks;
using YahooFinanceApi;

public class StockSymbolData
{
    public string CompanyName { get; set; }
    public string Price { get; set; }
    public string Volume { get; set; }
    public string MovingAvg50 { get; set; }
    public string MovingAvg200 { get; set; }
    public string High52Week { get; set; }
    public string Low52Week { get; set; }
    public string EPS { get; set; }
    public string MarketTime { get; set; }
    public string MarketCap { get; set; }
    public string SharesOutstanding { get; set; }
}

public class YahooRequestService
{
    public static async Task<StockSymbolData> GetStockDataAsync(string companyName)
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

            return new StockSymbolData
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
