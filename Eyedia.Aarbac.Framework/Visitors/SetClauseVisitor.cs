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

        public List<RbacSelectColumn> Columns = new List<RbacSelectColumn>();

        public override void ExplicitVisit(AssignmentSetClause assignSetClause)
        {
            RbacSelectColumn column = new RbacSelectColumn();
            if (!string.IsNullOrEmpty(TableName))
                column.Table.Name = TableName;

            if (assignSetClause.Column.MultiPartIdentifier.Identifiers.Count == 1)
            {                
                column.Name = assignSetClause.Column.MultiPartIdentifier.Identifiers[0].Value;

            }
            else if (assignSetClause.Column.MultiPartIdentifier.Identifiers.Count == 2)
            {
                column.Table.Alias = assignSetClause.Column.MultiPartIdentifier.Identifiers[0].Value;
                column.Name = assignSetClause.Column.MultiPartIdentifier.Identifiers[1].Value;
            }
           
            Columns.Add(column);
        }
       
    }
}
