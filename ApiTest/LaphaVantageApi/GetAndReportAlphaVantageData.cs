using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvesAuto.Infra.InfraCommonService;
using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using NUnit.Framework;
using System.Net;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvestAuto.ApiTest.LaphaVantageApi
{
    [TestFixture, Category(Categories.GetAlphaVintageData),
    Category(TestLevel.Level_1)]
    public class GetAndReportAlphaVantageData : ApiInfraTest
    {
        [Test]
        public async Task _GetAndReportAlphaVantageData()
        {
            #region Read the compnay list file
            InfraFileService infraFileService = new InfraFileService();
            string currentPath = Directory.GetCurrentDirectory();
            string indexXlsxPath = Path.Combine(currentPath, "GeneralFiles", "CompanyNames.csv");
            Console.WriteLine("The company files being load");
            string indexClumn = "A";
            int companyIndextCounter = infraFileService.GetCsvRowsIntValue(indexXlsxPath, indexClumn);
            int runingAttamp = 1;
            #endregion

            while (runingAttamp <= companyIndextCounter)
            {
                #region Get the company name and send the api request to alpha vantage
                string companyNameCsv = infraFileService.GetCsvValue(indexXlsxPath, $"{indexClumn}{runingAttamp}");
                Console.WriteLine( $"The company: {companyNameCsv} has being load and run");
                string finvizUtl = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={companyNameCsv}&apikey=D7MHCVHXPK2H37CO";
                SetUpBaseUrl(finvizUtl);
                var responseUserProfile = await HttpService
                .CallWithoutBody<AlphaVantageOutputDTO>(
                    new HttpCallOptionsSimple("")
                    { Method = HttpCallMethod.Get });

                Assert.That(HttpStatusCode.OK == responseUserProfile.HttpStatus,
                    responseUserProfile.BodyString);

                AlphaVantageService alphaVantageService = new AlphaVantageService();
                #endregion

                #region Get The value from the api responce
                string symbol = alphaVantageService
                    .GetJsonVantageResult(AlphaVantageService.AlphaVantageResponceEnum.Symbol);
                string price = alphaVantageService
                    .GetJsonVantageResult(AlphaVantageService.AlphaVantageResponceEnum.Price);
                string volume = alphaVantageService
                    .GetJsonVantageResult(AlphaVantageService.AlphaVantageResponceEnum.Volume);
                string time = alphaVantageService
                    .GetJsonVantageResult(AlphaVantageService.AlphaVantageResponceEnum.Time);
                #endregion

                #region Report the responce data
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                DateTime israelTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, israelTimeZone);

                string reportFilePath = Path.Combine(currentPath, "GeneralFiles", "AlphaVantage", $"{companyNameCsv}.csv");
                Console.WriteLine( "Report to alpha vantage has open");
                Dictionary<string, string> reportData = new Dictionary<string, string>
                {
                    { "Date", $"{israelTime}" },
                    { "symbol", $"{companyNameCsv}" },
                    { "price", $"{price}" },
                    { "volume", $"{volume}" },
                    { "time", $"{time}" }
                };

                bool isUpdateSuccess = await InfraFileService.ReadAndUpdateCSVFile(reportFilePath, reportData);
                #endregion
                runingAttamp++;
                Console.WriteLine( "The test while as being end!!!");
            }

        }
    }

}
