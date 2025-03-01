using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InvestAuto.MobileTest.InitalMobile
{
    public class MobileBaseFunction
    {
        public AndroidDriver appiumDriver;
        public AndroidDriver appiumChromeDriver;
        public TimeSpan timeOutInSeconds = TimeSpan.FromSeconds(4);
        int maxAttamp = 3;


        public MobileBaseFunction(AndroidDriver driver)
        {
            appiumDriver = driver;
            appiumChromeDriver = driver;
        }
        public bool isElementFound(By el)
        {
            WebDriverWait wait = new WebDriverWait(appiumDriver, timeOutInSeconds);
            bool isElementFound = false;
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(el));
                isElementFound = true;
            }
            catch (WebDriverTimeoutException e)
            {
                isElementFound = false;
            }
            return isElementFound;
        }


        public void WaitForElement(By el)
        {
            int attempt = 0;
            bool elementClick = false;

            while (attempt++ < maxAttamp)
            {
                if (isElementFound(el) == true)
                {
                    elementClick = true;
                    break;
                }
                attempt++;
            }
            var elementDetails = $"Element {el} search faild within {timeOutInSeconds}";
            Assert.That(elementClick, elementDetails);
        }
        public void MobileClickElement(By el)
        {
            int attempt = 0;
            bool elementClick = false;

            while (attempt++ < maxAttamp)
            {
                if (isElementFound(el) == true)
                {
                    appiumDriver.FindElement(el).Click();
                    elementClick = true;
                    break;
                }

                attempt++;
            }
            var elementDetails = $"Element {el} search faild within {timeOutInSeconds}";
            Assert.That(elementClick, elementDetails);
        }

        public void fillText(By el, String text)
        {
            bool elementClick = false;
            int attempt = 0;

            while (attempt++ < maxAttamp)
            {
                if (isElementFound(el) == true)
                {
                    appiumDriver.FindElement(el).SendKeys(text);
                    elementClick = true;
                    break;
                }

                attempt++;
            }
            var elementDetails = $"Element {el} sendkeys faild within {timeOutInSeconds} seconds.";
            Assert.That(elementClick, elementDetails);
        }

        public void MovbileClickBack()
        {
            appiumDriver.Navigate().Back();
        }
        public void HideKyboard()
        {
            appiumDriver.HideKeyboard();
        }
        public void OpenStatusBar()
        {
            appiumDriver.OpenNotifications();
        }
        public void CloseStatusBar()
        {

            var screenWidth = appiumDriver.Manage().Window.Size.Width;
            var screenHeight = appiumDriver.Manage().Window.Size.Height;

            var touchAction = new TouchAction(appiumDriver);
            touchAction.Press(screenWidth / 2, screenHeight - 10).Release().Perform();

        }
    }
}
