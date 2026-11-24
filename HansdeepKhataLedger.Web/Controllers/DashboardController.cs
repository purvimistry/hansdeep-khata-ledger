using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Web.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HansdeepKhataLedger.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService.GetDashboardDataAsync();
            var model = new DashboardViewModel
            {
                TotalCustomers = data.TotalCustomers,
                TotalPendingAmount = data.TotalPendingAmount,
                TodaysCredit = data.TodaysCredit,
                TodaysCreditEntries = data.TodaysCreditEntries,
                TodaysCollection = data.TodaysCollection,

                RecentLedgerEntries = data.RecentLedgerEntries.Select(x => new RecentTransactionViewModel
                {
                    CustomerName = x.CustomerName,
                    Amount = x.Amount,
                    EntryDate = x.EntryDate,
                    Description = x.Description
                }).ToList(),

                RecentPayments = data.RecentPayments.Select(x => new RecentTransactionViewModel
                {
                    CustomerName = x.CustomerName,
                    Amount = x.Amount,
                    EntryDate = x.EntryDate,
                    Description = x.Description
                }).ToList()
            };

            return View(model);
        }
    }
}
