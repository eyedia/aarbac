using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
   
    public class RbacOrderBy
    {
        public RbacOrderBy()
        {
            
        }

        public RbacOrderBy(string selfTableName, string name, string orderbyClause)
        {
            SelfTableName = selfTableName;
            Name = name;
            OrderbyClause = orderbyClause;           
        }

        public string SelfTableName { get; }
        public string Name { get; }        
        public string OrderbyClause { get; }

        public override string ToString()
        {
            return string.Format("<OrderBy name=\"{0}\" OrderBy =\"{1}\" />",
                Name, OrderbyClause);
        }

    }
}
