using System.Web.Http;
using XPW.Utilities.BaseContextManagement;

namespace XPW.Admin {
     public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(BaseWebApiConfig.Register);
        }
    }
}
