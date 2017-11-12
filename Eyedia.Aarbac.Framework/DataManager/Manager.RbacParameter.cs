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
