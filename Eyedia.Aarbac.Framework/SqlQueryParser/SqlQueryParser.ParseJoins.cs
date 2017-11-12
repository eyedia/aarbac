using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
        private void ParseJoins(QualifiedJoin aQualifiedJoin, RbacJoin ajoin = null)
        {
            if (aQualifiedJoin.FirstTableReference is QualifiedJoin)
            {
                ParseJoins((QualifiedJoin)aQualifiedJoin.FirstTableReference);
            }
            else if (aQualifiedJoin.FirstTableReference is NamedTableReference)
            {
                ParseJoin((QualifiedJoin)aQualifiedJoin);
            }

            if (aQualifiedJoin.SecondTableReference is QualifiedJoin)
            {
                ParseJoins((QualifiedJoin)aQualifiedJoin.SecondTableReference);
            }
            else if (aQualifiedJoin.SecondTableReference is NamedTableReference)
            {
                ParseJoin((QualifiedJoin)aQualifiedJoin);
            }

        }

        private void ParseJoin(QualifiedJoin aQualifiedJoin)
        {

            RbacJoin ajoin = new RbacJoin();
            ajoin.JoinType = (RbacJoinTypes)Enum.Parse(typeof(RbacJoinTypes), aQualifiedJoin.QualifiedJoinType.ToString(), true);

            if (aQualifiedJoin.FirstTableReference is NamedTableReference)
            {
                NamedTableReference namedTableReference = (NamedTableReference)aQualifiedJoin.FirstTableReference;
                ajoin.FromTableName = namedTableReference.SchemaObject.BaseIdentifier.Value;
                ajoin.FromTableAlias = namedTableReference.Alias.Value;
            }
            if (aQualifiedJoin.SecondTableReference is NamedTableReference)
            {
                NamedTableReference namedTableReference = (NamedTableReference)aQualifiedJoin.SecondTableReference;
                ajoin.WithTableName = namedTableReference.SchemaObject.BaseIdentifier.Value;
                ajoin.WithTableAlias = namedTableReference.Alias != null? namedTableReference.Alias.Value:string.Empty;
            }

            if (aQualifiedJoin.SearchCondition is BooleanComparisonExpression)
            {
                BooleanComparisonExpression boolComparison = (BooleanComparisonExpression)aQualifiedJoin.SearchCondition;
                if (boolComparison.FirstExpression is ColumnReferenceExpression)
                {
                    ColumnReferenceExpression columnComparison = (ColumnReferenceExpression)boolComparison.FirstExpression;
                    ajoin.WithTableAlias = columnComparison.MultiPartIdentifier.Identifiers[0].Value;
                    ajoin.WithTableColumn = columnComparison.MultiPartIdentifier.Identifiers[1].Value;
                }

                if (boolComparison.SecondExpression is ColumnReferenceExpression)
                {
                    ColumnReferenceExpression columnComparison = (ColumnReferenceExpression)boolComparison.SecondExpression;
                    ajoin.FromTableAlias = columnComparison.MultiPartIdentifier.Identifiers[0].Value;
                    ajoin.FromTableColumn = columnComparison.MultiPartIdentifier.Identifiers[1].Value;
                }
            }

            JoinClauses.Add(ajoin);
        }
    }
}
