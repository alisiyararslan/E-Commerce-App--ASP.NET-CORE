using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using EntityLayer.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using EntityLayer.Concrete;
using System.Text.RegularExpressions;
using NToastNotify;
using Humanizer;
using Microsoft.CodeAnalysis.Scripting;

namespace ShopListAppNKatmanli.Controllers
{
    public class SessionController : Controller
    {

        private readonly IConfiguration _configuration;

        private readonly IToastNotification _toast;
        public SessionController(IConfiguration configuration, IToastNotification toast)
        {
            _configuration = configuration;
            _toast = toast;
        }

        [HttpGet]
        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {

            string pattern = "^[A-Za-z0-9]+$";  //Pattern for allowing username only english characters

            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];

            var responseUser = await httpClient.GetAsync(apiUrl+"Users/");
            var contentUser = await responseUser.Content.ReadAsStringAsync();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(contentUser);

            if (!Regex.IsMatch(registerDto.UserName, pattern))
            {
                _toast.AddErrorToastMessage("Username should contain only english characters and numbers.", new ToastrOptions { Title = "Error" });
                return View();
            }

            if (!users.Any(u => u.Email == registerDto.Email)  )
            {
                if(!users.Any(u => u.UserName== registerDto.UserName))
                {
                        if(!(registerDto.BirthDate.AddYears(10) < DateTime.Now))
                        {
                            _toast.AddErrorToastMessage("Your birthdate must exceed 10 years.", new ToastrOptions { Title = "Error." });
                            return View();
                        }

                        if ((registerDto.Password.Length <= 16) && (registerDto.Password.Length >= 8)
                          && (registerDto.PhoneNumber.ToString().Length == 10)
                         && Regex.IsMatch((registerDto.UserName), pattern))
                        {
                            var response = await httpClient.PostAsJsonAsync(apiUrl + "Mail/verificationMail", registerDto);

                            var message = response.Content.ReadAsStringAsync();

                            var verifyDto = new VerifyDTO
                            {
                                Register = registerDto as RegisterDTO, // RegisterDTO nesnesini doğrudan VerifyDTO içine aktarabilirsiniz
                                verifyCode = message.Result.ToString().Trim() // response.Content.ToString() ile elde ettiğiniz değeri "Code" özelliğine atayın
                            };
                            string serializedModel = JsonConvert.SerializeObject(verifyDto);
                            TempData["MyUser"] = serializedModel;
                            return RedirectToAction("VerifyAccount", "Session");
                        }
                        else
                        {
                            _toast.AddErrorToastMessage("An unexpected error has been encountered. Please try again later.", new ToastrOptions { Title = "Error." });
                            return View();
                        }
                }
                else
                {
                    _toast.AddErrorToastMessage("There is a user in this username. Please try another username...", new ToastrOptions { Title = "Error." });
                    return View();
                }
                
            }
            else
            {
                _toast.AddErrorToastMessage("There is a user in this email. Please try another email address...", new ToastrOptions { Title = "Error." });
                return View();
                
            }

            return View();

        }

