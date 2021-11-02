namespace Quick.Order.Native.Services
{
    public class NavigationService : INavigationService
    {

        public NavigationService(ICommonNavigation commonNavigation, ISignInNavigation signInNavigation, ITakeOrderNavigation takeOrderNavigation, IBackOfficeNavigation backOfficeNavigation)
        {
            Common = commonNavigation;
            SignIn = signInNavigation;
            Order = takeOrderNavigation;
            BackOffice = backOfficeNavigation;
        }

        public ICommonNavigation Common { get; }

        public ISignInNavigation SignIn { get; }

        public ITakeOrderNavigation Order { get; }

        public IBackOfficeNavigation BackOffice { get; }
    }
}
