using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class AddOrderDTO
    {
        
        public int UserId { get; set; }

        public int AddressId { get; set; }

        public string Status { get; set; }


        public static implicit operator AddOrderDTO(Order order)
        {
            return new AddOrderDTO
            {               
                UserId = order.UserId,
                AddressId = order.AddressId,
                Status = order.Status,
               
            };
        }

    }
}
