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

namespace Eyedia.Aarbac.Framework
{
    public class RbacCache
    {
        private static RbacCache _Instance;
        public const string TempPassword = "temppassword";
        private RbacCache()
        {
            //string passKey = RbacPassKey.GetPassKey();
            //if (String.IsNullOrEmpty(passKey))
            //    RbacPassKey.CreatePassKey(Resources.PassKey);
            //passKey = RbacPassKey.GetPassKey();
            
            string passKey = Resources.PassKey;
            if (String.IsNullOrEmpty(passKey))
                RbacException.Raise("Cannot retrieve pass key!");
            Password = UTF8Encoding.UTF8.GetBytes(passKey);

            Contexts = new Dictionary<string, Rbac>();
        }

        public Dictionary<string, Rbac> Contexts { get; private set; }

        public static RbacCache Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new RbacCache();

                return _Instance;
            }
        }

        public byte[] Password { get; private set; }

        public void AddContext(string userName, Rbac context)
        {
            if (Contexts.ContainsKey(userName))
                Contexts[userName] = context;
            else
                Contexts.Add(userName, context);
        }

        public Rbac GetContext(string userName)
        {
            if (Contexts.ContainsKey(userName))
                return Contexts[userName];

            return null;
        }

    }
}

