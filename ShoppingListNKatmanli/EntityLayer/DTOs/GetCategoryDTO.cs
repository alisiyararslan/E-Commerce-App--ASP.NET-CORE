using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetCategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static implicit operator GetCategoryDTO(Category category)
        {
            return new GetCategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
