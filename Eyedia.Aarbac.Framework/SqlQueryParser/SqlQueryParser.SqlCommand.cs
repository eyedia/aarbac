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

            try
            {
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
                        column.Name = row["BaseColumnName"].ToString();
                        column.Table.Name = row["BaseTableName"].ToString();
                        Columns.Add(column);
                        RbacTable table = Context.User.Role.CrudPermissions.Find(column.Table.Name);
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
            catch(Exception ex)
            {
                Errors.Add(ex.Message);
            }
            return false;
        }
    }
}


