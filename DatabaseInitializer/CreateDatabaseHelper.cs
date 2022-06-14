//using MySql.Data.MySqlClient;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CreateDatabaseHelper
{
    public static bool Create(string connstr,FreeSql.DataType dataType)
    {
        bool isexisted = false;
        if (dataType == DataType.SqlServer)
        {
            SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder();
            sConnB.ConnectionString = connstr;
            string databasename = sConnB.InitialCatalog;
            sConnB.InitialCatalog = "Master";
            DbConnection conn = new SqlConnection(sConnB.ConnectionString);
            var sql1 = $"select * from sysdatabases where name=N'{databasename}'";
            var sql2 = $"CREATE DATABASE {databasename}";
            DbCommand cmd = new SqlCommand(sql1);
            cmd.Connection = conn;
            conn.Open();
            var reader = cmd.ExecuteReader();
            isexisted = reader.Read();
            reader.Close();
            if (!isexisted)
            {
                DbCommand cmd2 = conn.CreateCommand();
                cmd2.CommandText = sql2;
                cmd2.ExecuteNonQuery();
            }
            conn.Close();
        }
        //else if (dataType == DataType.PostgreSQL)
        //{
        //    NpgsqlConnectionStringBuilder sConnB = new NpgsqlConnectionStringBuilder();
        //    sConnB.ConnectionString = connstr;
        //    string databasename = sConnB.Database;
        //    sConnB.Database = "postgres";
        //    DbConnection conn = new NpgsqlConnection(sConnB.ConnectionString);
        //    var sql1 = $"SELECT FROM pg_database WHERE datname = '{databasename}'";
        //    var sql2 = $"CREATE DATABASE \"{databasename}\"";
        //    DbCommand cmd = new NpgsqlCommand(sql1);
        //    cmd.Connection = conn;
        //    conn.Open();
        //    var reader = cmd.ExecuteReader();
        //    isexisted = reader.Read();
        //    reader.Close();
        //    if (!isexisted)
        //    {
        //        DbCommand cmd2 = conn.CreateCommand();
        //        cmd2.CommandText = sql2;
        //        cmd2.ExecuteNonQuery();
        //    }
        //    conn.Close();
        //}
        //else if (dataType == DataType.MySql)
        //{
        //    MySqlConnectionStringBuilder sConnB = new MySqlConnectionStringBuilder();
        //    sConnB.ConnectionString = connstr;
        //    string databasename = sConnB.Database;
        //    sConnB.Database = "mysql";
        //    DbConnection conn = new MySqlConnection(sConnB.ConnectionString);
        //    DbCommand cmd1 = new MySqlCommand($"select *  from information_schema.TABLES where TABLE_NAME =  {databasename}");
        //    cmd1.Connection = conn;
        //    DbCommand cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {databasename}");
        //    cmd.Connection = conn;
        //    conn.Open();
        //    var reader = cmd1.ExecuteReader();
        //    isexisted = reader.Read();
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //}
        else
        {
            isexisted = true;
        }
        return isexisted;
    }
}
