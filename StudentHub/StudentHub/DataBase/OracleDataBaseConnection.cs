using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace StudentHub.DataBase
{
    public static class OracleDataBaseConnection
    {
        private static string host = "secret";
        private static int port = 1521;
        private static string sid = "secret.be.by";
        private static string password = "secret";
        private static string user = "secret";

        public static readonly string ConnectionString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                                                         + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
                                                         + sid + ")));Password=" + password + ";User ID=" + user;

        private const string UserConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentHub; Integrated Security=true;User ID=Default_User;Password=password";
        private const string AdminConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentHub; Integrated Security=true;User ID=Admin_User;Password=password";
        private const string DefaultConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=StudentHub; Integrated Security=True";

        public static string data = ConnectionString;

        //public static void ApplyAdminPrivileges()
        //{
        //    data = AdminConnectionString;
        //}

        //public static void ApplyUserPrivileges()
        //{
        //    data = UserConnectionString;
        //}

        //private const string ConnectionStringWithoutDb = @"Data Source=.\SQLEXPRESS; Integrated Security=True";
        //private const string DB_NAME = "StudentHub";

        //private static readonly string CREATE_DB_QUERY = $"Use master; CREATE DATABASE {DB_NAME};";
        //private static readonly string SQL_SCRIPT_FILE_PATH = Directory.GetCurrentDirectory() + @"\StudentHubSqlTables.sql";

        //private static bool IsDbCreated = false;


        //static OracleDataBaseConnection()
        //{
        //    IsDbCreated = CheckIfTheDbExists(DB_NAME);
        //    CreateDb();
        //}

        //private static bool CheckIfTheDbExists(string dbName)
        //{
        //    string checkDbScript = "SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE NAME = @dbName";

        //    using (SqlConnection connection = new SqlConnection(ConnectionStringWithoutDb))
        //    {
        //        using (SqlCommand checkDbCommand = new SqlCommand(checkDbScript, connection))
        //        {
        //            checkDbCommand.Parameters.Add("@dbName", System.Data.SqlDbType.NVarChar).Value = dbName;
        //            connection.Open();
        //            return Convert.ToInt32(checkDbCommand.ExecuteScalar()) > 0;
        //        }
        //    }
        //}

        //private static void CreateDb()
        //{
        //    if (IsDbCreated)
        //        return;

        //    IsDbCreated = true;

        //    using (SqlConnection connection = new SqlConnection(ConnectionStringWithoutDb))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            SqlCommand command = new SqlCommand(CREATE_DB_QUERY, connection);
        //            command.ExecuteNonQuery();

        //            ExecuteScript(connection);
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //            MessageBox.Show(e.StackTrace);
        //        }
        //    }
        //}

        //private static void ExecuteScript(SqlConnection connection)
        //{
        //    string script = File.ReadAllText(SQL_SCRIPT_FILE_PATH, Encoding.Default);

        //    // Разделяем содержимое файла на строки, которые разделяются оператором GO
        //    IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        //    foreach (string commandString in commandStrings)
        //    {
        //        if (commandString.Trim() != string.Empty)
        //        {
        //            SqlCommand command = new SqlCommand(commandString, connection);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}
    }
}
