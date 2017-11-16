using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eyedia.Aarbac.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {                
                if(SetDataDirectory())
                    Application.Run(new frmMain());
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool SetDataDirectory()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Eyedia.Aarbac.Framework\Databases");
            if(!System.IO.Directory.Exists(path))
                path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");
            if (!System.IO.Directory.Exists(path))
            {
                string msg = "Database files not found! Demo expects .mdf files in " + System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases")  + Environment.NewLine;
                msg += "Microsoft SQL Server or Express 2014 + required.";

                MessageBox.Show(msg,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var fullPath = System.IO.Path.GetFullPath(path);
            AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
            return true;
        }
    }
}
