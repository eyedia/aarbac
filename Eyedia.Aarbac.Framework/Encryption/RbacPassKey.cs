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


