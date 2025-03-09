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
       /*     InfraFileService infraFileService = new InfraFileService();
            string currentPath = Directory.GetCurrentDirectory();
            string companyPahtFile = infraFileService.GetCompanyStockFilePath();
            string indexClumn = "A";
            int companyIndextCounter = infraFileService
                .GetCsvRowsIntValue(companyPahtFile, indexClumn);*/

            #region Get symbol data from db
            GetMongoDb getMongoDbDTO = new GetMongoDb(MongoDbInfra.DataBaseCollection.stockCompanyList);
            var symbolList = await getMongoDbDTO
                .GetStockListFromDB();
            int dbStockCount = symbolList.Count();
            #endregion

            int runingAttamp = 0;
            UpdateMongoDb mongoDbService = new UpdateMongoDb();

            while (runingAttamp < dbStockCount)
            {
                //string companyNameCsv = infraFileService.GetCsvValue(companyPahtFile, $"{indexClumn}{runingAttamp}");

                string companyNameCsv = "";
                // Extract the values
                try
                {
                    
                    companyNameCsv = symbolList[runingAttamp];
                    Console.WriteLine($"The symbol value is: {companyNameCsv}");
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
                    string price = security[Field.RegularMarketPrice]?.ToString() ?? "Null";
                    string volume = security[Field.RegularMarketVolume]?.ToString() ?? "Null";
                    string movingAvg50 = security[Field.FiftyDayAverage]?.ToString() ?? "Null";
                    string movingAvg200 = security[Field.TwoHundredDayAverage]?.ToString() ?? "Null";
                    string high52Week = security[Field.FiftyTwoWeekHigh]?.ToString() ?? "Null";
                    string low52Week = security[Field.FiftyTwoWeekLow]?.ToString() ?? "Null";
                    string eps = security[Field.EpsTrailingTwelveMonths]?.ToString() ?? "Null";
                    string rsi = security[Field.RegularMarketTime]?.ToString() ?? "Null";

                    Console.WriteLine($"✅ The symbol {companyNameCsv} information from yahoou was updated!");

                    #region Report the responce data     
                    DicteneryInfraService dicteneryInfraService = new DicteneryInfraService();
                    Dictionary<string, string> reportDataService = await dicteneryInfraService.ReturnStockDataDictionary(
                         companyNameCsv,
                         price,
                         volume,
                         eps,
                         movingAvg50,
                         movingAvg200,
                         high52Week,
                         low52Week,
                         rsi // Convert everything to string
                     );
                    //Add it to the mongo db
                    await mongoDbService.InsertOrUpdateDicteneryDataToMongo(companyNameCsv, reportDataService,
                        MongoDbInfra.DataBaseCollection.stockData);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"⚠️ An error occurred: {e.Message}, for symbol {companyNameCsv}");
                    Console.WriteLine($"🔍 StackTrace: {e.StackTrace}");
                    // Optionally, log the full exception details
                }

                //bool isUpdateSuccess = await InfraFileService.ReadAndUpdateCSVFile(reportFilePath, reportData);
                runingAttamp++;
                //Do not remove it (to not blcok)
                await Task.Delay(1000);
                Console.WriteLine("The test while as being end!!!");
                #endregion
            }
        }
    }
}
