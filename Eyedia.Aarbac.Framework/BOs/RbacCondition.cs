using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
   
    [DataContract]
    public class RbacCondition
    {
        public RbacCondition()
        {
            
        }

        public RbacCondition(string selfTableName, string name, string whereClause)
        {
            SelfTableName = selfTableName;
            Name = name;
            WhereClause = whereClause;           
        }

        [DataMember]
        public string SelfTableName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string WhereClause { get; set; }

        public override string ToString()
        {
            return string.Format("<Condition Name=\"{0}\" WhereClause=\"{1}\" />",
                Name, WhereClause);
        }

    }
}
