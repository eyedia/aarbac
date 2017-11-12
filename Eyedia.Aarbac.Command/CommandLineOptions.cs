using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Command
{
    // Define a class to receive parsed values
    public class Options
    {
        [Option('c', "command", Required = false,
          HelpText = "Commanad.")]
        public string Command { get; set; }

        [Option('n', "name", Required = false,
          HelpText = "Name of the rbac.")]
        public string Name { get; set; }

        [Option('p', "password",
          HelpText = "Password of the rbac.")]
        public string Password { get; set; }

        [Option('d', Required = false, DefaultValue = "rbac",
         HelpText = "Name of the database, default is rbac.")]
        public string DbName { get; set; }

        [Option('s', "server", Required = false,
         HelpText = "MS Sql Server Name (e.g. 10.34.18.2,1100).")]
        public string SqlServer { get; set; }

        [Option('i', Required = false,
         HelpText = "Integrated Security).")]
        public bool IntegratedSecurity { get; set; }

        [Option('u', "ssuser", Required = false,
         HelpText = "MS Sql Server user name.")]
        public string SqlServerUserName { get; set; }

        [Option('w', "sspassword", Required = false,
         HelpText = "MS Sql Server user password.")]
        public string SqlServerPassword { get; set; }

        [Option('x', "description", Required = false,
         HelpText = "Description")]
        public string Description { get; set; }

        [Option('l', "appcs", Required = false,
         HelpText = "Application connection string")]
        public string AppCs { get; set; }

        [Option('f', "filename", Required = false,
        HelpText = "File name")]
        public string FileName { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Operations:");
            sb.AppendLine("\t: c - Create new rbac repository.");
            sb.AppendLine("\t: ic - Create new rbac instance for your application.");
            sb.AppendLine("\t: ie - Extract rbac instance");
            sb.AppendLine("\t: rs - Extract sample role xml");
            sb.AppendLine("\t: rc - Create new role. Use 'rs' command to get sample.");
            sb.AppendLine("\t: re - Extract existing role from {0}");
            sb.AppendLine("\t: uc - Create new user");
            sb.AppendLine("\t: ue - Extract existing user");
            sb.AppendLine("\t: q - Quit (Or Ctrl + C)");
            return sb.ToString();
        }
    }
}
