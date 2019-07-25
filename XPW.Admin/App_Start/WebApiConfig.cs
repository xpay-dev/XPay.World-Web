﻿using System.Web.Http;

namespace XPW.Admin {
     public static class WebApiConfig {
          public static void Register(HttpConfiguration config) {
               // Web API configuration and services

               // Web API routes
               config.MapHttpAttributeRoutes();

               config.Routes.MapHttpRoute(
                   name: "DefaultApi",
                   routeTemplate: "{controller}/{action}/{id}",
                   defaults: new { id = RouteParameter.Optional, token = RouteParameter.Optional }
               );

               var json = config.Formatters.JsonFormatter;
               json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
               config.Formatters.Remove(config.Formatters.XmlFormatter);
               json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          }
     }
}
