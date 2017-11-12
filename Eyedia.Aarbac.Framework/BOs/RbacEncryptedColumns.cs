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
