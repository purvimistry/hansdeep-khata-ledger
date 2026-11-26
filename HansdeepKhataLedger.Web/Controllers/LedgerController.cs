using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Enums;
using HansdeepKhataLedger.Web.Models.Ledger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var entries = await _ledgerService.GetCustomerLedgerAsync(customerId);
            var runningBalance = 0m;
            var entryViewModels = entries.OrderBy(e => e.EntryDate).ThenBy(e => e.Id)
                .Select(entry =>
                {
                    if (entry.EntryType == LedgerEntryType.Borrow)
                    {
                        runningBalance += entry.Amount;
                    }
                    else if (entry.EntryType == LedgerEntryType.Payment)
                    {
                        runningBalance -= entry.Amount;
                    }

                    return new LedgerEntryViewModel
                    {
                        Id = entry.Id,
                        EntryDate = entry.EntryDate,
                        EntryType = entry.EntryType,
                        Amount = entry.Amount,
                        Description = entry.Description,
                        RunningBalance = runningBalance,
                    };
                })
                .ToList();
            var model = new CustomerLedgerViewModel
            {
                CustomerId = customer.Id,
                CustomerName = customer.FullName,
                MobileNumber = customer.MobileNumber ?? string.Empty,
                VillageName = customer.Village.Name ?? string.Empty,
                AreaName = customer.Area?.Name,
                CurrentBalance = runningBalance,
                Entries = entryViewModels
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if(customer == null)
                return NotFound();

            var model = new LedgerEntryCreateViewModel
            {
                CustomerId = customerId,
                EntryDate = DateTime.Now
            };
            ViewBag.CustomerName = customer.FullName;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(LedgerEntryCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                var customer = await _customerService.GetCustomerByIdAsync(model.CustomerId);
                if(customer == null)
                {
                    return NotFound();
                }
                ViewBag.CustomerName = customer.FullName;
                return View(model);
            }

            var ledgerEntry = new LedgerEntry
            {
                CustomerId = model.CustomerId,
                EntryType = model.EntryType,
                Amount = model.Amount,
                EntryDate = model.EntryDate,
                Description = model.Description
            };

            await _ledgerService.AddLedgerEntryAsync(ledgerEntry);
            TempData["Success"] = "Ledger entry added successfully.";
            return RedirectToAction(
                nameof(Index),
                new { customerId = model.CustomerId });
        }
    }
}
