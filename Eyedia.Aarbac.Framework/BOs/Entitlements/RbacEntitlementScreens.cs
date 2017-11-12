using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    
    public class RbacEntitlementScreens
    {
        public List<RbacEntitlementScreen> Screens { get; private set; }

        public RbacEntitlementScreens()
        {
            Screens = new List<RbacEntitlementScreen>();
        }
        public static RbacEntitlementScreens FromXml(XmlNode entitlementMenuNode)
        {
            RbacEntitlementScreens screens = new RbacEntitlementScreens();
            foreach (XmlNode node in entitlementMenuNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                    continue;
                else if (node.Name == "RbacEntitlementScreen")
                    screens.Screens.Add(RbacEntitlementScreen.FromXml(node));
            }
            return screens;
        }

        internal XmlNode ToXml(XmlDocument doc)
        {
            XmlNode screens = doc.CreateElement("RbacEntitlementScreens");            
            foreach (RbacEntitlementScreen subscreen in Screens)
            {
                screens.AppendChild(subscreen.ToXml(screens));
            }
            return screens;
        }
    }
}
