using ComprehensivePlayrightAuto.ApiTest.HttpService;
using InvesAuto.ApiTest.HttpService;
using InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InvesAuto.ApiTest.ApiService
{
    public class InfraApiService : ApiInfraTest
    {
        //public async Task <int>Get
        public async Task<string> GetNewsInformationXmlBySymbol(string symbol)
        {
            string finvizUrl = $"https://feeds.finance.yahoo.com/rss/2.0/headline?s={symbol}&region=US&lang=en-US";
            SetUpBaseUrl(finvizUrl);
            var responseUserProfile = await HttpService
            .CallWithoutBody<FinvisOutputDto>(
                new HttpCallOptionsSimple("")
                { Method = HttpCallMethod.Get });
            Assert.That(HttpStatusCode.OK == responseUserProfile.HttpStatus,
            responseUserProfile.BodyString);

            var responseUserProfileBody = responseUserProfile.BodyString;
            return responseUserProfileBody;
        }
        public async Task<string> GetNewsApiInformationJson(int howManyArticle = 5)
        {
            string url = "https://newsapi.org/v2/everything?q=US stock market&language=en&sortBy=publishedAt&apiKey=a22df4aead2d4990a6f08da7ac8a0df3";
            SetUpBaseUrl(url);

            var httpCallOptions = new HttpCallOptionsSimple("")
            {
                Method = HttpCallMethod.Get,
                Headers = new Dictionary<string, string>
        {
            { "User-Agent", "MyStockNewsApp/1.0" }
        }
            };

            var response = await HttpService
                .CallWithoutBody<NewsapiOutputDto>(httpCallOptions);

            Assert.That(HttpStatusCode.OK == response.HttpStatus, response.BodyString);

            var responseListArticle = response.Result.Articles;

            var latestArticles = responseListArticle
                .OrderByDescending(a => a.publishedAt)
                .Take(howManyArticle)
                .Select(a => $"Title: {a.title}\nDescription: {a.description}\nContent: {a.content}\n---")
                .ToList();
            return string.Join("\n", latestArticles);
        }
        public async Task<string> GetCnbcNewsXml(int howManyArticle = 5)
        {
            string url = "https://search.cnbc.com/rs/search/combinedcms/view.xml?partnerId=wrss01&id=15837362";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "MyStockNewsApp/1.0");
                var response = await client.GetStringAsync(url);

                return ParseNewsXml(response, howManyArticle);
            }
        }

        private string ParseNewsXml(string xml, int howManyArticle)
        {
            XDocument doc = XDocument.Parse(xml);

            var articles = doc.Descendants("item")
                .Take(howManyArticle)
                .Select(item => new
                {
                    Title = item.Element("title")?.Value.Trim(),
                    Description = item.Element("description")?.Value.Trim()
                })
                .Select(a => $"Title: {a.Title}\nDescription: {a.Description}\n---")
                .ToList();

            return string.Join("\n", articles);
        }

        public static bool IsMarketCapHaveALargeThreshold(string marketCapStr, long threshold = 2_000_000_000)
        {
            if (long.TryParse(marketCapStr, out long marketCap))
            {
                return marketCap >threshold;
            }
            return false; // Return false if conversion fails
        }
    }
}
