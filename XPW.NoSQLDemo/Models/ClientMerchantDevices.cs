using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class ClientMerchantDevices : BaseModelGuid {
          public string Name { get; set; }
          public string SerialNumber { get; set; }
          public string PIN { get; set; }
     }
}