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
        public void ApplyPermission()
        {
            ParsedQueryStage1 = ParsedQuery;
            switch (QueryType)
            {
                case RbacQueryTypes.Select:
                    ApplyPermissionSelect();
                    break;
                case RbacQueryTypes.Insert:
                    ApplyPermissionInsert();
                    break;
                case RbacQueryTypes.Update:
                    ApplyPermissionUpdate();
                    break;
                case RbacQueryTypes.Delete:
                    ApplyPermissionDelete();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        
    }
}
