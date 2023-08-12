using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NToastNotify;
using NuGet.Packaging.Signing;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using X.PagedList;

namespace ShopListAppNKatmanli.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toast;
       
        public DashboardController(IConfiguration configuration,IToastNotification toast)
        {
            _configuration = configuration;
            _toast = toast;
        }
       

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetUserData()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> updateUserProfile(UserProfileDTO dto)
        {
            string pattern = "^[A-Za-z0-9]+$"; //Pattern for allowing username only english characters and numbers
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var responseUser = await httpClient.GetAsync(apiUrl+"Users/");
            var contentUser = await responseUser.Content.ReadAsStringAsync();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(contentUser);

            if (!Regex.IsMatch(dto.UserName, pattern))
            {
                _toast.AddErrorToastMessage("Username should contain only english characters and numbers.", new ToastrOptions { Title = "Error" });
                return RedirectToAction("Index", "Dashboard");
            }

            if (users.Any(x => x.UserName == dto.UserName && x.Id != dto.Id))
            {
                _toast.AddErrorToastMessage("This username is used by another user.", new ToastrOptions { Title = "Error" });
                return RedirectToAction("Index", "Dashboard");
            }

           
            var response = await httpClient.PutAsJsonAsync(apiUrl + "Users/updateUserProfile", dto);

            if (response.IsSuccessStatusCode)
            {
                _toast.AddSuccessToastMessage("Profile successfully updated.", new ToastrOptions { Title = "Successful" });
                return RedirectToAction("Index", "Dashboard");

            }

            _toast.AddErrorToastMessage("An error was encountered while updating the profile.", new ToastrOptions { Title = "Error" });
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Security()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ResetPasswordDTO dto)
        {
            if (dto.NewPassword == dto.PasswordAgain)
            {

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];

                var response = await httpClient.PutAsJsonAsync(apiUrl + "Users/updatePassword", dto);
                _toast.AddSuccessToastMessage("Password successfully changed", new ToastrOptions { Title = "Successful." });
                return RedirectToAction("Security", "Dashboard");
            }
            _toast.AddErrorToastMessage("The passwords do not match.", new ToastrOptions { Title = "Error." });
            return RedirectToAction("Security", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var id = HttpContext.Request.Cookies["UserID"];
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.DeleteAsync(apiUrl + "Users/delete?id=" + id);
            if(response.IsSuccessStatusCode)
            {
                _toast.AddSuccessToastMessage("Profile successfully deleted.", new ToastrOptions { Title = "Successful" });

                await HttpContext.SignOutAsync();
                HttpContext.Response.Cookies.Delete("UserID");
                _toast.AddInfoToastMessage("Signed Out.", new ToastrOptions { Title = "Sign Out" });

                return RedirectToAction("Index","Home");
            }
            _toast.AddErrorToastMessage("An error was encountered while deleting the profile.", new ToastrOptions { Title = "Error" });
            return RedirectToAction("Security", "Dashboard"); ;
        }


        [HttpGet]
        public async Task<IActionResult> Adresses(int page = 1)
        {
            try
            {
                var id = int.Parse(HttpContext.Request.Cookies["UserID"]);
                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Addresses/getAddressesByUserId?userId=" + id);

                var content = await response.Content.ReadAsStringAsync();
                List<GetAddressDTO> addressList = JsonConvert.DeserializeObject<List<GetAddressDTO>>(content);

                var addresses = addressList.ToPagedList(page, 10);


                return View(addresses);
            }

            catch (Exception e)
            {
                return View(new List<GetAddressDTO>().ToPagedList(page,10));
            }
        }

        [HttpGet]
        public IActionResult AddAddresses() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddresses(DefaultAddressDTO dto)
        {
            try
            {

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "Addresses/addaddress", dto);



                _toast.AddSuccessToastMessage("Address has been successfully added.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Adresses", "Dashboard");


            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while adding the Address..", new ToastrOptions { Title = "Error." });
                return View();
            }

        }
        
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.DeleteAsync(apiUrl + "Addresses/delete?addressID=" + id);



                _toast.AddSuccessToastMessage("Address has been successfully deleted.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Adresses", "Dashboard");


            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the Address..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Adresses", "Dashboard");
            }
        }

        public async Task<IActionResult> GetAddress(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Addresses/get?id=" + id);

                var userId = HttpContext.Request.Cookies["UserID"];

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var addressDTO = JsonConvert.DeserializeObject<GetAddressDTO>(jsonContent);

                    if (addressDTO.UserID != int.Parse(userId))
                    {
                        return RedirectToAction("ErrorAuth", "Error");
                    }

                    return View("GetAddress", addressDTO);
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI","Error");
                }
            }
        }
        public async Task<IActionResult> UpdateAddress(GetAddressDTO dto)
        {
            try
            {
                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PutAsJsonAsync(apiUrl + "Addresses/update", dto);


                _toast.AddSuccessToastMessage("Address has been successfully updated.", new ToastrOptions { Title = "Successful." });


                return RedirectToAction("Adresses", "Dashboard");
            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while updating the Address..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Adresses", "Dashboard");
            }
        }


        public async Task<IActionResult> GetAddressDetail(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Addresses/get?id=" + id);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var addressDTO = JsonConvert.DeserializeObject<GetAddressDTO>(jsonContent);

                    return View("GetAddressDetail", addressDTO);
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");
                }
            }
        }
        //orders view
        [HttpGet]
        public async Task<IActionResult> Orders(int page=1)
        {
            try
            {
                var id = int.Parse(HttpContext.Request.Cookies["UserID"]);
                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Orders/getOrdersByUserId?userId=" + id);

                var content = await response.Content.ReadAsStringAsync();
                List<GetOrderDTO> orderList = JsonConvert.DeserializeObject<List<GetOrderDTO>>(content);

                var orders = orderList.ToPagedList(page, 10);

                return View(orders);
            }
            catch (Exception e)
            {
                
                return View(new List<GetOrderDTO>().ToPagedList(page, 10));
            }
        }

        [HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderDTO dto)
        {
            try
            {

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var id = HttpContext.Request.Cookies["UserID"];
                dto.UserId = int.Parse(id);

                var response = await httpClient.PostAsJsonAsync(apiUrl + "Orders/addOrder", dto);





                _toast.AddSuccessToastMessage("Order has been successfully added.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Orders", "Dashboard");

            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while adding the Order..", new ToastrOptions { Title = "Error." });
                return View();
            }
        }

        [HttpGet]    
        public async Task<IActionResult> GetOrder(GetOrderDTO dto)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Orders/get?id=" + dto.Id);

                var userId = HttpContext.Request.Cookies["UserID"];


                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var orderDTO = JsonConvert.DeserializeObject<GetOrderDTO>(jsonContent);

                    if (orderDTO.UserId != int.Parse(userId))
                    {
                        return RedirectToAction("ErrorAuth", "Error");
                    }

                    return View("GetOrder", orderDTO);
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrder(GetOrderDTO dto)
        {
            try
            {

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PutAsJsonAsync(apiUrl + "Orders/updateOrder", dto);



                _toast.AddSuccessToastMessage("Order has been successfully updated.", new ToastrOptions { Title = "Successful." });


                return RedirectToAction("Orders", "Dashboard");
            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while updating the Order..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Orders", "Dashboard");
            }
        }

        public async Task<IActionResult> SelectMainOrder(MainOrderDTO dto)
        {
            try
            {

                var httpClient = new HttpClient();
                var order = new Order();
                var usertId = HttpContext.Request.Cookies["UserID"];
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PutAsJsonAsync(apiUrl + "Orders/selectMain?userId="+ usertId, dto);

               
                _toast.AddSuccessToastMessage("Order has been successfully selected as main.", new ToastrOptions { Title = "Successful." });


                return RedirectToAction("Orders", "Dashboard");
            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while selecting the Main Order..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Orders", "Dashboard");
            }
        }
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.DeleteAsync(apiUrl + "Orders/deleteOrder?orderID=" + id);

                _toast.AddSuccessToastMessage("Order has been successfully deleted.", new ToastrOptions { Title = "Successful." });


                return RedirectToAction("Orders", "Dashboard");
            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the Order..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Index", "Item");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Reminders(int page=1)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "Reminders");

            int userId = int.Parse(HttpContext.Request.Cookies["UserID"]);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<GetReminderDTO> reminderList = JsonConvert.DeserializeObject<List<GetReminderDTO>>(content);


                List<GetReminderDTO> remindersWithUserId = reminderList.Where(reminder => reminder.UserId == userId).ToList();

                var reminders = remindersWithUserId.ToPagedList(page, 10);
                return View(reminders);

            }

            else
            {
                return RedirectToAction("ErrorAPI", "Error");
            }

        }

        [HttpGet]
        public IActionResult AddReminder()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddReminder(DefaultReminderDTO dto)
        {
            try
            {

                var httpClient = new HttpClient();
                dto.UserId = int.Parse(HttpContext.Request.Cookies["UserID"]);
                dto.IsRead = false;
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "Reminders/addreminder", dto);





                _toast.AddSuccessToastMessage("Reminder has been successfully added.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Reminders", "Dashboard");

            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while adding the Reminder..", new ToastrOptions { Title = "Error." });
                return View();
            }
        }

        public async Task<IActionResult> GetReminder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Reminders/get?id=" + id);

                var userId = HttpContext.Request.Cookies["UserID"];

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var reminderDTO = JsonConvert.DeserializeObject<GetReminderDTO>(jsonContent);

                    if (reminderDTO.UserId != int.Parse(userId))
                    {
                        return RedirectToAction("ErrorAuth", "Error");
                    }

                    return View("GetReminder", reminderDTO);
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");
                }
            }
        }

        public async Task<IActionResult> DeleteReminder(int id)
        {
            try
            {

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.DeleteAsync(apiUrl + "Reminders/delete?reminderID=" + id);

                _toast.AddSuccessToastMessage("Reminder has been successfully deleted.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Reminders", "Dashboard");

            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the Reminder..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Reminders", "Dashboard");
            }
        }


        public async Task<IActionResult> UpdateReminder(GetReminderDTO dto)
        {
            try
            {

                dto.UserId = int.Parse(HttpContext.Request.Cookies["UserID"]);
                dto.IsRead = false;
                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PutAsJsonAsync(apiUrl + "Reminders/update", dto);
                _toast.AddSuccessToastMessage("Reminder has been successfully updated.", new ToastrOptions { Title = "Successful." });

                return RedirectToAction("Reminders", "Dashboard");

            }
            catch (Exception e)
            {
                _toast.AddErrorToastMessage("An error was encountered while updating the Reminder..", new ToastrOptions { Title = "Error." });
                return RedirectToAction("Reminders", "Dashboard");
            }
        }



    }
}
