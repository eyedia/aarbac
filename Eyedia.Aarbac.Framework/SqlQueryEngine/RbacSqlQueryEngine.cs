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
        public SqlQueryParser SqlQueryParser { get;}
        public bool IsExecuted { get; private set; }
        public bool IsErrored { get; private set; }
        public bool IsDebugMode { get; private set; }
        public bool SkipExecution { get; set; }
        public List<string> Errors { get; set; }
        public DataTable Table { get; private set; }

        public RbacSqlQueryEngine(SqlQueryParser sqlQueryParser, bool isDebugMode = false)
        {
            this.Errors = new List<string>();
            this.SqlQueryParser = sqlQueryParser;
            IsDebugMode = isDebugMode;
            SqlQueryParser.Context.Trace.WriteLine("Engine:{0}", this.GetType().Name);
        }
        public RbacSqlQueryEngine(Rbac context, string query, bool isDebugMode = false)
        {
            this.Errors = new List<string>();
            SqlQueryParser = new SqlQueryParser(context);
            SqlQueryParser.Parse(query);
            IsDebugMode = isDebugMode;
            SqlQueryParser.Context.Trace.WriteLine("Engine:{0}", this.GetType().Name);
        }
        public void Execute()
        {
            Table = new DataTable();
            if (SqlQueryParser.IsNotSupported)
            {
                SqlQueryParser.Context.Trace.WriteLine("Cannot execute query! Query parser is in error state as something was not allowed.");
                return;
            }
            else if ((!SqlQueryParser.IsParsingSkipped) && (!SqlQueryParser.IsParsed))
            {
                SqlQueryParser.Context.Trace.WriteLine("Cannot execute query! Query was not parsed.");
                return;
            }
            else if(SqlQueryParser.IsZeroSelectColumn)
            {
                SqlQueryParser.Context.Trace.WriteLine("Cannot execute query! Nothing to be selected, the select query has zero column.");
                return;
            }

            if (SkipExecution)
                return;

            SqlQueryParser.Context.Trace.WriteLine("Executing query...");
            string query = LimitNumberOfRows(SqlQueryParser.ParsedQuery);

            try
            {
                using (SqlConnection connection = new SqlConnection(SqlQueryParser.Context.ConnectionString))
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
            SqlQueryParser.Context.Trace.Write("{0} records returned.", Table.Rows.Count);            
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
