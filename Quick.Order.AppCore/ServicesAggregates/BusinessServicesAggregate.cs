using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;

namespace Quick.Order.AppCore.ServicesAggregate
{
    public class BusinessServicesAggregate
    {
        public BusinessServicesAggregate(BackOfficeRestaurantService backOffice,
            BackOfficeSessionService backOfficeSessionService,
            FrontOfficeRestaurantService frontOfficService,
            QrCodeGenerationService qrCodeGenerationService,
            ILocalHistoryService localHistoryService,
            OrderStatusTrackingService orderStatusTrackingService, IAuthenticationService authenticationService)
        {
            BackOffice = backOffice;
            BackOfficeSession = backOfficeSessionService;
            FrontOffice = frontOfficService;
            QrCodeGeneration = qrCodeGenerationService;
            LocalHistory = localHistoryService;
            OrdersStatusTracking = orderStatusTrackingService;
            Authentication = authenticationService;
        }

        public IAuthenticationService Authentication { get; }

        public BackOfficeRestaurantService BackOffice { get; }

        public BackOfficeSessionService BackOfficeSession { get; }

        public FrontOfficeRestaurantService FrontOffice { get; }

        public QrCodeGenerationService QrCodeGeneration { get; }

        public ILocalHistoryService LocalHistory { get; }

        public OrderStatusTrackingService OrdersStatusTracking { get; }

    }
}
