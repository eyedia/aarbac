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
