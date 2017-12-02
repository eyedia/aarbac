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
        public List<RbacTable> Tables { get; private set; }
        public Rbac Context { get; private set; }

        public NamedTableReferenceVisitor(Rbac context)
        {
            Context = context;
            Tables = new List<RbacTable>();
        }

        public override void ExplicitVisit(NamedTableReference node)
        {                       
            string tableName = node.SchemaObject.BaseIdentifier != null ? node.SchemaObject.BaseIdentifier.Value : string.Empty;
            string tableAlias = node.Alias != null ? node.Alias.Value : string.Empty;

            RbacTable table = Context.User.Role.CrudPermissions.Find(tableName);
            if(table == null)
                table = Context.User.Role.CrudPermissions.Find(tableAlias);
            if(table == null)
                RbacException.Raise(string.Format("The referred table {0} was not found in meta data!", tableName),
                   RbacExceptionCategories.Parser);

            table.Alias = tableAlias;
            table.Server = node.SchemaObject.ServerIdentifier != null ? node.SchemaObject.ServerIdentifier.Value : string.Empty;
            table.Database = node.SchemaObject.DatabaseIdentifier != null ? node.SchemaObject.DatabaseIdentifier.Value : string.Empty;
            table.Schema = node.SchemaObject.SchemaIdentifier != null ? node.SchemaObject.SchemaIdentifier.Value : string.Empty;            
            Tables.Add(table);
        }
    }
}
