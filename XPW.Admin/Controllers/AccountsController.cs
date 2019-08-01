using System.Web.Http;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.BaseContextManagement;

namespace XPW.Admin.Controllers {
     [RoutePrefix("accounts")]
     public class AccountsController : BaseServiceController<Account, XPWAdminContext> {
     }
}
