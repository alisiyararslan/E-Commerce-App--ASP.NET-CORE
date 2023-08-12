using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class FavoriteItemUserDTO
    {
        public int? UserId { get; set; }

        public int? ItemId { get; set; }

        public static explicit operator FavoriteItemUserDTO(FavoriteItemUser favoriteItemUser)
        {
            return new FavoriteItemUserDTO
            {
                UserId = favoriteItemUser.UserId,
                ItemId = favoriteItemUser.ItemId,

            };
        }
    }
}
