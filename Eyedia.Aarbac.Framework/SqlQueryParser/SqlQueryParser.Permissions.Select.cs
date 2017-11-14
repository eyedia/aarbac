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
using System.Diagnostics;
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

                            if (rbacColumn == null)
                                RbacException.Raise(
                                    string.Format("Role '{0}' belongs to '{1}' is not in sync with database. The column '{2}' of table '{3}' was not found in the role meta data",
                                    this.Context.User.UserName, this.Context.User.Role.Name, column.TableColumnName, column.ReferencedTableName));
                            
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
            //if (column.TableColumnName == "SSN")
            //    Debugger.Break();

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
                else if (!string.IsNullOrEmpty(column.ReferencedTableName))
                    colName = string.Format("{0}.{1}", column.ReferencedTableName, column.TableColumnName);

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

