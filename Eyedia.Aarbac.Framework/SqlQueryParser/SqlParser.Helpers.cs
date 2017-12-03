using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {

        private void UpdateReferredTables(List<RbacSelectColumn> columns)
        {
            foreach (RbacSelectColumn column in columns)
            {
                if (string.IsNullOrEmpty(column.Table.Name))
                {
                    RbacTable table = TablesReferred.Find(column.Table.Alias);
                    if (table == null)
                    {
                        UpdateReferredTables(column.Table.Name, column.Table.Alias);
                    }
                    else
                    {
                        column.Table = table;
                    }

                }

            }
        }

        private void UpdateReferredTables(string tableName, string tableAlias)
        {
            RbacTable actualTable = Context.User.Role.CrudPermissions.Find(tableName);
            actualTable.Alias = tableAlias;

            if (actualTable != null)
                TablesReferred.Add(actualTable);
            else
                RbacException.Raise(string.Format("The referred table {0} was not found in meta data!", tableName),
                    RbacExceptionCategories.Parser);
        }
    }
}
