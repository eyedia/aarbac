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
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
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

            IsParsed = true;
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
                    if (aQuerySpecification.FromClause == null)
                        return; //'select *' is valid query, but not of our interest
                    
                    //tables
                    NamedTableReferenceVisitor ntVisitor = new NamedTableReferenceVisitor(Context);
                    aQuerySpecification.FromClause.Accept(ntVisitor);
                    TablesReferred = ntVisitor.Tables;                   

                    //columns
                    ScalarExpressionVisitor seVisitor = null;
                    foreach (SelectElement selectStatement in aQuerySpecification.SelectElements)
                    {
                        seVisitor = new ScalarExpressionVisitor(Context, TablesReferred[0]);
                        selectStatement.Accept(seVisitor);
                        Columns.AddRange(seVisitor.Columns);                       
                    }

                    if (!string.IsNullOrEmpty(seVisitor.ParsedQuery))
                        ParsedQuery = seVisitor.ParsedQuery;    //todo
                    //else
                    //    ParsedQuery = Columns.GetParsedQuery(ParsedQuery);

                    UpdateReferredTables(Columns);                    

                    //ensure unique tables
                    TablesReferred = new List<RbacTable>(TablesReferred.DistinctBy(t => t.Name));

                    //joins
                    JoinClauseVisitor jcVisitor = new JoinClauseVisitor(Context);
                    aQueryExpression.AcceptChildren(jcVisitor);
                    JoinClauses = jcVisitor.JoinClauses;

                    //where clause
                    if (aQuerySpecification.WhereClause != null)
                    {
                        EqualVisitor cidv = new EqualVisitor(ParsedQuery);
                        InPredicateVisitor inpv = new InPredicateVisitor(ParsedQuery);
                        aQuerySpecification.WhereClause.AcceptChildren(cidv);
                        aQuerySpecification.WhereClause.AcceptChildren(inpv);
                        WhereClauses.AddRange(cidv.WhereClauses);
                        WhereClauses.AddRange(inpv.WhereClauses);
                        WhereClauses.ParseReferenceTableNames(JoinClauses);
                    }
                }
                
            }
            else
            {
                Errors.Add("Not a select statement!");
            }
        }
       
    }
}


