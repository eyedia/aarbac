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
        private void ParseSelect(string query)
        {
            //Columns = new RbacSelectColumns();
            //TSql120Parser SqlParser = new TSql120Parser(false);
            //IList<ParseError> parseErrors;
            //TSqlFragment result = SqlParser.Parse(new StringReader(query), out parseErrors);
            //ParseErrors = parseErrors;
            //PrintErrors();

            TSqlScript sqlScript = InitiateTSql110Parser(query) as TSqlScript;
            foreach (TSqlBatch sqlBatch in sqlScript.Batches)
            {
                foreach (TSqlStatement sqlStatement in sqlBatch.Statements)
                {
                    ParseSqlSelectStatement(sqlStatement);
                }
            }
            if (IsNotSupported)
                return;
            if (ParsedMethod == RbacSelectQueryParsedMethods.ScriptDom)
            {
                GetReferredTables();
                IsParsed = true;
            }

        }

        private void ParseSqlSelectStatement(TSqlStatement sqlStatement)
        {
            if (sqlStatement.GetType() == typeof(SelectStatement))
            {               
                SelectStatement aSelectStatement = (SelectStatement)sqlStatement;                           
                QueryExpression aQueryExpression = aSelectStatement.QueryExpression;
                if (aQueryExpression.GetType() == typeof(QuerySpecification))
                {
                    QuerySpecification aQuerySpecification = (QuerySpecification)aQueryExpression;
                    int aSelectElementID = 0;
                    foreach (SelectElement aSelectElement in aQuerySpecification.SelectElements)
                    {
                        if (aSelectElement.GetType() == typeof(SelectScalarExpression))
                        {
                            SelectScalarExpression aSelectScalarExpression = (SelectScalarExpression)aSelectElement;

                            string identStr = string.Empty;
                            IdentifierOrValueExpression aIdentifierOrValueExpression =
                                aSelectScalarExpression.ColumnName;
                            if (aIdentifierOrValueExpression != null)
                            {
                                if (aIdentifierOrValueExpression.ValueExpression == null)
                                {                                    
                                    identStr = aIdentifierOrValueExpression.Identifier.Value;
                                }
                                else
                                {
                                    Errors.Add("Expression");
                                }
                            }
                            Columns.AddIfNeeded(aSelectElementID, identStr);

                            ScalarExpression aScalarExpression = aSelectScalarExpression.Expression;
                            ParseScalarExperssions(aSelectElementID, aScalarExpression);
                        }
                        else
                        {
                            //lets try using SqlCommand
                            if (!ParseUsingSqlCommand())
                            {
                                //If still fail then give up
                                Columns.AddIfNeeded(aSelectElementID,
                                    "Error, something else than SelectScalarExpression found");
                                Errors.Add("Currently only SelectScalarExpressions are supported!");
                                IsNotSupported = true;                                
                            }
                            return;
                        }
                        aSelectElementID = aSelectElementID + 1;                        
                    }                   
                    FromClause aFromClause = aQuerySpecification.FromClause;
                   
                    foreach (TableReference aTableReference in aFromClause.TableReferences)
                    {
                        ParseTableReferences(aTableReference);
                    }
                    if (aFromClause.TableReferences[0] is QualifiedJoin)
                    {
                        ParseJoins((QualifiedJoin)aFromClause.TableReferences[0]);
                        JoinClauses = JoinClauses.GroupBy(d => new { d.FromTableAlias, d.FromTableColumn, d.FromTableName, d.WithTableAlias, d.WithTableColumn, d.WithTableName })
                                      .Select(d => d.First())
                                      .ToList();
                    }
                }
                Columns.FillEmptyAlias(); 
            }
            else
            {
                Errors.Add("Not a select statement!");
            }
        }
    }
}
