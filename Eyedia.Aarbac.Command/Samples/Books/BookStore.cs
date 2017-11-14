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
using Eyedia.Aarbac.Framework;
using System.Data;
using System.IO;
using GenericParsing;

namespace Eyedia.Aarbac.Command
{
    public class BookStore
    {
        const string __rootDir = @"..\..\..\Eyedia.Aarbac.Command\Samples";
        public static void Setup()
        {
            Rbac rbac = new Rbac();
            rbac = rbac.CreateNew("books", "books description",
                @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|books.mdf;Initial Catalog=books;Integrated Security=True",
                File.ReadAllText(Path.Combine(__rootDir,"Books","entitlement.xml")));
                       
            InsertRoles(rbac);
            Random rnd = new Random();

            GenericParserAdapter parser = new GenericParserAdapter(Path.Combine(__rootDir, "Users.csv"));
            parser.FirstRowHasHeader = true;
            DataTable table = parser.GetDataTable();
            foreach (DataRow r in table.Rows)
            {
                RbacUser user = Rbac.CreateUser(r[0].ToString(), r[1].ToString(), r[2].ToString(), "password", roles[rnd.Next(0, roles.Count -1)]);
                user.AddParameter("{CITYNAMES}", "('Charlotte','Raleigh')");
                user.AddParameter("{ZIPCODES}", "('28105')");
            }
        }
        static List<RbacRole> roles = new List<RbacRole>();
        private static void InsertRoles(Rbac rbac)
        {
            string roleMetaData = File.ReadAllText(Path.Combine(__rootDir, "Books", "role.xml"));
            for (int i = 0; i <= 3; i++)
            {
                RbacRole role = rbac.CreateRole("Country Manager "  + i, "Country manager description", roleMetaData,
                    File.ReadAllText(Path.Combine(__rootDir, "Books", "entitlement.xml")));
                roles.Add(role);
            }
        }
        public static RbacSqlQueryEngine TestOne(string query = null)
        {
            RbacSqlQueryEngine engine = null;
            using (Rbac rbac = new Rbac("essie"))
            {
                if (string.IsNullOrEmpty(query))
                    query = File.ReadAllText(Path.Combine(__rootDir, "Books", "Query.txt"));
                engine = new RbacSqlQueryEngine(rbac, query);
                engine.Execute();
                //if ((!engine.IsErrored) && (engine.SqlQueryParser.IsParsed) && (engine.SqlQueryParser.QueryType == RbacQueryTypes.Select))
                //    table = engine.Table; //--> gives you data table if it is a select query

            }
            if (engine.Table != null)
                Console.WriteLine("The query was a select query and returned {0} records", engine.Table.Rows.Count);

            File.WriteAllText(Path.Combine(__rootDir, "Books", "ParsedQuery.txt"), engine.Parser.ParsedQuery);
            return engine;
        }
        public static void TestBatch()
        {
            GenericParserAdapter genParser = new GenericParserAdapter(Path.Combine(__rootDir, "Books", "tests.csv"));
            genParser.FirstRowHasHeader = true;
            DataTable table = genParser.GetDataTable();

            Rbac rbac = new Rbac("essie");
            foreach (DataRow row in table.Rows)
            {                
                RbacRole role = Rbac.GetRole(row[2].ToString());                
                SqlQueryParser parser = new SqlQueryParser(rbac);
                parser.Parse(row[0].ToString());
                RbacSqlQueryEngine engine = new RbacSqlQueryEngine(parser, true);
                engine.Execute();
                row["ParsedQueryStage1"] = parser.ParsedQueryStage1;
                row["ParsedQueryStage2"] = parser.ParsedQuery;
                row["Errors"] = parser.AllErrors + Environment.NewLine;
            }
            table.ToCsv(Path.Combine(__rootDir, "Books", "tests_result.csv"));
        }
    }
}

