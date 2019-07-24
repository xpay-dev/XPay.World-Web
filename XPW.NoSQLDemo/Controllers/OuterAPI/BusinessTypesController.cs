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
     [RoutePrefix("admin/business-types")]
     public class BusinessTypesController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<BusinessType> businessTypes { get; set; }
          public BusinessTypesController() {
               Model1 = new BusinessType().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               FileChecker<BusinessType>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<BusinessType>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         businessTypes = await Reader<BusinessType>.JsonReaderListAsync(Model1FileName);
                         return businessTypes;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<BusinessType> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         businessTypes = await Reader<BusinessType>.JsonReaderListAsync(Model1FileName);
                         var role = businessTypes.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         return role;
                    } catch {
                         return null;
                    }
               });
          }
     }
}
