using InvesAuto.ApiTest.ApiService;
using InvesAuto.ApiTest.PolygonApi;
using InvesAuto.Infra.AiIntegrationService;
using InvesAuto.Infra.DbService;
using Newtonsoft.Json;
using NUnit.Framework;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InvesAuto.Infra.AiIntegrationService.OpenAiService;

namespace InvesAuto.ApiTest.TempTest
{
    public class TempApiTest :InfraApiService
    {
        [Test]
        public async Task _TestAIResponce()
        {
            PolygonApiServices polygonApiServices = new PolygonApiServices();
            string symbol = "LMND";
            string rsiPoygonData = await polygonApiServices.GetRsiPolygonData(symbol);
            var lastHistorySymbolData = await polygonApiServices.GetLastHistorySymbolData(symbol);
            #region check the  share outstandig and average daily volume
            YahooRequestService yahooRequestService = new YahooRequestService();
            StockSymbolDataDto result = await YahooRequestService.GetStockDataAsync(symbol);
            if (result != null)
            {
                // Example of using individual properties
                companyName = result.CompanyName;
                price = result.Price;
                volume = result.Volume;
                movingAvg50 = result.MovingAvg50;
                movingAvg200 = result.MovingAvg200;
                high52Week = result.High52Week;
                low52Week = result.Low52Week;
                eps = result.EPS;
                marketTime = result.MarketTime;
                marketCap = result.MarketCap;
                sharesOutstanding = result.SharesOutstanding;
                averageDailyVolume3Month = result.AverageDailyVolume3Month;
                trailingAnnualDividendRate = result.TrailingAnnualDividendRate;
                rsi = rsiPoygonData;
                //History from polygon
                HistoryVolume = lastHistorySymbolData.Volume;
                VolumeWeightedAvgPrice = lastHistorySymbolData.VolumeWeightedAvgPrice;
                Open = lastHistorySymbolData.Open;
                Close = lastHistorySymbolData.Close;
                High = lastHistorySymbolData.High;
                Low = lastHistorySymbolData.Low;
                NumberOfTransactions = lastHistorySymbolData.NumberOfTransactions;

                bool isSharesOutstandingDiffFromaverageDailyVolume3Month = yahooRequestService
                    .isSharesOutstandingDiffFromaverageDailyVolume3Month(sharesOutstanding, averageDailyVolume3Month);

                bool istrailingAnnualDividendRateBigThen3 = yahooRequestService
                    .isTrailingAnnualDividendRateIsBig(movingAvg50, movingAvg200, trailingAnnualDividendRate);
                #endregion
                #region send it for AI
                OpenAiService openAiService = new OpenAiService();
                var responceXmlNews = await GetCnbcNewsXml(20);
                /*            string topStockFromAI = await openAiService.GetGrokResponse(responceXmlNews,
                                        AiPrePromptType.promptScanStringFromResponceNews);
                            string topStockFromOpenAI = await openAiService.OpenAiServiceRequest(responceXmlNews,
                                       AiPrePromptType.promptScanStringFromResponceNews);
                            string deepSeek = await openAiService.DeepSeekResponceAi(responceXmlNews,
                                     AiPrePromptType.promptScanStringFromResponceNews);*/
                #endregion
            }
        }
    }
}
