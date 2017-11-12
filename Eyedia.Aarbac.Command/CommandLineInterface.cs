using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Aarbac.Framework;

namespace Eyedia.Aarbac.Command
{
    public class CommandLineInterface:CommandLineWorker
    {
        public Rbac Rbac { get; private set; }

        public CommandLineInterface()
        {

        }
        private bool Authenticate(Options options)
        {
            if (string.IsNullOrEmpty(options.Name))
            {
                WriteErrorLine("Rbac name is required. Please use -n <name>");
                return false;
            }

            if (string.IsNullOrEmpty(options.Password))
            {
                WriteErrorLine("Rbac password is required. Please use -p <password>");
                return false;
            }

            Rbac = Rbac.GetRbac(options.Name);
            return Rbac.Authenticate(options.Password);
        }
        public void Do(Options options)
        {
            try
            {
                switch (options.Command)
                {
                    case "c":
                        new CommandLineWorkerDba().CreateNew(options);
                        break;

                    case "ic":                      
                        new CommandLineWorkerRbac().CreateNew(options);
                        break;

                    case "ie":
                        if (!Authenticate(options))
                            return;
                        new CommandLineWorkerRbac().Extract(options, Rbac);
                        break;

                    case "rs":
                        if (!Authenticate(options))
                            return;
                        new CommandLineWorkerRole().GetSample(Rbac);
                        break;

                    case "rc":
                        if (!Authenticate(options))
                            return;
                        new CommandLineWorkerRole().CreateNew(Rbac, options);
                        break;

                    case "q":
                        break;
                }
            }
            catch(Exception ex)
            {
                WriteErrorLine(ex.Message);
            }
        }
    }
}
