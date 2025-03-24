using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.Infra.DbService
{

    public class StockSymbolDataDto
    {
        public string CompanyName { get; set; }
        public string Price { get; set; }
        public string Volume { get; set; }
        public string MovingAvg50 { get; set; }
        public string MovingAvg200 { get; set; }
        public string High52Week { get; set; }
        public string Low52Week { get; set; }
        public string EPS { get; set; }
        public string MarketTime { get; set; }
        public string MarketCap { get; set; }
        public string SharesOutstanding { get; set; }
        public string AverageDailyVolume3Month { get; set; }
        public string TrailingAnnualDividendRate { get; set; }

    }
    public class StockSymbolHistoryData
    {
        public string Volume { get; set; }        // "v"
        public string VolumeWeightedAvgPrice { get; set; } // "vw"
        public string Open { get; set; }          // "o"
        public string Close { get; set; }         // "c"
        public string High { get; set; }          // "h"
        public string Low { get; set; }           // "l"
        public string NumberOfTransactions { get; set; } // "n"
    }
}
