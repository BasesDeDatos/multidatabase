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
        private IMongoDatabase database;

        public MongoConnect(string pDatabase)
        {
            client = new MongoClient();
            database = client.GetDatabase(pDatabase);
        }

        public async void Select(string table)
        {
            var collection = database.GetCollection<BsonDocument>(table);
            var filter = new BsonDocument();
            var count = 0;
            ArrayList list = new ArrayList();
            using (var cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        list.Add(document);
                        count++;
                    }
                }
            }
        }
    }
}