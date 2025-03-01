using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestAuto.Infra.BaseTest
{
    public class TestData
    {
        public ApiInfraData Api { get; set; }
        public WebUiApiInfraData WebUi { get; set; }
        public MobileApiInfraData Mobile { get; set; }
    }

    public class ApiInfraData
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string BaseApiUrl { get; set; }
        public string AlphaVantageToken { get; set; }
        public string AlphaVantageToken2 { get; set; }
        public string OpenAiToken { get; set; }

    }
    public class WebUiApiInfraData
    {
        public string WebUrl { get; set; }
        public string WebUserName { get; set; }
        public string WebPassword { get; set; }
    }
   
    public class MobileApiInfraData
    {
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string appPackage { get; set; }
        public string appActivity { get; set; }

    }
}
