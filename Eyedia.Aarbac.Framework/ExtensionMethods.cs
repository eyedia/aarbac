using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public static class ExtensionMethods
    {
        public static RbacTable Find(this List<RbacTable> tables, string tableName)
        {
            if (tables != null)
                return tables.Where(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            return null;
        }

        public static RbacColumn FindColumn(this RbacTable table, string columnName)
        {
            return table.Columns.Where(c => c.Name.Equals(columnName)).SingleOrDefault();
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>( this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        public static string GetTableNameOrAlias(this SqlQueryParser sqlQueryParser, string tableName)
        {
            List<RbacSelectColumn> filtered = sqlQueryParser.Columns.List.Where(c => c.ReferencedTableName.ToLower() == tableName.ToLower()).ToList();
            if ((filtered.Count > 0) && (string.IsNullOrEmpty(filtered[0].TableAlias) == false))
                return filtered[0].TableAlias;

            RbacTable tempReferredtable = sqlQueryParser.TablesReferred.Where(t => t.ReferencedOnly && (t.Name == tableName)).SingleOrDefault();
            if (tempReferredtable != null)
                return tempReferredtable.TempAlias;

            return tableName;
        }

        public static string ToTextAndFixSpaces(this SqlQueryStringParser.SqlStringParser stringParser)
        {
            string parsedString = stringParser.ToText();
            //fixing spaces
            parsedString = parsedString.Replace("[ ", "[").Replace(" ]", "]").Replace(" . ", ".")
                .Replace("( ", "(").Replace(" )", ")");
          
            return parsedString;
        }

        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }

        public static void UpdateJoinClause(this List<RbacJoin> joins, string withTable, string joinClause)
        {
            joins.Where(jc => jc.WithTableName == withTable).SingleOrDefault().JoinClause = joinClause;
        }

        public static string ParseQuery(this List<RbacJoin> joins, string parsedQuery)
        {
            //string joinClause = joins.Select(i => i.JoinClause).Aggregate((i, j) => i + " " + j);
            //int position = originalQuery.IndexOf("where", StringComparison.OrdinalIgnoreCase);
            //if (position == -1)
            //    parsedQuery = originalQuery + joinClause;
            //else
            //    parsedQuery = parsedQuery.Insert(position, joinClause);

            if (joins.Count > 0)
            {
                string joinClause = joins.Select(i => i.JoinClause).Aggregate((i, j) => i + " " + j);
                int position = parsedQuery.IndexOf("where", StringComparison.OrdinalIgnoreCase);
                if (position == -1)
                    parsedQuery = parsedQuery + joinClause;
                else
                    parsedQuery = parsedQuery.Insert(position, joinClause);
            }
            return parsedQuery;
        }

        public static RbacJoin JoinExists(this List<RbacJoin> joins, string withTableOrAlias, string fromTableOrAlias)
        {
            return joins.Where(jc => ((jc.WithTableName.Equals(withTableOrAlias, StringComparison.OrdinalIgnoreCase)) 
            || (jc.WithTableAlias.Equals(withTableOrAlias, StringComparison.OrdinalIgnoreCase)))
            && ((jc.FromTableName.Equals(fromTableOrAlias, StringComparison.OrdinalIgnoreCase))
            || (jc.FromTableName.Equals(fromTableOrAlias, StringComparison.OrdinalIgnoreCase)))).SingleOrDefault();            
        }

        public static void AddParameter(this RbacUser user, string paramName, string paramValue)
        {
            if (user != null)
            {
                RbacParameter newParam = new DataManager.Manager(false).AddOrUpdateUserParameter(user.UserId, paramName, paramValue);
                user.Parameters.Add(newParam.Name, newParam.Value);
            }
        }

        public static string ToLine(this List<string> listOfString)
        {

            return listOfString.Count > 0 ? listOfString.Select(i => i).Aggregate((i, j) => i + Environment.NewLine + j) : string.Empty;
        }

        public static bool ChangePassword(this Rbac rbac, string password)
        {
            return new DataManager.Manager().ChangePasswordRbac(rbac.Name, password);
        }

        public static void Export(this RbacRole role, string fileName)
        {           
            RbacRoleWeb wRole = new RbacRoleWeb(role);
            StreamWriter sw = new StreamWriter(fileName);
            var s = new System.Xml.Serialization.XmlSerializer(wRole.GetType());
            s.Serialize(sw, wRole);
            sw.Close();
        }

        public static RbacRole ImportRole(this Rbac rbac, string fileName)
        {
            RbacRoleWeb wRole = null;
            StreamReader sr = new StreamReader(fileName);
            var s = new System.Xml.Serialization.XmlSerializer(typeof(RbacRoleWeb));
            wRole = (RbacRoleWeb)s.Deserialize(sr);
            sr.Close();

            if (wRole != null)
            {
                RbacRole role = new RbacRole(rbac.RbacId, wRole.Name, wRole.Description, wRole.MetaDataRbac, wRole.MetaDataEntitlements);
                return role;
            }
            return null;
        }

        public static void ToCsv(this DataTable table, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in table.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(fileName, sb.ToString());
        }
    }
}