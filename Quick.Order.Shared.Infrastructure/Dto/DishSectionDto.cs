using System.Collections.Generic;

namespace Quick.Order.Shared.Infrastructure.Dto
{
    public class DishSectionDto
    {
        public List<DishDto> Dishes { get; set; }
        public string Name { get; set; }
    }
}
