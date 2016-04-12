using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsMultiDBService
{
    class MariaConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public MariaConnect(string pServer, string pDatabase, string pUid, string pPassword)
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
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //Open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
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
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public void NonQuery(string query)
        {
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public string ReaderQuery(string qQuery)
        {
            string query = "CALL " + qQuery + ";";
            string result = "NULL";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

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

        //Select statement
        public int Select(string function)
        {
            string query = "SELECT " + function;
            //Create a list to store the result
            int values = -1;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    //list[0].Add(dataReader["id"] + "");
                     values = dataReader.GetInt32(0);
                    
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return values;
            }
            else
            {
                return values;
            }
        }

        public ArrayList Select(string tableName, string conditional)
        {
            string query = "SELECT * " + "FROM " + tableName + " WHERE " + conditional + ";";
            //Create a list to store the result
            ArrayList list = new ArrayList();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

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
            string result = "{}";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

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

        public string returnJSON(MySqlDataReader reader)
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
                    result += "\""+ stringValue + "\"";
                    if (x < columnCount - 1) result += ",";
                }
                result += "},";
                counter++;
            }
            result = result.TrimEnd(result[result.Length - 1]) + "}";
            if (result == "}") {
                result = "{}";
            }
            return result;
        }
    }
}