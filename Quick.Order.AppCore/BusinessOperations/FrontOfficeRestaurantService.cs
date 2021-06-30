using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace Quick.Order.AppCore.BusinessOperations
{
    public class FrontOfficeRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IOrdersRepository ordersRepository;

        public FrontOfficeRestaurantService(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
            //this.ordersRepository = ordersRepository;
        }
        public Task<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            return restaurantRepository.Get();
        }

        public Task<Restaurant> GetRestaurantById(Guid id)
        {
            return restaurantRepository.GetById(id);
        }

        public async Task PlaceOrder(Quick.Order.AppCore.Models.Order order)
        {
            SendEmail("Commande du resto réussie");

            //var result= ordersRepository.Add(order);

            //if (result != null)
            //{
            //    SendEmail("Commande du resto réussie");
            //}
        }

        public Task<Restaurant> GetRestaurantCloseBy(string adress, int maxDistance,int count)
        {
            return null;
        }

        public void SendEmail(string htmlString)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("tarektebtal@gmail.com");
                message.To.Add(new MailAddress("malikberkane@gmail.com"));
                message.Subject = "Commande";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("tarektebtal@gmail.com", "bbyufzyjbcizvvtf");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) 
            {

            }
        }
    }


}
