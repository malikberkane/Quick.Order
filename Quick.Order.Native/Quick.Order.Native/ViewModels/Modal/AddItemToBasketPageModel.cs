using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels.Modal
{
    public class AddItemToBasketPageModel : ModalPageModelBase<Dish, BasketItem>
    {

        public int Quantity { get; set; }

        public ICommand AddItemToBasketCommand { get; set; }

        public Dish Dish { get; set; }

        public AddItemToBasketPageModel()
        {
            AddItemToBasketCommand = new AsyncCommand(AddItemToBasket);

        }

      

        public override Task InitAsync()
        {
            Dish = Parameter;
            return base.InitAsync();
        }
        private Task AddItemToBasket()
        {
            if (Quantity != 0)
            {
                return SetResult(new BasketItem { Quantity = Quantity , Dish=Dish});
            }

            return SetResult(null);
           
        }


       

    }

}
