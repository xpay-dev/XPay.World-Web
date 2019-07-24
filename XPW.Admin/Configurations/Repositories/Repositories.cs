using XPW.Admin.Configurations.DataContext;
using XPW.Admin.Configurations.Entities;
using XPW.Utilities.BaseContext;

namespace XPW.Admin.Configurations.Repositories {
     public class AccountRepository : BaseRepository<XPWAdminContext, Account>, IAccountRepository { }
     public interface IAccountRepository : IBaseRepository<Account> { }
}