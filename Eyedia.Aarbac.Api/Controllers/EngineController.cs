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
    [RoutePrefix("api/engine")]
    public class EngineController : ApiController
    {
        [HttpGet]
        [Route]
        [ActionName("getall")]
        public List<Rbac> Get()
        {
            return Rbac.GetRbacs();
        }

        [HttpGet]
        [Route("{id}")]
        [ActionName("getbyid")]
        public RbacEngineWeb Get(int id)
        {
            RbacEngineWeb rbac = new RbacEngineWeb(Rbac.GetRbac(id));
            rbac.ConnectionString = string.Empty;
            return rbac;
        }

        [HttpGet]
        [Route]
        [ActionName("getbyname")]
        public RbacEngineWeb Get(string name)
        {
            RbacEngineWeb rbac = new RbacEngineWeb(Rbac.GetRbac(name));
            rbac.ConnectionString = string.Empty;
            return rbac;
        }

        // POST: api/engine
        [HttpPost]
        [Route]
        public RbacEngineWebResponse Post([FromBody]RbacEngineWebRequest request)
        {
            RbacEngineWebResponse response = new RbacEngineWebResponse();
            try
            {
                response.UserName = request.UserName;
                response.RoleName = request.RoleName;
                using (Rbac ctx = new Rbac(request.UserName, request.RbacName, request.RoleName))
                {
                    response.RbacName = request.RbacName;                    
                    SqlQueryParser parser = new SqlQueryParser(ctx, request.SkipParsing);
                    parser.Parse(request.Query);

                    using (RbacSqlQueryEngine eng = new RbacSqlQueryEngine(parser, request.DebugMode))
                    {                        
                        eng.SkipExecution = request.SkipExecution;
                        eng.Execute();
                        response.SetResult(eng);
                    }
                }
                
            }
            catch (Exception ex)
            {

                response.SetResult(ex.Message);
            }

            return response;
        }
       
        [HttpPut]
        [Route("{id}")]        
        public void Put(int id, [FromBody]RbacEngineWeb rbacEngineWeb)
        {
            Rbac.Save(rbacEngineWeb);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            Rbac.Delete(Rbac.GetRbac(id));
        }
    }
}
