using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Infrastructure.Services
{
    public class LedgerService : ILedgerService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILedgerRepository _ledgerRepository;
        public LedgerService(AppDbContext dbContext,ILedgerRepository ledgerRepository)
        {
            _dbContext = dbContext; 
            _ledgerRepository = ledgerRepository;
        }
        public async Task<List<LedgerEntry>> GetCustomerLedgerAsync(int customerId)
        {
            return await _ledgerRepository.GetByCustomerIdAsync(customerId);
        }
        public async Task AddLedgerEntryAsync(LedgerEntry ledgerEntry)
        {
            await _ledgerRepository.AddAsync(ledgerEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
