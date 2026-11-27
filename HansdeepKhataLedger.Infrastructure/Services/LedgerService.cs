using HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Application.Interfaces.Repositories;
using HansdeepKhataLedger.Application.Interfaces.Services;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Entities.Common.Reports;
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
        public async Task<PagedResult<LedgerEntry>> GetCreditEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize)
        {
            return await _ledgerRepository.GetCreditEntriesAsync(searchTerm,startDate,endDate,page,pageSize);
        }
        public async Task<decimal> GetTotalCreditAmountAsync(string? searchTerm,DateTime? startDate,DateTime? endDate)
        {
            return await _ledgerRepository.GetTotalCreditAmountAsync(searchTerm, startDate, endDate);
        }
        public async Task<PagedResult<LedgerEntry>> GetPaymentEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize)
        {
            return await _ledgerRepository.GetPaymentEntriesAsync(searchTerm, startDate, endDate, page, pageSize);
        }
        public async Task<decimal> GetTotalPaymentAmountAsync(string? searchTerm,DateTime? startDate,DateTime? endDate)
        {
            return await _ledgerRepository.GetTotalPaymentAmountAsync(searchTerm, startDate, endDate);
        }
        public async Task<List<CustomerOutstandingReport>> GetCustomerOutstandingReportsAsync(string? searchTerm)
        {
            return await _ledgerRepository.GetCustomerOutstandingReportsAsync(searchTerm);
        }
    }
}
