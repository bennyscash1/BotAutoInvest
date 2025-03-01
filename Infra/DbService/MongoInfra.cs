using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.Infra.DbService
{
    public class MongoDbInfra 
    {
        public readonly IMongoDatabase _database;
        public static string connectionString = "mongodb+srv://botfinvizauto:69s6gtoKlJ1OSWA4@cluster0.rtgzz.mongodb.net/?retryWrites=true&w=majority";

        public MongoDbInfra()
        {

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("botfinvizauto"); // ודא שזה שם ה-DB שלך

            Console.WriteLine("✅ Connected to MongoDB successfully!"); // שנה את שם ה-DB אם צריך
        }
    }
}
