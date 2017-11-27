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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Eyedia.Aarbac.Framework;
using System.Threading.Tasks;

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
        public RbacEngineWeb Put(int id, [FromBody]RbacEngineWeb rbacEngineWeb)
        {            
            return Rbac.Save(rbacEngineWeb);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            Rbac.Delete(Rbac.GetRbac(id));
            return Ok();
        }
       
    }
}

