using XPW.CommonData.DataContext;
using XPW.CommonData.Entities;
using XPW.Utilities.BaseContext;

namespace XPW.CommonData.Repositories {
     public class PasswordHistoryRepository : BaseRepository<XPWCommonDataContext, PasswordHistory>, IPasswordHistoryRepository { }
     public interface IPasswordHistoryRepository : IBaseRepository<PasswordHistory> { }
}
