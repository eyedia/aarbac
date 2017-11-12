using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

namespace Eyedia.Aarbac.Framework
{
    public partial class SqlQueryParser : IDisposable
    {
        public SqlQueryParser(Rbac context, bool skipParsing = false, bool silent = false)
        {
            Context = context;
            IsParsingSkipped = skipParsing;
            JoinClauses = new List<RbacJoin>();
            ExecutionTime = new ExecutionTime();
            ParseErrors = new List<ParseError>();
            ParamErrors = new List<string>();
            Errors = new List<string>();
            Warnings = new List<string>();
            IsSilent = silent;
        }

        public RbacQueryTypes QueryType { get; private set; }
        public List<string> Warnings { get; private set; }
        public List<string> Errors { get; private set; }
        public RbacSelectColumns Columns { get; private set; }
        public IList<ParseError> ParseErrors { get; private set; }
        public Rbac Context { get; private set; }        
        public bool IsParsed { get; private set; }
        public bool IsNotSupported { get; private set; }
        public bool IsZeroSelectColumn { get; private set; }
        public bool IsPermissionApplied { get; private set; }
        public bool IsParsingSkipped { get; private set; } 
        public bool SyntaxError { get; private set; }       
        public bool IsSilent { get; set; }
        public string OriginalQuery { get; private set; }
        public string ParsedQuery { get; private set; }
        public string ParsedQueryStage1 { get; private set; }
        public RbacSelectQueryParsedMethods ParsedMethod { get; set; }
        
        public List<RbacTable> TablesReferred { get; private set; }
        public List<RbacJoin> JoinClauses { get; private set; }
        public ExecutionTime ExecutionTime { get; private set;}

        List<string> ParamErrors;
        public string AllErrors
        {
            get
            {
                string allErrors = string.Empty;
                if ((ParseErrors != null) && (ParseErrors.Count > 0))
                    allErrors += ParseErrors.Select(i => i.Message).Aggregate((i, j) => i + Environment.NewLine + j);

                if ((Errors != null) && (Errors.Count > 0))
                    allErrors += Errors.ToLine();

                return allErrors;
            }
        }
        public void Parse(string query)
        {            
            if(IsSilent)
            {
                try
                {
                    ParseQuery(query);
                }
                catch(Exception ex)
                {
                    ParsedQuery = string.Empty;
                    ParsedQueryStage1 = string.Empty;
                    IsParsed = false;
                    Errors.Add(ex.Message);
                }
            }
            else
            {
                ParseQuery(query);
            }
        }

        private void ParseQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
                RbacException.Raise("Cannot parse an empty query!");

            OriginalQuery = query;
            ParsedQuery = query;
            if (!IsParsingSkipped)
            {
                ExecutionTime.Start("Parse Query");               
                ParseQueryType();                
                ParseInternal(query);
                ExecutionTime.Stop("Parse Query");
                if (!IsParsed)
                    return;

                if ((QueryType != RbacQueryTypes.Insert) && (QueryType != RbacQueryTypes.Delete))
                {
                    ExecutionTime.Start("Conditions & Relations");
                    ApplyConditions();
                    ApplyConditionsRelational();
                    ParsedQuery = JoinClauses.ParseQuery(ParsedQuery);
                    ExecutionTime.Stop("Conditions & Relations");
                }           
            }
            else
            {   
                Errors.Add("Parsing skipped!");
            }

            ExecutionTime.Start("Apply Permission");
            ApplyPermission();
            ExecutionTime.Stop("Apply Permission");

            if ((QueryType != RbacQueryTypes.Insert) && (QueryType != RbacQueryTypes.Delete))
            {
                ExecutionTime.Start("Apply Parameters");
                if (!ApplyParameters())
                    RbacException.Raise(ParamErrors.ToLine());
                ExecutionTime.Stop("Apply Parameters");
            }
            WriteLogParseDetails();
            
        }

        private void WriteLogParseDetails()
        {
            Context.Trace.Write(ExecutionTime.ToString());
            Context.Trace.Indent();
            Context.Trace.WriteLine("Original Query:{0}", OriginalQuery);
            Context.Trace.WriteLine();
            Context.Trace.WriteLine("Type:{0}", QueryType);

            if (QueryType == RbacQueryTypes.Select)
            {
                Context.Trace.WriteLine("Columns:");
                Context.Trace.Indent();
                Context.Trace.WriteLine(Columns.ToString());
                Context.Trace.UnIndent();
                //Context.Trace.WriteLine("Where Clause:{0}", HadWhereClause);
            }

            Context.Trace.WriteLine("Parsed Query:{0}", ParsedQuery);
            Context.Trace.UnIndent();
        }        

        #region Conditions
        private void ApplyCondition(RbacTable table)
        {
            if (table.Conditions.Count == 0)
                return;

            if (ConditionAppliedOn.Where(c => c == table.Name).SingleOrDefault() != null)
                return;

            string selfName = string.Empty;
            if (!table.ReferencedOnly)
                selfName = this.GetTableNameOrAlias(table.Name);
            else
                selfName = table.TempAlias;

            string thisItemWhereClause = table.WhereClause.Replace("__self__", selfName);
            SqlQueryStringParser.SqlStringParser strParser = new SqlQueryStringParser.SqlStringParser();
            strParser.Parse(ParsedQuery);


            string originalWhereClause = strParser.WhereClause;
            if (string.IsNullOrEmpty(originalWhereClause))
                strParser.WhereClause = thisItemWhereClause;
            else
                strParser.WhereClause = string.Format("({0}) AND ({1})", originalWhereClause, thisItemWhereClause);

            //if (!string.IsNullOrEmpty(table.OrderBy))
            //{
            //    string orginalOrderByClause = strParser.OrderByClause;

            //    if (string.IsNullOrEmpty(orginalOrderByClause))
            //        strParser.OrderByClause = table.OrderBy;
            //    else
            //        strParser.OrderByClause = string.Format("{0}, {1}", orginalOrderByClause, OrderbyClause.Text);
            //}

            ConditionAppliedOn.Add(table.Name);
            ParsedQuery = strParser.ToTextAndFixSpaces();
        }

        
        List<string> ConditionAppliedOn = new List<string>();

