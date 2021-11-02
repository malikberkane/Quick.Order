using Quick.Order.Native.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using MalikBerkane.MvvmToolkit;

namespace Quick.Order.Native.ViewModels
{
    public class CreateUserPageModel : ExtendedPageModelBase
    {
        public ICommand CreateUserCommand { get; }


        public string NewUserText { get; set; }
        public string NewUserEmail { get; set; }

        public string NewUserPassword { get; set; }

        public CreateUserPageModel()
        {
            
            CreateUserCommand = CreateAsyncCommand(CreateUser);
        }

        private async Task CreateUser()
        {
           
            var autenticatedRestaurantAdmin = await ServicesAggregate.Business.Authentication.CreateUser(NewUserText, NewUserEmail, NewUserPassword);

            if (autenticatedRestaurantAdmin?.RestaurantAdmin != null)
            {
                await ServicesAggregate.Business.BackOfficeSession.SetRestaurantForSession(autenticatedRestaurantAdmin.RestaurantAdmin);
            }

            if (autenticatedRestaurantAdmin?.AuthenticationToken != null)
            {
                AlertUserService.ShowSnack("Success", AlertType.Success);
                await NavigationService.BackOffice.GoToMainBackOffice();
            }
        }

    }

}
