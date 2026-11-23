using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Application.Common
{
    public class CustomerLedgerSummary
    {
        public int CustomerId { get; set; }
        public decimal PendingBalance { get; set; }
        public DateTime? LastTransactionDate { get; set; }
    }
}
