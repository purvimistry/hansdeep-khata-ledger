using HansdeepKhataLedger.Application.Common;
using HansdeepKhataLedger.Domain.Entities;
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
    }
}
