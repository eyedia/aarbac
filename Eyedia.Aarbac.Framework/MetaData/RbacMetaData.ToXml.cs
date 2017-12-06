using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    public partial class RbacMetaData
    {
        public static string ToXml(List<RbacTable> tables, string fileName = null)
        {
            string xml = string.Empty;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement("Rbac");
            doc.AppendChild(root);

            XmlElement summary = doc.CreateElement("Summary");
            XmlAttribute genOn = doc.CreateAttribute("GeneratedOn");
            genOn.Value = DateTime.Now.ToString();
            summary.Attributes.Append(genOn);
            XmlAttribute genBy = doc.CreateAttribute("GeneratedBy");
            genBy.Value = "TODO";
            summary.Attributes.Append(genBy);
            root.AppendChild(summary);

            XmlElement tablesNode = doc.CreateElement("Tables");
            root.AppendChild(tablesNode);

            foreach (RbacTable table in tables)
            {
                tablesNode.AppendChild(table.ToXml(doc));                
            }
            root.AppendChild(tablesNode);

            if (!string.IsNullOrEmpty(fileName))
            {
                doc.Save(fileName);
            }
            else
            {
                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xml = stringWriter.GetStringBuilder().ToString();
                }
            }

            return xml;
        }
    }
}
