using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishSectionPageModel : ModalPageModelBase<EditDishSectionParams, DishSectionEditionOperationResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public ICommand AddDishSectionCommand { get; set; }
        public ICommand DeleteDishSectionCommand { get; }
        public DishSection DishSectionToEdit { get; private set; }
        public string DishSectionName { get; set; }

        public Restaurant CurrentRestaurant { get; set; }
        public AddDishSectionPageModel(BackOfficeRestaurantService backOfficeRestaurantService, INavigationService navigationService)
        {
            AddDishSectionCommand = CreateAsyncCommand(AddDishSection);
            DeleteDishSectionCommand = CreateCommand(PromptDeleteDishSection);

            this.backOfficeRestaurantService = backOfficeRestaurantService;
            this.navigationService = navigationService;
        }


        private async Task PromptDeleteDishSection()
        {
            if (await navigationService.PromptForConfirmation("Attention", "Êtes-vous sûr de vouloir supprimer cette section du menu? Le plats contenus seront supprimés.", "Supprimer", "Annuler"))
            {
                await DeleteDishSection();

            }



        }

        private async Task DeleteDishSection()
        {
            CurrentRestaurant.Menu.DeleteDishSection(DishSectionToEdit);

            await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            await SetResult(new DishSectionEditionOperationResult { WasSuccessful = true, ResultDishSection = DishSectionToEdit, OperationType = OperationType.Deleted });
        }

        private async Task AddDishSection()
        {

            try
            {

                if (DishSectionToEdit == null)
                {
                    var newDishSection = new DishSection(DishSectionName);
                    CurrentRestaurant.AddDishSectionToMenu(newDishSection);
                    await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);
                    await SetResult(new DishSectionEditionOperationResult { WasSuccessful = true, OperationType= OperationType.Added, ResultDishSection=newDishSection });


                }
                else
                {
                    CurrentRestaurant.Menu.UpdateDishSection(DishSectionToEdit.Name, DishSectionName);
                    await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

                    var newEditedSection = new DishSection(DishSectionName, DishSectionToEdit.GetDishes()) ;

                    await SetResult(new DishSectionEditionOperationResult { WasSuccessful = true, OperationType = OperationType.Edited, ResultDishSection = newEditedSection });

                }



            }
            catch (System.Exception ex)
            {

                await SetResult(new DishSectionEditionOperationResult { WasSuccessful = false, ErrorMessage = ex.Message });

            }

        }


        public override Task InitAsync()
        {
            CurrentRestaurant = Parameter.Restaurant;
            if (Parameter.DishSectionToEdit != null)
            {
                DishSectionToEdit = Parameter.DishSectionToEdit.Clone();
                DishSectionName = DishSectionToEdit.Name;

            }
            return Task.CompletedTask;
        }
    }

    public class EditDishSectionParams
    {
        public Restaurant Restaurant { get; set; }
        public DishSection DishSectionToEdit { get; set; }


    }


    public class DishSectionEditionOperationResult: OperationResult
    {
        public DishSection ResultDishSection { get; set; }

        public OperationType OperationType { get; set; }
    }

    public enum OperationType
    {
        Added,
        Edited,
        Deleted
    }
}