        private void ApplyConditions()
        {
            ConditionAppliedOn.Clear();
            List<RbacTable> tablesWithCondition = TablesReferred.Where(tr => !string.IsNullOrEmpty(tr.WhereClause)).ToList();
            foreach (RbacTable table in tablesWithCondition)
            {
                ApplyCondition(table);
            }
        }

        private void ApplyConditionsRelational()
        {
            Dictionary<RbacRelation, List<RbacCondition>> relationConditions = GetRelationConditions();

            foreach (KeyValuePair<RbacRelation, List<RbacCondition>> relationCondition in relationConditions)
            {
                string joinClause = string.Empty;
                string whereClause = string.Empty;
                foreach (RbacCondition condition in relationCondition.Value)
                {
                    //was relation already referred in the query?
                    RbacTable condnTable = TablesReferred.Find(condition.SelfTableName);

                    if (condnTable == null)
                    {
                        //NO:
                        //Is there already join with that table?
                        RbacJoin join = JoinClauses.JoinExists(relationCondition.Key.WithTable, relationCondition.Key.SelfName);
                        if (join == null)
                        {
                            //add new join, at the end of operation we will stringify sequentially
                            join = RbacJoin.AddNewJoin(this, 
                                relationCondition.Key.SelfName, relationCondition.Key.SelfColumnName,
                                relationCondition.Key.WithTable, relationCondition.Key.WithColumn);
                     
                        }

                        //add into referred table as 'ReferencedOnly'
                        condnTable = new RbacTable(string.Empty, relationCondition.Key.WithTable, false);
                        condnTable.Conditions.Add(condition);
                        condnTable.ReferencedOnly = true;
                        condnTable.TempAlias = join.WithTableAlias;
                        TablesReferred.Add(condnTable);

                    }
                    //add condition
                    ApplyCondition(condnTable);
                }
            }
        }

        #endregion Conditions

        #region Relations
        private Dictionary<RbacRelation, List<RbacCondition>> GetRelationConditions()
        {
            Dictionary<RbacRelation, List<RbacCondition>> relationConditions = new Dictionary<RbacRelation, List<RbacCondition>>();
            foreach (RbacTable table in TablesReferred)
            {
                GetRelationCondition(table, relationConditions);
            }

            return relationConditions;
        }
        private void GetRelationCondition(RbacTable table, Dictionary<RbacRelation, List<RbacCondition>> relationConditions)
        {
            foreach (RbacRelation relation in table.Relations)
            {
                RbacTable withTable = Context.User.Role.CrudPermissions.Find(relation.WithTable);
                if (withTable != null)
                {
                    if (withTable.Conditions.Count > 0)
                    {
                        if (!relationConditions.ContainsKey(relation))
                        {
                            relationConditions.Add(relation, new List<RbacCondition>(withTable.Conditions));
                        }
                    }
                }
                GetRelationCondition(withTable, relationConditions);
            }
        }
        #endregion Relations        

        #region Helpers

        private void GetReferredTables()
        {
            TablesReferred = new List<RbacTable>();
            foreach (RbacSelectColumn cInfo in Columns.List)
            {
                RbacTable table = Context.User.Role.CrudPermissions.Find(cInfo.ReferencedTableName);
                if (table != null)
                    TablesReferred.Add(table);
                else
                    throw new Exception(string.Format("The referred table {0} was not found in meta data!", cInfo.ReferencedTableName));
            }
            TablesReferred = new List<RbacTable>(TablesReferred.DistinctBy(t => t.Name));
        }

        private void PrintErrors()
        {
            foreach (ParseError aParseError in ParseErrors)
            {
                Errors.Add(string.Format("Error:{0} at line nr:{1} column:{2} ", aParseError.Message, aParseError.Line,
                    aParseError.Column));
            }
        }
      
        private void ParseQueryType()
        {
            QueryType = RbacQueryTypes.Unknown;
            if ((OriginalQuery.Length >= 6) && (OriginalQuery.Substring(0, 6).ToLower() == "select"))
                QueryType = RbacQueryTypes.Select;
            else if ((OriginalQuery.Length >= 6) && (OriginalQuery.Substring(0, 6).ToLower() == "insert"))
                QueryType = RbacQueryTypes.Insert;
            else if ((OriginalQuery.Length >= 6) && (OriginalQuery.Substring(0, 6).ToLower() == "update"))
                QueryType = RbacQueryTypes.Update;
            else if ((OriginalQuery.Length >= 6) && (OriginalQuery.Substring(0, 6).ToLower() == "delete"))
                QueryType = RbacQueryTypes.Delete;
            else
                RbacException.Raise("Invalid query type!");
        }


        #endregion Helpers

        
        public void Dispose()
        {            
            GC.SuppressFinalize(this);
        }

    }
}
