using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class MainOrderDTO
    {
        public int Id { get; set; }

        public bool IsMain { get; set; }

        public static implicit operator MainOrderDTO(Order order)
        {
            return new MainOrderDTO
            {
                Id = order.Id,
                IsMain = order.IsMain,

            };
        }
    }
}
