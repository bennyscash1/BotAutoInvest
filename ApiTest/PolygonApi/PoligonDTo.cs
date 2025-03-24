using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.ApiTest.PolygonApi
{
    public class PolygonHistoryOutputDTO
    {
        public results[] results { get; set; }
        // Number of transactions
    }
    public class results
    {
        public string v { get; set; }  // Volume
        public string vw { get; set; } // Volume Weighted Average Price
        public string o { get; set; }  // Open price
        public string c { get; set; }  // Close price
        public string h { get; set; }  // High price
        public string l { get; set; }  // Low price
        public string n { get; set; }
    }
    // RSI data
    public class PolygonRsiOutputDTO
    {
        public resultsRsi results { get; set; }
    }
    public class resultsRsi
    {
        public values [] values { get; set; }
    }
    public class values
    {
        public string value { get; set; }
    }
}
