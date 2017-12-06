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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Eyedia.Aarbac.Framework
{
    
    public partial class Rbac : RbacBase, IRbac, IDisposable
    {        
        public int RbacId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; internal set; }        
        public string MetaDataRbac { get; internal set; }
        public string MetaDataEntitlements { get; internal set; }
        public int Version { get; set; }
        private byte[] PassKey;
        public StringWriterTrace Trace { get; private set; }
        public RbacUser User { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public Rbac()
        {
            //this is only to satisfy serializers
            this.Trace = new StringWriterTrace();
        }      

        /// <summary>
        /// This instance is allowed to call only from test bed - todo security
        /// </summary>
        /// <param name="userName">The logged in user name</param>
        /// <param name="rbacName">The overridden rbac name, empty will use user's default</param>
        /// <param name="roleName">The overriden role name, empty will use user's default</param>
        public Rbac(string userName, string rbacName, string roleName)
        {
            //no cache will be implemented

            DataManager.Manager manager = new DataManager.Manager(false);
            RbacUser user = new RbacUser(userName);
                       
            if (!string.IsNullOrEmpty(roleName))            
                user.Role = new RbacRole(roleName);  //override role

            if (user.Role == null)
                RbacException.Raise(string.Format("The role '{0}' is not found!", roleName), RbacExceptionCategories.Repository);

            Rbac dbRbac = null;
            if (!string.IsNullOrEmpty(rbacName))
                 dbRbac = manager.GetRbac(rbacName);    //override rbac
            else
                dbRbac = manager.GetRbac(user.Role.RbacId);

            if (dbRbac == null)
                RbacException.Raise(string.Format("The rbac '{0}' was not defined yet!", rbacName), RbacExceptionCategories.Repository);

            dbRbac.User = user; 
            Assign(dbRbac);           

        }       

        public Rbac(string userName)
        {
            Rbac rbacFromCache = RbacCache.Instance.GetContext(userName);
            if (rbacFromCache == null)
            {               
                RbacUser user = new RbacUser(userName);
                Rbac dbRbac = new DataManager.Manager(false).GetRbac(user.Role.RbacId);
                dbRbac.User = user;               
                Assign(dbRbac);
                RbacCache.Instance.Contexts.Add(userName, this);
            }
            else
            {
                Assign(rbacFromCache);
            }
        }

        public Rbac CreateNew(string rbacName, string description, string connectionString, string metaDataEntitlement)
        {
            DataManager.Manager manager = new DataManager.Manager(false);
            if (manager.GetRbac(rbacName) != null)
                RbacException.Raise(string.Format("'{0}' already exists! Please provide a different name.", rbacName), RbacExceptionCategories.Repository);

            Rbac newRbac = new Rbac();
            newRbac.Name = rbacName;
            newRbac.Description = description;
            newRbac.ConnectionString = connectionString;
            N("Generating meta data...");
            newRbac.MetaDataRbac = RbacMetaData.Generate(newRbac.ConnectionString);
            N("Done!", LogMessageTypes.Success);
            N("Saving your rbac instance...");
            Rbac rbac = manager.AddOrUpdate(newRbac);
            N("Done!", LogMessageTypes.Success);
            return rbac;            
        }


        /// <summary>
        /// This will refresh the rule from the meta data, byt merging latest changes from the database
        /// </summary>
        public void Refresh()
        {
            if ((User != null)
                && (User.Role != null)
                && (User.Role.CrudPermissions != null))
            {
                RbacRole dbRole = User.Role;
                dbRole.MetaDataRbac = RbacMetaData.Merge(ConnectionString, User.Role.CrudPermissions);
                new DataManager.Manager().AddOrUpdate(dbRole);
            }
        }

        //private void Assign(Rbac rbac, string userName)
        //{
        //    rbac.User = Rbac.GetUser(userName);
        //    Assign(rbac);
        //    RbacCache.Instance.Contexts.Add(userName, this);
        //}

        private void Assign(Rbac rbac)
        {
            this.User = rbac.User;
            this.RbacId = rbac.RbacId;
            this.Name = rbac.Name;
            this.Description = rbac.Description;
            this.ConnectionString = rbac.ConnectionString;
            this.MetaDataRbac = rbac.MetaDataRbac;
            this.MetaDataEntitlements = rbac.MetaDataEntitlements;
            this.Version = rbac.Version;
            this.MetaDataEntitlements = rbac.MetaDataEntitlements;
            this.Trace = new StringWriterTrace();            
        }

        public void Save()
        {
            new DataManager.Manager().AddOrUpdate(this);
        }

        public bool Authenticate(string password)
        {
            IsAuthenticated = new DataManager.Manager().Authenticate(this.RbacId, password);
            if (!IsAuthenticated)
                RbacException.Raise("Incorrect password!");

            return IsAuthenticated;
        }

        public RbacUser Authenticate(string userName, string password)
        {
            RbacUser user = GetUser(userName);
            if (user == null)
                RbacException.Raise("User name was not found!");

            user = new DataManager.Manager().Authenticate(userName, password);
            if (user == null)
                RbacException.Raise("Incorrect password!");

            return user;
        }

     
        public RbacRole CreateRole(string roleName, string roleDescription, string metaDataRbac, string metaDataEntitlements)
        {
            return new RbacRole(this.RbacId, roleName, roleDescription, metaDataRbac, metaDataEntitlements);
        }

        public void Export(string fileName)
        {
            Rbac rbac = new DataManager.Manager(false).GetRbac(this.Name);
            RbacEngineWeb wRbac = new RbacEngineWeb(rbac);            
            StreamWriter sw = new StreamWriter(fileName);
            var s = new System.Xml.Serialization.XmlSerializer(wRbac.GetType());
            s.Serialize(sw, wRbac);
            sw.Close();
        }

        #region Helpers

        int indentLevel = 0;
        public void WriteTrace(string message)
        {
            string prefix = string.Empty;
            Trace.Write(new string('\t', indentLevel) + prefix + message);
        }
        

        public override string ToString()
        {
            return Trace.ToString();
        }
        public void Dispose()
        {
            if (this.Trace != null)
            {
                this.Trace.Close();
                this.Trace.Dispose();
            }
        }

        #endregion Helpers

    }
}

