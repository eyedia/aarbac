using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Eyedia.Aarbac.Framework
{
    public class RbacRoleWeb
    {
        public RbacRoleWeb()
        { }

        public RbacRoleWeb(RbacRole role)
        {
            if (role == null)
                return;

            this.RoleId = role.RoleId;
            this.RbacId = role.RbacId;
            this.Name = role.Name;
            this.Description = role.Description;
            this.Entitlement = role.Entitlement;
            this.CrudPermissions = role.CrudPermissions;
            this.MetaDataRbac = role.MetaDataRbac;
            this.MetaDataEntitlements = role.MetaDataEntitlements;
        }

        public int RoleId { get; set; }
        public int RbacId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaDataRbac { get; set; }
        public string MetaDataEntitlements { get; set; }

        [XmlIgnore]
        public RbacEntitlement Entitlement { get; set; }

        [XmlIgnore]
        public List<RbacTable> CrudPermissions { get; set; }

        public static RbacRole Get(RbacRoleWeb rbacRoleWeb)
        {
            RbacRole role = new RbacRole();
            role.RoleId = rbacRoleWeb.RoleId;
            role.RbacId = rbacRoleWeb.RbacId;
            role.Name = rbacRoleWeb.Name;
            role.Description = rbacRoleWeb.Description;
            role.MetaDataRbac = rbacRoleWeb.MetaDataRbac;
            role.MetaDataEntitlements = rbacRoleWeb.MetaDataEntitlements;
            role.Entitlement = rbacRoleWeb.Entitlement;
            role.CrudPermissions = rbacRoleWeb.CrudPermissions;
            return role;
        }
    }
}
