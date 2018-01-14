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
        public string ToXml(List<RbacTable> tables, string fileName = null)
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

        public XmlDocument ValidateAndGetRbacXmlDocument(string xml = null)
        {
            XmlSchema schema = GetXmlSchema();
            return ValidateAgainstSchema(schema, xml);
        }


        public XmlDocument ValidateAndGetEntitlementXmlDocument(string xml = null)
        {
            XmlSchema schema = GetXmlEnitlementSchema();
            return ValidateAgainstSchema(schema, xml);
        }

        private XmlDocument ValidateAgainstSchema(XmlSchema schema, string xml = null)
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
                    eventHandler = null;
                }
            }
            else
            {
                document = new XmlDocument();
                document.Schemas.Add(schema);
            }

            return document;
        }

        private XmlSchema GetXmlSchema()
        {
            XmlSchema schema = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.Aarbac.Framework.MetaData.MetaData.xsd"))
            {
                schema = XmlSchema.Read(stream, null);
            }
            return schema;
        }

        private XmlSchema GetXmlEnitlementSchema()
        {
            XmlSchema schema = null;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Eyedia.Aarbac.Framework.MetaData.Entitlement.xsd"))
            {
                schema = XmlSchema.Read(stream, null);
            }
            return schema;
        }

        public List<string> XmlValidationErrors = new List<string>();

        void ValidationEventHandler(object sender, ValidationEventArgs e)
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

