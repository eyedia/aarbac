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

            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                WriteErrorLine("Connection string is required, use -x <connection string>");
                errored = true;
            }

            if (errored)
                return;

            Rbac rbac = new Rbac();
            rbac.Callback += Rbac_Callback;
            Rbac newRbac = rbac.CreateNew(options.Name, options.Description, options.ConnectionString, string.Empty);
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


