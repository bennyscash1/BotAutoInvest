using InvesAuto.ApiTest.ApiService;
using InvesAuto.Infra.AiIntegrationService;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InvesAuto.Infra.AiIntegrationService.OpenAiService;

namespace InvesAuto.ApiTest.TempTest
{
    public class TempApiTest :InfraApiService
    {
        [Test]
        public async Task TestAIResponce()
        {
            OpenAiService openAiService = new OpenAiService();
            var responceList = await GetNewsInformationJson(5);
            string jsonFormattedString = JsonConvert.SerializeObject(responceList, Formatting.Indented);
            #region send it for AI

            string topStockFromAI = await openAiService.GetGrokResponse(responceList,
                        AiPrePromptType.promptScanStringFromResponceNews);
            #endregion
        }

    }
}
