using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    public class InPredicateVisitor : TSqlConcreteFragmentVisitor
    {
        public InPredicateVisitor(string query)
        {
            this.Query = query;
        }
        public string Query { get; }

        public List<RbacWhereClause> WhereClauses = new List<RbacWhereClause>();

        public override void Visit(InPredicate exp)
        {
            if (exp.Expression is ColumnReferenceExpression)
            {
                RbacWhereClause aWhereClause = new RbacWhereClause();
                aWhereClause.Literal = RbacWhereClauseLiterals.InExpression;
                aWhereClause.OnTableAlias = ((ColumnReferenceExpression)exp.Expression).MultiPartIdentifier.Identifiers.First().Value;
                aWhereClause.OnColumn = ((ColumnReferenceExpression)exp.Expression).MultiPartIdentifier.Identifiers.Last().Value;
                aWhereClause.WhereClauseString = Query.Substring(exp.StartOffset, exp.FragmentLength);

                foreach (var value in exp.Values)
                {
                    aWhereClause.ConditionValues.Add(((Literal)value).Value);
                }

                WhereClauses.Add(aWhereClause);
            }
        }
    }
}
