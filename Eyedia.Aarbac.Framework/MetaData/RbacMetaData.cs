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
    public class RbacMetaData
    {

        public static string Generate(string connectionString, string fileName = null)
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

                            //XmlAttribute filterCol = doc.CreateAttribute("FilterColumn");
                            //filterCol.Value = cName.Value.IndexOf("Id", StringComparison.OrdinalIgnoreCase) >= 0 ? "True" : "False";
                            //columnNode.Attributes.Append(filterCol);
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

        static bool dummyConditionsAdded;
        //static bool dummyRelationWhereClauseAdded;
        static bool dummyParameterAdded;

        private static XmlNode AddConditions(XmlDocument doc, string oneColumnName)
        {
            XmlNode condnsNode = doc.CreateElement("Conditions");
            XmlNode condnNode = doc.CreateElement("Condition");
            condnsNode.AppendChild(condnNode);

            if (!dummyConditionsAdded)
            {

                XmlComment comment = doc.CreateComment("This is a sample condition, modify or remove");
                condnsNode.InsertBefore(comment, condnNode);

                XmlAttribute nameA = doc.CreateAttribute("Name");
                nameA.Value = "Condition1";
                condnNode.Attributes.Append(nameA);

            
                XmlAttribute whereClauseA = doc.CreateAttribute("WhereClause");
                whereClauseA.Value = "__self__." + oneColumnName + " in (123,456)";
                condnNode.Attributes.Append(whereClauseA);

                dummyConditionsAdded = true;
            }
            return condnsNode;
        }

        private static XmlNode AddParameters(XmlDocument doc, string oneColumnName)
        {
            XmlNode paramsNode = doc.CreateElement("Parameters");
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

        private static XmlNode GetForeignKeyNode(DataTable foreignKeys, string referencingTable, XmlDocument doc)
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

        private static DataTable GetForeignKeys(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(Resources.Query_ForeignKeys, connection);
            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            return table;
        }

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
                            columnNode.Attributes["Update"].Value, columnNode.Attributes["Delete"].Value));
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

                            if (condnNode.Attributes.Count  == 2)
                                table.Conditions.Add(new RbacCondition(table.Name, condnNode.Attributes["Name"].Value, condnNode.Attributes["WhereClause"].Value));
                        }
                    }
                    else if (node.Name == "Relations")
                    {
                        foreach (XmlNode relationNode in node.ChildNodes)
                        {
                            if (relationNode.NodeType == XmlNodeType.Comment)
                                continue;
                            if (relationNode.Attributes.Count == 2)
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
                                table.Parameters.Add(new RbacParameter(paramNode.Attributes["Name"].Value));
                        }
                    }
                }
                tables.Add(table);
            }

            return tables;
        }

        private static void AddCRUDDefaults(XmlDocument doc, XmlNode node, bool isTableNode)
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
    }
}
