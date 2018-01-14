#region Copyright Notice
/* Copyright (c) 2017, Deb'jyoti Das - debjyoti@debjyoti.com
 All rights reserved.
 Redistribution and use in source and binary forms, with or without
 modification, are not permitted.Neither the name of the 
 'Deb'jyoti Das' nor the names of its contributors may be used 
 to endorse or promote products derived from this software without 
 specific prior written permission.
 THIS SOFTWARE IS PROVIDED BY Deb'jyoti Das 'AS IS' AND ANY
 EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
 DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#region Developer Information
/*
Author  - Debjyoti Das (debjyoti@debjyoti.com)
Created - 11/12/2017 3:31:31 PM
Description  - 
Modified By - 
Description  - 
*/
#endregion Developer Information

#endregion Copyright Notice
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

