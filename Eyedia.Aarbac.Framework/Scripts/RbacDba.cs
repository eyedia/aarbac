#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Debjyoti Das (debjyoti@debjyoti.com)
Created - 11/12/2017 3:31:31 PM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice

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
            if (string.IsNullOrEmpty(dbName))
                dbName = "aarbac";

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
                string script = string.Format(GetScript("Eyedia.Aarbac.Framework.Scripts.01_DatabaseExist.sql"), dbName);
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
            string script = GetScript("Eyedia.Aarbac.Framework.Scripts.04_CreateTables.sql");
            ExecuteScript(connection, script.Replace(__dbname, dbName));
            WriteColor(ConsoleColor.Green, "Created." + Environment.NewLine);
            Console.Write("Create indexes...".PadRight(__maxInfoLen,'.'));
            CreateIndexes(connection, dbName);
            WriteColor(ConsoleColor.Green, "Created." + Environment.NewLine);
        }
        private void CreateIndexes(SqlConnection connection,string dbName)
        {
            string script = GetScript("Eyedia.Aarbac.Framework.Scripts.05_CreateIndexes.sql");
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
            string getLogLoc = GetScript("Eyedia.Aarbac.Framework.Scripts.02_GetServerDataFileLocation.sql");
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
            string script = GetScript("Eyedia.Aarbac.Framework.Scripts.03_CreateDB.sql");
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


