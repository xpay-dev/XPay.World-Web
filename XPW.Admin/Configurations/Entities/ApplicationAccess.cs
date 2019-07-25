using XPW.Utilities.BaseContext;

namespace XPW.Admin.Configurations.Entities {
     public class ApplicationAccess : BaseModelInt {
          public string Name { get; set; }
          public string ParentName { get; set; }
          public int Order { get; set; }
     }
}