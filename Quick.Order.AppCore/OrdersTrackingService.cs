﻿using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick.Order.AppCore
{
    public class OrdersTrackingService
    {

        private readonly BackOfficeRestaurantService backOfficeService;
        private readonly IOrdersRepository ordersRepository;
        private readonly BackOfficeSessionService backOfficeSessionService;
        private readonly ILoggerService loggerService;
        private readonly IConnectivityService connectivityService;

        public OrdersTrackingService(BackOfficeRestaurantService backOfficeService, IOrdersRepository ordersRepository, BackOfficeSessionService backOfficeSessionService, ILoggerService loggerService, IConnectivityService connectivityService)
        {
            this.backOfficeService = backOfficeService;
            this.ordersRepository = ordersRepository;
            this.backOfficeSessionService = backOfficeSessionService;
            this.loggerService = loggerService;
            this.connectivityService = connectivityService;
        }
        public event OrdersChangedEventHandler OrderListChanged;


        public delegate void OrdersChangedEventHandler(object sender, OrdersChangedEventArgs args);

        public IEnumerable<AppCore.Models.Order> CurrentOrders { get; set; } = new List<AppCore.Models.Order>();
        public void StartOrdersTracking()
        {

            ordersRepository.OrderAddedOrDeleted += OrdersRepository_OrderAddedOrDeleted;
            ordersRepository.StartOrdersObservation(backOfficeSessionService.CurrentRestaurantSession.Id);

        }

        private async void OrdersRepository_OrderAddedOrDeleted(object source, OrdersEventArgs e)
        {
            await CheckForNewOrders();
        }

        private async Task CheckForNewOrders()
        {

            try
            {
                if (!connectivityService.HasNetwork() || backOfficeSessionService.CurrentRestaurantSession==null)
                {
                    return;
                }

                var upToDateOrdersList = await backOfficeService.GetOrdersForRestaurant(backOfficeSessionService.CurrentRestaurantSession.Id);
                var comparison = CompareNewItems(upToDateOrdersList);
                if (!comparison.AreSame)
                {
                    CurrentOrders = upToDateOrdersList;
                    OrderListChanged.Invoke(this, new OrdersChangedEventArgs { ListDifferences = comparison });
                }
            }
            catch (Exception ex)
            {

                loggerService.Log(ex);
            }


        }

        public void StopOrderTracking()
        {
            ordersRepository.OrderAddedOrDeleted -= OrdersRepository_OrderAddedOrDeleted;
        }

        public ListDifferences CompareNewItems(IEnumerable<AppCore.Models.Order> upToDateList)
        {
            return new ListDifferences
            {
                NewItems = upToDateList.Except(CurrentOrders, new OrderEqualityComparer()).ToList(),
                RemovedItems = CurrentOrders.Except(upToDateList, new OrderEqualityComparer()).ToList()

            };
        }


    }

    public class ListDifferences
    {
        public bool AreSame => !NewItems.Any() && !RemovedItems.Any();

        public List<AppCore.Models.Order> NewItems { get; set; }

        public List<AppCore.Models.Order> RemovedItems { get; set; }

    }

    public class OrdersChangedEventArgs : EventArgs
    {
        public ListDifferences ListDifferences { get; set; }
    }


}
