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
     public class ContactInformationController : BaseAPIController {
          private static string Model1 { get; set; }
          private static string Model1FileName { get; set; }
          private List<ClientsContactInformation> contactInformation { get; set; }
          public ContactInformationController() {
               Model1 = new ClientsContactInformation().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               FileChecker<ClientsContactInformation>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<ClientsContactInformation> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         contactInformation = await Reader<ClientsContactInformation>.JsonReaderListAsync(Model1FileName);
                         var _contactInformation = contactInformation.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         return _contactInformation;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<ClientsContactInformation> Edit(ClientsContactInformation updatedData) {
               return await Task.Run(async () => {
                    try {
                         contactInformation = await Reader<ClientsContactInformation>.JsonReaderListAsync(Model1FileName);
                         if (contactInformation.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = contactInformation.Where(a => a.Id.Equals(updatedData.Id)).FirstOrDefault();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         contactInformation = contactInformation.Where(a => !a.Id.Equals(updatedData.Id)).ToList();
                         updatedData.DateUpdated = DateTime.Now;
                         contactInformation.Add(updatedData);
                         _ = await Writer<ClientsContactInformation>.JsonWriterListAsync(contactInformation, Model1FileName);
                         return old;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
     }
}
