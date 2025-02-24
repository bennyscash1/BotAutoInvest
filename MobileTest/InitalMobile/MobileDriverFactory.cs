using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using InvesAuto.Infra.BaseTest;

namespace InvestAuto.MobileTest.Inital
{
  
    public class MobileDriverFactory : Base
    {
        public AndroidDriver appiumDriver;

        public MobileDriverFactory()
        {
            appiumDriver = InitAppiumDriver();
        }

        public AndroidDriver InitAppiumDriver()
        {
            var appiumOption = InitAppiumOptions();
            var uri = new Uri("http://127.0.0.1:4723/wd/hub");
            var driver = new AndroidDriver(uri, appiumOption);
            return driver;
        }      
       

        public AppiumOptions InitAppiumOptions()
        {
            string deviceUuid = GetDeviceUUID();
            string buyerAppPackage = GetTestData(configDataEnum.appPackage);
            string buyerAppActivity = GetTestData(configDataEnum.appActivity);
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.NewCommandTimeout, 100000);
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.PlatformName, "android");
            appiumOptions.AddAdditionalAppiumOption("appPackage", buyerAppPackage);
            appiumOptions.AddAdditionalAppiumOption("appActivity", buyerAppActivity);
            appiumOptions.AddAdditionalAppiumOption(MobileCapabilityType.Udid, deviceUuid);
            return appiumOptions;
        }
        public string GetDeviceUUID()
        {
            // Start a new process for adb command
            Process process = new Process();
            process.StartInfo.FileName = "adb";
            process.StartInfo.Arguments = "get-serialno";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            // Start the process and get the output
            process.Start();
            string uuid = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            return uuid;
        }

        public void Dispose()
        {
            appiumDriver.Quit();
        }

        }
}
