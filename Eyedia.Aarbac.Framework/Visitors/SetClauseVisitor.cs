using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    public class SetClauseVisitor : TSqlConcreteFragmentVisitor
    {
        public SetClauseVisitor(string tableName = null)
        {
            this.TableName = tableName;
        }
        public string TableName { get; }

        public RbacSelectColumns Columns = new RbacSelectColumns();

        public override void ExplicitVisit(AssignmentSetClause assignSetClause)
        {
            RbacSelectColumn column = new RbacSelectColumn();
            if (!string.IsNullOrEmpty(TableName))
                column.ReferencedTableName = TableName;

            if (assignSetClause.Column.MultiPartIdentifier.Identifiers.Count == 1)
            {                
                column.TableColumnName = assignSetClause.Column.MultiPartIdentifier.Identifiers[0].Value;

            }
            else if (assignSetClause.Column.MultiPartIdentifier.Identifiers.Count == 2)
            {
                column.TableAlias = assignSetClause.Column.MultiPartIdentifier.Identifiers[0].Value;
                column.TableColumnName = assignSetClause.Column.MultiPartIdentifier.Identifiers[1].Value;
            }
           
            Columns.Add(column);
        }
       
    }
}
