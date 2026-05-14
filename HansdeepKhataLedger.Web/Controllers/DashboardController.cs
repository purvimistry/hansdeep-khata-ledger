using Microsoft.AspNetCore.Mvc;

namespace HansdeepKhataLedger.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
