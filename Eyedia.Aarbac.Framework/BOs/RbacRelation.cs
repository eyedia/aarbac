using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    [DataContract]
    public class RbacRelation
    {
        public RbacRelation()
        {

        }

        public RbacRelation(string selfName, string selfColumnName, string withTableDotColumn)
        {
            SelfName = selfName;
            SelfColumnName = selfColumnName;
            WithTable = withTableDotColumn.Split('.')[0];
            WithColumn = withTableDotColumn.Split('.')[1];
        }

        [DataMember]
        public string SelfName { get; set; }

        [DataMember]
        public string SelfColumnName{ get; set; }

        [DataMember]
        public string WithTable { get; set; }

        [DataMember]
        public string WithColumn { get; set; }

        public string JoinClause
        {
            get
            {
                return string.Format(" inner join {0} on {1}.{2} = {0}.{3}", WithTable, SelfName, SelfColumnName, WithColumn);
            }
        }
        
    }
}
