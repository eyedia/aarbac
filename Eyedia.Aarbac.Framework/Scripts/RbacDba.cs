using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacDba
    {
        const string __dbname = "__DBNAME__";
        const string __dbfile = "__DBFILE__";
        const string __dblogfile = "__DBLOGFILE__";
        public const int __maxInfoLen = 40;
        public string ConnectionString { get; private set; }   
             
        public RbacDba()
        {
          
        }

        public RbacDba(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void CreateDatabase(string dbName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                WriteColor(ConsoleColor.Green, "Contacted." + Environment.NewLine);
                Console.Write(string.Format("Creating database {0}", dbName).PadRight(__maxInfoLen, '.'));
                string script = GetScriptsCreateDatabase(connection, dbName);
                ExecuteScript(connection, script.Replace(__dbname, dbName));
                WriteColor(ConsoleColor.Green, "Created." + Environment.NewLine);
                Console.Write("Creating tables...".PadRight(__maxInfoLen, '.'));
                CreateTables(connection, dbName);
               
            }
        }

        public bool DatabaseExists(string dbName)
        {
            bool found = false;
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();               
                string script = string.Format(GetScript("Symplus.Rbac.Scripts.01_DatabaseExist.sql"), dbName);
                SqlCommand command = new SqlCommand(script, connection);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                if (table.Rows.Count > 0)
                    found = true;
                reader.Close();
                command.Dispose();
                connection.Close();
            }
            return found;
        }
       
        private void CreateTables(SqlConnection connection, string dbName)
        {
            string script = GetScript("Symplus.Rbac.Scripts.04_CreateTables.sql");
            ExecuteScript(connection, script.Replace(__dbname, dbName));
            WriteColor(ConsoleColor.Green, "Created." + Environment.NewLine);
            Console.Write("Create indexes...".PadRight(__maxInfoLen,'.'));
            CreateIndexes(connection, dbName);
            WriteColor(ConsoleColor.Green, "Created." + Environment.NewLine);
        }
        private void CreateIndexes(SqlConnection connection,string dbName)
        {
            string script = GetScript("Symplus.Rbac.Scripts.05_CreateIndexes.sql");
            ExecuteScript(connection, script.Replace(__dbname, dbName));
        }

        private void ExecuteScript(SqlConnection connection, string script)
        {           
            IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$",
                                         RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string commandString in commandStrings)
            {
                if (commandString.Trim() != "")
                {
                    using (var command = new SqlCommand(commandString, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            
        }

        private string GetScriptsCreateDatabase(SqlConnection connection, string dbName)
        {
            string defLoc = string.Empty;
            string getLogLoc = GetScript("Symplus.Rbac.Scripts.02_GetServerDataFileLocation.sql");
            SqlCommand command = new SqlCommand(getLogLoc, connection);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            if (table.Rows.Count > 0)
                defLoc = Path.GetDirectoryName(table.Rows[0]["location"].ToString());
            reader.Close();
            command.Dispose();

            string dbFile = Path.Combine(defLoc, dbName + ".mdf");
            string dbLogName = Path.Combine(defLoc, dbName + "_log.ldf");
            string script = GetScript("Symplus.Rbac.Scripts.03_CreateDB.sql");
            script = script.Replace(__dbname, dbName);
            script = script.Replace(__dbfile, dbFile);
            script = script.Replace(__dblogfile, dbLogName);
            return script;
            
        }

        private string GetScript(string name)
        {
            string script = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
            {
                script = reader.ReadToEnd();
            }           
            return script;
        }

        public static void WriteColor(ConsoleColor color, string message, params object[] arg)
        {
            ConsoleColor defColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message, arg);
            Console.ForegroundColor = defColor;

        }
    }
}
