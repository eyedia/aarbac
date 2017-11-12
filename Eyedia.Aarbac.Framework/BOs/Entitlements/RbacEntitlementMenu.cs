using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEntitlementMenu
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public List<RbacEntitlementMenu> SubMenus { get; set; }

        public RbacEntitlementMenu()
        {
            SubMenus = new List<RbacEntitlementMenu>();
        }

        public static RbacEntitlementMenu FromXml(XmlNode node)
        {
            RbacEntitlementMenu rootElement = new RbacEntitlementMenu();
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
                    rootElement.SubMenus.Add(FromXmlOne(childNode));
                }
            }
            return rootElement;
        }

      
        private static RbacEntitlementMenu FromXmlOne(XmlNode node)
        {
            RbacEntitlementMenu nodeElement = new RbacEntitlementMenu();
            nodeElement.Name = node.Attributes["Name"].Value;
            nodeElement.Text = node.Attributes["Text"].Value;
            nodeElement.Enabled = node.Attributes["Enabled"].Value.ToLower() == "true" ? true : false;
            nodeElement.Visible = node.Attributes["Visible"].Value.ToLower() == "true" ? true : false;

            foreach (XmlNode childNode in node)
            {
                nodeElement.SubMenus.Add(FromXmlOne(childNode));
            }
            return nodeElement;
        }

        internal XmlNode ToXml(XmlNode menu)
        {
            XmlNode rootElement = menu.OwnerDocument.CreateElement("RbacEntitlementMenu");

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
            enabled.Value = this.Enabled?"true":"false";
            rootElement.Attributes.Append(enabled);

            if (this.SubMenus.Count == 0)
            {
                return rootElement;
            }
            else
            {
                foreach (RbacEntitlementMenu subMenu in SubMenus)
                {
                    rootElement.AppendChild(ToXmlOne(rootElement, subMenu));
                }
            }
            return rootElement;
        }

        internal XmlNode ToXmlOne(XmlNode rootNode, RbacEntitlementMenu subMenu)
        {
            XmlNode nodeElement = rootNode.OwnerDocument.CreateElement("RbacEntitlementMenu");

            XmlAttribute name = rootNode.OwnerDocument.CreateAttribute("Name");
            name.Value = subMenu.Name;
            nodeElement.Attributes.Append(name);

            XmlAttribute text = rootNode.OwnerDocument.CreateAttribute("Text");
            text.Value = subMenu.Text;
            nodeElement.Attributes.Append(text);

            XmlAttribute visible = rootNode.OwnerDocument.CreateAttribute("Visible");
            visible.Value = subMenu.Visible ? "true" : "false";
            nodeElement.Attributes.Append(visible);

            XmlAttribute enabled = rootNode.OwnerDocument.CreateAttribute("Enabled");
            enabled.Value = subMenu.Enabled ? "true" : "false";
            nodeElement.Attributes.Append(enabled);


            foreach (RbacEntitlementMenu subsubsubMenu in subMenu.SubMenus)
            {
                nodeElement.AppendChild(ToXmlOne(nodeElement, subsubsubMenu));
            }
            return nodeElement;
        }



    }
}
