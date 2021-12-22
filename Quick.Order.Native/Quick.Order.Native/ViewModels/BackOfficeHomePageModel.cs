using Quick.Order.AppCore.Models;
using Quick.Order.AppCore.Resources;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace Quick.Order.Native.ViewModels
{
    public class BackOfficeHomePageModel : ExtendedPageModelBase
    {

        public ObservableCollection<Restaurant> Items { get; }

        public ObservableCollection<OrderVm> Orders { get; set; } = new ObservableCollection<OrderVm>();

        public RestaurantAdmin CurrentLoggedAccount { get; set; }
        public ICommand LogoutCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand SelectCultureCommand { get; }
        public ICommand GoToOrderDetailsCommand { get; }
        public ICommand GenerateQrCodeCommand { get; }
        public ICommand EditOrderStatusCommand { get; }
        public ICommand PickRestaurantPictureCommand { get; }

        public ICommand TakeRestaurantPictureCommand { get; }

        public DishSectionGroupedModelCollection MenuGroupedBySection { get; set; } = new DishSectionGroupedModelCollection();


        //This field is only here to serve
        //as binding source for collection view items source so that it works on xamarin.forms 5 +
        public DishSectionGroupedModelList MenuGroupedBySectionList { get; set; } = new DishSectionGroupedModelList();

        public BackOfficeHomePageModel()
        {
            Items = new ObservableCollection<Restaurant>();
            GenerateQrCodeCommand = CreateOfflineAsyncCommand(GoToQrCodeGeneration);
            LogoutCommand = CreateOfflineAsyncCommand(Logout);
            AddDishCommand = CreateCommand<string>(AddDish);
            PickRestaurantPictureCommand = CreateAsyncCommand(ChangeRestaurantPicture);
            ReloadMenuCommand = CreateAsyncCommand(Reload);
            AddDishSectionCommand = CreateCommand(AddDishSection);
            DeleteRestaurantCommand = CreateCommand(PromptDeleteCurrentRestaurant);
            EditSectionCommand = CreateCommand<string>(Add0rEditDishSection);
            GoToEditRestaurantInfosCommand = CreateCommand(GoToEditRestaurantInfos);
            GoToEditDishCommand = CreateCommand<Dish>(EditDish);
            AddItemCommand = CreateCommand(AddRestaurant);
            SelectCultureCommand = CreateOfflineCommand(SelectCulture);
            GoToOrderDetailsCommand = CreateAsyncCommand<OrderVm>(GoToOrderDetails);
        }

        private async Task SelectCulture()
        {
            await NavigationService.BackOffice.GoToCultureChoice();

            WorkaroundToForceLocalizationXamlBindings();

        }


        private async Task ChangeRestaurantPicture()
        {
            FileResult photo = null;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                photo = await MediaPicker.PickPhotoAsync();


            });

            if (photo != null)
            {
                await UpdateRestaurantPhoto(photo);

            }

        }

        private async Task UpdateRestaurantPhoto(FileResult photo)
        {
            if (await photo.OpenReadAsync() != null)
            {
                var url = await ServicesAggregate.Plugin.ImageService.SaveImage($"photo_{CurrentLoggedAccount.UserId}", await photo.OpenReadAsync());
                CurrentRestaurant.RestaurantPhotoSource = url;
                await ServicesAggregate.Business.BackOffice.UpdateRestaurant(CurrentRestaurant);

                
                OnPropertyChanged(nameof(CurrentRestaurant));
            }
        }



        private Task AddDishSection()
        {
            return Add0rEditDishSection(sectionName: null);
        }

        private Task GoToQrCodeGeneration()
        {
            return NavigationService.BackOffice.GoToQrGeneration(CurrentRestaurant);
        }

        private Task GoToOrderDetails(OrderVm order)
        {
            return NavigationService.BackOffice.GoToOrderDetails(MappingService.VmToModel(order));
        }

      
        public override Task InitAsync()
        {
            CurrentLoggedAccount = ServicesAggregate.Business.Authentication.LoggedUser?.RestaurantAdmin;
            CurrentRestaurant = ServicesAggregate.Business.BackOfficeSession.CurrentRestaurantSession;

            if (CurrentRestaurant?.Menu != null)
            {
                CreateMenuList(CurrentRestaurant.Menu);


            }

            if (CurrentRestaurant != null)
            {
                StartOrderTracking();

            }

            return Task.CompletedTask;

        }

        private void StartOrderTracking()
        {

            ServicesAggregate.Repositories.Orders.StartOrdersObservation(CurrentRestaurant.Id);
            ServicesAggregate.Repositories.Orders.OrderAddedOrDeleted += OrdersCrudEventHandler;
        }

        private void OrdersCrudEventHandler(object source, AppCore.Contracts.Repositories.OrdersEventArgs e)
        {
            if (Orders == null)
            {
                Orders = new ObservableCollection<OrderVm>();
            }

            var orderVm = e.Order.ModelToVm();
            var orderIndex = Orders.IndexOf(orderVm);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (orderIndex != -1)
                {
                    if (e.IsDeleted)
                    {
                        Orders.RemoveAt(orderIndex);
                    }
                    else
                    {
                        Orders[orderIndex] = orderVm;

                    }
                }
                else if (!e.IsDeleted)
                {
                    Orders.Insert(0, orderVm);
                }
            });
            

            
        }

        private async Task Reload()
        {
            if (CurrentRestaurant != null)
            {
                CurrentRestaurant = await ServicesAggregate.Repositories.Restaurants.GetById(CurrentRestaurant.Id);
                if (CurrentRestaurant?.Menu != null)
                {
                    CreateMenuList(CurrentRestaurant.Menu);
                }
            }

        }
        public override Task CleanUp()
        {
            MessagingService.Unsubscribe<OrderStatusEditionResult>("OrderStatusEdited", this);
            ServicesAggregate.Repositories.Orders.OrderAddedOrDeleted -= OrdersCrudEventHandler;
            ServicesAggregate.Repositories.Orders.StopOrdersObservation();
            return Task.CompletedTask;
        }


        private async Task Logout()
        {
            await this.CleanUp();
            await ServicesAggregate.Business.Authentication.SignOut();
            await NavigationService.SignIn.GoToLanding();
        }


        public ICommand AddDishCommand { get; set; }
        public ICommand ReloadMenuCommand { get; }
        public ICommand AddDishSectionCommand { get; set; }

        public ICommand DeleteRestaurantCommand { get; set; }
        public ICommand EditSectionCommand { get; }
        public ICommand GoToEditRestaurantInfosCommand { get; set; }

        public ICommand GoToEditDishCommand { get; set; }


        private void PopulateList()
        {
            if (DeviceInfo.Platform != DevicePlatform.iOS || MenuGroupedBySection==null)
            {
                return;
            }
            var newInstance = new DishSectionGroupedModelList();

            foreach (var item in MenuGroupedBySection)
            {
                newInstance.Add(item);
            }

            MenuGroupedBySectionList = newInstance;

        }
        private async Task Add0rEditDishSection(string sectionName)
        {
            var dishSectionToEdit = sectionName != null ? CurrentRestaurant.Menu.GetDishSectionByName(sectionName) : null;

            var addOrEditDishParams = new EditDishSectionParams() { Restaurant = CurrentRestaurant, DishSectionToEdit = dishSectionToEdit };

            var editDishResult = await NavigationService.BackOffice.GoToAddOrEditDishSection(addOrEditDishParams);
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

               PopulateList();


            }
        }

        private async Task EditDish(Dish dishToEdit)
        {
            var result = await NavigationService.BackOffice.GoToEditDish(new EditDishParams { Dish = dishToEdit, Restaurant = CurrentRestaurant });

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

           PopulateList();

        }

        private async Task GoToEditRestaurantInfos()
        {
            var restaurantEdited = await NavigationService.BackOffice.GoToEditRestaurantInfos(new Modal.RestaurantIdentity { Adresse = CurrentRestaurant.Adresse, Name = CurrentRestaurant.Name, Currency = CurrentRestaurant.Currency });

            if (restaurantEdited != null && restaurantEdited.IsValid())
            {
                CurrentRestaurant.EditIdentity(restaurantEdited.Name, restaurantEdited.Adresse, restaurantEdited.Currency);


                await EnsurePageModelIsInLoadingState(async () =>
                {
                    await ServicesAggregate.Business.BackOffice.UpdateRestaurant(CurrentRestaurant);

                    await Reload();
                });
            }
        }


        private async Task AddDish(string sectionName)
        {

            var section = CurrentRestaurant.Menu.GetDishSectionByName(sectionName);

            var navParams = new AddDishParams { Restaurant = CurrentRestaurant, Section = section };

            var result = await NavigationService.BackOffice.GoToAddDish(navParams);

            if (result != null && result.WasSuccessful)
            {
                if (result.AddedDish != null)
                {
                    MenuGroupedBySection.AddDishToSection(section.Name, result.AddedDish);
                }

            }

            PopulateList();

        }

        private async Task PromptDeleteCurrentRestaurant()
        {
            if (await NavigationService.Common.PromptForConfirmation(AppResources.RestaurantDeletionPrompt, AppResources.Delete, AppResources.Cancel))
            {
                await EnsurePageModelIsInLoadingState(DeleteCurrentRestaurant);

            }


        }

        private async Task DeleteCurrentRestaurant()
        {
            await ServicesAggregate.Business.BackOffice.DeleteRestaurant(CurrentRestaurant);

            CurrentRestaurant = null;
            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }

            AlertUserService.ShowSnack(AppResources.RestaurantWasDeleted);
        }

        public Restaurant CurrentRestaurant { get; set; }


        private void CreateMenuList(Menu menu)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var upToDateMenu = new DishSectionGroupedModelCollection();

                foreach (var section in menu.Sections)
                {
                    var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                    foreach (var dish in section.GetDishes())
                    {
                        newSection.Add(dish);
                    }

                    upToDateMenu.Add(newSection);
                }

                MenuGroupedBySection = upToDateMenu;


                PopulateList();
            });
        }

        private async Task AddRestaurant()
        {
            var editedRestaurant = await NavigationService.BackOffice.GoToRestaurantEdition();

            if (editedRestaurant != null)
            {
                CurrentRestaurant = editedRestaurant;
                CreateMenuList(editedRestaurant.Menu);

                StartOrderTracking();
            }
        }



        private void WorkaroundToForceLocalizationXamlBindings()
        {
            if (Orders == null)
            {
                return;
            }
            foreach (var item in Orders)
            {
                item.RaiseStatusPropertyChanged();

            }
        }


    }
}