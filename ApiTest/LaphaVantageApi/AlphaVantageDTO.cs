using Newtonsoft.Json;
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

    public class AlphaVantageGlobalQueteOutputDTO
    {
        public GlobalQuoteDto GlobalQuote { get; set; }
    }
    public class AlphaVantageOverviewOutputDTO
    {
        public string EPS {  get; set; }
        [JsonProperty("50DayMovingAverage")]
        public string MovingAverage50Day { get; set; }

        [JsonProperty("200DayMovingAverage")]
        public string MovingAverage200Day { get; set; }

        [JsonProperty("52WeekHigh")]
        public string Week52High { get; set; }

        [JsonProperty("52WeekLow")]
        public string Week52Low { get; set; }
    }
    public class RsiValueOutputDto
    {
        [JsonProperty("Technical Analysis: RSI")]
        public Dictionary<string, RsiEntry> TechnicalAnalysisRSI { get; set; }
    }

    public class RsiEntry
    {
        [JsonProperty("RSI")]
        public string RSI { get; set; }
    }





}
