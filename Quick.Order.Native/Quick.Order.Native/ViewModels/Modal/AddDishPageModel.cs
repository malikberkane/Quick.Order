using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishPageModel : ModalPageModelBase<AddDishParams, DishEditionResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;

        public ICommand AddDishCommand { get; set; }

        public string DishName { get; set; }
        public string DishDescription { get; set; }
        public double DishPrice { get; set; }

        public Restaurant CurrentRestaurant { get; set; }

        public DishSection DishSection { get; set; }
        public AddDishPageModel(BackOfficeRestaurantService backOfficeRestaurantService)
        {
            AddDishCommand = CreateAsyncCommand(AddDish);

            this.backOfficeRestaurantService = backOfficeRestaurantService;
        }

        private async Task AddDish()
        {
            var dishToAdd = new Dish { Name = DishName, Price = DishPrice, Description = DishDescription };
            if (dishToAdd.IsValid())
            {
                DishSection.AddDish(dishToAdd);
                await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

                await SetResult(new DishEditionResult { WasSuccessful = true, AddedDish=dishToAdd });

            }
            else
            {
                throw new InvalidDishException();

            }

           

        }


        protected override void PostParamInitialization()
        {
            CurrentRestaurant = Parameter.Restaurant;
            DishSection = Parameter.Section;
        }
    }

    public class AddDishParams
    {
        public Restaurant Restaurant { get; set; }

        public DishSection Section { get; set; }

    }


    public class DishEditionResult : OperationResult
    {
        public Dish AddedDish { get; set; }

        public Dish EditedDish { get; set; }
        public Dish DeletedDish { get; internal set; }
    }


}

