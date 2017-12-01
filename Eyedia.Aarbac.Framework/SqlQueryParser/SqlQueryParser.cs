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
            WhereClauses = new List<RbacWhereClause>();
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
        public List<RbacWhereClause> WhereClauses { get; private set; }
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

                if (QueryType == RbacQueryTypes.Select)
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
           

            foreach (RbacCondition condition in table.Conditions)
            {
                //if this condition's column name already exists in the original query, we need to remove that condition from original query
                IfSameColumnConditionExistsRemoveCondition(selfName, condition);

                string thisItemWhereClause = condition.WhereClause.Replace("__self__", selfName);                

                SqlQueryStringParser.SqlStringParser strParser = new SqlQueryStringParser.SqlStringParser();
                strParser.Parse(ParsedQuery);

                string originalWhereClause = strParser.WhereClause;
                if (string.IsNullOrEmpty(originalWhereClause))
                    strParser.WhereClause = thisItemWhereClause;
                else
                    strParser.WhereClause = string.Format("({0}) AND ({1})", originalWhereClause, thisItemWhereClause);

                ParsedQuery = strParser.ToTextAndFixSpaces();
            }

            //if (!string.IsNullOrEmpty(table.OrderBy))
            //{
            //    string orginalOrderByClause = strParser.OrderByClause;

            //    if (string.IsNullOrEmpty(orginalOrderByClause))
            //        strParser.OrderByClause = table.OrderBy;
            //    else
            //        strParser.OrderByClause = string.Format("{0}, {1}", orginalOrderByClause, OrderbyClause.Text);
            //}

            ConditionAppliedOn.Add(table.Name);
            
        }

        private void IfSameColumnConditionExistsRemoveCondition(string tableOrAliasName, RbacCondition condition)
        {
            bool somethingReplaced = false;
            foreach (string column in condition.Columns)
            {
                RbacWhereClause aWhereClause = WhereClauses.Find(tableOrAliasName, column);
                if (aWhereClause != null)
                {
                    //this column is referred as condition in original query
                    ParsedQuery = ParsedQuery.Replace(aWhereClause.WhereClauseString, string.Empty);
                    somethingReplaced = true;
                }
            }
            if(somethingReplaced)
            {                
                ParsedQuery = ParsedQuery.TrimEnd();
                string[] words = new string[] { "and", "where" };

                foreach (string word in words)
                {
                    if ((ParsedQuery.Length > word.Length) 
                        && (ParsedQuery.Substring(ParsedQuery.Length - word.Length, word.Length).Equals(word, StringComparison.OrdinalIgnoreCase)))
                        ParsedQuery = ParsedQuery.Remove(ParsedQuery.Length - word.Length, word.Length);
                }
            }
        }
        

        List<string> ConditionAppliedOn = new List<string>();

        private void ApplyConditions()
        {
            ConditionAppliedOn.Clear();
            List<RbacTable> tablesWithCondition = TablesReferred.Where(tr => tr.Conditions.Count > 0).ToList();
            foreach (RbacTable table in tablesWithCondition)
            {
                ApplyCondition(table);
            }
        }



        #endregion Conditions

        #region Relations
        
        
        private void ApplyConditionsRelational()
        {
            //lets add all joins
            List<RbacTable> tables = new List<RbacTable>(TablesReferred);
            foreach (RbacTable table in tables)
            {
                AddRelationalJoin(table);
            }
           
        }

        private void AddRelationalJoin(RbacTable table)
        {
            if (table.Relations.Count != 0)
            {
                foreach (RbacRelation relation in table.Relations)
                {
                    RbacTable withTable = Context.User.Role.CrudPermissions.Find(relation.WithTable);
                    if (withTable != null)
                    {
                        AddRelationalJoins(relation, withTable.Conditions);
                    }

                    AddRelationalJoin(withTable);
                }
            }
            else
            {
                //we have reached end of relation (linked list), there is no further relations ahead of us, time to cleanup
                //let's see if we need those joins, if not let's remove those.
                List<RbacTable> tables = new List<RbacTable>(TablesReferred);
                tables = tables.Where(t => t.ReferencedOnly).ToList();
                tables.Reverse();
                foreach (RbacTable t in tables)
                {
                    if (t.Conditions.Count == 0)
                    {
                        var joinTobeRemoved = JoinClauses.Where(jc => jc.WithTableName == t.Name).SingleOrDefault();
                        JoinClauses.Remove(joinTobeRemoved);
                    }
                    else
                    {
                        //oops while reversing back we found a condition, let's stop here!
                        break;
                    }
                }
            }
        }

        private void AddRelationalJoins(RbacRelation relation, List<RbacCondition> conditions)
        {
            //was relation already referred in the query?
            RbacTable relationTable = TablesReferred.Find(relation.WithTable);

            if (relationTable == null)
            {
                //NO:
                //Is there already join with that table?
                RbacJoin join = JoinClauses.JoinExists(relation.WithTable, relation.SelfName);
                if (join == null)
                {
                    //add new join, at the end of operation we will stringify sequentially
                    join = RbacJoin.AddNewJoin(this,
                        relation.SelfName, relation.SelfColumnName,
                        relation.WithTable, relation.WithColumn);

                }

                //add into referred table as 'ReferencedOnly'
                relationTable = new RbacTable(string.Empty, relation.WithTable, false);
                relationTable.Conditions.AddRange(conditions);
                relationTable.ReferencedOnly = true;
                relationTable.TempAlias = join.WithTableAlias;
                TablesReferred.Add(relationTable);

            }
            //add condition
            ApplyCondition(relationTable);


        }
        #region Commented
        /*
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
                    //if (withTable.Conditions.Count > 0)
                    //{
                        if (!relationConditions.ContainsKey(relation))
                        {
                            relationConditions.Add(relation, new List<RbacCondition>(withTable.Conditions));
                        }
                    //}
                }
                GetRelationCondition(withTable, relationConditions);
            }
        }
        */
        #endregion Commented
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

