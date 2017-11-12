using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eyedia.Aarbac.Framework;

namespace Eyedia.Aarbac.Api.Controllers
{
    [RoutePrefix("api/roles")]
    public class RolesController : ApiController
    {
        [HttpGet]
        [Route]
        [ActionName("getall")]
        public List<RbacRoleWeb> Get()
        {
            List<RbacRole> roles = Rbac.GetRoles();
            List<RbacRoleWeb> rolesW = new List<RbacRoleWeb>();

            foreach (var role in roles)
            {
                rolesW.Add(new RbacRoleWeb(role));
            }

            return rolesW;
        }

        [ActionName("getbyid")]
        public RbacRoleWeb Get(int id)
        {
            RbacRole role = Rbac.GetRole(id);
            if (role != null)
            {
                role.ParseMetaData();
                return new RbacRoleWeb(role);                
            }
            return null;
        }

        [ActionName("getbyname")]
        public RbacRoleWeb Get(string name)
        {
            RbacRole role = Rbac.GetRole(name);
            if (role != null)
            {
                role.ParseMetaData();
                return new RbacRoleWeb(role);
            }
            return null;
        }

        [HttpPost]
        [Route]
        public RbacRole Post([FromBody]RbacRoleWeb role)
        {
            return Rbac.Save(role);
        }

        [HttpPut]
        [Route]
        public RbacRole Put(int id, [FromBody]RbacRoleWeb role)
        {
            return Rbac.Save(role);
        }

        [HttpDelete]
        [Route]
        public void Delete(int id)
        {
        }
    }
}
