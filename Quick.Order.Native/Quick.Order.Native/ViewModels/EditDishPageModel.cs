using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class EditDishPageModel : ModalPageModelBase<EditDishParams, DishEditionResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public ICommand ValidateCommand { get; set; }

        public ICommand DeleteDishCommand { get; set; }


        public Restaurant CurrentRestaurant { get; set; }

        public Dish CurrentDish { get; set; }

        public Dish EditedDish { get; set; }

        public DishSection CurrentDishSection { get; set; }

        public EditDishPageModel(BackOfficeRestaurantService backOfficeRestaurantService,INavigationService navigationService)
        {
            ValidateCommand = CreateAsyncCommand(Validate);
            DeleteDishCommand = CreateCommand(PromptDeleteDishConfirmation);
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            this.navigationService = navigationService;
        }

        private async Task PromptDeleteDishConfirmation()
        {

            if (await navigationService.Common.PromptForConfirmation("Attention", "Êtes-vous sûr de vouloir supprimer ce plat du menu?", "Supprimer", "Annuler"))
            {
                await EnsurePageModelIsInLoadingState(DeleteDish);
            }



        }

        private async Task DeleteDish()
        {
            CurrentDishSection.Remove(CurrentDish);

            await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            await SetResult(new DishEditionResult { WasSuccessful = true, DeletedDish = CurrentDish });

        }

        private async Task Validate()
        {
            if (EditedDish.IsValid())
            {
                CurrentDishSection.UpdateDish(CurrentDish, EditedDish);
                await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

                await SetResult(new DishEditionResult { WasSuccessful = true, EditedDish = EditedDish });

            }
            else
            {
                throw new InvalidDishException();

            }




        }



        protected override void PostParamInitialization()
        {
            CurrentDish = Parameter.Dish;
            CurrentRestaurant = Parameter.Restaurant;
            CurrentDishSection = Parameter.Restaurant.Menu.GetDishSection(Parameter.Dish);
            EditedDish = Parameter.Dish.Clone();
        }
    }

    public class EditDishParams
    {
        public Restaurant Restaurant { get; set; }

        public Dish Dish { get; set; }



    }

}

