using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacPassKey
    {
        public static void CreatePassKey(string passKey)
        {
            try
            {
                RegistryKey software = Registry.LocalMachine.OpenSubKey("Software", true);
                if (software == null)
                {
                    Console.WriteLine("Could not access registry, please run this program 'As Administrator'");
                    return;
                }

                RegistryKey symplus = software.OpenSubKey("Symplus", true);
                if (symplus == null)
                {
                    symplus = software.CreateSubKey("Symplus");
                    symplus = software.OpenSubKey("Symplus", true);
                }
                RegistryKey version = symplus.OpenSubKey("Verion", true);
                if (version == null)
                {
                    version = symplus.CreateSubKey("Version");
                    //version = symplus.OpenSubKey("Verion", true);
                }
                version.SetValue("Key", passKey);
            }
            catch(System.Security.SecurityException ex)
            {
                RbacException.Raise("There was an error occurred, may be because RBAC is running first time on this computer, try executing RBAC with administratitve rights!");
            }
        }

        public static string GetPassKey()
        {
            RegistryKey software = Registry.LocalMachine.OpenSubKey("Software");
            if (software != null)
            {
                RegistryKey symplus = software.OpenSubKey("Symplus");
                if (symplus != null)
                {
                    RegistryKey version = symplus.OpenSubKey("Version");
                    if (version != null)
                    {
                        return version.GetValue("Key").ToString();
                    }
                }
            }

            return string.Empty;
        }

        public static byte[] GetPassKeyBytes()
        {
            string password = GetPassKey();
            if (!string.IsNullOrEmpty(password))                         
                return UTF8Encoding.UTF8.GetBytes(password);            
            return null;
        }

        public static void DeletePassKey()
        {
            RegistryKey software = Registry.LocalMachine.OpenSubKey("Software");
            if ((software != null) && (software.OpenSubKey("Symplus") != null))
                software.DeleteSubKey("Symplus");
        }
    }
}
