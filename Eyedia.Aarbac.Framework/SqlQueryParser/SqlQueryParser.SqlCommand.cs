using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
        
        public bool ParseUsingSqlCommand()
        {
            if (TablesReferred == null)
                TablesReferred = new List<RbacTable>();
            else
                TablesReferred.Clear();
            using (SqlConnection connection = new SqlConnection(Context.ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(OriginalQuery, connection);
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo);
                DataTable schemaTable = reader.GetSchemaTable();
                foreach (DataRow row in schemaTable.Rows)
                {
                    //if (row["BaseTableName"].ToString() == "City")
                    //    Debugger.Break();

                    RbacSelectColumn column = new RbacSelectColumn();
                    column.Alias = row["ColumnName"].ToString();
                    column.TableColumnName = row["BaseColumnName"].ToString();
                    column.ReferencedTableName = row["BaseTableName"].ToString();
                    Columns.Add(column);
                    RbacTable table = Context.User.Role.CrudPermissions.Find(column.ReferencedTableName);
                    if (table != null)
                        TablesReferred.Add(table);
                    else
                        throw new Exception(string.Format("The referred table {0} was not found in meta data!", row["BaseTableName"].ToString()));
                }

                TablesReferred = new List<RbacTable>(TablesReferred.DistinctBy(t => t.Name));
                connection.Close();
            }
            ParsedMethod = RbacSelectQueryParsedMethods.CommandBehavior;
            ParsedQuery = ParsedQuery.Replace("*", Columns.ToCommaSeparatedString());
            IsParsed = true;
            return true;
        }
    }
}
