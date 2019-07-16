using System.Web.Http;

namespace XPW.NoSQLDemo {
     public static class WebApiConfig {
          public static void Register(HttpConfiguration config) {
               // Web API configuration and services

               // Web API routes
               config.MapHttpAttributeRoutes();

               config.Routes.MapHttpRoute(
                   name: "DefaultOuterApi",
                   routeTemplate: "{controller}/{id}",
                   defaults: new { id = RouteParameter.Optional }
               );

               config.Routes.MapHttpRoute(
                   name: "DefaultInnerApi",
                   routeTemplate: "admin/{controller}/{id}",
                   defaults: new { id = RouteParameter.Optional }
               );

               var json = config.Formatters.JsonFormatter;
               json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
               config.Formatters.Remove(config.Formatters.XmlFormatter);
               json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          }
     }
}
