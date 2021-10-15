using Quick.Order.AppCore;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace Quick.Order.Native.ViewModels
{
    public class BackOfficeHomePageModel : ExtendedPageModelBase
    {
        private readonly BackOfficeRestaurantService backOfficeService;
        private readonly IImageService imageService;
        private readonly IVibrationService vibrationService;
        private readonly FrontOfficeRestaurantService restaurantService;
        private readonly OrdersTrackingService ordersTrackingService;
        private readonly INavigationService navigationService;
        private readonly PageModelMessagingService messagingService;
        private readonly IAuthenticationService authenticationService;
        private readonly BackOfficeSessionService backOfficeSessionService;

        public ObservableCollection<Restaurant> Items { get; }

        public ObservableCollection<OrderVm> Orders { get; set; }

        public RestaurantAdmin CurrentLoggedAccount { get; set; }
        public ICommand LogoutCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand GoToOrderDetailsCommand { get; }
        public ICommand GoToMenuEditionCommand { get; }
        public ICommand GenerateQrCodeCommand { get; }
        public ICommand EditOrderStatusCommand { get; }
        public ICommand PickRestaurantPictureCommand { get; }

        public ICommand TakeRestaurantPictureCommand { get; }

        public DishSectionGroupedModelCollection MenuGroupedBySection { get; set; } = new DishSectionGroupedModelCollection();

        public BackOfficeHomePageModel(BackOfficeRestaurantService backofficeService, IImageService imageService, IVibrationService vibrationService, FrontOfficeRestaurantService restaurantService, OrdersTrackingService ordersTrackingService, INavigationService navigationService, PageModelMessagingService messagingService, IAuthenticationService authenticationService, BackOfficeSessionService backOfficeSessionService)
        {
            Items = new ObservableCollection<Restaurant>();
            GoToMenuEditionCommand = CreateAsyncCommand<Restaurant>(GoToMenuEditionPage);
            GenerateQrCodeCommand = CreateAsyncCommand(GoToQrCodeGeneration);
            LogoutCommand = CreateAsyncCommand(Logout);
            AddDishCommand = CreateCommand<string>(AddDish);
            PickRestaurantPictureCommand = CreateCommand(ChangeRestaurantPicture);

            TakeRestaurantPictureCommand = CreateCommand(TakeRestaurantPicture);

            ReloadMenuCommand = CreateAsyncCommand(Reload);
            AddDishSectionCommand = CreateCommand(AddDishSection);
            DeleteRestaurantCommand = CreateAsyncCommand(DeleteCurrentRestaurant);
            EditSectionCommand = CreateCommand<string>(Add0rEditDishSection);
            GoToEditRestaurantInfosCommand = CreateCommand(GoToEditRestaurantInfos);
            GoToEditDishCommand = CreateCommand<Dish>(EditDish);
            this.backOfficeService = backofficeService;
            this.imageService = imageService;
            this.vibrationService = vibrationService;
            AddItemCommand = CreateCommand(AddRestaurant);
            GoToOrderDetailsCommand = CreateAsyncCommand<OrderVm>(GoToOrderDetails);
            this.restaurantService = restaurantService;
            this.ordersTrackingService = ordersTrackingService;
            this.navigationService = navigationService;
            this.messagingService = messagingService;
            this.authenticationService = authenticationService;
            this.backOfficeSessionService = backOfficeSessionService;
        }

        private async Task TakeRestaurantPicture()
        {
            Stream photoStream = null;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                photoStream = await photo.OpenReadAsync();


            });

            if (photoStream != null)
            {
                var url = await imageService.SaveImage($"photo_{CurrentLoggedAccount.UserId}", photoStream);

                CurrentRestaurant.RestaurantPhotoSource = url;

                await backOfficeService.UpdateRestaurant(CurrentRestaurant);

                await Reload();
            }

        }

        
        private async Task ChangeRestaurantPicture()
        {
            Stream photoStream = null;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var photo = await MediaPicker.PickPhotoAsync();

                photoStream = await photo.OpenReadAsync();
                messagingService.Send("photo", photoStream);
               

            });

            if (photoStream != null)
            {
                await Task.Delay(200);
                var url = await imageService.SaveImage($"photo_{CurrentLoggedAccount.UserId}", photoStream);
                CurrentRestaurant.RestaurantPhotoSource = url;
                await backOfficeService.UpdateRestaurant(CurrentRestaurant);

            }

        }

        private Task AddDishSection()
        {
            return Add0rEditDishSection(sectionName: null);
        }

        private Task GoToQrCodeGeneration()
        {
            return navigationService.GoToQrGeneration(CurrentRestaurant);
        }

        private Task GoToOrderDetails(OrderVm order)
        {
            return navigationService.GoToOrderDetails(MappingService.VmToModel(order));
        }

        private async Task GoToMenuEditionPage(Restaurant restaurant)
        {
            await navigationService.GoToMenuEdition(restaurant);
        }

        private void EditOrderStatus(OrderStatusEditionResult result)
        {
            if (result != null)
            {
                if (result.WasSuccessful)
                {
                    if (result.WasDeleted)
                    {
                        Orders.Remove(result.Order.ModelToVm());
                    }
                    else
                    {
                        var updateOrderIndex = Orders.IndexOf(result.Order.ModelToVm());
                        if (updateOrderIndex != -1)
                        {
                            Orders[updateOrderIndex].OrderStatus = result.ValidatedStatus;

                        }
                    }
                }
                else
                {
                    HandleError(result.ErrorMessage ?? "An error occured");

                }


            }


        }



        protected override void PostParamInitialization()
        {
            messagingService.Subscribe<OrderStatusEditionResult>("OrderStatusEdited", (edition) =>
                {
                    EditOrderStatus(edition);
                }, this);
        }

        public override Task InitAsync()
        {

            Orders = new ObservableCollection<OrderVm>();

            CurrentLoggedAccount = authenticationService.LoggedUser?.RestaurantAdmin;
            CurrentRestaurant = backOfficeSessionService.CurrentRestaurantSession;
            if (CurrentRestaurant?.Menu != null)
            {
                CreateMenuList(CurrentRestaurant.Menu);
            }
            ordersTrackingService.StartOrdersTracking();
            ordersTrackingService.OrderListChanged += OrdersTrackingService_OrderListChanged;

            return Task.CompletedTask;

        }


        private async Task Reload()
        {
            if (CurrentRestaurant != null)
            {
                CurrentRestaurant = await restaurantService.GetRestaurantById(CurrentRestaurant.Id);
                if (CurrentRestaurant?.Menu != null)
                {
                    CreateMenuList(CurrentRestaurant.Menu);
                }
            }

        }

        private void OrdersTrackingService_OrderListChanged(object sender, OrdersChangedEventArgs args)
        {
            try
            {
                if (args.ListDifferences.RemovedItems != null)
                {
                    foreach (var removedItem in args.ListDifferences.RemovedItems)
                    {
                        Orders.Remove(removedItem.ModelToVm());

                    }
                }

                if (args.ListDifferences.NewItems != null && args.ListDifferences.NewItems.Any())
                {
                    vibrationService.Vibrate();
                    AlertUserService.ShowSnack("Nouvelles commandes reçues");
                    foreach (var addedItems in args.ListDifferences.NewItems)
                    {
                        Orders.Insert(0, addedItems.ModelToVm());

                    }
                }
            }
            catch (System.Exception ex)
            {

                OnExceptionCaught(ex);
            }

        }

        public override Task CleanUp()
        {
            messagingService.Unsubscribe<OrderStatusEditionResult>("OrderStatusEdited", this);

            ordersTrackingService.StopOrderTracking();
            ordersTrackingService.OrderListChanged -= OrdersTrackingService_OrderListChanged;
            return Task.CompletedTask;
        }


        private async Task Logout()
        {

            await this.CleanUp();
            await authenticationService.SignOut();

            await navigationService.GoToLanding();
        }


        public ICommand AddDishCommand { get; set; }
        public ICommand ReloadMenuCommand { get; }
        public ICommand AddDishSectionCommand { get; set; }

        public ICommand DeleteRestaurantCommand { get; set; }
        public ICommand EditSectionCommand { get; }
        public ICommand GoToEditRestaurantInfosCommand { get; set; }

        public ICommand GoToEditDishCommand { get; set; }



        private async Task Add0rEditDishSection(string sectionName)
        {
            var dishSectionToEdit = sectionName != null ? CurrentRestaurant.Menu.GetDishSectionByName(sectionName) : null;

            var addOrEditDishParams = new EditDishSectionParams() { Restaurant = CurrentRestaurant, DishSectionToEdit = dishSectionToEdit };

            var editDishResult = await navigationService.GoToAddOrEditDishSection(addOrEditDishParams);
            if (editDishResult != null && editDishResult.WasSuccessful)
            {
                switch (editDishResult.OperationType)
                {
                    case OperationType.Added:
                        MenuGroupedBySection.AddDishSection(editDishResult.ResultDishSection);
                        break;
                    case OperationType.Edited:
                        MenuGroupedBySection.EditSection(sectionName, editDishResult.ResultDishSection.Name);
                        break;
                    case OperationType.Deleted:
                        MenuGroupedBySection.RemoveSection(sectionName);

                        break;
                    default:
                        break;
                }
            }
        }

        private async Task EditDish(Dish dishToEdit)
        {
            var result = await navigationService.GoToEditDish(new EditDishParams { Dish = dishToEdit, Restaurant = CurrentRestaurant });

            if (result != null && result.WasSuccessful)
            {
                if (result.DeletedDish != null)
                {
                    MenuGroupedBySection.RemoveDish(result.DeletedDish);
                }
                else
                {
                    MenuGroupedBySection.UpdateDish(dishToEdit, result.EditedDish);
                }
            }

        }

        private async Task GoToEditRestaurantInfos()
        {
            var restaurantEdited = await navigationService.GoToEditRestaurantInfos(new Modal.RestaurantIdentity { Adresse = CurrentRestaurant.Adresse, Name = CurrentRestaurant.Name });

            if (restaurantEdited != null && restaurantEdited.IsValid())
            {
                CurrentRestaurant.EditIdentity(restaurantEdited.Name, restaurantEdited.Adresse);


                await EnsurePageModelIsInLoadingState(async () =>
                {
                    await backOfficeService.UpdateRestaurant(CurrentRestaurant);

                    await Reload();
                });
            }
        }


        private async Task AddDish(string sectionName)
        {

            var section = CurrentRestaurant.Menu.GetDishSectionByName(sectionName);

            var navParams = new AddDishParams { Restaurant = CurrentRestaurant, Section = section };

            var result = await navigationService.GoToAddDish(navParams);

            if (result != null && result.WasSuccessful)
            {
                if (result.AddedDish != null)
                {
                    MenuGroupedBySection.AddDishToSection(section.Name, result.AddedDish);
                }

            }

        }

        private async Task DeleteCurrentRestaurant()
        {
            if (await navigationService.PromptForConfirmation("Attention", "êtes-vous sûr de vouloir supprimer le restaurant ? Le menu sera supprimé.", "Supprimer", "Annuler"))
            {
                await backOfficeService.DeleteRestaurant(CurrentRestaurant);

                CurrentRestaurant = null;
                if (MenuGroupedBySection != null)
                {
                    MenuGroupedBySection.Clear();
                }

                AlertUserService.ShowSnack("Restaurant supprimé");

            }


        }


        public Restaurant CurrentRestaurant { get; set; }


        private void CreateMenuList(Menu menu)
        {
            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }

            foreach (var section in menu.Sections)
            {
                var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                foreach (var dish in section.Dishes)
                {
                    newSection.Add(dish);
                }

                MenuGroupedBySection.Add(newSection);
            }
        }

        private async Task AddRestaurant()
        {
            var editedRestaurant = await navigationService.GoToRestaurantEdition();

            if (editedRestaurant != null)
            {
                CurrentRestaurant = editedRestaurant;
                CreateMenuList(editedRestaurant.Menu);

            }
        }


    }
}