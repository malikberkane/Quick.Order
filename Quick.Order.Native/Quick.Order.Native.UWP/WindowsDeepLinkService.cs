using Quick.Order.AppCore.Contracts;
using System;

namespace Quick.Order.Native.UWP
{
    public class WindowsDeepLinkService : IDeepLinkService
    {
        public string CreateDeepLinkUrl(string restaurantId)
        {
            return $"https://malikberkane.page.link/?link=http://quickorder/?id={restaurantId}/&apn=com.malikberkane.quickorder";
        }


        public string ExtractRestaurantIdFromUri(Uri uri)
        {
            return uri.Query.Replace("?link=http://quickorder/?id=", "").Replace("/&apn=com.malikberkane.quickorder", "");
        }
    }
}