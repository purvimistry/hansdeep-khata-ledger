using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Web.Models.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IVillageRepository _villageRepository;
        private readonly IAreaRepository _areaRepository;

        public CustomerController(ICustomerService customerService, IVillageRepository villageRepository, IAreaRepository areaRepository)
        {
            _customerService = customerService;
            _villageRepository = villageRepository;
            _areaRepository = areaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var model = new CustomerIndexViewModel
            {
                Customers = customers.Select(customer => new CustomerListItemViewModel
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    MobileNumber = customer.MobileNumber ?? string.Empty,
                    VillageName = customer.Village?.Name ?? string.Empty,
                    AreaName = customer.Area?.Name,
                    IsActive = customer.IsActive,
                    PendingBalance = 0,
                    LastTransactionDate = null
                }).ToList(),
                CurrentPage = 1,
                TotalPages = 1,
                TotalRecords = customers.Count(),
                SortBy = "Name",
                SortDescending = false
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel model)
        {
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
