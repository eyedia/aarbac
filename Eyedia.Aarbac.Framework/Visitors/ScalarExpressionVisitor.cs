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
        public ScalarExpressionVisitor(Rbac context)
        {
            this.Context = context;            
        }
        public Rbac Context { get; private set; }
        public string ParsedQuery { get; private set; }

        public List<RbacSelectColumn> Columns = new List<RbacSelectColumn>();

        public override void ExplicitVisit(SelectStarExpression node)
        {
            string query = String.Join(string.Empty, node.ScriptTokenStream.Select(sts => sts.Text).ToArray());
            
            string tableNameOrAlias = string.Empty;

            if (node.Qualifier != null)
            {
                tableNameOrAlias = node.Qualifier.Identifiers[0].Value;
            }
            else
            {
                int pos = node.ScriptTokenStream.Select((v, i) => new { token = v, index = i }).First(sts => sts.token.TokenType == TSqlTokenType.From).index;
                tableNameOrAlias = node.ScriptTokenStream[pos + 2].Text;
            }

            bool isAlias = false;
            RbacTable table = Context.User.Role.CrudPermissions.Find(tableNameOrAlias, ref isAlias);
            if (table != null)
            {
                foreach (RbacColumn col in table.Columns)
                {
                    RbacSelectColumn column = new RbacSelectColumn();
                    if (isAlias)
                    {
                        column.Table.Alias = tableNameOrAlias;
                        column.Table.Name = table.Name;
                        ParsedQuery = query.Replace(tableNameOrAlias + ".*", table.Columns.ToCommaSeparatedString(tableNameOrAlias));
                    }
                    else
                    {
                        column.Table.Name = tableNameOrAlias;
                        ParsedQuery = query.Replace("*", table.Columns.ToCommaSeparatedString(tableNameOrAlias));
                    }

                    column.Name = col.Name;
                    Columns.Add(column);
                }
            }
            else
            {
                RbacException.Raise(string.Format("The referred table {0} was not found in meta data!", tableNameOrAlias),
                    RbacExceptionCategories.Parser);
            }

        }

        public override void ExplicitVisit(ColumnReferenceExpression node)
        {
            RbacSelectColumn column = new RbacSelectColumn();           

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
