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


