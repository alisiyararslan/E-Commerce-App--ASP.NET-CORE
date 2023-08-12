using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
//using NToastNotify;
using System.Threading;
using X.PagedList;

namespace ShopListAppNKatmanli.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {

        //private Timer _timer;
        //private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public NotificationController(/*HttpClient httpClient,*/ IConfiguration configuration)
        {
            _configuration = configuration;
            //_httpClient = httpClient;
        }

        /// private readonly IToastNotification toast;


        //public NotificationController(IToastNotification _toast)
        //{
        //    toast = _toast;





        //}
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Notification(int page=1)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "Reminders");

            int userId = int.Parse(HttpContext.Request.Cookies["UserID"]);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<GetReminderDTO> reminderList = JsonConvert.DeserializeObject<List<GetReminderDTO>>(content);



                List<GetReminderDTO> remindersWithUserId = reminderList.Where(
                    reminder => reminder.UserId == userId &&
                    reminder.Date <= System.DateTime.Now).ToList();

                foreach (var reminder in remindersWithUserId)
                {
                    reminder.IsRead = true;
                    var httpClient2 = new HttpClient();
                    var response2 = await httpClient2.PutAsJsonAsync(apiUrl + "Reminders/update", reminder);
                }
                var reminders = remindersWithUserId.ToPagedList(page,10);
                return View(reminders);

            }



            return View();
        }

        public async Task<IActionResult> CurrentNotification()
        {

            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.GetAsync(apiUrl + "Reminders");

            int userId = int.Parse(HttpContext.Request.Cookies["UserID"]);


            var content = await response.Content.ReadAsStringAsync();
            List<GetReminderDTO> reminderList = JsonConvert.DeserializeObject<List<GetReminderDTO>>(content);

            DateTime currentTime = DateTime.Now;

            List<GetReminderDTO> remindersWithTime = reminderList.Where(
                reminder => reminder.UserId == userId &&
                reminder.Date.Year == currentTime.Year &&
                reminder.Date.Month == currentTime.Month &&
                reminder.Date.Hour == currentTime.Hour &&
                reminder.Date.Minute == currentTime.Minute &&
                reminder.Date.Day == currentTime.Day).ToList();

            return View(remindersWithTime);
        }
    }
}
