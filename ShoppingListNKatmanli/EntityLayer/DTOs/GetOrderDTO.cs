using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetOrderDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }

        public string Status { get; set; }

        public string ShareCode { get; set; }

        public DateTime CreatedDate { get; set; }
        

        public decimal TotalPrice { get; set; }

        public static implicit operator GetOrderDTO(Order order)
        {
            return new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                Status = order.Status,
                ShareCode = order.ShareCode,
                CreatedDate = order.CreatedDate,
                TotalPrice = order.TotalPrice
            };
        }

        public static implicit operator Order(GetOrderDTO order)
        {
            return new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                Status = order.Status,
                ShareCode = order.ShareCode,
                CreatedDate = order.CreatedDate,
                TotalPrice = order.TotalPrice
            };
        }

    }
}
