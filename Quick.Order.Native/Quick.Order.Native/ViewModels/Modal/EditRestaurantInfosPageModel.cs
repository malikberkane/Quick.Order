using MalikBerkane.MvvmToolkit;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels.Modal
{
    public class EditRestaurantInfosPageModel: ModalPageModelBase<RestaurantIdentity, RestaurantIdentity>
    {
        private readonly IBackOfficeNavigation backOfficeNavigation;

        public ICommand ValidateCommand { get; set; }
        public ICommand GoToCurrencySelectionCommand { get; }

        public RestaurantIdentity RestaurantIdentity { get; set; }
        public EditRestaurantInfosPageModel(IBackOfficeNavigation backOfficeNavigation)
        {
            ValidateCommand =CreateAsyncCommand(Validate);
            GoToCurrencySelectionCommand = CreateCommand(SelectCurrency);
            this.backOfficeNavigation = backOfficeNavigation;
        }

        private async Task SelectCurrency()
        {
            var selectedCurrency = await backOfficeNavigation.GoToCurrencyChoice();

            if (selectedCurrency != null)
            {
                RestaurantIdentity.Currency = selectedCurrency;
            }
        }

        protected override void PostParamInitialization()
        {
            RestaurantIdentity = Parameter.Clone();
        }
        public Task Validate()
        {
            if (RestaurantIdentity.IsValid())
            {
                return SetResult(RestaurantIdentity);

            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }

    public class RestaurantIdentity: ObservableObject
    {
        public string Name { get; set; }

        public string Adresse { get; set; }

        public Currency Currency { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Adresse);
        }

    }

    
}
