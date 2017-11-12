using System;
using System.Collections.Generic;
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


        public int UserId { get; set; }
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
