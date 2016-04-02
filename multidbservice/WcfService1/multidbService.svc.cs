using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.SqlClient;

namespace nsMultiDBService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "multiDBService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select multiDBService.svc or multiDBService.svc.cs at the Solution Explorer and start debugging.
    public class multiDBService : IMultiDBService
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "GetData/{id}")]
        public Person GetData(string id)
        {
            // lookup person with the requested id 
            return new Person()
            {
                Id = Convert.ToInt32(id),
                Name = "Leo Messi"
            };
        }

        //[Serializable]
        [WebInvoke(Method = "POST", 
                    ResponseFormat = WebMessageFormat.Json, 
                    RequestFormat = WebMessageFormat.Json,
                    UriTemplate = "addDatabase")]
        public parametrosAddDatabase JSONparametrosAddDatabase(parametrosAddDatabase addDatabase)
        {
            return new parametrosAddDatabase()
            {
                database_type = addDatabase.database_type,
                user = addDatabase.user,
                pass = addDatabase.pass,
                server = addDatabase.server,
                protocol = addDatabase.protocol,
                port = addDatabase.port,
                alias = addDatabase.alias,
                estadoConexion_maria = connection_maria(),
                estadoConexion_mongo = connection_mongo(),
                estadoConexion_sqlserver = connection_sqlserver()
            };            
        }
        
        public string connection_maria()
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=1029612;database=mysql;";

            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;
            conn.Open();

            string state = conn.State.ToString();
            conn.Close();

            return state;
        }

        public string connection_mongo()
        {
            IMongoClient _client;
            IMongoDatabase _database;
            
            _client = new MongoClient();
            _database = _client.GetDatabase("test");

            string state = _client.Cluster.Description.State.ToString();

            return state;
        }

        public string connection_sqlserver()
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Server=JEFFREY-PC;Database=TEST;Trusted_Connection=true";
            conn.Open();

            string state = conn.State.ToString();

            conn.Close();
            return state;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
    public class parametrosAddDatabase
    {
        public string database_type { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string server { get; set; }
        public string protocol { get; set; }
        public string port { get; set; }
        public string alias { get; set; }
        public string estadoConexion_maria { get; set; }
        public string estadoConexion_mongo { get; set; }
        public string estadoConexion_sqlserver { get; set; }
    }

}
