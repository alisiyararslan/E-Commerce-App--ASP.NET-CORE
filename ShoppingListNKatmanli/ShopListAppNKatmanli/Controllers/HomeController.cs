using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ShopListAppNKatmanli.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var responseTrendy = await httpClient.GetAsync(apiUrl + "Items/getTrendyItems");
            var responseLatest = await httpClient.GetAsync(apiUrl + "Items/getLatestItems");

            TrendyLatestDTO trendyLatestItems = new TrendyLatestDTO();

            if (responseTrendy.IsSuccessStatusCode && responseTrendy.IsSuccessStatusCode)
            {
                var contentTrendy = await responseTrendy.Content.ReadAsStringAsync();
                var contentLatest = await responseLatest.Content.ReadAsStringAsync();
                var trendy = JsonConvert.DeserializeObject<List<Item>>(contentTrendy);
                var latest = JsonConvert.DeserializeObject<List<Item>>(contentLatest);

                trendyLatestItems.TrendyItems = trendy;
                trendyLatestItems.LatestItems = latest;

            }

            return View(trendyLatestItems);
        }

        [HttpGet]

        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Contact(MailDTO mail)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.PostAsJsonAsync(apiUrl + "Mail/SendContactMail", mail);


            return RedirectToAction("Contact","Home");
        }




    }
}