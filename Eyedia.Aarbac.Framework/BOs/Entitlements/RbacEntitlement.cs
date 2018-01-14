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


