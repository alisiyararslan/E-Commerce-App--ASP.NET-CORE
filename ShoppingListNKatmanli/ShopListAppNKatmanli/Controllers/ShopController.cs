using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NToastNotify;
using X.PagedList;

namespace ShopListAppNKatmanli.Controllers
{
    public class ShopController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toast;
        public ShopController(IConfiguration configuration, IToastNotification toast)
        {
            _configuration = configuration;
            _toast = toast;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string[] price, string[] subCategory, string[] categoryDetail, string sort, string search, bool discount, int page = 1)
        {
            ViewBag.Prices = price;
            ViewBag.SubCategories = subCategory;
            ViewBag.CategoryDetails = categoryDetail;
            ViewBag.Sorting = sort;
            ViewBag.Searching = search;

            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var responseItem = await httpClient.GetAsync(apiUrl + "Items");

            List<GetItemDTO> itemsList = new List<GetItemDTO>();

            if (responseItem.IsSuccessStatusCode)
            {
                var contentItem = await responseItem.Content.ReadAsStringAsync();
                itemsList = JsonConvert.DeserializeObject<List<GetItemDTO>>(contentItem);
            }

            //Discount page
            if(discount)
            {
                List<GetItemDTO> discountedItems = itemsList.Where(x => x.Discount > 0).ToList();

                var pagedDiscounts = discountedItems.ToPagedList(page, 15);
                return View(pagedDiscounts);
            }

            //Price filtering
            if (price != null && price.Length > 0 && !price.Contains("all"))
            {
                itemsList = itemsList.Where(p =>
                {
                    double itemPrice = Convert.ToDouble(p.Price.ToString().Trim('₺').Replace(",00", ""));
                    foreach (string selectedPrice in price)
                    {
                        switch (selectedPrice)
                        {
                            case "1":
                                if (itemPrice >= 0 && itemPrice < 2000) return true;
                                break;
                            case "2":
                                if (itemPrice >= 2000 && itemPrice < 5000) return true;
                                break;
                            case "3":
                                if (itemPrice >= 5000 && itemPrice < 10000) return true;
                                break;
                            case "4":
                                if (itemPrice >= 10000 && itemPrice < 20000) return true;
                                break;
                            case "5":
                                if (itemPrice >= 20000) return true;
                                break;
                        }
                    }
                    return false;
                }).ToList();

            }


            //SubCategories filtering

            var responseSubCat = await httpClient.GetAsync(apiUrl + "SubCategories");
            var contentSubCat = await responseSubCat.Content.ReadAsStringAsync();
            List<GetSubCategoryDTO> subCategories = JsonConvert.DeserializeObject<List<GetSubCategoryDTO>>(contentSubCat);

            if (subCategory != null && subCategory.Length > 0 && !subCategory.Contains("all"))
            {
                itemsList = itemsList.Where(p =>
                {
                    int subCategoryId = p.SubCategoryId;
                    foreach (string selectedSubCategory in subCategory)
                    {
                        foreach (GetSubCategoryDTO subDTO in subCategories)
                        {
                            if (selectedSubCategory == subCategoryId.ToString())
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }).ToList();
            }

            //CategoryDetails filtering

            var responseCatDet = await httpClient.GetAsync(apiUrl + "CategoryDetails");
            var contentCatDet = await responseCatDet.Content.ReadAsStringAsync();
            List<GetCategoryDetailDTO> categoryDetails = JsonConvert.DeserializeObject<List<GetCategoryDetailDTO>>(contentCatDet);

            if (categoryDetail != null && categoryDetail.Length > 0 && !categoryDetail.Contains("all"))
            {
                itemsList = itemsList.Where(p =>
                {
                    int categoryDetailId = p.CategoryDetailId;
                    foreach (string selectedCategoryDetail in categoryDetail)
                    {
                        foreach (GetCategoryDetailDTO catDTO in categoryDetails)
                        {
                            if (selectedCategoryDetail == categoryDetailId.ToString())
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }).ToList();
            }

            //Price Ascending sorting
            if (sort != null && sort.Equals("ascending"))
            {
                var responsePriceAscending = await httpClient.GetAsync(apiUrl + "Items/getByPriceAscending");
                if (responsePriceAscending.IsSuccessStatusCode)
                {
                    var contentPriceAscending = await responsePriceAscending.Content.ReadAsStringAsync();
                    List<GetItemDTO> priceAscendingItems = JsonConvert.DeserializeObject<List<GetItemDTO>>(contentPriceAscending);
                    var priceAscendings = priceAscendingItems.ToPagedList(page, 15);
                    return View(priceAscendings);
                }
            }

            //Price Descending sorting
            if (sort != null && sort.Equals("descending"))
            {
                var responsePriceDescending = await httpClient.GetAsync(apiUrl + "Items/getByPriceDescending");
                if (responsePriceDescending.IsSuccessStatusCode)
                {
                    var contentPriceDescending = await responsePriceDescending.Content.ReadAsStringAsync();
                    List<GetItemDTO> priceDescendingItems = JsonConvert.DeserializeObject<List<GetItemDTO>>(contentPriceDescending);
                    var priceDescendings = priceDescendingItems.ToPagedList(page, 15);
                    return View(priceDescendings);
                }
            }

            //Latest sorting
            if (sort != null && sort.Equals("latest"))
            {
                var responseLatest = await httpClient.GetAsync(apiUrl + "Items/getByLatest");
                if (responseLatest.IsSuccessStatusCode)
                {
                    var contentLatest = await responseLatest.Content.ReadAsStringAsync();
                    List<GetItemDTO> LatestItems = JsonConvert.DeserializeObject<List<GetItemDTO>>(contentLatest);
                    var latests = LatestItems.ToPagedList(page, 15);
                    return View(latests);
                }
            }

            //Popularity sorting
            if (sort != null && sort.Equals("popularity"))
            {
                var responsePopularity = await httpClient.GetAsync(apiUrl + "Items/getByPopularity");
                if (responsePopularity.IsSuccessStatusCode)
                {
                    var contentPopularity = await responsePopularity.Content.ReadAsStringAsync();
                    List<GetItemDTO> PopularityItems = JsonConvert.DeserializeObject<List<GetItemDTO>>(contentPopularity);
                    var popularities = PopularityItems.ToPagedList(page, 15);
                    return View(popularities);
                }
            }

            //Searching
            if (search != null)
            {
                List<GetItemDTO> searchedItems = new();

                foreach (GetItemDTO item in itemsList)
                {
                    if (item.Title.ToLower().Contains(search.ToLower()))
                    {
                        searchedItems.Add(item);
                        continue;
                    }

                    if (item.Brand.ToLower().Contains(search.ToLower()))
                    {
                        searchedItems.Add(item);
                        continue;
                    }

                    var responseTempCat = await httpClient.GetAsync(apiUrl + "Categories/getById/" + item.CategoryId);
                    var contentTempCat = await responseTempCat.Content.ReadAsStringAsync();
                    GetCategoryDTO itemCat = JsonConvert.DeserializeObject<GetCategoryDTO>(contentTempCat);

                    if (itemCat.Name.ToLower().Contains(search.ToLower()))
                    {
                        searchedItems.Add(item);
                        continue;
                    }

                    var responseTempSubCat = await httpClient.GetAsync(apiUrl + "SubCategories/getById/" + item.SubCategoryId);
                    var contentTempSubCat = await responseTempSubCat.Content.ReadAsStringAsync();
                    GetSubCategoryDTO itemSubCat = JsonConvert.DeserializeObject<GetSubCategoryDTO>(contentTempSubCat);

                    if (itemSubCat.Name.ToLower().Contains(search.ToLower()))
                    {
                        searchedItems.Add(item);
                        continue;
                    }

                    var responseTempCatDet = await httpClient.GetAsync(apiUrl + "CategoryDetails/getById/" + item.CategoryDetailId);
                    var contentTempCatDet = await responseTempCatDet.Content.ReadAsStringAsync();
                    GetCategoryDetailDTO itemCatDet = JsonConvert.DeserializeObject<GetCategoryDetailDTO>(contentTempCatDet);

                    if (itemCatDet.Name.ToLower().Contains(search.ToLower()))
                    {
                        searchedItems.Add(item);
                        continue;
                    }
                }
                var searchItems = searchedItems.ToPagedList(page, 15);
                return View(searchItems);
            }



            var items = itemsList.ToPagedList(page, 15);
            return View(items);
        }

        public async Task<IActionResult> GetItemDetail(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.GetAsync(apiUrl + "Items/getById?id=" + id);

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var itemDTO = JsonConvert.DeserializeObject<GetItemDTO>(jsonContent);

                    return View("GetItemDetail", itemDTO);
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");

                }
            }
        }

        [Authorize]
        public async Task<IActionResult> AddOrderDetail(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var orderDetail = new OrderDetail();
                var userId = HttpContext.Request.Cookies["UserID"];
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "OrderDetails/addOrderDetail?itemID=" + id+ "&userId="+userId, orderDetail);
                
                if (response.IsSuccessStatusCode)
                {
                    _toast.AddSuccessToastMessage("Successfully added to basket", new ToastrOptions { Title = "Successful." });
                    return RedirectToAction("Index", "Shop");
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");
                }
            }
        }


        [Authorize]
        public async Task<IActionResult> AddOrderDetailIndex(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var orderDetail = new OrderDetail();
                var userId = HttpContext.Request.Cookies["UserID"];
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "OrderDetails/addOrderDetail?itemID=" + id + "&userId=" + userId, orderDetail);

                if (response.IsSuccessStatusCode)
                {
                    _toast.AddSuccessToastMessage("Successfully added to basket", new ToastrOptions { Title = "Successful." });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // API'dan veri alınırken hata oluştu, uygun bir hata sayfasına yönlendirilebilir.
                    return RedirectToAction("ErrorAPI", "Error");
                }
            }
        }

    }
}
