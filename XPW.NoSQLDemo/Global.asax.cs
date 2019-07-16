using System.Web.Http;
using XPW.NoSQLDemo.App_Start;

namespace XPW.NoSQLDemo {
     public class WebApiApplication : System.Web.HttpApplication {
          protected void Application_Start() {
               GlobalConfiguration.Configure(WebApiConfig.Register);
               DataTableConfig.CheckDataTable();
          }
     }
}
