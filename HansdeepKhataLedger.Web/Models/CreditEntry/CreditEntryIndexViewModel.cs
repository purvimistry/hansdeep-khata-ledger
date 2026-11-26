using HansdeepKhataLedger.Domain.Entities;

namespace HansdeepKhataLedger.Web.Models.CreditEntry
{
    public class CreditEntryIndexViewModel
    {
        public List<LedgerEntry> CreditEntries { get; set; } = new();

        public string? SearchTerm { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages =>
            PageSize > 0
                ? (int)Math.Ceiling((double)TotalRecords / PageSize)
                : 0;
        public decimal TotalCreditAmount { get; set; }
    }
}
