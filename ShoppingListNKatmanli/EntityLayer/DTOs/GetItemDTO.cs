using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetItemDTO
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int CategoryDetailId { get; set; }

        public int UserId { get; set; }

        public int FavoriteCount { get; set; }

        public string Title { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public int Discount { get; set; }

        public static implicit operator GetItemDTO(Item item)
        {
            return new GetItemDTO()
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Image = item.Image,
                Description = item.Description,
                Discount = item.Discount,
            };
        }
    }
}
