using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Symplus.Rbac.Api.Controllers
{
    //[Authorize]
    public class RbacController : ApiController
    {
        [ActionName("getall")]
        public string Get()
        {
            return JsonConvert.SerializeObject(new DataManager.Manager().GetRbacs());
        }

        [ActionName("getbyid")]
        public string Get(int id)
        {
            return JsonConvert.SerializeObject(new DataManager.Manager().GetRbac(id));
        }

        [ActionName("getbyname")]
        public string Get(string name)
        {
            return JsonConvert.SerializeObject(new DataManager.Manager().GetRbac(name));
        }

        // POST: api/Rbac
        public string Post([FromBody]RbacWebRequest request)
        {
            RbacWebResponse response = new RbacWebResponse(request.RbacId);

            if (request.RbacId == 0)
            {
                response.SetResult(string.Format("Rbac instance with rbac id '{0}' not found!", request.RbacId));
                return JsonConvert.SerializeObject(response);
            }

            RbacUser user = new Rbac(request.RbacId).GetUser(request.UserName);

            if (user == null)
            {
                response.SetResult(string.Format("User '{0}' not found!", request.UserName));
                return JsonConvert.SerializeObject(response);
            }

            if (!string.IsNullOrEmpty(request.RoleName))
            {
                user.Role = new RbacRole(request.RoleName);   //dangerous code, only to be used for role testing
                if (user.Role == null)
                {
                    response.SetResult(string.Format("Role '{0}' not found!", request.RoleName));
                    return JsonConvert.SerializeObject(response);
                }
            }
            SqlQueryParser parser = new SqlQueryParser(new RbacContext(user), request.SkipParsing);
            parser.Parse(request.Query);
            RbacSqlQueryEngine engine = new RbacSqlQueryEngine(parser, request.DebugMode);
            engine.SkipExecution = request.SkipExecution;
            engine.Execute();
            response.SetResult(engine);
            return JsonConvert.SerializeObject(response);
        }

        // PUT: api/Rbac/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Rbac/5
        public void Delete(int id)
        {
        }
    }
}
