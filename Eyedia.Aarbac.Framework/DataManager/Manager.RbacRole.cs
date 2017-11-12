using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework.DataManager
{
    internal partial class Manager
    {
        public List<Framework.RbacRole> GetRoles()
        {
            List<Framework.RbacRole> roles = new List<Framework.RbacRole>();
            using (var ctx = new Entities(ConnectionString))
            {
                List<RbacRole> dbRoles = ctx.RbacRoles.ToList();
                foreach (RbacRole dbRole in dbRoles)
                {
                    roles.Add(Assign(dbRole));
                }
            }
            return roles;
        }

        public Framework.RbacRole GetRole(int roleId)
        {
            Framework.RbacRole role = null;
            using (var ctx = new Entities(ConnectionString))
            {
                RbacRole dbRbacRole = ctx.RbacRoles.Where(r => r.RoleId == roleId).SingleOrDefault();
                if (dbRbacRole != null)
                    role = Assign(dbRbacRole);
            }
            return role;
        }

        public Framework.RbacRole GetRole(string roleName)
        {
            Framework.RbacRole role = null;
            using (var ctx = new Entities(ConnectionString))
            {
                RbacRole dbRbacRole = ctx.RbacRoles.Where(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (dbRbacRole != null)
                    role = Assign(dbRbacRole);
            }
            return role;
        }

        public Framework.RbacRole AddOrUpdate(Framework.RbacRole role)
        {
            RbacRole dbRole = null;
            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    dbRole = ctx.RbacRoles.Where(r => r.RoleId == role.RoleId).SingleOrDefault();
                    if(dbRole == null) //try with name
                        dbRole = ctx.RbacRoles.Where(r => r.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

                    if (dbRole == null)
                    {
                        RbacRole newRole = Assign(role);                        
                        newRole.Version = 1;
                        dbRole = ctx.RbacRoles.Add(newRole);
                    }
                    else
                    {
                        dbRole.Name = role.Name;
                        dbRole.Description = role.Description;                        
                        dbRole.MetaDataRbac = GetEncryptedString(role.MetaDataRbac);
                        dbRole.MetaDataEntitlements = GetEncryptedString(role.MetaDataEntitlements);
                        dbRole.Version = role.Version + 1;
                    }

                    ctx.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                RaiseError(e);
            }
            catch (DbUpdateException dbe)
            {
                RaiseError(dbe);
            }          
            return Assign(dbRole);
        }

        public void Delete(Framework.RbacRole user)
        {

        }
    }
}
