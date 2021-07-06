using System;
using System.Collections.Generic;
using System.Text;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Quick.Order.AppCore.Contracts;

namespace Quick.Order.Shared.Infrastructure
{
    public class EmailService : IEmailService
    {
        public Task SendEmailForOrder(AppCore.Models.Order order)
        {
            var htmlString = "<ul>";

            foreach (var item in order.OrderedItems)
            {
                htmlString += $"<li>{item.Dish.Name} (× {item.Quantity})</li>";

            }
            htmlString += "</ul>";
            
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("tarektebtal@gmail.com");
                message.To.Add(new MailAddress("malikberkane@gmail.com"));
                message.Subject = $"Commande {order.ClientName}";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("tarektebtal@gmail.com", "bbyufzyjbcizvvtf");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);


            return Task.CompletedTask;
        }

        

    }
}
