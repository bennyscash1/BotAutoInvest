using InvesAuto.Infra.DbService;
using InvesAuto.Infra.InfraCommonService;
using NUnit.Framework;
using YahooFinanceApi;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvesAuto.ApiTest.YahooFinanceApi
{
    [TestFixture, Category(Categories.ApiStockDataGit),
        Category(TestLevel.Level_1)]
    public class GetAndReportYhaooFinance
    {
        [Test]

        public async Task _GetAndReportYhaooFinance()
        {
            #region initial Yahoo data
            InfraFileService infraFileService = new InfraFileService();
            string currentPath = Directory.GetCurrentDirectory();
            string companyPahtFile = infraFileService.GetCompanyStockFilePath();
            string indexClumn = "A";
            int companyIndextCounter = infraFileService
                .GetCsvRowsIntValue(companyPahtFile, indexClumn);
            #endregion
            #region run on the list
            int runingAttamp = 1;

            while (runingAttamp <= companyIndextCounter)
            {
                string companyNameCsv = infraFileService.GetCsvValue(companyPahtFile, $"{indexClumn}{runingAttamp}");

                var securities = await Yahoo.Symbols(companyNameCsv)
                .Fields(
                    Field.RegularMarketPrice,       // Price
                    Field.RegularMarketVolume,      // Volume
                    Field.FiftyDayAverage,          // 50-day Moving Avg
                    Field.TwoHundredDayAverage,     // 200-day Moving Avg
                    Field.FiftyTwoWeekHigh,         // 52-week High
                    Field.FiftyTwoWeekLow,          // 52-week Low
                                                    // RSI Data (if available)
                    Field.EpsTrailingTwelveMonths,  // EPS
                    Field.RegularMarketTime         // Date & Time of last update
                ).QueryAsync();

                var security = securities[companyNameCsv];

                // Extract the values
                var date = DateTimeOffset.FromUnixTimeSeconds((long)security[Field.RegularMarketTime]).DateTime;
                var price = security[Field.RegularMarketPrice];
                var volume = security[Field.RegularMarketVolume];
                var movingAvg50 = security[Field.FiftyDayAverage];
                var movingAvg200 = security[Field.TwoHundredDayAverage];
                var high52Week = security[Field.FiftyTwoWeekHigh];
                var low52Week = security[Field.FiftyTwoWeekLow];
                var esp = security[Field.EpsTrailingTwelveMonths];
                var rsi = security[Field.RegularMarketTime];

                #region Report the responce data     
                DicteneryInfraService dicteneryInfraService = new DicteneryInfraService();
                Dictionary<string, string> reportDataService = await dicteneryInfraService.ReturnSymbolDictionary(
                     companyNameCsv,
                     price.ToString(),
                     volume.ToString(),
                     esp.ToString(),
                     movingAvg50.ToString(),
                     movingAvg200.ToString(),
                     high52Week.ToString(),
                     low52Week.ToString(),
                     rsi.ToString() // Convert everything to string
                 );
                //Add it to the mongo db
                UpdateMongoDb mongoDbService = new UpdateMongoDb();
                await mongoDbService.InsertOrUpdateDicteneryDataToMongo(companyNameCsv, reportDataService);
                //bool isUpdateSuccess = await InfraFileService.ReadAndUpdateCSVFile(reportFilePath, reportData);
                runingAttamp++;
                //Do not remove it (to not blcok)
                await Task.Delay(2000);
                Console.WriteLine("The test while as being end!!!");
                #endregion
            }
            #endregion
        }
    }
}
