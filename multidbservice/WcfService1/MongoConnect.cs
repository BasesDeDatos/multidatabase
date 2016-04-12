using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Driver.Core;

namespace nsMultiDBService
{
    public class MongoConnect
    {
        private IMongoClient client;
        private IMongoDatabase databaseInstance;
        private string server;
        private string database;
        private string uid;
        private string password;

        public MongoConnect(string pServer, string pDatabase, string pUid, string pPassword)
        {
            server = pServer;
            database = pDatabase;
            uid = pUid;
            password = pPassword;

            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "mongodb://" + uid + ":" + password +
            "@" + server + "/" + database;
            
            client = new MongoClient(connectionString);
            databaseInstance = client.GetDatabase(database);
        }

        /*var documnt = new BsonDocument
            {
                {"Brand","Dell"},
                {"Price","400"},
                {"Ram","8GB"},
                {"HardDisk","1TB"},
                {"Screen","16inch"}
            };*/

        public void Insert(BsonDocument document, string table)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            collection.InsertOneAsync(document);
        }

        public void Delete(string table, string column, string condition)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var filter = Builders<BsonDocument>.Filter.Eq(column, condition);
            collection.DeleteManyAsync(filter);
        }

        public void Update(string table, string column, string condition, BsonDocument document)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var filter = Builders<BsonDocument>.Filter.Eq(column, condition);
            collection.ReplaceOneAsync(filter, document);
        }

        public string Select(string table)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var filter = new BsonDocument();
            var result = collection.Find(filter).ToListAsync();
            string resultado = "{";
            for (int i = 0; i < result.Result.Count; i++)
            {
                resultado += "\"" + i + "\": " + result.Result[i].ToJson();
            }
            resultado += "}";
            return resultado;
        }
    }
}
