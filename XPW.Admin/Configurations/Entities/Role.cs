using XPW.Utilities.BaseContext;

namespace XPW.Admin.Configurations.Entities {
     public class Role : BaseModelInt {
          public string Name { get; set; }
          public int Order { get; set; }
     }
}