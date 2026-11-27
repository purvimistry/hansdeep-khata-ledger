using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Web.Models.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HansdeepKhataLedger.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ILedgerService _ledgerService;

        public ReportsController(ILedgerService ledgerService)
        {
            _ledgerService = ledgerService;
        }

        public async Task<IActionResult> Index(string? searchTerm)
        {
            var customers = await _ledgerService
                .GetCustomerOutstandingReportsAsync(searchTerm);

            var model = new ReportsIndexViewModel
            {
                SearchTerm = searchTerm,

                TotalCustomers = customers.Count,

                CustomersWithDues = customers.Count(c =>
                    c.OutstandingAmount > 0),

                TotalCredit = customers.Sum(c => c.TotalCredit),

                TotalCollection = customers.Sum(c => c.TotalPayment),

                TotalOutstanding = customers
                    .Where(c => c.OutstandingAmount > 0)
                    .Sum(c => c.OutstandingAmount),

                CustomerOutstandingReports = customers
            };

            return View(model);
        }
    }
}
