using HansdeepKhataLedger.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace HansdeepKhataLedger.Web.Models.Ledger
{
    public class LedgerEntryCreateViewModel
    {
        public int CustomerId { get; set; }

        [Required]
        public LedgerEntryType EntryType { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
