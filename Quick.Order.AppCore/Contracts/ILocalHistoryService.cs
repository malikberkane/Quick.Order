﻿using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts
{
    public interface ILocalHistoryService
    {
        (string id, DateTime orderDate) GetLocalPendingOrder();
        void AddLocalPendingOrder(AppCore.Models.Order order);

        void RemoveLocalPendingOrder();
        Models.Order GetLocalOrder();
        void SaveLocalOrder(Models.Order order);
        void DeleteLocalOrder();
        void SetAppCulture(CultureInfo cultureInfo);
        CultureInfo GetSavedAppCulture();
        void SetUserEmail(string userEmail);
        string GetUserEmail();
    }
}
