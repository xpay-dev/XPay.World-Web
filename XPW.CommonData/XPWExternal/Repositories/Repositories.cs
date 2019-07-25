using XPW.CommonData.XPWExternal.DataContext;
using XPW.CommonData.XPWExternal.Entities;
using XPW.Utilities.BaseContext;

namespace XPW.CommonData.XPWExternal.Repositories {
     public class PasswordHistoryRepository : BaseRepository<XPWExternalContext, PasswordHistory>, IPasswordHistoryRepository { }
     public interface IPasswordHistoryRepository : IBaseRepository<PasswordHistory> { }
}
