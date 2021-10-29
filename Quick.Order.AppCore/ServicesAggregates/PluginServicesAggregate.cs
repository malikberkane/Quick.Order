using Quick.Order.AppCore.Contracts;

namespace Quick.Order.AppCore.ServicesAggregate
{

    public class PluginServicesAggregate
    {
        public PluginServicesAggregate(IEmailService emailService,
            ILoggerService loggerService, IPrintService printService,
            IVibrationService vibrationService,
            IDeepLinkService deepLinkService,
            IImageService imageService,
            IConnectivityService connectivityService)
        {
            Email = emailService;
            Logger = loggerService;
            Printer = printService;
            Vibration = vibrationService;
            DeepLink = deepLinkService;
            ImageService = imageService;
            Connectivity = connectivityService;
        }
        public IEmailService Email { get; }


        public ILoggerService Logger { get; }

        public IPrintService Printer { get; }

        public IVibrationService Vibration { get; }

        public IDeepLinkService DeepLink { get; }

        public IImageService ImageService { get; }

        public IConnectivityService Connectivity { get; }

    }
}
