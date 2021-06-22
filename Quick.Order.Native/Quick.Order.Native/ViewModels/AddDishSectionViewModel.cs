using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishSectionViewModel : BaseViewModel
    {
        public ICommand AddDishSectionCommand { get; set; }

        public string DishSectionName { get; set; }
        public MenuService menuService { get; }

        public AppCore.Models.Menu CurrentMenu { get; set; }
        public AddDishSectionViewModel(MenuService menuService)
        {
            AddDishSectionCommand = new Command(AddDishSection);
            this.menuService = menuService;
        }

        private async void AddDishSection(object obj)
        {
            if (CurrentMenu.Dishes == null)
            {
                CurrentMenu.Dishes = new List<DishSection>();
            }
            CurrentMenu.Dishes.Add(new DishSection { Name = DishSectionName });

            await menuService.UpdateMenu(CurrentMenu);
        }
    }


}

