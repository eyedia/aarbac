using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
       private void UpdateReferredTables(List<ReferredTable> tables)
        {
            foreach (ReferredTable referredTable in tables)
            {
                UpdateReferredTables(referredTable.Name);
            }
        }

        private void UpdateReferredTables(RbacSelectColumns columns)
        {
            foreach (RbacSelectColumn column in columns.List)
            {
                UpdateReferredTables(column.ReferencedTableName);
            }
        }

        private void UpdateReferredTables(string tableName)
        {
            RbacTable actualTable = Context.User.Role.CrudPermissions.Find(tableName);
            if (actualTable != null)
                TablesReferred.Add(actualTable);
            else
                RbacException.Raise(string.Format("The referred table {0} was not found in meta data!", tableName),
                    RbacExceptionCategories.Parser);
        }
    }
}
