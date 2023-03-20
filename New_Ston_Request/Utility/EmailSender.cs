
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace New_Ston_Request.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("6b23a0061ab0c1cba301ae5c5637cdb8","921f0be1299e345a66fbbc594feeb725")
            {
                Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "mujtabamuhammadi037@gmail.com"},
        {"Name", "Mujtaba"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "WoW Supper Market"
         }
        }
       }
      }, {
       "Subject",
       subject
      },{
       "HTMLPart",
       body
      }
     }
             });
            await client.PostAsync(request);
        }
    }
}
