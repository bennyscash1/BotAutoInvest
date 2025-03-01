using OpenQA.Selenium;
using InvestAuto.Test.PageObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace InvestAuto.Test.UiTest.Tests.Flows
{
    public class LoginFlow  :BaseFlows
    {
        public IPage pDriver;
        LoginPage loginPage;

        public LoginFlow(IPage i_driver) : base(i_driver)
       {
            pDriver = i_driver;
           loginPage = new LoginPage(i_driver);

       }
        public async Task BoValidLoginAsync(string i_email, string password)
        {
            await loginPage
                .EnterEmail(i_email)
                .ContinueWith(async =>
                loginPage.EnterPassword(password))
                .ContinueWith(async =>
                loginPage.ClickEnter())
                .Unwrap();


        }
        public async Task <bool> IsHomePageDisplay()
        {
            return loginPage.IsHomePageDisplay();
        }
        /* public LoginFlow(IWebDriver i_driver) : base(i_driver)
         {
             m_driver = i_driver;
             loginPage = new LoginPage(i_driver);

         }*/

        /*   public LoginFlow OpenPage(bool i_navigateToLogonScreen = true, string i_url = null)
           {
               if (i_navigateToLogonScreen)
                   m_driver.Navigate().GoToUrl(i_url);          
               return this;
           }*/

        /*public LoginFlow BoValidLogin (string i_email, string i_password)
        {
            loginPage
                .EnterEmail(i_email)
                .EntePassword(i_password)
                .clickOnsubmitButton();
            return this;                     
        }*/

        /*   public bool isHomePageOpen()
           {
               loginPage.IsHomePageDisplay();
               return true;
           }*/


    }

}

