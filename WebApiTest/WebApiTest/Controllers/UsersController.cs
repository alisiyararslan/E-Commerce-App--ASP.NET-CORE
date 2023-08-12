using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using EntityLayer.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Humanizer;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Text.Json;
using Newtonsoft.Json;
using DataAccessLayer.Security;
using Azure.Core;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]  //api/Users
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;//loglama yaptık
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration; //configuraiton eklendi 

        public UsersController(IUserService userService, ILogger<UsersController> logger, IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;

        }

        [HttpGet]
        public List<User> GetAllUsers()
        {
            return _userService.GetListAll();
        }

        [HttpGet("getUserById")]
        public User GetUser(int id)
        {
          var user = _userService.GetElementById(id);

          if (user == null)
          {
              throw new Exception("NotFound");
          }

            return user;
        }

        [HttpPost("getUserIdByEmail")]
        public string GetUserIdByEmail(JustMailDTO dto)
        {
            var user = _userService.GetUserByEmail(dto.Email);

            if (user == null)
            {
                throw new Exception("NotFound");
            }

            return user.Id.ToString();
        }

        [HttpPost("{email}/{password}")]
        public User GetUserByEmailAndPassword(string email, string password)
        {
            var user = _userService.GetUserByEmailAndPassword(email,password);

            if (user == null)
            {
                throw new Exception("NotFound");
            }

            return user;
        }

        [HttpPost("login")]
        public async Task<TokenDTO> Login(LoginDTO dto)
        {
            TokenDTO model = new();
            JwtSecurityToken token = new();
            UserDTO userDTO = new UserDTO();

            if (_userService.Login(dto.Email, dto.Password))
            {
                var user = _userService.GetUserByEmailAndPassword(dto.Email, dto.Password);

                userDTO = user;

                token = GenerateAccessToken(userDTO);

            }

            var a = TokenSecurity.JwtToken(userDTO, "access_token", "ShopListAPP");
            model.Token = a[0].ToString();
            model.TokenExpire = a[1].ToString();

            return model;
        }

        [HttpGet("current")]
        public async Task<IActionResult> getLoggedInUserID()
        {
            var accessToken = HttpContext.Request.Cookies["access_token"];

            return Ok(accessToken);
        }

        [NonAction]
        public static string Generate128BitKey()
        {
            // Generate 16 bytes (128 bits) of random data
            byte[] randomBytes = new byte[16];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // Convert the random bytes to a hexadecimal string
            string hexKey = BitConverter.ToString(randomBytes).Replace("-", "").ToLower();

            return hexKey;
        }
 

        [NonAction]
        private JwtSecurityToken GenerateAccessToken(UserDTO userDTO)
        {
            // Replace this with your actual token generation login
            string coded = Generate128BitKey();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(coded));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString()),
            new Claim(ClaimTypes.Name, userDTO.UserName),
            new Claim(ClaimTypes.Name, userDTO.Password),
            new Claim(ClaimTypes.Name, userDTO.Name),
            new Claim(ClaimTypes.Name, userDTO.Surname),
            new Claim(ClaimTypes.Email, userDTO.Email),
            new Claim(ClaimTypes.Gender, userDTO.Gender.ToString()),
            new Claim(ClaimTypes.DateOfBirth, userDTO.BirthDate.ToString()),
            new Claim(ClaimTypes.Name, userDTO.RegisterDate.ToString()),
            new Claim(ClaimTypes.MobilePhone, userDTO.PhoneNumber.ToString()),
            new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                signingCredentials: creds
            );

            return token;
        }

        [HttpPut("updateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileDTO dto)
        {
            if(ModelState.IsValid)
            {
                var userData = _userService.GetElementById(dto.Id);
                if(userData == null)
                {
                    return NotFound();
                }
                userData.Name = dto.Name;
                userData.Surname = dto.Surname; 
                userData.PhoneNumber = dto.PhoneNumber;
                userData.ProfilePhoto = dto.ProfilePhoto;
                userData.UserName = dto.UserName;
                _userService.Update(userData);
                return Ok("User successfuly updated ");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }


        [HttpPut("updatePassword")]
        public async Task<IActionResult> UpdateUserPassword(ResetPasswordDTO dto)
        {

            if (ModelState.IsValid)
            {
                var userToPasswordUpdate = _userService.GetElementById(dto.Id);
                if (userToPasswordUpdate == null)
                {
                    return NotFound();
                }

                userToPasswordUpdate.Password = dto.NewPassword;



                _userService.Update(userToPasswordUpdate);

                return Ok("User password successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterDTO>> Register(RegisterDTO user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _userService.Insert(new User()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                RegisterDate = System.DateTime.Now,
                UserName = user.UserName,
                Password = hashedPassword,
                IsActive = true
            });

            return user;
        }

        [HttpDelete("delete/")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _userService.GetElementById(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = false;
            _userService.Update(user);

            return Ok("User deleted successfully");
        }

        

        //[Authorize]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        
    }
}
