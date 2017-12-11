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
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Configuration;
using GenericParsing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Eyedia.Aarbac.Command
{
    class Program
    {
        static void Main(string[] args)
        {
            SetDataDirectory();            
           

            

            //ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

            //// the following call to Validate succeeds.
            //document.Validate(eventHandler);

           
            
            //string query = File.ReadAllText(Path.Combine(@"..\..\..\Eyedia.Aarbac.Command\Samples", "Books", "Query.txt"));
            //string sub = query.Substring(186, 21);
            new BookStore().TestBatch();
            //try
            //{
            //new BookStore().TestBatch();
            //}
            //catch(RbacException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //TestSamples();

            return;
            CommandLineCommands.Do(args);
            
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }

        }

        private static bool SetDataDirectory()
        {
            string codingdir = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            var path = codingdir.Substring(0, codingdir.LastIndexOf("\\")) + @"\Eyedia.Aarbac.Command\Samples\Databases";
            if (!Directory.Exists(path))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "Samples");

            if (!Directory.Exists(path))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases");

            if (!Directory.Exists(path))
                path = Path.Combine(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName, "App_Data");

            //download zip folder
            if (!Directory.Exists(path))
                path = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName,
                    "content", "Samples", "Databases");


          
            var fullPath = Path.GetFullPath(path);
            AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
            return true;
        }

    }
}

