using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.BaseContext;

namespace XPW.CommonData.XPWAdmin.Repositories {
     public class AccountRepository : BaseRepository<XPWAdminContext, Account>, IAccountRepository { }
     public interface IAccountRepository : IBaseRepository<Account> { }

     public class RoleRepository : BaseRepository<XPWAdminContext, Role>, IRoleRepository { }
     public interface IRoleRepository : IBaseRepository<Role> { }
}