using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.FinvizApi
{
    public class FinvizApiService : ApiInfraTest
    {
        public async Task<bool> IsSymbolValid(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return false;
            }
            string finvizUrl = $"https://finviz.com/api/etf_holdings/{symbol}/top_ten";
            SetUpBaseUrl(finvizUrl);
            var resonceFinvizIsSymbolValid = await HttpService
            .CallWithoutBody<FinvisOutputDto>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == resonceFinvizIsSymbolValid.HttpStatus,
            resonceFinvizIsSymbolValid.BodyString);
            //Need to wait before get to the next request
            await Task.Delay(3000);
            var responseUserProfileBody = resonceFinvizIsSymbolValid.BodyString;
        
            if (responseUserProfileBody.Contains("\"rowData\": []") )
            {
                return false;
            }
            else
            {
                return true;
            }
          
        }

    }
}
