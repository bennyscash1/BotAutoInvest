using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Diagnostics;
using InvesAuto.Infra.BaseTest;


namespace InvestAuto.Test.UiTest.BaseData
{

    public class WebDriverFactory :Base
    {
        public IWebDriver driver;
        public TimeSpan timeOutInSeconds = TimeSpan.FromSeconds(20);

        public WebDriverFactory()
        {
            RemoveChromedriver();
            driver = WebDriver(BrowserType.Chrome);
            driver.Manage().Window.Maximize();

        }
        private static void CloseRunningChromeDriverProcesses()
        {
            try
            {
                foreach (var process in Process.GetProcessesByName("chromedriver"))
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while killing Chromedriver processes: {ex.Message}");
            }
        }
        private static void RemoveChromedriver()
        {
            CloseRunningChromeDriverProcesses();
            string chromeGeneralPath =Path.Combine( Directory.GetCurrentDirectory() , "Chromedriver.exe");
            try
            {
                if (File.Exists(chromeGeneralPath))
                {
                    File.SetAttributes(chromeGeneralPath, FileAttributes.Normal); // Ensure the file is not read-only
                    File.Delete(chromeGeneralPath);
                    Console.WriteLine($"Chromedriver removed successfully from {chromeGeneralPath}");
                }
                else
                {
                    Console.WriteLine("Chromedriver not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing Chromedriver: {ex.Message}");
            }
        }


        public enum BrowserType
        {
            IE,
            Chrome,
            Firefox,
            PhantomJS
        }

        public static IWebDriver WebDriver(BrowserType type)
        {
            IWebDriver driver = null;

            switch (type)
            {
                case BrowserType.IE:
                    driver = IeDriver();
                    break;
                case BrowserType.Firefox:
                    driver = FirefoxDriver();
                    break;
                case BrowserType.Chrome:
                    driver = ChromeDriver();
                    break;
                default:
                    driver = ChromeDriver();
                    break;
            }

            return driver;
        }

        /// <summary>
        /// Creates Internet Explorer Driver instance.
        /// </summary>
        /// <returns>A new instance of IEDriverServer</returns>
        private static IWebDriver IeDriver()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            options.EnsureCleanSession = true;
            IWebDriver driver = new InternetExplorerDriver(options);
            return driver;
        }

        /// <summary>
        /// Creates Firefox Driver instance.
        /// </summary>
        /// <returns>A new instance of Firefox Driver</returns>
        private static IWebDriver FirefoxDriver()
        {
            FirefoxProfile profile = new FirefoxProfile();
            IWebDriver driver = new FirefoxDriver();
            return driver;
        }


        /// <summary>
        /// Creates Chrome Driver instance.
        /// </summary>
        /// <returns>A new instance of Chrome Driver</returns>
        private static IWebDriver ChromeDriver()
        {
            ChromeDriver driver = new ChromeDriver();
            return driver;
        }

  


        // setup
        public void Dispose()
        {
            driver.Close();
        }

    }
}