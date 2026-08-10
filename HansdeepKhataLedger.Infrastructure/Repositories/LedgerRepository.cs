using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Domain.Entities;
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
    }
}
