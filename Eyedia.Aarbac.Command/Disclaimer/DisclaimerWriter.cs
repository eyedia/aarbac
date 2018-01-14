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
 DISCLAIMED. IN NO EVENT SHALL Debjyoti OR Deb'jyoti OR Debojyoti Das OR Eyedia BE LIABLE FOR ANY
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
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Eyedia.Aarbac.Command
{
    public class DisclaimerWriter
    {
        string _Disclaimer;
        string _SourceCodeLocation;        
        string _filePatterns;
        const string __regionName = "#region Copyright Notice";
        const string __regionNameEnd = "";
        const string __regionNameDeveloper = "#region Developer Information";
        const string __regionNameEndDeveloper = "#endregion Developer Information";

        List<string> _ignoreExtensions;

        public DisclaimerWriter(string sourceCodeLocation)
        {            
            _Disclaimer = __regionName + Environment.NewLine;
            _Disclaimer += GetDisclaimer();
            _Disclaimer += "{0}";
            _Disclaimer += Environment.NewLine + __regionNameEnd;            
            _SourceCodeLocation = sourceCodeLocation;;
            _filePatterns = "*.cs";
            _ignoreExtensions = new List<string>(".designer.cs".Split(",".ToCharArray()));
            
        }
        public string DeveloperRegionBlank
        {
            get
            {
                string strDeveloperRegion = __regionNameDeveloper + Environment.NewLine;
                strDeveloperRegion += "/*" + Environment.NewLine;
                strDeveloperRegion += "Author  - Debjyoti Das (debjyoti@debjyoti.com)" + Environment.NewLine;
                strDeveloperRegion += "Created - "  + DateTime.Now.ToString() + Environment.NewLine;
                strDeveloperRegion += "Description  - " + Environment.NewLine;
                strDeveloperRegion += "Modified By - " + Environment.NewLine;
                strDeveloperRegion += "Description  - " + Environment.NewLine;
                strDeveloperRegion += "*/" + Environment.NewLine;
                strDeveloperRegion += __regionNameEndDeveloper + Environment.NewLine;
                return strDeveloperRegion;
            }
        }
       
        public string FilePatterns
        {
            get { return _filePatterns; }
            set { _filePatterns = value; }
        }

        public string IgnoreExtensions
        {
            get { return _ignoreExtensions.ToString(); }
            set 
            {
                _ignoreExtensions = new List<string>(value.Split(",".ToCharArray()));
            }
        }

        public void Write()
        {
            List<string> files = DirSearch(_SourceCodeLocation).Distinct().ToList();
            
            foreach (string file in files)
            {
                if(!IsToBeIgnored(file))
                    WriteDisclaimer(file);
            }
        }

        bool IsToBeIgnored(string fileName)
        {
            if (_ignoreExtensions == null) return false;
            
            var result = (from e in _ignoreExtensions
                          where e.ToLower() == fileName.Substring(fileName.Length - e.Length).ToLower()
                          select e).SingleOrDefault();

            return result != null ? true : false;
        }

        void WriteDisclaimer(string fileName)
        {
            StringBuilder fileContent = new StringBuilder();
            StringBuilder developerContent = new StringBuilder();
            using (StreamReader sr = new StreamReader(fileName))
            {
                bool readingDisclaimer = false;
                bool readingDeveloperRegion = false;
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine();
                    if (line.Trim().Equals(__regionName, StringComparison.OrdinalIgnoreCase))
                        readingDisclaimer = true;
                    else if (line.Trim().Equals(__regionNameEnd, StringComparison.OrdinalIgnoreCase))
                        readingDisclaimer = false;                    
                    if (!readingDisclaimer)
                        fileContent.AppendLine(line);


                    if (line.Trim().Equals(__regionNameDeveloper, StringComparison.OrdinalIgnoreCase))
                        readingDeveloperRegion = true;
                    else if (line.Trim().Equals(__regionNameEndDeveloper, StringComparison.OrdinalIgnoreCase))
                    {
                        developerContent.AppendLine(line);
                        readingDeveloperRegion = false;
                    }

                    if (readingDeveloperRegion)
                        developerContent.AppendLine(line);
                }
            }
            
            fileContent.Replace(__regionNameEnd, string.Empty);
            
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                
                if(developerContent.ToString() != string.Empty)                
                    _Disclaimer = string.Format(_Disclaimer, developerContent);
                else
                    _Disclaimer = string.Format(_Disclaimer, DeveloperRegionBlank);

                sw.WriteLine(_Disclaimer);
                sw.WriteLine(fileContent.ToString());
            }

        }

        string GetDisclaimer()
        {
            string disclaimer = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly()
                               .GetManifestResourceStream("Eyedia.Aarbac.Command.Disclaimer.Disclaimer.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                disclaimer = reader.ReadToEnd();
            }

            return disclaimer;
        }

        List<string> DirSearch(string sDir)
        {
            if (!Directory.Exists(sDir))
                return new List<string>();

            List<string> files = new List<string>();
            foreach (string f in Directory.GetFiles(sDir, _filePatterns))
            {
                files.Add(f);
            }
            foreach (string d in Directory.GetDirectories(sDir))
            {                
                foreach (string f in Directory.GetFiles(d, _filePatterns))
                {
                    files.Add(f);
                }
                files.AddRange(DirSearch(d));
            }

            return files;
        } 
    }
}




