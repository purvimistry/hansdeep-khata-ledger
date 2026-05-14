using HansdeepKhataLedger.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HansdeepKhataLedger.Web.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
