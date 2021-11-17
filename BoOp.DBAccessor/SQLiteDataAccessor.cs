using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Data.SQLite;


namespace BoOp.DBAccessor
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: SQLiteDataAccessor.cs
    //Author : Manuel Janzen
    //Erstellt am : 30/9/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : Klasse zur bereitstellung zum laden und speichern in einer SQLite Datenbank
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class SQLiteDataAccessor
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }

        public static string GetConncetionString(string connectionStringName = "SQLite")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
