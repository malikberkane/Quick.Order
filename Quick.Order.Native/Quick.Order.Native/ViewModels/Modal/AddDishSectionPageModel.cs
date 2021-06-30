﻿using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishSectionPageModel : ModalPageModelBase<Restaurant,OperationResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public ICommand AddDishSectionCommand { get; set; }

        public string DishSectionName { get; set; }

        public Restaurant CurrentRestaurant { get; set; }
        public AddDishSectionPageModel(BackOfficeRestaurantService backOfficeRestaurantService, INavigationService navigationService)
        {
            AddDishSectionCommand = new AsyncCommand(AddDishSection);
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            this.navigationService = navigationService;
        }

        private async Task AddDishSection()
        {

            try
            {
                CurrentRestaurant.AddDishSectionToMenu(new DishSection { Name = DishSectionName });

                await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

                await SetResult(new OperationResult { WasSuccessful = true });

            }
            catch (System.Exception ex)
            {

                await SetResult(new OperationResult { WasSuccessful = false, ErrorMessage= ex.Message });

            }

        }


        public override Task InitAsync()
        {
            CurrentRestaurant = Parameter;
            return Task.CompletedTask;
        }
    }


}
