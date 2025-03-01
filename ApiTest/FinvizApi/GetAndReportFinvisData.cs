using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvesAuto.Infra.InfraCommonService;
using NUnit.Framework;
using System.Net;
using static InvesAuto.Infra.BaseTest.EnumServiceList;

namespace InvestAuto.ApiTest.FinvizApi
{
    [TestFixture, Category(Categories.GetFinvizData),
    Category(TestLevel.Level_2)]
    public class GetAndReportFinvisData : ApiInfraTest
    {
        [Test]
        public async Task _GetAndReportFinvisData()
        {
            #region Get company list from file
            InfraFileService infraFileService = new InfraFileService();
            string currentPath = Directory.GetCurrentDirectory();
            string indexXlsxPath = Path.Combine(currentPath, "GeneralFiles", "CompanyNames.csv");
            #endregion

            #region Get number of companies
            string indexClumn = "A";
            int companyIndextCounter = infraFileService.GetCsvRowsIntValue(indexXlsxPath, indexClumn);
            int runingAttamp = 1;
            #endregion
            while (runingAttamp<= companyIndextCounter)
            {
                string companyNameCsv = infraFileService.GetCsvValue(indexXlsxPath, $"{indexClumn}{runingAttamp}");

                string finvizUtl = $"https://finviz.com/api/quote.ashx?aftermarket=false&dateFrom=1706713200&dateTo=1740754799&events=false&financialAttachments=&instrument=stock&patterns=false&premarket=false&rev=1739706540851&ticker={companyNameCsv}&timeframe=d";
                SetUpBaseUrl(finvizUtl);
                var responseUserProfile = await HttpService
                .CallWithoutBody<FinvisOutputDto>(
                    new HttpCallOptionsSimple("")
                    { Method = HttpCallMethod.Get });

                Assert.That(HttpStatusCode.OK == responseUserProfile.HttpStatus, responseUserProfile.BodyString);
                string lastOpenResult = responseUserProfile.Result.lastOpen.ToString();
                string lastCloseResult = responseUserProfile.Result.lastClose.ToString();
                string lastVolumeResult = responseUserProfile.Result.lastVolume.ToString();

                #region Report the responce data

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo israelTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                DateTime israelTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, israelTimeZone);

                string reportFilePath = Path.Combine(currentPath, "GeneralFiles", $"{companyNameCsv}.csv");

                Dictionary<string, string> reportData = new Dictionary<string, string>
                {
                    { "Date", $"{israelTime}" },
                    { "lastOpen", $"{lastOpenResult}" },
                    { "lastClose", $"{lastCloseResult}" },
                    { "lastVolume", $"{lastVolumeResult}" }

                };

                bool isUpdateSuccess = await InfraFileService.ReadAndUpdateCSVFile(reportFilePath, reportData);
                #endregion
                runingAttamp++;
            }

        }
    }

}
