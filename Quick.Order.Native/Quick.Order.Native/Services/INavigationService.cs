namespace Quick.Order.Native.Services
{
    public interface INavigationService
    {
        ICommonNavigation Common { get; }
        ISignInNavigation SignIn { get; }
        ITakeOrderNavigation Order { get; }
        IBackOfficeNavigation BackOffice { get; }

    }
}