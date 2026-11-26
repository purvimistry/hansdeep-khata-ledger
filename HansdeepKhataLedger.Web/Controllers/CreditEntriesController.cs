using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Enums;
using HansdeepKhataLedger.Web.Models.CreditEntry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HansdeepKhataLedger.Web.Controllers
{
    public class CreditEntriesController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILedgerService _ledgerService;

        public CreditEntriesController(ICustomerService customerService,ILedgerService ledgerService)
        {
            _customerService = customerService;
            _ledgerService = ledgerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm,DateTime? startDate,DateTime? endDate,int page = 1)
        {
            const int pageSize = 10;

            if (page < 1)
                page = 1;

            if (startDate.HasValue && endDate.HasValue &&
                startDate.Value.Date > endDate.Value.Date)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Start date cannot be later than end date");
            }
            else
            {
                var result = await _ledgerService.GetCreditEntriesAsync(searchTerm,startDate,endDate,page,pageSize);
                var totalCreditAmount = await _ledgerService.GetTotalCreditAmountAsync(searchTerm, startDate, endDate);
                if (result.TotalPages > 0 && page > result.TotalPages)
                {
                    return RedirectToAction(nameof(Index), new
                    {
                        searchTerm,
                        startDate,
                        endDate,
                        page = result.TotalPages
                    });
                }
                var model = new CreditEntryIndexViewModel
                {
                    CreditEntries = result.Items,
                    SearchTerm = searchTerm,
                    StartDate = startDate,
                    EndDate = endDate,
                    CurrentPage = result.CurrentPage,
                    PageSize = result.PageSize,
                    TotalRecords = result.TotalRecords,
                    TotalCreditAmount = totalCreditAmount,
                };

                return View(model);
            }
            return View(new CreditEntryIndexViewModel
            {
                SearchTerm = searchTerm,
                StartDate = startDate,
                EndDate = endDate,
                CurrentPage = page,
                PageSize = pageSize
            });
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreditEntryViewModel();

            await LoadCustomersAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreditEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomersAsync(model);
                return View(model);
            }

            var customer = await _customerService
                .GetCustomerByIdAsync(model.CustomerId);

            if (customer == null || !customer.IsActive)
            {
                ModelState.AddModelError(
                    nameof(model.CustomerId),
                    "Please select a valid active customer");

                await LoadCustomersAsync(model);
                return View(model);
            }

            var ledgerEntry = new LedgerEntry
            {
                CustomerId = model.CustomerId,
                EntryType = LedgerEntryType.Borrow,
                Amount = model.Amount,
                EntryDate = model.EntryDate,
                Description = model.Description
            };

            await _ledgerService.AddLedgerEntryAsync(ledgerEntry);

            TempData["Success"] = "Credit entry added successfully";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCustomersAsync(CreditEntryViewModel model)
        {
            var result = await _customerService.GetAllCustomersAsync(
                null, null, null, 1, 1000);

            model.Customers = result.Items
                .Where(c => c.IsActive)
                .Select(c =>
                {
                    var location = string.Join(", ",
               new[] { c.Village?.Name, c.Area?.Name }
                   .Where(x => !string.IsNullOrWhiteSpace(x)));

                    return new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = string.IsNullOrWhiteSpace(location)
                            ? c.FullName
                            : $"{c.FullName} — {location}"
                    };
                })
                .ToList();
        }
    }
}
