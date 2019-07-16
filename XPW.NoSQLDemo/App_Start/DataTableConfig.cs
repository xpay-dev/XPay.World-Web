using System.IO;
using System.Web.Hosting;
using XPW.NoSQLDemo.Configurations;
using XPW.NoSQLDemo.Utilities;

namespace XPW.NoSQLDemo.App_Start {
     public static class DataTableConfig {
          public static void CheckDataTable() {
               string filePath = HostingEnvironment.ApplicationPhysicalPath + "App_Data\\data-table.json";
               FileChecker<DataTable>.AutoCreateIfNotExists(filePath);
          }
     }
}