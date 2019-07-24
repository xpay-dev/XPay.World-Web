using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.NoSQLDemo.Configurations;
using XPW.NoSQLDemo.Models;
using XPW.NoSQLDemo.Utilities;
using XPW.Utilities.NoSQL;

namespace XPW.NoSQLDemo.Controllers.OuterAPI {
     [RoutePrefix("business-types")]
     public class MerchantCategories : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<MerchantCategory> merchantCategories { get; set; }
          public MerchantCategories() {
               Model1 = new MerchantCategory().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               FileChecker<MerchantCategory>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<MerchantCategory>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         merchantCategories = await Reader<MerchantCategory>.JsonReaderListAsync(Model1FileName);
                         return merchantCategories;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<MerchantCategory> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         merchantCategories = await Reader<MerchantCategory>.JsonReaderListAsync(Model1FileName);
                         var role = merchantCategories.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         return role;
                    } catch {
                         return null;
                    }
               });
          }
     }
}
