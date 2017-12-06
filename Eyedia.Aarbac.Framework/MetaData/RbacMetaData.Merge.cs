using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
    public partial class RbacMetaData
    {
        public static string Merge(string connectionString, string existingRbacMetaData)
        {
            //generate fresh xml
            string freshMetaXml = Generate(connectionString);
            List<RbacTable> freshPermissions = ReadPermissions(freshMetaXml);
            List<RbacTable> existingPermissions = ReadPermissions(existingRbacMetaData);
            return Merge(freshPermissions, existingPermissions);

        }

        public static string Merge(string connectionString, List<RbacTable> existingPermissions)
        {
            //generate fresh xml
            string freshMetaXml = Generate(connectionString);
            List<RbacTable> freshPermissions = ReadPermissions(freshMetaXml);            
            return Merge(freshPermissions, existingPermissions);
            
        }

        private static string Merge(List<RbacTable> freshPermissions, List<RbacTable> existingPermissions)
        {
            foreach(RbacTable table in freshPermissions)
            {
                RbacTable existingTable = existingPermissions.Find(table.Name);
                if(existingTable != null)
                {                    
                    table.Conditions = existingTable.Conditions;
                    table.Parameters = existingTable.Parameters;
                }
            }
            return ToXml(freshPermissions);            
        }
    }
}
