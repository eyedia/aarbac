using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public enum RbacWhereClauseLiterals
    {
        Unknown,
        BooleanExpression,
        InExpression
    }


    public class RbacWhereClause
    {
        public string OnTable { get; set; }       
        public string OnTableAlias { get; set; }
        public string OnColumn { get; set; }
        public RbacWhereClauseLiterals Literal { get; set; }
        public string WhereClauseString { get; set; }
        /// <summary>
        /// When literal is 'BooleanExpression' this will have one value
        /// where literal is 'InExpression' this will have multiple values
        /// </summary>
        public List<string> ConditionValues { get; set; }

        public RbacWhereClause()
        {
            ConditionValues = new List<string>();
        }

        
    }
}
