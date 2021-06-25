using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels.Modal
{
    public class EditRestaurantInfosPageModel: ModalPageModelBase<RestaurantIdentity, RestaurantIdentity>
    {
        public ICommand ValidateCommand { get; set; }


        public RestaurantIdentity RestaurantIdentity { get; set; }
        public EditRestaurantInfosPageModel()
        {
            ValidateCommand = new AsyncCommand(Validate);

        }

        public override Task InitAsync()
        {
            RestaurantIdentity = Parameter.Clone();

            return Task.CompletedTask;
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

    public class RestaurantIdentity
    {
        public string Name { get; set; }

        public string Adresse { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Adresse);
        }

    }
}
