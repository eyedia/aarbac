using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    
    internal class NamedTableReferenceVisitor : TSqlFragmentVisitor
    {
        public List<ReferredTable> Tables { get; private set; }


        public NamedTableReferenceVisitor()
        {
            Tables = new List<ReferredTable>();
        }

        public override void ExplicitVisit(NamedTableReference node)
        {
            ReferredTable table = new ReferredTable();
            table.Server = node.SchemaObject.ServerIdentifier != null ? node.SchemaObject.ServerIdentifier.Value : string.Empty;
            table.Database = node.SchemaObject.DatabaseIdentifier != null ? node.SchemaObject.DatabaseIdentifier.Value : string.Empty;
            table.Schema = node.SchemaObject.SchemaIdentifier != null ? node.SchemaObject.SchemaIdentifier.Value : string.Empty;
            table.Name = node.SchemaObject.BaseIdentifier != null ? node.SchemaObject.BaseIdentifier.Value : string.Empty;
            table.Alias = node.Alias != null ? node.Alias.Value : string.Empty;
            Tables.Add(table);
        }
    }
}
