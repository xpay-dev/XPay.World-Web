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
     public class AdminRolesController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<XPayRole> roles { get; set; }
          public AdminRolesController() {
               Model1         = new XPayRole().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               FileChecker<XPayAccount>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get-all")]
          [HttpGet]
          public async Task<List<XPayRole>> GetAll() {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<XPayRole>.JsonReaderListAsync(Model1FileName);
                         return roles;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("save")]
          [HttpPost]
          public async Task<XPayRole> Save(XPayRole role) {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<XPayRole>.JsonReaderListAsync(Model1FileName);
                         roles.Add(role);
                         _ = await Writer<XPayRole>.JsonWriterListAsync(roles, Model1FileName);
                         return role;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<XPayRole> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<XPayRole>.JsonReaderListAsync(Model1FileName);
                         var role = roles.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         return role;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<XPayRole> Edit(XPayRole role) {
               return await Task.Run(async () => {
                    try {
                         roles = await Reader<XPayRole>.JsonReaderListAsync(Model1FileName);
                         if (roles.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = roles.Where(a => a.Id.Equals(role.Id)).FirstOrDefault();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         roles               = roles.Where(a => !a.Id.Equals(role.Id)).ToList();
                         role.DateUpdated     = DateTime.Now;
                         roles.Add(role);
                         _ = await Writer<XPayRole>.JsonWriterListAsync(roles, Model1FileName);
                         return old;
                    } catch (Exception ex){
                         throw ex;
                    }
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<List<XPayRole>> Delete(Guid id) {
               return await Task.Run(async () => {
                    List<XPayAccount> accounts = new List<XPayAccount>();
                    try {
                         roles = await Reader<XPayRole>.JsonReaderListAsync(Model1FileName);
                         roles = roles.Where(a => !a.Id.Equals(id)).ToList();
                         _ = await Writer<XPayAccount>.JsonWriterListAsync(accounts, Model1FileName);
                         return await Reader<XPayRole>.JsonReaderListAsync(Model1FileName); ;
                    } catch {
                         return null;
                    }
               });
          }
     }
}
