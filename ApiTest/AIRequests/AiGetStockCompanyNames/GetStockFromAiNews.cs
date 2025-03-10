using InvesAuto.ApiTest.ApiService;
using InvesAuto.ApiTest.FinvizApi;
using InvesAuto.Infra.AiIntegrationService;
using InvesAuto.Infra.DbService;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text.RegularExpressions;
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
            var responceList = await GetNewsInformationJson(5);
            string jsonFormattedString = JsonConvert.SerializeObject(responceList, Formatting.Indented);
            #endregion
            #region send it for AI
            string topStockFromAI = await openAiService.OpenAiServiceRequest(responceList,
                OpenAiService.AiPrePromptType.promptScanStringFromResponceNews);
            #endregion

            #region test if the pattern is valid
            // Regex pattern to match words after a number and a colon
            #region test if the pattern is valid
            string pattern = @"Symbols:\s([\w, ]+)";
            Match match = Regex.Match(topStockFromAI, pattern);
            #endregion
            if (match.Success)
            {
                #region Get symbol names from patten
                string symbolsString = match.Groups[1].Value; // Extract matched symbols
                string[] symbolsArray = symbolsString.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                MatchCollection matches = Regex.Matches(topStockFromAI, pattern);
                DicteneryInfraService dicteneryInfraService = new DicteneryInfraService();
                UpdateMongoDb updateMongoDb = new UpdateMongoDb();
                #endregion

                bool isHaveUpdatge = true;
                // Format each symbol separately
                for (int i = 0; i < symbolsArray.Length; i++)
                {
                    Console.WriteLine($"Symbols{i + 1}: {symbolsArray[i]}");
                    string symboleName = symbolsArray[i];
                    FinvizApiService finvizApiService = new FinvizApiService();
                    bool isSymbolValid = await finvizApiService.IsSymbolValid(symboleName);

                    if (isSymbolValid)
                    {
                        #region update the data to mongo
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
                        Console.WriteLine($"The symbole {symboleName} is not valid");
                    }
                }
            }
        }






        #endregion
        /*        // Find matches
                    string pattern = @"\d+:\s([A-Za-z]+)";

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

                }*/

    }
}
