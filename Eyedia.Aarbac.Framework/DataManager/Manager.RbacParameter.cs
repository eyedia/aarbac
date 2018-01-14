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
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework.DataManager
{
    internal partial class Manager
    {
        public List<Framework.RbacParameter> GetParameters(int userId)
        {
            List<Framework.RbacParameter> parameters = new List<Framework.RbacParameter>();
            using (var ctx = new Entities(ConnectionString))
            {
                List<RbacUserParameter> dbParameters = ctx.RbacUserParameters.Where(p => p.UserId == userId).ToList();
                foreach (RbacUserParameter dbParameter in dbParameters)
                {
                    parameters.Add(Assign(dbParameter));
                }
            }
            return parameters;
        }

       
        public Framework.RbacParameter AddOrUpdateUserParameter(int userId, string paramName, string paramValue)
        {
            Framework.RbacParameter parameter = new Framework.RbacParameter();
            parameter.Name = paramName;
            parameter.Value = paramValue;          
            byte[] bName = GetEncryptedString(paramName);
            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    RbacUserParameter existingParameter = null;
                    existingParameter = ctx.RbacUserParameters.AsEnumerable().Where(r => ((r.UserId == userId) && (r.Name.SequenceEqual(bName)))).SingleOrDefault();
       
                    if (existingParameter == null)
                    {
                        RbacUserParameter newParam = new RbacUserParameter();
                        newParam.Name = GetEncryptedString(paramName);
                        newParam.Value = GetEncryptedString(paramValue);
                        newParam.UserId = userId;
                        ctx.RbacUserParameters.Add(newParam);
                    }
                    else
                    {                       
                        existingParameter.Value = GetEncryptedString(paramValue);
                    }

                    ctx.SaveChanges();
                   
                }
            }
            catch (DbUpdateException dbe)
            {

            }
            catch (DbEntityValidationException e)
            {
                RaiseError(e);
            }
            return parameter;
        }
       
    }
}


