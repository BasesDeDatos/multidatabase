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
            Dictionary<string, Dictionary<string, object>> listaResultados = new Dictionary<string, Dictionary<string, object>>();
            List<string> excludedRows = new List<string>();

            foreach (tableXcolumn txc in query.tableXcolumn)
            {
                procedure= "get_tuplas(" + txc.column + ")";
                List<Dictionary<string, object> > resultados = db.CallProcedure(procedure);
                foreach (Dictionary<string, object> resultado in resultados)
                {
                    List<Dictionary<string, object>> dataQuery = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    string ID_tupla = resultado["ID_tupla"].ToString();
                    string column_name = resultado["table_name"].ToString() + "." + resultado["column_name"].ToString();
                    object value = new object();
                    string condition = "";

                    //if (query.filter.method.ToString() != "")
                    try
                    {   // Si la columna que se está obteniendo información coincide con la del where, se filtra
                        if (resultado["ID_column"].ToString() == query.filter.column) 
                        {
                            condition = " AND  data" + query.filter.method + " '" + query.filter.byValue + "'";
                        }
                    }
                    catch {}

                    //Si el Row del column a consultar ya se excluyó entonces no se consultan los datos
                    if (!excludedRows.Contains(resultado["ID_tupla"].ToString()))
                    { 
                        //Si la entrada ID_tupla no se ha agregado al diccionario, se crea una entrada Dictionary<string, object> y se agrega
                        if (!listaResultados.TryGetValue(ID_tupla, out row))
                        {
                            Dictionary<string, object> valor = new Dictionary<string, object>();
                            listaResultados.Add(ID_tupla, valor);
                        } 

                        switch (resultado["database_type"].ToString())
                        {
                            case "mariaDB":
                                dataQuery = executeQueryMaria(resultado, condition);
                                break;
                            case "SQLServer":
                                dataQuery = executeQueryServer(resultado, condition);
                                break;
                            case "mongoDB":
                                dataQuery = executeQueryMongo(resultado, query.filter.byValue);
                                break;
                        }

                        if (dataQuery.Count > 0)
                        {
                            value = dataQuery[0]["data"];
                            listaResultados[ID_tupla][column_name] = value; //Se el resultado a la lista que se retornará
                        }
                        else
                        {
                            excludedRows.Add(resultado["ID_tupla"].ToString()); //Se agrega el ID de la tupla que se va a excluir del resultado
                        }
                    }
                }
            }

            //Se borran los rows que no coinciden con el WHERE
            foreach (var key in excludedRows){
                listaResultados.Remove(key);
            }

            return JsonConvert.SerializeObject(listaResultados);
        }

        [WebInvoke(Method = "POST",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "insertRow")]
        public string insertRow(parametrosInsert insert)
        {
            try
            {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");
                //int row_id = Guid.NewGuid().GetHashCode();
                string row_id = generateID();
                foreach (valueXcolum valorXcolumna in insert.valueXcolumn)
                {
                    var data_id = db.GetValueFunction("add_tupla('" + row_id + "', '" + valorXcolumna.column + "');");
                    string procedure = "get_connection(" + valorXcolumna.column + ");";
                    List<Dictionary<string, object>> conexion = db.CallProcedure(procedure);
                    switch (conexion[0]["database_type"].ToString())
                    {
                        case "mariaDB":
                            insertMaria(conexion[0], valorXcolumna.value, data_id);
                            break;
                        case "SQLServer":
                            insertServer(conexion[0], valorXcolumna.value, data_id);
                            break;
                        case "mongoDB":
                            insertMongo(conexion[0], valorXcolumna.value, data_id);
                            break;
                    }
                }
                return "{\"message\": \"" + "SE INSERTO LA FILA EXITOSAMENTE!" + "\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        [WebInvoke(Method = "POST",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "updateQuery")]
        public string updateQuery(parametrosUpdate update)
        {
            try
            {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");

                string procedure = "get_tuplas(" + update.column + ");";
                List<Dictionary<string, object>> tuplas = db.CallProcedure(procedure);

                foreach (Dictionary<string, object> resultado in tuplas)
                {
                    object value = new object();
                    string database_type = resultado["database_type"].ToString();
                    string condition = update.filter.method + " '" + update.filter.byValue + "'";

                    switch (database_type)
                    {
                        case "mariaDB":
                            updateQueryMaria(resultado, condition, update.value);
                            break;

                        case "SQLServer":
                            updateQueryServer(resultado, condition, update.value);
                            break;

                        case "mongoDB":
                            updateQueryMongo(resultado, update.filter.byValue, update.value);
                            break;
                    }

                }
                return "{\"message\": \"" + "SE ACTUALIZO LA FILA EXITOSAMENTE!" + "\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        [WebInvoke(Method = "POST",
                  ResponseFormat = WebMessageFormat.Json,
                  RequestFormat = WebMessageFormat.Json,
                  UriTemplate = "query_deleteRow")]
        public string query_deleteRow(parametrosDelete delete)
        {
            try
            {
                MariaConnect db = new MariaConnect("localhost", "TEST", "prueba", "prueba", "3306");

                string procedure = "get_tuplas(" + delete.filter.column + ");";
                List<Dictionary<string, object>> tuplas = db.CallProcedure(procedure);
                List<string> deletedRows = new List<string>();

                foreach (Dictionary<string, object> resultado in tuplas)
                {
                    object value = new object();
                    string database_type = resultado["database_type"].ToString();
                    string condition = delete.filter.method + " '" + delete.filter.byValue + "'";
                    Debug.WriteLine(condition);
                    bool hasBeenDeleted = false;

                    switch (database_type)
                    {
                        case "mariaDB":
                            hasBeenDeleted = hasBeenDeletedMaria(resultado, condition);
                            break;

                        case "SQLServer":
                            hasBeenDeleted = hasBeenDeletedServer(resultado, condition);
                            break;

                        case "mongoDB":
                            hasBeenDeleted = hasBeenDeletedMongo(resultado, delete.filter.byValue);
                            break;
                    }

                    if (hasBeenDeleted && !deletedRows.Contains(resultado["ID_tupla"].ToString()))
                    {
                        deletedRows.Add(resultado["ID_tupla"].ToString());
                    }

                }

                foreach (string id_tupla in deletedRows)
                {
                    List<Dictionary<string, object>> dataTuplas = db.CallProcedure("CALL get_tupla('" + id_tupla + "')");

                    bool validDelete = true;
                    foreach (Dictionary<string, object> column in dataTuplas)
                    {
                        switch (column["column_type"].ToString())
                        {
                            case "mariaDB":
                                validDelete = validDelete && deleteDataMaria(column);
                                break;

                            case "SQLServer":
                                validDelete = validDelete && deleteDataServer(column);
                                break;

                            case "mongoDB":
                                validDelete = validDelete && deleteDataMongo(column);
                                break;
                        }
                    }
                    if (validDelete)
                        db.NonQuery("DELETE FROM tuplas WHERE ID_tupla = '" + id_tupla + "';");
                }

                return "{\"message\": \"" + "SE BORRON " + deletedRows.Count + " FILAS EXITOSAMENTE!" + "\"}";
            }
            catch (Exception ex)
            {
                return "{\"messageError\": \"" + ex.ToString() + "\"}";
            }
        }

        public bool deleteDataMaria(Dictionary<string, object> datos)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MariaConnect db = new MariaConnect(server, database, uid, pass, port);

            try
            {
                db.NonQuery("DELETE FROM " + datos["column_type"].ToString() + " WHERE data_id = '" + datos["ID_data"].ToString() + "';");
                return true;
            }

            catch
            {
                return false;
            }
        }

        public bool deleteDataServer(Dictionary<string, object> datos)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            ServerConnect db = new ServerConnect(server, database, uid, pass, port);

            try
            {
                db.NonQuery("DELETE FROM " + datos["column_type"].ToString() + " WHERE data_id = '" + datos["ID_data"].ToString() + "';");
                return exist;
            }
            catch
            {
                return false;
            }
        }

        public bool deleteDataMongo(Dictionary<string, object> datos)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MongoConnect db = new MongoConnect(server, database, uid, pass, port);

            try
            {
                List<Dictionary<string, object>> select = db.SelectListDictionary(datos["column_type"].ToString(), datos["ID_data"].ToString(), condition);
                Debug.WriteLine("select.Count = " + select.Count.ToString());
                bool exist = false;
                if (select.Count > 0) exist = true;
                /*if (exist)
                {
                    Debug.WriteLine(datos["column_type"].ToString()+" "+datos["ID_data"].ToString());
                    db.Delete(datos["column_type"].ToString(), datos["ID_data"].ToString());
                }*/

                return exist;
            }

            catch
            {
                return false;
            }
        }

        public bool hasBeenDeletedMaria(Dictionary<string, object> datos, string condition)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MariaConnect db = new MariaConnect(server, database, uid, pass, port);

            try
            {
                List<Dictionary<string, object>> select =  db.SelectListDictionary(datos["column_type"].ToString(), "data_id = " + datos["ID_data"] + " AND data " + condition + ";");
                bool exist = false;
                if (select.Count > 0) exist = true;
                /*if (exist)
                {
                    db.NonQuery("DELETE FROM " + datos["column_type"].ToString() + " WHERE data_id = '" + datos["ID_data"].ToString() + "';");
                }*/

                return exist;
            }

            catch
            {
                return false;
            }
        }

        public bool hasBeenDeletedServer(Dictionary<string, object> datos, string condition)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            ServerConnect db = new ServerConnect(server, database, uid, pass, port);

            try
            {
                List<Dictionary<string, object>> select = db.SelectListDictionary(datos["column_type"].ToString(), "data_id = " + datos["ID_data"] + " AND data " + condition + ";");
                bool exist = false;
                if (select.Count > 0) exist = true;
                /*if (exist)
                {
                    db.NonQuery("DELETE FROM " + datos["column_type"].ToString() + " WHERE data_id = '" + datos["ID_data"].ToString() + "';");
                }*/

                return exist;
            }

            catch
            {
                return false;
            }
        }

        public bool hasBeenDeletedMongo(Dictionary<string, object> datos, string condition)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MongoConnect db = new MongoConnect(server, database, uid, pass, port);

            try
            {
                List<Dictionary<string, object>> select = db.SelectListDictionary(datos["column_type"].ToString(), datos["ID_data"].ToString(), condition);
                Debug.WriteLine("select.Count = " + select.Count.ToString());
                bool exist = false;
                if (select.Count > 0) exist = true;
                /*if (exist)
                {
                    Debug.WriteLine(datos["column_type"].ToString()+" "+datos["ID_data"].ToString());
                    db.Delete(datos["column_type"].ToString(), datos["ID_data"].ToString());
                }*/

                return exist;
            }

            catch
            {
                return false;
            }
        }

        public bool updateQueryMaria(Dictionary<string, object> datos, string condition, string value)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();
            
            MariaConnect db = new MariaConnect(server, database, uid, pass, port);

            //update string set data = 'holi' WHERE data_id = 1 AND data = 'pi';
            
            db.NonQuery("UPDATE " + datos["column_type"].ToString() + " SET data = '" + value +
                        "' WHERE data_id = " + datos["ID_data"] + " AND data " + condition + ";");

            return true;
        }

        public bool updateQueryServer(Dictionary<string, object> datos, string condition, string value)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            ServerConnect db = new ServerConnect(server, database, uid, pass, port);

            //update string set data = 'holi' WHERE data_id = 1 AND data = 'pi';

            db.NonQuery("UPDATE " + datos["column_type"].ToString() + " SET data = '" + value +
                        "' WHERE data_id = " + datos["ID_data"] + " AND data " + condition + ";");

            return true;
        }

        public bool updateQueryMongo(Dictionary<string, object> datos, string condition, string value)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MongoConnect db = new MongoConnect(server, database, uid, pass, port);
            
            db.Update(datos["column_type"].ToString(), datos["ID_data"].ToString(), value, condition);

            return true;
        }

        public List<Dictionary<string, object>> executeQueryMaria(Dictionary<string, object> datos, string condition)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MariaConnect db = new MariaConnect(server, database, uid, pass, port);
            return db.SelectListDictionary(datos["column_type"].ToString(), "data_id = " + datos["ID_data"] + condition + ";");
        }

        public List<Dictionary<string, object>> executeQueryServer(Dictionary<string, object> datos, string condition)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            ServerConnect db = new ServerConnect(server, database, uid, pass, port);
            return db.SelectListDictionary(datos["column_type"].ToString(), "data_id = " + datos["ID_data"] + condition + ";");
        }

        public List<Dictionary<string, object>> executeQueryMongo(Dictionary<string, object> datos, string condition)
        {
            string server = datos["server"].ToString();
            string database = "multidb_datos";
            string uid = datos["user"].ToString();
            string pass = datos["pass"].ToString();
            string port = datos["port"].ToString();

            MongoConnect db = new MongoConnect(server, database, uid, pass, port);

            return db.SelectListDictionary(datos["column_type"].ToString(), datos["ID_data"].ToString(), condition);
        }

        public bool insertMaria(Dictionary<string, object> conexion, object dato, int data_id)
        {
            string server = conexion["server"].ToString();
            string database = "multidb_datos";
            string uid = conexion["user"].ToString();
            string pass = conexion["pass"].ToString();
            string port = conexion["port"].ToString();

            MariaConnect db = new MariaConnect(server, database, uid, pass, port);

            try
            { 
                db.NonQuery("INSERT INTO " + conexion["columna"] + "(data_id, data) VALUES ('" + data_id + "', '" + dato.ToString() + "');");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertServer(Dictionary<string, object> conexion, object dato, int data_id)
        {
            string server = conexion["server"].ToString();
            string database = "multidb_datos";
            string uid = conexion["user"].ToString();
            string pass = conexion["pass"].ToString();
            string port = conexion["port"].ToString();

            ServerConnect db = new ServerConnect(server, database, uid, pass, port);

            try
            {
                db.NonQuery("INSERT INTO " + conexion["columna"] + "(data_id, data) VALUES ('" + data_id + "', '" + dato.ToString() + "');");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool insertMongo(Dictionary<string, object> conexion, object dato, int data_id)
        {
            string server = conexion["server"].ToString();
            string database = "multidb_datos";
            string uid = conexion["user"].ToString();
            string pass = conexion["pass"].ToString();
            string port = conexion["port"].ToString();

            MongoConnect db = new MongoConnect(server, database, uid, pass, port);

            try
            {
                var row = new BsonDocument
                {
                    {"data_id",data_id.ToString()},
                    {"data", dato.ToString()}
                };
                db.Insert(row, conexion["columna"].ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }


        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
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
        public filter filter { get; set; }
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

    public class parametrosInsert
    {
        public string source { get; set; }
        public List<valueXcolum> valueXcolumn { get; set; }
    }

    public class valueXcolum
    {
        public string value { get; set; }
        public string column { get; set; }
    }

    public class parametrosUpdate
    {
        public string column  { get; set; }
        public string value { get; set; }
        public filter filter { get; set; }
    }

    public class parametrosDelete
    {
        public string source { get; set; }
        public string tabla { get; set; }
        public filter filter { get; set; }
    }
}
