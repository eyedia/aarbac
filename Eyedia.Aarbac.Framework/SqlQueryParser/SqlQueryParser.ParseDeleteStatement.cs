using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
        private void ParseDelete(string query)
        {
            //Columns = new RbacSelectColumns();
            //TablesReferred = new List<RbacTable>();
            //IList<ParseError> parseErrors;
            //TSql110Parser parser = new TSql110Parser(true);
            //TSqlFragment tree = parser.Parse(new StringReader(query), out parseErrors);
            //ParseErrors = parseErrors;
            //PrintErrors();
            TSqlFragment tree = InitiateTSql110Parser(query);
            if (SyntaxError)
                return;

            if (tree is TSqlScript)
            {
                TSqlScript tSqlScript = tree as TSqlScript;
                foreach (TSqlBatch sqlBatch in tSqlScript.Batches)
                {
                    foreach (TSqlStatement sqlStatement in sqlBatch.Statements)
                    {
                        ParseSqlDeleteStatement(sqlStatement);
                    }
                }
            }
            RbacTSqlFragmentVisitor rbacTSqlFragmentVisitor = new RbacTSqlFragmentVisitor();
            tree.Accept(rbacTSqlFragmentVisitor);
            this.Warnings.AddRange(rbacTSqlFragmentVisitor.Warnings);

            TablesReferred = new List<RbacTable>(TablesReferred.DistinctBy(t => t.Name));
            IsParsed = true;
        }

        private void ParseSqlDeleteStatement(TSqlStatement sqlStatement)
        {
            if (sqlStatement.GetType() == typeof(DeleteStatement))
            {
                DeleteStatement aDeleteStatement = (DeleteStatement)sqlStatement;
                string tableName = string.Empty;
                string tableAlias = string.Empty;

                if (aDeleteStatement.DeleteSpecification.Target is NamedTableReference)
                {
                    NamedTableReference tableRef = aDeleteStatement.DeleteSpecification.Target as NamedTableReference;
                    tableName = ((SchemaObjectName)tableRef.SchemaObject).Identifiers[0].Value;
                    tableAlias = tableRef.Alias != null ? tableRef.Alias.ToString() : string.Empty;

                    RbacTable table = Context.User.Role.CrudPermissions.Find(tableName);
                    if (table != null)
                        TablesReferred.Add(table);
                    else
                        RbacException.Raise(string.Format("The referred table {0} was not found in meta data!", tableName), RbacExceptionCategories.Parser);

                }                
                
            }
            else
            {
                Errors.Add("Not a update statement!");
            }
        }
    }
   

}
