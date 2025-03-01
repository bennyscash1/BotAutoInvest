using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using Newtonsoft.Json;
using NUnit.Framework;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.LaphaVantageApi
{
    public class AlphaVantageApiService : ApiInfraTest
    {
        #region Global Quete data
        public async Task <string> GetGlobalQueteData(string companyName)
        {
            Console.WriteLine($"The company Global Quete data: {companyName} has being load and run");
            string finvizUtl = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={companyName}&apikey=D7MHCVHXPK2H37CO";
            SetUpBaseUrl(finvizUtl);
            var responseUserProfile = await HttpService
            .CallWithoutBody<AlphaVantageGlobalQueteOutputDTO>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });

            Assert.That(HttpStatusCode.OK == responseUserProfile.HttpStatus,
                responseUserProfile.BodyString);

            string symbol = GetJsonVantageResult(AlphaVantageResponceEnum.Symbol);
            string price = GetJsonVantageResult(AlphaVantageResponceEnum.Price);
            string volume = GetJsonVantageResult(AlphaVantageResponceEnum.Volume);
            string time = GetJsonVantageResult(AlphaVantageResponceEnum.Time);
            return $"Symbol: {symbol}, Price: {price}, Volume: {volume}, Time: {time}";
        }

        public string GetJsonVantageResult(AlphaVantageResponceEnum paramType)
        {
            string jsonData = @"{
                    ""Global Quote"": {
                        ""01. symbol"": ""{companyName}"",
                        ""02. open"": ""35.9500"",
                        ""03. high"": ""36.7900"",
                        ""04. low"": ""33.3000"",
                        ""05. price"": ""33.4200"",
                        ""06. volume"": ""2064847"",
                        ""07. latest trading day"": ""2025-02-21"",
                        ""08. previous close"": ""35.2100"",
                        ""09. change"": ""-1.7900"",
                        ""10. change percent"": ""-5.0838%""
                    }
                }";
            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                JsonElement globalQuote = document.RootElement.GetProperty("Global Quote");

                // Map the enum value to the corresponding JSON property
                string returnParam = "";
                switch (paramType)
                {
                    case AlphaVantageResponceEnum.Symbol:
                        returnParam = globalQuote.GetProperty("01. symbol").GetString();
                        break;

                    case AlphaVantageResponceEnum.Price:
                        returnParam = globalQuote.GetProperty("05. price").GetString(); // Assuming "price" refers to "05. price"
                        break;

                    case AlphaVantageResponceEnum.Volume:
                        returnParam = globalQuote.GetProperty("06. volume").GetString();
                        break;

                    case AlphaVantageResponceEnum.Time:
                        returnParam = globalQuote.GetProperty("07. latest trading day").GetString();
                        break;

                    default:
                        returnParam = null; // Or throw an exception for invalid enum values
                        break;
                }
                return returnParam;
            }
        }
            public enum AlphaVantageResponceEnum
        {
            Symbol,
            Price,
            Volume,
            Time
        }
        #endregion

        #region Overview Data
        public async Task <string> GetOverviewData(string companyName)
        {
            string token = GetTestData(configDataEnum.AlphaVantageToken);
            Console.WriteLine("Get Overview global vantage data");
            string finvizUtl = $"https://www.alphavantage.co/query?function=OVERVIEW&symbol={companyName}&apikey={token}";
            SetUpBaseUrl(finvizUtl);
            var response = await HttpService
            .CallWithoutBody<AlphaVantageOverviewOutputDTO>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == response.HttpStatus, response.BodyString);


            string jsonResponse = response.BodyString;
            var data = JsonConvert.DeserializeObject<AlphaVantageOverviewOutputDTO>(jsonResponse);
            string esp = data.EPS;
            string Dayes50MovingAvg = data.MovingAverage50Day;
            string Dayes200MovingAvg = data.MovingAverage200Day;
            string week52High = data.Week52High;
            string week52Low = data.Week52Low;

            return $"esp: {esp}, Dayes50MovingAvg: {Dayes50MovingAvg}, " +
                $"Dayes200MovingAvg: {Dayes200MovingAvg}, week52High: {week52High}," +
                $"week52Low: {week52Low}, ";
        }
        #endregion
        #region Get RsI data
        public async Task<string> GetLastUpdatedRsi(string CompanyNameCsv)
        {
            string token = GetTestData(configDataEnum.AlphaVantageToken);
            Console.WriteLine("Get Rsi Alphav vantage data");
            string finvizUtl = $"https://www.alphavantage.co/query?function=RSI&symbol={CompanyNameCsv}&interval=daily&time_period=14&series_type=close&apikey={token}";
            SetUpBaseUrl(finvizUtl);
            var response = await HttpService
            .CallWithoutBody<RsiValueOutputDto>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == response.HttpStatus, response.BodyString);

            string jsonResponse = response.BodyString;
            var data = JsonConvert.DeserializeObject<RsiValueOutputDto>(jsonResponse);
            if (data?.TechnicalAnalysisRSI == null || data.TechnicalAnalysisRSI.Count == 0)
                throw new Exception("RSI data not found");

            string latestDate = data.TechnicalAnalysisRSI.Keys.FirstOrDefault();
            string rsi = data.TechnicalAnalysisRSI[latestDate].RSI;

            return rsi;
        }
        #endregion
    }
}
