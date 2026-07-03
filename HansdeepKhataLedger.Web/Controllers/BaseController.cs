using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HansdeepKhataLedger.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int CurrentUserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(userId, out var id) ? id : 0;
            }
        }
        protected string CurrentUserName => User.FindFirstValue(ClaimTypes.Name) ?? "";
        protected string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? "";
    }
}
