using System;
using System.Data;
using Eyedia.Aarbac.Framework;

namespace Eyedia.Aarbac.Command
{
    public class AarbacSamples
    {
        public void BookStoreTestOne()
        {
            new BookStore().TestOne();
        }

        public void BookStoreTestBatch()
        {
            new BookStore().TestBatch();
        }

        public DataTable RealWorldSelect(string query = null)
        {
            if (query == null)
            {
                query = "select a.AuthorId, a.Name as [AuthorName], a.ZipCodeId, c.Name as City from Author a ";
                query += "inner join Zipcode zc on zc.ZipCodeId = a.ZipCodeId ";
                query += "inner join City c on c.CityId = zc.CityId ";
                query += "where c.Name = 'Charlotte'";
            }
            using (Rbac rbac = new Rbac("essie"))   //<-- you should pass the logged in user name from the context
            {
                using (RbacSqlQueryEngine engine = new RbacSqlQueryEngine(rbac, query))
                {
                    engine.Execute(); //<-- automatically parse and transform query based on role
                    if ((!engine.IsErrored) && (engine.Parser.IsParsed) && (engine.Parser.QueryType == RbacQueryTypes.Select))
                        return engine.Table; //<-- if it is select query, the table will be loaded 
                }
            }
            return null;
        }

        public void RealWorldInsertOrUpdateOrDelete(string query = null)
        {
            if (query == null)
                query = "update [author] set [name] = 'An Author' where authorId = 1";

            try
            {
                IsAllowedToInsertOrUpdateOrDelete(query);
            }
            catch(RbacException ex)
            {
                Console.WriteLine("You are good!!! User 'essie' does not have permission to insert. Aarbac is working!");
                Console.WriteLine(ex.Message);
            }

            //perform your regular insert/update/delete here            
        }
        private void IsAllowedToInsertOrUpdateOrDelete(string query = null)
        { 
            using (Rbac rbac = new Rbac("essie"))   //<-- you should pass the logged in user name from the context
            {
                using (SqlQueryParser parser = new SqlQueryParser(rbac))
                {
                    parser.Parse(query); //<-- this will throw exception if not permitted                    
                    //<-- if you are here, you are goood. Just perform basic insert/update/delete
                }
            }
        }
        
    }
}
