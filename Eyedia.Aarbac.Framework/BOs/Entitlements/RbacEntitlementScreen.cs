using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEntitlementScreen
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public List<RbacEntitlementScreen> SubScreens { get; set; }

        public RbacEntitlementScreen()
        {
            SubScreens = new List<RbacEntitlementScreen>();
        }

        public static RbacEntitlementScreen FromXml(XmlNode node)
        {
            RbacEntitlementScreen rootElement = new RbacEntitlementScreen();
            rootElement.Name = node.Attributes["Name"].Value;
            rootElement.Text = node.Attributes["Text"].Value;
            rootElement.Enabled = node.Attributes["Enabled"].Value.ToLower() == "true" ? true : false;
            rootElement.Visible = node.Attributes["Visible"].Value.ToLower() == "true" ? true : false;

            if (node.ChildNodes.Count == 0)
            {
                return rootElement;
            }
            else
            {
                foreach (XmlNode childNode in node)
                {
                    rootElement.SubScreens.Add(FromXmlOne(childNode));
                }
            }
            return rootElement;
        }

        private static RbacEntitlementScreen FromXmlOne(XmlNode node)
        {
            RbacEntitlementScreen nodeElement = new RbacEntitlementScreen();
            nodeElement.Name = node.Attributes["Name"].Value;
            nodeElement.Text = node.Attributes["Text"].Value;
            nodeElement.Enabled = node.Attributes["Enabled"].Value.ToLower() == "true" ? true : false;
            nodeElement.Visible = node.Attributes["Visible"].Value.ToLower() == "true" ? true : false;

            foreach (XmlNode childNode in node)
            {
                nodeElement.SubScreens.Add(FromXmlOne(childNode));
            }
            return nodeElement;
        }

        internal XmlNode ToXml(XmlNode menu)
        {
            XmlNode rootElement = menu.OwnerDocument.CreateElement("RbacEntitlementScreen");

            XmlAttribute name = menu.OwnerDocument.CreateAttribute("Name");
            name.Value = this.Name;
            rootElement.Attributes.Append(name);

            XmlAttribute text = menu.OwnerDocument.CreateAttribute("Text");
            text.Value = this.Text;
            rootElement.Attributes.Append(text);

            XmlAttribute visible = menu.OwnerDocument.CreateAttribute("Visible");
            visible.Value = this.Visible ? "true" : "false";
            rootElement.Attributes.Append(visible);

            XmlAttribute enabled = menu.OwnerDocument.CreateAttribute("Enabled");
            enabled.Value = this.Enabled ? "true" : "false";
            rootElement.Attributes.Append(enabled);

            if (this.SubScreens.Count == 0)
            {
                return rootElement;
            }
            else
            {
                foreach (RbacEntitlementScreen subscreen in SubScreens)
                {
                    rootElement.AppendChild(ToXmlOne(rootElement, subscreen));
                }
            }
            return rootElement;
        }

        internal XmlNode ToXmlOne(XmlNode rootNode, RbacEntitlementScreen subScreen)
        {
            XmlNode nodeElement = rootNode.OwnerDocument.CreateElement("RbacEntitlementScreen");

            XmlAttribute name = rootNode.OwnerDocument.CreateAttribute("Name");
            name.Value = subScreen.Name;
            nodeElement.Attributes.Append(name);

            XmlAttribute text = rootNode.OwnerDocument.CreateAttribute("Text");
            text.Value = subScreen.Text;
            nodeElement.Attributes.Append(text);

            XmlAttribute visible = rootNode.OwnerDocument.CreateAttribute("Visible");
            visible.Value = subScreen.Visible ? "true" : "false";
            nodeElement.Attributes.Append(visible);

            XmlAttribute enabled = rootNode.OwnerDocument.CreateAttribute("Enabled");
            enabled.Value = subScreen.Enabled ? "true" : "false";
            nodeElement.Attributes.Append(enabled);


            foreach (RbacEntitlementScreen subsubsubScreen in subScreen.SubScreens)
            {
                nodeElement.AppendChild(ToXmlOne(nodeElement, subsubsubScreen));
            }
            return nodeElement;
        }


    }
}
