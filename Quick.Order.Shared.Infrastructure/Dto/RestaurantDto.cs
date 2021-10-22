using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quick.Order.Shared.Infrastructure.Dto
{
    public class RestaurantDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get;  set; }
        public string Adresse { get;  set; }
        public bool IsOpen { get; set; }

        public string RestaurantPhotoSource { get; set; } = "menuheader.jpg";

        public RestaurantAdminDto Administrator { get; set; }

        public MenuDto Menu { get; set; }

    }

    public static class Mappers
    {
        public static RestaurantDto ModelToDto(this Restaurant model)
        {
            return new RestaurantDto
            {
                Id = model.Id,
                Adresse = model.Adresse,
                IsOpen = model.IsOpen,
                Administrator = model.Administrator.ModelToDto(),
                Menu=model.Menu.ModelToDto()
            };
        }

        public static Restaurant DtoToModel(this RestaurantDto dto)
        {
            return new Restaurant(dto.Name, dto.Adresse, dto.Menu.DtoToModel());
        }


        public static RestaurantAdminDto ModelToDto(this RestaurantAdmin model)
        {
            return new RestaurantAdminDto
            {
                Email = model.Email,
                Name = model.Name,
                UserId = model.UserId
            };
        }
        public static RestaurantAdmin DtoToModel(this RestaurantAdminDto dto)
        {
            return new RestaurantAdmin
            {
                Email = dto.Email,
                Name = dto.Name,
            };
        }


        public static MenuDto ModelToDto(this Menu model)
        {
            var dto = new MenuDto
            {
                Sections = new System.Collections.Generic.List<DishSectionDto>()
            };

            foreach (var item in model.Sections)
            {
                dto.Sections.Add(item.ModelToDto());
            }

            return dto;
        }

        public static Menu DtoToModel(this MenuDto model)
        {
            return new Menu()
        }

        

        public static DishSectionDto ModelToDto(this DishSection model)
        {
            var dto = new DishSectionDto
            {
                Name = model.Name,
                Dishes = new System.Collections.Generic.List<DishDto>()
            };
            foreach (var item in model.GetDishes())
            {
                dto.Dishes.Add(item.ModelToDto());
            }

            return dto;
        }


        public static DishSection DtoToModel(this DishSectionDto dto)
        {
            var dishes = new List<Dish>();
            foreach (var item in dto.Dishes)
            {
                dishes.Add(item.DtoToModel());
            }
            return new DishSection(dto.Name, dishes);
        }


        public static DishDto ModelToDto(this Dish model)
        {
            return new DishDto()
            {
                Description = model.Description,
                Name = model.Name,
                Price = model.Price


            };
        }

        public static Dish DtoToModel(this DishDto dto)
        {
            return new Dish()
            {
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price


            };
        }

    }
}
