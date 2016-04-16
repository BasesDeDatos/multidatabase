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
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json;
namespace nsMultiDBService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "multiDBService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select multiDBService.svc or multiDBService.svc.cs at the Solution Explorer and start debugging.
    public class multiDBService : IMultiDBService
    {
        [WebInvoke(Method = "POST", 
                    ResponseFormat = WebMessageFormat.Json, 
                    RequestFormat = WebMessageFormat.Json,
                    UriTemplate = "addDatabase")]
        public string addDatabase(parametrosAddDatabase addDatabase)
        {
            try {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
                db.NonQuery("CALL registrar_conexion('" + addDatabase.database_type + "','"
                                                   + addDatabase.user + "','"
                                                   + addDatabase.pass + "','"
                                                   + addDatabase.server + "','"
                                                   + addDatabase.protocol + "','"
                                                   + addDatabase.port + "','"
                                                   + addDatabase.alias + "');");

                return "{\"message\": \"Conexion creada exitosamente\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        [WebInvoke(Method = "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    UriTemplate = "createDatabase")]
        public string createDatabase(parametrosCreateDatabase createDatabase) {
            try
            {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
                db.NonQuery("CALL add_data_base('" + createDatabase.name + "');");
                return "{\"message\": \"Base de datos creada exitosamente\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        [WebInvoke(Method = "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    RequestFormat = WebMessageFormat.Json,
                    UriTemplate = "createTable")]
        public string createTable(parametrosCreateTable createTable)
        {
            try
            {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
                var id_table = db.GetValueFunction("add_table('" + createTable.table_name + "',"+
                                                "" + createTable.database_id + ");");
                
                foreach (parametrosColumn column in createTable.columns)
                {
                   db.NonQuery("CALL add_Column(" + id_table + "," +
                                                "'" + column.DB_alias + "'," +
                                                "'" + column.column_name + "',"+
                                                "'" + column.Type + "'," +
                                                "'" + column.Null + "'" +
                                                ");");
                }
                return "{\"message\": \"" + "SE CREO LA TABLA EXITOSAMENTE!" + "\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "getDBConnections")]
        public string getDBConnections()
        {
            MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
            return db.Select2("db_connection");
        }

        [WebInvoke(Method = "GET",
                   ResponseFormat = WebMessageFormat.Json,
                   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "getDatabases")]
        public string getDatabases()
        {
            MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
            return db.ReaderQuery("get_databases(NULL)");
        }

        [WebInvoke(Method = "GET",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "getTables")]
        public string getTables()
        {
            MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
            return db.ReaderQuery("get_tables(NULL)");
        }

        [WebInvoke(Method = "GET",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "getColumns")]
        public string getColumns()
        {
            MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
            return db.ReaderQuery("get_columns(NULL)");
        }

        [WebInvoke(Method = "DELETE",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "dropTable")]
        public string dropTable(parametroID dropTable)
        {
            try
            {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
                db.NonQuery("DELETE FROM TABLAS WHERE ID =" + dropTable.ID);
                return "{\"message\": \"Tabla borrada exitosamente\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        [WebInvoke(Method = "POST",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "executeQuery")]
        public string executeQuery(parametrosQuery query)
        {
            MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
            string procedure;

            Dictionary<int, Dictionary<string, object>> listaResultados = new Dictionary<int, Dictionary<string, object>>();

            foreach (tableXcolumn txc in query.tableXcolumn)
            {
                procedure= "get_tuplas(" + txc.column + ")";
                List<Dictionary<string, object> > resultados = db.CallProcedure(procedure);
                foreach (Dictionary<string, object> resultado in resultados)
                {
                    List<Dictionary<string, object>> dataQuery = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    int ID_tupla = Convert.ToInt32(resultado["ID_tupla"]);
                    string column_name = resultado["column_name"].ToString();
                    object value = new object();

                    //Si la entrada ID_tupla no se ha agregado al diccionario, se crea una entrada Dictionary<string, object> y se agrega
                    if (!listaResultados.TryGetValue(ID_tupla, out row))
                    {
                        Dictionary<string, object> valor = new Dictionary<string, object>();
                        listaResultados.Add(ID_tupla, valor);
                    }

                    switch(resultado["database_type"].ToString()){
                        case "mariaDB":
                            dataQuery = executeQueryMaria(resultado);
                            value = dataQuery[0]["data"];
                            break;
                        case "SQLServer":
                            dataQuery = executeQueryServer(resultado);
                            value = dataQuery[0]["data"];
                            break;
                        case "mongoDB":
                            break;
                    }

                    listaResultados[ID_tupla][column_name] = value;
                }
            }
                
            return JsonConvert.SerializeObject(listaResultados);
        }

        public List<Dictionary<string, object>> executeQueryMaria(Dictionary<string, object> datos)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MariaConnect db = new MariaConnect(server, database, uid, pass, port);
            return db.SelectListDictionary(datos["column_type"].ToString(), "data_id = " + datos["ID_data"]);
        }
        public List<Dictionary<string, object>> executeQueryServer(Dictionary<string, object> datos)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            ServerConnect db = new ServerConnect(server, database, uid, pass, port);

            return db.SelectListDictionary(datos["column_type"].ToString(), "data_id = " + datos["ID_data"]);
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
        public string estadoConexion_maria { get; set; }
        public string estadoConexion_mongo { get; set; }
        public string estadoConexion_sqlserver { get; set; }
    }
    public class parametrosCreateDatabase
    {
        public string name { get; set; }
    }

    public class parametrosCreateTable {
        public string table_name { get; set; }
        public string database_id { get; set; }
        public List<parametrosColumn> columns { get; set; }
    }

    public class parametrosColumn {
        public string DB_alias { get; set; }
        public string column_name { get; set; }
        public string Type { get; set; }
        public string Null { get; set; }
    }

    public class parametroID
    {
        public string ID { get; set; }
    }

    public class parametrosQuery
    {
        public string source { get; set; }
        public List<tableXcolumn> tableXcolumn { get; set; }
        public List<filter> filter { get; set; }
        public List<order> order { get; set; }
    }

    public class tableXcolumn
    {
        public string table { get; set; }
        public string column { get; set; }
    }

    public class filter
    {
        public string column { get; set; }
        public string method { get; set; }
        public string byValue { get; set; }
    }

    public class order
    {
        public string method { get; set; }
        public string byValue { get; set; }
    }

}
