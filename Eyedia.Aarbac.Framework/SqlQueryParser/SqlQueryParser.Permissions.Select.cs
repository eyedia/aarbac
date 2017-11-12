using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {       
        public void ApplyPermissionSelect()
        {
            var tables = Columns.List.GroupBy(c => c.ReferencedTableName).Select(grp => grp.ToList()).ToList();
            foreach (var allColumnnsInATable in tables)
            {
                if (allColumnnsInATable.Count > 0)
                {
                    RbacTable rbacTable = TablesReferred.Find(allColumnnsInATable[0].ReferencedTableName);
                    if (rbacTable == null)
                        throw new Exception("Could not find table name in referred tables!");
                    if (rbacTable.AllowedOperations.HasFlag(RbacDBOperations.Read))
                    {
                        foreach (RbacSelectColumn column in allColumnnsInATable)
                        {
                            RbacColumn rbacColumn = rbacTable.FindColumn(column.TableColumnName);
                            if (!rbacColumn.AllowedOperations.HasFlag(RbacDBOperations.Read))
                                RemoveColumnFromSelect(column);
                        }
                    }
                    else
                    {
                        //user do not have access to this table
                    }
                }
            }

            IsPermissionApplied = true;
        }

        private void RemoveColumnFromSelect(RbacSelectColumn column)
        {

            int fromIndex = ParsedQuery.IndexOf(" from", StringComparison.OrdinalIgnoreCase);
            string selectStatement = ParsedQuery.Substring(0, fromIndex);
            string otherStatement = ParsedQuery.Substring(fromIndex, ParsedQuery.Length - fromIndex);
            if (!selectStatement.Contains(",")) //we hit the single/last column
            {
                ParsedQuery = "SELECT 'null' " + otherStatement;
                IsZeroSelectColumn = true;
            }
            else
            {
                string colName = column.TableColumnName;
                if (!string.IsNullOrEmpty(column.TableAlias))
                    colName = string.Format("{0}.{1}", column.TableAlias, column.TableColumnName);

                int pos = selectStatement.IndexOf(colName);
                int nextommaPos = selectStatement.IndexOf(",", pos);
                if (nextommaPos == -1) //we hit the last column
                    nextommaPos = selectStatement.Length;

                colName = selectStatement.Substring(pos, (nextommaPos - pos));
                selectStatement = selectStatement.Replace(colName, string.Empty);
                selectStatement = selectStatement.Replace(",  ,", ",").Replace(", ,", ",").Replace(",,", ",");
                selectStatement = selectStatement.Trim();
                if ((selectStatement.Length > 0) && (selectStatement.Substring(selectStatement.Length -1, 1) == ","))
                    selectStatement = selectStatement.Substring(0, selectStatement.Length - 1) + " ";

                if(selectStatement.Trim().Equals("SELECT", StringComparison.OrdinalIgnoreCase))
                {
                    selectStatement = "SELECT 'null' ";
                    IsZeroSelectColumn = true;
                }

                ParsedQuery = selectStatement + otherStatement;
                ParsedQuery = ParsedQuery.Replace("SELECT ,", "SELECT");
            }
        }
    }
}
