using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetFavoriteItemUserDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ItemId { get; set; }

        public static explicit operator GetFavoriteItemUserDTO(FavoriteItemUser favoriteItemUser)
        {
            return new GetFavoriteItemUserDTO
            {
                Id = favoriteItemUser.UserId,
                UserId = favoriteItemUser.UserId,
                ItemId = favoriteItemUser.ItemId
            };
        }
    }
}
