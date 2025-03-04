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
            bool isSymbolValid = false;
            string finvizUrl = $"https://finviz.com/api/statement.ashx?t={symbol}&so=F&s=IA";
            SetUpBaseUrl(finvizUrl);
            var responseUserProfile = await HttpService
            .CallWithoutBody<FinvisOutputDto>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == responseUserProfile.HttpStatus,
            responseUserProfile.BodyString);

            var responseUserProfileBody = responseUserProfile.BodyString;
        
            if (responseUserProfileBody.Contains("error"))
            {
                isSymbolValid = false;
            }
            else
            {
                isSymbolValid = true;
            }
            return isSymbolValid;
        }

    }
}
