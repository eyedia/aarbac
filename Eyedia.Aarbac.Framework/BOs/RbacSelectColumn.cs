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
using System.Collections.ObjectModel;

namespace Eyedia.Aarbac.Framework
{
    public class RbacSelectColumn
    {       
        public string Alias { get; set; }
        public string Name { get; set; }
        public RbacTable Table { get; set; }

        public RbacSelectColumn()
        {
            Table = new RbacTable();
        }

    }

    public class RbacSelectColumns
    {
        private List<RbacSelectColumn> _List = new List<RbacSelectColumn>();

        public List<RbacSelectColumn> List
        {
            get { return _List; }
            set { _List = value; }
        }

        public void Add(RbacSelectColumn column)
        {
            this._List.Add(column);
        }

        public void FillEmptyAlias()
        {
            //foreach (RbacSelectColumn aColumnInfo in List)
            //{
            //    if (string.IsNullOrWhiteSpace(aColumnInfo.Alias))
            //    {
            //        aColumnInfo.Alias = aColumnInfo.TableColumnName;
            //    }
            //}
        }

        public void AddIfNeeded(int aSelectElementID, string aIdentifier)
        {
            //if (List.Count > aSelectElementID)
            //{
            //}
            //else
            //{
            //    List.Add(new RbacSelectColumn { Alias = aIdentifier });
            //}
        }

        public void AddRefereceIdentifier(int aSelectElementID, MultiPartIdentifier aMultiPartIdentifier)
        {
            //if (List.Count > aSelectElementID)
            //{
            //    if (aMultiPartIdentifier.Identifiers.Count == 1)
            //    {
            //        List[aSelectElementID].TableAlias = aMultiPartIdentifier.Identifiers[0].Value;
            //    }
            //    else if (aMultiPartIdentifier.Identifiers.Count == 2)
            //    {
            //        List[aSelectElementID].TableAlias = aMultiPartIdentifier.Identifiers[0].Value;
            //        List[aSelectElementID].TableColumnName = aMultiPartIdentifier.Identifiers[1].Value;
            //    }
            //    else if (aMultiPartIdentifier.Identifiers.Count == 3)
            //    {
            //        List[aSelectElementID].ReferencedTableSchema = aMultiPartIdentifier.Identifiers[0].Value;
            //        List[aSelectElementID].ReferencedTableName = aMultiPartIdentifier.Identifiers[1].Value;
            //        List[aSelectElementID].TableColumnName = aMultiPartIdentifier.Identifiers[1].Value;
            //    }

            //}

            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RbacSelectColumn col in List)
            {
                sb.AppendLine(string.Format("\t\t{0} [as {1}].{2}", col.Table.Name, col.Table.Alias, col.Name));
            }
            return sb.ToString();
        }

        public string ToCommaSeparatedString()
        {           
            string columns = string.Empty;
            foreach (var i in List)
            {
                if (string.IsNullOrEmpty(i.Table.Alias))
                    columns += i.Table.Name + "." + i.Name + ",";
                else
                    columns += i.Table.Alias + "." + i.Name + ",";
            }
            return columns.Length > 0 ? columns.Substring(0, columns.Length - 1) : columns;
        }

        public void AddTableReference(SchemaObjectName schema, Identifier alias)
        {

            //if (List.Count > 0)
            //{
            //    foreach (RbacSelectColumn column in List)
            //    {
            //        if (alias != null &&
            //            column.TableAlias.ToLower() == alias.Value.ToLower())
            //        {
            //            AssignSchemaDetailsToColumn(column, schema);                  
            //        }
            //        else if ((alias == null) &&
            //                 (schema.BaseIdentifier != null) &&
            //                 (schema.BaseIdentifier.Value.ToLower() == column.TableAlias.ToLower()))
            //        {
            //            AssignSchemaDetailsToColumn(column, schema);
            //        }
            //    }
            //}
        }

        private void AssignSchemaDetailsToColumn(RbacSelectColumn column, SchemaObjectName schema)
        {
            //column.ReferencedTableServer = schema.ServerIdentifier != null ? schema.ServerIdentifier.Value : string.Empty;
            //column.ReferencedTableDatabase = schema.DatabaseIdentifier != null ? schema.DatabaseIdentifier.Value : string.Empty;
            //column.ReferencedTableSchema = schema.SchemaIdentifier != null ? schema.SchemaIdentifier.Value : string.Empty;
            //column.ReferencedTableName = schema.BaseIdentifier != null ? schema.BaseIdentifier.Value : string.Empty;
        }
    }
}

