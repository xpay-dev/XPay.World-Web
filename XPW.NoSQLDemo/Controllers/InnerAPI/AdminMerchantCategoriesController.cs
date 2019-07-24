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
     public class AdminMerchantCategories : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<MerchantCategory> merchantCategories { get; set; }
          public AdminMerchantCategories() {
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
          [Route("save")]
          [HttpPost]
          public async Task<MerchantCategory> Save(MerchantCategory newData) {
               return await Task.Run(async () => {
                    try {
                         merchantCategories = await Reader<MerchantCategory>.JsonReaderListAsync(Model1FileName);
                         merchantCategories.Add(newData);
                         _ = await Writer<MerchantCategory>.JsonWriterListAsync(merchantCategories, Model1FileName);
                         return newData;
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
          [Route("edit")]
          [HttpPut]
          public async Task<MerchantCategory> Edit(MerchantCategory updatedData) {
               return await Task.Run(async () => {
                    try {
                         merchantCategories = await Reader<MerchantCategory>.JsonReaderListAsync(Model1FileName);
                         if (merchantCategories.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = merchantCategories.Where(a => a.Id.Equals(updatedData.Id)).FirstOrDefault();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         merchantCategories = merchantCategories.Where(a => !a.Id.Equals(updatedData.Id)).ToList();
                         updatedData.DateUpdated = DateTime.Now;
                         merchantCategories.Add(updatedData);
                         _ = await Writer<MerchantCategory>.JsonWriterListAsync(merchantCategories, Model1FileName);
                         return old;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<List<MerchantCategory>> Delete(Guid id) {
               return await Task.Run(async () => {
                    List<MerchantCategory> accounts = new List<MerchantCategory>();
                    try {
                         merchantCategories = await Reader<MerchantCategory>.JsonReaderListAsync(Model1FileName);
                         merchantCategories = merchantCategories.Where(a => !a.Id.Equals(id)).ToList();
                         _ = await Writer<MerchantCategory>.JsonWriterListAsync(accounts, Model1FileName);
                         return await Reader<MerchantCategory>.JsonReaderListAsync(Model1FileName); ;
                    } catch {
                         return null;
                    }
               });
          }
     }
}
