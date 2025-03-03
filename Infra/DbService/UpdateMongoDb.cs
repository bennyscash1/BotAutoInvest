using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvesAuto.Infra.DbService
{
    public class UpdateMongoDb : MongoDbInfra
    {
        // Remove this line; no need to redefine _database
        // private readonly IMongoDatabase _database;

        public UpdateMongoDb() : base() // Ensure MongoDbInfra's constructor is called
        {
            Console.WriteLine("✅ UpdateMongoDb constructor called");
        }

        public async Task InsertOrUpdateDicteneryDataToMongo(string stockSymbol, Dictionary<string, string> 
            stockData, DataBaseCollection dataBaseCollection)
        {
            var collection = _database.GetCollection<BsonDocument>(dataBaseCollection.ToString()); // Now uses inherited _database

            var newEntry = new BsonDocument(stockData);
            var filter = Builders<BsonDocument>.Filter.Eq("symbol", stockSymbol);
            var update = Builders<BsonDocument>.Update
                .Push("entries", newEntry);
            var options = new UpdateOptions { IsUpsert = true };

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

        public async Task InsertOrUpdateStockData(string stockName, string field, string value)
        {
            var collection = _database.GetCollection<BsonDocument>("stocks"); // Now uses inherited _database
            var filter = Builders<BsonDocument>.Filter.Eq("stockName", stockName);
            var update = Builders<BsonDocument>.Update.Set(field, value);
            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }
    }
}