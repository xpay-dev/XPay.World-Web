using System;
using System.Web.Http;
using XPW.Utilities.BaseContext;

namespace XPW.Admin.Configurations.Base {
     public abstract class BaseController : ApiController {
     }
     public abstract class BaseControllerService<S> : ApiController where S : class, new() {
          public S Service { get; set; } = Activator.CreateInstance<S>();
     }
}
