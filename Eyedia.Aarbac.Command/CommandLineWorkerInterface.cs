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
 DISCLAIMED. IN NO EVENT SHALL Synechron Holdings Inc BE LIABLE FOR ANY
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
using System.Diagnostics;

namespace Eyedia.Aarbac.Command
{
    public class CommandLineWorkerInterface : CommandLineWorker
    {
        public Rbac Rbac { get; private set; }

        public CommandLineWorkerInterface()
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
            //Debugger.Launch();
            try
            {
                switch (options.Command)
                {
                    case "id":
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

                    case "ss":
                        new BookStore().Setup(options);
                        break;

                    case "stb":
                        new BookStore().TestBatch();
                        break;

                    case "st1":
                        new BookStore().TestOne();
                        break;

                    case "q":
                        break;

                    default:                      
                       Console.Write(Resources.Commands);
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteErrorLine(ex.Message);
            }
        }
    }
}

