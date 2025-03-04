using InvesAuto.ApiTest.ApiService;
using InvesAuto.ApiTest.FinvizApi;
using InvesAuto.Infra.AiIntegrationService;
using InvesAuto.Infra.DbService;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvesAuto.ApiTest.AIRequests.AiGetStockCompanyNames
{
    [TestFixture, Category(Categories.ApiStockDataGit),
         Category(TestLevel.Level_1)]
    public class GetStockFromAiNews :InfraApiService
    {

        [Test]
        public async Task _GetStockFromAiNews()
        {
            #region Get stock company names from AI api news for local
            /*
                        var newsArticle = await GetNewsInformationJson(3);
                        string formattedArticles = string.Join("\n\n---\n\n", newsArticle);

                        string formattedArticlesJson = JsonConvert.SerializeObject(newsArticle, Formatting.Indented);*/
            #endregion



            /*           string topStockFromGrokAI = await openAiService.GetGrokResponse(urlNews,
                          OpenAiService.AiPrePromptType.GetStockCompanysPrompts);*/

            OpenAiService openAiService = new OpenAiService();
            string urlNews = "https://www.barrons.com/market-data/stocks/stock-picks?mod=BOL_TOPNAV";
            string topStockFromAI = await openAiService.OpenAiServiceRequest(urlNews,
                OpenAiService.AiPrePromptType.GetStockCompanysPrompts);
            // Regex pattern to match words after a number and a colon
            string pattern = @"\d+:\s([A-Za-z]+)";
            // Find matches
            MatchCollection matches = Regex.Matches(topStockFromAI, pattern);
            DicteneryInfraService dicteneryInfraService = new DicteneryInfraService();
            UpdateMongoDb updateMongoDb = new UpdateMongoDb();
            bool isHaveUpdatge = true;
            #region Get stock company names from AI
            foreach (Match match in matches)
            {
                Console.WriteLine(match.Groups[1].Value);
                var x = match.Groups[1];
                string symboleName = x.Value;

                #region Test if symbol from ai is valid
                FinvizApiService finvizApiService = new FinvizApiService();
                bool isSymbolValid = await finvizApiService.IsSymbolValid(symboleName);
                #endregion
                if (isSymbolValid)
                {
                    Dictionary<string, string> reportDataName = await dicteneryInfraService
                    .ReturnStockNameDictionary(symboleName, isHaveUpdatge.ToString());

                    await updateMongoDb
                        .InsertOrUpdateDicteneryDataToMongo(symboleName, reportDataName,
                        MongoDbInfra.DataBaseCollection.stockCompanyList);
                    Console.WriteLine($"The symbole {symboleName} was update on Db successfull");
                }
                else
                {
                    Console.WriteLine($"The symbole {symboleName} is not valid");
                }

            }
            #endregion
        }
    }
}
