using FreshMvvm;
using MalikBerkane.MvvmToolkit;

namespace Quick.Order.Native.Services
{
    public class BaseNavigationService
    {
        protected  ViewModelNavigationService viewModelNavigationService { get; } = FreshIOC.Container.Resolve<ViewModelNavigationService>();

    }
}
