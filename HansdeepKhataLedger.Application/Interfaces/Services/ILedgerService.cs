using HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Interfaces.Services
{
    public interface ILedgerService
    {
        Task<List<LedgerEntry>> GetCustomerLedgerAsync(int customerId);
        Task AddLedgerEntryAsync(LedgerEntry ledgerEntry);
        Task<PagedResult<LedgerEntry>> GetCreditEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize);
        Task<decimal> GetTotalCreditAmountAsync(string? searchTerm,DateTime? startDate,DateTime? endDate);
        Task<PagedResult<LedgerEntry>> GetPaymentEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize);
        Task<decimal> GetTotalPaymentAmountAsync(string? searchTerm,DateTime? startDate,DateTime? endDate);
    }
}
