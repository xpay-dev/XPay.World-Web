using XPW.Utilities.BaseContext;

namespace XPW.NoSQL.Configurations.Entities {
     public class ClientRole : BaseModelInt {
          public string Name { get; set; }
          public int Order { get; set; }
     }
}