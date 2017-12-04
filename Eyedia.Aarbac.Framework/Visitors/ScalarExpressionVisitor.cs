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
        public ScalarExpressionVisitor(Rbac context, RbacTable table = null)
        {
            this.Context = context;
            this.Table = table;
        }
        public Rbac Context { get; private set; }
        public string ParsedQuery { get; private set; }
        public RbacTable Table { get; private set; }       

        public List<RbacSelectColumn> Columns = new List<RbacSelectColumn>();

        public override void ExplicitVisit(SelectStarExpression node)
        {            
            string query = String.Join(string.Empty, node.ScriptTokenStream.Select(sts => sts.Text).ToArray());
            
            string tableNameOrAlias = string.Empty;
            bool hasIdentifier = false;

            if (node.Qualifier != null)
            {
                tableNameOrAlias = node.Qualifier.Identifiers[0].Value;
                hasIdentifier = true;
            }
            else
            {
                int pos = node.ScriptTokenStream.Select((v, i) => new { token = v, index = i }).First(sts => sts.token.TokenType == TSqlTokenType.From).index;
                tableNameOrAlias = node.ScriptTokenStream[pos + 2].Text;

                if ((node.ScriptTokenStream.Count >
                    (pos + 2 + 2))
                    && (node.ScriptTokenStream[pos + 4].TokenType == TSqlTokenType.Identifier))
                {
                    //e.g. 'select * from Author a' getting 'a'
                    tableNameOrAlias = node.ScriptTokenStream[pos + 4].Text;
                }
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
                    }
                    else
                    {
                        column.Table.Name = tableNameOrAlias;                       
                    }

                    column.Name = col.Name;
                    Columns.Add(column);
                }

                if ((isAlias) && (hasIdentifier))
                    ParsedQuery = query.Replace(tableNameOrAlias + ".*", table.Columns.ToCommaSeparatedString(tableNameOrAlias));
                else
                    ParsedQuery = query.Replace("*", table.Columns.ToCommaSeparatedString(tableNameOrAlias));

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
                column.Table = Table;              
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
