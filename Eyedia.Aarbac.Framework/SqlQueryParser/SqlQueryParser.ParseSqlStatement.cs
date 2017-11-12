using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
        private void ParseInternal(string query)
        {
            if (QueryType == RbacQueryTypes.Select)
                ParseSelect(query);
            else if (QueryType == RbacQueryTypes.Insert)
                ParseInsert(query);
            else if (QueryType == RbacQueryTypes.Update)
                ParseUpdate(query);
            else if (QueryType == RbacQueryTypes.Delete)
                ParseDelete(query);
        }        

    }
}
