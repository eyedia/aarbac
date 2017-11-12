using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    [DataContract]
    public class RbacEncryptedColumn
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }

        public RbacEncryptedColumn(string columnName, string tableName)
        {
            this.ColumnName = columnName;
            this.TableName = tableName;
        }
    }
}
