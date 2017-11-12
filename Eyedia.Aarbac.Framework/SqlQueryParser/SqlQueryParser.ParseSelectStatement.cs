#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Synechron Holdings Inc BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#region Developer Information
/*
Author  - Debjyoti Das (debjyoti@debjyoti.com)
Created - 11/12/2017 3:31:31 PM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice
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

