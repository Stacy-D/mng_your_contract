using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;

namespace AspNet.Identity.MySQL
{
     /// <summary>
     /// клас що інкапсулює MySQL дб звязок 
     /// і CRUD операції
     /// </summary>
    public class MySQLDatabase : IDisposable
    {
        private MySqlConnection _connection;

        /// дефолтний конструктор
        /// </summary>
        public MySQLDatabase()
            : this("DefaultConnection")
        {
        }

        /// <summary>
        /// конекшн конструктор
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MySQLDatabase(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// MySQL 
        /// </summary>
        public int Execute(string commandText, Dictionary<string, object> parameters)
        {
            int result = 0;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Помилка");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Поверрнення значень
        /// </summary>
        public object QueryValue(string commandText, Dictionary<string, object> parameters)
        {
            object result = null;

            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Command text cannot be null or empty.");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                result = command.ExecuteScalar();
            }
            finally
            {
                EnsureConnectionClosed();
            }

            return result;
        }

        /// <summary>
        /// Повертає колнки зі значеннями
        /// </summary>
        public List<Dictionary<string, string>> Query(string commandText, Dictionary<string, object> parameters)
        {
            List<Dictionary<string, string>> rows = null;
            if (String.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("Помилка");
            }

            try
            {
                EnsureConnectionOpen();
                var command = CreateCommand(commandText, parameters);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    rows = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, string>();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.IsDBNull(i) ? null : reader.GetString(i);
                            row.Add(columnName, columnValue);
                        }
                        rows.Add(row);
                    }
                }
            }
            finally
            {
                EnsureConnectionClosed();
            }

            return rows;
        }

        /// <summary>
        /// Відкриваємо конекшн
        /// </summary>
        private void EnsureConnectionOpen()
        {
            var retries = 3;
            if (_connection.State == ConnectionState.Open)
            {
                return;
            }
            else
            {
                while (retries >= 0 && _connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                    retries--;
                    Thread.Sleep(30);
                }
            }
        }

        /// <summary>
        /// Закриваємо конекшн
        /// </summary>
        public void EnsureConnectionClosed()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// SQL що повертає параметри
        /// </summary>
        private MySqlCommand CreateCommand(string commandText, Dictionary<string, object> parameters)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = commandText;
            AddParameters(command, parameters);

            return command;
        }

        /// <summary>
        /// додаємо параметри
        /// </summary>
        private static void AddParameters(MySqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Key;
                parameter.Value = param.Value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }
        }

        /// <summary>
        /// Допоміжний метод
        /// </summary>
        public string GetStrValue(string commandText, Dictionary<string, object> parameters)
        {
            string value = QueryValue(commandText, parameters) as string;
            return value;
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
