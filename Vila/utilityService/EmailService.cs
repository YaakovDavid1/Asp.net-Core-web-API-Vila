using Castle.Core.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using Vila.Data.Entities;
// קוד מהמייל qhyd hrrm zcbl nyug
namespace Vila.utilityService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = "ydmeuman3@gmail.com";
            emailMessage.From.Add(new MailboxAddress("ydmeuman3", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };
            using(var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("ydmeuman3@gmail.com", "qhydhrrmzcblnyug");
                    client.Send(emailMessage);
                }
                catch(Exception ex) 
                {
                    throw;
                }
                finally 
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
