using InvesAuto.ApiTest.ApiService;
using InvesAuto.ApiTest.FinvizApi;
using InvesAuto.ApiTest.LaphaVantageApi;
using InvesAuto.ApiTest.YahooFinanceApi;
using InvesAuto.Infra.AiIntegrationService;
using InvesAuto.Infra.DbService;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text.RegularExpressions;
using static InvesAuto.Infra.AiIntegrationService.OpenAiService;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvesAuto.ApiTest.AIRequests.AiGetStockCompanyNames
{
    [TestFixture, Category(Categories.ApiStockDataGit),
         Category(TestLevel.Level_1)]
    public class GetStockFromAiNews : InfraApiService
    {

        [Test]
        public async Task _GetStockFromAiNews()
        {
            #region Get information from news and get string 
            OpenAiService openAiService = new OpenAiService();
            //var responceList = await GetNewsApiInformationJson(50);
            string responceXmlNews = await GetCnbcNewsXml(20);
            //string jsonFormattedString = JsonConvert.SerializeObject(responceList, Formatting.Indented);
            #endregion

            #region send it for AI
            string topStockFromOpenAI = await openAiService.OpenAiServiceRequest(responceXmlNews,
                OpenAiService.AiPrePromptType.promptScanStringFromResponceNews);
            #endregion
            //DeepsSeekResponceAi
        /*    string deepSeekResponce = await openAiService.DeepSeekResponceAi(responceXmlNews,
                   AiPrePromptType.promptScanStringFromResponceNews);
            //Grok 
            string grokResponce = await openAiService.GetGrokResponse(responceXmlNews,
                  AiPrePromptType.promptScanStringFromResponceNews);*/

            #region test if the pattern is valid
            string pattern = @"Symbols:\s([\w, ]+)";
            Match match = Regex.Match(topStockFromOpenAI, pattern);
            #endregion
            if (match.Success)
            {
                #region Get symbol names from patten
                string symbolsString = match.Groups[1].Value; // Extract matched symbols
                string[] symbolsArray = symbolsString.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                MatchCollection matches = Regex.Matches(topStockFromOpenAI, pattern);
                DicteneryInfraService dicteneryInfraService = new DicteneryInfraService();
                #endregion

                bool isHaveUpdatge = true;
                // Format each symbol separately
                for (int i = 0; i < symbolsArray.Length; i++)
                {
                    Console.WriteLine($"Symbols{i + 1}: {symbolsArray[i]}");
                    string symboleName = symbolsArray[i];
                    FinvizApiService finvizApiService = new FinvizApiService();
                    bool isSymbolValid = await finvizApiService.IsSymbolValid(symboleName);
                  
                    #region update the data to mongo only if the symbol is valid and the market cap bigger then 2 bilion
                    if (isSymbolValid)
                    {
                        #region Get symbol data from yahoo
                        AlphaVantageApiService alphaVantageApiService = new AlphaVantageApiService();
                        string marketCap = await alphaVantageApiService.GetMarketCapabilityData(symboleName);

                        YahooRequestService yahooRequestService = new YahooRequestService();
                        StockSymbolDataDto resultSymbolData = await YahooRequestService.GetStockDataAsync(symboleName);
                        //marketCap = resultSymbolData.MarketCap;
                 
                        bool isMarketCapLargeAmount = YahooRequestService.IsMarketCapHaveALargeThreshold(marketCap);
                        if (isMarketCapLargeAmount)
                        {
                            DbFilterValidation dbFilterValidation = new DbFilterValidation();
                            sharesOutstanding = resultSymbolData.SharesOutstanding;
                            averageDailyVolume3Month = resultSymbolData.AverageDailyVolume3Month;
                            trailingAnnualDividendRate = resultSymbolData.TrailingAnnualDividendRate;

                            bool isAllFilterPass = await dbFilterValidation.isFilterDataValidToSaveOnDb(symboleName, marketCap,
                                sharesOutstanding, averageDailyVolume3Month, trailingAnnualDividendRate,
                                movingAvg50, movingAvg200, yahooRequestService);
                            #endregion
                            if (isAllFilterPass)
                            {
                                #region update the data to mongo
                                UpdateMongoDb updateMongoDb = new UpdateMongoDb();
                                Dictionary<string, string> reportDataName = await dicteneryInfraService
                                   .ReturnStockNameDictionary(symboleName, isHaveUpdatge.ToString());
                                await updateMongoDb
                                    .InsertOrUpdateDicteneryDataToMongo(symboleName, reportDataName,
                                MongoDbInfra.DataBaseCollection.stockCompanyList);
                                #endregion
                                Console.WriteLine($"The symbole {symboleName} was update on Db successfull");
                            }
                            else
                            {
                                Console.WriteLine($"The symbol : {symboleName} filter failed");
                            }
                            #endregion
                        }
                        else
                        {
                            Console.WriteLine($"The market cap amount: {marketCap} is less than 2 bil for symbol {symboleName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The symbole {symboleName} is not valid");
                    }

                }
            }
        }
    }
}
