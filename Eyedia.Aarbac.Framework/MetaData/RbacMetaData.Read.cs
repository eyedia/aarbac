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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    public partial class RbacMetaData
    {
        public static List<RbacTable> ReadPermissions(string metaDataxml)
        {
            List<RbacTable> tables = new List<RbacTable>();
            if (string.IsNullOrEmpty(metaDataxml))
                return tables;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(metaDataxml);

            foreach (XmlNode tableNode in doc.DocumentElement.ChildNodes[1])
            {
                if (tableNode.NodeType == XmlNodeType.Comment)
                    continue;

                RbacTable table = new RbacTable(tableNode.Attributes["Id"].Value, tableNode.Attributes["Name"].Value,
                    tableNode.Attributes["Create"].Value, tableNode.Attributes["Read"].Value,
                        tableNode.Attributes["Update"].Value, tableNode.Attributes["Delete"].Value);
                foreach (XmlNode node in tableNode)
                {
                    if (node.NodeType == XmlNodeType.Comment)
                        continue;

                    if (node.Name == "Columns")
                    {
                        foreach (XmlNode columnNode in node.ChildNodes)
                        {
                            if (columnNode.NodeType == XmlNodeType.Comment)
                                continue;
                            //if ((table.Name == "Author") && (columnNode.Attributes["Name"].Value == "AuthorId"))
                            //    Debugger.Break();

                            table.Columns.Add(new RbacColumn(columnNode.Attributes["Name"].Value, columnNode.Attributes["Type"].Value,
                            columnNode.Attributes["Create"].Value, columnNode.Attributes["Read"].Value,
                            columnNode.Attributes["Update"].Value));
                        }
                    }
                    else if (node.Name == "Conditions")
                    {
                        //if (tableNode.Attributes["Name"].Value == "City")
                        //    Debugger.Break();

                        foreach (XmlNode condnNode in node.ChildNodes)
                        {
                            if (condnNode.NodeType == XmlNodeType.Comment)
                                continue;

                            if (condnNode.Attributes.Count == 3)
                                table.Conditions.Add(new RbacCondition(table.Name, condnNode.Attributes["Name"].Value, condnNode.Attributes["Columns"].Value, condnNode.Attributes["WhereClause"].Value));
                        }
                    }
                    else if (node.Name == "Relations")
                    {
                        //if (tableNode.Attributes["Name"].Value == "Author")
                        //    Debugger.Break();

                        foreach (XmlNode relationNode in node.ChildNodes)
                        {
                            if (relationNode.NodeType == XmlNodeType.Comment)
                                continue;
                            if (relationNode.Attributes.Count >= 2)
                                table.Relations.Add(new RbacRelation(table.Name, relationNode.Attributes["My"].Value, relationNode.Attributes["With"].Value));
                        }
                    }
                    else if (node.Name == "Parameters")
                    {
                        //if (tableNode.Attributes["Name"].Value == "City")
                        //    Debugger.Break();

                        foreach (XmlNode paramNode in node.ChildNodes)
                        {
                            if (paramNode.NodeType == XmlNodeType.Comment)
                                continue;

                            if (paramNode.Attributes.Count >= 1)
                                table.Parameters.Add(new RbacParameter(paramNode.Attributes["Name"].Value, paramNode.Attributes["Description"].Value));
                        }
                    }
                }
                tables.Add(table);
            }

            return tables;
        }
    }
}
