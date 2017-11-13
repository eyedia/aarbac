using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
        public void ParseWhereClause(WhereClause aWhereClause)
        {
            EqualVisitor cidv = new EqualVisitor(ParsedQuery);
            InPredicateVisitor inpv = new InPredicateVisitor(ParsedQuery);
            aWhereClause.AcceptChildren(cidv);
            aWhereClause.AcceptChildren(inpv);

            WhereClauses.AddRange(cidv.WhereClauses);
            WhereClauses.AddRange(inpv.WhereClauses);

            WhereClauses.ParseReferenceTableNames(JoinClauses);
        }

        class EqualVisitor : TSqlConcreteFragmentVisitor
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

        class InPredicateVisitor : TSqlConcreteFragmentVisitor
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
}
