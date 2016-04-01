using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

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
                estadoConexion_mongo = connection_mongo()
            };
            /*return new parametrosAddDatabase()
            {
                database_type = "database_type",
                user = "user",
                pass = "pass",
                server = "server",
                protocol = "protocol",
                port = "port",
                alias = "alias"
            };*/
            //return addDatabase.user;
            
        }
        
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool connection_maria()
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=1029612;database=mysql;";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                return true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return false;
            }
        }

        public string connection_mongo()
        {
            IMongoClient _client;
            IMongoDatabase _database;
            
            _client = new MongoClient();
            _database = _client.GetDatabase("test");

            string state = _client.Cluster.Description.State.ToString();

            return state;
            /*if (state == "Disconnected")
            {
                return false;
            }
            else
            {
                return true;
            }*/
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

 
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
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
        public bool estadoConexion_maria { get; set; }
        public string estadoConexion_mongo { get; set; }
    }

}
