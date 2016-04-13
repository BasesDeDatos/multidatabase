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
                var id_table = db.Select("add_table('" + createTable.table_name + "',"+
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
        public parametrosQuery executeQuery(parametrosQuery query)
        {
            try
            {
                return query;
            }
            catch (Exception ex)
            {
                return query;
            }
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
        public List<tableXcolumn> tablesXcolumns { get; set; }
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
