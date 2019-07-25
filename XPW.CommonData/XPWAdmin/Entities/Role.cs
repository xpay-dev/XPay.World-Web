using XPW.Utilities.BaseContext;

namespace XPW.CommonData.XPWAdmin.Entities {
     public class Role : BaseModelInt {
          public string Name { get; set; }
          public int Order { get; set; }
     }
}