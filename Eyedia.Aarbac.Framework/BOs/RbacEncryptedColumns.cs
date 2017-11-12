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
using System.Xml;
using System.IO;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEncryptedColumns : List<RbacEncryptedColumn>
    {
        public string MetaData { get; set; }

        public RbacEncryptedColumns() { }
        public RbacEncryptedColumns(string metaDataXml)
        {
            RbacEncryptedColumns cols = FromXml(metaDataXml);
            foreach(RbacEncryptedColumn col in cols)
            {
                this.Add(col);
            }

        }

        public RbacEncryptedColumn this[string columnName]
        {
            get
            {
                RbacEncryptedColumn column = (from c in this
                                              where c.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase)
                                              select c).SingleOrDefault();
                return column;
            }
        }

        public string ToXml()
        {
            string xml = string.Empty;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement("RbacEncryptedColumns");
            doc.AppendChild(root);

            foreach (RbacEncryptedColumn col in this)
            {
                XmlElement colNode = doc.CreateElement("RbacEncryptedColumn");
                XmlAttribute name = doc.CreateAttribute("ColumnName");
                name.Value = col.ColumnName;
                colNode.Attributes.Append(name);

                XmlAttribute table = doc.CreateAttribute("TableName");
                table.Value = col.TableName;
                colNode.Attributes.Append(table);

                root.AppendChild(colNode);
            }
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                doc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                xml = stringWriter.GetStringBuilder().ToString();
            }

            return xml;
        }

        public static RbacEncryptedColumns FromXml(string metaDataxml)
        {
            RbacEncryptedColumns cols = new RbacEncryptedColumns();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(metaDataxml);

            foreach (XmlNode encColumn in doc.DocumentElement.ChildNodes)
            {
                if (encColumn.NodeType == XmlNodeType.Comment)
                    continue;

               cols.Add(new RbacEncryptedColumn(encColumn.Attributes["ColumnName"].Value, encColumn.Attributes["TableName"].Value));
            }
            return cols;
        }
    }
}

