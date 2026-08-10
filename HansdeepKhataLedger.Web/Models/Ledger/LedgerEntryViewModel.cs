using HansdeepKhataLedger.Domain.Enums;

namespace HansdeepKhataLedger.Web.Models.Ledger
{
    public class LedgerEntryViewModel
    {
        public int Id { get; set; }

        public DateTime EntryDate { get; set; }

        public LedgerEntryType EntryType { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public decimal RunningBalance { get; set; }
    }
}
