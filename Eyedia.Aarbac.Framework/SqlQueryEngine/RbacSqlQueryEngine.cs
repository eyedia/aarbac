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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class RbacSqlQueryEngine : IDisposable
    {
        public SqlQueryParser Parser { get;}
        public bool IsExecuted { get; private set; }
        public bool IsErrored { get; private set; }
        public bool IsDebugMode { get; private set; }
        public bool SkipExecution { get; set; }
        public List<string> Errors { get; set; }
        public DataTable Table { get; private set; }

        public RbacSqlQueryEngine(SqlQueryParser sqlQueryParser, bool isDebugMode = false)
        {
            this.Errors = new List<string>();
            this.Parser = sqlQueryParser;
            IsDebugMode = isDebugMode;
            Parser.Context.Trace.WriteLine("Engine:{0}", this.GetType().Name);
        }
        public RbacSqlQueryEngine(Rbac context, string query, bool isDebugMode = false)
        {
            this.Errors = new List<string>();
            Parser = new SqlQueryParser(context);
            Parser.Parse(query);
            IsDebugMode = isDebugMode;
            Parser.Context.Trace.WriteLine("Engine:{0}", this.GetType().Name);
        }
        public void Execute()
        {
            Table = new DataTable();
            if (Parser.IsNotSupported)
            {
                Parser.Context.Trace.WriteLine("Cannot execute query! Query parser is in error state as something was not allowed.");
                return;
            }
            else if ((!Parser.IsParsingSkipped) && (!Parser.IsParsed))
            {
                Parser.Context.Trace.WriteLine("Cannot execute query! Query was not parsed.");
                return;
            }
            else if(Parser.IsZeroSelectColumn)
            {
                Parser.Context.Trace.WriteLine("Cannot execute query! Nothing to be selected, the select query has zero column.");
                return;
            }

            if (SkipExecution)
                return;

            Parser.Context.Trace.WriteLine("Executing query...");
            string query = LimitNumberOfRows(Parser.ParsedQuery);

            try
            {
                using (SqlConnection connection = new SqlConnection(Parser.Context.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    Table.Load(command.ExecuteReader());
                    IsExecuted = true;
                }
            }
            catch(Exception ex)
            {
                IsErrored = true;
                this.Errors.Add(ex.Message);
            }
            Parser.Context.Trace.Write("{0} records returned.", Table.Rows.Count);            
        }

        private string LimitNumberOfRows(string parsedQuery)
        {
            if(IsDebugMode)
            {
                string selectTop10 = parsedQuery.Substring(0, 6) + " top 10";
                return selectTop10 + parsedQuery.Substring(6, parsedQuery.Length - 6);
            }
            else
            {
                return parsedQuery;
            }
        }

        public void Dispose()
        {

        }
    }
}

