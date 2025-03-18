namespace InvesAuto.Infra.DbService
{
    public class DicteneryInfraService
    {
        public async Task<Dictionary<string, string>> ReturnStockDataDictionary(
          string symbol, string price, string volume, string eps,
          string movingAvg50, string movingAvg200, string high52Week, string low52Week, string rsi ,
          string marketTime, string marketCap, string sharesOutstanding,
          string averageDailyVolume3Month, string trailingAnnualDividendRate, string futurePrice, string label)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            DateTime israelTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, israelTimeZone);

            Console.WriteLine("Report to alpha vantage has opened");

            Dictionary<string, string> reportData = new Dictionary<string, string>
            {
                { "Date", israelTime.ToString() },
                { "symbol", symbol },
                { "price", price },
                { "volume", volume },
                { "eps", eps },  // Fixed typo from "esp" to "eps"
                { "Dayes50MovingAvg", movingAvg50 },
                { "Dayes200MovingAvg", movingAvg200 },
                { "week52High", high52Week },
                { "week52Low", low52Week },
                { "RsiData", rsi },
                { "MarketTime", marketTime }, // Added missing Market Time
                { "MarketCap", marketCap },   // Added missing Market Cap
                { "AverageDailyVolume3Month", averageDailyVolume3Month }, // Added missing Shares Outstanding
                { "TrailingAnnualDividendRate", trailingAnnualDividendRate }, // Added missing Shares Outstanding
                { "FuturePrice", futurePrice }, // Added missing Shares Outstanding
                { "Label", label } // Added missing Shares Outstanding
            };

            return reportData; // ✅ Return the dictionary
        }

        public async Task<Dictionary<string, string>> ReturnStockNameDictionary(
                string symbol, string isHaveUpdatge)

        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
            DateTime israelTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, israelTimeZone);

            Console.WriteLine("Report to stock company list");

            Dictionary<string, string> reportData = new Dictionary<string, string>
            {
                { "Date", israelTime.ToString() },
                { "symbol", symbol },
                { "isHaveUpdatge", isHaveUpdatge },          
            };

            return reportData; // ✅ Return the dictionary
        }
    }
}
