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
        private TSqlFragment InitiateTSql110Parser(string query)
        {
            Columns = new RbacSelectColumns();
            TablesReferred = new List<RbacTable>();
            IList<ParseError> parseErrors;
            TSql110Parser parser = new TSql110Parser(true);
            TSqlFragment tree = parser.Parse(new StringReader(query), out parseErrors);
            ParseErrors = parseErrors;
            PrintErrors();
            SyntaxError = ParseErrors.Count > 0 ? true : false;
            return tree;
        }
        private void ParseInsert(string query)
        {
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
                        ParseSqlInsertStatement(sqlStatement);
                    }
                }
            }            
            //tree.Accept(new RbacTSqlFragmentVisitor());            
            TablesReferred = new List<RbacTable>(TablesReferred.DistinctBy(t => t.Name));
            IsParsed = true;
        }

        private void ParseSqlInsertStatement(TSqlStatement sqlStatement)
        {
            if (sqlStatement.GetType() == typeof(InsertStatement))
            {
                InsertStatement aInsertStatement = (InsertStatement)sqlStatement;
                string tableName = string.Empty;
                string tableAlias = string.Empty;

                if (aInsertStatement.InsertSpecification.Target is NamedTableReference)
                {
                    NamedTableReference tableRef = aInsertStatement.InsertSpecification.Target as NamedTableReference;
                    tableName = ((SchemaObjectName)tableRef.SchemaObject).Identifiers[1].Value;
                    tableAlias = tableRef.Alias != null ? tableRef.Alias.ToString() : string.Empty;
                }

                foreach (ColumnReferenceExpression columnExp in aInsertStatement.InsertSpecification.Columns)
                {
                    
                    var columnName = columnExp.MultiPartIdentifier.Identifiers[0].Value;

                    RbacSelectColumn column = new RbacSelectColumn();
                    column.Alias = string.Empty;
                    column.TableColumnName = columnName;
                    column.ReferencedTableName = tableName;
                    Columns.Add(column);
                    RbacTable table = Context.User.Role.CrudPermissions.Find(column.ReferencedTableName);
                    if (table != null)
                        TablesReferred.Add(table);
                    else
                        RbacException.Raise(string.Format("The referred table {0} was not found in meta data!", tableName), RbacExceptionCategories.Parser);
                }
                
                Columns.FillEmptyAlias();
            }
            else
            {
                Errors.Add("Not a update statement!");
            }
        }
    }
   

}

