using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacException:Exception
    {
        public RbacExceptionCategories ExceptionCategory { get; }

        private RbacException(string message) : base(message)
        {
            
        }

        public static void Raise(string message, RbacExceptionCategories exceptionCategory = RbacExceptionCategories.Core)
        {
            throw new RbacException(Format(message, exceptionCategory));
        }

        private static string Format(string message, RbacExceptionCategories exceptionCategory)
        {
            string prefix = string.Empty;
            switch(exceptionCategory)
            {
                case RbacExceptionCategories.Configuration:
                    prefix = "RBAC.CNF";
                    break;

                case RbacExceptionCategories.Core:
                    prefix = "RBAC.Core";
                    break;

                case RbacExceptionCategories.Parser:
                    prefix = "RBAC.PRS";
                    break;

                case RbacExceptionCategories.QueryEngine:
                    prefix = "RBAC.ENG";
                    break;

                case RbacExceptionCategories.Repository:
                    prefix = "RBAC.REPO";
                    break;

                default:
                    prefix = "RBAC";
                    break;
            }
            return string.Format("{0} - {1}", prefix, message);
        }
    }
}
