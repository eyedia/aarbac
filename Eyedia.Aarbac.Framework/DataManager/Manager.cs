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
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework.DataManager
{
   
    internal partial class Manager
    {
        private string ConnectionString { get; set; }
        public bool HideSensitiveData { get; internal set; }       

        public Manager()
        {
            Init();
            HideSensitiveData = true;
        }

        internal Manager(bool hideSensitiveData = true)
        {
            Init();
            HideSensitiveData = hideSensitiveData;
        }

        private void Init()
        {
            ConnectionString = GetEFConnectionString();                    
        }

        private Rbac Assign(Framework.Rbac rbac)
        {
            if (rbac == null)
                return null;

            Rbac newRbac = new Rbac();
            newRbac.RbacId = rbac.RbacId;
            newRbac.Name = rbac.Name;
            newRbac.Description = rbac.Description;
            newRbac.ConnectionString = GetEncryptedString(rbac.ConnectionString);
            newRbac.MetaDataRbac = GetEncryptedString(rbac.MetaDataRbac);
            newRbac.MetaDataEntitlements = GetEncryptedString(rbac.MetaDataEntitlements);            
            newRbac.Version = rbac.Version;
            return newRbac;
        }

        private Framework.Rbac Assign(Rbac rbac)
        {
            if (rbac == null)
                return null;

            Framework.Rbac newRbac = new Framework.Rbac();
            newRbac.RbacId = rbac.RbacId;
            newRbac.Name = rbac.Name;
            newRbac.Description = rbac.Description;
            if (!HideSensitiveData)
            {
                newRbac.ConnectionString = GetDecryptedString(rbac.ConnectionString);
                newRbac.MetaDataRbac = GetDecryptedString(rbac.MetaDataRbac);                
                newRbac.MetaDataEntitlements = GetDecryptedString(rbac.MetaDataEntitlements);
            }
            newRbac.Version = rbac.Version;

            return newRbac;
        }

        private RbacRole Assign(Framework.RbacRole rbacRole)
        {
            if (rbacRole == null)
                return null;

            RbacRole newRole = new RbacRole();
            newRole.RoleId = rbacRole.RoleId;
            newRole.RbacId = rbacRole.RbacId;           
            newRole.Name = rbacRole.Name;
            newRole.Description = rbacRole.Description;            
            newRole.MetaDataRbac = GetEncryptedString(rbacRole.MetaDataRbac);
            newRole.MetaDataEntitlements = GetEncryptedString(rbacRole.MetaDataEntitlements);
            newRole.Version = rbacRole.Version;
            return newRole;
        }

        private Framework.RbacRole Assign(RbacRole rbacRole)
        {
            if (rbacRole == null)
                return null;

            Framework.RbacRole newRbacRole = new Framework.RbacRole();
            newRbacRole.RbacId = rbacRole.RbacId;           
            newRbacRole.RoleId = rbacRole.RoleId;
            newRbacRole.Name = rbacRole.Name;
            newRbacRole.Description = rbacRole.Description;
            if (!HideSensitiveData)
            {
                newRbacRole.MetaDataRbac = GetDecryptedString(rbacRole.MetaDataRbac);
                newRbacRole.MetaDataEntitlements = GetDecryptedString(rbacRole.MetaDataEntitlements);
            }
            newRbacRole.Version = (int)rbacRole.Version;
            return newRbacRole;
        }

        //private RbacEntitlement Assign(Framework.RbacEntitlement entitlement)
        //{
        //    if (entitlement == null)
        //        return null;

        //    RbacEntitlement dbEntitlement = new RbacEntitlement();
        //    dbEntitlement.RbacId = entitlement.RbacId;
        //    dbEntitlement.EntitlementId = entitlement.EntitlementId;          
        //    dbEntitlement.Name = entitlement.Name;
        //    dbEntitlement.Description = entitlement.Description;
        //    dbEntitlement.MetaData = GetEncryptedString(entitlement.MetaDatar);
        //    dbEntitlement.Version = entitlement.Version;
        //    return dbEntitlement;
        //}

        //private Framework.RbacEntitlement Assign(RbacEntitlement rbacRole)
        //{
        //    if (rbacRole == null)
        //        return null;

        //    Framework.RbacEntitlement newRbacRole = new Framework.RbacEntitlement();
        //    newRbacRole.RbacId = rbacRole.RbacId;           
        //    newRbacRole.EntitlementId = rbacRole.EntitlementId;            
        //    newRbacRole.Name = rbacRole.Name;
        //    newRbacRole.Description = rbacRole.Description;
        //    if (!HideSensitiveData)
        //    {
        //        newRbacRole.MetaData = GetDecryptedString(rbacRole.MetaData);
        //    }
        //    newRbacRole.Version = (int)rbacRole.Version;
        //    return newRbacRole;
        //}

        private RbacUser Assign(Framework.RbacUser user)
        {
            if (user == null)
                return null;

            RbacUser newUser = new RbacUser();
            newUser.RoleId = user.Role.RoleId;
            newUser.UserId = user.UserId;
            newUser.UserName = user.UserName;            
            newUser.FullName = user.FullName;
            newUser.Email = user.Email;            
            return newUser;
        }

        private Framework.RbacUser Assign(RbacUser user)
        {
            if (user == null)
                return null;

            Framework.RbacUser newUser = new Framework.RbacUser();
            if (!HideSensitiveData)
                newUser.Role = GetRole(user.RoleId);
            newUser.UserId = user.UserId;
            newUser.UserName = user.UserName;           
            newUser.FullName = user.FullName;
            newUser.Email = user.Email;            
            return newUser;
        }

        private RbacUserParameter Assign(Framework.RbacParameter parameter)
        {
            RbacUserParameter newParam = new RbacUserParameter();
            newParam.Name = GetEncryptedString(parameter.Name);
            newParam.Value = GetEncryptedString(parameter.Value);         
            return newParam;
        }

        private Framework.RbacParameter Assign(RbacUserParameter parameter)
        {
            Framework.RbacParameter newParam = new Framework.RbacParameter();
            newParam.Name = GetDecryptedString(parameter.Name);
            newParam.Value = GetDecryptedString(parameter.Value);            
            return newParam;
        }

        private byte[] GetEncryptedString(string plainText)
        {
            return new Encryption().Encrypt(plainText);
        }

        private string GetDecryptedString(byte[] encryptedString)
        {
            return new Encryption().Decrypt(encryptedString);
        }

        private string RaiseError(DbEntityValidationException e, bool raiseError = true)
        {
            List<string> errors = new List<string>();
            foreach (var eve in e.EntityValidationErrors)
            {
                string s = string.Format("{0} {1}", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    errors.Add(string.Format("{0} - Property {1}, Error = {2}", s,
                        ve.PropertyName, ve.ErrorMessage));
                    errors.Add(s);
                }
            }

            string errorMessage = errors.Select(i => i).Aggregate((i, j) => i + Environment.NewLine + j);

            if (raiseError)
                RbacException.Raise(errorMessage, RbacExceptionCategories.Repository);

            return errorMessage;
        }

        private string RaiseError(Exception ex, bool raiseError = true)
        {
            //List<string> errors = new List<string>();

            //try
            //{

            //    foreach (var result in dbu.Entries)
            //    {                    
            //        errors.Add(string.Format("Type: {0} was part of the problem. ", result.Entity.GetType().Name));
            //    }
            //}
            //catch (Exception e)
            //{
            //    errors.Add(e.ToString());
            //}

            //string errorMessage = errors.Select(i => i).Aggregate((i, j) => i + Environment.NewLine + j);

           List<Exception> exceptions = ex.GetInnerExceptions().ToList();
            exceptions.RemoveAll(e => e.Message == "An error occurred while updating the entries. See the inner exception for details.");
            string errorMessage = exceptions.Select(i => i.Message).Aggregate((i, j) => i + Environment.NewLine + j);

            if (raiseError)
                RbacException.Raise(errorMessage, RbacExceptionCategories.Repository);

            return errorMessage;
        }

        public static string GetEFConnectionString()
        {            
            var cs = ConfigurationManager.ConnectionStrings["aarbac"];
            if (cs == null)
                RbacException.Raise(MessagesConfigurations.conn_str_not_found, RbacExceptionCategories.Configuration);

            string entConnection =
                string.Format(
                    "metadata=res://*/DataManager.RbacDbModel.csdl|res://*/DataManager.RbacDbModel.ssdl|res://*/DataManager.RbacDbModel.msl;provider=System.Data.SqlClient;provider connection string=\"{0};MultipleActiveResultSets=True;App=EntityFramework\"",
                    cs.ConnectionString);
            return entConnection;
        }

    }
}

