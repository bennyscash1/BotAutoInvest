namespace InvesAuto.Infra.DbService
{
    public class DicteneryInfraService
    {
        public async Task<Dictionary<string, string>> ReturnSymbolDictionary(
          string symbol, string price, string volume, string esp,
          string movingAvg50, string movingAvg200, string high52Week, string low52Week, string rsi)
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
                { "esp", esp },
                { "Dayes50MovingAvg", movingAvg50 },
                { "Dayes200MovingAvg", movingAvg200 },
                { "week52High", high52Week },
                { "week52Low", low52Week },
                { "RsiData", rsi }
            };

            return reportData; // ✅ Return the dictionary
        }

    }
}
