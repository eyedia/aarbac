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
        private string _Alias = String.Empty;
        private string _ReferencedTableDatabase = String.Empty;
        private string _ReferencedTableName = String.Empty;
        private string _ReferencedTableSchema = String.Empty;
        private string _ReferencedTableServer = String.Empty;
        private string _TableAlias = String.Empty;
        private string _TableColumnName = String.Empty;

        public string Alias
        {
            get { return _Alias; }
            set { _Alias = value; }
        }

        public string TableAlias
        {
            get { return _TableAlias; }
            set { _TableAlias = value; }
        }

        public string TableColumnName
        {
            get { return _TableColumnName; }
            set { _TableColumnName = value; }
        }

        public string ReferencedTableServer
        {
            get { return _ReferencedTableServer; }
            set { _ReferencedTableServer = value; }
        }

        public string ReferencedTableDatabase
        {
            get { return _ReferencedTableDatabase; }
            set { _ReferencedTableDatabase = value; }
        }

        public string ReferencedTableSchema
        {
            get { return _ReferencedTableSchema; }
            set { _ReferencedTableSchema = value; }
        }

        public string ReferencedTableName
        {
            get { return _ReferencedTableName; }
            set { _ReferencedTableName = value; }
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
            foreach (RbacSelectColumn aColumnInfo in List)
            {
                if (string.IsNullOrWhiteSpace(aColumnInfo.Alias))
                {
                    aColumnInfo.Alias = aColumnInfo.TableColumnName;
                }
            }
        }

        public void AddIfNeeded(int aSelectElementID, string aIdentifier)
        {
            if (List.Count > aSelectElementID)
            {
            }
            else
            {
                List.Add(new RbacSelectColumn { Alias = aIdentifier });
            }
        }

        public void AddRefereceIdentifier(int aSelectElementID, MultiPartIdentifier aMultiPartIdentifier)
        {
            if (List.Count > aSelectElementID)
            {
                if (aMultiPartIdentifier.Identifiers.Count == 1)
                {
                    List[aSelectElementID].TableAlias = aMultiPartIdentifier.Identifiers[0].Value;
                }
                else if (aMultiPartIdentifier.Identifiers.Count == 2)
                {
                    List[aSelectElementID].TableAlias = aMultiPartIdentifier.Identifiers[0].Value;
                    List[aSelectElementID].TableColumnName = aMultiPartIdentifier.Identifiers[1].Value;
                }
                else if (aMultiPartIdentifier.Identifiers.Count == 3)
                {
                    List[aSelectElementID].ReferencedTableSchema = aMultiPartIdentifier.Identifiers[0].Value;
                    List[aSelectElementID].ReferencedTableName = aMultiPartIdentifier.Identifiers[1].Value;
                    List[aSelectElementID].TableColumnName = aMultiPartIdentifier.Identifiers[1].Value;
                }

                //ColumnInfo aColumnInfo = ColumnList[aSelectElementID];

                //int aIdentIdx = 0;
                //foreach (Identifier aIdentifier in aMultiPartIdentifier.Identifiers)
                //{
                //    if (aMultiPartIdentifier.Identifiers.Count == 1)
                //    {
                //        if (aIdentIdx == 0)
                //            aColumnInfo.TableAlias = aIdentifier.Value;
                //    }
                //    else if (aMultiPartIdentifier.Identifiers.Count == 2)
                //    {
                //        if (aIdentIdx == 0)
                //            aColumnInfo.TableAlias = aIdentifier.Value;
                //        if (aIdentIdx == 1)
                //            aColumnInfo.TableColumnName = aIdentifier.Value;
                //    }
                //    else if (aMultiPartIdentifier.Identifiers.Count == 3)
                //    {
                //        if (aIdentIdx == 0)
                //            aColumnInfo.ReferencedTableSchema = aIdentifier.Value;
                //        if (aIdentIdx == 1)
                //            aColumnInfo.ReferencedTableName = aIdentifier.Value;
                //        if (aIdentIdx == 2)
                //            aColumnInfo.TableColumnName = aIdentifier.Value;
                //    }

                //    aIdentIdx = aIdentIdx + 1;
                //}
            }

            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (RbacSelectColumn col in List)
            {
                sb.AppendLine(string.Format("\t\t{0} [as {1}].{2}", col.ReferencedTableName, col.TableAlias, col.TableColumnName));
            }
            return sb.ToString();
        }

        public string ToCommaSeparatedString()
        {
            return List.Count > 0 ? List.Select(i => i.TableColumnName).Aggregate((i, j) => i + ", " + j) : string.Empty; ;
        }

        public void AddTableReference(SchemaObjectName schema, Identifier alias)
        {

            if (List.Count > 0)
            {
                foreach (RbacSelectColumn column in List)
                {
                    if (alias != null &&
                        column.TableAlias.ToLower() == alias.Value.ToLower())
                    {
                        AssignSchemaDetailsToColumn(column, schema);
                        //if (schema.ServerIdentifier != null)
                        //    aColumnInfo.ReferencedTableServer = schema.ServerIdentifier.Value;
                        //if (schema.DatabaseIdentifier != null)
                        //    aColumnInfo.ReferencedTableDatabase = schema.DatabaseIdentifier.Value;
                        //if (schema.SchemaIdentifier != null)
                        //    aColumnInfo.ReferencedTableSchema = schema.SchemaIdentifier.Value;
                        //if (schema.BaseIdentifier != null)
                        //    aColumnInfo.ReferencedTableName = schema.BaseIdentifier.Value;
                    }
                    else if ((alias == null) &&
                             (schema.BaseIdentifier != null) &&
                             (schema.BaseIdentifier.Value.ToLower() == column.TableAlias.ToLower()))
                    {
                        AssignSchemaDetailsToColumn(column, schema);

                        //if (schema.ServerIdentifier != null)
                        //    aColumnInfo.ReferencedTableServer = schema.ServerIdentifier.Value;
                        //if (schema.DatabaseIdentifier != null)
                        //    aColumnInfo.ReferencedTableDatabase = schema.DatabaseIdentifier.Value;
                        //if (schema.SchemaIdentifier != null)
                        //    aColumnInfo.ReferencedTableSchema = schema.SchemaIdentifier.Value;
                        //if (schema.BaseIdentifier != null)
                        //    aColumnInfo.ReferencedTableName = schema.BaseIdentifier.Value;
                    }
                }
            }
        }

        private void AssignSchemaDetailsToColumn(RbacSelectColumn column, SchemaObjectName schema)
        {
            column.ReferencedTableServer = schema.ServerIdentifier != null ? schema.ServerIdentifier.Value : string.Empty;
            column.ReferencedTableDatabase = schema.DatabaseIdentifier != null ? schema.DatabaseIdentifier.Value : string.Empty;
            column.ReferencedTableSchema = schema.SchemaIdentifier != null ? schema.SchemaIdentifier.Value : string.Empty;
            column.ReferencedTableName = schema.BaseIdentifier != null ? schema.BaseIdentifier.Value : string.Empty;
        }
    }
}