        [HttpGet]
        public async Task<IActionResult> VerifyAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAccount(VerifyDTO dto)
        {
            var model = JsonConvert.DeserializeObject<VerifyDTO>(TempData["MyUser"] as string);

            if (model.verifyCode == dto.UserCode)
            {
                var httpClient = new HttpClient();
                string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                var response = await httpClient.PostAsJsonAsync(apiUrl + "Users/register", model.Register);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Session");
                }
                else
                {
                    return RedirectToAction("Index", "Cart");
                }

            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }


        }



        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            var response = await httpClient.PostAsJsonAsync(apiUrl + "Users/login", dto);
            TokenDTO model = new();

            var response2 = await httpClient.GetAsync(apiUrl + "Users");// valid user check
            var content2 = await response2.Content.ReadAsStringAsync();
            List<UserDTO> userList = JsonConvert.DeserializeObject<List<UserDTO>>(content2);

            var isCredentialsValid = false;
            UserDTO validUser = new();

            foreach (var user in userList)
            {
                try
                {
                    if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password) && user.Email == dto.Email)
                    {
                        isCredentialsValid = true;
                        validUser = user;
                    }
                }
                catch(Exception ex)
                {
                    _toast.AddErrorToastMessage("An unexpected error is encountered. Please try again later.", new ToastrOptions { Title = "Error." });
                    return View();
                }
            }

            if (!isCredentialsValid)
            {
                _toast.AddErrorToastMessage("Email or password are incorrect, please try again....", new ToastrOptions { Title = "Error." });
                return View();
            }

            if(!validUser.isActive)
            {
                _toast.AddErrorToastMessage("This user is not active!", new ToastrOptions { Title = "Error." });
                return View();
            }

            if (response != null)
            {
                var content = await response.Content.ReadAsStringAsync();

                model = JsonConvert.DeserializeObject<TokenDTO>(content);

                var claims = new List<Claim>
                {
                    new Claim("access_token", model.Token, ClaimValueTypes.String),
                };

                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var userPrincipal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = false
                });
                var user = HttpContext.User;
                var mailDTO = new JustMailDTO { Email = dto.Email };
                var idResponse = await httpClient.PostAsJsonAsync(apiUrl + "Users/getUserIdByEmail/", mailDTO);
                var id = idResponse.Content.ReadAsStringAsync().Result;

                HttpContext.User.AddIdentity(userIdentity);

                HttpContext.Response.Cookies.Append("UserID", id);
                _toast.AddSuccessToastMessage("Successfully logged in.", new ToastrOptions { Title = "Successful." });
                return RedirectToAction("Index", "Home");
            }



            return View();

        }


        [Authorize]
        public async Task<ActionResult> LogOut()
        {


            //var httpClient = new HttpClient();
            //string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
            //var response = await httpClient.GetAsync(apiUrl + "Users/logout");

            await HttpContext.SignOutAsync();

            HttpContext.Response.Cookies.Delete("UserID");
            _toast.AddInfoToastMessage("Signed Out.", new ToastrOptions { Title = "Sign Out" });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetUpdatePassword(ResetPasswordDTO dto)
        {
            var model = JsonConvert.DeserializeObject<VerifyDTO>(TempData["MyUserCode"] as string);
            if (model.verifyCode == dto.verifyCode) {
                dto.Id =int.Parse(model.UserCode.ToString());
                if (dto.NewPassword == dto.PasswordAgain)
                {
                    var httpClient = new HttpClient();
                    string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
                    var response = await httpClient.PutAsJsonAsync(apiUrl + "Users/updatePassword", dto);

                    _toast.AddSuccessToastMessage("Password successfuly changed.", new ToastrOptions { Title = "Successful" });
                    return RedirectToAction("Login", "Session");
                }
                else
                {
                    _toast.AddErrorToastMessage("The passwords do not match.", new ToastrOptions { Title = "Error" });
                    return RedirectToAction("ResetPassword","Session");
                }
            }
            else
            {
                _toast.AddErrorToastMessage("The entered code is invalid.", new ToastrOptions { Title = "Error" });
                return RedirectToAction("ResetPassword", "Session");
                //verilen kodla kullanıcının girdiği kod eşleşmedi
            }

        }

        [HttpGet]
        public IActionResult SendResetMail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetMail(SendResetMailDTO dto)
        {

            var httpClient = new HttpClient();
            string apiUrl = _configuration["MyConfigurations:MyApiUrl"];
           var justmailDto=new JustMailDTO
           {
               Email = dto.Email,
           };
            var responseUserId = await httpClient.PostAsJsonAsync(apiUrl + "Users/getUserIdByEmail/", justmailDto);
            var userIdDto=responseUserId.Content.ReadAsStringAsync();
            
            var response = await httpClient.PostAsJsonAsync(apiUrl + "Mail/sendPasswordResetMail", dto);
            var message = response.Content.ReadAsStringAsync();
            
            var verifyDto = new VerifyDTO
            {
                UserCode=userIdDto.Result.ToString().Trim(),
                verifyCode = message.Result.ToString().Trim()// response.Content.ToString() ile elde ettiğiniz değeri "Code" özelliğine atayın
            };

           
            string serializedModel = JsonConvert.SerializeObject(verifyDto);
            
            TempData["MyUserCode"] = serializedModel;
            return RedirectToAction("Login", "Session");
        }

       

    }
}
