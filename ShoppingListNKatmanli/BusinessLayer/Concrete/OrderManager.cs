using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using DataAccessLayer.Abstract;
using EntityLayer.DTOs;

namespace BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }
        public void Delete(Order t)
        {
            _orderDal.Delete(t);
        }

        public Order GetElementById(int id)
        {
            return _orderDal.GetElementById(id);
        }

        public List<Order> GetListAll()
        {
            return _orderDal.GetListAll();
        }

        public void Insert(Order t)
        {
            _orderDal.Insert(t);
        }

        public void Update(Order t)
        {
            _orderDal.Update(t);
        }
        public List<Order> GetOrdersByUserId(int userId)
        {
            List<Order> orders = _orderDal.GetOrdersByUserId(userId);

            return orders;
        }

        public Order GetOrderByShareCode(string shareCode)
        {
            Order order = _orderDal.GetOrderByShareCode(shareCode);

            return order;
        }
        public Order GetMainOrderByIsMain(int userId)
        {
            Order order = _orderDal.GetMainOrderByIsMain(userId);

            return order;
        }
    }
}
