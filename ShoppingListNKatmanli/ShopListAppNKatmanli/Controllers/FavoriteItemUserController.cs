using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using X.PagedList;

namespace ShopListAppNKatmanli.Controllers
{
    [Authorize]
    public class FavoriteItemUserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toast;
        public FavoriteItemUserController(IConfiguration configuration, IToastNotification toast)
        {
            _configuration = configuration;
            _toast = toast;
        }

        //[HttpGet]
        //public IActionResult AddFavoriteItemUser()
        //{
        //    return View();
        //}


        //[HttpPost]
        public async Task<IActionResult> AddFavoriteItemUser(int itemId)
        {
            DefaultFavoriteItemUserDTO dto = new DefaultFavoriteItemUserDTO();
            var httpClient = new HttpClient();
            dto.UserId = int.Parse(HttpContext.Request.Cookies["UserID"]);
            dto.ItemId = itemId;
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.PostAsJsonAsync(apiUrl+"FavoriteItemUsers/addfavoriteitemuser", dto);


            var httpClient2 = new HttpClient();

            var response2 = await httpClient2.GetAsync(apiUrl+"Items/getById?id=" + itemId);

            if (response2.IsSuccessStatusCode)
            {
                var jsonContent = await response2.Content.ReadAsStringAsync();
                var itemDTO = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                itemDTO.FavoriteCount = itemDTO.FavoriteCount + 1;

                var httpClient3 = new HttpClient();
                var response3 = await httpClient3.PutAsJsonAsync(apiUrl+"Items/update", itemDTO);



            }
            else
            {

                return RedirectToAction("ErrorAPI", "Error");
            }

            if (response.IsSuccessStatusCode)
            {
                _toast.AddInfoToastMessage("Favourite Item Added", new ToastrOptions { Title = "New Favourite Item"});
            }

            return RedirectToAction("Index", "Shop");
        }

        public async Task<IActionResult> AddFavoriteItemUserHome(int itemId)
        {
            DefaultFavoriteItemUserDTO dto = new DefaultFavoriteItemUserDTO();
            var httpClient = new HttpClient();
            dto.UserId = int.Parse(HttpContext.Request.Cookies["UserID"]);
            dto.ItemId = itemId;
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.PostAsJsonAsync(apiUrl + "FavoriteItemUsers/addfavoriteitemuser", dto);


            var httpClient2 = new HttpClient();

            var response2 = await httpClient2.GetAsync(apiUrl + "Items/getById?id=" + itemId);

            if (response2.IsSuccessStatusCode)
            {
                var jsonContent = await response2.Content.ReadAsStringAsync();
                var itemDTO = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                itemDTO.FavoriteCount = itemDTO.FavoriteCount + 1;

                var httpClient3 = new HttpClient();
                var response3 = await httpClient3.PutAsJsonAsync(apiUrl + "Items/update", itemDTO);



            }
            else
            {

                return RedirectToAction("ErrorAPI", "Error");
            }

            if (response.IsSuccessStatusCode)
            {
                _toast.AddInfoToastMessage("Favourite Item Added", new ToastrOptions { Title = "New Favourite Item" });
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> DeleteFavoriteItemUser(int itemId)
        {

            try
            {
                DefaultFavoriteItemUserDTO dto = new DefaultFavoriteItemUserDTO()
                {
                    ItemId = itemId,
                    UserId = int.Parse(HttpContext.Request.Cookies["UserID"])
                };

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "FavoriteItemUsers/delete/" , dto);

                var httpClient2 = new HttpClient();

                var response2 = await httpClient2.GetAsync(apiUrl + "Items/getById?id=" + itemId);

                if (response2.IsSuccessStatusCode)
                {
                    var jsonContent = await response2.Content.ReadAsStringAsync();
                    var itemDTO = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                    itemDTO.FavoriteCount = itemDTO.FavoriteCount - 1;

                    var httpClient3 = new HttpClient();
                    var response3 = await httpClient3.PutAsJsonAsync(apiUrl + "Items/update", itemDTO);



                }
                else
                {
                    return RedirectToAction("ErrorAPI", "Error");
                }

                _toast.AddInfoToastMessage("Favourite Item Removed", new ToastrOptions { Title = "Removed" });
                return RedirectToAction("Index", "Shop");

            }

            catch(Exception ex)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the favourite item", new ToastrOptions { Title = "Error" });

                return RedirectToAction("Index", "Shop");

            }
        }


