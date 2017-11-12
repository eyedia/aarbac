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
