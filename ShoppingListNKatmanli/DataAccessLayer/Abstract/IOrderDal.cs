using EntityLayer.Concrete;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IOrderDal : IGenericDal<Order>
    {
        List<Order> GetOrdersByUserId(int userId);

        Order GetOrderByShareCode(string shareCode);

        Order GetMainOrderByIsMain(int userId);
    }
}
