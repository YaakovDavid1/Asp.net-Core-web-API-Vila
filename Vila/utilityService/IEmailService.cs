using System;
using Vila.Data.Entities;

namespace Vila.utilityService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
