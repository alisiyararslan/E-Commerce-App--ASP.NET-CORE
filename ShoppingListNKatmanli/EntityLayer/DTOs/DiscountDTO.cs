using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class DiscountDTO
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string DiscountTitle { get; set; }
        public string DiscountContent { get; set; }
        public string DiscountCode { get; set; }
    }
}
