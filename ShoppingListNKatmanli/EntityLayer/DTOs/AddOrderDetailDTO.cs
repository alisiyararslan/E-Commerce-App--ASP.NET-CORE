using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class AddOrderDetailDTO
    {        
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public static implicit operator AddOrderDetailDTO(OrderDetail orderDetail)
        {
            return new AddOrderDetailDTO
            {                
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            };
        }

    }
}
