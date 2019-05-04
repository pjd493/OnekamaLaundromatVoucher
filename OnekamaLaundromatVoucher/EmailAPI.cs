using System.IO;
using System.Net.Mail;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnekamaLaundromatVoucher
{
    
    class Validation
    {
        
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = {  GmailService.Scope.GmailSend, GmailService.Scope.GmailModify, GmailService.Scope.GmailInsert, GmailService.Scope.GmailCompose };
        static string ApplicationName = "Gmail API .NET Quickstart";

        /// <summary>
        ///Generates Email.         
        /// </summary>
        /// <param>none</param
        public class TestEmail
        {
            //Defines content of email as well as sends out the email. Autentication with Gmail.
            public static void SendIt()
            {//Defines Email content
                var msg = new AE.Net.Mail.MailMessage
                {
                    Subject = "Onekama Voucher Program",
                    Body = "A voucher with serial number " + VoucherForm.serial + " for $" + VoucherForm.amount + " has been created.",
                    From = new MailAddress("pjd493@gmail.com")
                };
                msg.To.Add(new MailAddress("pjd493@gmail.com"));
                msg.ReplyTo.Add(msg.From); // Fills in required field, from.
                var msgStr = new StringWriter();
                msg.Save(msgStr);
                
                //Gmail Authentication
                UserCredential credential;

                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "admin",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                var gmail = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                var result = gmail.Users.Messages.Send(new Google.Apis.Gmail.v1.Data.Message
                {
                    Raw = Base64UrlEncode(msgStr.ToString())
                }, "me").Execute();
                MessageBox.Show("Voucher was Created.", "Success! You're A Winner! Don't you ever forget that.");
            }

            //Change message contents to required format. 
            private static string Base64UrlEncode(string input)
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                // base64 required Gmail, prevent injection. 
                return System.Convert.ToBase64String(inputBytes)
                  .Replace('+', '-')
                  .Replace('/', '_')
                  .Replace("=", "");
            }
        }
    }
}

