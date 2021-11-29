using FreshMvvm;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.Shared.Infrastructure.Authentication;
using Quick.Order.Shared.Infrastructure.Repositories;

namespace Quick.Order.Shared.Infrastructure
{
    public class Setup
    {
        public static void Init()
        {
            FreshIOC.Container.Register<IRestaurantRepository, RestaurantRepository>();
            FreshIOC.Container.Register<IOrdersRepository, OrdersRepository>();
            FreshIOC.Container.Register<ICurrencyRepository, CurrencyRepository>();

            FreshIOC.Container.Register<IAuthenticationService, FirebaseAuthenticationService>();
            FreshIOC.Container.Register<IConnectivityService, ConnectivityService>();

            FreshIOC.Container.Register<IEmailService, EmailService>();
            FreshIOC.Container.Register<ILocalHistoryService, LocalHistoryService>();
            FreshIOC.Container.Register<IVibrationService, VibrationService>();
            FreshIOC.Container.Register<BackOfficeSessionService, BackOfficeSessionService>().AsSingleton();
            FreshIOC.Container.Register<IImageService, ImageService>().AsSingleton();

        }

    }
}
