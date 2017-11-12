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
