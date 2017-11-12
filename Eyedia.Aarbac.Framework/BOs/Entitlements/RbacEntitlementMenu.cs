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

