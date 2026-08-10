using HansdeepKhataLedger.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HansdeepKhataLedger.Web.Controllers
{
    [Authorize]
    public class LedgerController : Controller
    {
        private readonly ILedgerService _ledgerService;
        private readonly ICustomerService _customerService;

        public LedgerController(ILedgerService ledgerService,ICustomerService customerService)
        {
            _ledgerService = ledgerService;
            _customerService = customerService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int customerId)
        {
            return View();
        }
    }
}
