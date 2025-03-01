using InvesAuto.Infra.BaseTest;
using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InvestAuto.WebTest.IntialWeb
{
    public class PlaywrightFactory : Base
    {
        public IBrowser Browser { get; private set; }
        public IPage pPage { get; private set; }
        public BrowserTypeLaunchOptions LaunchOptions { get; private set; }
        public TimeSpan TimeOutInSeconds { get; private set; } = TimeSpan.FromSeconds(20);

        public PlaywrightFactory(BrowserType browserType = BrowserType.Chrome)
        {
            InitializePlaywright(browserType).GetAwaiter().GetResult();
        }

        private async Task InitializePlaywright(BrowserType browserType)
        {
            var playwright = await Playwright.CreateAsync();
            LaunchOptions = new BrowserTypeLaunchOptions { Headless = false };
          
            switch (browserType)
            {
                case BrowserType.IE:
                    Browser = await playwright.Webkit.LaunchAsync(LaunchOptions);
                    break;
                case BrowserType.Firefox:
                    Browser = await playwright.Firefox.LaunchAsync(LaunchOptions);
                    break;
                case BrowserType.Chrome:
                default:
                    Browser = await playwright.Chromium.LaunchAsync(LaunchOptions);
                    break;
            }

            pPage = await Browser.NewPageAsync();
            await pPage.SetViewportSizeAsync(1920, 1080);
        }

        public async Task GotoAsync(string url)
        {
            await pPage.GotoAsync(url);
        }

        public async Task<string> GetTitleAsync()
        {
            return await pPage.TitleAsync();
        }

      /*  public void Dispose()
        {
            pPage?.CloseAsync().GetAwaiter().GetResult();
            Browser?.CloseAsync().GetAwaiter().GetResult();
        }*/
    }

    public enum BrowserType
    {
        IE,
        Chrome,
        Firefox
    }
}
