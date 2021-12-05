using Quick.Order.AppCore.Contracts;
using System;

namespace Quick.Order.Native.UWP
{
    public class WindowsDeepLinkService : IDeepLinkService
    {
        public string CreateDeepLinkUrl(string restaurantId)
        {
            return restaurantId;
        }

        public string ExtractRestaurantIdFromUri(Uri uri)
        {
            return null;
        }
    }
}