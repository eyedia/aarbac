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
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    /*
    public class RbacContext : IDisposable
    {
        public string ConnectionString { get; }
        public RbacUser User { get; }
        public string MetaData { get; }
        public StringWriterTrace Trace { get; }
        public string InstanceId { get; }
        public RbacContext(RbacUser user)
        {
            var cs = ConfigurationManager.ConnectionStrings["rbac"];
            if (cs == null)
                RbacException.Raise(MessagesConfigurations.conn_str_not_found, RbacExceptionCategories.Configuration);

            this.ConnectionString = cs.ConnectionString;
            this.User = user;
            this.InstanceId = Guid.NewGuid().ToString();
            this.Trace = new StringWriterTrace();
            Trace.WriteLine(InstanceId);
        }

        int indentLevel = 0;
        public void WriteTrace(string message)
        {
            string prefix = string.Empty;
            Trace.Write(new string('\t', indentLevel) + prefix + message);
        }

        #region Overriden Members

        public override string ToString()
        {
            return Trace.ToString();
        }
        public void Dispose()
        {
            if (this.Trace != null)
            {
                this.Trace.Close();
                this.Trace.Dispose();
            }
        }

        #endregion
    }*/
}

