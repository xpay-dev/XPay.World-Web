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
     [RoutePrefix("admin/accounts")]
     public class AdminAccountsController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private static string Model2 { get; set; }
          private static string Model2FileName { get; set; }
          private List<XPayAccount> accounts { get; set; }
          public AdminAccountsController() {
               Model1 = new XPayAccount().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               Model2 = new XPayRole().GetType().Name;
               Model2FileName = AppDataFolder + "InnerAPI\\" + Model2 + ".json";
               FileChecker<XPayAccount>.AutoCreateIfNotExists(Model1FileName);
               FileChecker<XPayRole>.AutoCreateIfNotExists(Model2FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<XPayAccount>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<XPayAccount>.JsonReaderListAsync(Model1FileName);
                         return accounts;
                    } catch {
                         return accounts;
                    }
               });
          }
          [Route("save")]
          [HttpPost]
          public async Task<XPayAccount> Save(XPayAccount newData) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<XPayAccount>.JsonReaderListAsync(Model1FileName);
                         accounts.Add(newData);
                         _ = await Writer<XPayAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return newData;
                    } catch {
                         return newData;
                    }
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<XPayAccount> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<XPayAccount>.JsonReaderListAsync(Model1FileName);
                         var account = accounts.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         var roles = await Reader<XPayRole>.JsonReaderListAsync(Model2FileName);
                         var role = roles.Where(a => a.Id.Equals(account.XPayRoleId)).FirstOrDefault();
                         account.XPayRole = role;
                         return account;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<XPayAccount> Edit(XPayAccount updatedData) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<XPayAccount>.JsonReaderListAsync(Model1FileName);
                         if (accounts.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = accounts.Where(a => a.Id.Equals(updatedData.Id)).ToList();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         accounts = accounts.Where(a => !a.Id.Equals(updatedData.Id)).ToList();
                         updatedData.DateUpdated = DateTime.Now;
                         accounts.Add(updatedData);
                         _ = await Writer<XPayAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return updatedData;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<List<XPayAccount>> Delete(Guid id) {
               return await Task.Run(async () => {
                    List<XPayAccount> accounts = new List<XPayAccount>();
                    try {
                         accounts = await Reader<XPayAccount>.JsonReaderListAsync(Model1FileName);
                         accounts = accounts.Where(a => !a.Id.Equals(id)).ToList();
                         _ = await Writer<XPayAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return accounts;
                    } catch {
                         return accounts;
                    }
               });
          }
     }
}
