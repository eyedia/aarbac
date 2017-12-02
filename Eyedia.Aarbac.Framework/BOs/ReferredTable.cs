using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyedia.Aarbac.Framework
{
    public class ReferredTable
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }

        public ReferredTable()
        {

        }
    }
}
