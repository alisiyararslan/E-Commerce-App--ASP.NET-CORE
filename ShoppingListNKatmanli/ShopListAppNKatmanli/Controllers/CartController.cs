using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;

namespace ShopListAppNKatmanli.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toast;

        public CartController(IConfiguration configuration, IToastNotification toast)
        {
            _configuration = configuration;
            _toast = toast;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string shareCode)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "Orders/getOrderWithShareCode?shareCode=" + shareCode);


            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<GetOrderDTO>(content);

                return View("Index", order);
            }
            else
            {
                // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                return RedirectToAction("Error404", "Error");
            }
        }

        public async Task<IActionResult> UpdateOrderDetailPlus(int id)
        {
            var httpClient = new HttpClient();

            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "OrderDetails/updateplus?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            Order order = JsonConvert.DeserializeObject<Order>(content);
            return RedirectToAction("Index", order);

        }
        public async Task<IActionResult> UpdateOrderDetailMinus(int id)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "OrderDetails/updateminus?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            Order order = JsonConvert.DeserializeObject<Order>(content);
            return RedirectToAction("Index", order);
        }


        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];

            var response = await httpClient.GetAsync(apiUrl + "OrderDetails/get?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            var orderDetail = JsonConvert.DeserializeObject<GetOrderDetailDTO>(content);

            int orderId = orderDetail.OrderId;
            var responseShareCode = await httpClient.GetAsync(apiUrl+"Orders/get?id="+orderId);
            var shareCodeContent=await responseShareCode.Content.ReadAsStringAsync();
            var order=JsonConvert.DeserializeObject<Order>(shareCodeContent);

            try
            {

                var response2 =await httpClient.DeleteAsync(apiUrl + "OrderDetails/deleteOrderDetail?orderdetailID=" + id);
                _toast.AddSuccessToastMessage("Item has been successfully deleted from basket.", new ToastrOptions { Title = "Successful." });

            }

            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the Item from basket..", new ToastrOptions { Title = "Error." });

            }

            return RedirectToAction("Index", "Cart", new { @shareCode = order.ShareCode });
        }

        public async Task<IActionResult> SaveCart(GetOrderDTO dto)
        {
            var id = int.Parse(HttpContext.Request.Cookies["UserID"]);
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "Users/getUserById?id=" + id);

            if (response.IsSuccessStatusCode)
            {

                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(content);

                var responseFirstAddress = await httpClient.GetAsync(apiUrl + "Addresses/getFirstAddressByUserId?userId=" + id);
                var contentFirstAddress = await responseFirstAddress.Content.ReadAsStringAsync();

                Address firstAddress = JsonConvert.DeserializeObject<Address>(contentFirstAddress);

                var savedOrderDTO = new AddOrderDTO
                {
                    UserId = int.Parse(HttpContext.Request.Cookies["UserID"]),
                    AddressId = firstAddress.Id,
                    
                    Status = dto.Status
                };

                var responseSave = await httpClient.PostAsJsonAsync(apiUrl + "Orders/addOrder" , savedOrderDTO);
                var contentSaved = await responseSave.Content.ReadAsStringAsync();

                Order savedOrderContent = JsonConvert.DeserializeObject<Order>(contentSaved);
                GetOrderDTO savedOrder = savedOrderContent;

                var responseOrderDetail = await httpClient.GetAsync(apiUrl + "OrderDetails/orderId/" + dto.Id);
                var contentOrderDetail = await responseOrderDetail.Content.ReadAsStringAsync();
                List<GetOrderDetailDTO> orderDetailList = JsonConvert.DeserializeObject<List<GetOrderDetailDTO>>(contentOrderDetail);

                foreach (var orderDetail in orderDetailList)
                {
                    AddOrderDetailDTO addedOrderDetail = new AddOrderDetailDTO
                    {
                        OrderId = savedOrder.Id,
                        ItemId = orderDetail.ItemId,
                        Amount = orderDetail.Amount,
                        UnitPrice = orderDetail.UnitPrice,
                        LineTotal = orderDetail.LineTotal,

                    };
                    savedOrder.TotalPrice += addedOrderDetail.LineTotal;


                    var responsesaveOrderDetail = await httpClient.PostAsJsonAsync(apiUrl + "OrderDetails/addCopiedOrderDetail", addedOrderDetail);
                }

                var orderSaved = await httpClient.PutAsJsonAsync(apiUrl + "Orders/SavedOrder", savedOrder);


                _toast.AddSuccessToastMessage("Cart successfully saved.", new ToastrOptions { Title = "Successful." });
                return RedirectToAction("Orders", "Dashboard");

            }

            _toast.AddErrorToastMessage("An error was encountered while saving the basket..", new ToastrOptions { Title = "Error." });
            return RedirectToAction("Index", "Cart");
        }

    }
}
