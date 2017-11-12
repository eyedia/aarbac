using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEngineWebRequest
    {        
        public string UserName { get; set; }
        public string RbacName { get; set; }
        public string RoleName { get; set; }
        public string Query { get; set; }
        public bool SkipParsing { get; set; }
        public bool SkipExecution { get; set; }
        public bool DebugMode { get; set; }

        public RbacEngineWebRequest()
        {
           
        }
       
    }
}
