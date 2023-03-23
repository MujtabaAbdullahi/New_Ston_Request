
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace New_Ston_Request.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSettings mailJetsettings { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            mailJetsettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client = new MailjetClient(mailJetsettings.ApiKey, mailJetsettings.SecretKey)
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
        {"Name", "WoW Suppermarket"}
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
