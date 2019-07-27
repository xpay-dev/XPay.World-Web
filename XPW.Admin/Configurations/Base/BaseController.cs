using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Utilities.BaseContext;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Configurations.Base {
     public abstract class BaseController : ApiController {
     }
     public abstract class BaseController<S> : ApiController where S : class, new() {
          public S BaseService { get; set; } = Activator.CreateInstance<S>();
     }
     public abstract class ServiceController<E, C> : ApiController where E : class, new() where C : DbContext, new() {
          public class BaseRepo : BaseRepository<C, E>, IBaseRepo  { }
          public interface IBaseRepo : IBaseRepository<E> { }
          public class BaseServices : BaseService<E, BaseRepo> { }
          public BaseServices Service = new BaseServices();

          internal string errorMessage = string.Empty;
          internal List<string> errorDetails = new List<string>();
          [Route("get-all")]
          [HttpGet]
          public async Task<GenericResponseListModel<E>> GetAll() {
               return await Task.Run(() => {
                    var data = new List<E>();
                    try {
                         data = Service.GetAll().ToList();
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseListModel<E>() {
                         Code                = string.IsNullOrEmpty(errorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(errorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(errorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details             = errorDetails,
                              ErrNumber           = "800",
                              Message             = errorMessage
                         }
                    };
               });
          }
     }
}
