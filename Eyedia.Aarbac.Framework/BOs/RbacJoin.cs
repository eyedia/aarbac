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

namespace Eyedia.Aarbac.Framework
{
    public class RbacJoin
    {
        public string FromTableName { get; internal set; }
        public string FromTableAlias { get; internal set; }
        public string FromTableColumn { get; internal set; }
        public string WithTableName { get; internal set; }
        public string WithTableAlias { get; internal set; }
        public string WithTableColumn { get; internal set; }
        public RbacJoinTypes JoinType { get; set; }

        public string JoinClause { get; internal set; }
        public bool WasFromBaseQuery { get; }

        public RbacJoin()
        {
            WasFromBaseQuery = true;
        }

        public RbacJoin(string fromTableName, string fromTableNameAlias,
            string withTableName, string withTableNameAlias)
        {
            if (string.IsNullOrEmpty(fromTableNameAlias))
                fromTableNameAlias = fromTableName;
            if (string.IsNullOrEmpty(withTableNameAlias))
                withTableNameAlias = withTableName;

            FromTableName = fromTableName;
            FromTableAlias = fromTableNameAlias;
            WithTableName = withTableName;
            WithTableAlias = withTableNameAlias;

            //WasFromBaseQuery = true;
        }

        private RbacJoin(string withTableName, string withTableAlias)
        {
            WithTableName = withTableName;
            WithTableAlias = withTableAlias;            
        }

        public static RbacJoin AddNewJoin(string fromTableName, string fromTableNameAlias,
            string withTableName, string withTableNameAlias)
        {
            return new Framework.RbacJoin(fromTableName, fromTableNameAlias, withTableName, withTableNameAlias);
        }

        public static RbacJoin AddNewJoin(SqlQueryParser sqlQueryParser, string fromTableName, string fromTableColumn,
            string withTableName, string withTableColumn)
        {
            //RbacJoin joinClause = existingJoins.Where(jc => jc.WithTableName.Equals(withTableName).SingleOrDefault();
            RbacJoin join = sqlQueryParser.JoinClauses.JoinExists(withTableName, fromTableName);
            if (join == null)
            {
                string fromTableNameAlias = sqlQueryParser.GetTableNameOrAlias(fromTableName);
                aliasNumber++;
                string withTableAlias = "t" + aliasNumber;
                join = new RbacJoin(fromTableName, fromTableNameAlias, withTableName, withTableAlias);
                join.FromTableColumn = fromTableColumn;
                join.WithTableColumn = withTableColumn;

                join.JoinClause = string.Format(" {0} join [{1}] [{2}] on [{2}].{3} = [{4}].{5} ", join.JoinType.ToString().ToLower(),
                withTableName, withTableAlias, withTableColumn, fromTableNameAlias, fromTableColumn);

                sqlQueryParser.JoinClauses.Add(join);
                return join;
            }
            else
            {
                return join;
            }
        }

        static int aliasNumber;
        
    }
}


