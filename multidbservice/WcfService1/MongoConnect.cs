using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        /*
        public ArrayList Select(string table)
        {
            var collection = databaseInstance.GetCollection<BsonDocument>(table);
            var filter = new BsonDocument();
            var count = 0;
            ArrayList result = new ArrayList();
            while (await cursor.MoveNextAsync())
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    result.Add(document.ToString());
                    count++;
                }
            }
            return result;
        }*/

    }
}