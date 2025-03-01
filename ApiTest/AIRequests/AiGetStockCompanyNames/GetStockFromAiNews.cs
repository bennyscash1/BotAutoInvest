using InvesAuto.Infra.AiIntegrationService;
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
            string urlNews = "https://www.reuters.com/markets/us/";
            string TopStockFromAI = await openAiService.OpenAiServiceRequest(urlNews,
                OpenAiService.AiRequestType.GetStockCompanysPrompts);
            #endregion

            Match stockCountMatch = Regex.Match(TopStockFromAI, @"found (\d+) stock");
            int stockCompanySum = stockCountMatch.Success ? int.Parse(stockCountMatch.Groups[1].Value) : 0;

            // Extract stock names
            MatchCollection stockMatches = Regex.Matches(TopStockFromAI, @"\b([A-Z]+)\b");

            // Store values
            string[] stockValues = stockMatches.Cast<Match>().Select(m => m.Value).ToArray();

            // Print results
            Console.WriteLine($"Stock Company Sum: {stockCompanySum}");
            for (int i = 0; i < stockValues.Length; i++)
            {
                Console.WriteLine($"Stock Value {i + 1}: {stockValues[i]}");
            }
        }
    }
}
