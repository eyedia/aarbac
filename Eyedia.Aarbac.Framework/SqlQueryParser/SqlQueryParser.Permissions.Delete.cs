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

        public void ApplyPermissionDelete()
        {            
            foreach (var table in TablesReferred)
            {

                if (!table.AllowedOperations.HasFlag(RbacDBOperations.Delete))
                {
                    RbacException.Raise(string.Format("User '{0}' does not have permission to delete record from table '{1}'!",
                                Context.User.UserName, table.Name), RbacExceptionCategories.Parser);
                }

            }

            IsPermissionApplied = true;
        }
        
    }
}
