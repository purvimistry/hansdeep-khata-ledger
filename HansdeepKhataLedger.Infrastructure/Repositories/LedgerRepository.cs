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
    }
}
