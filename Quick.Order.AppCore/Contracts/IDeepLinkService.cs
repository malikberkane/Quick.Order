using System;

namespace Quick.Order.AppCore.Contracts
{
    public interface IDeepLinkService
    {
        string CreateDeepLinkUrl(string restaurantId);
        string ExtractRestaurantIdFromUri(Uri uri);
    }
}
