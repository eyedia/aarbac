using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Eyedia.Aarbac.Api.Providers;

namespace Eyedia.Aarbac.Api
{
    

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Attribute routing. (with inheritance)
            //config.MapHttpAttributeRoutes(new WebApiCustomDirectRouteProvider());

            config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            // config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
         name: "getall",
         routeTemplate: "api/{controller}",
         defaults: new { Action = "getall" }
          );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { Action = "getbyid" },
                constraints: new { id = @"^\d+$" }
            );

            config.Routes.MapHttpRoute(
          name: "getbyname",
          routeTemplate: "api/{controller}/{name}",
          defaults: new { Action = "getbyname" }
           );


        }
    }
}
