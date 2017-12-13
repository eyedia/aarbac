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
        private void ParseUpdate(string query)
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
                        ParseSqlUpdateStatement(sqlStatement);
                    }
                }
            }
            RbacTSqlFragmentVisitor rbacTSqlFragmentVisitor = new RbacTSqlFragmentVisitor();
            tree.Accept(rbacTSqlFragmentVisitor);
            this.Warnings.AddRange(rbacTSqlFragmentVisitor.Warnings);
                        
            TablesReferred = new List<RbacTable>(TablesReferred.DistinctBy(t => t.Name));
            IsParsed = true;
        }

        private void ParseSqlUpdateStatement(TSqlStatement sqlStatement)
        {
            if (sqlStatement.GetType() == typeof(UpdateStatement))
            {
                UpdateStatement aUpdateStatement = (UpdateStatement)sqlStatement;

                #region Handle Target Table


                #endregion Handle Target Table

                RbacTable targetTable = new RbacTable();

                #region Handle From Clause - When Update with Join
                if (aUpdateStatement.UpdateSpecification.FromClause != null)
                {
                    //mostly update with join case
                    NamedTableReferenceVisitor fromClauseNtVisitor = new NamedTableReferenceVisitor(Context);
                    aUpdateStatement.UpdateSpecification.FromClause.AcceptChildren(fromClauseNtVisitor);
                    this.TablesReferred = fromClauseNtVisitor.Tables;
                    if (TablesReferred.Count > 0)
                    {
                        targetTable = TablesReferred[0];
                        //if alias is being updated, we need to fix table name
                        RbacTable tryTable = Context.User.Role.CrudPermissions.Find(targetTable.Name);
                        if (tryTable == null)
                        {
                            //alias is being updated, lets get the actual table name
                            var tt = fromClauseNtVisitor.Tables.Where(t => t.Alias == targetTable.Name).ToList()[0];
                            if (tt != null)
                                targetTable.Name = tt.Name;
                        }
                    }
                    else
                    {
                        RbacException.Raise("No target table found in the update query!");
                    }
                }
                else
                {                    
                    NamedTableReferenceVisitor ntVisitor = new NamedTableReferenceVisitor(Context);
                    aUpdateStatement.UpdateSpecification.Target.Accept(ntVisitor);

                    if (ntVisitor.Tables.Count == 0)
                        RbacException.Raise("No target table found in the update query!");
                    else if (ntVisitor.Tables.Count == 1)
                        targetTable = ntVisitor.Tables[0];
                    else
                        RbacException.Raise("More than 1 target tables found in the update query! Currently not supported.");

                    TablesReferred.Add(targetTable);
                }
                #endregion Handle From Clause - When Update with Join

                #region Handle Columns

                SetClauseVisitor scVisitor = new SetClauseVisitor(targetTable.Name);
                aUpdateStatement.UpdateSpecification.AcceptChildren(scVisitor);                
                Columns = scVisitor.Columns;
                UpdateReferredTables(Columns);                

                #endregion Handle Columns
            }
            else
            {
                Errors.Add("Not a update statement!");
            }
        }
    }

    

}

