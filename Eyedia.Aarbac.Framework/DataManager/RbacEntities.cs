using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Eyedia.Aarbac.Framework.DataManager
{
    public partial class Entities : DbContext
    {
        public Entities(string connectionString):base(connectionString)
        {

        }
    }
}
