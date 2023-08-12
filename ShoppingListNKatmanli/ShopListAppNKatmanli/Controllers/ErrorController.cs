using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ShopListAppNKatmanli.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IToastNotification _toast;
        public ErrorController(IToastNotification toast)
        {
            _toast = toast;
        }
        public IActionResult Error404()
        {
            return View();
        }
        public IActionResult Error500()
        {
            return View();
        }

        public IActionResult ErrorAPI()
        {
            return View();
        }

        public IActionResult ErrorAuth()
        {
            return View();
        }

        public IActionResult MainCartError()
        {
            _toast.AddErrorToastMessage("You do not have a main basket.", new ToastrOptions { Title = "Error." });
            return RedirectToAction("Index", "Home");
        }
    }
}
