using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    
    public class RbacEntitlementMenus
    {
        public List<RbacEntitlementMenu> Menu {get;set;}

        public RbacEntitlementMenus()
        {
            Menu = new List<RbacEntitlementMenu>();
        }

        public static RbacEntitlementMenus FromXml(XmlNode menusNode)
        {
            RbacEntitlementMenus menus = new RbacEntitlementMenus();
            foreach (XmlNode node in menusNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                    continue;
                else if (node.Name == "RbacEntitlementMenu")
                    menus.Menu.Add(RbacEntitlementMenu.FromXml(node));  
            }
            return menus;
        }

        internal XmlNode ToXml(XmlDocument doc)
        {
            XmlNode menu = doc.CreateElement("RbacEntitlementMenus");
            foreach (RbacEntitlementMenu submenu in Menu)
            {
                menu.AppendChild(submenu.ToXml(menu));
            }         
            return menu;
        }
    }
}
