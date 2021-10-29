using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using System.Threading.Tasks;
namespace Quick.Order.AppCore.BusinessOperations
{
    public class FrontOfficeRestaurantService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IEmailService emailService;
        private readonly ILocalHistoryService localSettingsService;

        public FrontOfficeRestaurantService(IOrdersRepository ordersRepository, IEmailService emailService, ILocalHistoryService localSettingsService)
        {
            this.ordersRepository = ordersRepository;
            this.emailService = emailService;
            this.localSettingsService = localSettingsService;
        }
     

        public async Task PlaceOrder(Models.Order order)
        {
            var result = await ordersRepository.Add(order);

            if (result != null)
            {
               localSettingsService.AddLocalPendingOrder(order);
               localSettingsService.DeleteLocalOrder();
               await emailService.SendEmailForOrder(order);
            }
        }



       

    }


}
