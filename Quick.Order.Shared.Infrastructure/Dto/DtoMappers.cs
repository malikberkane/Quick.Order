using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;

namespace Quick.Order.Shared.Infrastructure.Dto
{
    public static class DtoMappers
    {
        public static RestaurantDto ModelToDto(this Restaurant model)
        {
            return new RestaurantDto
            {
                Id = model.Id,
                Name = model.Name,
                Adresse = model.Adresse,
                RestaurantPhotoSource=model.RestaurantPhotoSource,
                IsOpen = model.IsOpen,
                Administrator = model.Administrator.ModelToDto(),
                Menu = model.Menu.ModelToDto()
            };
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


        public static MenuDto ModelToDto(this Menu model)
        {
            var dto = new MenuDto
            {
                Sections = new List<DishSectionDto>()
            };

            if (model.Sections != null)
            {
                foreach (var item in model.Sections)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    dto.Sections.Add(item.ModelToDto());
                }
            }



            return dto;
        }




        public static DishSectionDto ModelToDto(this DishSection model)
        {
            var dto = new DishSectionDto
            {
                Name = model.Name,
                Dishes = new List<DishDto>()
            };

            var modelDishes = model.GetDishes();

            if (modelDishes != null)
            {
                foreach (var item in modelDishes)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    dto.Dishes.Add(item.ModelToDto());
                }
            }


            return dto;
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
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            return new Dish()
            {
                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price


            };
        }

        public static DishSection DtoToModel(this DishSectionDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            var dishes = new List<Dish>();

            if (dto.Dishes != null)
            {
                foreach (var item in dto.Dishes)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    dishes.Add(item.DtoToModel());
                }
            }

            return new DishSection(dto.Name, dishes);
        }
        public static Menu DtoToModel(this MenuDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            var sections = new List<DishSection>();

            if (dto.Sections != null)
            {
                foreach (var item in dto.Sections)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    sections.Add(item.DtoToModel());
                }
            }

            return new Menu(sections);
        }

        public static Restaurant DtoToModel(this RestaurantDto dto)
        {
            return new Restaurant(dto.Name, dto.Adresse, dto.Menu.DtoToModel(),dto.Administrator.DtoToModel(),dto.RestaurantPhotoSource,dto.Id);
        }
        public static RestaurantAdmin DtoToModel(this RestaurantAdminDto dto)
        {
            return new RestaurantAdmin
            {
                Email = dto.Email,
                Name = dto.Name,
            };
        }

    }
}
