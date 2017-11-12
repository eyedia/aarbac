using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    interface IRbac
    {
        RbacUser Authenticate(string userName, string password);
        //bool ChangePassword(string userName, string oldPassword, string newPassword);
        //RbacEntitlement CreateEntilement(string name, string description, string metaData);
        RbacRole CreateRole(string roleName, string roleDescription, string metaDataRbac, string metaDataEntitlements);
        //RbacRole GetRole(string roleName);
        //RbacUser CreateUser(string userName, string fullName, string email, string password, RbacRole role);
        //RbacUser GetUser(string userName);
        void Save();
    }
}
