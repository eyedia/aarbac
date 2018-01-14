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
using System.Data.Entity.Validation;
using System.Data.Entity;
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
                    logs = ctx.RbacLogs.Where(l => l.Parsed == false).Include(lu => lu.RbacUser).Include(lr => lr.RbacRole).ToList();
                else
                    logs = ctx.RbacLogs.Include(lu => lu.RbacUser).Include(lr => lr.RbacRole).ToList();
            }
            return logs;
        }

        public RbacLog Save(string query, string parsedQuery, bool isParsed, string errors, int roleId, int userId, ExecutionTime executionTime)
        {
            RbacLog log = new RbacLog();
            log.Query = query;
            log.ParsedQuery = parsedQuery;
            log.Parsed = isParsed;
            log.Errors = errors;
            log.RoleId = roleId;
            log.UserId = userId;
            if (executionTime.Items.ContainsKey(ExecutionTimeTrackers.ParseQuery))
                log.ElapsedTimeParseQuery = executionTime.Items[ExecutionTimeTrackers.ParseQuery].ElapsedTicks;
            if (executionTime.Items.ContainsKey(ExecutionTimeTrackers.ConditionsNRelations))
                log.ElapsedTimeConditionsNRelations = executionTime.Items[ExecutionTimeTrackers.ConditionsNRelations].ElapsedTicks;
            if (executionTime.Items.ContainsKey(ExecutionTimeTrackers.ApplyPermissions))
                log.ElapsedTimeApplyPermission = executionTime.Items[ExecutionTimeTrackers.ApplyPermissions].ElapsedTicks;
            if (executionTime.Items.ContainsKey(ExecutionTimeTrackers.ApplyParameters))
                log.ElapsedTimeApplyParameters = executionTime.Items[ExecutionTimeTrackers.ApplyParameters].ElapsedTicks;
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

