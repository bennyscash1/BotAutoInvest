using InvesAuto.Infra.AiIntegrationService;
using InvesAuto.Infra.DbService;
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
    [TestFixture, Category
        (Categories.AiIntegration), Category(TestLevel.Level_2)]
    public class GetStockFromAiNews
    {

        [Test]
        public async Task _GetStockFromAiNews()
        {
            #region Get stock company names from AI
            OpenAiService openAiService = new OpenAiService();
            string urlNews = "https://www.barrons.com/market-data/stocks/stock-picks?mod=BOL_TOPNAV";
           
 /*           string topStockFromGrokAI = await openAiService.GetGrokResponse(urlNews,
               OpenAiService.AiPrePromptType.GetStockCompanysPrompts);*/

            string topStockFromAI = await openAiService.OpenAiServiceRequest(urlNews,
                OpenAiService.AiPrePromptType.GetStockCompanysPrompts);
            #endregion
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

                Dictionary<string, string> reportDataName = await dicteneryInfraService
                    .ReturnStockNameDictionary(symboleName, isHaveUpdatge.ToString());

                await updateMongoDb
                    .InsertOrUpdateDicteneryDataToMongo(symboleName, reportDataName, 
                    MongoDbInfra.DataBaseCollection.stockCompanyList);
            }
            #endregion
        }
    }
}
