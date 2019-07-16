using System.Web.Hosting;
using System.Web.Http;

namespace XPW.NoSQLDemo.Configurations {
     public class BaseAPIController : ApiController {
          public static string AppDataFolder { get; set; } = string.Empty;
          public BaseAPIController() {
               AppDataFolder = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\";
          }

     }
}
