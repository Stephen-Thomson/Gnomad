/************************************************************************************************
*
* Author: Bryce Schultz, Andrew Rice, Karter Zwetschke, Andrew Ramirez, Stephen Thomson
* Date: 11/28/2022
*
* Purpose: This class contains all the Database Connection Functions.
*
************************************************************************************************/

using MySql.Data.MySqlClient;
using System;

namespace TravelCompanionAPI.Data
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private string _connection_string;
        private static object _lock = new object();

        private DatabaseConnection()
        { }

        public void setConnectionString(string connection_string)
        {
            _connection_string = connection_string;
        }

        public static DatabaseConnection getInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                }
            }

            return _instance;
        }

        public MySqlConnection getConnection()
        {
            // ensure that a connectionn string is set
            if (_connection_string == null)
            {
                throw new Exception("DatabaseConnection does not have a connection string.");
            }

            // create the connection
            MySqlConnection connection = new MySqlConnection(_connection_string);

            // try to open the connection, if it isn't able to
            // print the error message and return null.
            try
            {
                connection.Open();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not open database connection: {0}", exception.Message);
                return null;
            }

            // return the opened connection,
            // user will have to cose the connection manually.
            return connection;
        }
    }
}
