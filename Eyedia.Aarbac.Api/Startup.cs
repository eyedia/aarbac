using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Eyedia.Aarbac.Api.Startup))]

namespace Eyedia.Aarbac.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SetDataDirectory();
        }

        private static void SetDataDirectory()
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\Eyedia.Aarbac.Framework\Databases");
            var fullPath = System.IO.Path.GetFullPath(path);
            AppDomain.CurrentDomain.SetData("DataDirectory", fullPath);
        }

    }
}
