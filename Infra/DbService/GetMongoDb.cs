using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.Infra.DbService
{
    public class GetMongoDbDTO
    {
        public ObjectId Id { get; set; }
        public string symbol { get; set; }
        public decimal price { get; set; }
        public string volume { get; set; }
        public string time { get; set; }
        public string Date { get; set; }
        public string updatedAt { get; set; }
        public List<object> entries { get; set; } // Adjust if needed
    }

    public class GetMongoDb : MongoDbInfra
    {
        private readonly IMongoCollection<GetMongoDbDTO> _collection;

        public GetMongoDb(DataBaseCollection collectionName = DataBaseCollection.stockCompanyList) // Default
        {
            _collection = _database.GetCollection<GetMongoDbDTO>(collectionName.ToString());
        }

        public async Task<List<string>> GetStockListFromDB()
        {
            var filter = Builders<GetMongoDbDTO>.Filter.Empty; // Fetch all documents
            var documents = await _collection.Find(filter).ToListAsync();
            var symbols = new List<string>();

            foreach (var doc in documents)
            {
                if (!string.IsNullOrEmpty(doc.symbol)) // Ensure symbol exists
                {
                    symbols.Add(doc.symbol);
                }
            }

            return symbols; // Return List<string> instead of string
        }


        public async Task<string> GetAllStocks()
        {
            var results = await _collection.Find(Builders<GetMongoDbDTO>.Filter.Empty).ToListAsync();
            Console.WriteLine(JsonConvert.SerializeObject(results, Formatting.Indented));

            return results.Count == 0 ? "No data found in MongoDB." : JsonConvert.SerializeObject(results, Formatting.Indented);
        }
        public async Task<string> GetStockDataBySymbol(string symbol)
        {
            Console.WriteLine("🔍 Fetching all stock symbols for debugging...");
            var allStocks = await _collection.Find(Builders<GetMongoDbDTO>.Filter.Empty).ToListAsync();

            Console.WriteLine("Found symbols in DB:");
            foreach (var stock in allStocks)
            {
                Console.WriteLine($"- {stock.symbol}");
            }

            var filter = Builders<GetMongoDbDTO>.Filter.Eq(s => s.symbol, symbol);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                Console.WriteLine($"❌ No stock data found for symbol: {symbol}");
                return "[]";
            }

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
        public async Task<int> AnalyzeStockBySymbol(string symbol, int daysAhead = 2)
        {
            Console.WriteLine($"🔍 Fetching stock data for symbol: {symbol}...");

            var filter = Builders<GetMongoDbDTO>.Filter.Eq(s => s.symbol, symbol);
            var stocks = await _collection.Find(filter).Sort(Builders<GetMongoDbDTO>.Sort.Ascending(s => s.Date)).ToListAsync();

            if (stocks.Count < daysAhead)
            {
                Console.WriteLine($"❌ Not enough data for prediction (need at least {daysAhead} entries).");
                return 2; // Hold
            }

            // חישוב Future Return ל-X ימים קדימה
            for (int i = 0; i < stocks.Count - daysAhead; i++)
            {
                double priceToday = Convert.ToDouble(stocks[i].price);
                double priceFuture = Convert.ToDouble(stocks[i + daysAhead].price);
                double futureReturn = ((priceFuture - priceToday) / priceToday) * 100;

                Console.WriteLine($"📅 Date: {stocks[i].Date}, Price: {priceToday}, Future Price: {priceFuture}, Future Return: {futureReturn}%");

                // החלטת קנייה/מכירה לפי הסף
                if (futureReturn > 5)
                    return 1; // Buy
                if (futureReturn < -5)
                    return 3; // Sell
            }

            return 2; // Hold (ברירת מחדל)
        }

    }
}

