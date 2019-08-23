using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.CommonData.XPWAdmin.Repositories;
using XPW.Utilities.BaseContext;
using XPW.Utilities.CryptoHashingManagement;
using XPW.Utilities.EmailManagement;
using XPW.Utilities.Functions;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.CommonData.XPWAdmin.ervices {
     public class AccountService : BaseService<Account, AccountRepository> {
          private string userDefaultFolder = ConfigurationManager.AppSettings["UserDefaultFolder"].ToString();
          public static readonly string key = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          public HashUtilityManagement crypto = new HashUtilityManagement(key, iv);
          public async Task GenerateUserJsonData(Account account) {
               userDefaultFolder += "\\" + base.DatabaseName() + "\\" + DateTime.Now.ToString("MM-yyyy");
               string fileName = account.AccountInformation.FirstName + " " + account.AccountInformation.LastName + ".json";
               if (!Directory.Exists(userDefaultFolder)) {
                    Directory.CreateDirectory(userDefaultFolder);
               }
               await Task.Run(async () => {
                    try {
                         _ = await Writer<Account>.JsonWriterAsync(account, userDefaultFolder + "//" + fileName);
                    } catch (Exception ex) {
                         System.Diagnostics.Debug.WriteLine(ex.ToString());
                         return;
                    }
               });
          }
          public async Task<bool> AccountEmail(Account account, string subject, string applicationUrl) {
               try {
                    List<EmailManagementodelReceipientModel> receipients = new List<EmailManagementodelReceipientModel>();
                    string displayName = account.AccountInformation.FirstName + (!string.IsNullOrEmpty(account.AccountInformation.MiddleName) ? " " + account.AccountInformation.MiddleName : string.Empty) + " " + account.AccountInformation.LastName;
                    receipients.Add(new EmailManagementodelReceipientModel {
                         DisplayName = displayName,
                         Email = account.EmailAddress
                    });
                    string htmlBody = "<html><body><h1>Welcome to XPay.World!</h1><br><br><br>"
                      + "To activate your account, please login <a heref=\"" + applicationUrl + "\"><strong>here</strong></a> to confirm your account.<br /><br />"
                      + "Your Username was :" + "<b>" + account.Username + "</b>" + "<br />"
                      + "Your Password was :" + "<b>" + crypto.Decrypt(account.Password) + "</b>" + "<br />"
                      + "If you need any assistance, please feel free to contact us at <a href=\"mailto:support-admin@xpay.world\">support-admin@xpay.world</a><br /><br /><br />"
                      + "*This email contains important and confidential information about your XPay.World Account, please save this message for future reference.<br /><br /><br />"
                      + "**Activation of account will gonna expired in one hour after receiving this email.<br />"
                      + "***Failed to activate account will gonna result in a given appropriate action, depending on the company policies.<br /><br /><br /><br /><br />"
                      + "Thank You,<br />XPay.World<br /></body></html>";
                    await Task.Run(() => {
                         new EmailSending().SendEmail(new EmailManagementodel {
                              EmailBccReceipients = new List<EmailManagementodelReceipientModel>(),
                              EmailCcReceipients = new List<EmailManagementodelReceipientModel>(),
                              EmailReceipients = receipients,
                              SenderDisplayName = displayName,
                              Subject = subject,
                              EmailContent = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html)
                         }, "Admin", "Default");
                    });
                    return true;
               } catch (Exception ex) {
                    return false;
               }
          }
          public string ActivationTokenGenerator(Account account, string password) {
               string toEncrypt = account.Username + "|" + password + "|" + account.DateCreated.ToString("MM-dd-yyyy");
               string cryptoString = crypto.Encrypt(toEncrypt);
               string base64String = crypto.EncodingToBase64(cryptoString);
               return base64String;
          }
          public async Task<Tuple<Account, bool, string>> ValidateActivationToken(string activator) {
               return await Task.Run(async () => {
                    try {
                         string cryptoString = crypto.DecodingFromBase64(activator);
                         string toDecypt = crypto.Decrypt(cryptoString);
                         string[] values = toDecypt.Split('|');
                         Account account = base.GetAll().Where(a => a.Username == values[0]).FirstOrDefault();
                         if (account == null) {
                              throw new Exception("Sorry, account not found");
                         }
                         if (account.IsDeleted) {
                              throw new Exception("Sorry, account not found");
                         }
                         if (account.IsVerified) {
                              throw new Exception("Account was already activated");
                         }
                         if (account.DateCreated.ToString("MM-dd-yyyy") != values[2]) {
                              throw new Exception("Invalid activation Token");
                         }
                         if (account.DateCreated.AddMinutes(70) < DateTime.Now) {
                              throw new Exception("Invalid activation Token");
                         }
                         if (crypto.Decrypt(account.Password) != values[1]) {
                              throw new Exception("Activation of account is expired, please contact administrator or support for assistance");
                         }
                         account.IsVerified = true;
                         account.DateUpdated = DateTime.Now;
                         account = await base.UpdateReturnAsync(account);
                         return new Tuple<Account, bool, string>(account, true, "Validated");
                    } catch (Exception ex) {
                         return new Tuple<Account, bool, string>(null, false, ex.Message);
                    }
               });
          }
          public async Task<Tuple<Account, bool, string>> ValidateActivationPasscodeToken(string activator) {
               return await Task.Run(async () => {
                    try {
                         List<Account> accounts = base.GetAll().ToList();
                         List<string> passcodes = new List<string>();
                         if (accounts == null) {
                              throw new Exception("Sorry, account not found");
                         }
                         if (accounts.Count == 0) {
                              throw new Exception("Sorry, account not found");
                         }
                         accounts.ForEach(a => {
                              passcodes.Add(a.Id + "=" + Checker.NumberExtractor(a.Id.ToString()) + "-" + a.AccountInformationId.ToString());
                         });
                         string passcodeId1 = string.Empty;
                         string passcodeId2 = string.Empty;
                         string[] passcodeArray = passcodes.ToArray();
                         for (int i = 0; i < passcodes.Count; i++) {
                              string[] values = passcodeArray[i].Split('=');
                              if (!string.IsNullOrEmpty(passcodeId1)) {
                                   break;
                              } else {
                                   if (values[1] == activator) {
                                        passcodeId1 = values[0];
                                        passcodeId2 = values[1].Split('-')[1];
                                   }
                              }
                         };
                         if (string.IsNullOrEmpty(passcodeId1)) {
                              throw new Exception("Sorry, account not found");
                         }
                         Account account = await Get(Guid.Parse(passcodeId1));
                         if (account == null) {
                         }
                         if (account.IsDeleted) {
                              throw new Exception("Sorry, account not found");
                         }
                         if (account.IsVerified) {
                              throw new Exception("Account was already activated");
                         }
                         if (account.DateCreated.AddMinutes(70) < DateTime.Now) {
                              throw new Exception("Invalid activation Token");
                         }
                         if (account.AccountInformationId.ToString() != passcodeId2) {
                              throw new Exception("Invalid activation Token");
                         }
                         account.IsVerified = true;
                         account.DateUpdated = DateTime.Now;
                         account = await base.UpdateReturnAsync(account);
                         return new Tuple<Account, bool, string>(account, true, "Validated");
                    } catch (Exception ex) {
                         return new Tuple<Account, bool, string>(null, false, ex.Message);
                    }
               });
          }
          public async Task<Account> UpdateForgottenAccount(string userAccess) {
               return await Task.Run(() => {
                    try {
                         Account account = userAccess.Contains("@")
                              ? Repository.AllIncluding(a => a.AccountInformation).Where(a => a.EmailAddress == userAccess).FirstOrDefault()
                              : Repository.AllIncluding(a => a.AccountInformation).Where(a => a.Username.Equals(userAccess, StringComparison.CurrentCulture)).FirstOrDefault();
                         if (account == null) {
                              throw new Exception("Invalid Account details");
                         }
                         string newPassword = Generator.StringGenerator();
                         account.Password = crypto.Encrypt(newPassword);
                         account.DateUpdated = DateTime.Now;
                         UpdateAsync(account);
                         return account;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          public async Task<bool> AccountForgotPasswordEmail(Account account, string subject, string applicationUrl) {
               try {
                    List<EmailManagementodelReceipientModel> receipients = new List<EmailManagementodelReceipientModel>();
                    string displayName = account.AccountInformation.FirstName + (!string.IsNullOrEmpty(account.AccountInformation.MiddleName) ? " " + account.AccountInformation.MiddleName : string.Empty) + " " + account.AccountInformation.LastName;
                    receipients.Add(new EmailManagementodelReceipientModel {
                         DisplayName = displayName,
                         Email = account.EmailAddress
                    });
                    string htmlBody = "<html><body><h1>Forgot Password!</h1><br><br><br>"
                      + "Hi " + account.AccountInformation.FirstName + " " + account.AccountInformation.LastName + ",<br />"
                      + "Please click <a heref=\"" + applicationUrl + "\"><strong>here</strong></a> change your password<br /><br />"
                      + "Your Temporary Password was :" + "<b>" + crypto.Decrypt(account.Password) + "</b>" + "<br />"
                      + "If you need any assistance, please feel free to contact us at <a href=\"mailto:support-admin@xpay.world\">support-admin@xpay.world</a><br /><br /><br />"
                      + "*This email contains important and confidential information about your XPay.World Account, please save this message for future reference.<br /><br /><br />"
                      + "**Activation of account will gonna expired in one hour after receiving this email.<br />"
                      + "***Failed to activate account will gonna result in a given appropriate action, depending on the company policies.<br /><br /><br /><br /><br />"
                      + "Thank You,<br />XPay.World<br /></body></html>";
                    await Task.Run(() => {
                         new EmailSending().SendEmail(new EmailManagementodel {
                              EmailBccReceipients = new List<EmailManagementodelReceipientModel>(),
                              EmailCcReceipients = new List<EmailManagementodelReceipientModel>(),
                              EmailReceipients = receipients,
                              SenderDisplayName = displayName,
                              Subject = subject,
                              EmailContent = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html)
                         }, "Admin", "Default");
                    });
                    return true;
               } catch (Exception ex) {
                    return false;
               }
          }
          public async Task<Tuple<Account, bool, string>> ForgotPasswordTokenValidator(string token) {
               return await Task.Run(async () => {
                    try {
                         List<Account> accounts = base.GetAll().ToList();
                         List<string> passcodes = new List<string>();
                         if (accounts == null) {
                              throw new Exception("Sorry, account not found");
                         }
                         if (accounts.Count == 0) {
                              throw new Exception("Sorry, account not found");
                         }
                         accounts.ForEach(a => {
                              passcodes.Add(a.Id + "=" + a.RoleId.ToString() + "-" + a.DateCreated.ToString("yyddMM") + "_" + Checker.NumberExtractor(a.Id.ToString()) + "-" + a.AccountInformationId.ToString());
                         });
                         string Id           = string.Empty;
                         string RoleId       = string.Empty;
                         string DateCreated  = string.Empty;
                         string AccInfoId    = string.Empty;
                         string[] passcodeArray = passcodes.ToArray();
                         for (int i = 0; i < passcodes.Count; i++) {
                              string[] values = passcodeArray[i].Split('=');
                              if (!string.IsNullOrEmpty(Id)) {
                                   break;
                              } else {
                                   if (values[1] == token) {
                                        Id             = values[0];
                                        RoleId         = values[1].Split('-')[0];
                                        DateCreated    = values[1].Split('-')[1];
                                        AccInfoId      = values[1].Split('-')[3];
                                   }
                              }
                         };
                         if (string.IsNullOrEmpty(Id)) {
                              throw new Exception("Sorry, account not found");
                         }
                         Account account = await Get(Guid.Parse(Id));
                         if (account == null) {
                         }
                         if (account.IsDeleted) {
                              throw new Exception("Sorry, account not found");
                         }
                         if (account.DateUpdated.Value.AddMinutes(70) < DateTime.Now) {
                              throw new Exception("Invalid activation Token");
                         }
                         if (account.AccountInformationId.ToString() != AccInfoId) {
                              throw new Exception("Invalid activation Token");
                         }
                         if (account.RoleId.ToString() != RoleId) {
                              throw new Exception("Invalid activation Token");
                         }
                         if (account.DateCreated.ToString("yyddMM") != DateCreated) {
                              throw new Exception("Invalid activation Token");
                         }
                         return new Tuple<Account, bool, string>(account, true, "Validated");
                    } catch (Exception ex) {
                         return new Tuple<Account, bool, string>(null, false, ex.Message);
                    }
               });
          }
     }
}