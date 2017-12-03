using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    public class JoinClauseVisitor : TSqlFragmentVisitor
    {
        public List<RbacJoin> JoinClauses = new List<RbacJoin>();
        public Rbac Context { get; set; }

        public JoinClauseVisitor(Rbac context)
        {
            Context = context;
        }


        public override void ExplicitVisit(QualifiedJoin aQualifiedJoin)
        {
            ScalarExpressionVisitor seVisitor = new ScalarExpressionVisitor(Context);
            aQualifiedJoin.AcceptChildren(seVisitor);
            AddJoin(aQualifiedJoin.QualifiedJoinType, seVisitor.Columns);

        }

        private void AddJoin(QualifiedJoinType joinType, List<RbacSelectColumn> columns)
        {

            for (int c = 0; c < columns.Count; c += 2)
            {
                RbacJoin ajoin = new RbacJoin();
                ajoin.JoinType = (RbacJoinTypes)Enum.Parse(typeof(RbacJoinTypes), joinType.ToString(), true);

                ajoin.FromTableName = columns[c].Table.Name;
                ajoin.FromTableAlias = columns[c].Table.Alias;
                ajoin.FromTableColumn = columns[c].Name;

                ajoin.WithTableName = columns[c + 1].Table.Name;
                ajoin.WithTableAlias = columns[c + 1].Table.Alias;
                ajoin.WithTableColumn = columns[c + 1].Name;

                if (string.IsNullOrEmpty(ajoin.FromTableName))
                {
                    RbacTable table = Context.User.Role.CrudPermissions.Find(ajoin.FromTableAlias);
                    ajoin.FromTableName = table.Name;
                }
                if (string.IsNullOrEmpty(ajoin.WithTableName))
                {
                    RbacTable table = Context.User.Role.CrudPermissions.Find(ajoin.FromTableAlias);
                    ajoin.WithTableName = table.Name;
                }
                JoinClauses.Add(ajoin);

            }
        }
    }
}
