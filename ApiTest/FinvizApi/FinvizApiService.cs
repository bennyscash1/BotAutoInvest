using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.FinvizApi
{
    public class FinvizApiService : ApiInfraTest
    {
        public async Task<bool> IsSymbolValid(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return false;
            }
            string finvizUrl = $"https://finviz.com/api/etf_holdings/{symbol}/top_ten";
            SetUpBaseUrl(finvizUrl);
            var resonceFinvizIsSymbolValid = await HttpService
            .CallWithoutBody<FinvisOutputDto>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == resonceFinvizIsSymbolValid.HttpStatus,
            resonceFinvizIsSymbolValid.BodyString);
            //Need to wait before get to the next request
            await Task.Delay(3000);
            var responseUserProfileBody = resonceFinvizIsSymbolValid.BodyString;
        
            if (responseUserProfileBody.Contains("\"rowData\": []") )
            {
                return false;
            }
            else
            {
                return true;
            }
          
        }
        public static async Task<string> GetRsiFromFinviz(string ticker)
        {
            string url = $"https://finviz.com/api/quote.ashx?aftermarket=false&dateFrom=1706713200&dateTo=1740754799&events=false&financialAttachments=&instrument=stock&patterns=false&premarket=false&rev=1739706540851&ticker={ticker}&timeframe=d";

            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);
            JObject data = JObject.Parse(response);

            if (data["close"] == null || data["close"].Count() < 15)
                throw new Exception("Not enough closing prices to calculate RSI.");

            var closePrices = data["close"].Select(c => (double)c).ToArray();
            var changes = closePrices.Skip(1).Zip(closePrices, (current, previous) => current - previous).ToArray();

            var gains = changes.Select(x => x > 0 ? x : 0).ToArray();
            var losses = changes.Select(x => x < 0 ? -x : 0).ToArray();

            int period = 14;
            double avgGain = gains.Take(period).Average();
            double avgLoss = losses.Take(period).Average();

            for (int i = period; i < gains.Length; i++)
            {
                avgGain = (avgGain * (period - 1) + gains[i]) / period;
                avgLoss = (avgLoss * (period - 1) + losses[i]) / period;
            }

            double rs = avgLoss == 0 ? double.MaxValue : avgGain / avgLoss;
            double rsi = 100 - (100 / (1 + rs));

            return Math.Round(rsi, 2).ToString();
        }

    }
}
