using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //try
            //{                
                if(SetDataDirectory())
                    Application.Run(new frmMain());
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private static bool SetDataDirectory()
        {
            string codingdir = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
                       
            var path = codingdir.Substring(0, codingdir.LastIndexOf("\\")) + @"\Eyedia.Aarbac.Command\Samples\Databases";
            if (!Directory.Exists(path))           
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "Samples");           

            if (!Directory.Exists(path))           
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");          

            if (!Directory.Exists(path))         
                path = Path.Combine(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName, "App_Data");

            if (!Directory.Exists(path))
            {
                string msg = "Database (.MDF) files not found! Pease set connection string using Tools-->Connection String";
                MessageBox.Show(msg,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            var fullPath = Path.GetFullPath(path);
            AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
            return true;
        }
    }
}
