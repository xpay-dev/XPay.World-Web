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
          private List<XPayContactInformation> contactInformation { get; set; }
          public ContactInformationController() {
               Model1 = new XPayContactInformation().GetType().Name;
               Model1FileName = AppDataFolder + "InnerAPI\\" + Model1 + ".json";
               FileChecker<XPayContactInformation>.AutoCreateIfNotExists(Model1FileName);
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<XPayContactInformation> Get(Guid id) {
               return await Task.Run(async () => {
                    try {
                         contactInformation = await Reader<XPayContactInformation>.JsonReaderListAsync(Model1FileName);
                         var _contactInformation = contactInformation.Where(a => a.Id.Equals(id)).FirstOrDefault();
                         return _contactInformation;
                    } catch {
                         return null;
                    }
               });
          }
          [Route("edit")]
          [HttpPut]
          public async Task<XPayContactInformation> Edit(XPayContactInformation _contactInformation) {
               return await Task.Run(async () => {
                    try {
                         contactInformation = await Reader<XPayContactInformation>.JsonReaderListAsync(Model1FileName);
                         if (contactInformation.Count == 0) {
                              throw new Exception("No data to be update");
                         }
                         var old = contactInformation.Where(a => a.Id.Equals(_contactInformation.Id)).FirstOrDefault();
                         if (old == null) {
                              throw new Exception("No data to be update");
                         }
                         contactInformation = contactInformation.Where(a => !a.Id.Equals(_contactInformation.Id)).ToList();
                         _contactInformation.DateUpdated = DateTime.Now;
                         contactInformation.Add(_contactInformation);
                         _ = await Writer<XPayContactInformation>.JsonWriterListAsync(contactInformation, Model1FileName);
                         return old;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
     }
}
