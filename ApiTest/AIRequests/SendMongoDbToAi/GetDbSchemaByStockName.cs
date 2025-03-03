using InvesAuto.Infra.AiIntegrationService;
using InvesAuto.Infra.DbService;
using InvesAuto.Infra.InfraCommonService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvesAuto.ApiTest.AIRequests.SendMongoDbToAi
{
    [TestFixture, Category(Categories.AiIntegration),
        Category(TestLevel.Level_2)]
    public class GetDbSchemaByStockName
    {
        [Test]
        public async Task _GetDbSchemaByStockName()
        {
  
            // Get the schema from the database
            #region Get company name from the file
            InfraFileService infraFileService = new InfraFileService();
            string companyPahtFile = infraFileService.GetCompanyStockFilePath();
            string indexClumn = "A";
            // runingAttamp shult be attamp using while loop
            int runingAttamp = 1;
      
            int companyIndextCounter = infraFileService
                .GetCsvRowsIntValue(companyPahtFile, indexClumn);
            #endregion

            #region Send the schema to AI for each stock company
            while (runingAttamp <= companyIndextCounter)
            {
                string companyNameFromCsv = infraFileService.GetCsvValue(companyPahtFile, $"{indexClumn}{runingAttamp}");
                #region Get the schema from the database
                GetMongoDb getMongoDbData = new GetMongoDb();
                var schemaByCompnayName = await getMongoDbData
                    .GetStockDataBySymbol(companyNameFromCsv);
                #endregion

                #region Send the DB to analytic code
                int stockAnlyticksValue =await getMongoDbData
                    .AnalyzeStockBySymbol(companyNameFromCsv);
                #endregion

                #region Send the schema to AI
                OpenAiService openAiService = new OpenAiService();
                string aiAnalyticsRespones = await openAiService.OpenAiServiceRequest(schemaByCompnayName,
                    OpenAiService.AiRequestType.DataBaseAnalyst);
                #endregion
                if (aiAnalyticsRespones!="2")
                {
                    Console.WriteLine($"The company{companyNameFromCsv} It's worth looking at.");
                    //send sms
                   /* string currentPath = Directory.GetCurrentDirectory();
                    string reportFilePath = Path.Combine(currentPath, "GeneralFiles", "AlphaVantage", $"{companyNameCsv}.csv");
                    bool isUpdateSuccess = await InfraFileService.ReadAndUpdateCSVFile(reportFilePath, reportData);*/
                }
                else
                {
                    Console.WriteLine("");
                }
                runingAttamp++;
            }
            #endregion
        }
    }
}
