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
 DISCLAIMED. IN NO EVENT SHALL Synechron Holdings Inc BE LIABLE FOR ANY
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
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    /*
    public partial class SqlQueryParser
    {
        private void ParseJoins(QualifiedJoin aQualifiedJoin, RbacJoin ajoin = null)
        {
            if (aQualifiedJoin.FirstTableReference is QualifiedJoin)
            {
                ParseJoins((QualifiedJoin)aQualifiedJoin.FirstTableReference);
            }
            else if (aQualifiedJoin.FirstTableReference is NamedTableReference)
            {
                ParseJoin((QualifiedJoin)aQualifiedJoin);
            }

            if (aQualifiedJoin.SecondTableReference is QualifiedJoin)
            {
                ParseJoins((QualifiedJoin)aQualifiedJoin.SecondTableReference);
            }
            else if (aQualifiedJoin.SecondTableReference is NamedTableReference)
            {
                ParseJoin((QualifiedJoin)aQualifiedJoin);
            }

        }

        private void ParseJoin(QualifiedJoin aQualifiedJoin)
        {

            RbacJoin ajoin = new RbacJoin();
            ajoin.JoinType = (RbacJoinTypes)Enum.Parse(typeof(RbacJoinTypes), aQualifiedJoin.QualifiedJoinType.ToString(), true);

            if (aQualifiedJoin.FirstTableReference is NamedTableReference)
            {
                NamedTableReference namedTableReference = (NamedTableReference)aQualifiedJoin.FirstTableReference;
                ajoin.FromTableName = namedTableReference.SchemaObject.BaseIdentifier.Value;
                ajoin.FromTableAlias = namedTableReference.Alias.Value;
            }
            if (aQualifiedJoin.SecondTableReference is NamedTableReference)
            {
                NamedTableReference namedTableReference = (NamedTableReference)aQualifiedJoin.SecondTableReference;
                ajoin.WithTableName = namedTableReference.SchemaObject.BaseIdentifier.Value;
                ajoin.WithTableAlias = namedTableReference.Alias != null? namedTableReference.Alias.Value:string.Empty;
            }

            if (aQualifiedJoin.SearchCondition is BooleanComparisonExpression)
            {
                BooleanComparisonExpression boolComparison = (BooleanComparisonExpression)aQualifiedJoin.SearchCondition;
                if (boolComparison.FirstExpression is ColumnReferenceExpression)
                {
                    ColumnReferenceExpression columnComparison = (ColumnReferenceExpression)boolComparison.FirstExpression;
                    ajoin.WithTableAlias = columnComparison.MultiPartIdentifier.Identifiers[0].Value;
                    ajoin.WithTableColumn = columnComparison.MultiPartIdentifier.Identifiers[1].Value;
                }

                if (boolComparison.SecondExpression is ColumnReferenceExpression)
                {
                    ColumnReferenceExpression columnComparison = (ColumnReferenceExpression)boolComparison.SecondExpression;
                    ajoin.FromTableAlias = columnComparison.MultiPartIdentifier.Identifiers[0].Value;
                    ajoin.FromTableColumn = columnComparison.MultiPartIdentifier.Identifiers[1].Value;
                }
            }

            JoinClauses.Add(ajoin);
        }
    }*/
}

