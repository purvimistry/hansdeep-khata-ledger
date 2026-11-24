using HansdeepKhataLedger.Application.Common.HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILedgerRepository _ledgerRepository;

        public DashboardService(
            ICustomerRepository customerRepository,
            ILedgerRepository ledgerRepository)
        {
            _customerRepository = customerRepository;
            _ledgerRepository = ledgerRepository;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var recentEntries = await _ledgerRepository.GetRecentLedgerEntriesAsync(5);
            var recentPayments = await _ledgerRepository.GetRecentPaymentsAsync(5);

            return new DashboardData
            {
                TotalCustomers = await _customerRepository.GetTotalCustomersAsync(),

                TotalPendingAmount = await _ledgerRepository.GetTotalPendingAmountAsync(),

                TodaysCredit = await _ledgerRepository.GetTodaysCreditAsync(),

                TodaysCreditEntries = await _ledgerRepository.GetTodaysCreditEntriesCountAsync(),

                TodaysCollection = await _ledgerRepository.GetTodaysCollectionAsync(),

                RecentLedgerEntries = recentEntries.Select(l => new RecentTransactionData
                {
                    CustomerName = l.Customer.FullName,
                    Amount = l.Amount,
                    EntryDate = l.EntryDate,
                    Description = l.Description
                }).ToList(),

                RecentPayments = recentPayments.Select(l => new RecentTransactionData
                {
                    CustomerName = l.Customer.FullName,
                    Amount = l.Amount,
                    EntryDate = l.EntryDate,
                    Description = l.Description
                }).ToList()
            };
        }
    }
}

