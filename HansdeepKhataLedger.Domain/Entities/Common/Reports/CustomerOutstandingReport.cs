using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Domain.Entities.Common.Reports
{
    public class CustomerOutstandingReport
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string VillageName { get; set; } = string.Empty;
        public string? AreaName { get; set; }
        public string? MobileNumber { get; set; }
        public decimal AdvanceBalance { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal OutstandingAmount { get; set; }
        public DateTime? LastTransactionDate { get; set; }
    }
}
