using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishSectionPageModel : ModalPageModelBase<EditDishSectionParams, OperationResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;

        public ICommand AddDishSectionCommand { get; set; }
        public ICommand DeleteDishSectionCommand { get; }
        public DishSection DishSectionToEdit { get; private set; }
        public string DishSectionName { get; set; }

        public Restaurant CurrentRestaurant { get; set; }
        public AddDishSectionPageModel(BackOfficeRestaurantService backOfficeRestaurantService, INavigationService navigationService)
        {
            AddDishSectionCommand = CreateAsyncCommand(AddDishSection);
            DeleteDishSectionCommand = CreateAsyncCommand(DeleteDishSection);

            this.backOfficeRestaurantService = backOfficeRestaurantService;
        }


        private async Task DeleteDishSection()
        {
            CurrentRestaurant.Menu.DeleteDishSection(DishSectionToEdit);

            await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            Parameter.MenuGroupedBySection.RemoveSection(DishSectionToEdit.Name);

            await SetResult(new OperationResult { WasSuccessful = true });

        }

        private async Task AddDishSection()
        {

            try
            {

                if (DishSectionToEdit == null)
                {
                    CurrentRestaurant.AddDishSectionToMenu(new DishSection { Name = DishSectionName });
                    await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

                    Parameter.MenuGroupedBySection.Add(new DishSectionGroupedModel() { SectionName = DishSectionName });
                }
                else
                {
                    CurrentRestaurant.Menu.UpdateDishSection(DishSectionToEdit.Name, DishSectionName);
                    await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

                    Parameter.MenuGroupedBySection.EditSection(DishSectionToEdit.Name, DishSectionName);
                }

               
                await SetResult(new OperationResult { WasSuccessful = true });

            }
            catch (System.Exception ex)
            {

                await SetResult(new OperationResult { WasSuccessful = false, ErrorMessage= ex.Message });

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

        public DishSectionGroupedModelCollection MenuGroupedBySection { get; set; } = new DishSectionGroupedModelCollection();

    }
}

