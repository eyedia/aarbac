using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    public class ScalarExpressionVisitor : TSqlFragmentVisitor
    {        
        public ScalarExpressionVisitor(string tableName = null)
        {
            this.TableName = tableName;
        }
        public string TableName { get; }

        public RbacSelectColumns Columns = new RbacSelectColumns();

        public override void ExplicitVisit(ColumnReferenceExpression node)
        {
            RbacSelectColumn column = new RbacSelectColumn();
            if (!string.IsNullOrEmpty(TableName))
                column.Table.Name = TableName;

            if (node.MultiPartIdentifier.Identifiers.Count == 1)
            {
                column.Name = node.MultiPartIdentifier.Identifiers[0].Value;

            }
            else if (node.MultiPartIdentifier.Identifiers.Count == 2)
            {
                column.Table.Alias = node.MultiPartIdentifier.Identifiers[0].Value;
                column.Name = node.MultiPartIdentifier.Identifiers[1].Value;
            }

            Columns.Add(column);

        }
    }
}
