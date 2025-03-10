using InvesAuto.ApiTest.ApiService;
using InvesAuto.Infra.DbService;
using NUnit.Framework;
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

                string companyNameFromDB = "";
                // Extract the values


                companyNameFromDB = symbolList[runingAttamp];
                StockSymbolData result = await YahooRequestService.GetStockDataAsync(companyNameFromDB);
                // Initialize variables as empty strings
                string companyName = "";
                string price = "";
                string volume = "";
                string movingAvg50 = "";
                string movingAvg200 = "";
                string high52Week = "";
                string low52Week = "";
                string eps = "";
                string marketTime = "";
                string marketCap = "";
                string sharesOutstanding = "";
                if (result != null)
                {

                    // Example of using individual properties
                    companyName = result.CompanyName;
                    price = result.Price;
                    volume = result.Volume;
                    movingAvg50 = result.MovingAvg50;
                    movingAvg200 = result.MovingAvg200;
                    high52Week = result.High52Week;
                    low52Week = result.Low52Week;
                    eps = result.EPS;
                    marketTime = result.MarketTime;
                    marketCap = result.MarketCap;
                    sharesOutstanding = result.SharesOutstanding;
                }
                bool isSymbolLargeStock = InfraApiService
                    .IsMarketCapHaveALargeThreshold(marketCap);
                if (isSymbolLargeStock)
                {
                    Console.WriteLine($"✅ The symbol {companyNameFromDB} information from yahoou was updated!");

                    #region Report the responce data     
                    DicteneryInfraService dicteneryInfraService = new DicteneryInfraService();
                    Dictionary<string, string> reportDataService = await dicteneryInfraService.ReturnStockDataDictionary(
                         companyNameFromDB,
                         price,
                         volume,
                         eps,
                         movingAvg50,
                         movingAvg200,
                         high52Week,
                         low52Week,
                         eps,
                         marketTime,
                         marketCap,
                         sharesOutstanding
                     );
                    //Add it to the mongo db
                    await mongoDbService.InsertOrUpdateDicteneryDataToMongo(companyNameFromDB, reportDataService,
                        MongoDbInfra.DataBaseCollection.stockData);
                    Console.WriteLine($"Company stock: {companyNameFromDB} was updated");
                }
                else
                {
                    Console.WriteLine($"company stock: {companyNameFromDB} cap amount is {marketCap}");
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
