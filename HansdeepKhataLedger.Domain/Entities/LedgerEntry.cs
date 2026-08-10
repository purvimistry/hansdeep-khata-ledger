using HansdeepKhataLedger.Domain.Entities.Common;
using HansdeepKhataLedger.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansdeepKhataLedger.Domain.Entities
{
    public class LedgerEntry : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null;
        public LedgerEntryType EntryType { get; set; }
        public decimal Amount {  get; set; }
        public string? Description {  get; set; }
        public DateTime EntryDate {  get; set; }
    }
}
