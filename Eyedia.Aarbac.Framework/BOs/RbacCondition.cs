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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eyedia.Aarbac.Framework
{
   
    [DataContract]
    public class RbacCondition
    {
        public RbacCondition()
        {
            Columns = new List<string>();
        }

        public RbacCondition(string selfTableName, string name, string columns, string whereClause)
        {
            SelfTableName = selfTableName;
            Name = name;
            Columns = new List<string>();
            if(columns.Contains(","))
            {
                Columns = new List<string>(columns.Split(",".ToCharArray()));
            }
            else
            {
                Columns.Add(columns);
            }
            WhereClause = whereClause;            
        }

        [DataMember]
        public string SelfTableName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string WhereClause { get; set; }

        [DataMember]
        public List<string> Columns { get; set; }

        public XmlNode ToXml(XmlDocument doc)
        {
            XmlNode condnNode = doc.CreateElement("Condition");
            XmlAttribute nameA = doc.CreateAttribute("Name");
            nameA.Value = Name;
            condnNode.Attributes.Append(nameA);


            XmlAttribute columnsA = doc.CreateAttribute("Columns");
            columnsA.Value = string.Join(",", Columns.ToArray());
            condnNode.Attributes.Append(columnsA);

            XmlAttribute whereClauseA = doc.CreateAttribute("WhereClause");
            whereClauseA.Value = WhereClause;
            condnNode.Attributes.Append(whereClauseA);

            return condnNode;
        }

    }
}


