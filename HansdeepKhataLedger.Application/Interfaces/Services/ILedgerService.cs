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
    }
}
