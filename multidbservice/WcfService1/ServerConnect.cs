using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace nsMultiDBService
{
    class ServerConnect
    {
        private SqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string port;

        //Constructor
        public ServerConnect(string pServer, string pDatabase, string pUid, string pPassword, string pPort)
        {
            connection = new SqlConnection();
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
            connectionString = "Server=" + server + ", " + port + ";Database=" + 
            database + ";User Id=" + uid + ";Password=" + password + ";";

            connection.ConnectionString = connectionString;
        }

        //Open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        //Close connection to database
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public void NonQuery(string query)
        {
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                SqlCommand cmd = new SqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //GetValueFunction statement
        public ArrayList Select(string tableName)
        {
            string query = "SELECT * " + "FROM " + tableName;
            //Create a list to store the result
            ArrayList list = new ArrayList();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                SqlCommand cmd = new SqlCommand(query, connection);
                //Create a data reader and Execute the command
                SqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    //list[0].Add(dataReader["id"] + "");
                    object[] values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    list.Add(values);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //GetValueFunction statement
        public ArrayList Select(string tableName, string conditional)
        {
            string query = "SELECT * " + "FROM " + tableName + " WHERE " + conditional + ";";
            //Create a list to store the result
            ArrayList list = new ArrayList();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                SqlCommand cmd = new SqlCommand(query, connection);
                //Create a data reader and Execute the command
                SqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    //list[0].Add(dataReader["id"] + "");
                    object[] values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    list.Add(values);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        public string Select2(string tableName)
        {
            string query = "SELECT * FROM " + tableName + ";";
            //ArrayList result = new ArrayList();
            string result = "NULL";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                SqlCommand cmd = new SqlCommand(query, connection);
                //Create a data reader and Execute the command
                SqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                result = returnJSON(dataReader);

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return result;
            }
            else
            {
                return result;
            }
        }

        /*private ArrayList returnJSON(MySqlDataReader reader)
        {

            ArrayList list = new ArrayList();
            int columnCount = reader.FieldCount;
            while (reader.Read())
            {
                string result = "";
                result += "{";
                for (int x = 0; x < columnCount; x++)
                {
                    result += reader.GetName(x) + ":";
                    string stringValue = "";
                    if (!reader.IsDBNull(x))
                    {
                        stringValue = "'" + reader.GetValue(x).ToString() + "'";
                    }
                    else
                    {
                        stringValue = "NULL";
                    }
                    result += stringValue;
                    if (x < columnCount - 1) result += ",";
                }
                result += "}";

                list.Add(result);
            }

            return list;
        }*/

        public string returnJSON(SqlDataReader reader)
        {

            string result = "{";
            int columnCount = reader.FieldCount;
            int counter = 0;
            while (reader.Read())
            {
                result += "\"" + counter + "\": {";
                for (int x = 0; x < columnCount; x++)
                {
                    result += "\"" + reader.GetName(x) + "\":";
                    string stringValue = "";
                    if (!reader.IsDBNull(x))
                    {
                        stringValue = reader.GetValue(x).ToString();
                    }
                    else
                    {
                        stringValue = "NULL";
                    }
                    result += "\"" + stringValue + "\"";
                    if (x < columnCount - 1) result += ",";
                }
                result += "},";
                counter++;
            }
            result = result.TrimEnd(result[result.Length - 1]) + "}";
            if (result == "}")
            {
                result = "{}";
            }
            return result;
        }
    }
}