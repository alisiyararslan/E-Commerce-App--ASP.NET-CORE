using Azure;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using NToastNotify;
using X.PagedList;
namespace ShopListAppNKatmanli.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toast;

        public ItemController(IConfiguration configuration, IToastNotification toast)
        {
            _configuration = configuration;
            _toast = toast;
        }

        //product list Dashboard Myproduct pages
        public async Task<IActionResult>  Index(int page=1)
        {
            var id = int.Parse(HttpContext.Request.Cookies["UserID"]);
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "Items/getItemsByUserId?userId=" + id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<GetItemDTO> ItemList = JsonConvert.DeserializeObject<List<GetItemDTO>>(content);

               var Items=ItemList.ToPagedList(page,5);
                return View(Items);

            }

            return View();
        }

        //product delete Dashboard Myproduct pages
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {


                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response =await httpClient.DeleteAsync(apiUrl + "Items/deleteById?itemId=" + id);





                _toast.AddSuccessToastMessage("Item has been successfully deleted.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Index", "Item");

            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the Item..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Index", "Item");
            }
        }

        //product add Dashboard Myproduct pages

        [HttpGet]
        public async Task<IActionResult> AddItem()
        {

            return View();
        }
    


        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemDTO dto) 
        {
            try
            {


                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "Items/additem", dto);





                _toast.AddSuccessToastMessage("Item has been successfully added.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Index", "Item");

            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while adding the Item..", new ToastrOptions { Title = "Error." });
                return View();
            }
        }

        public async Task<IActionResult> GetItem(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Items/getById?id=" + id);

                var userId = HttpContext.Request.Cookies["UserID"];

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemDto = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                    if(itemDto.UserId != int.Parse(userId))
                    {
                        return RedirectToAction("ErrorAuth", "Error");
                    }

                    return View("GetItem", itemDto);
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");
                }
            }
          
        }


        public async Task<IActionResult> UpdateItems(GetItemDTO dto)
        {
            try
            {


                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PutAsJsonAsync(apiUrl + "Items/update", dto);





                _toast.AddSuccessToastMessage("Item has been successfully updated.", new ToastrOptions { Title = "Successful." });


                return RedirectToAction("Index", "Item");
            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while updating the Item..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Index", "Item");
            }
        }

    }
}
