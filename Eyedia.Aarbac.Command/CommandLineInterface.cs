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

