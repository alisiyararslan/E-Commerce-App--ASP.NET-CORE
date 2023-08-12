using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Concrete;
using BusinessLayer.Abstract;
using EntityLayer.DTOs;



namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteItemUsersController : ControllerBase
    {
        private readonly IFavoriteItemUserService _favoriteItemUserService;


        public FavoriteItemUsersController(IFavoriteItemUserService favoriteItemUserService)
        {
            _favoriteItemUserService = favoriteItemUserService;
        }

        [HttpPost("addfavoriteitemuser")]
        public async Task<ActionResult<DefaultFavoriteItemUserDTO>> AddFavoriteItemUser(DefaultFavoriteItemUserDTO favoriteItemUser)
        {
            _favoriteItemUserService.Insert(new FavoriteItemUser()
            {

                UserId = favoriteItemUser.UserId,
                ItemId = favoriteItemUser.ItemId,

            });

            return favoriteItemUser;
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteFavoriteItemUser(DefaultFavoriteItemUserDTO dto)
        {
            var favoriteItemUser = _favoriteItemUserService.GetListAll().Where(x => x.ItemId == dto.ItemId && x.UserId == dto.UserId);

            if (favoriteItemUser == null)
            {
                return NotFound();
            }

            var deletedFavoriteItem = favoriteItemUser.First();

            _favoriteItemUserService.Delete(deletedFavoriteItem);

            return Ok("FavoriteItemUser deleted successfully");
        }


        //[Authorize]
        [HttpGet("get")]
        public FavoriteItemUser GetFavoriteItemUser(int id)
        {
            var favoriteItemUser = _favoriteItemUserService.GetElementById(id);

            if (favoriteItemUser == null)
            {
                throw new Exception("NotFound");
            }

            return favoriteItemUser;
        }

        [HttpGet("getByUserId")]
        public List<GetFavoriteItemUserDTO> GetFavoriteItemUsersByUserId(int userId)
        {
            var favoriteItemUsers = _favoriteItemUserService.GetListAll().Where(x=> x.UserId == userId);

            List<GetFavoriteItemUserDTO> favoriteItemUsersDTOs = favoriteItemUsers.Select(favoriteItemUser => new GetFavoriteItemUserDTO
            {
                Id = favoriteItemUser.Id,
                ItemId = favoriteItemUser.ItemId,
                UserId = favoriteItemUser.UserId,

            }).ToList();

            return favoriteItemUsersDTOs;
        }


        // [Authorize]
        [HttpGet]
        public List<GetFavoriteItemUserDTO> GetAllFavoriteItemUsers()
        {
            List<FavoriteItemUser> favoriteItemUsers = _favoriteItemUserService.GetListAll();

            List<GetFavoriteItemUserDTO> favoriteItemUsersDTOs = favoriteItemUsers.Select(favoriteItemUser => new GetFavoriteItemUserDTO
            {
                Id = favoriteItemUser.Id,
                ItemId = favoriteItemUser.ItemId,
                UserId = favoriteItemUser.UserId,



            }).ToList();

            return favoriteItemUsersDTOs;
        }

    }
}
