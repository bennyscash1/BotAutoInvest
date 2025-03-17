﻿using InvesAuto.ApiTest.ApiService;
using InvesAuto.Infra.DbService;
using NUnit.Framework;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvesAuto.ApiTest.YahooFinanceApi
{
    [TestFixture, Category(Categories.ApiStockDataGit),
        Category(TestLevel.Level_1)]
    public class GetAndReportYhaooFinance :YahooRequestService
    {
        [Test]
        public async Task _GetAndReportYhaooFinance()
        {

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
                string companyNameFromDB = "";

                companyNameFromDB = symbolList[runingAttamp];
                StockSymbolDataDto result = await YahooRequestService.GetStockDataAsync(companyNameFromDB);
                // Initialize variables as empty strings
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
                    averageDailyVolume3Month = result.AverageDailyVolume3Month;
                    trailingAnnualDividendRate = result.TrailingAnnualDividendRate;

                    if (!string.IsNullOrEmpty(price))
                    {
                        bool isSymbolLargeStock = YahooRequestService
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
                    }
                    else
                    {
                        Console.WriteLine($"company stock: {companyNameFromDB} price is value {price}");
                    }
                }
                
                    
                runingAttamp++;
                await Task.Delay(1000);
                Console.WriteLine("The test while as being end!!!");
                #endregion
            }

        }
    }
}
