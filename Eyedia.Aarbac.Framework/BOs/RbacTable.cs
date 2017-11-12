using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    [DataContract]
    public class RbacTable
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        [DataMember]
        public List<RbacColumn> Columns { get; set; }

        [DataMember]
        public List<RbacCondition> Conditions { get; set; }
        public RbacOrderBy OrderBy { get; }

        [DataMember]
        public List<RbacParameter> Parameters { get; set; }

        [DataMember]
        public List<RbacRelation> Relations { get; set; }

        [DataMember]
        public RbacDBOperations AllowedOperations { get; set; }
        public bool ReferencedOnly { get; internal set; }
        public string TempAlias { get; internal set; }

        public RbacTable(string objectId, string name,
            bool create = false, bool read = false, bool update = false, bool delete = false)
        {
            this.ObjectId = objectId;
            this.Name = name;
            this.Columns = new List<RbacColumn>();
            this.Conditions = new List<RbacCondition>();            
            this.Relations = new List<RbacRelation>();
            this.Parameters = new List<RbacParameter>();            
            this.AllowedOperations = Rbac.ParseOperations(create, read, update, delete);
        }

        public RbacTable(string objectId, string name,
           string create = "False", string read = "False", string update = "False", string delete = "False")
        {
            this.ObjectId = objectId;
            this.Name = name;
            this.Columns = new List<RbacColumn>();
            this.Conditions = new List<RbacCondition>();            
            this.Relations = new List<RbacRelation>();
            this.Parameters = new List<RbacParameter>();
            this.AllowedOperations = Rbac.ParseOperations(create, read, update, delete);
        }

        
        public string WhereClause
        {
            get
            {
                if (Conditions.Count == 0)
                    return string.Empty;
                else
                    return Conditions.Select(i => i.WhereClause).Aggregate((i, j) => i + " and " + j);

            }
        }
    }

}
