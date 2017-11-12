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
                user.AddParameter("{CITYNAME}", "('New York')");
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
        public static void TestOne()
        {
            DataTable table = null;
            using (Rbac rbac = new Rbac("essie"))
            {
                string query = "select * from book";
                using (RbacSqlQueryEngine engine = new RbacSqlQueryEngine(rbac, query))
                {
                    engine.Execute();
                    if ((!engine.IsErrored) && (engine.SqlQueryParser.IsParsed) && (engine.SqlQueryParser.QueryType == RbacQueryTypes.Select))
                        table = engine.Table; //--> gives you data table if it is a select query
                }
            }
            if (table != null)
                Console.WriteLine("The query was a select query and returned {0} records", table.Rows.Count);
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
