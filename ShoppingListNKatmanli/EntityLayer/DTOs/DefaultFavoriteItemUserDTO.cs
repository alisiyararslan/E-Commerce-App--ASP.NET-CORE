using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class DefaultFavoriteItemUserDTO
    {
        public int UserId { get; set; }

        public int ItemId { get; set; }

        public static explicit operator DefaultFavoriteItemUserDTO(FavoriteItemUser favoriteItemUser)
        {
            return new DefaultFavoriteItemUserDTO
            {
                UserId = favoriteItemUser.UserId,
                ItemId = favoriteItemUser.ItemId,
                
            };
        }
    }
}
