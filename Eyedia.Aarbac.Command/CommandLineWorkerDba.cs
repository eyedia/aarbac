using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Aarbac.Framework;
using System.Configuration;

namespace Eyedia.Aarbac.Command
{
    public class CommandLineWorkerDba: CommandLineWorker
    {
        const string __csformat = "data source={0};Integrated security=True;";
        const string __csformatuser = "data source={0};User Id={1};Password={2};";
   
        public CommandLineWorkerDba():base()
        {

        }
        /// <summary>
        /// Create new rbac database on specified MS Sql Server instance. usage: rbac -c c -server servername -is false -ssuser username -sspassword password -db dbname
        /// </summary>
        /// <param name="options"></param>
        public void CreateNew(Options options)
        {
            bool errored = false;
            if (string.IsNullOrEmpty(options.SqlServer))
            {
                WriteErrorLine("Sql server name is required. Please use -server <servername>");
                errored = true;
            }

            if (!options.IntegratedSecurity)
            {
                if (string.IsNullOrEmpty(options.SqlServerUserName))
                {
                    WriteErrorLine("Sql server user name is required when integrated security is false. Please use -ssuser <username>");
                    errored = true;
                }

                if (string.IsNullOrEmpty(options.SqlServerPassword))
                {
                    WriteErrorLine("Sql server name user password is required required when integrated security is false. Please use -sspassword <password>");
                }

                Console.WriteLine("Please use -is <true/false> to set integrated security");
            }

            if (errored)
                return;

            string connectionString = string.Empty;
            if (options.IntegratedSecurity)
                connectionString = string.Format(__csformat, options.SqlServer);
            else
                connectionString = string.Format(__csformatuser, options.SqlServer, options.SqlServerUserName, options.SqlServerPassword);

            RbacDba dba = new RbacDba(connectionString);
            if (dba.DatabaseExists(options.DbName))
            {
                WriteErrorLine("Database {0} already exist!", options.DbName);
                return;
            }

            Console.Write("Contacting server...".PadRight(RbacDba.__maxInfoLen, '.'));
            new RbacDba(connectionString).CreateDatabase(options.DbName);
            WriteColor(ConsoleColor.Green, "All done!" + Environment.NewLine);
            Console.WriteLine();
            string connectionStringWithDb = connectionString + "Initial Catalog=" + options.DbName;


            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionStringsSection.ConnectionStrings["rbac"] != null)
                connectionStringsSection.ConnectionStrings["rbac"].ConnectionString = connectionString;
            else
                connectionStringsSection.ConnectionStrings.Add(new ConnectionStringSettings("rbac", connectionString));
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

        }
        
    }
}
