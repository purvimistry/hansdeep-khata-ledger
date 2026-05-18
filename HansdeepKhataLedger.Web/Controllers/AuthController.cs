using HansdeepKhataLedger.Application.Interfaces;
using HansdeepKhataLedger.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HansdeepKhataLedger.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService; 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var isValid = await _authService.ValidateAdmin(model.Username, model.Password);
            if(!isValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model); 
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
