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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        
        [HttpGet]
        public List<GetOrderDTO> GetOrders()
        {
            List<EntityLayer.Concrete.Order> baskets = _orderService.GetListAll();

            List<GetOrderDTO> orderDTOs = baskets.Select(order => new GetOrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                Status = order.Status,
                ShareCode = order.ShareCode,
                CreatedDate = order.CreatedDate,
                TotalPrice = order.TotalPrice
            }).ToList();

            return orderDTOs;
        }

        
        [HttpGet("get")]
        public EntityLayer.Concrete.Order GetOrder(int id)
        {
            var order = _orderService.GetElementById(id);

            if (order == null)
            {
                throw new Exception("NotFound");
            }

            return order;
        }

        [HttpGet("getOrderIdByShareCode")]
        public int GetOrderByShareCode(string shareCode)
        {
            var order = _orderService.GetOrderByShareCode(shareCode);

            if (order == null)
            {
                throw new Exception("NotFound");
            }

            return order.Id;
        }
        [HttpGet("getOrderWithShareCode")]
        public Order GetOrderWithShareCode(string shareCode)
        {
            var order = _orderService.GetOrderByShareCode(shareCode);

            if (order == null)
            {
                throw new Exception("NotFound");
            }

            return order;
        }

        [HttpGet("getOrdersByUserId")]
        public List<Order> GetOrdersByUserId(int userId)
        {
            List<Order> baskets = _orderService.GetOrdersByUserId(userId);

            List<Order> orderByUserId = baskets.Select(order => new Order
            {
                Id = order.Id,
                UserId = order.UserId,
                AddressId = order.AddressId,
                Status = order.Status,
                ShareCode = order.ShareCode,
                CreatedDate = order.CreatedDate,
                TotalPrice = order.TotalPrice
            }).ToList();

            return orderByUserId;
        }

        [HttpGet("getMainOrder")]
        public Order GetMainOrder(int userId)
        {
            var order = _orderService.GetMainOrderByIsMain(userId);

            if (order == null)
            {
                return null;
            }

            return order;
        }

        [HttpPost("addOrder")]
        public async Task<ActionResult<Order>> AddOrder(AddOrderDTO order)
        {
            if (GetOrdersByUserId(order.UserId).Count == 0)
            {
                var newOrder = new EntityLayer.Concrete.Order()
                {
                    UserId = order.UserId,
                    AddressId = order.AddressId,
                    Status = order.Status,
                    ShareCode = RandomString(10),
                    CreatedDate = DateTime.Now,
                    TotalPrice = 0
                };

                newOrder.IsMain = true;

                _orderService.Insert(newOrder);

                return newOrder;
            }

            else
            {
                var newOrder = new EntityLayer.Concrete.Order()
                {
                    UserId = order.UserId,
                    AddressId = order.AddressId,
                    Status = order.Status,
                    ShareCode = RandomString(10),
                    CreatedDate = DateTime.Now,
                    TotalPrice = 0
                };

                _orderService.Insert(newOrder);

                return newOrder;
            }

        }

        [HttpPut("updateOrder")]
        public async Task<IActionResult> UpdateOrder(GetOrderDTO dto)
        {

            if (ModelState.IsValid)
            {
                var orderToUpdate = _orderService.GetElementById(dto.Id);
                if (orderToUpdate == null)
                {
                    return NotFound();
                }

                orderToUpdate.Status = dto.Status;
                //orderToUpdate.AddressId = dto.AddressId;

                _orderService.Update(orderToUpdate);

                return Ok("Address successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }


        [HttpPut("SavedOrder")]
        public async Task<IActionResult> SavedOrder(GetOrderDTO order)
        {

            if (ModelState.IsValid)
            {
                var orderToUpdate = _orderService.GetElementById(order.Id);
                if (orderToUpdate == null)
                {
                    return NotFound();
                }

                orderToUpdate.Status = order.Status;
                orderToUpdate.TotalPrice = order.TotalPrice;
                //orderToUpdate.AddressId = dto.AddressId;

                _orderService.Update(orderToUpdate);

                return Ok("Order successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }



        [HttpDelete("deleteOrder")]
        public async Task<IActionResult> DeleteOrder(int orderID)
        {
            var order = _orderService.GetElementById(orderID);
            if (order == null)
            {
                return NotFound();
            }

            _orderService.Delete(order);

            return Ok("Order deleted successfully");
        }
        [HttpPut("selectMain")]
        public async Task<IActionResult> SelectMainOrder(MainOrderDTO dto,int userId)
        {

            if (ModelState.IsValid)
            {
                List<Order> orderList = GetOrdersByUserId(userId);

                foreach (var order in orderList)
                {
                    var nonMainOrder = _orderService.GetElementById(order.Id);
                    nonMainOrder.IsMain = false;
                    _orderService.Update(nonMainOrder);
                }

                var orderToUpdate = _orderService.GetElementById(dto.Id);
                if (orderToUpdate == null)
                {
                    return NotFound();
                }

                orderToUpdate.IsMain = true;

                _orderService.Update(orderToUpdate);

                return Ok("Order successfully selected as main.");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }
    }
}