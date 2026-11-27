using HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Domain.Entities;
using HansdeepKhataLedger.Domain.Entities.Common.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Interfaces.Repositories
{
    public interface ILedgerRepository
    {
        Task<List<LedgerEntry>> GetByCustomerIdAsync(int customerId);
        Task AddAsync(LedgerEntry ledgerEntry);
        Task<List<CustomerLedgerSummary>> GetCustomerLedgerSummariesAsync(
    List<int> customerIds);
        Task<decimal> GetTotalPendingAmountAsync();
        Task<decimal> GetTodaysCreditAsync();
        Task<int> GetTodaysCreditEntriesCountAsync();
        Task<decimal> GetTodaysCollectionAsync();
        Task<List<LedgerEntry>> GetRecentLedgerEntriesAsync(int count);
        Task<List<LedgerEntry>> GetRecentPaymentsAsync(int count);
        Task<PagedResult<LedgerEntry>> GetCreditEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize);
        Task<decimal> GetTotalCreditAmountAsync(string? searchTerm,DateTime? startDate,DateTime? endDate);
        Task<PagedResult<LedgerEntry>> GetPaymentEntriesAsync(string? searchTerm,DateTime? startDate,DateTime? endDate,int page,int pageSize);
        Task<decimal> GetTotalPaymentAmountAsync(string? searchTerm, DateTime? startDate, DateTime? endDate);
        Task<List<CustomerOutstandingReport>> GetCustomerOutstandingReportsAsync(string? searchTerm);
    }
}
