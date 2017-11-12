using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {       

        public void ApplyPermissionUpdate()
        {
            var tables = Columns.List.GroupBy(c => c.ReferencedTableName).Select(grp => grp.ToList()).ToList();
            foreach (var allColumnnsInATable in tables)
            {
                if (allColumnnsInATable.Count > 0)
                {
                    RbacTable rbacTable = TablesReferred.Find(allColumnnsInATable[0].ReferencedTableName);
                    if (rbacTable == null)
                        throw new Exception("Could not find table name in referred tables!");
                    if (rbacTable.AllowedOperations.HasFlag(RbacDBOperations.Update))
                    {
                        foreach (RbacSelectColumn column in allColumnnsInATable)
                        {
                            RbacColumn rbacColumn = rbacTable.FindColumn(column.TableColumnName);
                            if (!rbacColumn.AllowedOperations.HasFlag(RbacDBOperations.Update))
                                RbacException.Raise(string.Format("User '{0}' has permission to update table '{1}', however has no permission to update column '{2}'!",
                                    Context.User.UserName, rbacTable.Name, rbacColumn.Name), RbacExceptionCategories.Parser);
                        }
                    }
                    else
                    {
                        RbacException.Raise(string.Format("User '{0}' does not have permission to update table '{1}'!",
                                    Context.User.UserName, rbacTable.Name), RbacExceptionCategories.Parser);
                    }
                }
            }

            IsPermissionApplied = true;
        }
        
    }
}
