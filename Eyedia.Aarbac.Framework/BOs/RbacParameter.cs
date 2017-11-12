using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    [DataContract]
    public class RbacParameter
    {
        public RbacParameter()
        {

        }

        public RbacParameter(string name, string value = null)
        {
            Name = name;
            Value = value;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
