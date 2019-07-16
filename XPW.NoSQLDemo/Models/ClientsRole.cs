using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class ClientsRole : BaseModelGuid {
          public string Name { get; set; }
          public int Order { get; set; }
     }
}