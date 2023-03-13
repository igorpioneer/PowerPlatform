using Application;
using Application.DataTransfer;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Repository
{
    public class SmtpEmailSender : IEmailSender
    {
        public void Send(EmailDto dto)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("igor.rankovic.50.14@ict.edu.rs", "BarcelonA12")
            };

            var message = new MailMessage("igor.rankovic.50.14@ict.edu.rs", dto.SendTo);
            var attachment = new Attachment(new MemoryStream(dto.Attachment), "SuccessfullCampaignSalesReport.csv", "text/csv");
            message.Attachments.Add(attachment);
            message.Subject = dto.Subject;
            message.Body = dto.Content;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }
}
