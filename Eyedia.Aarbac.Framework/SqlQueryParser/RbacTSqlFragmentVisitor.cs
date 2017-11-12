using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    internal class RbacTSqlFragmentVisitor : TSqlFragmentVisitor
    {
        internal List<string> Warnings { get; private set; }
        internal RbacTSqlFragmentVisitor()
        {
            Warnings = new List<string>();
        }

        public override void ExplicitVisit(UpdateStatement node)
        {
            
            if (node.UpdateSpecification.WhereClause == null)
            {
                Warnings.Add(String.Format("Missing where clause in the update statement at startLine {0} and startColumn {1}",
                    node.StartLine, node.StartColumn));
            }
            base.Visit(node);
        }

        public override void ExplicitVisit(DeleteStatement node)
        {
            if (node.DeleteSpecification.WhereClause == null)
            {
                Warnings.Add(String.Format("Missing where clause in the delete statement at startLine {0} and startColumn {1}",
                    node.StartLine, node.StartColumn));
            }
            base.Visit(node);
        }

        public override void ExplicitVisit(DropTableStatement node)
        {
            Warnings.Add(String.Format("Found a drop table statement at startLine {0} and startColumn {1}",
                    node.StartLine, node.StartColumn));//drop is not yet supported            
            base.Visit(node);
        }

    }
}
