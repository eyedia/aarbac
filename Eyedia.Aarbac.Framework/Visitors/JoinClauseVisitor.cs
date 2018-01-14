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

