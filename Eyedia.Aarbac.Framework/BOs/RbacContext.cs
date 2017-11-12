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
