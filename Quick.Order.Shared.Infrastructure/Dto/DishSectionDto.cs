using System.Collections.Generic;

namespace Quick.Order.Shared.Infrastructure.Dto
{
    public class DishSectionDto
    {
        public List<DishDto> Dishes { get; set; } = new List<DishDto>();
        public string Name { get; set; }
    }
}
