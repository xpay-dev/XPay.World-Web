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
     [RoutePrefix("clients-business-information")]
     public class ClientsBusinessInformationController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private static string Model2 { get; set; }
          private static string Model2FileName { get; set; }
          private List<ClientsBusinessInformation> clinetsbusinessinformation { get; set; }
          public ClientsBusinessInformationController() {
               Model1 = new ClientsBusinessInformation().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               Model2 = new MDRRate().GetType().Name;
               Model2FileName = AppDataFolder + "InnerAPI\\" + Model2 + ".json";
               FileChecker<MDRRate>.AutoCreateIfNotExists(Model2FileName);
               FileChecker<ClientsBusinessInformation>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<ClientsBusinessInformation>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         clinetsbusinessinformation = await Reader<ClientsBusinessInformation>.JsonReaderListAsync(Model1FileName);
                         return clinetsbusinessinformation;
                    } catch {
                         return clinetsbusinessinformation;
                    }
               });
          }
          [Route("save")]
          [HttpPost]
          public async Task<ClientsBusinessInformation> Save(ClientsBusinessInformation newData) {
               return await Task.Run(async () => {
                    try {
                         clinetsbusinessinformation = await Reader<ClientsBusinessInformation>.JsonReaderListAsync(Model1FileName);
                         clinetsbusinessinformation.Add(newData);
                         _ = await Writer<ClientsBusinessInformation>.JsonWriterListAsync(clinetsbusinessinformation, Model1FileName);
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
                         clinetsbusinessinformation = await Reader<ClientsBusinessInformation>.JsonReaderListAsync(Model1FileName);
                         var ClientsBusinessInformationData = clinetsbusinessinformation.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         var roles = await Reader<XPayRole>.JsonReaderListAsync(Model2FileName);
                         var role = roles.Where(a => a.Id.Equals(ClientsBusinessInformationData)).FirstOrDefault();
                         account.XPayRole = role;
                         return account;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<XPayAccount> Edit(XPayAccount account) {
               return await Task.Run(async () => {
                    try {
                         accounts = await Reader<XPayAccount>.JsonReaderListAsync(Model1FileName);
                         if (accounts.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = accounts.Where(a => a.Id.Equals(account.Id)).ToList();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         accounts = accounts.Where(a => !a.Id.Equals(account.Id)).ToList();
                         account.DateUpdated = DateTime.Now;
                         accounts.Add(account);
                         _ = await Writer<XPayAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return account;
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
