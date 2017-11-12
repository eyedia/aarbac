using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEngineWeb
    {
        public RbacEngineWeb()
        { }

        public RbacEngineWeb(Rbac rbac)
        {
            this.RbacId = rbac.RbacId;
            this.Name = rbac.Name;
            this.Description = rbac.Description;
            this.ConnectionString = rbac.ConnectionString;
            this.MetaDataRbac = rbac.MetaDataRbac;
            this.MetaDataEntitlements = rbac.MetaDataEntitlements;
        }

        public int RbacId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }
        public string MetaDataRbac { get; set; }
        public string MetaDataEntitlements { get; set; }
    }
}
