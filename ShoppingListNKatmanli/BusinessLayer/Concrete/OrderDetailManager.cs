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
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailDal;

        public OrderDetailManager(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }
        public void Delete(OrderDetail t)
        {
            _orderDetailDal.Delete(t);
        }

        public OrderDetail GetElementById(int id)
        {
            return _orderDetailDal.GetElementById(id);
        }

        public List<OrderDetail> GetListAll()
        {
            return _orderDetailDal.GetListAll();
        }

        public void Insert(OrderDetail t)
        {
            _orderDetailDal.Insert(t);
        }

        public void Update(OrderDetail t)
        {
            _orderDetailDal.Update(t);
        }
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _orderDetailDal.GetOrderDetailsByOrderId(orderId);
        }
    }
}
