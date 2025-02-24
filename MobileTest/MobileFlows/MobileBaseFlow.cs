using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestAuto.Test.UiTest.MobileTest.MobileFlows
{
    public class MobileBaseFlow
    {
        public AndroidDriver appiumDriver;
        public MobileBaseFlow(AndroidDriver i_driver)
        {
            this.appiumDriver = i_driver;
        }
    }
}
