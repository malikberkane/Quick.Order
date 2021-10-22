﻿using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class CreateUserViewModel : ExtendedPageModelBase
    {
        public ICommand CreateUserCommand { get; }


        private readonly IAuthenticationService authenticationService;
        private readonly BackOfficeSessionService backOfficeSessionService;
        private readonly INavigationService navigationService;

        public string NewUserText { get; set; }
        public string NewUserEmail { get; set; }

        public string NewUserPassword { get; set; }

        public CreateUserViewModel(IAuthenticationService authenticationService, BackOfficeSessionService backOfficeSessionService, INavigationService navigationService)
        {
            this.authenticationService = authenticationService;
            this.backOfficeSessionService = backOfficeSessionService;
            this.navigationService = navigationService;
            CreateUserCommand = CreateAsyncCommand(CreateUser);
        }

        private async Task CreateUser()
        {
           
            var autenticatedRestaurantAdmin = await authenticationService.CreateUser(NewUserText, NewUserEmail, NewUserPassword);

            if (autenticatedRestaurantAdmin?.RestaurantAdmin != null)
            {
                await backOfficeSessionService.SetRestaurantForSession(autenticatedRestaurantAdmin.RestaurantAdmin);
            }

            if (autenticatedRestaurantAdmin?.AuthenticationToken != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", autenticatedRestaurantAdmin.AuthenticationToken, "ok");
                await navigationService.GoToMainBackOffice();
            }
        }

    }

}
