using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

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

        public static XmlDocument ValidateAndGetRbacXmlDocument(string xml = null)
        {
            XmlSchema schema = GetXmlSchema();
            return ValidateAgainstSchema(schema, xml);
        }


        public static XmlDocument ValidateAndGetEntitlementXmlDocument(string xml = null)
        {
            XmlSchema schema = GetXmlEnitlementSchema();
            return ValidateAgainstSchema(schema, xml);
        }

        private static XmlDocument ValidateAgainstSchema(XmlSchema schema, string xml = null)
        {
            XmlDocument document = null;
            XmlReaderSettings settings = new XmlReaderSettings();            
            settings.Schemas.Add(schema);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

            if (!string.IsNullOrEmpty(xml))
            {
                using (StringReader stringReader = new StringReader(xml))
                using (XmlReader reader = XmlReader.Create(stringReader, settings))
                {
                    XmlValidationErrors = new List<string>();
                    document = new XmlDocument();
                    try
                    {
                        document.Load(reader);
                    }
                    catch (XmlSchemaValidationException ex)
                    {
                        XmlValidationErrors.Add(string.Format("Line:{0};Position:{1} - {2}",
                            ex.LineNumber, ex.LinePosition, ex.Message));
                        return document;
                    }

                    ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                    document.Validate(eventHandler);
                }
            }
            else
            {
                document = new XmlDocument();
                document.Schemas.Add(schema);
            }

            return document;
        }

        private static XmlSchema GetXmlSchema()
        {
            XmlSchema schema = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.Aarbac.Framework.MetaData.MetaData.xsd"))
            {
                schema = XmlSchema.Read(stream, null);
            }
            return schema;
        }

        private static XmlSchema GetXmlEnitlementSchema()
        {
            XmlSchema schema = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.Aarbac.Framework.MetaData.Entitlement.xsd"))
            {
                schema = XmlSchema.Read(stream, null);
            }
            return schema;
        }

        public static List<string> XmlValidationErrors = new List<string>();

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    XmlValidationErrors.Add(e.Message);
                    break;
                case XmlSeverityType.Warning:
                    XmlValidationErrors.Add(e.Message);
                    break;
            }

        }

    }
}
