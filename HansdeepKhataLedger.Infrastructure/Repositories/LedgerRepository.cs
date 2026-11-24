using HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Enums;
using HansdeepKhataLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Repositories
{
    public class LedgerRepository : ILedgerRepository
    {
        private readonly AppDbContext _dbContext;
        public LedgerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public async Task<List<LedgerEntry>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbContext.LedgerEntries
                .Where(l => l.CustomerId == customerId)
                .OrderBy(l => l.EntryDate)
                .ToListAsync();
        }
        public async Task AddAsync(LedgerEntry ledgerEntry)
        {
            await _dbContext.LedgerEntries.AddAsync(ledgerEntry);
        }
        public async Task<List<CustomerLedgerSummary>> GetCustomerLedgerSummariesAsync(
    List<int> customerIds)
        {
            return await _dbContext.LedgerEntries
                .Where(l => customerIds.Contains(l.CustomerId))
                .GroupBy(l => l.CustomerId)
                .Select(g => new CustomerLedgerSummary
                {
                    CustomerId = g.Key,

                    PendingBalance = g.Sum(l =>
                        l.EntryType == LedgerEntryType.Borrow
                            ? l.Amount
                            : -l.Amount),

                    LastTransactionDate = g.Max(l => l.EntryDate)
                })
                .ToListAsync();
        }
        public async Task<decimal> GetTotalPendingAmountAsync()
        {
            var customerBalances = await _dbContext.Customers
                .Select(c => new
                {
                    Balance = c.AdvanceBalance +
                        c.LedgerEntries.Sum(l =>
                            l.EntryType == LedgerEntryType.Borrow
                                ? l.Amount
                                : -l.Amount)
                })
                .ToListAsync();

            return customerBalances
                .Where(c => c.Balance > 0)
                .Sum(c => c.Balance);
        }

        public async Task<decimal> GetTodaysCreditAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _dbContext.LedgerEntries
                .Where(l => l.EntryType == LedgerEntryType.Borrow &&
                            l.EntryDate >= today &&
                            l.EntryDate < tomorrow)
                .SumAsync(l => (decimal?)l.Amount) ?? 0;
        }

        public async Task<int> GetTodaysCreditEntriesCountAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _dbContext.LedgerEntries
                .CountAsync(l => l.EntryType == LedgerEntryType.Borrow &&
                                 l.EntryDate >= today &&
                                 l.EntryDate < tomorrow);
        }

        public async Task<decimal> GetTodaysCollectionAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            return await _dbContext.LedgerEntries
                .Where(l => l.EntryType == LedgerEntryType.Payment &&
                            l.EntryDate >= today &&
                            l.EntryDate < tomorrow)
                .SumAsync(l => (decimal?)l.Amount) ?? 0;
        }

        public async Task<List<LedgerEntry>> GetRecentLedgerEntriesAsync(int count)
        {
            return await _dbContext.LedgerEntries
                .Include(l => l.Customer)
                .Where(l => l.EntryType == LedgerEntryType.Borrow)
                .OrderByDescending(l => l.EntryDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<LedgerEntry>> GetRecentPaymentsAsync(int count)
        {
            return await _dbContext.LedgerEntries
                .Include(l => l.Customer)
                .Where(l => l.EntryType == LedgerEntryType.Payment)
                .OrderByDescending(l => l.EntryDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
