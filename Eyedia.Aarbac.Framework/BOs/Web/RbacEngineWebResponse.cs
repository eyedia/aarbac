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
        public string Errors { get; set; }

        //parser specific
        public string QueryType { get; private set; }
        public string Log { get; private set; }
        public List<RbacSelectColumn> Columns { get; private set; }              
        public bool IsParsed { get; private set; }
        public bool IsNotSupported { get; private set; }
        public bool IsZeroSelectColumn { get; private set; }
        public bool IsPermissionApplied { get; private set; }
        public bool IsParsingSkipped { get; private set; }
        public string OriginalQuery { get; private set; }
        public string ParsedQuery { get; private set; }
        public string ParsedQueryStage1 { get; private set; }
        public string ParsedMethod { get; set; }
        public string ExecutionTime { get; set; }

        //engine specific
        public bool IsEngineExecuted { get; private set; }
        public bool IsEngineDebugMode { get; private set; }
        public DataTable Table { get; set; }

        public RbacEngineWebResponse()
        {
            Columns = new List<RbacSelectColumn>();
        }

        public RbacEngineWebResponse(RbacSqlQueryEngine engine)
        {            
            SetResult(engine);
        }

        public void SetResult(string errorMessage)
        {
            Errors = errorMessage;
        }

        public void SetResult(SqlQueryParser parser)
        {
            QueryType = parser.QueryType.ToString();
            Log = parser.AllErrors;
            Columns = parser.Columns;
            Errors = parser.AllErrors;
            IsParsed = parser.IsParsed;
            IsNotSupported = parser.IsNotSupported;
            IsZeroSelectColumn = parser.IsZeroSelectColumn;
            IsPermissionApplied = parser.IsPermissionApplied;
            IsParsingSkipped = parser.IsParsingSkipped;
            OriginalQuery = parser.OriginalQuery;
            ParsedQuery = parser.ParsedQuery;
            ParsedQueryStage1 = parser.ParsedQueryStage1;
            ParsedMethod = parser.ParsedMethod.ToString();
            QueryType = parser.QueryType.ToString();
            ExecutionTime = parser.ExecutionTime.TotalTimeFormatted;

            IsEngineExecuted = false;
            IsEngineDebugMode = false;
            Table = null;
        }

        public void SetResult(RbacSqlQueryEngine engine)
        {
            QueryType = engine.Parser.QueryType.ToString();
            Log = engine.Parser.AllErrors + engine.AllErrors;
            Columns = engine.Parser.Columns;
            Errors = engine.Parser.AllErrors + engine.AllErrors;
            IsParsed = engine.Parser.IsParsed;
            IsNotSupported = engine.Parser.IsNotSupported;
            IsZeroSelectColumn = engine.Parser.IsZeroSelectColumn;
            IsPermissionApplied = engine.Parser.IsPermissionApplied;
            IsParsingSkipped = engine.Parser.IsParsingSkipped;
            OriginalQuery = engine.Parser.OriginalQuery;
            ParsedQuery = engine.Parser.ParsedQuery;
            ParsedQueryStage1 = engine.Parser.ParsedQueryStage1;
            ParsedMethod = engine.Parser.ParsedMethod.ToString();
            QueryType = engine.Parser.QueryType.ToString();
            ExecutionTime = engine.Parser.ExecutionTime.TotalTimeFormatted;

            IsEngineExecuted = engine.IsExecuted;
            IsEngineDebugMode = engine.IsDebugMode;
            Table = engine.Table;
        }
       
    }
}

