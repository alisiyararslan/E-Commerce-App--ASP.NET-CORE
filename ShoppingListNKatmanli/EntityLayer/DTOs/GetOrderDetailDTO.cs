using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class GetOrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public static implicit operator GetOrderDetailDTO(OrderDetail orderDetail)
        {
            return new GetOrderDetailDTO
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            };
        }

    }
}
