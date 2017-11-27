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
                        if(!string.IsNullOrEmpty(rbac.Name))
                            dbRbac.Name = rbac.Name;

                        if (!string.IsNullOrEmpty(rbac.Description))
                            dbRbac.Description = rbac.Description;

                        if (!string.IsNullOrEmpty(rbac.ConnectionString))
                            dbRbac.ConnectionString = GetEncryptedString(rbac.ConnectionString);

                        if (!string.IsNullOrEmpty(rbac.MetaDataRbac))
                            dbRbac.MetaDataRbac = GetEncryptedString(rbac.MetaDataRbac);

                        if (!string.IsNullOrEmpty(rbac.MetaDataEntitlements))
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

