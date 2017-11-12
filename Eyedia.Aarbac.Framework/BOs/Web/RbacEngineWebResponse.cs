using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacEngineWebResponse
    {
        //base
        public string RbacName { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public bool RequestProcessed { get; set; }
        public List<string> Errors { get; set; }

        //parser specific
        public string QueryType { get; private set; }
        public string Log { get; private set; }
        public RbacSelectColumns Columns { get; private set; }
        public IList<ParseError> ParseErrors { get; private set; }        
        public bool IsParsed { get; private set; }
        public bool IsNotSupported { get; private set; }
        public bool IsZeroSelectColumn { get; private set; }
        public bool IsPermissionApplied { get; private set; }
        public bool IsParsingSkipped { get; private set; }
        public string OriginalQuery { get; private set; }
        public string ParsedQuery { get; private set; }
        public string ParsedQueryStage1 { get; private set; }
        public string ParsedMethod { get; set; }


        //engine specific
        public bool IsEngineExecuted { get; private set; }
        public bool IsEngineDebugMode { get; private set; }
        public DataTable Table { get; private set; }

        public RbacEngineWebResponse()
        {            
            Errors = new List<string>();
        }

        public RbacEngineWebResponse(RbacSqlQueryEngine engine)
        {
            Errors = new List<string>();
            SetResult(engine);
        }

        public void SetResult(string errorMessage)
        {
            Errors.Add(errorMessage);
        }

        public void SetResult(RbacSqlQueryEngine engine)
        {
            QueryType = engine.SqlQueryParser.QueryType.ToString();
            Log = engine.SqlQueryParser.AllErrors;
            Columns = engine.SqlQueryParser.Columns;
            ParseErrors = engine.SqlQueryParser.ParseErrors;
            IsParsed = engine.SqlQueryParser.IsParsed;
            IsNotSupported = engine.SqlQueryParser.IsNotSupported;
            IsZeroSelectColumn = engine.SqlQueryParser.IsZeroSelectColumn;
            IsPermissionApplied = engine.SqlQueryParser.IsPermissionApplied;
            IsParsingSkipped = engine.SqlQueryParser.IsParsingSkipped;
            OriginalQuery = engine.SqlQueryParser.OriginalQuery;
            ParsedQuery = engine.SqlQueryParser.ParsedQuery;
            ParsedQueryStage1 = engine.SqlQueryParser.ParsedQueryStage1;
            ParsedMethod = engine.SqlQueryParser.ParsedMethod.ToString();
            QueryType = engine.SqlQueryParser.QueryType.ToString();

            IsEngineExecuted = engine.IsExecuted;
            IsEngineDebugMode = engine.IsDebugMode;
            Table = engine.Table;            
        }
       
    }
}
