using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Common
{
    namespace HansdeepKhataLedger.Application.Common
    {
        public class DashboardData
        {
            public int TotalCustomers { get; set; }
            public decimal TotalPendingAmount { get; set; }
            public decimal TodaysCredit { get; set; }
            public int TodaysCreditEntries { get; set; }
            public decimal TodaysCollection { get; set; }

            public List<RecentTransactionData> RecentLedgerEntries { get; set; } = new();
            public List<RecentTransactionData> RecentPayments { get; set; } = new();
        }

        public class RecentTransactionData
        {
            public string CustomerName { get; set; } = string.Empty;
            public decimal Amount { get; set; }
            public DateTime EntryDate { get; set; }
            public string? Description { get; set; }
        }
    }
}
