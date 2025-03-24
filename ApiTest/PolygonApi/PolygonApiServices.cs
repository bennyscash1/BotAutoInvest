using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvesAuto.Infra.DbService;
using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.PolygonApi

{
    public class PolygonApiServices : ApiInfraTest
    {
  
        public async Task<StockSymbolHistoryData> GetLastHistorySymbolData(string symbolName)
        {
            string polygonToken = "ppKPWMUXFqo09isu17qfPQgc8ZrJ8dDy";

            string todayDate = DateTime.Today.ToString("yyyy-MM-dd");

            string historyDate6Month = DateTime.Today.AddMonths(-6).ToString("yyyy-MM-dd");
            string polygonHistoryUrl = $"https://api.polygon.io/v2/aggs/ticker/{symbolName}/range/1/day/{historyDate6Month}/{todayDate}?apiKey={polygonToken}";
            SetUpBaseUrl(polygonHistoryUrl);
            var resonceHistory = await HttpService
            .CallWithoutBody<PolygonHistoryOutputDTO>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == resonceHistory.HttpStatus, resonceHistory.BodyString);
            await Task.Delay(1000);

            var volume = resonceHistory.Result.results[0].v;

            var historydata = new StockSymbolHistoryData
            {
                Volume = resonceHistory.Result.results[0].v,                  // 3.789326e+06
                VolumeWeightedAvgPrice = resonceHistory.Result.results[0].vw, // "m" suffix makes it a decimal
                Open = resonceHistory.Result.results[0].o,
                Close = resonceHistory.Result.results[0].c,
                High = resonceHistory.Result.results[0].h,
                Low = resonceHistory.Result.results[0].l,
                NumberOfTransactions = resonceHistory.Result.results[0].n
            };
            return historydata;
        }
        public async Task<string> GetRsiPolygonData(string symbolName)
        {
            string polygonToken = "ppKPWMUXFqo09isu17qfPQgc8ZrJ8dDy";
            string rsiUrl = $"https://api.polygon.io/v1/indicators/rsi/{symbolName}?timespan=day&apiKey={polygonToken}";
            SetUpBaseUrl(rsiUrl);
            var resonceRsi = await HttpService
            .CallWithoutBody<PolygonRsiOutputDTO>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == resonceRsi.HttpStatus, resonceRsi.BodyString);
            string rsiValue = resonceRsi.Result.results.values[0].value;
            await Task.Delay(1000);
            return rsiValue;
        }
    }
}