        public async Task<IActionResult> DeleteFavoriteItemUserFavoritePage(int itemId)
        {
            try
            {
                DefaultFavoriteItemUserDTO dto = new DefaultFavoriteItemUserDTO()
                {
                    ItemId = itemId,
                    UserId = int.Parse(HttpContext.Request.Cookies["UserID"])
                };

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "FavoriteItemUsers/delete/", dto);

                var httpClient2 = new HttpClient();

                var response2 = await httpClient2.GetAsync(apiUrl + "Items/getById?id=" + itemId);

                if (response2.IsSuccessStatusCode)
                {
                    var jsonContent = await response2.Content.ReadAsStringAsync();
                    var itemDTO = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                    itemDTO.FavoriteCount = itemDTO.FavoriteCount - 1;

                    var httpClient3 = new HttpClient();
                    var response3 = await httpClient3.PutAsJsonAsync(apiUrl + "Items/update", itemDTO);



                }
                else
                {
                    return RedirectToAction("ErrorAPI", "Error");
                }

                _toast.AddInfoToastMessage("Favourite Item Removed", new ToastrOptions { Title = "Removed" });
                return RedirectToAction("FavoriteItems", "FavoriteItemUser");

            }

            catch (Exception ex)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the favourite item", new ToastrOptions { Title = "Error" });

                return RedirectToAction("FavoriteItems", "FavoriteItemUser");

            }

        }

        public async Task<IActionResult> DeleteFavoriteItemUserHome(int itemId)
        {
            try
            {
                DefaultFavoriteItemUserDTO dto = new DefaultFavoriteItemUserDTO()
                {
                    ItemId = itemId,
                    UserId = int.Parse(HttpContext.Request.Cookies["UserID"])
                };

                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response =await httpClient.PostAsJsonAsync(apiUrl + "FavoriteItemUsers/delete/", dto);

                var httpClient2 = new HttpClient();

                var response2 = await httpClient2.GetAsync(apiUrl + "Items/getById?id=" + itemId);

                if (response2.IsSuccessStatusCode)
                {
                    var jsonContent = await response2.Content.ReadAsStringAsync();
                    var itemDTO = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                    itemDTO.FavoriteCount = itemDTO.FavoriteCount - 1;

                    var httpClient3 = new HttpClient();
                    var response3 = await httpClient3.PutAsJsonAsync(apiUrl + "Items/update", itemDTO);



                }
                else
                {
                    return RedirectToAction("ErrorAPI", "Error");
                }

                _toast.AddInfoToastMessage("Favourite Item Removed", new ToastrOptions { Title = "Removed" });
                return RedirectToAction("Index", "Home");

            }

            catch (Exception ex)
            {
                _toast.AddErrorToastMessage("An error was encountered while deleting the favourite item", new ToastrOptions { Title = "Error" });

                return RedirectToAction("Index", "Home");

            }

        }



        [HttpGet]
        public async Task<IActionResult> FavoriteItems(int page=1)
        {
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(apiUrl+"FavoriteItemUsers");

            int userId = int.Parse(HttpContext.Request.Cookies["UserID"]);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<GetFavoriteItemUserDTO> favoriteItemUserList = JsonConvert.DeserializeObject<List<GetFavoriteItemUserDTO>>(content);



                List<GetFavoriteItemUserDTO> favoriteItemUsersWithUserId = favoriteItemUserList.Where(
                    favoriteItemUser => favoriteItemUser.UserId == userId).ToList();

                var httpClient2 = new HttpClient();
                var response2 = await httpClient.GetAsync(apiUrl+"Items");

                List<GetItemDTO> itemsList = new List<GetItemDTO>();

                if (response2.IsSuccessStatusCode)
                {
                    var content2 = await response2.Content.ReadAsStringAsync();
                    itemsList = JsonConvert.DeserializeObject<List<GetItemDTO>>(content2);
                }



                List<GetItemDTO> joinedList = itemsList
                    .Join(favoriteItemUsersWithUserId,
                        itemsList => itemsList.Id,
                        favoriteItemUsersWithUserId => favoriteItemUsersWithUserId.ItemId,
                        (itemsList, favoriteItemUsersWithUserId) => new GetItemDTO
                        {
                            Id = itemsList.Id,
                            CategoryId = itemsList.CategoryId,
                            SubCategoryId = itemsList.SubCategoryId,
                            CategoryDetailId = itemsList.CategoryDetailId,
                            UserId = itemsList.UserId,
                            FavoriteCount = itemsList.FavoriteCount,
                            Title = itemsList.Title,
                            Brand = itemsList.Brand,
                            Price = itemsList.Price,
                            Image = itemsList.Image,
                            Description = itemsList.Description,

                        })
                    .ToList();




                // Now you have the joinedList which contains the matched elements from both lists
                var favoriteItems = joinedList.ToPagedList(page, 12);
                return View(favoriteItems);

                

            }



            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> FavoriteItems()
        //{
        //    return View();
        //}



    }
}
