using MalikBerkane.MvvmToolkit;
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
            ValidateCommand =CreateAsyncCommand(Validate);

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
