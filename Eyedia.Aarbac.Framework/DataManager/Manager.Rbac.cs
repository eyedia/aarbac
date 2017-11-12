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
        public bool Authenticate(int rbactId, string password)
        {            
            byte[] bPassword = GetEncryptedString(password);
            
            using (var ctx = new Entities(ConnectionString))
            {
                Rbac dbRbac = ctx.Rbacs.AsEnumerable().Where(r => ((r.RbacId == rbactId) && (r.Password.SequenceEqual(bPassword)))).SingleOrDefault();

                if (dbRbac != null)
                    return true;
            }

            return false;
        }

        public List<Framework.Rbac> GetRbacs()
        {
            List<Framework.Rbac> rbacs = new List<Framework.Rbac>();
            using (var ctx = new Entities(ConnectionString))
            {
                List<Rbac> dbRbacs = ctx.Rbacs.ToList();
                foreach(Rbac dbRbac in dbRbacs)
                {
                    rbacs.Add(Assign(dbRbac));
                }
            }
            return rbacs;
        }

        public Framework.Rbac GetRbac(int rbacId)
        {
            Framework.Rbac rbac = null;
            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    Rbac dbRbac = ctx.Rbacs.Where(r => r.RbacId == rbacId).SingleOrDefault();
                    if (dbRbac != null)
                        rbac = Assign(dbRbac);
                }
            }
            catch (Exception ex)
            {
                RaiseError(ex);
            }
            return rbac;
        }

        public Framework.Rbac GetRbac(string name)
        {
            Framework.Rbac rbac = null;
            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    Rbac dbRbac = ctx.Rbacs.Where(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (dbRbac != null)
                        rbac = Assign(dbRbac);
                }
            }
            catch(Exception ex)
            {
                RaiseError(ex);
            }
            return rbac;
        }

        public bool ChangePasswordRbac(string rbacName, string newPassword)
        {
            byte[] bNewPassword = GetEncryptedString(newPassword);

            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    Rbac dbRbac = ctx.Rbacs.Where(r => r.Name.Equals(rbacName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (dbRbac != null)
                    {
                        dbRbac.Password = bNewPassword;
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

        public Framework.Rbac AddOrUpdate(Framework.Rbac rbac)
        {
            Rbac dbRbac = null;

            try
            {               
                using (var ctx = new Entities(ConnectionString))
                {
                    dbRbac = ctx.Rbacs.Where(r => r.RbacId == rbac.RbacId).SingleOrDefault();
                    if (dbRbac == null) //try with name
                        dbRbac = ctx.Rbacs.Where(r => r.Name.Equals(rbac.Name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    
                    if (dbRbac == null)
                    {
                        Rbac newRbac = Assign(rbac);
                        newRbac.Password = GetEncryptedString("password");  //default, user supposed to change immediately
                        newRbac.Version = 1;
                        dbRbac = ctx.Rbacs.Add(newRbac);
                    }
                    else
                    {
                        dbRbac.Name = rbac.Name;
                        dbRbac.Description = rbac.Description;
                        dbRbac.ConnectionString = GetEncryptedString(rbac.ConnectionString);
                        dbRbac.MetaDataRbac = GetEncryptedString(rbac.MetaDataRbac);
                        dbRbac.MetaDataEntitlements = GetEncryptedString(rbac.MetaDataEntitlements);                        
                        dbRbac.Version = rbac.Version + 1;                        
                    }

                    ctx.SaveChanges();
                    
                }
            }
            catch (DbEntityValidationException e)
            {
                RaiseError(e);
            }
            
            return Assign(dbRbac);
        }       

        public void Delete(Framework.Rbac user)
        {

        }
    }
}
