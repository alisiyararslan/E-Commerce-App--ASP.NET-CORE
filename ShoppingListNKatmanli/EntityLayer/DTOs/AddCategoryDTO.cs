using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class AddCategoryDTO
    {
        public string Name { get; set; }

        public static implicit operator AddCategoryDTO(Category category)
        {
            return new AddCategoryDTO
            {
                Name = category.Name
            };
        }

    }
}
