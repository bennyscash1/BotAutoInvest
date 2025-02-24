using OpenQA.Selenium;
using InvestAuto.Test.PageObject;
using System.ComponentModel;
using Microsoft.Playwright;

namespace InvestAuto.Test.UiTest.Tests.Flows
{
    public class BaseFlows 
    {
        public IWebDriver m_driver;
        public IPage pDriver ;

        public BasePages basePages;


        public BaseFlows(IPage i_driver)
        {
            this.pDriver = i_driver;
            basePages = new BasePages(i_driver);
        }

        public string GetCurrentUrl()
        {
            return m_driver.Url;
        }
        public void NavigateToUrl(string i_url)
        {
            m_driver.Navigate().GoToUrl(i_url);
        }

    }
}
