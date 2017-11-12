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
        private void ParseScalarExperssions(int aSelectElementID, ScalarExpression aScalarExpression)
        {
            if (aScalarExpression.GetType() == typeof(ColumnReferenceExpression))
            {
                ColumnReferenceExpression aColumnReferenceExpression = (ColumnReferenceExpression)aScalarExpression;                
                MultiPartIdentifier aMultiPartIdentifier = aColumnReferenceExpression.MultiPartIdentifier;
                MultiPartIdentifierToString(aSelectElementID, aMultiPartIdentifier);
            }
            else if (aScalarExpression.GetType() == typeof(ConvertCall))
            {
                ConvertCall aConvertCall = (ConvertCall)aScalarExpression;
                ScalarExpression aScalarExpressionParameter = aConvertCall.Parameter;
                ParseScalarExperssions(aSelectElementID, aScalarExpressionParameter);
            }
            else
            {
                Errors.Add(String.Format("Not supported Expression:{0}", aScalarExpression.GetType()));
            }
        }

        private string MultiPartIdentifierToString(int aSelectElementID, MultiPartIdentifier aMultiPartIdentifier)
        {
            string res = aMultiPartIdentifier.Identifiers.Select(i => i.Value).Aggregate((i, j) => i + "." + j);
        
            Columns.AddRefereceIdentifier(aSelectElementID, aMultiPartIdentifier);
            return res;
        }
    }
}
