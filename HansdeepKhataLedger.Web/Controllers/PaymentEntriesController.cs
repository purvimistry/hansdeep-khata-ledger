using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Enums;
using HansdeepKhataLedger.Web.Models.PaymentEntry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HansdeepKhataLedger.Web.Controllers
{
    public class PaymentEntriesController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILedgerService _ledgerService;

        public PaymentEntriesController(ICustomerService customerService,ILedgerService ledgerService)
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

                return View(new PaymentEntryIndexViewModel
                {
                    SearchTerm = searchTerm,
                    StartDate = startDate,
                    EndDate = endDate,
                    CurrentPage = page,
                    PageSize = pageSize
                });
            }

            var result = await _ledgerService.GetPaymentEntriesAsync(searchTerm, startDate, endDate, page, pageSize);

            var totalPaymentAmount = await _ledgerService.GetTotalPaymentAmountAsync(searchTerm, startDate, endDate);

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

            var model = new PaymentEntryIndexViewModel
            {
                PaymentEntries = result.Items,
                SearchTerm = searchTerm,
                StartDate = startDate,
                EndDate = endDate,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords,
                TotalPaymentAmount = totalPaymentAmount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new PaymentEntryViewModel();
            await LoadCustomersAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCustomersAsync(model);
                return View(model);
            }

            var customer = await _customerService.GetCustomerByIdAsync(model.CustomerId);

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
                EntryType = LedgerEntryType.Payment,
                Amount = model.Amount,
                EntryDate = model.EntryDate,
                Description = model.Description
            };

            await _ledgerService.AddLedgerEntryAsync(ledgerEntry);

            TempData["Success"] = "Payment entry added successfully";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCustomersAsync(PaymentEntryViewModel model)
        {
            var result = await _customerService.GetAllCustomersAsync(null, null, null, 1, 1000);

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
