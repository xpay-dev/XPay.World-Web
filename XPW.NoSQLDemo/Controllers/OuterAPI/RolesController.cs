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
     [RoutePrefix("admin/roles")]
     public class RolesController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<ClientsRole> roles { get; set; }
          public RolesController() {
               Model1 = new ClientsRole().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               FileChecker<ClientsAccount>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<ClientsRole>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<ClientsRole>.JsonReaderListAsync(Model1FileName);
                         return roles;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("save")]
          [HttpPost]
          public async Task<ClientsRole> Save(ClientsRole newData) {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<ClientsRole>.JsonReaderListAsync(Model1FileName);
                         roles.Add(newData);
                         _ = await Writer<ClientsRole>.JsonWriterListAsync(roles, Model1FileName);
                         return newData;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<ClientsRole> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<ClientsRole>.JsonReaderListAsync(Model1FileName);
                         var role = roles.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         return role;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<ClientsRole> Edit(ClientsRole updatedData) {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<ClientsRole>.JsonReaderListAsync(Model1FileName);
                         if (roles.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = roles.Where(a => a.Id.Equals(updatedData.Id)).FirstOrDefault();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         roles = roles.Where(a => !a.Id.Equals(updatedData.Id)).ToList();
                         updatedData.DateUpdated = DateTime.Now;
                         roles.Add(updatedData);
                         _ = await Writer<ClientsRole>.JsonWriterListAsync(roles, Model1FileName);
                         return old;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<List<ClientsRole>> Delete(Guid id) {
               return await Task.Run(async () => {
                    List<ClientsAccount> accounts = new List<ClientsAccount>();
                    try {
                         roles = await Reader<ClientsRole>.JsonReaderListAsync(Model1FileName);
                         roles = roles.Where(a => !a.Id.Equals(id)).ToList();
                         _ = await Writer<ClientsAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return await Reader<ClientsRole>.JsonReaderListAsync(Model1FileName); ;
                    } catch {
                         return null;
                    }
               });
          }
     }
}
