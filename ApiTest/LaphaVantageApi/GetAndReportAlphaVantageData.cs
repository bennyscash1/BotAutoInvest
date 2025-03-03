using InvesAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.LaphaVantageApi;
using InvesAuto.Infra.DbService;
using InvesAuto.Infra.InfraCommonService;
using NUnit.Framework;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvestAuto.ApiTest.LaphaVantageApi
{

    public class GetAndReportAlphaVantageData : ApiInfraTest
    {
        [Test]
        public async Task _GetAndReportAlphaVantageData()
        {
            #region Read the compnay list file
            Console.WriteLine("Test start!!!!!!!!!!!!");
            InfraFileService infraFileService = new InfraFileService();   
            string companyPahtFile = infraFileService.GetCompanyStockFilePath();
            string indexClumn = "A";
            int companyIndextCounter = infraFileService
                .GetCsvRowsIntValue(companyPahtFile, indexClumn);

            Console.WriteLine($"The company counter list is: {companyIndextCounter}");
            #endregion

            int runingAttamp = 1;

            while (runingAttamp <= companyIndextCounter)
            {
                #region Get Global Quete Data
                string companyNameCsv = infraFileService.GetCsvValue(companyPahtFile, $"{indexClumn}{runingAttamp}");
                AlphaVantageApiService alphaVantageApiService = new AlphaVantageApiService();
                string currentPath = Directory.GetCurrentDirectory();

                string globalQuoteData = await alphaVantageApiService.GetGlobalQueteData(companyNameCsv);
                string[] globalQuoteParts = globalQuoteData.Split(',');
                string symbol = companyNameCsv;
                string price = globalQuoteParts[1].Split(':')[1].Trim();
                string volume = globalQuoteParts[2].Split(':')[1].Trim();
                string time = globalQuoteParts[3].Split(':')[1].Trim();
                #endregion

                #region Update Overview data
                string overViewData = await alphaVantageApiService.GetOverviewData(companyNameCsv);

                string[] overViewDataParts = overViewData.Split(',');

                string esp = overViewDataParts[0].Split(':')[1].Trim();
                string Dayes50MovingAvg = overViewDataParts[1].Split(':')[1].Trim();
                string Dayes200MovingAvg = overViewDataParts[2].Split(':')[1].Trim();
                string week52High = overViewDataParts[3].Split(':')[1].Trim();
                string week52Low = overViewDataParts[4].Split(':')[1].Trim();
                #endregion

                #region Get RSI data
                string RsiData = await alphaVantageApiService
                    .GetLastUpdatedRsi(companyNameCsv);
                #endregion

                #region Report the responce data
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                DateTime israelTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, israelTimeZone);


                string reportFilePath = Path.Combine(currentPath, "GeneralFiles", "AlphaVantage", $"{companyNameCsv}.csv");
                Console.WriteLine("Report to alpha vantage has open");
                Dictionary<string, string> reportData = new Dictionary<string, string>
                {
                    { "Date", $"{israelTime}" },
                    { "symbol", $"{companyNameCsv}" },
                    { "price", $"{price}" },
                    { "volume", $"{volume}" },
                    { "esp", $"{esp}" },
                    { "Dayes50MovingAvg", $"{Dayes50MovingAvg}" },
                    { "Dayes200MovingAvg", $"{Dayes200MovingAvg}" },
                    { "week52High", $"{week52High}" },
                    { "week52Low", $"{week52Low}" },
                    { "RsiData", $"{RsiData}" }
                };
                UpdateMongoDb mongoDbService = new UpdateMongoDb();
                await mongoDbService.InsertOrUpdateDicteneryDataToMongo(companyNameCsv, reportData);
                //bool isUpdateSuccess = await InfraFileService.ReadAndUpdateCSVFile(reportFilePath, reportData);
                runingAttamp++;
                Console.WriteLine("The test while as being end!!!");
                #endregion

            }
     

        }
   

    }

}
