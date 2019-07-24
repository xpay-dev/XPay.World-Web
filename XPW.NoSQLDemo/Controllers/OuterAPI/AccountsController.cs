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
     [RoutePrefix("accounts")]
     public class AccountsController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private static string Model2 { get; set; }
          private static string Model2FileName { get; set; }
          private List<ClientsAccount> accounts { get; set; }
          public AccountsController() {
               Model1 = new ClientsAccount().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               Model2 = new ClientsRole().GetType().Name;
               Model2FileName = AppDataFolder + "InnerAPI\\" + Model2 + ".json";
               FileChecker<ClientsRole>.AutoCreateIfNotExists(Model2FileName);
               FileChecker<ClientsAccount>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<ClientsAccount>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<ClientsAccount>.JsonReaderListAsync(Model1FileName);
                         return accounts;
                    } catch {
                         return accounts;
                    }
               });
          }
          [Route("save")]
          [HttpPost]
          public async Task<ClientsAccount> Save(ClientsAccount newData) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<ClientsAccount>.JsonReaderListAsync(Model1FileName);
                         accounts.Add(newData);
                         _ = await Writer<ClientsAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return newData;
                    } catch {
                         return newData;
                    }
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<ClientsAccount> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<ClientsAccount>.JsonReaderListAsync(Model1FileName);
                         var account = accounts.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         var roles = await Reader<ClientsRole>.JsonReaderListAsync(Model2FileName);
                         var role = roles.Where(a => a.Id.Equals(account.ClientsRoleId)).FirstOrDefault();
                         account.ClientsRole = role;
                         return account;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<ClientsAccount> Edit(ClientsAccount updatedData) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<ClientsAccount>.JsonReaderListAsync(Model1FileName);
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
                         _ = await Writer<ClientsAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return updatedData;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<List<ClientsAccount>> Delete(Guid id) {
               return await Task.Run(async () => {
                    List<ClientsAccount> accounts = new List<ClientsAccount>();
                    try {
                         accounts = await Reader<ClientsAccount>.JsonReaderListAsync(Model1FileName);
                         accounts = accounts.Where(a => !a.Id.Equals(id)).ToList();
                         _ = await Writer<ClientsAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return accounts;
                    } catch {
                         return accounts;
                    }
               });
          }
     }
}
