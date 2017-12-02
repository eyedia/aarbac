using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    public class EqualVisitor : TSqlConcreteFragmentVisitor
    {
        public EqualVisitor(string query)
        {
            this.Query = query;
        }
        public string Query { get; }

        public List<RbacWhereClause> WhereClauses = new List<RbacWhereClause>();

        public override void Visit(BooleanComparisonExpression exp)
        {
            if (exp.FirstExpression is ColumnReferenceExpression)
            {
                RbacWhereClause aWhereClause = new RbacWhereClause();
                aWhereClause.Literal = RbacWhereClauseLiterals.BooleanExpression;
                aWhereClause.OnTableAlias = ((ColumnReferenceExpression)exp.FirstExpression).MultiPartIdentifier.Identifiers.First().Value;
                aWhereClause.OnColumn = ((ColumnReferenceExpression)exp.FirstExpression).MultiPartIdentifier.Identifiers.Last().Value;
                aWhereClause.ConditionValues.Add(((Literal)exp.SecondExpression).Value);
                aWhereClause.WhereClauseString = Query.Substring(exp.StartOffset, exp.FragmentLength);
                WhereClauses.Add(aWhereClause);
            }
        }
    }
}
