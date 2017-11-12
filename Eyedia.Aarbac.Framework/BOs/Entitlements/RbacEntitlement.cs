using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEntitlement
    {
        public RbacRole Role { get; private set; }
     
        public RbacEntitlementMenus Menus { get; internal set; }
        public RbacEntitlementScreens Screens { get; internal set; }

        public RbacEntitlement()
        {
            Menus = new RbacEntitlementMenus();
            Screens = new RbacEntitlementScreens();
        }
        public RbacEntitlement(RbacRole role)
        {
            if (role == null)
                RbacException.Raise("A valid role is required to create entitlements!");

            RbacEntitlement entitlement = FromXml(role.MetaDataEntitlements);
            if (entitlement != null)
            {
                this.Menus = entitlement.Menus;
                this.Screens = entitlement.Screens;
            }
        }

        public string ToXml(string fileName = null)
        {
            string xml = string.Empty;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement("RbacEntitlements");
            doc.AppendChild(root);

            root.AppendChild(Menus.ToXml(doc));
            root.AppendChild(Screens.ToXml(doc));

            #region Write Xml
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
            #endregion Write Xml

            return xml;
        }

        private static RbacEntitlement FromXml(string metaDataxml)
        {
            if (string.IsNullOrEmpty(metaDataxml))
                return null;

            RbacEntitlement entitlements = new RbacEntitlement();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(metaDataxml);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                    continue;
                else if (node.Name == "RbacEntitlementMenus")
                    entitlements.Menus = RbacEntitlementMenus.FromXml(node);
                if (node.Name == "RbacEntitlementScreens")
                    entitlements.Screens = RbacEntitlementScreens.FromXml(node);
            }            
            return entitlements;
        }
    }
}
