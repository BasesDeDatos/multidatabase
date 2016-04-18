using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Driver.Core;
using Newtonsoft.Json;

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
        private string port;

        public MongoConnect(string pServer, string pDatabase, string pUid, string pPassword, string pPort)
        {
            server = pServer;
            database = pDatabase;
            uid = pUid;
            password = pPassword;
            port = pPort;

            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "mongodb://" + uid + ":" + password +
            "@" + server + ":" + port + "/" + database;
            
            client = new MongoClient(connectionString);
            databaseInstance = client.GetDatabase(database);
        }

        public string isConnected()
        {
            string connection = client.Cluster.Description.State.ToString();
            return connection;
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

        public void Delete(string table, string conditional_id)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("data_id", conditional_id);
            collection.DeleteManyAsync(filter);
        }

        public void Update(string table, string conditional_id, string value, string conditional_value)
        {
            var builder = Builders<BsonDocument>.Filter;

            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var update = Builders<BsonDocument>.Update.Set("data", value);
            var filter = builder.Eq("data_id", conditional_id) & builder.Eq("data", conditional_value);
            collection.UpdateOneAsync(filter, update);
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

        public List<Dictionary<string, object>> SelectListDictionary(string table, string conditional_id, string conditional_value)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("data_id", conditional_id);
            if (conditional_value != null) filter = filter & builder.Eq("data", conditional_value);
            var selectResult = collection.Find(filter).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToListAsync();
            List<Dictionary<string, object>> resultado = new List<Dictionary<string, object>>();

            for (int i = 0; i < selectResult.Result.Count; i++)
            {
                resultado.Add(JsonConvert.DeserializeObject<Dictionary<string, object>>(selectResult.Result[i].ToString()));
            }

            return resultado;

        }
    }
}
