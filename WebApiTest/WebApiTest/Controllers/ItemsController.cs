using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IUserService _userService;
        public ItemsController(IItemService itemService, IUserService userService)
        {
            _itemService = itemService;
            _userService = userService;
        }

        [HttpGet]
        public List<GetItemDTO> GetAllItems()
        {
            List<Item> items = _itemService.GetListAll();

            List<GetItemDTO> itemDTOs = items.Select(item => new GetItemDTO
            {
                
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            }).ToList();

            return itemDTOs;
        }

        [HttpGet("getTrendyItems")]
        public List<Item> GetTrendyItems()
        {
            List<Item> items = _itemService.GetListAll();
            List<Item> trendyItems = new List<Item>();

            trendyItems = items.OrderByDescending(item => item.FavoriteCount).Take(12).ToList();

            return trendyItems;
        }

        [HttpGet("getLatestItems")]
        public List<Item> GetLatestItems()
        {
            List<Item> items = _itemService.GetListAll();
            List<Item> latestItems = new List<Item>();

            latestItems = items.OrderByDescending(item => item.Id).Take(12).ToList();

            return latestItems;
        }

        [HttpGet("getItemsByUserId")]
        public List<Item> GetItemsByUserId(int userId)
        {
            List<Item> items = _itemService.GetItemsByUserId(userId);

            List<Item> itemsByUserId = items.Select(item => new Item
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            }).ToList();

            return itemsByUserId;
        }


        [HttpGet("getById")]
        public Item GetItem(int id)
        {
            var item = _itemService.GetElementById(id);

            if (item == null)
            {
                throw new Exception("NotFound");
            }

            return item;
        }

        
        [HttpGet("getByName/{name:alpha}")]
        public Item GetItem(string brand)
        {
            var item = _itemService.GetItemByBrand(brand);

            if (item == null)
            {
                throw new Exception("NotFound");
            }

            return item;
        }

        
        [HttpGet("getByPriceAscending")]
        public List<GetItemDTO> GetItemsByPriceAscending()
        {
            List<Item> items = _itemService.GetListAll();

            List<GetItemDTO> itemDTOs = items.Select(item => new GetItemDTO
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            }).ToList();

            itemDTOs = itemDTOs.OrderBy(dto => dto.Price).ToList();

            return itemDTOs;
        }

        
        [HttpGet("getByPriceDescending")]
        public List<GetItemDTO> GetItemsByPriceDescending()
        {
            List<Item> items = _itemService.GetListAll();

            List<GetItemDTO> itemDTOs = items.Select(item => new GetItemDTO
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            }).ToList();

            itemDTOs = itemDTOs.OrderByDescending(dto => dto.Price).ToList();

            return itemDTOs;
        }

        
        [HttpGet("getByLatest")]
        public List<GetItemDTO> GetItemsByLatest()
        {
            List<Item> items = _itemService.GetListAll();

            List<GetItemDTO> itemDTOs = items.Select(item => new GetItemDTO
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            }).ToList();

            itemDTOs = itemDTOs.OrderByDescending(dto => dto.Id).ToList();

            return itemDTOs;
        }

        
        [HttpGet("getByPopularity")]
        public List<GetItemDTO> GetItemsByPopularity()
        {
            List<Item> items = _itemService.GetListAll();

            List<GetItemDTO> itemDTOs = items.Select(item => new GetItemDTO
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = item.FavoriteCount,
                Title = item.Title,
                Brand = item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            }).ToList();

            itemDTOs = itemDTOs.OrderByDescending(dto => dto.FavoriteCount).ToList();

            return itemDTOs;
        }

        
        [HttpPost("additem")]
        public async Task<ActionResult<AddItemDTO>> AddItem(AddItemDTO item)
        {
            _itemService.Insert(new Item()
            {
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId,
                CategoryDetailId = item.CategoryDetailId,
                UserId = item.UserId,
                FavoriteCount = 0,
                Title = item.Title,
                Brand= item.Brand,
                Price = item.Price,
                Discount = item.Discount,
                Image = item.Image,
                Description = item.Description,
            });;

            return item;
        }

        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateItem(GetItemDTO dto)
        {

            if (ModelState.IsValid)
            {
                var itemToUpdate = _itemService.GetElementById(dto.Id);
                if (itemToUpdate == null)
                {
                    return NotFound();
                }

                itemToUpdate.CategoryId = dto.CategoryId;
                itemToUpdate.SubCategoryId = dto.SubCategoryId;
                itemToUpdate.CategoryDetailId = dto.CategoryDetailId;
                itemToUpdate.UserId = dto.UserId;
                itemToUpdate.FavoriteCount = dto.FavoriteCount;
                itemToUpdate.Title = dto.Title;
                itemToUpdate.Brand = dto.Brand;
                itemToUpdate.Price = dto.Price;
                itemToUpdate.Discount = dto.Discount;
                itemToUpdate.Image = dto.Image;
                itemToUpdate.Description = dto.Description;
 

                _itemService.Update(itemToUpdate);

                return Ok("Item successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        
        [HttpDelete("deleteById/")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var item = _itemService.GetElementById(itemId);
            if (item == null)
            {
                return NotFound();
            }

            _itemService.Delete(item);

            return Ok("Item deleted successfully");
        }

        
        [HttpDelete("deleteByBrand/{brand:alpha}")]
        public async Task<IActionResult> DeleteItem(string brand)
        {
            var item = _itemService.GetItemByBrand(brand);
            if (item == null)
            {
                return NotFound();
            }

            _itemService.Delete(item);

            return Ok("Item deleted successfully");
        }
        private bool ItemExists(int id)
        {
            var item = _itemService.GetElementById(id);

            return item != null;
        }

        private bool ItemExists(string brand)
        {
            var item = _itemService.GetItemByBrand(brand);

            return item != null;
        }
    }
}
