using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    [DataContract]
    public class RbacColumn
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public RbacDataTypes DataType { get; set; }

        [DataMember]
        public RbacDBOperations AllowedOperations { get; set; }

        public RbacColumn(string name, RbacDataTypes dataType,
            bool create = false, bool read = false, bool update = false, bool delete = false)
        {
            this.Name = name;
            this.DataType = dataType;
            //this.IsFilterColumn = isFilterColumn;
            this.AllowedOperations = Rbac.ParseOperations(create, read, update, delete);
        }

        public RbacColumn(string name, string dataType,string create = "False", string read = "False", string update = "False", string delete = "False")
        {
            switch (dataType)
            {
                case "numeric":
                    dataType = "Decimal";
                    break;
            }
            this.Name = name;
            this.DataType = (RbacDataTypes)Enum.Parse(typeof(RbacDataTypes), dataType, true);
            //this.IsFilterColumn = bool.Parse(isFilterColumn);
            this.AllowedOperations = Rbac.ParseOperations(create, read, update, delete);
        }


    }
}
