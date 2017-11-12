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
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework.DataManager
{
    internal partial class Manager
    {
        public Framework.RbacUser Authenticate(string userName, string password)
        {
            Framework.RbacUser user = null;
            byte[] bPassword = GetEncryptedString(password);

            using (var ctx = new Entities(ConnectionString))
            {
                RbacUser dbUser = ctx.RbacUsers.AsEnumerable().Where(r => ((r.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)) && (r.Password.SequenceEqual(bPassword)))).SingleOrDefault();

                if (dbUser != null)
                {
                    HideSensitiveData = false;
                    user = Assign(dbUser);                    
                }              
            }
            return user;
        }

        public bool ChangePassword(string userName, string newPassword)
        {            
            byte[] bNewPassword = GetEncryptedString(newPassword);

            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    RbacUser dbUser = ctx.RbacUsers.Where(r => r.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (dbUser != null)
                    {
                        dbUser.Password = bNewPassword;
                        ctx.SaveChanges();
                        return true;
                    }
                }
            }
            catch (DbUpdateException dbe)
            {
                RaiseError(dbe);
            }
            catch (DbEntityValidationException e)
            {
                RaiseError(e);
            }
            return false;
        }

        public List<Framework.RbacUser> GetUsers()
        {
            List<Framework.RbacUser> users = new List<Framework.RbacUser>();
            using (var ctx = new Entities(ConnectionString))
            {
                List<RbacUser> dbUsers = ctx.RbacUsers.ToList();
                foreach (RbacUser dbUser in dbUsers)
                {
                    users.Add(Assign(dbUser));
                }
            }
            return users;
        }

        public Framework.RbacUser GetUser(int userId)
        {
            Framework.RbacUser user = null;

            using (var ctx = new Entities(ConnectionString))
            {
                RbacUser dbRbacUser = ctx.RbacUsers.Where(r => r.UserId == userId).SingleOrDefault();
                if (dbRbacUser != null)
                    user = Assign(dbRbacUser);
            }
            return user;
        }

        public Framework.RbacUser GetUser(string userName)
        {
            Framework.RbacUser user = null;

            using (var ctx = new Entities(ConnectionString))
            {
                RbacUser dbRbacUser = ctx.RbacUsers.Where(r => r.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (dbRbacUser != null)
                    user = Assign(dbRbacUser);
            }
            return user;
        }

        public Framework.RbacUser AddOrUpdate(Framework.RbacUser user)
        {
            RbacUser dbUser = null;
            try
            {
                using (var ctx = new Entities(ConnectionString))
                {                   
                    dbUser = ctx.RbacUsers.Where(r => r.UserId == user.UserId).SingleOrDefault();
                    if(dbUser == null) //try with userName
                        dbUser = ctx.RbacUsers.Where(r => r.UserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

                    if (dbUser == null)
                    {
                        RbacUser newUser = Assign(user);
                        newUser.RoleId = user.Role.RoleId;
                        newUser.Password = GetEncryptedString(RbacCache.TempPassword); //this needs to be immediately changed
                        dbUser = ctx.RbacUsers.Add(newUser);
                    }
                    else
                    {
                        user.UserId = dbUser.UserId;
                        dbUser.FullName = user.FullName;                        
                        dbUser.Email = user.Email;
                    }
                    ctx.SaveChanges();                   
                }
            }
            catch (DbUpdateException dbe)
            {
                RaiseError(dbe);
            }
            catch (DbEntityValidationException e)
            {
                RaiseError(e);
            }
            return Assign(dbUser);
        }

        public void Delete(Framework.RbacUser user)
        {

        }
    }
}

