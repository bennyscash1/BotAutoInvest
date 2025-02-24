using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using InvestAuto.UiTest.BaseData;


namespace InvestAuto.Test.PageObject
{
    public class LoginPage : BasePages
    {

        private By m_userNameInputField = By.XPath("//input[@id='username']");
        private By m_passwordInputField = By.XPath("//input[@id='password']");
        private By m_submitButton = By.XPath("//button[@id='submit']");
        private By m_homePageLogoBy= By.XPath("//h1[normalize-space()='Logged In Successfully']");
        //
        private string m_userNameInputStr = "//input[@id='username']";
        private string m_userPasswordInputStr = "//input[@id='password']";
        private string m_submitStr = "//button[@id='submit']";
        private string m_hellowHomePage = "//h1[normalize-space()='Logged In Successfully']";

        public IPage pDriver;

        public LoginPage(IPage _page) : base(_page)
        {
            pDriver = _page;
        }
        public async Task EnterEmail(string i_email)
        {
            await FuillTextAsync(m_userNameInputStr, i_email);
        }
        public async Task EnterPassword(string i_email)
        {
            await FuillTextAsync(m_userPasswordInputStr, i_email);
        }
        public async Task ClickEnter()
        {
            await ClickAsync(m_submitStr);
        }
        public bool IsHomePageDisplay()
        {
            IsElementFoundAsync(m_hellowHomePage);
            return true;
        }
        /*    public LoginPage EnterEmail(string i_email)
            {
                fillText(m_userNameInputField, i_email);
                return this;                           
            }
            public LoginPage EntePassword (string i_password)
            {
                fillText(m_passwordInputField, i_password);
                return this;
            }
            public LoginPage clickOnsubmitButton()
            {
                click(m_submitButton);
                return this;
            }
            public bool IsHomePageDisplay()
            {
                waitForElementVisibility(m_homePageLogoBy);
                return true;
            }*/

    }
}
