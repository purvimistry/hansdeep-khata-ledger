using HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Entities.Common.Reports;
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

        public async Task<PagedResult<LedgerEntry>> GetCreditEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize)
        {
            var query = GetCreditEntriesQuery(searchTerm, startDate, endDate);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(l => l.EntryDate)
                .ThenByDescending(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LedgerEntry>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
        public async Task<decimal> GetTotalCreditAmountAsync(string? searchTerm,DateTime? startDate,DateTime? endDate)
        {
            return await GetCreditEntriesQuery(searchTerm, startDate, endDate)
                .SumAsync(l => (decimal?)l.Amount) ?? 0m;
        }
        private IQueryable<LedgerEntry> GetCreditEntriesQuery(string? searchTerm,DateTime? startDate,DateTime? endDate)
        {
            var query = _dbContext.LedgerEntries
                .Include(l => l.Customer)
                    .ThenInclude(c => c.Village)
                .Include(l => l.Customer)
                    .ThenInclude(c => c.Area)
                .Where(l => l.EntryType == LedgerEntryType.Borrow);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                query = query.Where(l =>
                    l.Customer.FullName.Contains(searchTerm) ||
                    l.Customer.Village.Name.Contains(searchTerm) ||
                    (l.Customer.Area != null &&
                     l.Customer.Area.Name.Contains(searchTerm)) ||
                    (l.Customer.MobileNumber != null &&
                     l.Customer.MobileNumber.Contains(searchTerm)));
            }

            if (startDate.HasValue)
                query = query.Where(l => l.EntryDate >= startDate.Value.Date);

            if (endDate.HasValue)
            {
                var exclusiveEndDate = endDate.Value.Date.AddDays(1);
                query = query.Where(l => l.EntryDate < exclusiveEndDate);
            }

            return query;
        }
        public async Task<PagedResult<LedgerEntry>> GetPaymentEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize)
        {
            var query = GetPaymentEntriesQuery(searchTerm, startDate, endDate);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(l => l.EntryDate)
                .ThenByDescending(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LedgerEntry>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
        public async Task<decimal> GetTotalPaymentAmountAsync(string? searchTerm, DateTime? startDate,DateTime? endDate)
        {
            return await GetPaymentEntriesQuery(
                    searchTerm, startDate, endDate)
                .SumAsync(l => (decimal?)l.Amount) ?? 0m;
        }
        private IQueryable<LedgerEntry> GetPaymentEntriesQuery(string? searchTerm,DateTime? startDate,DateTime? endDate)
        {
            var query = _dbContext.LedgerEntries
                .Include(l => l.Customer)
                    .ThenInclude(c => c.Village)
                .Include(l => l.Customer)
                    .ThenInclude(c => c.Area)
                .Where(l => l.EntryType == LedgerEntryType.Payment);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                query = query.Where(l =>
                    l.Customer.FullName.Contains(searchTerm) ||
                    l.Customer.Village.Name.Contains(searchTerm) ||
                    (l.Customer.Area != null &&
                     l.Customer.Area.Name.Contains(searchTerm)) ||
                    (l.Customer.MobileNumber != null &&
                     l.Customer.MobileNumber.Contains(searchTerm)));
            }

            if (startDate.HasValue)
            {
                query = query.Where(l =>
                    l.EntryDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                var exclusiveEndDate = endDate.Value.Date.AddDays(1);

                query = query.Where(l =>
                    l.EntryDate < exclusiveEndDate);
            }

            return query;
        }

        public async Task<List<CustomerOutstandingReport>> GetCustomerOutstandingReportsAsync(string? searchTerm)
        {
            var query = _dbContext.Customers
                .Include(c => c.Village)
                .Include(c => c.Area)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                query = query.Where(c =>
                    c.FullName.Contains(searchTerm) ||
                    c.Village.Name.Contains(searchTerm) ||
                    (c.Area != null && c.Area.Name.Contains(searchTerm)) ||
                    (c.MobileNumber != null &&
                     c.MobileNumber.Contains(searchTerm)));
            }

            return await query
                .Select(c => new CustomerOutstandingReport
                {
                    CustomerId = c.Id,
                    CustomerName = c.FullName,
                    VillageName = c.Village.Name,
                    AreaName = c.Area != null ? c.Area.Name : null,
                    MobileNumber = c.MobileNumber,

                    AdvanceBalance = c.AdvanceBalance,

                    TotalCredit = c.LedgerEntries
                        .Where(l => l.EntryType == LedgerEntryType.Borrow)
                        .Sum(l => (decimal?)l.Amount) ?? 0m,

                    TotalPayment = c.LedgerEntries
                        .Where(l => l.EntryType == LedgerEntryType.Payment)
                        .Sum(l => (decimal?)l.Amount) ?? 0m,

                    OutstandingAmount = c.AdvanceBalance +
                        c.LedgerEntries.Sum(l =>
                            l.EntryType == LedgerEntryType.Borrow
                                ? l.Amount
                                : -l.Amount),

                    LastTransactionDate = c.LedgerEntries
                        .Max(l => (DateTime?)l.EntryDate)
                })
                .OrderByDescending(c => c.OutstandingAmount)
                .ToListAsync();
        }

    }
}
