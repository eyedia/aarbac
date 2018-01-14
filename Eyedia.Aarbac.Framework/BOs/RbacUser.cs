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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Eyedia.Aarbac.Framework
{
    public class RbacUser
    {
        public RbacUser()
        {
        }

        public RbacUser(int userId)
        {
            RbacUser user = new DataManager.Manager(false).GetUser(UserId);
            if (user == null)
                RbacException.Raise(string.Format("User not found with userId as '{0}'!", userId), RbacExceptionCategories.Repository);

            Assign(user);
            PopulateParameters();
        }

        public RbacUser(string userName)
        {
            RbacUser user = new DataManager.Manager(false).GetUser(userName);
            if (user == null)
                RbacException.Raise(string.Format("User not found with username as '{0}'!", userName), RbacExceptionCategories.Repository);

            Assign(user);
            PopulateParameters();
            
        }

        private void PopulateParameters()
        {
            this._Parameters = new ObservableDictionary<string, string>();
            this._Parameters.PropertyChanged += ParametersPropertyChanged;
            this._Parameters.CollectionChanged += ParametersCollectionChanged;

            List<RbacParameter> parameters = new DataManager.Manager(false).GetParameters(this.UserId);
            foreach(RbacParameter parameter in parameters)
            {
                this._Parameters.Add(parameter.Name, parameter.Value);
            }
        }

        public RbacUser(string userName, string fullName, string email, RbacRole role, string password = null)
        {
            RbacUser newUser = new RbacUser();
            newUser.UserName = userName;           
            newUser.FullName = fullName;
            newUser.Email = email;
            newUser.Role = role;
            newUser = new DataManager.Manager(false).AddOrUpdate(newUser);
            Assign(newUser);
            Parameters = new ObservableDictionary<string, string>();
        }

        private void Assign(RbacUser user)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.FullName = user.FullName;
            this.Email = user.Email;
            this.Role = user.Role;
            this.Role.ParseMetaData();
        }

        [ReadOnly(true)]
        public int UserId { get; set; }

        [ReadOnly(true)]
        public string UserName { get; set; }

        public string FullName { get; set; }        
        public string Email { get; set; }
        private RbacRole _Role;
        public RbacRole Role
        {
            get
            {
                return _Role;
            }
            set
            {
                _Role = value;
                _Role.ParseMetaData();
                ParseRoleParameters();
            }
        }       

        ObservableDictionary<string, string> _Parameters;
        public ObservableDictionary<string, string> Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; ParseRoleParameters(); }
        }

        private void ParseRoleParameters()
        {
            if ((_Parameters == null) || (_Parameters.Count == 0))
                return;

            foreach (RbacParameter parameter in _Role.Parameters)
            {
                if (this._Parameters.ContainsKey(parameter.Name))
                    parameter.Value = this._Parameters[parameter.Name];
            }
        }

        private void ParametersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //ParseRoleParameters();
        }

        private void ParametersPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ParseRoleParameters();
        }
    }
}


