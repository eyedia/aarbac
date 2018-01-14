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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    public partial class RbacMetaData : IDisposable
    {

        public string Generate(string connectionString, string fileName = null)
        {
            if(connectionString.IndexOf("MultipleActiveResultSets=true", StringComparison.OrdinalIgnoreCase) == -1)
            {
                if (connectionString.TrimEnd().Substring(connectionString.Length - 1, 1) != ";")
                    connectionString += ";";

                connectionString += "MultipleActiveResultSets=true";
            }

            string xml = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable foreignKeys = GetForeignKeys(connection);

                    SqlCommand tCommand = new SqlCommand("select object_id, name from sys.tables where name != 'sysdiagrams' order by name", connection);
                    SqlDataReader tReader = tCommand.ExecuteReader();

                    XmlDocument doc = ValidateAndGetRbacXmlDocument();                    
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

                    while (tReader.Read())
                    {
                        XmlNode tableNode = doc.CreateElement("Table");
                        XmlAttribute Id = doc.CreateAttribute("Id");
                        Id.Value = tReader[0].ToString();
                        tableNode.Attributes.Append(Id);
                        XmlAttribute Name = doc.CreateAttribute("Name");
                        Name.Value = tReader[1].ToString();
                        tableNode.Attributes.Append(Name);
                        AddCRUDDefaults(doc, tableNode, true);

                        SqlCommand cCommand = new SqlCommand(
                            string.Format("select c.name, tp.name from sys.columns c inner join sys.tables t on t.object_id = c.object_id inner join sys.types tp on tp.system_type_id = c.system_type_id where t.object_id = {0}"
                            , Id.Value), connection);
                        SqlDataReader cReader = cCommand.ExecuteReader();
                        string oneColumnName = string.Empty;
                        XmlNode columnsNode = doc.CreateElement("Columns");

                        while (cReader.Read())
                        {
                            XmlNode columnNode = doc.CreateElement("Column");
                            XmlAttribute cName = doc.CreateAttribute("Name");
                            cName.Value = cReader[0].ToString();
                            columnNode.Attributes.Append(cName);

                            XmlAttribute type = doc.CreateAttribute("Type");
                            type.Value = cReader[1].ToString();
                            columnNode.Attributes.Append(type);
                            AddCRUDDefaults(doc, columnNode, false);

                            columnsNode.AppendChild(columnNode);

                            oneColumnName = cName.Value;
                        }
                        cReader.Close();
                        tableNode.AppendChild(columnsNode);
                        tableNode.AppendChild(GetForeignKeyNode(foreignKeys, Name.Value, doc));
                        tableNode.AppendChild(AddConditions(doc, oneColumnName));
                        tableNode.AppendChild(AddParameters(doc, oneColumnName));
                        tablesNode.AppendChild(tableNode);
                    }

                    doc.DocumentElement.SetAttribute("xmlns:xsd", "https://github.com/eyedia/aarbac");
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
                            ValidateAndGetRbacXmlDocument(xml);

                            if (XmlValidationErrors.Count > 0)
                                RbacException.Raise("Cannot generate meta data, XML validation failed!" + Environment.NewLine + XmlValidationErrors.ToLine());
                        }
                    }
                    tReader.Close();
                    connection.Close();
                }
            }
            catch(ArgumentException ex)
            {
                RbacException.Raise("Failed, check your connection string and try again.");
            }
            
            return xml;
        }

        bool dummyConditionsAdded;    
        bool dummyParameterAdded;

        private XmlNode AddConditions(XmlDocument doc, string oneColumnName)
        {
            XmlNode condnsNode = doc.CreateElement("Conditions");
            return condnsNode;
            //below code was written for sample, as this creates confusion, samples will not be added into meta data

            XmlNode condnNode = doc.CreateElement("Condition");
            condnsNode.AppendChild(condnNode);

            if (!dummyConditionsAdded)
            {

                XmlComment comment = doc.CreateComment("This is a sample condition, modify or remove");
                condnsNode.InsertBefore(comment, condnNode);

                XmlAttribute nameA = doc.CreateAttribute("Name");
                nameA.Value = "Condition1";
                condnNode.Attributes.Append(nameA);

            
                XmlAttribute columnsA = doc.CreateAttribute("Columns");
                columnsA.Value = oneColumnName;
                condnNode.Attributes.Append(columnsA);

                XmlAttribute whereClauseA = doc.CreateAttribute("WhereClause");
                whereClauseA.Value = "__self__." + oneColumnName + " in (123,456)";
                condnNode.Attributes.Append(whereClauseA);

                dummyConditionsAdded = true;
            }
            return condnsNode;
        }

        private XmlNode AddParameters(XmlDocument doc, string oneColumnName)
        {
            XmlNode paramsNode = doc.CreateElement("Parameters");
            return paramsNode;
            //below code was written for sample, as this creates confusion, samples will not be added into meta data


            XmlNode paramNode = doc.CreateElement("Parameter");
            paramsNode.AppendChild(paramNode);

            if (!dummyParameterAdded)
            {

                XmlComment comment = doc.CreateComment("This is a sample parameter, modify & use or remove");
                paramsNode.InsertBefore(comment, paramNode);

                XmlAttribute nameA = doc.CreateAttribute("Name");
                nameA.Value = "{P1}";
                paramNode.Attributes.Append(nameA);

                XmlAttribute nameD = doc.CreateAttribute("Description");
                nameD.Value = "Please mention proper description (e.g. data type with example) as this wil be shown as hint on UI";
                paramNode.Attributes.Append(nameD);

                dummyParameterAdded = true;
            }
            return paramsNode;
        }

        private XmlNode GetForeignKeyNode(DataTable foreignKeys, string referencingTable, XmlDocument doc)
        {
            DataRow[] rows = foreignKeys.Select(string.Format("ReferencingTable = '{0}'", referencingTable));
            XmlElement relationsNode = doc.CreateElement("Relations");          

            foreach (DataRow row in rows)
            {
                XmlNode relationNode = doc.CreateElement("Relation");
                XmlAttribute type = doc.CreateAttribute("Type");
                type.Value = "Auto";
                relationNode.Attributes.Append(type);

                XmlAttribute my = doc.CreateAttribute("My");
                my.Value = row["ReferencingColumn"].ToString();
                relationNode.Attributes.Append(my);
                XmlAttribute with = doc.CreateAttribute("With");
                with.Value = string.Format("{0}.{1}", row["ReferencedTable"].ToString(), row["ReferencedColumn"].ToString());
                relationNode.Attributes.Append(with);
                relationsNode.AppendChild(relationNode);
            }

            return relationsNode;
        }

        private DataTable GetForeignKeys(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(Resources.Query_ForeignKeys, connection);
            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            return table;
        }        

        private void AddCRUDDefaults(XmlDocument doc, XmlNode node, bool isTableNode)
        {
            XmlAttribute create = doc.CreateAttribute("Create");
            create.Value = "False";
            node.Attributes.Append(create);

            XmlAttribute read = doc.CreateAttribute("Read");
            read.Value = "True";
            node.Attributes.Append(read);

            XmlAttribute update = doc.CreateAttribute("Update");
            update.Value = "False";
            node.Attributes.Append(update);

            if (isTableNode)
            {
                XmlAttribute delete = doc.CreateAttribute("Delete");
                delete.Value = "False";
                node.Attributes.Append(delete);
            }
        }

        public void Dispose()
        {
            XmlValidationErrors = null;
        }
    }
}


