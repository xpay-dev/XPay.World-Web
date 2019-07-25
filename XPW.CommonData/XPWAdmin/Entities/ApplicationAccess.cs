using XPW.Utilities.BaseContext;

namespace XPW.CommonData.XPWAdmin.Entities {
     public class ApplicationAccess : BaseModelInt {
          public string Name { get; set; }
          public string ParentName { get; set; }
          public int Order { get; set; }
     }
}