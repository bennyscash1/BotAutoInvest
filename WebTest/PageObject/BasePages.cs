
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Drawing;
using InvestAuto.UiTest.BaseData;
using Microsoft.Playwright;

namespace InvestAuto.Test.PageObject
{
    public class BasePages : BaseFunctions

    {
        
        public IWebDriver m_driver;
        public IPage pPage;
        public TimeSpan timeOutInSeconds = TimeSpan.FromSeconds(6);
        public By m_okButonInPopupWindowBy = By.XPath("//button[normalize-space()='Ok']");


        public BasePages(IPage i_driver) : base(i_driver)
        {
            pPage = i_driver;
        }




    }
}

