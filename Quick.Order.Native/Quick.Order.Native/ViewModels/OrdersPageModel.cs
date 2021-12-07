﻿using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class OrdersPageModel: ExtendedPageModelBase<AppCore.Models.Order>
    {
       

        public OrderVm Order { get; private set; }
        public ICommand EditOrderStatusCommand { get; }

        public ICommand DeleteOrderCommand { get; }

        public OrdersPageModel()
        {
            EditOrderStatusCommand = CreateCommand(EditOrderStatus);
            DeleteOrderCommand = CreateCommand(DeleteOrder);

        }




        private async Task EditOrderStatus()
        {

            var result = await NavigationService.BackOffice.GoToEditOrderStatus(Order.VmToModel());

            if (result != null && result.WasSuccessful)
            {
                Order.OrderStatus = result.ValidatedStatus;
            }


        }

        private async Task DeleteOrder()
        {
            await  ServicesAggregate.Business.BackOffice.DeleteOrder(Parameter);
            await NavigationService.Common.GoBack();
           
        }


        protected override void PostParamInitialization()
        {
            Order = Parameter.ModelToVm();
        }

    }
}
