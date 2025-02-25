using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.Infra.DbService
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService()
        {
            string connectionString = "mongodb+srv://botfinvizauto:69s6gtoKlJ1OSWA4@cluster0.rtgzz.mongodb.net/?retryWrites=true&w=majority";

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("botfinvizauto"); // ודא שזה שם ה-DB שלך

            Console.WriteLine("✅ Connected to MongoDB successfully!"); // שנה את שם ה-DB אם צריך
        }

        public async Task InsertOrUpdateStockData(string stockName, string field, string value)
        {
            
            var collection = _database.GetCollection<BsonDocument>("stocks"); // טבלה של מניות
            var filter = Builders<BsonDocument>.Filter.Eq("stockName", stockName);
            var update = Builders<BsonDocument>.Update.Set(field, value);
            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
        public async Task InsertOrUpdateDicteneryData(string stockSymbol, Dictionary<string, string> stockData)
        {
            var collection = _database.GetCollection<BsonDocument>("stocks");

            // צור מסמך חדש בדיוק כמו שקיבלת
            var newEntry = new BsonDocument(stockData);

            // חפש אם ה-symbol קיים
            var filter = Builders<BsonDocument>.Filter.Eq("symbol", stockSymbol);

            // הוסף את המסמך החדש כעוד עותק של הרשומה
            var update = Builders<BsonDocument>.Update
                .Push("entries", newEntry); // כל נתון נוסף יישמר ברשימה נפרדת

            var options = new UpdateOptions { IsUpsert = true }; // אם לא קיים, צור רשומה חדשה

            var result = await collection.UpdateOneAsync(filter, update, options);

            if (result.ModifiedCount > 0)
            {
                Console.WriteLine($"✅ Added new record for: {stockSymbol}");
            }
            else if (result.UpsertedId != null)
            {
                Console.WriteLine($"✅ Inserted new stock entry for: {stockSymbol}");
            }
            else
            {
                Console.WriteLine($"⚠ No changes made to: {stockSymbol}");
            }
        }

    }
}
