/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ComprehensiveAutomation.Test.UiTest.BaseData;
using ComprehensiveAutomation.Test.UiTest.Tests.Flows;
using Serilog;
using System.IO;
using static ComprehensiveAutomation.Test.Infra.BaseTest.EnumList;

namespace ComprehensiveAutomation.Test.UiTest.Tests.LoginTest
{
    public class LoginWeb : WebDriverFactory, IDisposable
    {

        private string m_url = GetTestData(configDataEnum.WebUrl);
        string WebUserName = GetTestData(configDataEnum.WebUserName);
        string WebPassword = GetTestData(configDataEnum.WebPassword);

     
        public void _LoginWeb()
        {
            BaseFlows baseFlow;

            LoginFlow loginFlow = new LoginFlow(driver);

            loginFlow =
            loginFlow.OpenPage
                (i_navigateToLogonScreen: true, m_url);
            loginFlow
                .BoValidLogin(WebUserName, WebPassword);

            bool isHomePageOpen =
            loginFlow.isHomePageOpen();

            
        }


    }
}
*/