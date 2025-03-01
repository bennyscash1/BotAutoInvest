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

        public GetMongoDb()
        {
            _collection = _database.GetCollection<GetMongoDbDTO>("stocks");
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

    }
}

