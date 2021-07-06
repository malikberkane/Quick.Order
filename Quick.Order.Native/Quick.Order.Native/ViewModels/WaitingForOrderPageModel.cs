using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using System;
using System.Threading.Tasks;

namespace Quick.Order.Native.ViewModels
{
    public class WaitingForOrderPageModel : PageModelBase<AppCore.Models.Order>
    {
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;

        public AppCore.Models.Order Order { get; set; }
        public WaitingForOrderPageModel(FrontOfficeRestaurantService frontOfficeRestaurantService)
        {
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
        }


        public override Task InitAsync()
        {
            Order = Parameter;
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(20);

            var timer = new System.Threading.Timer(async (e) =>
            {
                await EnsurePageModelIsInLoadingState(async () =>
                {
                    Order = await frontOfficeRestaurantService.GetOrderStatuts(Order);

                });
            }, null, startTimeSpan, periodTimeSpan);

            return Task.CompletedTask;
        }
    }




}