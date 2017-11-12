using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacRole
    {

        /// <summary>
        /// Instantiates a new Rbac Role
        /// </summary>
        /// <param name="rbacId">The rbac id it will belong to</param> 
        /// <param name="name">Name of the new role</param>
        /// <param name="description">Description of the new role</param>
        /// <param name="metaDataRbac">Meta data of the role</param>
        /// <param name="metaDataEntitlements">Meta data of the entilements</param>
        public RbacRole(int rbacId, string name, string description, string metaDataRbac, string metaDataEntitlements)
        {
            if (rbacId == 0)
                RbacException.Raise("Creation of role requires a valid rbacId!");

            Rbac rbac = new Framework.Rbac();

            //create new role
            RbacRole newRole = new RbacRole();
            newRole.RbacId = rbacId;            
            newRole.Name = name;
            newRole.Description = description;
            newRole.MetaDataRbac = metaDataRbac;
            newRole.MetaDataEntitlements = metaDataEntitlements;
            newRole = new DataManager.Manager(false).AddOrUpdate(newRole);
            Assign(newRole);           

        }

        public RbacRole(int roleId)
        {
            RbacRole role = new DataManager.Manager(false).GetRole(roleId);
            if (role == null)
                RbacException.Raise(string.Format("Role with id '{0}' was not found in repository!", roleId), RbacExceptionCategories.Repository);

            Assign(role);
            ParseMetaData();
        }

        public RbacRole(string roleName)
        {
            RbacRole role = new DataManager.Manager(false).GetRole(roleName);
            if (role == null)
                RbacException.Raise(string.Format("Role '{0}' was not foundin repository!", roleName), RbacExceptionCategories.Repository);

            Assign(role);
            ParseMetaData();
        }

        public RbacRole() { }

        private void Assign(RbacRole role)
        {
            if (role == null)
                return;
            this.RoleId = role.RoleId;
            this.RbacId = role.RbacId;           
            this.Name = role.Name;
            this.Description = role.Description;
            this.MetaDataRbac = role.MetaDataRbac;
            this.MetaDataEntitlements = role.MetaDataEntitlements;
            this.Version = role.Version;
        }

        public int RoleId { get; internal set; }
        public int RbacId { get; internal set; }       
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string MetaDataRbac { get; internal set; }
        public string MetaDataEntitlements { get; internal set; }
        public RbacEntitlement Entitlement { get; internal set; }
        public List<RbacTable> CrudPermissions { get; internal set; }
        public List<RbacParameter> Parameters { get; private set; }
        public int Version { get; internal set; }

        public int Save(string connectionString)
        {            
            return 0;
        }

        public void ParseMetaData()
        {
            this.CrudPermissions = RbacMetaData.ReadPermissions(MetaDataRbac);
            Parameters = this.CrudPermissions.SelectMany(t => t.Parameters).ToList();

            this.Entitlement = new RbacEntitlement(this);
        }


        public static RbacRole GetSample(Rbac rbac)
        {
            rbac = new DataManager.Manager(false).GetRbac(rbac.Name);

            RbacRole role = new RbacRole();
            role.Name = "Sample Role";
            role.Description = "Sample role description";
            role.MetaDataEntitlements = rbac.MetaDataEntitlements;
            role.MetaDataRbac = rbac.MetaDataRbac;
            role.RbacId = rbac.RbacId;
            return role;
        }

    }
}
