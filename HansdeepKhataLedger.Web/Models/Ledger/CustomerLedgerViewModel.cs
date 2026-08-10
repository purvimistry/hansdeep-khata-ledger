namespace HansdeepKhataLedger.Web.Models.Ledger
{
    public class CustomerLedgerViewModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string VillageName { get; set; } = string.Empty;

        public string? AreaName { get; set; }

        public decimal CurrentBalance { get; set; }

        public List<LedgerEntryViewModel> Entries { get; set; } = new();
    }
}
