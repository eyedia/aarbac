using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {        
        private bool ApplyParameters()
        {
            if ((Context.User == null)
                || (Context.User.Parameters == null))
                return true;

            var regex = new Regex("{.*?}");
            var parameters = regex.Matches(ParsedQuery);
            ParamErrors = new List<string>();
            foreach (var parameter in parameters)
            {
                string key = parameter.ToString().Replace(" ", "");
                if (Context.User.Parameters.ContainsKey(key))
                {
                    ParsedQuery = ParsedQuery.Replace(parameter.ToString(), Context.User.Parameters[key]);
                }
                else
                {
                    ParamErrors.Add(string.Format("User '{0}' does not have parameter '{1}'", Context.User.UserName, key));
                }
            }

            return ParamErrors.Count == 0 ? true : false;
        }
    }
}
