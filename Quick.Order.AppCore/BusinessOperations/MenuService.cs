using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class MenuService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IMenuRepository menuRepository;
        private readonly BackOfficeSessionService backOfficeSessionService;
        public MenuService(IRestaurantRepository restaurantRepository, IMenuRepository menuRepository, BackOfficeSessionService backOfficeSessionService)
        {
            this.restaurantRepository = restaurantRepository;
            this.menuRepository = menuRepository;
            this.backOfficeSessionService = backOfficeSessionService;
        }
        public async Task AddMenu(Menu menu)
        {

            await menuRepository.Add(menu);

        }

        public async Task UpdateMenu(Menu menu)
        {

            await menuRepository.Update(menu);

        }

        public  Task<Menu> GetMenu(string restaurantId)
        {

            return menuRepository.GetById(Guid.Parse(restaurantId));

        }
        //public async Task AddDish(Dish dish, Menu menu)
        //{
        //    if (menu.Dishes.Any(d => d.Name == dish.Name))
        //    {
        //        throw new InvalidDishException("Dish with the same name already in menu");
        //    }

        //    restaurant.Dishes.Add(new DishSection() { Name = "Plats" });
        //}
        //private async Task AddDishCore(Dish dish, Restaurant restaurant)
        //{
        //    if (restaurant.Dishes.Any(d => d.Name == dish.Name))
        //    {
        //        throw new InvalidDishException("Dish with the same name already in menu");
        //    }

        //    restaurant.Dishes.Add(new DishSection() { Name="Plats"});

        //    await restaurantRepository.Update(restaurant);
        //}

    }
}
