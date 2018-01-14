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
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework.DataManager
{
    internal partial class Manager
    {
        public List<Framework.RbacRole> GetRoles(int rbacId = 0)
        {
            List<Framework.RbacRole> roles = new List<Framework.RbacRole>();
            using (var ctx = new Entities(ConnectionString))
            {
                List<RbacRole> dbRoles = new List<RbacRole>();
                if (rbacId == 0)
                    dbRoles = ctx.RbacRoles.ToList();
                else
                    dbRoles = ctx.RbacRoles.Where(r => r.RbacId == rbacId).ToList();

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
            RbacMetaData rbacMetaData = new RbacMetaData();
            rbacMetaData.ValidateAndGetRbacXmlDocument(role.MetaDataRbac);
            if (rbacMetaData.XmlValidationErrors.Count > 0)
                RbacException.Raise("Cannot save role meta data, XML validation failed!" 
                    + Environment.NewLine 
                    + rbacMetaData.XmlValidationErrors.ToLine());

            rbacMetaData.ValidateAndGetEntitlementXmlDocument(role.MetaDataEntitlements);
            if (rbacMetaData.XmlValidationErrors.Count > 0)
                RbacException.Raise("Cannot save role entitlement meta data, XML validation failed!"
                    + Environment.NewLine
                    + rbacMetaData.XmlValidationErrors.ToLine());

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


