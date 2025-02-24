using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvestAuto.Test.ExternalApiTests.GenerateApiUserTokenTest
{
    public class GlobalQuoteDto
    {
        public decimal OpenEnum { get; set; }
 
    }

    public class AlphaVantageOutputDTO
    {
        public GlobalQuoteDto GlobalQuote { get; set; }
    }

    public class AlphaVantageService
    {
        public string GetJsonVantageResult(AlphaVantageResponceEnum paramType)
        {
            string jsonData = @"{
                        ""Global Quote"": {
                            ""01. symbol"": ""{companyName}"",
                            ""02. open"": ""35.9500"",
                            ""03. high"": ""36.7900"",
                            ""04. low"": ""33.3000"",
                            ""05. price"": ""33.4200"",
                            ""06. volume"": ""2064847"",
                            ""07. latest trading day"": ""2025-02-21"",
                            ""08. previous close"": ""35.2100"",
                            ""09. change"": ""-1.7900"",
                            ""10. change percent"": ""-5.0838%""
                        }
                    }";
            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                JsonElement globalQuote = document.RootElement.GetProperty("Global Quote");

                // Map the enum value to the corresponding JSON property
                string returnParam = "";
                switch (paramType)
                {
                    case AlphaVantageResponceEnum.Symbol:
                        returnParam = globalQuote.GetProperty("01. symbol").GetString();
                        break;

                    case AlphaVantageResponceEnum.Price:
                        returnParam = globalQuote.GetProperty("05. price").GetString(); // Assuming "price" refers to "05. price"
                        break;

                    case AlphaVantageResponceEnum.Volume:
                        returnParam = globalQuote.GetProperty("06. volume").GetString();
                        break;

                    case AlphaVantageResponceEnum.Time:
                        returnParam = globalQuote.GetProperty("07. latest trading day").GetString();
                        break;

                    default:
                        returnParam = null; // Or throw an exception for invalid enum values
                        break;
                }
                return returnParam;
            }
        }
        public enum AlphaVantageResponceEnum
        {
            Symbol,
            Price,
            Volume,
            Time
        }
    }

}
