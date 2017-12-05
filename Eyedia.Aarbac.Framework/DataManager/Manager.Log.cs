using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework.DataManager
{
    internal partial class Manager
    {
        public List<RbacLog> GetLogs(bool erroredOnly = true)
        {
            List<RbacLog> logs = new List<RbacLog>();
            using (var ctx = new Entities(ConnectionString))
            {
                //todo:max limit
                if(erroredOnly)
                    logs = ctx.RbacLogs.Where(l => l.Parsed == false).ToList();
                else
                    logs = ctx.RbacLogs.ToList();
            }
            return logs;
        }

        public RbacLog Save(string query, string parsedQuery, bool isParsed, string errors, int roleId, int userId)
        {
            RbacLog log = new RbacLog();
            log.Query = query;
            log.ParsedQuery = parsedQuery;
            log.Parsed = isParsed;
            log.Errors = errors;
            log.RoleId = roleId;
            log.UserId = userId;
            return Save(log);
        }
        public RbacLog Save(RbacLog log)
        {
            RbacLog dbLog = null;

            try
            {
                using (var ctx = new Entities(ConnectionString))
                {
                    log.DateTime = DateTime.Now;
                    dbLog = ctx.RbacLogs.Add(log);
                    ctx.SaveChanges();

                }
            }
            catch (DbEntityValidationException e)
            {
                RaiseError(e);
            }

            return dbLog;
        }
    }
}
