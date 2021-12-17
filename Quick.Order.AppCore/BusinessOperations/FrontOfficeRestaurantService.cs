using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Exceptions;
using System.Threading.Tasks;
namespace Quick.Order.AppCore.BusinessOperations
{
    public class FrontOfficeRestaurantService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IEmailService emailService;
        private readonly ILocalHistoryService localSettingsService;
        private readonly IConnectivityService connectivityService;

        public FrontOfficeRestaurantService(IOrdersRepository ordersRepository, IEmailService emailService, ILocalHistoryService localSettingsService, IConnectivityService connectivityService)
        {
            this.ordersRepository = ordersRepository;
            this.emailService = emailService;
            this.localSettingsService = localSettingsService;
            this.connectivityService = connectivityService;
        }
     

        public async Task PlaceOrder(Models.Order order)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            var result = await ordersRepository.Add(order);

            if (result != null)
            {
               localSettingsService.AddLocalPendingOrder(order);
               localSettingsService.DeleteLocalOrder();
            }
        }



       

    }


}
