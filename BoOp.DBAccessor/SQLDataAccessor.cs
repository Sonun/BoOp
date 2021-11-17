using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoOp.DBAccessor
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Dateiname: SQLDataAccessor.cs
    //Author : Manuel Janzen
    //Erstellt am : 3/9/2021
    //Bearbeitet von : Manuel Janzen
    //Beschreibung : Klasse zur bereitstellung zum laden und speichern in einer Datenbank
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class SQLDataAccessor
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }

        public static string GetConncetionString(string connectionStringName = "Default")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var output = config.GetConnectionString(connectionStringName);

            return output;
        }
        public void RemoveData( string sqlStatement, string connectionString)
        {

        }
    }
}
