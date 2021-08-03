using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels.Modal
{
    public class EditItemInBasketPageModel: ModalPageModelBase<BasketItem, EditItemInBasketModalResult>
    {
        public ICommand DeleteItemFromBasketCommand { get; set; }

        public ICommand EditBasketItemCommand { get; set; }

        public BasketItem BasketItem { get; set; }

        public EditItemInBasketPageModel()
        {
            EditBasketItemCommand = CreateAsyncCommand(EditItemToBasket);
            DeleteItemFromBasketCommand = CreateAsyncCommand(DeleteBasketItem);

        }

        private Task EditItemToBasket()
        {
            
          return SetResult(new EditItemInBasketModalResult { BasketItem=BasketItem});
           

        }

        private Task DeleteBasketItem()
        {

            return SetResult(new EditItemInBasketModalResult { BasketItem = BasketItem, NeedToBeDeleted=true});


        }

       

        protected override void PostParamInitialization()
        {
            BasketItem = Parameter.Clone();
        }


    }

    public class EditItemInBasketModalResult
    {
        public BasketItem BasketItem { get; set; }

        public bool NeedToBeDeleted { get; set; }
    }

}
