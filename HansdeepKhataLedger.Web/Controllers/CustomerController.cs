using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Web.Models.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Web.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
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
        public async Task<IActionResult> Create()
        {
            await LoadDropdownsAsync();
            return View(new CreateCustomerViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel model)
        {
            if(!ModelState.IsValid)
            {
                await LoadDropdownsAsync(model.VillageId);
                return View(model);
            }

            var customer = new Customer
            {
                FullName = model.FullName,
                MobileNumber = model.MobileNumber.Trim(),
                AlternateMobileNumber = model.AlternateMobileNumber?.Trim(),
                VillageId = model.VillageId,
                AreaId = model.AreaId,
                Notes = model.Notes,
                AdvanceBalance = model.AdvanceBalance ?? 0,
                IsActive = true
            };
            await _customerService.AddCustomerAsync(customer, CurrentUserId);
            TempData["Success"] = "Customer added successfully.";

            return RedirectToAction(nameof(Index));

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

        [HttpGet]
        public async Task<IActionResult> GetAreas(int villageId)
        {
            var areas =await _customerService.GetAreasByVillageAsync(villageId);
            return Json(areas.Select(a => new
            {
                id = a.Id,
                text = a.Name
            }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillage(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Village name is required"
                    });
                }

                var village = await _customerService.AddVillageAsync(name);
                return Json(new
                {
                    success = true,
                    id = village.Id,
                    text = village.Name
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public async Task<IActionResult> CreateArea(string name, int villageId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Area name is required"
                    });
                }


                var area = await _customerService.AddAreaAsync(name, villageId);
                return Json(new
                {
                    success = true,
                    id = area.Id,
                    text = area.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        private async Task LoadDropdownsAsync(int? villageId = null)
        {
            ViewBag.Villages = new SelectList(
                await _customerService.GetVillagesAsync(),
                "Id",
                "Name",
                villageId);

            if (villageId.HasValue)
            {
                ViewBag.Areas = new SelectList(
                    await _customerService.GetAreasByVillageAsync(villageId.Value),
                    "Id",
                    "Name");
            }
            else
            {
                ViewBag.Areas = new SelectList(
                    Enumerable.Empty<SelectListItem>());
            }
        }
    }
}
