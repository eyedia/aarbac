using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser
    {
        private void ParseTableReferences(TableReference aTableReference)
        {
            if (aTableReference.GetType() == typeof(NamedTableReference))
            {
                NamedTableReference aNamedTableReference = (NamedTableReference)aTableReference;
                Identifier aAliasIdentifier = aNamedTableReference.Alias;
                SchemaObjectName aSchemaObjectName = aNamedTableReference.SchemaObject;
                Columns.AddTableReference(aSchemaObjectName, aAliasIdentifier);
            }

            if (aTableReference.GetType() == typeof(QualifiedJoin))
            {
                QualifiedJoin aQualifiedJoin = (QualifiedJoin)aTableReference;                                     
                ParseTableReferences(aQualifiedJoin.FirstTableReference);
                ParseTableReferences(aQualifiedJoin.SecondTableReference);
            }
            if (aTableReference.GetType() == typeof(JoinTableReference))
            {
                JoinTableReference aJoinTableReference = (JoinTableReference)aTableReference;
                ParseTableReferences(aJoinTableReference.FirstTableReference);
                ParseTableReferences(aJoinTableReference.SecondTableReference);
            }
        }
       
    }
}
