using System;
using System.Text;

namespace Quick.Order.Shared.Infrastructure.Dto
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get;  set; }
        public string Adresse { get;  set; }
        public bool IsOpen { get; set; }

        public CurrencyDto Currency { get; set; }

        public string RestaurantPhotoSource { get; set; } = "menuheader.jpg";

        public RestaurantAdminDto Administrator { get; set; }

        public MenuDto Menu { get; set; }

    }
}
