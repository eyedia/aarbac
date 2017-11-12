using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Aarbac.Framework;
using System.IO;

namespace Eyedia.Aarbac.Command
{
    public class CommandLineWorkerRole:CommandLineWorker
    {
        public CommandLineWorkerRole() { }

        public void CreateNew(Rbac rbac, Options options)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.FileName);
            RbacRole role = rbac.ImportRole(fileName);
            Console.WriteLine();
            WriteColor(ConsoleColor.Green, "Role from " + fileName + " imported into "
                + rbac.Name + ". The role id is:" + role.RoleId + "." + Environment.NewLine);
        }

        public void GetSample(Rbac rbac)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rbac.Name + "_sample_role.xml");
            RbacRole.GetSample(rbac).Export(fileName);
            WriteColor(ConsoleColor.Green, fileName + " exported." + Environment.NewLine);

        }
    }
}
