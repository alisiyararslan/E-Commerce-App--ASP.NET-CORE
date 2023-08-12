using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAccessLayer.Migrations;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IItemService _itemService;

        public OrderDetailsController(IOrderService orderService, IOrderDetailService orderDetailService, IItemService itemService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _itemService = itemService;
        }


        [HttpGet]
        public List<GetOrderDetailDTO> GetOrderDetails()
        {
            List<EntityLayer.Concrete.OrderDetail> orderDetails = _orderDetailService.GetListAll();

            List<GetOrderDetailDTO> orderDetailDTOs = orderDetails.Select(orderDetail => new GetOrderDetailDTO
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            }).ToList();

            return orderDetailDTOs;
        }


        [HttpGet("get")]
        public OrderDetail GetOrderDetail(int id)
        {
            var orderList = _orderDetailService.GetElementById(id);

            if (orderList == null)
            {
                throw new Exception("NotFound");
            }

            return orderList;
        }
        [HttpGet("orderId/{id:int}")]
        public List<GetOrderDetailDTO> GetOrderDetailsByOrderId(int id)
        {
            List<OrderDetail> orderDetails = _orderDetailService.GetOrderDetailsByOrderId(id);

            List<GetOrderDetailDTO> orderDetailDTOs = orderDetails.Select(orderDetail => new GetOrderDetailDTO
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            }).ToList();

            return orderDetailDTOs;
        }

        [HttpPost("addCopiedOrderDetail")]
        public async Task<ActionResult<AddOrderDetailDTO>> AddCopiedOrderDetail(AddOrderDetailDTO orderDetail)
        {
            _orderDetailService.Insert(new OrderDetail()
            {
                OrderId = orderDetail.OrderId,
                ItemId = orderDetail.ItemId,
                Amount = orderDetail.Amount,
                UnitPrice = orderDetail.UnitPrice,
                LineTotal = orderDetail.LineTotal,
            });

            return orderDetail;
        }

        [HttpPost("addOrderDetail")]
        public async Task<ActionResult<AddOrderDetailDTO>> AddOrderDetail(int itemID,int userId)
        {
            bool isItemIn = false;
            OrderDetail existingOrderDetail = null;
            
            var order = _orderService.GetMainOrderByIsMain(userId);
            if (order == null)
            {
                return NotFound();
            }

            var orderDetailList = _orderDetailService.GetOrderDetailsByOrderId(order.Id);
            foreach (var orderDetail in orderDetailList)
            {
                if (orderDetail.ItemId == itemID)
                {
                    existingOrderDetail = orderDetail;
                    isItemIn = true;
                    break;
                }
            }
            var item = _itemService.GetElementById(itemID);
            if (item == null)
            {
                return NotFound();
            }

            var orderDetailDTO = new AddOrderDetailDTO();

            orderDetailDTO.OrderId = order.Id;
            orderDetailDTO.ItemId = item.Id;
            orderDetailDTO.Amount = 1;
            orderDetailDTO.UnitPrice = item.Price * (100-item.Discount)/100;
            orderDetailDTO.LineTotal = orderDetailDTO.Amount * orderDetailDTO.UnitPrice;

            if (isItemIn)
            {

                UpdateOrderDetailPlus(existingOrderDetail.Id);
                order.TotalPrice += existingOrderDetail.LineTotal;

            }


            else
            {
                
              

                _orderDetailService.Insert(new OrderDetail()
                {
                    OrderId = order.Id,
                    ItemId = item.Id,
                    Amount = orderDetailDTO.Amount,
                    UnitPrice = item.Price * (100 - item.Discount) / 100,
                    LineTotal = orderDetailDTO.Amount * item.Price * (100 - item.Discount) / 100
            });

                order.TotalPrice += orderDetailDTO.LineTotal;
                _orderService.Update(order);
            }



            return orderDetailDTO;
        }

        [HttpGet("updateplus")]
        public async Task<Order> UpdateOrderDetailPlus(int id)
        {

            if (ModelState.IsValid)
            {
                var orderDetailToUpdate = _orderDetailService.GetElementById(id);
                if (orderDetailToUpdate == null)
                {
                    return null;
                }
                var order = _orderService.GetElementById(orderDetailToUpdate.OrderId);


                orderDetailToUpdate.Amount++;
                orderDetailToUpdate.LineTotal = orderDetailToUpdate.UnitPrice * orderDetailToUpdate.Amount;
                order.TotalPrice += orderDetailToUpdate.UnitPrice;
                _orderService.Update(order);
                _orderDetailService.Update(orderDetailToUpdate);

                return order;
            }
            else
            {
                return null;
            }
        }


        [HttpGet("updateminus")]
        public async Task<Order> UpdateOrderDetailMinus(int id)
        {

            if (ModelState.IsValid)
            {
                var orderDetailToUpdate = _orderDetailService.GetElementById(id);
                if (orderDetailToUpdate == null || orderDetailToUpdate.Amount == 0)
                {
                    return null;
                }
                var order = _orderService.GetElementById(orderDetailToUpdate.OrderId);

                orderDetailToUpdate.Amount--;
                orderDetailToUpdate.LineTotal = orderDetailToUpdate.UnitPrice * orderDetailToUpdate.Amount;
                order.TotalPrice -= orderDetailToUpdate.UnitPrice;
                _orderService.Update(order);
                _orderDetailService.Update(orderDetailToUpdate);

                return order;


            }
            else
            {
                return null;
            }
        }

        //
        [HttpDelete("deleteOrderDetail")]
        public async Task<IActionResult> DeleteOrderDetail(int orderdetailID)
        {

            var orderDetail = _orderDetailService.GetElementById(orderdetailID);
            if (orderDetail == null)
            {
                return NotFound();
            }

            var order = _orderService.GetElementById(orderDetail.OrderId);

            order.TotalPrice -= orderDetail.LineTotal;

            _orderService.Update(order);

            _orderDetailService.Delete(orderDetail);


            return Ok("Order deleted successfully");
        }
    }
}