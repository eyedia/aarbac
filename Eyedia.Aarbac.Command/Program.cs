using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyedia.Aarbac.Framework;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Configuration;
using GenericParsing;

namespace Eyedia.Aarbac.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            SetDataDirectory();
            BookStore.Setup();
            BookStore.TestBatch();
            return;
        }

        private static void SetDataDirectory()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Eyedia.Aarbac.Framework\Databases");
            var fullPath = System.IO.Path.GetFullPath(path);
            AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
        }


        #region GetCityId
        private static void GetCityId(string connStr)
        {
            DataTable t = new GenericParserAdapter("c:\\temp\\sc.csv").GetDataTable();

            DataTable newT = new DataTable();        
            newT.Columns.Add("ZipCode");
            newT.Columns.Add("CityId");

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                foreach (DataRow r in t.Rows)
                {
                    DataRow newr = newT.NewRow();                    
                    SqlCommand tCommand = new SqlCommand(string.Format("select CityId from City where Name = '{0}' and StateId = 41", r["City"]), connection);
                    SqlDataReader tReader = tCommand.ExecuteReader();
                    tReader.Read();
                    newr[0] = r[0];
                    newr[1] = tReader[0];
                    tReader.Close();
                    newT.Rows.Add(newr);
                }
                connection.Close();
            }

            //newT.ToCsv("c:\\temp\\zipcode.csv");
        }
        #endregion GetCityId

    }
}
