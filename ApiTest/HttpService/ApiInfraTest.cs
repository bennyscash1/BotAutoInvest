
using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvestAuto.Infra.BaseTest;

namespace InvesAuto.ApiTest.HttpService
{
    public class ApiInfraTest : BaseTest
    {
        protected HttpServiceInfra HttpService { get; private set; }
        public void SetUpBaseUrl(string defultUrl  )
        {
            HttpService = new HttpServiceInfra(
                new HttpServiceOptions
                {
                    BaseUrl = defultUrl
                }
             );
        }
    }
}
