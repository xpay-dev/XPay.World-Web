using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.NoSQLDemo.Configurations;
using XPW.NoSQLDemo.Models;
using XPW.NoSQLDemo.Utilities;
using XPW.Utilities.NoSQL;

namespace XPW.NoSQLDemo.Controllers.InnerAPI {
     [RoutePrefix("admin/business-types")]
     public class AdminBusinessTypesController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<BusinessType> businessTypes { get; set; }
          public AdminBusinessTypesController() {
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
          [Route("save")]
          [HttpPost]
          public async Task<BusinessType> Save(BusinessType newData) {
               return await Task.Run(async () => {
                    try {
                         businessTypes = await Reader<BusinessType>.JsonReaderListAsync(Model1FileName);
                         businessTypes.Add(newData);
                         _ = await Writer<BusinessType>.JsonWriterListAsync(businessTypes, Model1FileName);
                         return newData;
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
          [Route("edit")]
          [HttpPut]
          public async Task<BusinessType> Edit(BusinessType updatedData) {
               return await Task.Run(async () => {
                    try {
                         businessTypes = await Reader<BusinessType>.JsonReaderListAsync(Model1FileName);
                         if (businessTypes.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = businessTypes.Where(a => a.Id.Equals(updatedData.Id)).FirstOrDefault();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         businessTypes = businessTypes.Where(a => !a.Id.Equals(updatedData.Id)).ToList();
                         updatedData.DateUpdated = DateTime.Now;
                         businessTypes.Add(updatedData);
                         _ = await Writer<BusinessType>.JsonWriterListAsync(businessTypes, Model1FileName);
                         return old;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<List<BusinessType>> Delete(Guid id) {
               return await Task.Run(async () => {
                    List<BusinessType> accounts = new List<BusinessType>();
                    try {
                         businessTypes = await Reader<BusinessType>.JsonReaderListAsync(Model1FileName);
                         businessTypes = businessTypes.Where(a => !a.Id.Equals(id)).ToList();
                         _ = await Writer<BusinessType>.JsonWriterListAsync(accounts, Model1FileName);
                         return await Reader<BusinessType>.JsonReaderListAsync(Model1FileName); ;
                    } catch {
                         return null;
                    }
               });
          }
     }
}
