using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.DTOs
{
    public class TrendyLatestDTO
    {
        public List<Item> TrendyItems { get; set; }
        public List<Item> LatestItems { get; set; }
    }
}
