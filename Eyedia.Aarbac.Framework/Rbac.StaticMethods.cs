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

namespace Eyedia.Aarbac.Framework
{
    public partial class Rbac
    {
        public static Rbac GetRbac(int rbacId)
        {
            Rbac dbRbac = new DataManager.Manager(false).GetRbac(rbacId);
            if (dbRbac == null)
                RbacException.Raise(string.Format("Rbac with id '{0}' was not defined yet!", rbacId), RbacExceptionCategories.Web);

            return dbRbac;
        }
        public static Rbac GetRbac(string rbacName)
        {
            Rbac dbRbac = new DataManager.Manager(false).GetRbac(rbacName);
            if (dbRbac == null)
                RbacException.Raise(string.Format("Rbac '{0}' was not defined yet!", rbacName), RbacExceptionCategories.Web);

            return dbRbac;
        }

        public static List<Rbac> GetRbacs()
        {
            return new DataManager.Manager().GetRbacs();
        }

        public static void Delete(Rbac rbac)
        {
            new DataManager.Manager().Delete(rbac);
        }

        public static void Save(RbacEngineWeb rbacEngineWeb)
        {
            Rbac rbac = GetRbac(rbacEngineWeb.Name);
            rbac.Description = rbacEngineWeb.Description;
            rbac.ConnectionString = rbacEngineWeb.ConnectionString;
            new DataManager.Manager().AddOrUpdate(rbac);
        }

        public static RbacRole Save(RbacRoleWeb rbacRoleWeb)
        {
            return new DataManager.Manager().AddOrUpdate(RbacRoleWeb.Get(rbacRoleWeb));
        }

        public static void Delete(RbacUser rbacUser)
        {
            new DataManager.Manager().Delete(rbacUser);
        }

        public static List<RbacRole> GetRoles()
        {
            return new DataManager.Manager().GetRoles();            
        }

        public static RbacRole GetRole(int roleId)
        {
            return new DataManager.Manager(false).GetRole(roleId);
        }

        public static RbacRole GetRole(string roleName)
        {
            return new DataManager.Manager(false).GetRole(roleName);
        }

        public static List<RbacUser> GetUsers()
        {
            return new DataManager.Manager().GetUsers();
        }

        public static RbacUser GetUser(int userId)
        {
            return new DataManager.Manager().GetUser(userId);
        }

        public static RbacUser GetUser(string userName)
        {
            return new DataManager.Manager().GetUser(userName);
        }

        public static RbacUser CreateUser(string userName, string fullName, string email, string password, RbacRole role)
        {
            RbacUser user = new RbacUser(userName, fullName, email, role);
            ChangePassword(user.UserName, RbacCache.TempPassword, password);
            return user;
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return new DataManager.Manager().ChangePassword(userName, newPassword);
        }

        public static RbacDBOperations ParseOperations(string create = "False", string read = "False", string update = "False", string delete = "False")
        {
            return ParseOperations(bool.Parse(create), bool.Parse(read), bool.Parse(update), bool.Parse(delete));
        }

        public static RbacDBOperations ParseOperations(bool create = false, bool read = false, bool update = false, bool delete = false)
        {
            RbacDBOperations operations = RbacDBOperations.None;

            if (create)
            {
                operations = operations | RbacDBOperations.Create;
                operations &= ~RbacDBOperations.None;

            }
            if (read)
            {
                operations = operations | RbacDBOperations.Read;
                operations &= ~RbacDBOperations.None;
            }
            if (update)
            {
                operations = operations | RbacDBOperations.Update;
                operations &= ~RbacDBOperations.None;
            }
            if (delete)
            {
                operations = operations | RbacDBOperations.Delete;
                operations &= ~RbacDBOperations.None;
            }
            return operations;
        }

    }
}

