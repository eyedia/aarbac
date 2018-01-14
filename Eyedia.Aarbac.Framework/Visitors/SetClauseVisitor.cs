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

namespace Eyedia.Aarbac.Framework
{
    public class SetClauseVisitor : TSqlConcreteFragmentVisitor
    {
        public SetClauseVisitor(string tableName = null)
        {
            this.TableName = tableName;
        }
        public string TableName { get; }

        public List<RbacSelectColumn> Columns = new List<RbacSelectColumn>();

        public override void ExplicitVisit(AssignmentSetClause assignSetClause)
        {
            RbacSelectColumn column = new RbacSelectColumn();
            if (!string.IsNullOrEmpty(TableName))
                column.Table.Name = TableName;

            if (assignSetClause.Column.MultiPartIdentifier.Identifiers.Count == 1)
            {                
                column.Name = assignSetClause.Column.MultiPartIdentifier.Identifiers[0].Value;

            }
            else if (assignSetClause.Column.MultiPartIdentifier.Identifiers.Count == 2)
            {
                column.Table.Alias = assignSetClause.Column.MultiPartIdentifier.Identifiers[0].Value;
                column.Name = assignSetClause.Column.MultiPartIdentifier.Identifiers[1].Value;
            }
           
            Columns.Add(column);
        }
       
    }
}

