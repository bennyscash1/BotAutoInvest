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
    }
}
