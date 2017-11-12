using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Aarbac.Framework;
using System.Configuration;
using System.IO;

namespace Eyedia.Aarbac.Command
{
    public class CommandLineWorkerRbac: CommandLineWorker
    {        
        public CommandLineWorkerRbac():base()
        {
            
        }
        public void CreateNew(Options options)
        {
            bool errored = false;
            if (string.IsNullOrEmpty(options.Name))
            {
                WriteErrorLine("Rbac name is required");
                errored = true;
            }

            if (string.IsNullOrEmpty(options.AppCs))
            {
                WriteErrorLine("Application connection string is required");
                errored = true;
            }

            if (errored)
                return;

            Rbac rbac = new Rbac();
            rbac.Callback += Rbac_Callback;
            Rbac newRbac = rbac.CreateNew(options.Name, options.Description, options.AppCs, string.Empty);
            rbac.ChangePassword(options.Password);
            WriteColor(ConsoleColor.Green, "Done!" + Environment.NewLine);
            Console.WriteLine();
            Console.Write("Rbac '{0}' was created with id '{1}. Now it's time to configure some roles & users in the RBAC website.",
                options.Name, newRbac.RbacId);
            Console.WriteLine();

        }

        public void Extract(Options options, Rbac rbac)
        {            
            try
            {
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rbac.Name + ".xml");
                rbac.Export(fileName);
                WriteColor(ConsoleColor.Green, fileName + " exported.");

            }
            catch (Exception ex)
            {
                WriteErrorLine(ex.Message);
            }

        }

        private void Rbac_Callback(string message, LogMessageTypes messageType)
        {
            switch(messageType)
            {
                case LogMessageTypes.Info:
                    Console.Write(message);
                    break;
                case LogMessageTypes.Success:
                    WriteColor(ConsoleColor.Green, message + Environment.NewLine);
                    break;
                case LogMessageTypes.Fail:
                    WriteErrorLine(message);
                    break;
            }
        }
    }
}
